namespace OnePieceLab2;

// Фабрика повертає конкретні ігри як базовий тип Game (upcasting).
public static class GameFactory
{
    public static Game CreateStandardGame(GameAccount firstPlayer,
        GameAccount secondPlayer, int rating)
    {
        return (Game)new StandardGame(firstPlayer, secondPlayer, rating);
    }

    public static Game CreateTrainingGame(GameAccount firstPlayer,
        GameAccount secondPlayer)
    {
        return (Game)new TrainingGame(firstPlayer, secondPlayer);
    }

    public static Game CreateOneSidedGame(GameAccount firstPlayer,
        GameAccount secondPlayer, int rating, GameAccount ratedPlayer)
    {
        return (Game)new OneSidedGame(firstPlayer, secondPlayer, rating, ratedPlayer);
    }
}
