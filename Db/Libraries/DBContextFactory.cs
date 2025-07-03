using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Db.Libraries
{
    public class DBContextFactory<TContext> where TContext : ReadWriteEFContext, new()
    {
        /// <summary>
        /// Write資料庫
        /// </summary>
        private readonly TContext _writeDB;

        /// <summary>
        /// Read資料庫
        /// </summary>
        private readonly TContext _readDB;

        public DBContextFactory(IConfiguration configuration)
        {
            // 建立讀寫資料庫
            string? readDBConfig = configuration.GetConnectionString("ReadDB");
            string? writeDBConfig = configuration.GetConnectionString("WriteDB");
            _readDB = CreateContext(readDBConfig);
            _writeDB = CreateContext(writeDBConfig);
        }

        /// <summary>
        /// 回傳Read資料庫物件
        /// </summary>
        /// <returns></returns>
        public TContext GetReadDB()
        {
            return _readDB;
        }

        /// <summary>
        /// 回傳Write資料庫物件
        /// </summary>
        /// <returns></returns>
        public TContext GetWriteDB()
        {
            return _writeDB;
        }

        /// <summary>
        /// 產生資料庫實體
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <returns></returns>
        private TContext CreateContext(string connectionString)
        {
            TContext context = new();
            context.Database.SetConnectionString(connectionString);
            return context;
        }
    }
}
