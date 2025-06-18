using System.Collections.ObjectModel;
using System.Windows.Input;
using SozTopar.Services;
using SozTopar.Models;

namespace SozTopar.ViewModels;

public class GamePageViewModel : BaseViewModel
{
    private readonly IGameService _gameService;
    private readonly IEmojiCodeService _emojiCodeService;
    
    private GameWord _currentWord;
    private string _encodedWord = string.Empty;
    private string _currentGuess = string.Empty;
    private int _attemptCount;
    private bool _isGameActive;
    private DateTime _gameStartTime;
    private string _gameStatus = string.Empty;

    public GamePageViewModel(IGameService gameService, IEmojiCodeService emojiCodeService)
    {
        _gameService = gameService;
        _emojiCodeService = emojiCodeService;
        
        GuessHistory = new ObservableCollection<GuessResult>();
        
        SubmitGuessCommand = new Command(async () => await SubmitGuess(), () => CanSubmitGuess());
        NewGameCommand = new Command(async () => await StartNewGame());
        ShowHintCommand = new Command(async () => await ShowHint());
        
        StartNewGame();
    }

    public ObservableCollection<GuessResult> GuessHistory { get; }

    public string EncodedWord
    {
        get => _encodedWord;
        set => SetProperty(ref _encodedWord, value);
    }

    public string CurrentGuess
    {
        get => _currentGuess;
        set
        {
            SetProperty(ref _currentGuess, value);
            ((Command)SubmitGuessCommand).ChangeCanExecute();
        }
    }

    public int AttemptCount
    {
        get => _attemptCount;
        set => SetProperty(ref _attemptCount, value);
    }

    public bool IsGameActive
    {
        get => _isGameActive;
        set => SetProperty(ref _isGameActive, value);
    }

    public string GameStatus
    {
        get => _gameStatus;
        set => SetProperty(ref _gameStatus, value);
    }

    public string WordCategory => _currentWord?.Category ?? string.Empty;
    public int WordDifficulty => _currentWord?.Difficulty ?? 0;

    public ICommand SubmitGuessCommand { get; }
    public ICommand NewGameCommand { get; }
    public ICommand ShowHintCommand { get; }

    private async Task StartNewGame()
    {
        _emojiCodeService.GenerateNewCodeMap();
        _currentWord = _gameService.GetRandomWord();
        EncodedWord = _emojiCodeService.EncodeWord(_currentWord.Word);
        
        CurrentGuess = string.Empty;
        AttemptCount = 0;
        IsGameActive = true;
        _gameStartTime = DateTime.Now;
        GameStatus = "O'yinni boshlang! Kodlangan so'zni toping.";
        
        GuessHistory.Clear();
        
        OnPropertyChanged(nameof(WordCategory));
        OnPropertyChanged(nameof(WordDifficulty));
    }

    private bool CanSubmitGuess()
    {
        return IsGameActive && !string.IsNullOrWhiteSpace(CurrentGuess);
    }

    private async Task SubmitGuess()
    {
        if (!CanSubmitGuess()) return;

        AttemptCount++;
        var result = _gameService.CheckGuess(CurrentGuess, _currentWord.Word);
        GuessHistory.Insert(0, result);

        if (result.IsCorrect)
        {
            IsGameActive = false;
            GameStatus = "🎉 Tabriklaymiz! To'g'ri javob!";
            
            var gameTime = DateTime.Now - _gameStartTime;
            _gameService.UpdateGameStats(true, AttemptCount, gameTime);
            
            await Task.Delay(2000);
            await Application.Current.MainPage.DisplayAlert(
                "Yutdingiz!", 
                $"So'z: {_currentWord.Word}\nUrinishlar: {AttemptCount}\nVaqt: {gameTime:mm\\:ss}", 
                "OK");
        }
        else
        {
            GameStatus = result.Feedback;
        }

        CurrentGuess = string.Empty;
    }

    private async Task ShowHint()
    {
        if (!IsGameActive) return;

        var codeMap = _emojiCodeService.GetCurrentCodeMap();
        var hint = "Kod kaliti:\n";
        
        // Show first 3 letters as hint
        var shownLetters = 0;
        foreach (var kvp in codeMap.Take(3))
        {
            hint += $"{kvp.Value} = {kvp.Key}\n";
            shownLetters++;
        }

        await Application.Current.MainPage.DisplayAlert("Yordam", hint, "OK");
    }
}