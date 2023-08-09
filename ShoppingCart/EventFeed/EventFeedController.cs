namespace ShoppingCart.EventFeed
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    [Rout("/events")]
    public class EventFeedController : Controller
    {
        private readonly IEventStore eventStore;

        public EventFeedController(IEventStore eventStore) => 
            this.eventStore = eventStore;
        
        [HttpGet("")]
        public EventFeed[] Get([FromQuery] long start, [FromQuery] long end = long.MaxValue) => 
            this.eventStore.GetEvents(start, end).ToArray();
    }
}