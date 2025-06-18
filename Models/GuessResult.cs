namespace SozTopar.Models;

public enum HintLevel
{
    Sovuq,    // Cold - 0-20% correct
    Iliq,     // Warm - 21-60% correct
    Issiq,    // Hot - 61-90% correct
    Yonayapti // Burning - 91-100% correct
}

public class GuessResult
{
    public string Guess { get; set; } = string.Empty;
    public HintLevel HintLevel { get; set; }
    public int CorrectLetters { get; set; }
    public int CorrectPositions { get; set; }
    public bool IsCorrect { get; set; }
    public string Feedback { get; set; } = string.Empty;
}