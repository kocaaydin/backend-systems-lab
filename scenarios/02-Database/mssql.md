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





	1.	SQL Server Giriş-Hizmeti benzetmesi ile SQL Server çalışma mantığı anlatımı
	2.	SQL Server 2014 Kurulumu 1 | Kurulum menüleri
	3.	SQL Server 2014 Kurulumu 2 | Failover Cluster Kavramı
	4.	SQL Server 2014 Kurulumu 3 | Sürüm Karşılaştırma
	5.	SQL Server 2014 Kurulumu 4 | Kurulum Adımları
	6.	SQL Server 2014 Kurulumu 5 | SQL Server Replication Kavramı
	7.	SQL Server 2014 Kurulumu 6 | Feature Selection
	8.	SQL Server 2014 Kurulumu 7 | Framework 3.5 Kurulumu
	9.	SQL Server 2014 Kurulumu 8 | Database Engine Servisleri
	10.	SQL Server 2014 Kurulumu 9 | Server Collation Kavramı
	11.	SQL Server 2014 Kurulumu 10 | Kurulumun Tamamlanması
	12.	Sistem Databaseleri 2
	13.	Sistem Databaseleri 1
	14.	Veritabanı İşlemleri | Veritabanı oluşturma
	15.	SQL Server Veri Tipleri | Tablo oluşturma
	16.	SQL Server Veri Tipleri | Sayısal Veri Tipleri
	17.	SQL Server Veri Tipleri | String Veri Tipleri
	18.	SQL Server Veri Tipleri | Tarih Saat Veri Tipleri
	19.	SQL Server Veri Tipleri | Diğer Veri Tipleri
	20.	SQL Server Veri Tipleri | Information Schema View’ları ile kolonları listelemek
	21.	SQL Server’da Index Kavramı | Örnek Data Oluşturma, Excel’den Import
	22.	SQL Server’da Index Kavramı | Script ile Random Kayıt Atma
	23.	SQL Server’da Index Kavramı | Index Giriş
	24.	SQL Server’da Index Kavramı | Binary Search Algoritması ile Index Kullanımı
	25.	SQL Server’da Index Kavramı | Büyük Veride Index Performans Hesaplaması
	26.	SQL Server’da Index Kavramı | Bir Milyon Satırda Index Performans Testi
	27.	SQL Server’da Index Kavramı | Index Maliyeti
	28.	SQL Server’da Index Kavramı | Fill Factor Kavramı
	29.	SQL Server’da Index Kavramı | Index Fragmentation
	30.	SQL Server’da Index Kavramı | İstatistikler
	31.	View’lar | Örnek Logo Datasını Sisteme Dönme
	32.	View’lar | View Oluşturma
	33.	Stored Procedure | Stored Procedure Kavramı
	34.	Stored Procedure | Stored Procedure Kullanmanın Faydaları, Memory Hızında Çalışma
	35.	Stored Procedure | Stored Procedure Kullanmanın Diğer Faydaları
	36.	Stored Procedure | Stored Procedure Yazma Uygulaması
	37.	Stored Procedure | Procedure İçinden Başka Procedure Çağırma
	38.	Stored Procedure | RaiseError ile Hata Bastırma
	39.	Stored Procedure | Procedure Execution İstatistiklerini Görme
	40.	Stored Procedure | SQL Injection Uygulaması
	41.	User Defined Function | Temel Fonksiyon Kavramı, Ay Getiren Fonksiyon Yazma
	42.	User Defined Function | Aylık Satış Toplamı Getiren Fonksiyon
	43.	User Defined Function | Rakamı Yazıya Çeviren Fonksiyon
	44.	User Defined Function | Table Valued Functions
	45.	Transaction Kavramı | Temel Anlamda OLTP
	46.	Transaction Kavramı | MDF, LDF Dosyalar
	47.	Transaction Kavramı | Örnek E-Ticaret Uygulaması
	48.	Trigger’lar | Trigger Kavramı
	49.	Trigger’lar | Trigger ile Toplam Tablosu Güncelleme
	50.	Trigger’lar | Değiştirilen/Silinen Kayıtları Loglama-1 (Management Studio)
	51.	Trigger’lar | Silinen / Değiştirilen Kayıtları Loglama-2 (Logo Tiger Üzerinden)
	52.	SQL Backup / Restore İşlemleri | Temel Backup Restore İşlemleri
	53.	SQL Backup / Restore İşlemleri | Backup Türleri
	54.	SQL Backup / Restore İşlemleri | Backup Stratejisi Planlama
	55.	SQL Backup / Restore İşlemleri | Backup Uygulama
	56.	SQL Server DB Mail | DB Mail Konfigürasyonu
	57.	SQL Server DB Mail | Send Email Script Yazma
	58.	SQL Server Agent Kavramı
	59.	Maintenance Plan | Maintenance Plan Kavramı
	60.	Maintenance Plan | DB Shrink
	61.	Maintenance Plan | Full Backup Wizard
	62.	Maintenance Plan | Full Backup Wizard Olmadan
	63.	Maintenance Plan | Differential Backup
	64.	Maintenance Plan | Index Bakımları (Rebuild)
	65.	Maintenance Plan | İstatistik Güncelleme
	66.	Büyük Veri Uygulamaları | Büyük Veri Kavramı
	67.	Büyük Veri Uygulamaları | Veritabanı Oluşturma
	68.	Büyük Veri Uygulamaları | 125 Milyon Satırlık Eczane Datası
	69.	Büyük Veri Uygulamaları | 125 Milyon Satırlık Eczane Simülasyonu-2
	70.	Büyük Veri Uygulamaları | 125 Milyon Satırlık Eczane Simülasyonu-3
	71.	Büyük Veri Uygulamaları | 125 Milyon Satırlık Eczane Simülasyonu-4
	72.	Büyük Veri Uygulamaları | 1 Milyar Satırlık Eczane Simülasyonu-1
	73.	Büyük Veri Uygulamaları | Büyük Veride SQL Server Profiler
	74.	İleri Seviye Administration | SP_Configure Komutu
	75.	İleri Seviye Administration | XP_CMDShell Komutu
	76.	İleri Seviye Administration | Donanımların MSSQL Performansı Üzerindeki Etkisi
	77.	Felaket Yönetimi | SQL Server Log Shipping
	78.	Felaket Yönetimi | Database Mirroring
	79.	İleri Seviye TSQL | TSQL ile Merkez Bankası Döviz Kurlarını Çekme Bölüm 1
	80.	İleri Seviye TSQL | TSQL ile Merkez Bankası Döviz Kurlarını Çekme Bölüm 2
	81.	İleri Seviye TSQL | Bir klasördeki resimleri toplu olarak veritabanına yazma
	82.	İleri Seviye TSQL | Veritabanında binary tutulan dosyaları toplu dışarı alma
	83.	İleri Seviye TSQL | Google Translate servisini kullanarak TSQL ile çeviri yapma
	84.	Sizlerden gelen sorular bölümü | Ensar Kartal :16.01.2018
	85.	Sizlerden gelen sorular bölümü | Ali Yeşilçiçek, Linked server kullanımı
	86.	Sizlerden gelen sorular bölümü | Ali Yeşilçiçek, TSQL Kodları ile FTP Kullanma
	87.	Sizlerden gelen sorular bölümü | Ömer, Otel Rezervasyon Sistemi
	88.	SQL Server Always On-1 Disaster Recovery Kavramları
	89.	SQL Server Always On-2 SQL Server Kurulum
	90.	SQL Server Always On-3 Failover Cluster Kurulum
	91.	SQL Server Always On-4 Always On Kurulum
	92.	SQL Server Always On-5 Performans Testi 1
	93.	SQL Server Always On-6 Logo Tiger Üzerinde Gerçek Süpermarket Simülasyonu
	94.	SQL Server Always On-7 Secondary Node kapalı iken Index oluşturma ve Index Rebuild