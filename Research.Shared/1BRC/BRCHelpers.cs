using System.Globalization;
using System.Text;
using Research.Shared._1BRC;

namespace Research.Shared._1BRC;

public static class BRCHelpers
{
    public static string GetDataFile(string fileName)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
        var file = Path.Combine(solutionDirectory, "Data", fileName);

        return file;
    }

    public static SortedDictionary<string, StationResult> ParseResults(string fileName)
    {
        var dict =  new Dictionary<string, StationResult>();
        
        var x = File.ReadAllText(fileName, new UTF8Encoding());
        x = x.Replace("{", "").Replace("}", "");

        var stations = x.Split(",");
        foreach (var station in stations)
        {
            var parts =  station.Split("=");
            var stationName = parts[0].Trim();

            var resultParts = parts[1].Split("/");
            var min = double.Parse(resultParts[0], CultureInfo.InvariantCulture);
            var mean = double.Parse(resultParts[1], CultureInfo.InvariantCulture);
            var max = double.Parse(resultParts[2], CultureInfo.InvariantCulture);
            
            dict[stationName] = new StationResult(min, mean, max);
        }
        
        return new SortedDictionary<string, StationResult>(dict);
    }
}