using SNO;

interface IEventDataService
{
    ICollection<Event> GetEvents(DateTime from, DateTime to);

    void PushEvent(Event @event);
    void GetEventDetails(Event @event);
}

