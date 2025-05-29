using Microsoft.AspNetCore.Mvc;
using MyApi.Model.Product;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly IProductService _productService;

        public ShoppingCartController([FromServices] ICouponService couponService, [FromServices] IProductService productService)
        {
            _productService = productService; // considerar a existência de um método chamado UpdateAsync para atualizar produto
            _couponService = couponService; // considerar a existência de um método chamado validateAsync para validar cupons
        }


        [HttpPost("checkout")]
        public async Task<IActionResult> ProcessCheckout([FromBody] CheckoutRequest request)
        {
            decimal total = 0;
            
            foreach (var item in request.Items)
            {
                var product = await _productService.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    return BadRequest($"Produto {item.ProductId} não encontrado");
                }
            }
            
            var order = new Order()
            {
                Items = request.Items,
                Total = total,
                Status = OrderStatus.Pending
            };
            
            await _orderService.CreateAsync(order);

            return Ok(new { OrderId = order.Id, Total = total });
        }
    }   
}

