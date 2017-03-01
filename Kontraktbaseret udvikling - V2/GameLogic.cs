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
    public class GameLogic
    {
        /*
        * Invariant 
        *   Players                     != null
        */
        public List<IPlayer> Players             { get; private set; }
        private Queue<IPlayer> _playerQueue;

        public GameResult? GameResult           { get; private set; }
        public List<IPlayer> GameWinners         { get; private set; }

        /*
        * Invariant 
        *   PlayerLimit                 > 1
        */
        public int PlayerLimit                  { get; private set; }
        public bool IsStarted                   { get; private set; }

        private Random _random;

        /*
        * Creation Command 
        * Ensure:
        *   Players                     = new List<Player>()
        *   PlayersLimit                = 2
        */
        public GameLogic()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.Players        = new List<IPlayer>();
            this.PlayerLimit    = 2;
            this._random        = new Random();
        }

        /*
        * Command 
        * Require:
        *   IsStarted                   = false
        *   i                           >= 2
        * Ensure:
        *   PlayersLimit                = i
        */
        public void SetCustomPlayerAmount(int i)
        {
            this.PlayerLimit = i;
        }

        /*
        * Command 
        * Require:
        *   IsStarted                   = false
        *   IsPlayerListFull            = false
        * Ensure:
        *   Players.Count               = old Players.Count + 1
        *   Player.Last                 = new Player(name, type)
        */
        public void CreateNewPlayer(string name, PlayerType type)
        {
            var player =  type == PlayerType.Human
                ? (IPlayer)new Player(
                    id: (this.Players.Count + 1),
                    name: name)
                : (IPlayer)new Ai(
                    id: (this.Players.Count + 1),
                    name: name);

            this.Players.Add(player);
        }

        /*
        * Command 
        * Require:
        *   IsStarted                   = false
        *   IsPlayerListFull            = false
        * Ensure:
        *   Players.Count               = old Players.Count + 1
        *   Player.Last                 = player
        */
        public void AddExistingPlayer(IPlayer player)
        {
            this.Players.Add(player);
        }

        /*
        * Command 
        * Require:
        *   IsStarted                   = false
        *   IsPlayerListFull            = true
        * Ensure:
        *   IsStarted                   = true
        *   _playerQueue                = new Queue(Players)
        */
        public void StartGame()
        {
            this.QueuePlayers();
            this.IsStarted = true;
        }

        private void QueuePlayers()
        {
            this._playerQueue = new Queue<IPlayer>(this.Players);
        }

        /*
        * Command 
        * Require:
        *   IsStarted                   = true
        *   IsPlayerQueueEmpty          = false
        * Ensure:
        *   old _playerQueue.First.Pick     = pick
        *   _playerQueue.Count          = old _playerQueue.Count - 1
        *
        * Frame rule:
        *   For all i : 0 < old _playerQueue.Count - 1
        *       _playerQueue[i] = old _playerQueue[i]
        */
        public void AssignPlayerPick(Pick pick)
        {
            var currentPlayer = this._playerQueue.Dequeue();
            currentPlayer.AssignPick(pick);
        }

        /*
        * Command 
        * Require:
        *   IsStarted                   = true
        *   IsPlayerQueueEmpty          = true
        * Ensure:
        *   IsStarted                   = false
        *   GameWinners                 != null
        *   GameResult                  = GameResult.Win || GameResult.Draw
        *   GameResult = GameResult.Win implies
        *       For all players in GameWinners
        *           Player.Wins = old Player.Wins + 1
        *
        * Frame rule:
        *   GameResult = GameResult.Draw implies
        *       For all i : 0 < Players.Count
        *           Players[i] = old Players[i]
        *
        */
        public void DetermineGameResult()
        {
            var playerPicks = this.GeneratePlayerPickList();

            this.GameResult = (playerPicks.Count == 2 ? Enums.GameResult.Win : Enums.GameResult.Draw);

            if (this.GameResult == Enums.GameResult.Win)
            {
                this.GameWinners = ((int) playerPicks.First().Key)%3 + 1 == ((int) playerPicks.Last().Key)
                    ? playerPicks.First().Value
                    : playerPicks.Last().Value;

                foreach (var player in this.GameWinners)
                    player.Wins++;
            }
            else
                this.GameWinners = new List<IPlayer>();

            this.IsStarted = false;
        }

        /*
        * Command 
        * Require:
        *   Players                     != null
        *   IsStarted                   = false
        * Ensure:
        *   GameResult                  = null
        *   For all players in Players:
        *       Player.Wins             = 0
        */
        public void ResetGameResults()
        {
            this.GameResult = null;
            this.Players.ForEach(x => x.Wins = 0);
        }

        /*
        * Derived Query 
        * Requires:
        *   IsStarted                   = true
        *   IsPlayerQueueEmpty()        = false
        * Ensure:
        *   Result                      = _playerQueue.Peek()
        */
        public IPlayer GetNextPlayer()
        {
            return this._playerQueue.Peek();
        }

        /*
        * Derived Query 
        * Ensure:
        *   Result                      = (GetPlayersWithMaxWins() < 2)
        */
        public bool IsWinnerFound()
        {
            return this.GetPlayersWithMaxWins().Count < 2;
        }

        /*
        * Query 
        * Require:
        *   Players                     != null
        */
        public List<IPlayer> GetPlayersWithMaxWins()
        {
            return this.Players.FindAll(x => x.Wins == this.Players.Max(z => z.Wins));
        }

        /*
        * Query 
        * Require:
        *   _playerQueue                != null
        */
        public bool IsPlayerQueueEmpty()
        {
            return this._playerQueue.Count == 0;
        }

        /*
        * Query 
        * Require:
        *   Players                     != null
        *   PlayerLimit                 != null
        */
        public bool IsPlayerListFull()
        {
            return this.Players.Count == this.PlayerLimit;
        }

        private Dictionary<Pick, List<IPlayer>> GeneratePlayerPickList()
        {
            var dic = new Dictionary<Pick, List<IPlayer>>();

            foreach (var player in this.Players)
                if (dic.ContainsKey(player.Pick))
                    dic[player.Pick].Add(player);
                else
                    dic.Add(player.Pick, new List<IPlayer>() { player });

            return dic;
        }

    }
}
