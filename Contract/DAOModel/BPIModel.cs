using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DAOModel
{
    public class BPIModel
    {
        public string Id { get; set; }
        public string Name {  get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Rate { get; set; }
        public string? Description {  get; set; } 
        public decimal RateFload { get; set; }
    }
}
