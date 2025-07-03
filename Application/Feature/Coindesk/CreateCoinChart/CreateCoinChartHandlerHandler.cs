
using Application.Model;
using Contract.Repository;
using MediatR;
using Share.Utiity;
using System.Reflection;

namespace Application.Feature.Coindesk.CreateCoinChart
{
    public class CreateCoinChartHandlerHandler : IRequestHandler<CreateCoinChartHandlerReq, Response>
    {
        private readonly IChartRepository _chartRepository;
        private readonly IBPIRepository _bpiRepository;
        public CreateCoinChartHandlerHandler(IChartRepository chartRepository, IBPIRepository bPIRepository)
        {
            _chartRepository = chartRepository;
            _bpiRepository = bPIRepository;
        }

        public async Task<Response> Handle(CreateCoinChartHandlerReq request, CancellationToken cancellationToken)
        {
            // 建立圖表
            var chartId = _chartRepository.CreateChart(request.Data.Name);
            // DAO TO DTO
            var bpiList = BPIDTOToBPIEntity(request.Data.BPIs);
            // 建立BPI
            _bpiRepository.CreateBpisFlow(chartId, bpiList);
            return new Response(200, true);
        }
        protected List<Contract.DAOModel.BPIModel> BPIDTOToBPIEntity(List<BPIModel> models)
        {
            var retData = new List<Contract.DAOModel.BPIModel>();
            models.ForEach(x =>
            {
                var addItem = Mapper.Map<BPIModel, Contract.DAOModel.BPIModel>(x);
                retData.Add(addItem);
            });
            return retData;
        }
    }
}
