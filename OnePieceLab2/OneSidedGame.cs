namespace OnePieceLab2;

public sealed class OneSidedGame : Game
{
    public override string GameType => "Одностороння рейтингова";
    public GameAccount RatedPlayer { get; }

    public OneSidedGame(GameAccount firstPlayer, GameAccount secondPlayer,
        int baseRating, GameAccount ratedPlayer)
        : base(firstPlayer, secondPlayer, baseRating)
    {
        ValidatePlayer(ratedPlayer);
        RatedPlayer = ratedPlayer;
    }

    public override int GetRating(GameAccount player)
    {
        ValidatePlayer(player);
        return ReferenceEquals(player, RatedPlayer) ? BaseRating : 0;
    }
}
