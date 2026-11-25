namespace OnePieceLab2;

// Запис результату конкретного гравця; два гравці можуть мати один ID матчу.
public sealed class GameRecord
{
    public Game Game { get; }
    public bool IsWin { get; }
    public int PlayedRating { get; }
    public int RatingChange { get; }
    public int Index { get; }

    internal GameRecord(Game game, bool isWin, int playedRating,
        int ratingChange, int index)
    {
        Game = game;
        IsWin = isWin;
        PlayedRating = playedRating;
        RatingChange = ratingChange;
        Index = index;
    }
}
