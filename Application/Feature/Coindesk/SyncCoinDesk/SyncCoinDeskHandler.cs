
using Application.Model;
using Contract.DAOModel;
using Contract.Repository;
using Contract.Service;
using MediatR;

namespace CoindeskApi.Application.Feature.Coindesk.SyncCoinDesk
{
    public class SyncCoinDeskHandler : IRequestHandler<SyncCoinDeskReq, Response>
    {
        private readonly ISyncCoinDeskService _syncCoinDeskService;
        private readonly IChartRepository _chartRepository;
        private readonly IBPIRepository _bpiRepository;
        public SyncCoinDeskHandler(ISyncCoinDeskService syncCoinDeskService, IChartRepository chartRepository, IBPIRepository bPIRepository)
        {
            _syncCoinDeskService = syncCoinDeskService;
            _chartRepository = chartRepository;
            _bpiRepository = bPIRepository;
        }

        public async Task<Response> Handle(SyncCoinDeskReq request, CancellationToken cancellationToken)
        {
            var result = _syncCoinDeskService.SendAndGetCoinDeskResult();
            var bpiList = GetBpiDaoModel(result.Bpi);
            var thisChartId = _chartRepository.GetChartIdByName(result.ChartName);
            if (string.IsNullOrEmpty(thisChartId))
            {
                thisChartId = _chartRepository.CreateChart(result.ChartName);
                _bpiRepository.CreateBpisFlow(thisChartId, bpiList);
            }
            else
            {
                _chartRepository.EditChart(new ChartModel()
                {
                    Id = thisChartId,
                    Name = result.ChartName,
                });
                var bpiIds = _bpiRepository.UpdateBpisByBpiModels(bpiList, thisChartId, true);
                // 編輯圖表與BPI關聯
                _chartRepository.UpdateChartBpiMaps(thisChartId, bpiIds);

            }
            return new Response(200, true);
        }
        protected List<Contract.DAOModel.BPIModel> GetBpiDaoModel(CoinDeskBpi bpi)
        {
            var retDatas = new List<Contract.DAOModel.BPIModel>();
            retDatas.Add(new Contract.DAOModel.BPIModel()
            {
                Code = bpi.GBP.Code,
                Description = bpi.GBP.Description,
                Name = "GBP",
                Rate = bpi.GBP.Rate,
                RateFload = bpi.GBP.RateFloat,
                Symbol = bpi.GBP.Symbol
            });
            retDatas.Add(new Contract.DAOModel.BPIModel()
            {
                Code = bpi.EUR.Code,
                Description = bpi.EUR.Description,
                Name = "EUR",
                Rate = bpi.EUR.Rate,
                RateFload = bpi.EUR.RateFloat,
                Symbol = bpi.EUR.Symbol
            });

            retDatas.Add(new Contract.DAOModel.BPIModel()
            {
                Code = bpi.USD.Code,
                Description = bpi.USD.Description,
                Name = "USD",
                Rate = bpi.USD.Rate,
                RateFload = bpi.USD.RateFloat,
                Symbol = bpi.USD.Symbol
            });

            return retDatas;
        }
    }
}
