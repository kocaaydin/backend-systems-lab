# SRE Platform Advanced Track

SRE/Platform seviyesinde ag problemlerini analiz ederken amac, semptomu gormekle yetinmeyip kok nedeni dogru katmanda bulmaktir.
Asil odak, komut ezberi degil; problem aninda packet, metric ve log sinyallerini birlikte okuyup dogru teshis koymaktir.

## 1. SRE Bakis Acisi: Semptom ve Kok Neden

Uretimde ilk gorulen sey genelde semptomdur: p99 artis, timeout, error spike, RPS dususu.
Kok neden ise farkli bir katmanda sakli olabilir: kernel queue doygunlugu, LB timeout uyumsuzlugu, TLS handshake maliyeti veya ag kaybi.

Bu nedenle SRE analizinde temel kural sudur:

1. Packet ne diyor? (SYN/FIN/RST/retransmission)
2. Metric ne diyor? (latency, throughput, saturation)
3. Log ne diyor? (timeout, reset, upstream disconnect)

Bu uc sinyal ayni yone bakiyorsa teshis guclenir.

## 2. Linux Networking Konulari

### 2.1 Ephemeral Port Nedir?

Ephemeral port, istemcinin outbound baglantilar icin kullandigi gecici port havuzudur.
Sistem cok hizli baglanti ac-kapa yapiyorsa bu havuz baski altina girer.
Bunun sonucu connect gecikmesi, timeout ve bazi durumlarda baglanti acamama olabilir.

Pratik yorum:
- Kisa omurlu connection churn arttikca port baskisi artar.
- Keep-alive ve connection reuse bu baskiyi azaltir.

### 2.2 Conntrack Nedir?

Conntrack, Linux netfilter katmaninin baglanti takip tablosudur.
NAT/firewall olan ortamlarda her akis burada izlenir.
Tablo kapasiteye yaklasirsa paket dusumu, anlik timeout ve tutarsiz baglanti hatalari gorulebilir.

Pratik yorum:
- Uygulama saglikli olsa bile altyapi katmani sorun cikarabilir.
- Bu nedenle app metrikleri iyi gorunurken ag semptomlari kotulesebilir.

### 2.3 Backlog ve somaxconn Nedir?

Backlog, sunucunun kabul etmeden once kuyrukta bekletebildigi baglanti istegi miktaridir.
`somaxconn` ise kernel tarafindaki ust siniri temsil eder.
SYN artisi aninda backlog dolarsa, istemci tarafinda gecikme veya baglanti hatasi gorulebilir.

Pratik yorum:
- Ani trafik artisinda sadece CPU degil, accept hattinin kapasitesi de belirleyicidir.

## 3. L4 L7 Load Balancer ve Proxy Davranisi

L4 dengeleyici TCP/UDP seviyesinde calisir, daha hafiftir ama protokol icerigini bilmez.
L7 dengeleyici HTTP seviyesinde calisir, daha akilli routing yapar ama parsing ve policy maliyeti getirir.

Buradaki kritik nokta yalnizca "hangisi hizli" degildir.
Asil soru: trafik modelin, guvenlik ihtiyacin ve gozlemlenebilirlik ihtiyacin hangi katmani gerektiriyor?

### 3.1 Idle Timeout Uyumsuzlugu

LB tarafindaki idle timeout ile uygulama tarafindaki keep-alive timeout uyumsuzsa beklenmedik RST dalgalari gorulebilir.
Bu durumda p95/p99 bozulur, istemciler yeniden baglanmaya zorlanir.

Pratik yorum:
- Timeout degerleri tekil degil, zincir halinde dusunulmelidir (client -> LB -> app).

## 4. TLS Operasyonu

TLS yalnizca guvenlik konusu degildir; performans konusu da dogrudan TLS'ten etkilenir.

### 4.1 Sertifika Rotasyonu

Sertifika yenileme sureci kesintisiz olmali, rotate sonrasi handshake hata orani artmamali.
Aksi durumda problem uygulama degil, trust/cert zinciri olabilir.

### 4.2 Cipher ve TLS Versiyon Politikasi

Guvenlik sertlestirmesi yaparken istemci uyumlulugu korunmalidir.
Desteklenmeyen cipher/versiyon politikasi, baglanti basarisizliklari olarak geri doner.

### 4.3 Session Resumption

Session resumption, her baglantida tam TLS handshake yapma zorunlulugunu azaltir.
Bu da handshake gecikmesini ve CPU maliyetini dusurur.

## 5. Observability: RED ve USE

RED servis davranisini anlatir:
- Rate: ne kadar is geliyor?
- Errors: ne kadar hata var?
- Duration: ne kadar gecikme var?

USE kaynak davranisini anlatir:
- Utilization: kaynak ne kadar dolu?
- Saturation: kuyruk/bekleme var mi?
- Errors: kaynak katmaninda hata var mi?

SRE acisindan dogru okuma sekli:
- RED bozulduysa kullanici etkisi var.
- USE bozulduysa kapasite/sistem sorunu var.
- Ikisi birlikte bozuluyorsa incidentin kok nedeni daha hizli bulunur.

### 5.1 eBPF Nerede Devreye Girer?

eBPF, kernel seviyesinde dusuk overhead ile derin gozlem saglar.
Ozellikle "uygulama mi yavas, kernel mi bekletiyor" sorusunu ayirmada degerlidir.

## 6. Failure Testing Neden Gerekli?

Uretimde problem oldugunda ilk kez goruyorsan gec kalmissindir.
Failure testing, hatayi kontrollu ortamda onceden gorup sistemin tepkisini ogrenme yontemidir.

### 6.1 Packet Loss Enjeksiyonu

Packet loss arttiginda retransmission artar, p95/p99 bozulur, timeout tetiklenebilir.
Bu test, ag kalitesi bozuldugunda uygulamanin ne kadar dayandigini gosterir.

### 6.2 Latency Enjeksiyonu

Latency artinca kuyruklar buyur, tail latency daha hizli bozulur.
Bu test timeout ve retry pencerelerinin dogru ayarlanip ayarlanmadigini ortaya cikarir.

### 6.3 Retry Storm

Hata aninda her istemci ayni anda tekrar deniyorsa trafik katlanir.
Bu durum sorunu iyilestirmek yerine buyutur.

### 6.4 Circuit Breaking

Circuit breaking, sorunlu downstream'e sonsuz trafik basmayi engelleyerek sistemi korur.
Amaç tam cokus yerine kontrollu bozulma ve hizli toparlanmadir.

## 7. Retransmission Deep Dive

Retransmission, TCP'nin guvenilirlik mekanizmasidir: ACK gelmeyen segment tekrar gonderilir.
Bu mekanizma gerekli olsa da surekli yuksek gorulmesi bir kalite veya kapasite sorunu olduguna isaret eder.

### 7.1 Neden Olur?

1. Gercek packet loss
2. Kuyruk/buffer doygunlugu
3. Receiver yavasligi
4. Yol dalgalanmasi veya anlik congestion

### 7.2 Neden Onemlidir?

1. Tail latency artar (p95/p99)
2. Effective throughput duser
3. Uygulama timeout ve retry zinciri tetiklenir

### 7.3 Nasil Yorumlanir?

Retransmission'i tek basina okumak yanlistir.
Asagidaki sinyallerle birlikte okunmalidir:

- RST artisi var mi?
- Timeout loglari artis gosteriyor mu?
- RPS duserken p99 yukseliyor mu?

Bu sinyaller birlikte bozuluyorsa kok neden ag/transport katmanina daha yakin demektir.

## 8. Olay Aninda Hizli Teshis Runbook'u

1. RED metriklerine bak: kullanici etkisi var mi?
2. USE metriklerine bak: saturation var mi?
3. Packet'ta SYN/FIN/RST/retransmission dagilimina bak.
4. LB ve app timeout zincirini kontrol et.
5. TLS degisikligi veya cert rotasyonu oldu mu bak.
6. Retry politikalari olayi buyutuyor mu kontrol et.
7. Gecici mitigasyon uygula, kalici duzeltme planini ayri yaz.

## 9. Cikis Kriteri

Hedef, su sorulara net cevap verebilmektir:

1. Gozlenen semptom hangi katmanda basliyor?
2. Bu bir kapasite problemi mi, konfig uyumsuzlugu mu, ag kalitesi problemi mi?
3. Kisa vadede nasil stabilize edilir?
4. Uzun vadede hangi ayar/mimari degisiklik yapilmali?
