using Application.Model;
using Contract.Repository;
using MediatR;
using Share.Utiity;
using System.Reflection;

namespace CoindeskApi.Application.Feature.Coindesk.GetCoinChart
{
    public class GetCoinChartHandlerHandler : IRequestHandler<GetCoinChartHandlerReq, ResponseData<GetCoinChartHandlerRes>>
    {
        private readonly IChartRepository _chartRepository;
        private readonly IBPIRepository _bpiRepository;
        public GetCoinChartHandlerHandler(IChartRepository chartRepository, IBPIRepository bPIRepository)
        {
            _chartRepository = chartRepository;
            _bpiRepository = bPIRepository;
        }

        public async Task<ResponseData<GetCoinChartHandlerRes>> Handle(GetCoinChartHandlerReq request, CancellationToken cancellationToken)
        {
            var retData = new CoinChartModel();
            var chart = _chartRepository.GetChartById(request.ChartId);
            var thisBipIds = _chartRepository.GetBpiIdsByChartId(request.ChartId);
            var bpiDAO = _bpiRepository.GetBPIDatasByIds(thisBipIds);
            var bpiDTO = BPIDaoModelToDTO(bpiDAO);
            retData.Name = chart.Name;
            retData.BPIs = bpiDTO;
            retData.Id = chart.Id;
            return await Task.FromResult(new ResponseData<GetCoinChartHandlerRes>(new GetCoinChartHandlerRes()
            {
                Data = retData,
            }));
        }

        protected List<BPIModel> BPIDaoModelToDTO(List<Contract.DAOModel.BPIModel> models)
        {
            var retData = new List<BPIModel>();
            models.ForEach(x =>
            {
                var addItem = Mapper.Map<Contract.DAOModel.BPIModel, BPIModel>(x);
                retData.Add(addItem);
            });
            return retData;
        }

    }
}
