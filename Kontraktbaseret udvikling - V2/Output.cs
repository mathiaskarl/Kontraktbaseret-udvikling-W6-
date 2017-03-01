using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___V2.DataModels;
using Kontraktbaseret_udvikling___V2.Enums;
using Kontraktbaseret_udvikling___V2.Interfaces;

namespace Kontraktbaseret_udvikling___V2
{
    public class Output
    {
        public static void GameTiedExtension(List<IPlayer> players)
        {
            Console.WriteLine("\nThe game was a tie between:");
            foreach(var player in players)
                Console.WriteLine(player.Name);
            Console.WriteLine("\nWith {0} total win{1}\n", players[0].Wins, players[0].Wins > 1 ? "s": "");
            Console.WriteLine("Starting tiebreaker");
        }

        public static void PickAmountOfRounds()
        {
            Console.Clear();
            Console.WriteLine("Please pick the amount of rounds, you wish to play.\n");
        }

        public static void PressAnythingToContinue()
        {
            Console.WriteLine("\nPress anything to continue.\n");
        }

        public static void CurrentRoundInfo(int round)
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("-------------Current round: {0}{1}-----------", round, round > 9 ? "" : "-");
            Console.WriteLine("-----------------------------------------\n");
        }

        public static void TieBreaker()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("----------------Tiebreaker---------------");
            Console.WriteLine("-----------------------------------------\n");
        }

        public static void PickGameType()
        {
            Console.WriteLine("Please pick your desired gametype");
            Console.WriteLine("- Type 1 for Tournament");
            Console.WriteLine("- Type 2 for Classic");
            Console.WriteLine("- Type 3 for Best of");
        }

        public static void PlayAgain()
        {
            Console.WriteLine("\nDo you wish to play again? (Y/N)");
        }

        public static void MustEnterNumberBetween(int from, int to)
        {
            Console.WriteLine("You must enter a number between {0} and {1}", from, to);
        }

        public static void SubmitNameAndPlayerType(int playerId)
        {
            Console.Clear();
            Console.WriteLine("Please submit the name and player type for player: #{0}", playerId);
            Console.WriteLine("- Player name must be between 1 and 15 characters long");
            Console.WriteLine("- Type 1 for Human player");
            Console.WriteLine("- Type 2 for Ai player");
        }

        public static void PlayerNameMustBeBetween()
        {
            Console.WriteLine("The player name must be between 1 and 15 characters long");
        }

        public static void PickAValidPlayerType()
        {
            Console.WriteLine("You must pick a valid player type.");
            Console.WriteLine("- Type 1 for Human player");
            Console.WriteLine("- Type 2 for Ai player");
        }

        public static void GameStarted()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("------------Game has started.------------");
            Console.WriteLine("-----------------------------------------\n");
        }

        public static void Welcome()
        {
            Console.Clear();
            Console.WriteLine("----------------Welcome to---------------");
            Console.WriteLine("----------Rock, paper, scissors.---------");
            Console.WriteLine("-----------------------------------------\n");
        }

        public static void PickAmountOfPlayersBetween(int from, int to)
        {
            Console.WriteLine("Please pick the amount of players you wish to play with.");
            Console.WriteLine("- Type a number between {0} and {1}.", from, to);
        }

        public static void NextPlayerTurn(IPlayer player)
        {
            Console.WriteLine("Your turn {0} - Please choose your pick", player.Name);
            Console.WriteLine("- Type 1 for Scissor");
            Console.WriteLine("- Type 2 for Paper");
            Console.WriteLine("- Type 3 for Rock");
        }

        public static void ChooseValidPick()
        {
            Console.WriteLine("You must choose a valid pick");
            Console.WriteLine("- Type 1 for Scissor");
            Console.WriteLine("- Type 2 for Paper");
            Console.WriteLine("- Type 3 for Rock");
        }

        public static void GameResults()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("--------------Game results:--------------");
            Console.WriteLine("-----------------------------------------\n");
        }

        public static void GameTypeChosen(GameType gameType)
        {
            Console.Clear();
            Console.WriteLine("-------------You have chosen {0}-------------\n", gameType);
        }

        public static void MatchUp(IPlayer player, IPlayer enemy)
        {
            Console.Clear();
            Console.WriteLine("-------------{0} vs {1}-------------\n", player.Name, enemy.Name);
        }

        public static void CurrentResults(List<IPlayer> players)
        {
            Console.WriteLine("\n-----------Current standings:------------\n");
            foreach (var player in players)
                Console.WriteLine("{0}: {1} win{2}.", player.Name, player.Wins, player.Wins > 1 ? "s" : "");
        }

        public static void PlayerResult(List<IPlayer> players)
        {
            Console.WriteLine("\n-------------Player picks:---------------\n");
            foreach (var player in players)
                Console.WriteLine("{0} picked: {1}", player.Name, player.Pick);
        }

        public static void Winner(List<IPlayer> players)
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("--------------" + (players.Count > 1 ? "The winners are:" : "The winner is:-") + "------------");
            Console.WriteLine("-----------------------------------------\n");
            foreach(var player in players)
                Console.WriteLine(player.Name);
        }

        public static void Draw()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("----------The round was a draw-----------");
            Console.WriteLine("-----------------------------------------\n");
        }
    }
}
