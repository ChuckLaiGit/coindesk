using Db.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Entity
{
    public class Chart : BaseEntity
    {
        public required string Name {  get; set; }
    }
}
