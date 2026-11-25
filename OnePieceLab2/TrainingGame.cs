namespace OnePieceLab2;

public sealed class TrainingGame : Game
{
    public override string GameType => "Тренувальна";

    public TrainingGame(GameAccount firstPlayer, GameAccount secondPlayer)
        : base(firstPlayer, secondPlayer, 0) { }

    public override int GetRating(GameAccount player)
    {
        ValidatePlayer(player);
        return 0; // Тренувальна гра не впливає на рейтинг.
    }
}
