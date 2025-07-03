## 專案概述

本專案負責從外部端點獲取資料並同步更新至本地資料庫，採用現代化的軟體架構模式，確保系統的可維護性、擴展性和穩定性。

## 技術架構

### 設計模式

| 模式 | 實作技術 | 效益 |
|------|----------|------|
| Mediator Pattern | MediatR 實現 CQRS 架構 | 將業務邏輯與控制器解耦 |
| 讀寫分離 | 資料庫層採用 CQRS 模式 | 優化讀取與寫入效能 |
| Repository Pattern | 資料存取層抽象化 | 提升可測試性 |

### 核心功能
- 外部 API 資料獲取與同步
- 完整的錯誤處理機制
- 讀寫分離資料庫架構
- Docker 容器化部署
- 結構化日誌記錄

---

## 快速開始
### 前置需求
-  .NET 8.0 SDK
-  Docker & Docker Compose

### 安裝步驟

#### **複製專案**
```bash
git clone [repository-url]
cd [project-name]
```

#### **Docker 部署**
```bash
docker-compose up -d
```

#### **檢查服務狀態**
```bash
docker-compose ps
```

---

## 專案結構

```
 Project Root
├──  API/              # Web API 層
├──  Application/      # 應用服務層 (MediatR)
├──  ExternalApi/      # 外部 API 整合
├──  Contract/         # 服務與基礎設施註冊層
├──   Db/              # 資料庫層 (Entity)
├──  Infrastructure/   # 基礎設施層
│   └──  Data/         # 資料存取 (讀寫分離)
└──   Share/           # 共用方法及服務層
```

---

## 健康檢查

| 檢查項目 | 端點 | 說明 |
|----------|------|------|
|  API 健康檢查 | `GET /health` | 服務整體狀態 |
|  資料庫連線檢查 | `GET /health/database` | 資料庫連線狀態 |

---

##  待實作功能

###  安全性強化
-  設定檔加密: 使用 Data Protection API 加密敏感設定

### 多語系支援
-  IStringLocalizer: 實作資源檔案多語系架構

### 單元測試

---

## 技術說明

### Mediator Pattern 實作
使用 MediatR 套件實現命令查詢責任分離：

```csharp
public class SyncDataCommand : IRequest<SyncResult>
{
    public bool ForceSync { get; set; }
    public int BatchSize { get; set; }
}

public class SyncDataHandler : IRequestHandler<SyncDataCommand, SyncResult>
{
    // 實作業務邏輯
}
```

### 讀寫分離架構
```csharp
services.AddDbContext<WriteDbContext>(options => 
    options.UseSqlServer(writeConnectionString));
    
services.AddDbContext<ReadDbContext>(options => 
    options.UseSqlServer(readConnectionString));
```

---

## 專案特色

 高效能設計: 讀寫分離與 CQRS 架構  
 可擴展性: 使用 Mediator 模式確保架構彈性  
 容器化部署: 一鍵 Docker 部署，環境一致性保證  
