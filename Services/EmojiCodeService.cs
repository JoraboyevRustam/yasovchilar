namespace SozTopar.Services;

public class EmojiCodeService : IEmojiCodeService
{
    private Dictionary<char, string> _codeMap;
    private readonly Random _random;
    private readonly List<string> _emojis;

    public EmojiCodeService()
    {
        _random = new Random();
        _emojis = InitializeEmojis();
        GenerateNewCodeMap();
    }

    private List<string> InitializeEmojis()
    {
        return new List<string>
        {
            "🔥", "❄️", "⭐", "🌙", "☀️", "🌈", "⚡", "🌊",
            "🎵", "🎨", "🎯", "🎪", "🎭", "🎲", "🎸", "🎺",
            "🚀", "✈️", "🚗", "🚲", "⛵", "🏠", "🏰", "🗼",
            "💎", "👑", "🔑", "⚔️", "🛡️", "🏆", "🎖️", "🏅"
        };
    }

    public void GenerateNewCodeMap()
    {
        _codeMap = new Dictionary<char, string>();
        var availableEmojis = new List<string>(_emojis);
        var uzbekAlphabet = "ABDEFGHIJKLMNOPQRSTUVXYZ".ToCharArray();

        foreach (var letter in uzbekAlphabet)
        {
            if (availableEmojis.Count > 0)
            {
                var randomIndex = _random.Next(availableEmojis.Count);
                _codeMap[letter] = availableEmojis[randomIndex];
                availableEmojis.RemoveAt(randomIndex);
            }
        }
    }

    public string EncodeWord(string word)
    {
        var encoded = string.Empty;
        foreach (var letter in word.ToUpper())
        {
            if (_codeMap.ContainsKey(letter))
            {
                encoded += _codeMap[letter];
            }
            else
            {
                encoded += "❓"; // Unknown character
            }
        }
        return encoded;
    }

    public Dictionary<char, string> GetCurrentCodeMap()
    {
        return new Dictionary<char, string>(_codeMap);
    }
}