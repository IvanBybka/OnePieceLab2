using System;
using System.Threading;

namespace OnePieceLab2;

// Абстрактний базовий клас гри. Один об'єкт = один матч двох гравців.
public abstract class Game
{
    private static long _lastId;
    private bool? _firstPlayerWon;
    private bool? _secondPlayerWon;

    public long Id { get; }
    public GameAccount FirstPlayer { get; }
    public GameAccount SecondPlayer { get; }
    public int BaseRating { get; }
    public abstract string GameType { get; }

    protected Game(GameAccount firstPlayer, GameAccount secondPlayer, int baseRating)
    {
        FirstPlayer = firstPlayer ?? throw new ArgumentNullException(nameof(firstPlayer));
        SecondPlayer = secondPlayer ?? throw new ArgumentNullException(nameof(secondPlayer));

        if (ReferenceEquals(firstPlayer, secondPlayer))
            throw new ArgumentException("Гравець не може грати сам проти себе.");

        if (baseRating < 0)
            throw new ArgumentOutOfRangeException(nameof(baseRating),
                "Рейтинг гри не може бути від'ємним.");

        BaseRating = baseRating;
        Id = Interlocked.Increment(ref _lastId);
    }

    // Кожен конкретний тип гри сам визначає ставку для даного гравця.
    public abstract int GetRating(GameAccount player);

    protected void ValidatePlayer(GameAccount player)
    {
        if (player is null)
            throw new ArgumentNullException(nameof(player));

        if (!ReferenceEquals(player, FirstPlayer) &&
            !ReferenceEquals(player, SecondPlayer))
            throw new ArgumentException("Гравець не бере участі в цій грі.", nameof(player));
    }

    public GameAccount GetOpponent(GameAccount player)
    {
        ValidatePlayer(player);
        return ReferenceEquals(player, FirstPlayer) ? SecondPlayer : FirstPlayer;
    }

    // Не дозволяємо двічі записати той самий матч або дві перемоги в ньому.
    internal void RegisterResult(GameAccount player, bool isWin)
    {
        ValidatePlayer(player);
        bool isFirst = ReferenceEquals(player, FirstPlayer);
        bool? ownResult = isFirst ? _firstPlayerWon : _secondPlayerWon;
        bool? otherResult = isFirst ? _secondPlayerWon : _firstPlayerWon;

        if (ownResult.HasValue)
            throw new InvalidOperationException("Результат цього гравця вже записаний.");

        if (otherResult.HasValue && otherResult.Value == isWin)
            throw new InvalidOperationException("У матчі не можуть бути дві перемоги або дві поразки.");

        if (isFirst)
            _firstPlayerWon = isWin;
        else
            _secondPlayerWon = isWin;
    }
}
