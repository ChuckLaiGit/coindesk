using Application.Model;
using MediatR;


namespace CoindeskApi.Application.Feature.Coindesk.DeleteCoinChart
{
    public class DeleteCoinChartHandlerReq : IRequest<Response>
    {
        public string ChartId { get; set; }
    }
}
