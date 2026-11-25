namespace OnePieceLab2;

public sealed class StandardAccount : GameAccount
{
    public override string AccountType => "Звичайний акаунт";

    public StandardAccount(string userName, int initialRating = 100)
        : base(userName, initialRating) { }

    protected override int CalculateRatingChange(Game game, bool isWin)
    {
        int rating = game.GetRating(this);
        return isWin ? rating : -rating;
    }
}
