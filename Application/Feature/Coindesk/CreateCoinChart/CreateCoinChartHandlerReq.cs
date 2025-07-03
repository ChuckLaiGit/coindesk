using Application.Model;
using MediatR;

namespace Application.Feature.Coindesk.CreateCoinChart
{
    public class CreateCoinChartHandlerReq : IRequest<Response>
    {
        public CoinChartModel Data {  get; set; }
    }
}
