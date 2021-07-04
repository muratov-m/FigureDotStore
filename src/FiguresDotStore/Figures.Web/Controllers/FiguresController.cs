using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Figures.Core.Domain;
using Figures.Core.Logic;
using Figures.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Figures.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FiguresController : ControllerBase
    {
        private readonly ILogger<FiguresController> _logger;
        private readonly IOrderService _orderService;

        public FiguresController(ILogger<FiguresController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// Оформить заказ.
        /// </summary>
        /// <param name="cart">Корзина.</param>
        /// <returns>Стоимость заказа.</returns>
        [HttpPost]
        [Route(nameof(Order))]
        public async Task<ActionResult> Order(Cart cart)
        {
            try
            {
                var order = new Order
                {
                    Positions = cart.Positions
                        .Select(x =>
                            new OrderPosition(
                                FiguresFactory.Create(x.Type, x.SideA, x.SideB, x.SideC),
                                x.Count))
                        .ToList()
                };

                var result = await _orderService.MakeOrder(order);

                return new OkObjectResult(result);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "Request to create an order is incorrect.");
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error occurred while creating the order.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
