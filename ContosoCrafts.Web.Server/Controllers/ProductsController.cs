using System.Threading.Tasks;
using ContosoCrafts.Web.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace ContosoCrafts.Web.Server.Controllers
{
    /// <summary>
    /// Products Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// The product service
        /// </summary>
        private readonly IProductService productService;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ProductsController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController" /> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public ProductsController(IProductService productService, IConfiguration configuration, ILogger<ProductsController> logger)
        {
            this.logger = logger;
            this.productService = productService;
        }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        [HttpGet("products")]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                await productService.GetProducts();
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the single.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("products/{id}")]
        public async Task<ActionResult> GetSingle(string id)
        {
            try
            {
                var result = productService.GetProduct(id);

                if (result == null)
                    return null;

                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("products")]
        public async Task<ActionResult> RateProduct([FromBody] RatingRequest request)
        {
            try
            {
                await productService.AddRating(request.ProductId, request.Rating);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
