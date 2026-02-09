# Kişiselleştirilmiş Öncelik Sıralaması (Roadmap of Roadmaps)

## 1. Öncelik: Yangın Söndürme & Temeller 
* Performanslı uygulama geliştirmen için önce veritabanı ve kaynak yönetimi konularını halletmen gerek.*

*   **Kaynak:** `storage_consistency_roadmap.md`
    *   **Odak:** Connection Pool Exhaustion, Slow Query Etkileri, Lock/Deadlock.
*   **Kaynak:** `experiments_roadmap.md`
    *   **Odak:** **Thread Starvation** (Bu .NET performansının en kritik konusudur).

## 2. Öncelik: Sistem Sağlamlığı 
*Yeni iş mülakatlarında "Sistemin çökünce ne yapıyorsun?" sorusuna cevap verebilmek için.*

*   **Kaynak:** `resilience_roadmap.md`
    *   **Odak:** Retry Storm (Yanlış retry sistemi nasıl öldürür?), Circuit Breaker, Timeout yönetimi.

## 3. Öncelik: Mimari Derinleşme (Büyük Hedef)
*Mikroservis dünyasına girmek için. İlk iki maddeyi halletmeden buna geçersen temel eksik kalır.*

*   **Kaynak:** `microservice_roadmap.md` & `q_roadmap.md`
    *   **Odak:** Asenkron iletişim, RabbitMQ Backpressure, Eventual Consistency.

---
**Özet:** Önce **Storage & Threading** (Hayatta Kal), sonra **Resilience** (Güçlen), en son **Microservices** (Yüksel).
