namespace SNO.API;

public partial class Event
{
    public int Eventid { get; set; }

    public DateTime? Begindate { get; set; }

    public DateTime? Estimationdate { get; set; }

    public string? Title { get; set; }

    public string? Mddescription { get; set; }
}
