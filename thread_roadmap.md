


Thread Pool / Worker Pool’u derin anlamak için roadmap (deney + stres test)
Amaç:
“Teoride biliyorum”dan çıkıp,
“Yanlış tasarımın sistemi nasıl boğduğunu gözümle gördüm” seviyesine gelmek.
Tek pool ile başla
API içinde:
/fast
/cpu-heavy (loop, hash, vs.)
Her şey aynı thread pool’da çalışsın.
Test:
100 paralel istek at.
Gözle:
/fast neden yavaşlıyor?
Response süreleri nasıl patlıyor?
Starvation’ı bilinçli üret
Pool size: örn. 10 thread
10 tane /cpu-heavy isteği at
Ardından /fast çağır
Beklenen:
/fast bekler.
Sistem “ayakta ama cevap vermiyor” hâline gelir.
Worker thread pool ayır
Heavy işleri ayrı bir pool’a taşı.
Request handler sadece:
işi worker pool’a submit etsin
hemen dönsün
Aynı testi tekrar yap.
Gözle:
/fast artık neden etkilenmiyor?
Latency grafiği nasıl değişti?
Queue + worker simülasyonu
In-memory queue kur.
Worker thread’leri bu queue’dan iş çeksin.
Test:
Queue’yu bilinçli şişir.
Worker sayısını azalt / artır.
Şunu net gör:
Backpressure nasıl oluşuyor?
Worker sayısı artınca neresi tıkanıyor?
Failure senaryosu
Worker’ı ortada öldür.
İş yarım kalsın.
Retry mekanizması ekle.
Amaç:
“Asılı kalan task” nasıl oluşur?
Bunu mimari olarak nasıl engellersin?
Bu iki roadmap’i bitirdiğinde:
Log senin için “debug output” değil, sistemin röntgeni olur.
Thread pool / worker pool farkı “teori” değil, kas hafızası olur.