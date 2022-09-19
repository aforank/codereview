using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using ContosoCrafts.Web.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace ContosoCrafts.Web.Client.Pages
{
    /// <summary>
    /// Index Base class.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    /// <seealso cref="System.IAsyncDisposable" />
    public class IndexBase : ComponentBase, IAsyncDisposable
    {
        /// <summary>
        /// Gets or sets the client factory.
        /// </summary>
        /// <value>
        /// The client factory.
        /// </value>
        [Inject]
        private IHttpClientFactory ClientFactory { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        /// <value>
        /// The navigation manager.
        /// </value>
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the js runtime.
        /// </summary>
        /// <value>
        /// The js runtime.
        /// </value>
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        [Inject]
        private ILogger<IndexBase> logger { get; set; }

        /// <summary>
        /// Gets or sets the toast service.
        /// </summary>
        /// <value>
        /// The toast service.
        /// </value>
        [Inject]
        private IToastService toastService { get; set; }

        /// <summary>
        /// The hub connection
        /// </summary>
        private HubConnection hubConnection;
        /// <summary>
        /// The module
        /// </summary>
        private IJSObjectReference module;

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            logger.LogInformation("OnInitializedAsync called");

            hubConnection = new HubConnectionBuilder()
                        .WithUrl(NavigationManager.ToAbsoluteUri("/events"))
                        .Build();

            hubConnection.On<string, CheckoutResponse>("CheckoutSessionStarted", async (pubKey, chkResp) =>
            {
                logger.LogInformation("CheckoutSessionStarted fired");

                if (module == null)
                {
                    module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/checkout.js");
                }

                await module.InvokeVoidAsync("checkout", pubKey, chkResp.CheckoutSessionID);
            });

            await hubConnection.StartAsync();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous dispose operation.
        /// </returns>
        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();

            if (module != null)
            {
                await module.DisposeAsync();
            }
        }
    }
}
