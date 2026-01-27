MSSQL Performance & Design Reflex Lab

Amaç

Bu lab’in amacı, MSSQL tarafında:

-   Şema tasarımı
-   Index stratejisi
-   Sorgu optimizasyonu
-   Execution Plan okuma
-   IO / CPU / Memory etkilerini sezgisel olarak kavrama
-   “Bu niye yavaş?” sorusuna içgüdüsel cevap verebilme

reflekslerini geliştirmektir.

Hedef: “Bu sistem neden yavaş?” sorusunu 3 dakikada kök sebebe
indirebilen biri olmak.

------------------------------------------------------------------------

Ortam

-   SQL Server Developer Edition (Local)
-   SSMS
-   Query Store: ON
-   LabDb adında bir database

Veri modeli: Basit bir e-ticaret simülasyonu

Customers (1M) Orders (5M) OrderItems (20M) Products (200K) Payments
(5M) Shipment (5M)

Veriler eşit dağılmayacak: - %5 müşteri, siparişlerin %60’ını versin -
Son 30 gün, datanın %40’ı olsun - Bazı ürünler aşırı hot olsun

------------------------------------------------------------------------

Senaryo Grupları

1. Kötü Tasarım Lab’i

Bilerek yanlış yap: - PK olmayan tablolar - Clustered index’siz büyük
tablolar - NVARCHAR(MAX) ile join - Tarih alanı üzerinde fonksiyon
kullanılan sorgular - Aynı kolon üzerinde 5 farklı nonclustered index

Senaryo: “Order ekranı 4–6 saniye sürüyor. Kod değişmeyecek. Sadece DB
tarafında çöz.”

Görevlerin: - Actual Execution Plan incele - Scan mi var, Lookup mı
patlıyor, cardinality mi bozuk? - Hangi index gerçekten işe yarıyor?

Amaç: Planı gördüğün anda “Bu Seek olmalıydı” diyebilmek.

------------------------------------------------------------------------

2. Update / Write Path Ayrıştırma Lab’i

Model:

Orders -> OLTP Orders_Stage -> Write Buffer Orders_Report -> Read
Optimized

Akış: 1. Uygulama Orders_Stage tablosuna yazar. 2. SQL Agent Job: - Her
5 dakikada bir: - Stage’den batch alır - Transform eder - Orders_Report
tablosuna yazar 3. Raporlar sadece Orders_Report üzerinden çalışır.

Senaryolar: - Job 20 dakikada bir çalışsın → Raporlar gecikmeli ama
hızlı - Job her 1 dakikada bir çalışsın → IO patlaması - Batch size 1K /
10K / 100K karşılaştır

Sorular: - Batch büyüdükçe log büyümesi nasıl değişiyor? - Lock
escalation ne zaman başlıyor? - Rapor sorguları write yükünden nasıl
etkileniyor?

Amaç: Write path ile read path ayrılmazsa sistem ölür gerçeğini
içselleştirmek.

------------------------------------------------------------------------

3. Execution Plan Okuma Refleksi

Her sorguda:

SET STATISTICS IO ON; SET STATISTICS TIME ON;

Sor: - Neden Scan yaptı? - Bu join neden Nested Loop? - Estimated vs
Actual neden farklı? - Parameter sniffing var mı?

------------------------------------------------------------------------

4. Büyüyen Sistem Senaryosu

Aynı sorgu: - 100K satır - 1M satır - 10M satır

Her aşamada: - Süre - IO - Plan değişimi

Sorular: - Ne zaman Seek → Scan oldu? - Ne zaman Hash Join çıktı? - Bu
tasarım büyümeye hazır mıydı?

------------------------------------------------------------------------

Çıktı Formatı

{ “timestamp”: “2026-01-25T21:30:00”, “scenario”: “StageToReportBatch”,
“rows”: 50000, “duration_ms”: 1830, “io_reads”: 42000, “plan_notes”:
“Hash Join + Table Scan. Missing index on Orders_Report(OrderDate)”,
“decision”: “Batch 20k üstü log pressure yaratıyor” }
