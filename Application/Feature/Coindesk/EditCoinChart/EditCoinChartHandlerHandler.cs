using Application.Model;
using Contract.Repository;
using MediatR;
using Share.Utiity;
using System.Reflection;


namespace CoindeskApi.Application.Feature.Coindesk.EditCoinChart
{
    public class EditCoinChartHandlerHandler : IRequestHandler<EditCoinChartHandlerReq, Response>
    {
        private readonly IChartRepository _chartRepository;
        private readonly IBPIRepository _bpiRepository;

        public EditCoinChartHandlerHandler(IChartRepository chartRepository, IBPIRepository bPIRepository)
        {
            _chartRepository = chartRepository;
            _bpiRepository = bPIRepository;
        }


        public async Task<Response> Handle(EditCoinChartHandlerReq request, CancellationToken cancellationToken)
        {
            // 編輯圖表
            _chartRepository.EditChart(new Contract.DAOModel.ChartModel() { Id = request.Data.Id, Name = request.Data.Name });
            // 編輯圖表與BPI關聯
            _chartRepository.UpdateChartBpiMaps(request.Data.Id, request.Data.BPIs.Select(x => x.Id).ToList());
            var bpis = BPIDTOToBPIEntity(request.Data.BPIs);
            // 編輯BPI
            _bpiRepository.UpdateBpisByBpiModels(bpis);
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
