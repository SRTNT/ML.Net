// See https://aka.ms/new-console-template for more information

namespace ML.Net.Domains;

public class Advertisement
{
    public float Area { get; set; }
    public float BuildYear { get; set; }
    public float Rooms { get; set; }
    public float TotalPrice { get; set; }
    public float Floor { get; set; }

    public DateTime date { get; set; } = DateTime.Now;

    public float Year { get; set; } = DateTime.Now.Year;
    public float Month { get; set; } = DateTime.Now.Month;
    public float Day { get; set; } = DateTime.Now.Day;
    public float Hour { get; set; } = DateTime.Now.Hour;
    public float IsWeekend { get; set; } = DateTime.Now.DayOfWeek == DayOfWeek.Friday ? 1 : 0;

    public bool Elevator { get; set; }
    public bool Parking { get; set; }
    public bool Storage { get; set; }
    public string LocationName { get; set; } = "";
}