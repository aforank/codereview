using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ContosoCrafts.Web.Server.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Stripe.Checkout;

namespace ContosoCrafts.Web.Server.Controllers
{
    /// <summary>
    /// Json File Product Service.
    /// </summary>
    /// <seealso cref="ContosoCrafts.Web.Server.Controllers.IProductService" />
    public class JsonFileProductService : IProductService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<JsonFileProductService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFileProductService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public JsonFileProductService(ILogger<JsonFileProductService> logger)
        {
            this.logger = logger;

            var manifestEmbeddedProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly);
            var fileInfo = manifestEmbeddedProvider.GetFileInfo("_data/products.json");
            using var reader = new StreamReader(fileInfo.CreateReadStream());


            var fileContent = reader.ReadToEnd();

            Products = JsonSerializer.Deserialize<List<Server.Models.Product>>(fileContent,
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   });
        }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        private List<Server.Models.Product> Products { get; }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Server.Models.Product>> GetProducts() => Task.FromResult(Products.AsEnumerable());

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public Task<Server.Models.Product> GetProduct(string productId)
        {
            var selectedProduct = new Product();

            foreach (var p in Products)
            {
                if (p.Id == productId)
                {
                    selectedProduct = p;
                }
            }
            
            return Task.FromResult(selectedProduct);
        }

        /// <summary>
        /// Adds the rating.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="rating">The rating.</param>
        public async Task AddRating(string productId, int rating)
        {
            var products = await GetProducts();

            if (products.First(x => x.Id == productId).Ratings == null)
            {
                products.First(x => x.Id == productId).Ratings = new int[] { rating };
            }
            else
            {
                var ratings = products.First(x => x.Id == productId).Ratings.ToList();
                ratings.Add(rating);
                products.First(x => x.Id == productId).Ratings = ratings.ToArray();
            }
        }

        /// <summary>
        /// Checks the out.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="callbackRoot">The callback root.</param>
        /// <returns></returns>
        public async Task<CheckoutResponse> CheckOut(IEnumerable<CartItem> Items, string callbackRoot)
        {
            logger.LogInformation($"Checking out from the JsonFilePRoductService...");

            // Create a payment flow from the items in the cart.
            var options = new SessionCreateOptions
            {
              SuccessUrl = $"{callbackRoot}/api/checkout/session?session_id=" + "{CHECKOUT_SESSION_ID}", /// redirect after checkout
              CancelUrl = $"{callbackRoot}/checkout/failure",  /// checkout cancelled
              PaymentMethodTypes = new List<string>
              {
                "card",
              },
              LineItems = new List<SessionLineItemOptions>
              {
                new SessionLineItemOptions
                {
                  PriceData = new()
                  {
                    UnitAmount = 1000L,
                    Currency = "USD",
                    ProductData = new()
                    {
                        Name = "Example",
                    },
                  },
                  Quantity = 2,
                },
              },
              Mode = "payment",
            };
            var service = new SessionService();
            Stripe.StripeConfiguration.ApiKey = "@vdf(+eywy8}#RFt";
            var session = await service.CreateAsync(options);
            return new CheckoutResponse(session.Id);
        }
    }
}
