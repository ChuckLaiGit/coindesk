using Db.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Entity
{
    public class ChartBpiMap:BaseEntity
    {
        public required string ChartId {  get; set; }
        public required string BpiId { get; set; }
    }
}
