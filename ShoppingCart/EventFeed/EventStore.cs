public interface IEventStore
{
    IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);
    void Raise(string eventName, object content);
}

public class EventStore : IEventStore
{
    public void Raise(string eventName, object content)
    {
        //TODO...
        var sequenceNumber = database.NextSequenceNumber();
        database.Add(new EventStore(
            sequenceNumber,
            DateTimeOffset.UtcNow,
            eventName,
            content
        ));
    }
}