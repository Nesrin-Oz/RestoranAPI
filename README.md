# Restaurant Reservation System (Client-Server Architecture)

A robust Client-Server architecture solution for restaurant table management and reservation scheduling. This project demonstrates backend API development, asynchronous UI updates, and strict database conflict resolution mechanisms.

## System Architecture

The solution is divided into two decoupled layers:
*   **RestoranAPI:** An ASP.NET Core Web API acting as the central backend. It handles business logic, database connections via ADO.NET, and utilizes Dependency Injection for clean service management.
*   **RestoranWinForm:** A Windows Forms desktop client that consumes the RESTful API endpoints using `HttpClient`. It employs Task-based Asynchronous Patterns (async/await) and multithreading to ensure a responsive user interface during network calls.

## Key Features

*   **Conflict Prevention Algorithm:** The API evaluates existing bookings at the database level and strictly prevents overlapping reservations for the same table.
*   **Dynamic Slot Calculation:** Calculates and returns unbooked time slots for any requested table within operational hours (10:00 - 22:00).
*   **Secure Session Memory:** The desktop application utilizes Windows Registry to securely remember user login credentials across sessions.
*   **Cross-Thread UI Updates:** Implements thread-safe UI updates in WinForms when retrieving and displaying remote data.

## Technology Stack
*   **Backend:** C#, ASP.NET Core Web API, Swagger UI
*   **Frontend:** Windows Forms (.NET), Newtonsoft.Json
*   **Database:** MS SQL Server (ADO.NET)



## Restoran Rezervasyon Sistemi (İstemci-Sunucu Mimarisi)

Restoran masa yönetimi ve rezervasyon planlaması için geliştirilmiş İstemci-Sunucu (Client-Server) mimarisi. Bu proje; arka uç API geliştirmesini, asenkron arayüz yönetimini ve veritabanı seviyesinde çakışma çözümleme mekanizmalarını göstermektedir.

**Sistem Mimarisi**
*   **RestoranAPI:** Merkezi arka uç olarak çalışan ASP.NET Core Web API. İş mantığını ve ADO.NET üzerinden veritabanı bağlantılarını yönetir; temiz kod mimarisi için Bağımlılık Enjeksiyonu (Dependency Injection) kullanır.
*   **RestoranWinForm:** RESTful API uç noktalarını `HttpClient` ile tüketen masaüstü istemcisi. Ağ çağrıları sırasında arayüzün donmasını engellemek için Task tabanlı Asenkron Desenler (async/await) ve Threading kullanır.

**Temel Özellikler**
*   **Çakışma Önleme Algoritması:** Mevcut rezervasyonları veritabanında sorgulayarak aynı masaya ait saat çakışmalarını kesin olarak engeller.
*   **Dinamik Boş Saat Hesaplama:** İstenilen masa için çalışma saatleri (10:00 - 22:00) arasındaki uygun zaman dilimlerini hesaplar.
*   **Güvenli Oturum Hafızası:** Kullanıcı giriş bilgilerini oturumlar arası hatırlamak için Windows Registry kullanır.
*   **Thread-Safe Arayüz Güncellemeleri:** Uzak sunucudan veri çekerken WinForms üzerinde güvenli (cross-thread) kontrolleri uygular.

**Teknoloji Yığını**
*   **Arka Uç:** C#, ASP.NET Core Web API, Swagger UI
*   **Ön Uç:** Windows Forms (.NET), Newtonsoft.Json
*   **Veritabanı:** MS SQL Server (ADO.NET)
