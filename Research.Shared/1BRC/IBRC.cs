namespace Research.Shared._1BRC;

public interface IBRC
{
    public Dictionary<string, StationResult> Collect();
}

public record StationResult(double Min, double Mean, double Max);