namespace SozTopar.Models;

public class GameWord
{
    public string Word { get; set; } = string.Empty;
    public string EmojiCode { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Difficulty { get; set; }
}