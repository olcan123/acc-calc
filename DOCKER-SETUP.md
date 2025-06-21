# ACC-CALC Docker Setup ✅ BAŞARILI

Bu dokümantasyon ACC-CALC uygulamasının Docker ile nasıl çalıştırılacağını açıklar.

**🎉 Durum: Docker konfigürasyonu başarıyla tamamlandı ve tüm servisler çalışıyor!**

## Gereksinimler

- Docker Desktop
- Docker Compose
- Windows PowerShell/Command Prompt

## Proje Yapısı

```
acc-calc/
├── NetCoreBackend/           # .NET Core API
│   ├── Dockerfile
│   ├── WebAPI/
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json  # Yerel geliştirme (korundu)
│   │   └── appsettings.Docker.json       # Docker ortamı
│   └── ...
├── VueFrontend/              # Vue.js Frontend
│   ├── Dockerfile
│   ├── nginx.conf
│   ├── src/config/
│   │   ├── .env              # Yerel geliştirme (korundu)
│   │   └── .env.docker       # Docker ortamı
│   └── ...
├── docker-compose.yml        # Production Docker setup
├── docker-compose.dev.yml    # Development Docker setup
├── start-docker.bat         # Windows başlatma scripti
└── stop-docker.bat          # Windows durdurma scripti
```

## Hızlı Başlangıç

### 1. Docker ile Çalıştırma (Production)

```bash
# Windows'ta
start-docker.bat

# Veya manuel olarak
docker-compose up --build -d
```

### 2. Geliştirme Ortamı

```bash
# Yerel SQL Server ile geliştirme ortamı
docker-compose -f docker-compose.dev.yml up --build -d
```

## Servis Erişimi

### Production Ortamı
- **Frontend**: http://localhost
- **Backend API**: http://localhost:5000
- **SQL Server**: localhost:1433 (sa/AccCalc123!)

### Development Ortamı
- **Backend API**: http://localhost:5001
- **SQL Server**: localhost:1434 (sa/AccCalc123!)

## Ortam Konfigürasyonları

### .NET Backend

Proje iki farklı konfigürasyona sahip:

1. **appsettings.Development.json** (Yerel geliştirme - korundu)
   - LocalDB/SQL Express bağlantısı
   - Geliştirme ayarları

2. **appsettings.Docker.json** (Docker ortamı)
   - Docker SQL Server bağlantısı
   - Production benzeri ayarlar

### Vue Frontend

İki farklı environment file:

1. **src/config/.env** (Yerel geliştirme - korundu)
   - `VITE_API_URL=https://localhost:7096/api/`

2. **src/config/.env.docker** (Docker ortamı)
   - `VITE_API_URL=http://localhost:5000/api/`

## Docker Komutları

### Temel Komutlar

```bash
# Servisleri başlat
docker-compose up -d

# Servisleri durdur
docker-compose down

# Logları görüntüle
docker-compose logs -f

# Servis durumunu kontrol et
docker-compose ps

# Yeniden build et
docker-compose up --build -d
```

### Debugging

```bash
# Backend container'ına bağlan
docker exec -it acc-calc-backend bash

# SQL Server'a bağlan
docker exec -it acc-calc-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P AccCalc123!

# Frontend container'ına bağlan
docker exec -it acc-calc-frontend sh
```

### Temizlik

```bash
# Tüm container'ları durdur ve sil
docker-compose down --remove-orphans

# Volume'ları da sil (DİKKAT: Veritabanı verileri silinir!)
docker-compose down -v

# Kullanılmayan Docker objelerini temizle
docker system prune -a
```

## Veri Kalıcılığı

- SQL Server verileri `sqlserver_data` volume'unda saklanır
- Container'lar silinse bile veriler korunur
- Verileri silmek için: `docker-compose down -v`

## Ağ Konfigürasyonu

Servisler `acc-calc-network` adlı bridge network üzerinde haberleşir:

- Frontend → Backend: `http://backend:5000`
- Backend → SQL Server: `sqlserver:1433`

## Sorun Giderme

### 1. Port Çakışması
```bash
# Kullanılan portları kontrol et
netstat -ano | findstr :80
netstat -ano | findstr :5000
netstat -ano | findstr :1433
```

### 2. SQL Server Bağlantı Problemi
```bash
# SQL Server durumunu kontrol et
docker-compose logs sqlserver

# Bağlantıyı test et
docker exec -it acc-calc-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P AccCalc123! -Q "SELECT 1"
```

### 3. Frontend Build Problemi
```bash
# Frontend loglarını kontrol et
docker-compose logs frontend

# Node.js version kontrol et
docker run --rm node:20-alpine node --version

# Pnpm lockfile güncellemesi gerekirse
cd VueFrontend
pnpm install
```

### 4. Backend API Problemi
```bash
# Backend loglarını kontrol et
docker-compose logs backend

# Health check
curl http://localhost:5000/health
```

### 5. Pnpm Lockfile Problemi
```bash
# Eğer pnpm-lock.yaml güncel değilse
cd VueFrontend
pnpm install  # Lockfile'ı günceller

# Sonra Docker build tekrar deneyin
docker-compose up --build frontend
```

### 6. Vue Component Problemi
```bash
# Eğer boş Vue dosyaları nedeniyle build hatası alıyorsanız
# PowerShell ile boş dosyaları bulun:
cd VueFrontend
Get-ChildItem -Path src -Filter "*.vue" -Recurse | Where-Object {$_.Length -eq 0}

# Bu dosyalar için minimal template'ler oluşturulmuştur
# Eğer yeni boş dosyalar varsa, en az şu içeriği ekleyin:
# <template><div>Geliştirme aşamasında</div></template>
# <script setup></script>
```

## Geliştirme Workflow'u

1. **Yerel Geliştirme**: Mevcut VS Code/Visual Studio kurulumunuzu kullanın
2. **Test**: Docker ile tam stack test edin
3. **Production**: Docker Compose ile deploy edin

```bash
# Geliştirme
npm run dev                    # Frontend
dotnet run                     # Backend

# Test
docker-compose up --build      # Full stack

# Production
docker-compose -f docker-compose.yml up -d
```

## Güvenlik Notları

- Production'da güçlü şifreler kullanın
- Secrets management sistemi entegre edin
- SSL/TLS sertifikaları ekleyin
- Database backup stratejisi oluşturun

## Performans İpuçları

- Multi-stage build kullanılarak image boyutları optimize edildi
- Non-root user ile güvenlik artırıldı
- Health check'ler eklendi
- Nginx ile static content optimize edildi

---

**Not**: Bu setup mevcut geliştirme ortamınızı bozmayacak şekilde tasarlanmıştır. Yerel konfigürasyonlarınız korunmuş ve Docker için ayrı konfigürasyonlar oluşturulmuştur.

## ✅ Docker Setup Tamamlandı!

**Tarih**: 20 Haziran 2025

### 🎉 Başarılı Kurulum Onayı

Tüm servisler başarıyla çalışıyor ve sağlıklı durumda:

```bash
$ docker-compose ps
NAME                 IMAGE                                        STATUS                    PORTS
acc-calc-backend     acc-calc-backend                             Up (healthy)              0.0.0.0:5000->5000/tcp
acc-calc-frontend    acc-calc-frontend                            Up (healthy)              0.0.0.0:80->80/tcp
acc-calc-sqlserver   mcr.microsoft.com/mssql/server:2022-latest   Up (healthy)              0.0.0.0:1433->1433/tcp
```

### ✅ Doğrulanmış Özellikler

1. **Frontend (Vue.js)**
   - ✅ http://localhost adresinde erişilebilir
   - ✅ Nginx proxy düzgün çalışıyor
   - ✅ Vue build başarılı
   - ✅ Healthcheck geçiyor

2. **Backend (.NET Core)**
   - ✅ http://localhost:5000 adresinde erişilebilir
   - ✅ SQL Server bağlantısı çalışıyor
   - ✅ Database otomatik oluşturuluyor
   - ✅ API endpoint'leri çalışıyor (örn: /api/accounts)
   - ✅ Healthcheck geçiyor (/health endpoint)

3. **Database (SQL Server)**
   - ✅ Başarıyla başlatılıyor
   - ✅ NetCoreBackend database otomatik oluşturuluyor
   - ✅ Entity Framework EnsureCreated() çalışıyor
   - ✅ Healthcheck geçiyor

4. **Güvenlik Güncellemeleri**
   - ✅ Node.js güncellendi (node:22-alpine)
   - ✅ Nginx güncellendi (nginx:1.25-alpine)
   - ✅ apt paketleri güncellendi (apk update && apk upgrade)
   - ✅ curl healthcheck için yüklendi

### 🔧 Çözülen Problemler

1. **SQL Server Connection**: EnsureCreated() ile düzeltildi
2. **Vue Build Errors**: Boş .vue dosyaları dolduruldu
3. **Docker Health Checks**: curl yüklendi ve healthcheck'ler düzeltildi
4. **Environment Configuration**: Docker-specific configs oluşturuldu
5. **Nginx Proxy**: API proxy'si düzgün çalışıyor
6. **Security Vulnerabilities**: Base image'lar güncellendi

### 🚀 Kullanıma Hazır

Artık aşağıdaki komutlarla uygulamayı çalıştırabilirsiniz:

```bash
# Tüm servisleri başlat
docker-compose up -d

# Durumu kontrol et
docker-compose ps

# Uygulamaya eriş
# Frontend: http://localhost
# Backend API: http://localhost:5000
# SQL Server: localhost:1433 (sa/AccCalc123!)
```
