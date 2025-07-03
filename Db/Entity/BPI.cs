using Db.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Entity
{
    public class BPI : BaseEntity
    {
        public required string Name {  get; set; }
        public required string Code { get; set; }
        public required string Symbol {  get; set; }
        public required string Rate {  get; set; }
        public string? Description { get; set;}
        public required decimal RateFload {  get; set; }
    }
}
