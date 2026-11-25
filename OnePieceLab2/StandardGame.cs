namespace OnePieceLab2;

public sealed class StandardGame : Game
{
    public override string GameType => "Стандартна";

    public StandardGame(GameAccount firstPlayer, GameAccount secondPlayer, int baseRating)
        : base(firstPlayer, secondPlayer, baseRating) { }

    public override int GetRating(GameAccount player)
    {
        ValidatePlayer(player);
        return BaseRating;
    }
}
