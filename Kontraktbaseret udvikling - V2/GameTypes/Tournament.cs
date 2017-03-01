using System.Collections.Generic;
using System.Linq;
using Kontraktbaseret_udvikling___V2.Interfaces;

namespace Kontraktbaseret_udvikling___V2.GameTypes
{
    public class Tournament : GameType
    {
        public Tournament(GameLogic game)
        {
            this._game = game;

            this.Start(this._game.Players);

            Output.GameResults();

            Output.CurrentResults(this._game.Players);
        }

        private void Start(List<IPlayer> players)
        {
            foreach (var player in players)
                player.ResetHasPlayedAgainst();
                
            var amount = (players.Count*(players.Count - 1))/2;

            for (int i = amount; i > 0; i--)
            {
                var player = players.FirstOrDefault(x => x.AmountOfGames == players.Min(z => z.AmountOfGames));

                if (player == null)
                    throw new PlayerNotFoundException();

                var enemyPlayers = players.FindAll(x => x != player && !player.HasPlayedAgainst.Contains(x));
                var enemy = enemyPlayers.FirstOrDefault(x => x.AmountOfGames == enemyPlayers.Min(z => z.AmountOfGames));

                if (enemy == null)
                    throw new PlayerNotFoundException();

                this.Game(player, enemy);

                player.AddPlayedAgainst(enemy);
                enemy.AddPlayedAgainst(player);
            }

            players = this.GetPlayersWithMaxWins(players);

            if(players.Count > 1)
                this.Start(players);
        }

        private List<IPlayer> GetPlayersWithMaxWins(List<IPlayer> players)
        {
            return players.FindAll(x => x.Wins == players.Max(z => z.Wins));
        }

        private void Game(IPlayer player, IPlayer enemy)
        {
            var game = new GameLogic();
            game.SetCustomPlayerAmount(2);

            foreach (var obj in new[] { player, enemy })
                game.AddExistingPlayer(obj);

            game.StartGame();

            this.AssignPlayerPicks(game, () => Output.MatchUp(player, enemy));

            game.DetermineGameResult();

            this.ShowGameResults(game);
            Output.CurrentResults(this._game.Players);

            Output.PressAnythingToContinue();
            Input.PressAnything();
        }
    }
}
