using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ContosoCrafts.Web.Server.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Stripe.Checkout;
using Stripe.BillingPortal;
using Session = Stripe.Checkout.Session;
using SessionService = Stripe.Checkout.SessionService;

namespace ContosoCrafts.Web.Server.Controllers
{
    /// <summary>
    /// Checkout Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/checkout")]
    public class CheckoutController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CheckoutController> logger;
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration configuration;
        /// <summary>
        /// The product service
        /// </summary>
        private readonly IProductService productService;
        /// <summary>
        /// The cache
        /// </summary>
        private readonly IDistributedCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutController" /> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="logger">The logger.</param>
        public CheckoutController(IProductService productService,
                                  IConfiguration configuration,
                                  IDistributedCache cache,
                                  ILogger<CheckoutController> logger)
        {
            this.cache = cache;
            this.productService = productService;
            this.logger = logger;
            this.configuration = configuration;
        }

        /// <summary>
        /// Checkouts the order.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="sp">The sp.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CheckoutOrder([FromBody] IEnumerable<CartItem> items, [FromServices] IServiceProvider sp)
        {
            logger.LogInformation("Order received...");

            // Build the URL to which the customer will be redirected after paying.
            var server = sp.GetRequiredService<IServer>();
            var callbackRoot = server.Features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();

            try
            {
                var checkoutResponse = await productService.CheckOut(items, callbackRoot);
                var pubKey = configuration["Stripe:PubKey"];

                var checkoutOrderResponse = new CheckoutOrderResponse()
                {
                    SessionId = checkoutResponse.CheckoutSessionID,
                    PubKey = pubKey
                };
                
                return Ok(checkoutOrderResponse);
            }
            catch
            {
                return this.Ok("Exception occurred in Order request.");
            }
        }
    }
}
