using CoindeskApi.Application.Feature.Coindesk.GetCoinChart;
using Contract.DAOModel;
using Contract.Repository;
using Moq;

namespace TestCoindesk
{
    public class GetCoinChartHandlerTests
    {
        private readonly Mock<IChartRepository> _mockChartRepository;
        private readonly Mock<IBPIRepository> _mockBPIRepository;
        private readonly GetCoinChartHandlerHandler _getCoinChartHandlerHandler;

        public GetCoinChartHandlerTests()
        {
            _mockChartRepository = new Mock<IChartRepository>();
            _mockBPIRepository = new Mock<IBPIRepository>();
            _getCoinChartHandlerHandler = new GetCoinChartHandlerHandler(_mockChartRepository.Object, _mockBPIRepository.Object);
        }

        [Fact]
        public async Task Handle_DataExists_ReturnsDataAsync()
        {
            // Arrange
            var query = new GetCoinChartHandlerReq() { ChartId = "test-chart-id" };
            var expectedChart = new ChartModel { Id = "test-chart-id", Name = "Test Chart" };
            var expectedBpiIds = new List<string> { "bpi1", "bpi2" };

            // 設定 Mock 行為
            _mockChartRepository.Setup(x => x.GetChartById(query.ChartId))
                .Returns(expectedChart);
            _mockChartRepository.Setup(x => x.GetBpiIdsByChartId(query.ChartId))
                .Returns(expectedBpiIds);

            var result = await _getCoinChartHandlerHandler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedChart.Name, result.Data.Data.Name);

            // 驗證 
            _mockChartRepository.Verify(x => x.GetChartById(query.ChartId), Times.Once);
            _mockChartRepository.Verify(x => x.GetBpiIdsByChartId(query.ChartId), Times.Once);
        }
    }
}