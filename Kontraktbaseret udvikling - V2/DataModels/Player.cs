using System.Collections.Generic;
using Kontraktbaseret_udvikling___V2.Enums;
using Kontraktbaseret_udvikling___V2.Interfaces;

namespace Kontraktbaseret_udvikling___V2.DataModels
{
    public class Player : IPlayer
    {
        public int Id                   { get; }
        public string Name              { get; }
        public Pick Pick                { get; set; }
        public int Wins                 { get; set; }
        public int AmountOfGames        { get; set; }
        public List<IPlayer> HasPlayedAgainst { get; private set; }


        /*
        * Creation Command 
        * Ensure:
        *   this.Id                     = id;
        *   this.Name                   = name;
        *   this.HasPlayedAgainst       = new List<IPlayer>();
        */
        public Player(int id, string name)
        {
            this.Id         = id;
            this.Name       = name;
            this.HasPlayedAgainst = new List<IPlayer>();
        }

        public void AddPlayedAgainst(IPlayer player)
        {
            this.HasPlayedAgainst.Add(player);
            this.AmountOfGames++;
        }

        public void ResetHasPlayedAgainst()
        {
            this.HasPlayedAgainst = new List<IPlayer>();
            this.AmountOfGames = 0;
        }

        /*
        * Command 
        * Require:
        *   pick                              != Pick.Default
        * Ensure:
        *   Pick                              = pick
        */
        public void AssignPick(Pick pick)
        {
            this.Pick = pick;
        }
    }
}
