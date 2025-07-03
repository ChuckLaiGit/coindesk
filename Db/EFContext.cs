using Db.Entity;
using Db.Libraries;
using Microsoft.EntityFrameworkCore;

namespace Db
{
    public class EFContext:ReadWriteEFContext
    {
        public virtual DbSet<BPI> BPI { get; set; }
        public virtual DbSet<Chart> Chart { get; set; }
        public virtual DbSet<ChartBpiMap> ChartBpiMap { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // 將 Primary Key 設為非叢集
                entity.FindPrimaryKey()?.SetIsClustered(false);

                foreach (var prop in entity.GetProperties())
                {
                    switch (prop.Name)
                    {
                        case "Id":
                            prop.SetDefaultValueSql("NEWID()");
                            break;
                        case "CreateTime":
                        case "UpdateTime":
                            prop.SetDefaultValueSql("GETDATE()");
                            break;
                        case "IsDeleted":
                            prop.SetDefaultValueSql("0");
                            break;
                    }
                }
            }
        }
    }
}
