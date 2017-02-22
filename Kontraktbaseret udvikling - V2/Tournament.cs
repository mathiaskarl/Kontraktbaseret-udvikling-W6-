using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___V2.DataModels;

namespace Kontraktbaseret_udvikling___V2
{
    public class Tournament
    {
        private List<Player> _players;
        private Action<Player, Player> _game;
         
        public Tournament(Action<Player, Player> game)
        {
            this._game = game;
        }

        public void Start(List<Player> players)
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

                this._game(player, enemy);

                player.AddPlayedAgainst(enemy);
                enemy.AddPlayedAgainst(player);
            }

            players = this.GetPlayersWithMaxWins(players);

            if(players.Count > 1)
                this.Start(players);
        }

        public List<Player> GetPlayersWithMaxWins(List<Player> players)
        {
            return players.FindAll(x => x.Wins == players.Max(z => z.Wins));
        }
    }
}
