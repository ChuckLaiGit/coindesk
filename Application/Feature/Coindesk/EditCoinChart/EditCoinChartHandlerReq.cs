using Application.Model;
using MediatR;


namespace CoindeskApi.Application.Feature.Coindesk.EditCoinChart
{
    public class EditCoinChartHandlerReq : IRequest<Response>
    {
        public CoinChartModel Data { get; set; }
    }
}
