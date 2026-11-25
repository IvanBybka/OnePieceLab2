using System;
using System.Collections.Generic;

namespace OnePieceLab2;

// Базовий клас акаунта. Дані приховані, зовнішній код не може змінювати рейтинг.
public abstract class GameAccount
{
    private readonly List<GameRecord> _history = new();

    public string UserName { get; }
    public int CurrentRating { get; private set; }
    public int GamesCount => _history.Count;
    public int ConsecutiveRatedWins { get; private set; }
    public abstract string AccountType { get; }

    protected GameAccount(string userName, int initialRating = 100)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("Ім'я гравця не може бути порожнім.", nameof(userName));

        if (initialRating < 1)
            throw new ArgumentOutOfRangeException(nameof(initialRating),
                "Початковий рейтинг має бути не меншим за 1.");

        UserName = userName.Trim();
        CurrentRating = initialRating;
    }

    // Поліморфізм: у кожного типу акаунта власна формула зміни рейтингу.
    protected abstract int CalculateRatingChange(Game game, bool isWin);

    // На відміну від ЛР №1, передаємо об'єкт базового класу гри, а не число.
    public void WinGame(Game game) => CompleteGame(game, true);
    public void LoseGame(Game game) => CompleteGame(game, false);

    private void CompleteGame(Game game, bool isWin)
    {
        if (game is null)
            throw new ArgumentNullException(nameof(game));

        // GetRating також перевіряє, що поточний акаунт є учасником матчу.
        int playedRating = game.GetRating(this);

        foreach (GameRecord previous in _history)
        {
            if (ReferenceEquals(previous.Game, game))
                throw new InvalidOperationException("Цю гру вже внесено в історію гравця.");
        }

        int change = CalculateRatingChange(game, isWin);
        if (playedRating == 0 && change != 0)
            throw new InvalidOperationException("Гра з нульовим рейтингом не змінює бали.");

        int newRating = Math.Max(1, checked(CurrentRating + change));
        GameRecord record = new GameRecord(game, isWin, playedRating,
            newRating - CurrentRating, GamesCount + 1);

        game.RegisterResult(this, isWin);
        _history.Add(record);
        CurrentRating = newRating;

        // Тренувальні та інші безрейтингові матчі не впливають на серію.
        if (playedRating > 0)
            ConsecutiveRatedWins = isWin ? checked(ConsecutiveRatedWins + 1) : 0;
    }

    // Приклад статичного поліморфізму: перевантаження методу GetStats.
    public void GetStats() => GetStats(true);

    public void GetStats(bool showHistory)
    {
        Console.WriteLine($"\n--- {UserName} ({AccountType}) ---");
        Console.WriteLine($"Рейтинг: {CurrentRating}; Кількість ігор: {GamesCount}");

        if (!showHistory)
            return;

        if (_history.Count == 0)
        {
            Console.WriteLine("Історія ігор порожня.");
            return;
        }

        foreach (GameRecord record in _history)
        {
            string result = record.IsWin ? "Перемога" : "Поразка";
            string signedChange = record.RatingChange > 0
                ? $"+{record.RatingChange}"
                : record.RatingChange.ToString();

            Console.WriteLine(
                $"Гра №{record.Index} | ID: {record.Game.Id} | " +
                $"Тип: {record.Game.GameType} | " +
                $"Суперник: {record.Game.GetOpponent(this).UserName} | " +
                $"{result} | Рейтинг гри: {record.PlayedRating} | " +
                $"Зміна рейтингу: {signedChange}");
        }
    }
}
