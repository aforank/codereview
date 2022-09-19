using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ContosoCrafts.Web.Server.Hubs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ContosoCrafts.Web.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using ContosoCrafts.Web.Server.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

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
        /// The events hub
        /// </summary>
        private readonly IHubContext<EventsHub> eventsHub;
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
        /// Initializes a new instance of the <see cref="CheckoutController"/> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        /// <param name="eventsHub">The events hub.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="logger">The logger.</param>
        public CheckoutController(IProductService productService,
                                  IHubContext<EventsHub> eventsHub,
                                  IConfiguration configuration,
                                  IDistributedCache cache,
                                  ILogger<CheckoutController> logger)
        {
            this.cache = cache;
            this.productService = productService;
            this.logger = logger;
            this.eventsHub = eventsHub;
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
            var host = $"{Request.Scheme}://{Request.Host.ToString()}";
            var server = sp.GetRequiredService<IServer>();
            var callbackRoot = server.Features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();

            var checkoutResponse = await productService.CheckOut(items, callbackRoot);
            var pubKey = configuration["Stripe:PubKey"];

            try
            {
                await eventsHub.Clients.All.SendAsync("CheckoutSessionStarted", pubKey, checkoutResponse);
            }
            catch
            {
                return this.Ok("Exception occurred in Order request.");
            }

            return Ok();
        }

        /// <summary>
        /// Checkouts the success.
        /// </summary>
        /// <param name="session_id">The session identifier.</param>
        /// <returns></returns>
        [HttpGet("session")]
        public async Task<ActionResult> CheckoutSuccess(string session_id)
        {
            try
            {
                //var sessionService = new SessionService();
                //Session session = await sessionService.GetAsync(session_id);

                var checkoutInfo = new CheckoutInfo
                {
                    AmountTotal = 0, //session.AmountTotal.Value,
                    CustomerEmail = "Unknown" // session.CustomerDetails.Email
                };

                var checkoutStr = JsonSerializer.Serialize<CheckoutInfo>(checkoutInfo);
                await cache.SetStringAsync("checkout/info", checkoutStr);
                return Redirect("/checkout/success");
            }
            catch
            {
                return this.BadRequest("Exception occurred in checkout.");
            }
        }

        /// <summary>
        /// Gets the checkout information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<ActionResult> GetCheckoutInfo()
        {
            var checkoutInfo = new CheckoutInfo();

            try
            {
                var checkoutStr = await cache.GetAsync("checkout/info");
                checkoutInfo = JsonSerializer.Deserialize<CheckoutInfo>(checkoutStr);
            }
            catch (Exception ex)
            {
                return this.BadRequest("Exception occurred in checkout.");
            }

            return Ok(checkoutInfo);
        }
    }
}
