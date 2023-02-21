using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System.Net;

namespace Ordering.KafkaAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
         
        private readonly IMediator _mediator;

        public KafkaController(IMediator mediator)
        {
            _mediator = mediator;
        }
         
        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(OrdersVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrderByUserName(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
         
    }
}
