using Application.Feature.Coindesk.CreateCoinChart;
using CoindeskApi.Application.Feature.Coindesk.DeleteCoinChart;
using CoindeskApi.Application.Feature.Coindesk.EditCoinChart;
using CoindeskApi.Application.Feature.Coindesk.GetCoinChart;
using CoindeskApi.Application.Feature.Coindesk.SyncCoinDesk;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace 國泰.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoinDeskController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<CoinDeskController> _logger;

        public CoinDeskController(ILogger<CoinDeskController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpPost("chart/get")]
        public async Task<IActionResult> GetCoinChart(GetCoinChartHandlerReq req)
        {
            var res = await _mediator.Send(req);
            return Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChartFlow(CreateCoinChartHandlerReq req)
        {
            var res = await _mediator.Send(req);
            return Ok(res);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditChartFlow(EditCoinChartHandlerReq req)
        {
            var res = await _mediator.Send(req);
            return Ok(res);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteChartFlow(DeleteCoinChartHandlerReq req)
        {
            var res = await _mediator.Send(req);
            return Ok(res);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncCoinDesk(SyncCoinDeskReq req)
        {
            var res = await _mediator.Send(req);
            return Ok(res);
        }

    }
}
