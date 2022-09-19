using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using ContosoCrafts.Web.Shared.Models;
using EventAggregator.Blazor;
using Microsoft.AspNetCore.Components;

namespace ContosoCrafts.Web.Client.Shared
{
    /// <summary>
    /// Product List Base class.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public class ProductListBase : ComponentBase
    {
        /// <summary>
        /// Gets or sets the event aggregator.
        /// </summary>
        /// <value>
        /// The event aggregator.
        /// </value>
        [Inject]
        private IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Gets or sets the local storage.
        /// </summary>
        /// <value>
        /// The local storage.
        /// </value>
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        /// <summary>
        /// Gets or sets the client factory.
        /// </summary>
        /// <value>
        /// The client factory.
        /// </value>
        [Inject]
        private IHttpClientFactory ClientFactory { get; set; }

        /// <summary>
        /// The products
        /// </summary>
        protected IEnumerable<Product> products;
        /// <summary>
        /// The selected product
        /// </summary>
        protected Product selectedProduct;

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {

            if (products == null)
            {
                var client = ClientFactory.CreateClient("localapi");
                products = await client.GetFromJsonAsync<IEnumerable<Product>>("/api/products");
            }

            var state = await LocalStorage.GetItemAsync<Dictionary<string, CartItem>>("state.cart") ?? new();
            if (state.Any())
            {
                await EventAggregator.PublishAsync(new ShoppingCartUpdated { ItemCount = state.Keys.Count });
            }

        }

        /// <summary>
        /// Selects the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        protected void SelectProduct(string productId)
        {
            selectedProduct = products.Where(p => p.Id == productId).SingleOrDefault();
        }

        /// <summary>
        /// Submits the rating.
        /// </summary>
        /// <param name="rating">The rating.</param>
        public async Task SubmitRating(int rating)
        {
            var client = ClientFactory.CreateClient("localapi");
            var ratings = selectedProduct.Ratings;

            // resize ratings array
            Array.Resize(ref ratings, ratings.Length + 1);
            ratings[^1] = rating;
            selectedProduct.Ratings = ratings;

            await client.PutAsJsonAsync($"/api/products/{selectedProduct.Id}", new { rating = rating });
        }

        /// <summary>
        /// Adds to cart.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="title">The title.</param>
        protected async Task AddToCart(string productId, string title)
        {
            // get state
            var state = await LocalStorage.GetItemAsync<Dictionary<string, CartItem>>("state.cart") ?? new();
            if (state.ContainsKey(productId))
            {
                // Product already in cart
                CartItem selectedItem = state[productId];
                selectedItem.Quantity++;
                state[productId] = selectedItem;
            }
            else
            {
                // Add product to cart
                state[productId] = new CartItem { Id = productId, Title = title, Quantity = 1 };
            }

            // persist state in dapr
            await LocalStorage.SetItemAsync("state.cart", state);
            await EventAggregator.PublishAsync(new ShoppingCartUpdated { ItemCount = state.Keys.Count });
        }
    }
}
