using System.Diagnostics;
using Research.Shared._1BRC;

namespace Research.Labs._1BRC;

public class NaiveTests
{
    [Fact]
    public void NaiveSolutionGivesCorrectResultsForDatasetOf20()
    {
        var resultsFile = BRCHelpers.GetDataFile("results-20.txt");
        var results = BRCHelpers.ParseResults(resultsFile);

        var measurementsFile = BRCHelpers.GetDataFile("measurements-20.txt");
        var naiveRunner = new Naive(measurementsFile);
        var naiveResults = new SortedDictionary<string, Naive.ResultRow>(naiveRunner.Collect());
        
        Assert.Equal(results, naiveResults);
    }
    
    [Fact]
    public void NaiveSolutionGivesCorrectResultsForDatasetOf1000()
    {
        var resultsFile = BRCHelpers.GetDataFile("results-1000.txt");
        var results = BRCHelpers.ParseResults(resultsFile);

        var measurementsFile = BRCHelpers.GetDataFile("measurements-1000.txt");
        var naiveRunner = new Naive(measurementsFile);
        var naiveResults = new SortedDictionary<string, Naive.ResultRow>(naiveRunner.Collect());
        
        foreach (var key in results.Keys)
        {
            Assert.True(naiveResults.ContainsKey(key), $"Key '{key}' missing from naiveResults");
        
            var expected = results[key];
            var actual = naiveResults[key];
        
            Assert.Equal(expected.Min, actual.Min, precision: 1);
            Assert.Equal(expected.Max, actual.Max, precision: 1);
            Assert.InRange(actual.Mean, expected.Mean - 0.05, expected.Mean + 0.05);
        }
    }
}