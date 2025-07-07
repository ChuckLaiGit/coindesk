using Application.UnitTest;
using CoindeskApi.Application.Feature.Coindesk.GetCoinChart;
using Contract.Repository;
using Infrastructure.CoinChart;
using Moq;

namespace TestCoindesk
{
    public class GetCoinChartHandlerTests
    {
        private readonly IChartRepository _ChartRepository;
        private readonly Mock<IChartRepository> _mockChartRepository;
        private readonly IBPIRepository _BPIRepository;
        private readonly GetCoinChartHandlerHandler _getCoinChartHandlerHandler;

        public GetCoinChartHandlerTests()
        {
            var db = Utility.GetTestDb();
            _BPIRepository = new BPIRepository(db);
            _ChartRepository = new ChartRepository(db);
            _mockChartRepository = new Mock<IChartRepository>();
            _getCoinChartHandlerHandler = new GetCoinChartHandlerHandler(_ChartRepository, _BPIRepository);
        }

        [Fact]
        public async Task Handle_DataExists_ReturnsDataAsync()
        {
            var query = new GetCoinChartHandlerReq() { ChartId = "" };
            var expectedData = new GetCoinChartHandlerRes()
            {

            };
            var result = await _getCoinChartHandlerHandler.Handle(query, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedData.Data.Name, result.Data.Data.Name);

            // 驗證只調用了本地資料庫，沒有調用外部 API
            _mockChartRepository.Verify(x => x.GetChartById(query.ChartId), Times.Once);
            _mockChartRepository.Verify(x => x.GetBpiIdsByChartId(query.ChartId), Times.Once);
        }
    }
}