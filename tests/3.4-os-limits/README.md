# Deney 3.4: OS Limit Simulation (ulimit)

## Amaç
Uygulama ne kadar optimize olursa olsun, işletim sistemi limitlerinin (ulimit) en üst sınırı belirlediğini göstermek.

**OS Limits (File Descriptors) Nedir?**
Linux tabanlı sistemlerde "her şey bir dosyadır". TCP bağlantıları (socket) da işletim sistemi gözünde bir dosya (File Descriptor - FD) olarak sayılır. `ulimit -n` komutu ile bir sürece (process) aynı anda açabileceği maksimum dosya sayısı limiti konur. Eğer bu limit düşükse (örneğin 1024), uygulamanız teorik olarak binlerce isteği kaldırabilecek güçte olsa bile, işletim sistemi 1025. bağlantıyı açmasına izin vermez ve uygulama **crash** olur veya hata verir.

## Senaryo
*   **Limit:** `api` servisi için Docker seviyesinde `ulimits: nofile: 50` ayarlandı. Bu, uygulamanın aynı anda en fazla 50 dosya (soketler de dosyadır) açabileceği anlamına gelir.
*   **Dahil Olanlar:** .NET runtime'ın kendi açtığı dosyalar (dll'ler vs) da bu limite dahildir, yani elimize dışarıya açmak için çok az (belki 10-20) soket hakkı kalır.
*   **Yük:** 100 Eşzamanlı Kullanıcı (VU). Her biri dış servise bağlanmaya çalışıyor.

## Beklenen Davranış
*   Uygulama kısa sürede **"Too many open files"** veya benzeri `IOException` / `SocketException` hataları fırlatmaya başlar.
*   k6 raporunda yüksek oranda **Hata (Fail Rate)** görülür.

## Nasıl Çalıştırılır?
```bash
./tests/3.4-os-limits/run.sh
```
