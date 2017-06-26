using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Prompt
    {
        public static bool YesOrNo(string question)
        {
            while (true)
            {
                Console.WriteLine(question);

                var answer = Console.ReadLine().Trim().ToLower();
                switch (answer)
                {
                    case "y":
                        return true;
                    case "n":
                        return false;
                    default:
                        Console.WriteLine("Please enter a valid command");
                        continue;
                }
            }
        }

        public static Player ChoosePlayer(List<Player> players, string question) // refactor method display logic!!!
        {
            while (true)
            {
                Console.WriteLine(question);

                for (var i = 0; i < players.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {players[i].PlayerName}");
                }

                int.TryParse(Console.ReadLine(), out int answer);

                if (answer >= 1 && answer <= players.Count)
                    return players[answer - 1];
                Console.WriteLine("Please enter a valid number");
            }
        }

        public static IFieldRentable ChooseFieldRentable(List<IFieldRentable> fields, string question) // refactor method display logic!!!
        {
            while (true)
            {
                Console.WriteLine(question);

                for (var i = 0; i < fields.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {fields[i].FieldName}");
                }
                Console.WriteLine($"{fields.Count + 1}. None");

                int.TryParse(Console.ReadLine(), out int answer);

                if (answer >= 1 && answer <= fields.Count)
                    return fields[answer - 1];
                if (answer == fields.Count + 1)
                    return null;
                Console.WriteLine("Please enter a valid number");
            }
        }

        public static string ChooseOption(Dictionary<string, Action> choices, string question)
        {
            while (true)
            {
                Console.WriteLine(question);

                foreach (var choice in choices)
                {
                    Console.WriteLine(choice.Key);
                }

                var chosen = Console.ReadLine().ToLower().Trim();

                if (choices.Any(c => c.Key.ToLower().StartsWith(chosen)))
                    return choices.Keys.First(k => k.ToLower().StartsWith(chosen));
                Console.WriteLine("Please enter a valid command");
            }
        }

        public static int EnterAmount(Player player, string question)
        {
            while (true)
            {
                Console.WriteLine(question);

                int.TryParse(Console.ReadLine(), out int answer);

                if (answer >= 0 && answer <= player.Money)
                    return answer;
                Console.WriteLine("Please enter a valid amount");
            }
        }
    }
}