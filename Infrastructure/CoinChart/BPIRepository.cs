using Contract.DAOModel;
using Contract.Repository;
using Db;
using Db.Entity;
using Db.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CoinChart
{
    public class BPIRepository : IBPIRepository
    {
        private readonly DBContextFactory<EFContext> _dbContext;
        public BPIRepository(DBContextFactory<EFContext> dbContext)
        {
            _dbContext = dbContext;
        }

        #region 查詢
        /// <summary>
        /// 依照ID列表取得BPI列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<BPIModel> GetBPIDatasByIds(List<string> ids)
        {
            var readDb = _dbContext.GetReadDB();
            return readDb.GetAll<BPI>(true).Where(x => ids.Contains(x.Id) && !x.IsDeleted)
                .Select(x => new BPIModel()
                {
                    Id  = x.Id,
                    Code = x.Code,
                    Description = x.Description,
                    Name = x.Name,
                    Rate = x.Rate,
                    RateFload = x.RateFload,
                    Symbol = x.Symbol,
                }).OrderBy(x=>x.Symbol).ToList();
        }
        #endregion

        #region 新增
        /// <summary>
        /// 建立BPI及與圖表關聯
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateBpisFlow(string chartId,List<BPIModel> models)
        {
            var writeDb = _dbContext.GetWriteDB();
            using (var trans = writeDb.Database.BeginTransaction())
            {
                try
                {
                    foreach (var model in models)
                    {
                        string bpiId = Guid.NewGuid().ToString();
                        // 建立BPI
                        writeDb.Create(new BPI()
                        {
                            Code = model.Code,
                            Id = bpiId,
                            Description = model.Description,
                            Name = model.Name,
                            Rate = model.Rate,
                            RateFload = model.RateFload,
                            Symbol = model.Symbol,
                        });
                        // 建立圖表與BPI對應
                        writeDb.Create(new ChartBpiMap()
                        {
                            BpiId = bpiId,
                            ChartId = chartId,
                            Id = Guid.NewGuid().ToString()
                        });
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        #endregion
        #region 編輯
        /// <summary>
        /// 編輯BPI
        /// </summary>
        /// <param name="models"></param>
        public List<string> UpdateBpisByBpiModels(List<BPIModel> models, string? chartId = null, bool isSync = false)
        {
            var db = _dbContext.GetWriteDB();
            List<BPI> bpis;
            if (!isSync)
            {
                bpis = db.GetAll<BPI>().Where(x => models.Select(y => y.Id).Contains(x.Id) && !x.IsDeleted).ToList();
            }
            else
            {
                bpis = (from m in db.ChartBpiMap
                        join bpi in db.BPI on m.BpiId equals bpi.Id
                        where m.ChartId == chartId && models.Select(x => x.Name).Contains(bpi.Name) && !m.IsDeleted && !bpi.IsDeleted
                        select bpi).ToList();
            }
            foreach ( var bp in bpis)
            {
                var thisUpdateData = !isSync ? models.Find(x => x.Id == bp.Id) : models.Find(x=>x.Name == bp.Name);
                if (thisUpdateData == null) continue;
                bp.Code = thisUpdateData.Code;
                bp.Description = thisUpdateData.Description;
                bp.Name = thisUpdateData.Name;
                bp.Rate = thisUpdateData.Rate;
                bp.RateFload = thisUpdateData.RateFload;
                bp.Symbol = thisUpdateData.Symbol;
            }
            db.SaveChanges();
            return bpis.Select(x => x.Id).ToList();
        }
        #endregion
        #region 刪除
        public void DeleteBpisByIds(List<string> ids)
        {
            var db = _dbContext.GetWriteDB();
            ids.ForEach(x => db.SoftDelete<BPI>(x));
        }
        #endregion
    }
}
