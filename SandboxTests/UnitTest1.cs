namespace SandboxTests;

public class RentalTime
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public RentalTime(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }
}

public static class CarRental
{
    public static bool CanScheduleAll(IEnumerable<RentalTime> unloadingTimes)
    {
        var orderedTimes = unloadingTimes.OrderBy(x => x.Start).ToList();

        for (var i = 0; i <= orderedTimes.Count - 2; i++)
        {
            if (orderedTimes[i].End > orderedTimes[i+1].Start)
                return false;
        }

        return true;
    }

    public static void Maain()
    {
        var format = System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat;

        RentalTime[] unloadingTimes = new RentalTime[] 
        {
            new RentalTime(DateTime.Parse("3/4/2019 19:00", format), DateTime.Parse("3/4/2019 20:30", format)),
            new RentalTime(DateTime.Parse("3/4/2019 22:10", format), DateTime.Parse("3/4/2019 22:30", format)),
            new RentalTime(DateTime.Parse("3/4/2019 20:30", format), DateTime.Parse("3/4/2019 22:00", format))
        };

        Console.WriteLine(CarRental.CanScheduleAll(unloadingTimes));
    }
}