namespace SozTopar.Models;

public class GameStats
{
    public int TotalGames { get; set; }
    public int WonGames { get; set; }
    public int TotalAttempts { get; set; }
    public double AverageAttempts { get; set; }
    public int BestScore { get; set; }
    public TimeSpan BestTime { get; set; }
    public DateTime LastPlayed { get; set; }
}