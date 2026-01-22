Log’ları gerçekten anlamak için roadmap (deney + test odaklı)
Amaç:
“Log yazıyorum” seviyesinden çıkıp,
“Bir sistemi sadece log’a bakarak debug edebiliyorum” seviyesine gelmek.
Baseline kur
Basit bir API yaz:
/fast
/slow (sleep 2–5 sn)
Structured log kullan (timestamp, level, requestId, threadId).
Request lifecycle’ı logla
Request başında log
Handler içinde log
Response öncesi log
Amaç: tek bir request’in izini sürmeyi öğrenmek.
Concurrency testi
50–100 paralel istek at.
Log’lardan şunları gözle:
Aynı anda kaç request aktif?
ThreadId’ler nasıl dağılıyor?
Slow endpoint fast olanları nasıl etkiliyor?
Hata senaryosu üret
Random exception fırlat.
Retry eden bir mekanizma ekle.
Log’dan şunu okuyabilir hale gel:
Hangi request neden öldü?
Kaç kere retry oldu?
Nerede takıldı?
Correlation
requestId / taskId ile:
API log’u
Worker log’u
bağlanabilir olsun.
Amaç: “Bu iş nerede kayboldu?” sorusuna sadece log ile cevap verebilmek.

# Logging Roadmap

Bu roadmap, loglama sisteminin temel zorluklarını ve çözüm desenlerini inceleyen deneyleri kapsar.

## 1. Log Rotation
> "Bir uygulama günlük olarak log üretiyor. Log dosyaları belirli boyutlara ulaştığında otomatik olarak yeni dosya açılıyor. Ancak log dosyaları belirli süre boyunca kalıyor ve disk alanı tükeniyor. Log rotation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 2. Log Aggregation
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 3. Log Filtering
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 4. Log Rotation
> "Bir uygulama günlük olarak log üretiyor. Log dosyaları belirli boyutlara ulaştığında otomatik olarak yeni dosya açılıyor. Ancak log dosyaları belirli süre boyunca kalıyor ve disk alanı tükeniyor. Log rotation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 5. Log Aggregation
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 6. Log Filtering
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 7. Log Rotation
> "Bir uygulama günlük olarak log üretiyor. Log dosyaları belirli boyutlara ulaştığında otomatik olarak yeni dosya açılıyor. Ancak log dosyaları belirli süre boyunca kalıyor ve disk alanı tükeniyor. Log rotation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 8. Log Aggregation
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 9. Log Filtering
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 10. Log Rotation
> "Bir uygulama günlük olarak log üretiyor. Log dosyaları belirli boyutlara ulaştığında otomatik olarak yeni dosya açılıyor. Ancak log dosyaları belirli süre boyunca kalıyor ve disk alanı tükeniyor. Log rotation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 11. Log Aggregation
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 12. Log Filtering
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 13. Log Rotation
> "Bir uygulama günlük olarak log üretiyor. Log dosyaları belirli boyutlara ulaştığında otomatik olarak yeni dosya açılıyor. Ancak log dosyaları belirli süre boyunca kalıyor ve disk alanı tükeniyor. Log rotation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."

## 14. Log Aggregation
> "Bir uygulama birden fazla sunucuda çalışıyor. Her sunucuda log dosyaları ayrı ayrı saklanıyor. Log aggregation nasıl yapıldığını ve belirli süre boyunca log dosyalarının silinmesi nasıl yapıldığını inceleyin."    


Amaç, log seviyelerini ve thread/worker mimarisini ezberlemek değil, davranışlarını gözünle görerek içselleştirmek olmalı: Basit bir API kurup her request’in başına ve sonuna INFO, ara adımlarına DEBUG, anormal ama çalışmaya devam eden durumlara WARN, request’i bozan hatalara ERROR, sistemi ayağa kalkamaz hâle getiren durumlara CRITICAL, kullanıcı aksiyonlarına ise ayrı bir kanal olarak AUDIT log yaz; sonra bilinçli olarak seviyeleri yanlış kullanıp prod benzeri ortamda nasıl ya gürültüye boğulduğunu ya da körleştiğini gözlemle, filtreleme yaparak hangi seviyede sistemin “hikâyesini” kaybettiğini gör; paralelinde tek thread pool’lu bir servis kurup fast ve heavy endpoint’leri aynı havuzda koştur, yük altında starvation’ı loglardan oku, ardından heavy işleri ayrı bir worker thread pool’a veya queue + worker mimarisine taşı, handler’ın sadece kuyruğa yazıp dönmesini sağla ve aynı testleri tekrar et; bu sefer queue’ya yazıldı INFO, worker aldı INFO, gecikmeler WARN, task düşmeleri ERROR, worker’ın ayağa kalkamaması CRITICAL olarak loglansın; hedefin, bir sistemi sadece log’a bakarak “şu an sağlıklı mı, tıkanıyor mu, kullanıcı mı hata yaptı, sistem mi çöküyor?” diye ayırt edebilecek refleksi kazanmaktır.