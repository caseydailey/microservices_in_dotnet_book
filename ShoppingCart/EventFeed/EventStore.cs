public interface IEventStore
{
    IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);
    void Raise(string eventName, object content);
}

public class EventStore : IEventStore
{
    // TODO
}