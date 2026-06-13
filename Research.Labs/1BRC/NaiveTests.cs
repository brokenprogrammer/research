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
        
    }
}