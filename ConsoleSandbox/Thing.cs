using System;
using System.Collections.Generic;
using System.Linq;

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
    
    // void static Sort(ref int a, ref int b)
    // {
    //     if (a > b)
    //     {
    //         int tmp = a;
    //         a = b;
    //         b = tmp;
    //     }
    // }
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

    public static void Main()
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

public class Prefix
{
    public static IEnumerable<string> AllPrefixes(int prefixLength, IEnumerable<string> words)
    {
        return words.Where(x => x.Length >= prefixLength)
            .Select(x => x.Substring(0, prefixLength))
            .Distinct();
    }
    
    public static void AMain()
    {
        // Should print "flo", "fle", and "fla" since those are the distinct, length 3 prefixes.
        foreach (var p in AllPrefixes(3, new string[] { "flow", "flowers", "flew", "flag", "fm" }))
            Console.WriteLine(p);
    }
}

public class Book
{
    public string Genre { get; set; }
    public string Name { get; set; }
    public int PageCount { get; set; }
}

public class RouteA
{
    public string AddBook(string genre,  Book book)
    {
        return $"General genre: {book.Genre}, "
               + $"name: {book.Name}, page count: {book.PageCount}, "
               + $"book genre: {book.Genre}";
    }
}


