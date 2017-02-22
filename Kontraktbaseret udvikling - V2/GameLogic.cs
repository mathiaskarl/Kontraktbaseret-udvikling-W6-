using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___V2.DataModels;
using Kontraktbaseret_udvikling___V2.Enums;

namespace Kontraktbaseret_udvikling___V2
{
    public class GameLogic
    {
        /*
        * Invariant 
        *   Players                     != null
        */
        public List<Player> Players             { get; private set; }
        private Queue<Player> _playerQueue;

        public GameResult? GameResult           { get; private set; }
        public List<Player> GameWinners         { get; private set; }

        /*
        * Invariant 
        *   PlayerLimit                 > 1
        */
        public int PlayerLimit                  { get; private set; }
        public bool IsStarted                   { get; private set; }

        private Random _random;

        /*
        * Creation Command 
        * Signature: 
        *   void Initialize()
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
            this.Players        = new List<Player>();
            this.PlayerLimit    = 2;
            this._random        = new Random();
        }

        /*
        * Command 
        * Signature: 
        *   void SetCustomPlayerAmount(int i)
        * Require:
        *   IsStarted                   = false
        *   i                           >= 2
        * Ensure:
        *   PlayersLimit                = i
        */
        public void SetCustomPlayerAmount(int i)
        {
            if (this.IsStarted)
                throw new GameAlreadyStartedException();

            if (i < 2)
                throw new InvalidPlayerAmountException();

            this.PlayerLimit = i;
        }

        /*
        * Command 
        * Signature: 
        *   void CreateNewPlayer(string name, PlayerType type)
        * Require:
        *   IsStarted                   = false
        *   IsPlayerListFull            = false
        * Ensure:
        *   Players.Count               = old Players.Count + 1
        *   Player.Last                 = new Player(name, type)
        */
        public void CreateNewPlayer(string name, PlayerType type)
        {
            if(this.IsPlayerListFull())
                throw new PlayerListFullException();

            this.Players.Add(new Player(
                id:     (this.Players.Count + 1),
                name:   name,
                type:   type
            ));
        }

        /*
        * Command 
        * Signature: 
        *   void AddExistingPlayer(Player player)
        * Require:
        *   IsStarted                   = false
        *   IsPlayerListFull            = false
        * Ensure:
        *   Players.Count               = old Players.Count + 1
        *   Player.Last                 = player
        */
        public void AddExistingPlayer(Player player)
        {
            if (this.IsPlayerListFull())
                throw new PlayerListFullException();

            this.Players.Add(player);
        }

        /*
        * Command 
        * Signature: 
        *   void StartGame()
        * Require:
        *   IsStarted                   = false
        *   IsPlayerListFull            = true
        * Ensure:
        *   IsStarted                   = true
        *   _playerQueue                = new Queue(Players)
        */
        public void StartGame()
        {
            if(this.IsStarted)
                throw new GameAlreadyStartedException();

            if (!this.IsPlayerListFull())
                throw new PlayerListNotFullException();

            this.QueuePlayers();
            this.IsStarted = true;
        }

        private void QueuePlayers()
        {
            this._playerQueue = new Queue<Player>(this.Players);
        }

        /*
        * Command 
        * Signature: 
        *   void AssignPlayerPick(Pick pick = Pick.Default)
        * Require:
        *   IsStarted                   = true
        *   IsPlayerQueueEmpty          = false
        *   _playerQueue.First.PlayerType = PlayerType.Human implies:
        *       pick                    != Pick.Default
        * Ensure:
        *   _playerQueue.First.Pick     = pick
        *   _playerQueue.Count          = old _playerQueue.Count - 1
        */
        public void AssignPlayerPick(Pick pick = Pick.Default)
        {
            if (!this.IsStarted)
                throw new GameNotStartedException();

            if (this.IsPlayerQueueEmpty())
                throw new NoPlayersLeftInQueueException();

            var currentPlayer = this._playerQueue.Dequeue();

            if (currentPlayer.PlayerType == PlayerType.Human)
            {
                if (pick == Pick.Default)
                    throw new InvalidPlayerPickException();
            }
            else
                pick = this.GenerateRandomPick();

            currentPlayer.Pick = pick;
        }

        /*
        * Command 
        * Signature: 
        *   void DetermineGameResult()
        * Require:
        *   IsStarted                   = true
        *   IsPlayerQueueEmpty          = true
        * Ensure:
        *   IsStarted                   = false
        *   GameWinners                 != null
        *   GameResult                  = GameResult.Win implies
                                            For all Players in Winners
                                                Player.Wins = old Player.Wins + 1
                                        || GameResult.Draw          
        */
        public void DetermineGameResult()
        {
            if (!this.IsStarted)
                throw new GameNotStartedException();

            if (!this.IsPlayerQueueEmpty())
                throw new PlayersLeftInQueueException();

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
                this.GameWinners = new List<Player>();

            this.IsStarted = false;
        }

        /*
        * Command 
        * Signature: 
        *   void ResetGameResults()
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
        * Signature: 
        *   Player GetNextPlayer()
        * Requires:
        *   IsStarted                   = true
        *   IsPlayerQueueEmpty()        = false
        * Ensure:
        *   Result                      = _playerQueue.Peek()
        */
        public Player GetNextPlayer()
        {
            if(!this.IsStarted) 
                throw new GameNotStartedException();

            if (this.IsPlayerQueueEmpty())
                throw new NoPlayersLeftInQueueException();

            return this._playerQueue.Peek();
        }

        /*
        * Derived Query 
        * Signature: 
        *   bool IsWinnerFound()
        * Ensure:
        *   Result                      = (GetPlayersWithMaxWins() < 2)
        */
        public bool IsWinnerFound()
        {
            return this.GetPlayersWithMaxWins().Count < 2;
        }

        /*
        * Query 
        * Signature: 
        *   List<Player> GetPlayersWithMaxWins()
        * Require:
        *   Players                     != null
        */
        public List<Player> GetPlayersWithMaxWins()
        {
            return this.Players.FindAll(x => x.Wins == this.Players.Max(z => z.Wins));
        }

        /*
        * Query 
        * Signature: 
        *   bool IsPlayerQueueEmpty()
        * Require:
        *   _playerQueue                != null
        */
        public bool IsPlayerQueueEmpty()
        {
            return this._playerQueue.Count == 0;
        }

        /*
        * Query 
        * Signature: 
        *   bool IsPlayerListFull()
        * Require:
        *   Players                     != null
        *   PlayerLimit                 != null
        */
        public bool IsPlayerListFull()
        {
            return this.Players.Count == this.PlayerLimit;
        }

        private Pick GenerateRandomPick()
        {
            return (Pick) this._random.Next(1, 4);
        }

        private Dictionary<Pick, List<Player>> GeneratePlayerPickList()
        {
            var dic = new Dictionary<Pick, List<Player>>();

            foreach (var player in this.Players)
                if (dic.ContainsKey(player.Pick))
                    dic[player.Pick].Add(player);
                else
                    dic.Add(player.Pick, new List<Player>() { player });

            return dic;
        }

    }
}
