# coindesk

本專案負責從外部端點獲取資料並同步更新至本地資料庫。

設計模式
  -Mediator Pattern: 使用 MediatR 實現 CQRS 架構，將業務邏輯與控制器解耦
  -讀寫分離: 資料庫層採用 CQRS 模式，優化讀取與寫入效能
  -Repository Pattern: 資料存取層抽象化，提升可測試性

快速開始 : 
  前置需求
  .NET 8.0 SDK
  Docker & Docker Compose
  
  安裝步驟
  1.複製專案
  bashgit clone [repository-url]
  cd [project-name]
  
  2.Docker 部署
  bashdocker-compose up -d
  
  3.檢查服務狀態
  bashdocker-compose ps

專案結構

── API/                 # Web API 層
── Application/         # 應用服務層 (MediatR)
   ├── ExternalApi/     # 外部 API 整合
── Contract/            # 服務與基礎設施註冊層 
── Db/                  # 資料庫層 (Entity)
── Infrastructure/      # 基礎設施層
   ├── Data/            # 資料存取 (讀寫分離)
── Share/               # 共用方法及服務層

健康檢查
API 健康檢查: GET /health
資料庫連線檢查: GET /health/database

待實作功能:
安全性強化
設定檔加密: 使用 Data Protection API 加密敏感設定
API 金鑰管理: 整合 Azure Key Vault 或 HashiCorp Vault
JWT 驗證: 實作 Bearer Token 認證機制

多語系支援
IStringLocalizer: 實作資源檔案多語系架構
文化特定格式: 數字、日期、貨幣格式本地化
動態語言切換: 支援執行時語言變更

效能優化
Redis 快取: 熱點資料快取機制
非同步處理: 背景任務與訊息佇列整合
API 限流: 實作 Rate Limiting 保護機制

技術說明:
Mediator Pattern 實作 使用 MediatR 套件實現命令查詢責任分離
public class SyncDataCommand : IRequest<SyncResult>
{
    public bool ForceSync { get; set; }
    public int BatchSize { get; set; }
}

public class SyncDataHandler : IRequestHandler<SyncDataCommand, SyncResult>
{
    // 實作業務邏輯
}

讀寫分離架構
services.AddDbContext<WriteDbContext>(options => 
    options.UseSqlServer(writeConnectionString));
    
services.AddDbContext<ReadDbContext>(options => 
    options.UseSqlServer(readConnectionString));
