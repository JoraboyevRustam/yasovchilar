using System.Windows.Input;
using SozTopar.Services;
using SozTopar.Models;

namespace SozTopar.ViewModels;

public class MainPageViewModel : BaseViewModel
{
    private readonly IGameService _gameService;
    private GameStats _gameStats;

    public MainPageViewModel(IGameService gameService)
    {
        _gameService = gameService;
        _gameStats = _gameService.GetGameStats();
        
        StartGameCommand = new Command(async () => await StartGame());
        ViewStatsCommand = new Command(async () => await ViewStats());
        ResetStatsCommand = new Command(async () => await ResetStats());
    }

    public GameStats GameStats
    {
        get => _gameStats;
        set => SetProperty(ref _gameStats, value);
    }

    public ICommand StartGameCommand { get; }
    public ICommand ViewStatsCommand { get; }
    public ICommand ResetStatsCommand { get; }

    private async Task StartGame()
    {
        await Shell.Current.GoToAsync(nameof(Views.GamePage));
    }

    private async Task ViewStats()
    {
        var stats = _gameService.GetGameStats();
        var message = $"Jami o'yinlar: {stats.TotalGames}\n" +
                     $"Yutilgan o'yinlar: {stats.WonGames}\n" +
                     $"Yutish foizi: {(stats.TotalGames > 0 ? (double)stats.WonGames / stats.TotalGames * 100 : 0):F1}%\n" +
                     $"O'rtacha urinishlar: {stats.AverageAttempts:F1}\n" +
                     $"Eng yaxshi natija: {(stats.BestScore > 0 ? stats.BestScore.ToString() : "Yo'q")}";

        await Application.Current.MainPage.DisplayAlert("Statistika", message, "OK");
    }

    private async Task ResetStats()
    {
        var result = await Application.Current.MainPage.DisplayAlert(
            "Statistikani tozalash", 
            "Barcha statistikani o'chirishni xohlaysizmi?", 
            "Ha", "Yo'q");

        if (result)
        {
            _gameService.ResetStats();
            GameStats = _gameService.GetGameStats();
        }
    }

    public void RefreshStats()
    {
        GameStats = _gameService.GetGameStats();
    }
}