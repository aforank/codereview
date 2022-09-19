using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.Web.Server.Hubs
{
    /// <summary>
    /// Events Hub manager.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.SignalR.Hub" />
    public class EventsHub : Hub
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<EventsHub> logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsHub"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EventsHub(ILogger<EventsHub> logger)
        {
            this.logger = logger;

        }
        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous connect.
        /// </returns>
        public override Task OnConnectedAsync()
        {
            logger.LogInformation($"Connection ID =>{Context.ConnectionId}\n User =>{Context.UserIdentifier}");
            return base.OnConnectedAsync();
        }
    }
}