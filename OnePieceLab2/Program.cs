using System;

namespace OnePieceLab2;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (args.Length > 0 && args[0] == "--test")
        {
            SelfTests.Run();
            return;
        }

        Console.WriteLine("=== ONE PIECE | Лабораторна робота №2 ===");

        // Поліморфізм: змінні базового типу зберігають різні похідні класи.
        GameAccount luffy = new StreakAccount("Монкі Д. Луффі", 100);
        GameAccount zoro = new ProtectedAccount("Ророноа Зоро", 100);
        GameAccount sanji = new StandardAccount("Санджі", 100);

        // 1. Звичайна гра: Луффі +20, Зоро -10 (захищений акаунт).
        Game game1 = GameFactory.CreateStandardGame(luffy, zoro, 20);
        luffy.WinGame(game1);
        zoro.LoseGame(game1);

        // 2. Друга перемога Луффі поспіль.
        Game game2 = GameFactory.CreateStandardGame(luffy, sanji, 20);
        luffy.WinGame(game2);
        sanji.LoseGame(game2);

        // 3. Третя перемога Луффі: +20 і бонус +10.
        Game game3 = GameFactory.CreateStandardGame(luffy, zoro, 20);
        luffy.WinGame(game3);
        zoro.LoseGame(game3);

        // 4. Тренувальна гра: рейтинг обох гравців не змінюється.
        Game game4 = GameFactory.CreateTrainingGame(luffy, sanji);
        luffy.LoseGame(game4);
        sanji.WinGame(game4);

        // 5. Одностороння: тільки Зоро грає на рейтинг.
        Game game5 = GameFactory.CreateOneSidedGame(zoro, sanji, 40, zoro);
        zoro.WinGame(game5);
        sanji.LoseGame(game5);

        // 6. Велика поразка Зоро: рейтинг не впаде нижче 1.
        Game game6 = GameFactory.CreateStandardGame(zoro, sanji, 400);
        zoro.LoseGame(game6);
        sanji.WinGame(game6);

        // 7. Одностороння: тільки Луффі втрачає рейтинг.
        Game game7 = GameFactory.CreateOneSidedGame(luffy, zoro, 50, luffy);
        luffy.LoseGame(game7);
        zoro.WinGame(game7);

        // 8. Звичайний матч після серії.
        Game game8 = GameFactory.CreateStandardGame(luffy, zoro, 20);
        luffy.LoseGame(game8);
        zoro.WinGame(game8);

        // Перевірка викидання винятку за від'ємний рейтинг.
        Console.WriteLine("\nПеревірка некоректного рейтингу:");
        try
        {
            GameFactory.CreateStandardGame(luffy, zoro, -10);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Помилку перехоплено: {ex.Message}");
        }

        Console.WriteLine("\n=== ІСТОРІЯ ІГОР ===");
        GameAccount[] players = { luffy, zoro, sanji };
        foreach (GameAccount player in players)
            player.GetStats();

        Console.WriteLine("\nДля автоматичних перевірок запусти: dotnet run -- --test");
    }
}
