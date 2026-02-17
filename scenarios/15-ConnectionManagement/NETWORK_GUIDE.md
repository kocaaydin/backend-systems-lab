# Backend Icin Network Konu Anlatimi

Bu dokuman, backend gelistirme perspektifiyle ag temellerini anlamak icin konu anlatimi formatinda hazirlandi.
Hedef: test sonuclarini sadece rakam olarak degil, ag davranisi olarak da yorumlayabilmek.

## 1. Neden Network Bilgisi Gerekli?

Backend performans sorunlari sadece koddan cikmaz.
Ayni endpoint ve ayni business logic ile bile su farklar maliyeti degistirir:

- baglanti yeniden kuruluyor mu?
- baglanti ne kadar sik kapaniyor?
- paket kaybi veya retransmission var mi?
- TLS veya load balancer ek gecikme getiriyor mu?

Bu nedenle, k6 sonucu (P50/P95/P99/RPS) ile TCP davranisini birlikte okumak gerekir.

## 2. TCP 3-Way Handshake (SYN, SYN-ACK, ACK)

TCP baglantisi kurulumunda 3 adim vardir:

1. Istemci `SYN` gonderir (baglanti baslatma istegi)
2. Sunucu `SYN-ACK` ile yanit verir
3. Istemci `ACK` gonderir ve baglanti kurulur

Bu adimlarin maliyeti vardir. Her yeni baglanti acilisinda en az bir RTT etkisi olusur.

Pratik sonucu:

- Baglantiyi sik kapatan model: daha fazla handshake maliyeti uretir.
- Baglantiyi tekrar kullanan model: handshake maliyetini azaltir.

## 3. TCP Kapanis: FIN, ACK, RST, TIME_WAIT

### 3.1 Normal kapanis (FIN/ACK)

Baglanti zarif sekilde kapanirken FIN ve ACK paketleri gorulur.
Bu beklenen durumdur.

### 3.2 Sert kapanis (RST)

`RST` ani kapanisi ifade eder.
Sebep: timeout, hata, uygulama tarafli reset, protocol mismatch vb.

Detay:

1. Timeout kaynakli RST:
   - Bir taraf beklenen surede veri/yanit alamazsa baglantiyi sert kapatabilir.
2. Uygulama kaynakli RST:
   - Uygulama socket'i zorla kapatir (ornek: process crash, cancel, force close).
3. Protocol mismatch:
   - Istemci ve sunucu ayni protokolu beklemiyordur.
   - Ornekler:
     - HTTP istegini HTTPS/TLS bekleyen porta gondermek
     - TLS handshake bekleyen tarafa plain HTTP gondermek
     - HTTP/2 beklenen hatta HTTP/1.1 veya tersini zorlamak
4. Gecersiz/istenmeyen trafik:
   - Sunucu, guvenlik veya dogrulama kurallari nedeniyle baglantiyi reddedebilir.

Yorum:

- Tekil RST normal olabilir.
- Surekli veya burst halinde RST goruluyorsa timeout, konfig uyumsuzlugu veya protocol mismatch arastirilmalidir.

### 3.3 TIME_WAIT

Baglanti kapandiktan sonra OS bir sure soketi `TIME_WAIT` durumunda tutar.
Amac: gec paketlerin yeni baglantilarla karismasini onlemek.

Pratik sonucu:

- Daha fazla ac-kapa davranisi -> daha fazla TIME_WAIT birikimi.
- Daha fazla baglanti yeniden kullanim -> daha dusuk TIME_WAIT.

## 4. HTTP/1.1 Keep-Alive Mantigi

Keep-Alive, ayni TCP baglantisi uzerinden birden fazla HTTP istegi gondermeyi saglar.

- Keep-Alive acik:
  - daha az baglanti kurulumu
  - daha az kapanis
  - daha dusuk gecikme potansiyeli

- Keep-Alive kapali (`Connection: close`):
  - her istekte baglanti yasam dongusu tekrarina daha yakin davranis
  - handshake ve teardown maliyeti artar

Kavramsal olarak ana fikir sudur: request ayni kalsa bile baglanti yonetimi degisince maliyet degisir.

## 5. Latency Nerede Olusur?

Bir request su asamalarda zaman kaybeder:

1. TCP connect (SYN/SYN-ACK/ACK)
2. (Varsa) TLS handshake
3. Sunucuya ulasim + kuyruklama
4. Uygulama islemi
5. Response transfer

Keep-alive acik/kapali senaryolarinda en buyuk fark genelde 1. adimda (connect maliyeti) ve kapanis yan etkilerinde gorulur.

## 6. Genelde Nasil Uygulanir?

Olcum tasarimi icin genel prensip:

1. Isinma (warm-up) trafigini olcum trafiginden ayir.
2. Ayni endpoint ve benzer yuk kosullariyla sadece baglanti davranisini degistir.
3. Metrikleri ayni pencerede topla (`p50/p95/p99`, `RPS`, hata orani, socket state).
4. Packet analizi yapacaksan test fazlarini (warm-up vs measure) ayri etiketle veya ayri capture penceresi kullan.

Bu sayede karsilastirma daha adil olur ve yorum hatalari azalir.

## 7. Wireshark ile Konu Bazli Inceleme

### 7.1 Handshake baskisi

- Senaryo A: `tcp.flags.syn == 1 and tcp.flags.ack == 0 and tcp.port == <PORT_A>`
- Senaryo B: `tcp.flags.syn == 1 and tcp.flags.ack == 0 and tcp.port == <PORT_B>`

Yorum: Baglanti acilis baskisi yuksek olan senaryoda SYN yogunlugu artar.

### 7.2 Kapanis baskisi

- Senaryo A: `tcp.flags.fin == 1 and tcp.port == <PORT_A>`
- Senaryo B: `tcp.flags.fin == 1 and tcp.port == <PORT_B>`

Yorum: Baglanti yasam dongusu daha sik tekrarlanan senaryoda FIN trafigi artar.

### 7.3 Anomali kontrolu

- Reset: `tcp.flags.reset == 1 and (tcp.port == <PORT_A> or tcp.port == <PORT_B>)`
- Retransmission: `tcp.analysis.retransmission and (tcp.port == <PORT_A> or tcp.port == <PORT_B>)`

Yorum: Bu alanlar yuksekse yalnizca keep-alive degil, ag kalitesi de sonucu etkiliyor olabilir.

### 7.4 Retransmission Nedir? (Temel ve Kritik Kavram)

`Retransmission`, gonderilen bir TCP segmentinin zamaninda onay (ACK) alamamasi nedeniyle yeniden gonderilmesidir.

Kisa tanim:

- Paket gitti ama ACK gelmedi -> TCP ayni paketi tekrar yollar.
- Bu durum guvenilirlik icin gereklidir, ama maliyetlidir.

Neden olur?

1. Gercek paket kaybi (network congestion, interface sorunu, buffer drop)
2. Geciken ACK (asiri kuyruk, CPU baskisi, receiver tarafinda gecikme)
3. Out-of-order veya gecici yol dalgalanmalari

Neye mal olur?

1. Gecikme artar (ozellikle p95/p99)
2. Effective throughput duser
3. Uygulama timeout/retry zinciri tetiklenebilir

Wireshark'ta nasil gorulur?

- Genel: `tcp.analysis.retransmission`
- Senaryo A: `tcp.port == <PORT_A> and tcp.analysis.retransmission`
- Senaryo B: `tcp.port == <PORT_B> and tcp.analysis.retransmission`

Dogru yorum prensibi:

- Retransmission sayisi tek basina karar verdirmez.
- Ayni anda `RST`, `timeout`, `p99` ve `RPS` ile birlikte degerlendirilmelidir.

Onemli ayrim:

- `Retransmission` TCP katmaninda paketin tekrar gonderimi.
- `Retry` uygulama/istemci katmaninda istegin tekrar atilmasi.
- Ikisi ayni sey degildir ama birbirini tetikleyebilir.

## 8. .NET Tarafiyla Birlikte Okuma

Sadece packet bakmak yetmez. k6 ve .NET metrikleri ile birlikte yorumlanmali:

- k6: P50, P95, P99, RPS
- Socket etkisi: TIME_WAIT
- .NET runtime: CPU, GC, thread pool, request queue davranisi

Yorum prensibi:

- Wireshark "neden"i,
- k6/.NET "etki"yi gosterir.

## 9. Sonuc Cikarim Kalibi

Rapor yazarken su kalibi kullan:

1. Gozlem: Senaryo A'da SYN ve FIN trafigi daha yogun.
2. Etki: P95/P99 daha yuksek, RPS daha dusuk.
3. Sistem izi: TIME_WAIT artisi.
4. Sonuc: baglanti yasam dongusu maliyeti performansi asagi cekiyor.

## 10. Ogrenme Hedefi Kontrol Listesi

Bu dokumani bitirdiginde sunlari net anlatabiliyor olmalisin:

1. Keep-Alive neden fayda saglar?
2. TIME_WAIT neden artar, ne zaman sorun olur?
3. SYN/FIN/RST paketleri hangi uygulama davranisina karsilik gelir?
4. Wireshark bulgusu ile k6/.NET sonucunu nasil birlestirirsin?

## 11. Temel Kavram Sozlugu (Hizli Basvuru)

Bu bolumde dokuman icinde gecen ama karisan terimler kisa ve net anlatilir.

### 11.1 RTT

- `RTT` (Round Trip Time), bir paketin gidip cevabinin donme suresidir.
- Handshake maliyeti ve request latency uzerinde dogrudan etkilidir.

### 11.2 p50 p95 p99

- `p50`: ortanca deneyim (tipik)
- `p95`: yavas taraftaki yuzde 5'in baslangici
- `p99`: en yavas yuzde 1 (tail latency)
- Incident riski genelde p95/p99'da gorunur.

### 11.3 RPS

- `RPS` (Requests Per Second), sistemin birim zamanda isledigi istek sayisi.
- Yalniz basina okunmaz; latency ve error ile birlikte yorumlanir.

### 11.4 Socket ve tcp.stream Farki

- `Socket`: OS tarafindaki baglanti nesnesi/state'i (`ESTABLISHED`, `TIME_WAIT`).
- `tcp.stream`: Wireshark'in "ayni TCP konusmasi" icin verdigi kimlik.
- Iliskili ama birebir ayni metrik degildir.

### 11.5 Connection Churn

- Cok sik baglanti acma-kapama davranisidir.
- Sonuc: SYN/FIN artisi, TIME_WAIT birikimi, CPU/kernel overhead.

### 11.6 Timeout

- Belirli surede cevap gelmezse islemin iptal edilmesi.
- Kisa timeout + agresif retry, zincirleme hata yaratabilir.

### 11.7 Retry ve Retransmission Farki

- `Retransmission`: TCP seviyesinde paket tekrar gonderimi.
- `Retry`: uygulama/istemci seviyesinde istegin yeniden atilmasi.
- Birbirini tetikleyebilir, ama farkli katman davranisidir.