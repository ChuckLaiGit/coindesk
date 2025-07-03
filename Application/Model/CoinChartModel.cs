using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class CoinChartModel
    {
        public string Id {  get; set; }
        public string Name { get; set; }
        public List<BPIModel> BPIs { get; set; }
    }
    public class BPIModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Rate {  get; set; }
        public Decimal RateFloat {  get; set; }
        public string Code { get; set; }

    }

}
