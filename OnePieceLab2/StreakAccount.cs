namespace OnePieceLab2;

public sealed class StreakAccount : GameAccount
{
    public override string AccountType => "Акаунт із бонусом за серію";

    public StreakAccount(string userName, int initialRating = 100)
        : base(userName, initialRating) { }

    protected override int CalculateRatingChange(Game game, bool isWin)
    {
        int rating = game.GetRating(this);

        if (!isWin)
            return -rating;

        if (rating == 0)
            return 0;

        // Починаючи з третьої поспіль рейтингової перемоги, бонус +10.
        int bonus = ConsecutiveRatedWins >= 2 ? 10 : 0;
        return checked(rating + bonus);
    }
}
