#  BlogApp - ASP.NET Core MVC + Docker + MSSQL

Bu proje, ASP.NET Core MVC ile geliştirilmiş bir blog uygulamasıdır. Kullanıcı yönetimi, blog gönderileri, kategori sistemi ve JWT kimlik doğrulaması içerir. Uygulama Docker ve MSSQL Server kullanılarak konteyner tabanlı bir ortamda çalıştırılabilir.

---

##  Özellikler

- ASP.NET Core MVC mimarisi
- JWT + Cookie authentication
- MSSQL veritabanı ile Entity Framework Core
- Swagger dokümantasyonu
- Docker + Docker Compose ile hızlı kurulum
- Migration + Seed verileri ile örnek veri tabanı

---

##  Kullanılan Teknolojiler

- .NET 8.0
- ASP.NET Core MVC
- Entity Framework Core
- MSSQL Server (Docker imajı)
- Docker / Docker Compose
- Swagger UI
- JWT Authentication

---

##  Docker ile Kurulum Adımları

1. **Projeyi klonlayın:**
   ```bash
   git clone https://github.com/kullaniciadi/BlogApp.git
   cd BlogApp

2. **(Opsiyonel) Daha önce oluşturulmuş volume’leri temizleyin:**
   ```bash
    docker-compose down -v
 3. **Docker imajlarını build edin ve konteynerleri başlatın:**
    ```bash
    docker-compose up --build

4. **Kurulum tamamlandığında uygulamaya erişin:**
```bash
-Ana Sayfa: http://localhost:8080
-Swagger Dokümantasyonu: http://localhost:8080/swagger




   
