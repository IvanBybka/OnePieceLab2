using System;

namespace OnePieceLab2;

internal static class SelfTests
{
    private static int _count;

    public static void Run()
    {
        _count = 0;
        Console.WriteLine("=== Автоматичні перевірки лабораторної №2 ===");

        var standard = new StandardAccount("Стандарт", 100);
        var protectedPlayer = new ProtectedAccount("Захищений", 100);
        Game ordinary = GameFactory.CreateStandardGame(standard, protectedPlayer, 21);
        standard.WinGame(ordinary);
        protectedPlayer.LoseGame(ordinary);
        Check(standard.CurrentRating == 121, "Звичайний акаунт: перемога +21");
        Check(protectedPlayer.CurrentRating == 89, "Захищений акаунт: поразка -11");
        Check(standard.GamesCount == 1 && protectedPlayer.GamesCount == 1,
            "Історія обох учасників зберігає матч");

        var streak = new StreakAccount("Серія", 100);
        var rival = new StandardAccount("Опонент", 100);
        for (int i = 0; i < 3; i++)
        {
            Game match = GameFactory.CreateStandardGame(streak, rival, 20);
            streak.WinGame(match);
            rival.LoseGame(match);
        }
        Check(streak.CurrentRating == 170, "Бонус +10 за третю перемогу");

        Game training = GameFactory.CreateTrainingGame(streak, rival);
        streak.LoseGame(training);
        rival.WinGame(training);
        Check(streak.CurrentRating == 170 && rival.CurrentRating == 40,
            "Тренувальна гра не змінює рейтинг");
        Check(streak.ConsecutiveRatedWins == 3,
            "Тренувальні ігри не переривають рейтингову серію");

        Game oneSided = GameFactory.CreateOneSidedGame(streak, rival, 30, streak);
        streak.LoseGame(oneSided);
        rival.WinGame(oneSided);
        Check(streak.CurrentRating == 140 && rival.CurrentRating == 40,
            "Одностороння гра впливає тільки на вибраного учасника");
        Check(streak.ConsecutiveRatedWins == 0,
            "Рейтингова поразка перериває серію");

        Game bigLoss = GameFactory.CreateStandardGame(standard, protectedPlayer, 400);
        standard.LoseGame(bigLoss);
        protectedPlayer.WinGame(bigLoss);
        Check(standard.CurrentRating == 1, "Рейтинг не опускається нижче 1");

        Game uniqueA = GameFactory.CreateStandardGame(standard, rival, 10);
        Game uniqueB = GameFactory.CreateTrainingGame(standard, rival);
        Check(uniqueA.Id != uniqueB.Id && uniqueA.Id != ordinary.Id,
            "Кожен об'єкт Game має унікальний ID");

        Expect<ArgumentOutOfRangeException>(() =>
            GameFactory.CreateStandardGame(standard, rival, -1),
            "Від'ємний рейтинг заборонений");
        Expect<ArgumentException>(() => new StandardAccount("   "),
            "Порожнє ім'я гравця заборонене");
        Expect<ArgumentOutOfRangeException>(() => new StreakAccount("Новий", 0),
            "Початковий рейтинг 0 заборонений");
        Expect<ArgumentException>(() =>
            GameFactory.CreateStandardGame(standard, standard, 10),
            "Сам із собою грати не можна");
        Expect<ArgumentException>(() =>
            GameFactory.CreateOneSidedGame(standard, rival, 10, streak),
            "Рейтинговий учасник мусить бути в матчі");
        Expect<ArgumentException>(() => streak.WinGame(uniqueA),
            "Сторонній гравець не може змінити результат матчу");

        Game duplicate = GameFactory.CreateStandardGame(standard, rival, 5);
        standard.WinGame(duplicate);
        Expect<InvalidOperationException>(() => standard.LoseGame(duplicate),
            "Повторне внесення матчу заборонене");
        Expect<InvalidOperationException>(() => rival.WinGame(duplicate),
            "Дві перемоги в одному матчі заборонені");
        rival.LoseGame(duplicate);
        Check(standard.GamesCount == 3 && rival.GamesCount == 6,
            "Помилкові операції не додають ігор в історію");

        // Навіть звичайна гра може мати нульову ставку: результат рахується,
        // але рейтинг та рейтингова серія не змінюються.
        int beforeStandard = standard.CurrentRating;
        int beforeRival = rival.CurrentRating;
        Game zeroRating = GameFactory.CreateStandardGame(standard, rival, 0);
        standard.WinGame(zeroRating);
        rival.LoseGame(zeroRating);
        Check(standard.CurrentRating == beforeStandard &&
              rival.CurrentRating == beforeRival,
            "Нульова ставка не змінює рейтинг");

        Expect<ArgumentNullException>(() => standard.WinGame(null!),
            "Метод WinGame не приймає null");
        Expect<ArgumentNullException>(() => rival.LoseGame(null!),
            "Метод LoseGame не приймає null");

        // Переконуємося, що GetStats дійсно виводить історію у потрібному форматі.
        using (var capture = new System.IO.StringWriter())
        {
            System.IO.TextWriter savedOutput = Console.Out;
            try
            {
                Console.SetOut(capture);
                standard.GetStats();
            }
            finally
            {
                Console.SetOut(savedOutput);
            }

            string output = capture.ToString();
            Check(output.Contains("Гра №1") &&
                  output.Contains($"ID: {ordinary.Id}") &&
                  output.Contains("Суперник: Захищений") &&
                  output.Contains("Рейтинг гри: 21") &&
                  output.Contains("Перемога"),
                "GetStats показує індекс, ID, суперника, результат і рейтинг");
        }

        Console.WriteLine($"\nУспішно виконано {_count} перевірок.");
    }

    private static void Check(bool condition, string description)
    {
        if (!condition)
            throw new Exception("ПОМИЛКА: " + description);
        _count++;
        Console.WriteLine("[OK] " + description);
    }

    private static void Expect<TException>(Action action, string description)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            Check(true, description);
            return;
        }
        throw new Exception("ПОМИЛКА: " + description);
    }
}
