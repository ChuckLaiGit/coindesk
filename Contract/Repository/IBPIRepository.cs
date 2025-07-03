using Contract.DAOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Repository
{
    public interface IBPIRepository
    {
        public List<BPIModel> GetBPIDatasByIds(List<string> ids);
        public void CreateBpisFlow(string chartId, List<BPIModel> models);
        public List<string> UpdateBpisByBpiModels(List<BPIModel> models, string? chartId = null, bool isSync = false);
        public void DeleteBpisByIds(List<string> ids);

    }
}
