using Application.Model;
using Contract.Repository;
using MediatR;


namespace CoindeskApi.Application.Feature.Coindesk.DeleteCoinChart
{
    public class DeleteCoinChartHandlerHandler : IRequestHandler<DeleteCoinChartHandlerReq, Response>
    {
        private readonly IChartRepository _chartRepository;
        private readonly IBPIRepository _bpiRepository;


        public DeleteCoinChartHandlerHandler(IChartRepository chartRepository, IBPIRepository bpiRepository)
        {
            _chartRepository = chartRepository;
            _bpiRepository = bpiRepository; 
        }

        public async Task<Response> Handle(DeleteCoinChartHandlerReq request, CancellationToken cancellationToken)
        {
            var bpiIds = _chartRepository.DeleteChartById(request.ChartId);
            _bpiRepository.DeleteBpisByIds(bpiIds);
            return new Response(200, true);
        }
    }
}
