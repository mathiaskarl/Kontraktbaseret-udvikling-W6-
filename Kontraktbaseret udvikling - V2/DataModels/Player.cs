using System.Collections.Generic;
using Kontraktbaseret_udvikling___V2.Enums;
using Kontraktbaseret_udvikling___V2.Interfaces;

namespace Kontraktbaseret_udvikling___V2.DataModels
{
    public class Player : IPlayer
    {
        public int Id                   { get; }
        public string Name              { get; }
        public PlayerType PlayerType    { get; }
        public Pick Pick                { get; set; }

        /*
        * Invariant 
        *   Wins                        >= 0
        */
        public int Wins                 { get; set; }

        /*
        * Invariant 
        *   AmountOfGames               >= 0
        */
        public int AmountOfGames        { get; set; }

        /*
        * Invariant 
        *   HasPlayedAgainst            != 0
        */
        public List<Player> HasPlayedAgainst { get; private set; }


        /*
        * Creation Command 
        * Signature: 
        *   Player(int id, string name, PlayerType type)
        * Ensure:
        *   this.Id                     = id;
        *   this.Name                   = name;
        *   this.PlayerType             = type;
        *   this.HasPlayedAgainst       = new List<Player>();
        */
        public Player(int id, string name, PlayerType type)
        {
            this.Id         = id;
            this.Name       = name;
            this.PlayerType = type;
            this.HasPlayedAgainst = new List<Player>();
        }

        /*
        * Command 
        * Signature: 
        *   void AddPlayedAgainst(Player player)
        * Requre:
        *   HasPlayedAgainst.Contains(player) != false
        * Ensure:
        *   HasPlayedAgainst.Contains(player) = true
        *   AmountOfGames                     = old AmountOfGames + 1
        */
        public void AddPlayedAgainst(Player player)
        {
            if(this.HasPlayedAgainst.Contains(player))
                throw new HasPlayedAgainstException();

            this.HasPlayedAgainst.Add(player);
            this.AmountOfGames++;
        }

        /*
        * Command 
        * Signature: 
        *   void ResetHasPlayedAgainst()
        * Ensure:
        *   HasPlayedAgainst                  = new List<Player>()
        *   AmountOfGames                     = 0
        */
        public void ResetHasPlayedAgainst()
        {
            this.HasPlayedAgainst = new List<Player>();
            this.AmountOfGames = 0;
        }
    }
}
