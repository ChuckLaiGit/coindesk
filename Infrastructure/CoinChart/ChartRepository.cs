using Contract.DAOModel;
using Contract.Repository;
using Db;
using Db.Entity;
using Db.Libraries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CoinChart
{
    public class ChartRepository : IChartRepository
    {
        private readonly DBContextFactory<EFContext> _dbContext;
        public ChartRepository(DBContextFactory<EFContext> dbContext)
        {
            _dbContext = dbContext;
        }
        #region 查詢
        /// <summary>
        /// 取得圖表名稱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChartModel GetChartById(string id)
        {
            var retData = _dbContext.GetReadDB().GetAll<Chart>().Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new ChartModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefault();
            if (retData == null) throw new Exception("找不到資料");
            return retData;
        }
        public string? GetChartIdByName(string name)
        {
            return _dbContext.GetReadDB().Chart.Where(x => x.Name == name && !x.IsDeleted).Select(x => x.Id).FirstOrDefault();
        }
        /// <summary>
        /// 依照圖表Id取得BPI Ids
        /// </summary>
        /// <param name="chartId"></param>
        /// <returns></returns>
        public List<string> GetBpiIdsByChartId(string chartId)
        {
            return _dbContext.GetReadDB().GetAll<ChartBpiMap>().Where(x => x.ChartId == chartId && !x.IsDeleted)
                .Select(x => x.BpiId).ToList();
        }
        #endregion
        #region 新增
        /// <summary>
        /// 建立圖表
        /// </summary>
        /// <param name="chartName"></param>
        public string CreateChart(string chartName)
        {
            var db = _dbContext.GetWriteDB();
            var id = Guid.NewGuid().ToString();
            db.Create(new Chart()
            {
                Id = id,
                Name = chartName,
            });
            return id;
        }
        #endregion
        #region 編輯
        /// <summary>
        /// 編輯圖表
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="Exception"></exception>
        public void EditChart(ChartModel model)
        {
            var db = _dbContext.GetWriteDB();
            var thisChart = db.FindOne<Chart>(x => x.Id == model.Id);
            if (thisChart == null) throw new Exception("找不到資料");
            thisChart.Name = model.Name;
            db.Update(thisChart);
        }
        /// <summary>
        /// 更新圖表與BPI對應
        /// </summary>
        /// <param name="chartId"></param>
        /// <param name="bpiIds"></param>
        public void UpdateChartBpiMaps(string chartId ,List<string> bpiIds)
        {
            var db = _dbContext.GetWriteDB();
            var oriMaps = db.GetAll<ChartBpiMap>().Where(x => x.ChartId == chartId && !x.IsDeleted).ToList();
            // 刪除非於傳入ID資料
            var deleteIds = oriMaps.Select(x => x.Id).Except(bpiIds).ToList();
            deleteIds.ForEach(x => db.SoftDelete<ChartBpiMap>(x));
            // 新增傳入與原資料差集
            var addIds = bpiIds.Except(oriMaps.Select(x => x.Id)).ToList();
            addIds.ForEach(x => db.Create(new ChartBpiMap()
            {
                BpiId = x,
                ChartId = chartId,
                Id = Guid.NewGuid().ToString()
            }));
        }
        #endregion

        #region 刪除
        /// <summary>
        /// 刪除圖表
        /// </summary>
        /// <param name="id"></param>
        public List<string> DeleteChartById(string id)
        {
            var db = _dbContext.GetWriteDB();
            db.SoftDelete<Chart>(id);
            var bpiMaps = db.GetAll<ChartBpiMap>().Where(x => x.ChartId == id && !x.IsDeleted).ToList();
            bpiMaps.ForEach(x => x.IsDeleted = true);
            db.SaveChanges();
            return bpiMaps.Select(x => x.BpiId).ToList();
        }
        #endregion
    }
}
