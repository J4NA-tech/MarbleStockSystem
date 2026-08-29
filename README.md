# Mermer Stok ve Satış Takip Sistemi

Windows Forms tabanlı, N-Katmanlı mimari kullanılarak geliştirilmiş mermer stok ve satış takip uygulaması.

## Proje Yapısı

Bu proje **N-Katmanlı Mimari** (N-Tier Architecture) prensiplerine uygun olarak geliştirilmiştir:

- **MarbleStockSystem.DAL** (Data Access Layer): Veritabanı erişim katmanı
  - Entity Framework Core ORM
  - Generic Repository Pattern
  - Entity'ler ve DbContext

- **MarbleStockSystem.BLL** (Business Logic Layer): İş mantığı katmanı
  - Service katmanı
  - İş kuralları ve validasyonlar
  - SOLID prensipleri

- **MarbleStockSystem.PL** (Presentation Layer): Kullanıcı arayüzü katmanı
  - Windows Forms
  - Dependency Injection

## Teknolojiler

- .NET 8.0
- Windows Forms
- Entity Framework Core 8.0
- SQL Server (LocalDB)
- Dependency Injection (Microsoft.Extensions.DependencyInjection)

## Özellikler

### Mermer Yönetimi
- Mermer ekleme, güncelleme, silme ve listeleme
- Mermer özellikleri: Ad, Tip, Renk, Kalınlık, Fiyat/m², Stok miktarı

### Müşteri Yönetimi
- Müşteri ekleme, güncelleme, silme ve listeleme
- Müşteri bilgileri: Ad Soyad, Telefon, Adres

### Satış İşlemleri
- Satış yapma
- Otomatik stok azaltma
- Otomatik fiyat hesaplama
- Stok yetersizliği kontrolü

## İş Kuralları

1. **Stok Kontrolü**: Satış yapılırken stok miktarı kontrol edilir. Yetersiz stok durumunda satış yapılamaz.
2. **Otomatik Stok Güncelleme**: Satış yapıldığında stok otomatik olarak azalır.
3. **Otomatik Fiyat Hesaplama**: Satış fiyatı, mermer fiyatı × miktar formülü ile otomatik hesaplanır.
4. **Stok Geri Ekleme**: Satış silindiğinde stok miktarı geri eklenir.

## Veritabanı

Uygulama **Code First** yaklaşımı kullanır. İlk çalıştırmada veritabanı otomatik oluşturulur.

### Tablolar

1. **Marbles** (Mermerler)
   - MarbleId (PK)
   - Name
   - Type
   - Color
   - Thickness
   - PricePerM2
   - StockQuantity

2. **Customers** (Müşteriler)
   - CustomerId (PK)
   - FullName
   - Phone
   - Address

3. **Sales** (Satışlar)
   - SaleId (PK)
   - MarbleId (FK)
   - CustomerId (FK)
   - Quantity
   - TotalPrice
   - SaleDate

## Kurulum

1. Projeyi klonlayın veya indirin
2. Visual Studio 2022 veya üzeri bir sürüm kullanın
3. SQL Server LocalDB'nin yüklü olduğundan emin olun
4. Solution'ı açın ve restore edin
5. Connection string'i `Program.cs` dosyasında düzenleyin (gerekirse)
6. Projeyi çalıştırın

## Connection String

Varsayılan connection string:
```
Server=(localdb)\mssqllocaldb;Database=MarbleStockSystemDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

Farklı bir SQL Server kullanmak için `MarbleStockSystem.PL/Program.cs` dosyasındaki connection string'i güncelleyin.

## Kullanım

1. Uygulamayı başlattığınızda ana menü ekranı açılır
2. **Mermer Yönetimi**: Mermer ekleme, düzenleme ve silme işlemleri
3. **Müşteri Yönetimi**: Müşteri ekleme, düzenleme ve silme işlemleri
4. **Satış Yap**: Mermer ve müşteri seçerek satış işlemi yapma

## Mimari Prensipler

- **SOLID Prensipleri**: Tüm katmanlarda SOLID prensiplerine uyulmuştur
- **Dependency Injection**: Servisler ve repository'ler DI ile yönetilir
- **Generic Repository Pattern**: Tüm entity'ler için ortak CRUD işlemleri
- **Separation of Concerns**: Her katman kendi sorumluluğuna odaklanır
- **Clean Code**: Açıklayıcı yorumlar ve temiz kod yapısı

## Geliştirici Notları

- Entity Framework Core migrations kullanılmamıştır (Code First otomatik oluşturma)
- İlk çalıştırmada veritabanı otomatik oluşturulur
- Foreign key constraint'ler nedeniyle ilişkili kayıtlar silinemez

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir.



