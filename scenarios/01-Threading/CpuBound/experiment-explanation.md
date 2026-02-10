# Deney #2: CPU Darboğazı (CPU Bound) - Açıklama

Bu deneyin amacı, sistemin **I/O beklemesi yerine işlemci (CPU) darboğazına** girdiğinde nasıl tepki verdiğini simüle etmek ve gözlemlemektir.

## 🛠 Neler Yapıldı?

1.  **"İşlemciyi Yoracak" Kod Eklendi (`CpuBoundApi/Program.cs`)**
    *   API'ye `/experiments/cpu` adında endpoint eklendi.
    *   Bu endpoint, veritabanı veya ağ işlemi (I/O) yapmak yerine **yoğun matematiksel işlem** yapar.
    *   **Yöntem:** Belirli bir sayıya kadar (varsayılan: 10,000) olan asal sayıları "brute-force" (kaba kuvvet) yöntemiyle hesaplar. Bu yöntem bilerek verimsiz seçilmiştir; böylece her istek geldiğinde sunucu işlemcisi %100 yük altına girer ve ilgili thread kilitlenir.

2.  **Yük Testi Senaryosu Oluşturuldu (`k6/cpu-bound.js`)**
    *   `k6` yük testi aracı için özel bir senaryo hazırlandı.
    *   Senaryo, sürekli olarak `/experiments/cpu` endpoint'ine istek gönderir.
    *   Her istekte `n=20000` parametresi gönderilerek işlemcinin her seferinde ciddi bir hesaplama yapması sağlanır.

3.  **Otomasyon Scripti Hazırlandı (`run-experiment-2-cpu.sh`)**
    *   Deneyi otomatize etmek için bir Bash script yazıldı.
    *   Sistem sırasıyla **5 RPS**, **20 RPS** ve **50 RPS** yük altında test edilir.

## 🎯 Beklenen Sonuç

*   **Düşük Yük (5 RPS):** İşlemci talebi karşılayabilir, sistem yanıt verir.
*   **Yüksek Yük (20+ RPS):** İşlemci kapasitesi (CPU core sayısı) yetersiz kalmaya başlar.
    *   **CPU Starvation:** İş parçacıkları (threads) işlemci zamanı bulamaz.
    *   **Belirtiler:** Yanıt süreleri (Latency) dramatik şekilde artar, Timeout hataları başlar ve hatta normalde çok hızlı çalışan `/health` endpointi bile yavaşlayabilir.
