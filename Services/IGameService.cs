using SozTopar.Models;

namespace SozTopar.Services;

public interface IGameService
{
    GameWord GetRandomWord();
    GuessResult CheckGuess(string guess, string targetWord);
    GameStats GetGameStats();
    void UpdateGameStats(bool won, int attempts, TimeSpan gameTime);
    void ResetStats();
}