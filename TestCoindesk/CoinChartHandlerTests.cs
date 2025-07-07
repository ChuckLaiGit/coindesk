using Application.Feature.Coindesk.CreateCoinChart;
using Application.Model;
using Azure.Core;
using CoindeskApi.Application.Feature.Coindesk.DeleteCoinChart;
using CoindeskApi.Application.Feature.Coindesk.EditCoinChart;
using CoindeskApi.Application.Feature.Coindesk.GetCoinChart;
using CoindeskApi.Application.Feature.Coindesk.SyncCoinDesk;
using Contract.DAOModel;
using Contract.Repository;
using Contract.Service;
using Db.Entity;
using Moq;
using System.Reflection.Metadata;

namespace TestCoindesk
{
    public class CoinChartHandlerTests
    {
        private readonly Mock<IChartRepository> _mockChartRepository;
        private readonly Mock<IBPIRepository> _mockBPIRepository;
        private readonly Mock<ISyncCoinDeskService> _syncCoinDeskService;
        private readonly GetCoinChartHandlerHandler _getCoinChartHandlerHandler;
        private readonly SyncCoinDeskHandler _syncCoinDeskHandler;
        private readonly CreateCoinChartHandlerHandler _createCoinChartHandlerHandler;
        private readonly EditCoinChartHandlerHandler _editCoinChartHandlerHandler;
        private readonly DeleteCoinChartHandlerHandler _deleteCoinChartHandlerHandlerr;

        public CoinChartHandlerTests()
        {
            _mockChartRepository = new Mock<IChartRepository>();
            _mockBPIRepository = new Mock<IBPIRepository>();
            _syncCoinDeskService = new Mock<ISyncCoinDeskService>();
            _getCoinChartHandlerHandler = new GetCoinChartHandlerHandler(_mockChartRepository.Object, _mockBPIRepository.Object);
            _syncCoinDeskHandler = new SyncCoinDeskHandler(_syncCoinDeskService.Object, _mockChartRepository.Object, _mockBPIRepository.Object);
            _createCoinChartHandlerHandler = new CreateCoinChartHandlerHandler(_mockChartRepository.Object, _mockBPIRepository.Object);
            _editCoinChartHandlerHandler = new EditCoinChartHandlerHandler(_mockChartRepository.Object, _mockBPIRepository.Object);
            _deleteCoinChartHandlerHandlerr = new DeleteCoinChartHandlerHandler(_mockChartRepository.Object, _mockBPIRepository.Object);
        }
        #region GetCoinChart
        #region Success
        [Fact]
        public async Task GetCoinChart_DataExists_ReturnsSuccessAsync()
        {
            // Arrange
            var query = new GetCoinChartHandlerReq() { ChartId = "test-chart-id" };
            var expectedChart = new ChartModel { Id = "test-chart-id", Name = "Test Chart" };
            var expectedBpiIds = new List<string> { "bpi1", "bpi2" };
            var expectedBpiDatas = new List<Contract.DAOModel.BPIModel>() 
            { 
                new Contract.DAOModel.BPIModel()
                {

                }
            };

            // 設定 Mock 行為
            _mockChartRepository.Setup(x => x.GetChartById(query.ChartId))
                .Returns(expectedChart);
            _mockChartRepository.Setup(x => x.GetBpiIdsByChartId(query.ChartId))
                .Returns(expectedBpiIds);
            _mockBPIRepository.Setup(x => x.GetBPIDatasByIds(expectedBpiIds))
                .Returns(expectedBpiDatas);

            var result = await _getCoinChartHandlerHandler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedChart.Name, result.Data.Data.Name);

            // 驗證 
            _mockChartRepository.Verify(x => x.GetChartById(query.ChartId), Times.Once);
            _mockChartRepository.Verify(x => x.GetBpiIdsByChartId(query.ChartId), Times.Once);
        }
        [Fact]
        public async Task GetCoinChart_ChartWithNoBpiData_ReturnsSuccessWithEmptyData()
        {
            // Arrange
            var query = new GetCoinChartHandlerReq() { ChartId = "test-chart-id" };
            var expectedChart = new ChartModel
            {
                Id = "test-chart-id",
                Name = "Empty Chart",
            };
            // 檢驗BPI為空者
            var expectedBpiIds = new List<string>();

            // 設定 Mock 行為
            _mockChartRepository.Setup(x => x.GetChartById(query.ChartId))
                .Returns(expectedChart);
            _mockChartRepository.Setup(x => x.GetBpiIdsByChartId(query.ChartId))
                .Returns(expectedBpiIds);
            _mockBPIRepository.Setup(x => x.GetBPIDatasByIds(expectedBpiIds))
                .Returns(new List<Contract.DAOModel.BPIModel>());
            // Act
            var result = await _getCoinChartHandlerHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedChart.Name, result.Data.Data.Name);
            Assert.Empty(result.Data.Data.BPIs);
        }
        #endregion
        #region Error 
        [Fact]
        public async Task Handle_ChartNotFound_ReturnsFailureResult()
        {
            // Arrange
            var chartId = "non-existent-chart-id";
            var request = new GetCoinChartHandlerReq { ChartId = chartId };

            _mockChartRepository.Setup(x => x.GetChartById(chartId))
                               .Returns((ChartModel)null);

            // Act
            var result = await _getCoinChartHandlerHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("找不到資料", result.Message);
            Assert.Null(result.Data);

            // Verify only GetChartById was called
            _mockChartRepository.Verify(x => x.GetChartById(chartId), Times.Once);
            _mockChartRepository.Verify(x => x.GetBpiIdsByChartId(It.IsAny<string>()), Times.Never);
            _mockBPIRepository.Verify(x => x.GetBPIDatasByIds(It.IsAny<List<string>>()), Times.Never);
        }

        #endregion
        #endregion
    }
}