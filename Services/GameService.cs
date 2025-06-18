using SozTopar.Models;
using SozTopar.Services;

namespace SozTopar.Services;

public class GameService : IGameService
{
    private readonly List<GameWord> _words;
    private readonly Random _random;
    private GameStats _gameStats;

    public GameService()
    {
        _random = new Random();
        _gameStats = LoadGameStats();
        _words = InitializeWords();
    }

    private List<GameWord> InitializeWords()
    {
        return new List<GameWord>
        {
            new() { Word = "OLMA", Category = "Meva", Difficulty = 1 },
            new() { Word = "KITOB", Category = "Buyum", Difficulty = 2 },
            new() { Word = "MASHINA", Category = "Transport", Difficulty = 3 },
            new() { Word = "KOMPYUTER", Category = "Texnika", Difficulty = 4 },
            new() { Word = "DASTURCHI", Category = "Kasb", Difficulty = 5 },
            new() { Word = "OILA", Category = "Inson", Difficulty = 1 },
            new() { Word = "MAKTAB", Category = "Bino", Difficulty = 2 },
            new() { Word = "TELEFON", Category = "Texnika", Difficulty = 3 },
            new() { Word = "RESTORAN", Category = "Bino", Difficulty = 3 },
            new() { Word = "UNIVERSITET", Category = "Bino", Difficulty = 4 }
        };
    }

    public GameWord GetRandomWord()
    {
        var word = _words[_random.Next(_words.Count)];
        return word;
    }

    public GuessResult CheckGuess(string guess, string targetWord)
    {
        guess = guess.ToUpper().Trim();
        targetWord = targetWord.ToUpper().Trim();

        var result = new GuessResult
        {
            Guess = guess,
            IsCorrect = guess == targetWord
        };

        if (result.IsCorrect)
        {
            result.HintLevel = HintLevel.Yonayapti;
            result.Feedback = "🎉 To'g'ri! Tabriklaymiz!";
            result.CorrectLetters = targetWord.Length;
            result.CorrectPositions = targetWord.Length;
            return result;
        }

        // Calculate similarity
        var similarity = CalculateSimilarity(guess, targetWord);
        result.CorrectLetters = CountCorrectLetters(guess, targetWord);
        result.CorrectPositions = CountCorrectPositions(guess, targetWord);

        // Determine hint level based on similarity
        result.HintLevel = similarity switch
        {
            >= 0.8 => HintLevel.Yonayapti,
            >= 0.6 => HintLevel.Issiq,
            >= 0.3 => HintLevel.Iliq,
            _ => HintLevel.Sovuq
        };

        result.Feedback = result.HintLevel switch
        {
            HintLevel.Yonayapti => "🔥 Yonayapti! Juda yaqin!",
            HintLevel.Issiq => "🌡️ Issiq! Yaqinlashyapsiz!",
            HintLevel.Iliq => "🌤️ Iliq. To'g'ri yo'ldasiz.",
            HintLevel.Sovuq => "❄️ Sovuq. Boshqacha harakat qiling.",
            _ => "🤔 Qaytadan urinib ko'ring."
        };

        return result;
    }

    private double CalculateSimilarity(string guess, string target)
    {
        if (string.IsNullOrEmpty(guess) || string.IsNullOrEmpty(target))
            return 0;

        var distance = LevenshteinDistance(guess, target);
        var maxLength = Math.Max(guess.Length, target.Length);
        return 1.0 - (double)distance / maxLength;
    }

    private int LevenshteinDistance(string s1, string s2)
    {
        var matrix = new int[s1.Length + 1, s2.Length + 1];

        for (int i = 0; i <= s1.Length; i++)
            matrix[i, 0] = i;

        for (int j = 0; j <= s2.Length; j++)
            matrix[0, j] = j;

        for (int i = 1; i <= s1.Length; i++)
        {
            for (int j = 1; j <= s2.Length; j++)
            {
                var cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                matrix[i, j] = Math.Min(
                    Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }
        }

        return matrix[s1.Length, s2.Length];
    }

    private int CountCorrectLetters(string guess, string target)
    {
        var targetChars = target.ToCharArray().ToList();
        var correctCount = 0;

        foreach (var c in guess)
        {
            if (targetChars.Contains(c))
            {
                targetChars.Remove(c);
                correctCount++;
            }
        }

        return correctCount;
    }

    private int CountCorrectPositions(string guess, string target)
    {
        var correctPositions = 0;
        var minLength = Math.Min(guess.Length, target.Length);

        for (int i = 0; i < minLength; i++)
        {
            if (guess[i] == target[i])
                correctPositions++;
        }

        return correctPositions;
    }

    public GameStats GetGameStats()
    {
        return _gameStats;
    }

    public void UpdateGameStats(bool won, int attempts, TimeSpan gameTime)
    {
        _gameStats.TotalGames++;
        _gameStats.TotalAttempts += attempts;
        _gameStats.LastPlayed = DateTime.Now;

        if (won)
        {
            _gameStats.WonGames++;
            if (_gameStats.BestScore == 0 || attempts < _gameStats.BestScore)
                _gameStats.BestScore = attempts;

            if (_gameStats.BestTime == TimeSpan.Zero || gameTime < _gameStats.BestTime)
                _gameStats.BestTime = gameTime;
        }

        _gameStats.AverageAttempts = (double)_gameStats.TotalAttempts / _gameStats.TotalGames;
        SaveGameStats();
    }

    public void ResetStats()
    {
        _gameStats = new GameStats();
        SaveGameStats();
    }

    private GameStats LoadGameStats()
    {
        // In a real app, load from preferences or database
        return new GameStats();
    }

    private void SaveGameStats()
    {
        // In a real app, save to preferences or database
    }
}