namespace OnePieceLab2;

public sealed class ProtectedAccount : GameAccount
{
    public override string AccountType => "Захищений акаунт";

    public ProtectedAccount(string userName, int initialRating = 100)
        : base(userName, initialRating) { }

    protected override int CalculateRatingChange(Game game, bool isWin)
    {
        int rating = game.GetRating(this);
        // За поразку знімається приблизно половина (непарне: округлення вгору).
        return isWin ? rating : -(rating / 2 + rating % 2);
    }
}
