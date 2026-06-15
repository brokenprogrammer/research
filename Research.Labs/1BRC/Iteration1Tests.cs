using Research.Shared._1BRC;
using Research.Shared._1BRC.Iteration1;

namespace Research.Labs._1BRC;

public class Iteration1Tests
{
    [Fact]
    public void Iteration1AndNaiveGivesSameResultsForDatasetOf20()
    {
        var measurementsFile = BRCHelpers.GetDataFile("measurements-20.txt");
        
        var naiveRunner = new Naive(measurementsFile);
        var naiveResults = new SortedDictionary<string, Naive.ResultRow>(naiveRunner.Collect());
     
        var brc = new BRC(measurementsFile);
        var brcResults =  new SortedDictionary<string, Naive.ResultRow>(brc.Collect());
        Assert.Equal(brcResults, naiveResults);
    }
    
    [Fact]
    public void Iteration1AndNaiveGivesSameResultsForDatasetOf1000()
    {
        var measurementsFile = BRCHelpers.GetDataFile("measurements-1000.txt");
        
        var naiveRunner = new Naive(measurementsFile);
        var naiveResults = new SortedDictionary<string, Naive.ResultRow>(naiveRunner.Collect());
     
        var brc = new BRC(measurementsFile);
        var brcResults =  new SortedDictionary<string, Naive.ResultRow>(brc.Collect());
        Assert.Equal(brcResults, naiveResults);
    }
}