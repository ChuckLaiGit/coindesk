using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Db.Libraries
{
    public class ReadWriteEFContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        #region 查詢
        /// <summary>
        /// 查詢全資料表
        /// </summary>
        /// <returns></returns>
        public IQueryable<TTable> GetAll<TTable>(bool asNoTracking = false) where TTable : BaseEntity
        {
            if (asNoTracking)
            {
                return this.Set<TTable>().AsNoTracking();
            }
            return this.Set<TTable>();
        }

        /// <summary>
        /// 查詢全資料表並使用Where條件篩選
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public IQueryable<TTable> FindMultiByCondition<TTable>(Expression<Func<TTable, bool>> match, bool asNoTracking = false) where TTable : BaseEntity
        {
            if (asNoTracking)
            {
                return this.Set<TTable>().AsNoTracking().Where(match);
            }
            return this.Set<TTable>().Where(match);
        }

        /// <summary>
        /// 查詢全資料表並使用Where條件篩選
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public TTable? FindOne<TTable>(Expression<Func<TTable, bool>> match, bool asNoTracking = false) where TTable : BaseEntity
        {
            if (asNoTracking)
            {
                return this.Set<TTable>().AsNoTracking().Where(match).FirstOrDefault();
            }
            return this.Set<TTable>().Where(match).FirstOrDefault();
        }
        #endregion

        #region 新增
        /// <summary>
        /// 單筆新增
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TTable Create<TTable>(TTable instance) where TTable : BaseEntity
        {
            try
            {
                if (instance == null)
                {
                    throw new ArgumentNullException(nameof(instance));
                }
                else
                {
                    this.Set<TTable>().Add(instance);
                    this.SaveChanges();
                    return instance;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 多筆新增
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<TTable> CreateRange<TTable>(List<TTable> instances) where TTable : BaseEntity
        {
            try
            {
                if (instances == null)
                {
                    throw new ArgumentNullException(nameof(instances));
                }
                else
                {
                    foreach (var item in instances)
                    {
                        this.Set<TTable>().Add(item);
                    }
                    this.SaveChanges();
                    return instances;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
        }
        #endregion
        #region 編輯
        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public new TTable Update<TTable>(TTable instance) where TTable : BaseEntity
        {
            try
            {
                if (instance == null)
                {
                    throw new ArgumentNullException(nameof(instance));
                }
                else
                {
                    if(this.FindOne<TTable>(x=>x.Id == instance.Id) == null) throw new ArgumentNullException(nameof(instance.Id));
                    instance.UpdateTime = DateTime.Now;
                    this.Set<TTable>().Update(instance).CurrentValues.SetValues(instance);
                    this.SaveChanges();
                    return instance;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
        }
        #endregion
        #region 刪除
        /// <summary>
        /// 軟刪除資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool SoftDelete<TTable>(string id) where TTable : BaseEntity
        {
            try
            {
                TTable? existing = this.FindOne<TTable>(x=> x.Id == id) ?? throw new ArgumentNullException(nameof(id));
                existing.UpdateTime = DateTime.Now;
                existing.IsDeleted = true;
                this.Set<TTable>().Update(existing);
                this.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
        }
        #endregion
    }
}
