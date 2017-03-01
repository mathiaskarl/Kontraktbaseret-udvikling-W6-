using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___V2.Enums;

namespace Kontraktbaseret_udvikling___V2.Interfaces
{
    public interface IPlayer
    {
        int Id                  { get; }
        string Name             { get; }
        Pick Pick               { get; set; }

        /*
        * Invariant 
        *   Wins                        >= 0
        */
        int Wins { get; set; }

        /*
        * Invariant 
        *   AmountOfGames               >= 0
        */
        int AmountOfGames { get; set; }

        /*
        * Invariant 
        *   HasPlayedAgainst            != 0
        */
        List<IPlayer> HasPlayedAgainst { get; }

        /*
        * Command 
        * Requre:
        *   HasPlayedAgainst.Contains(player) != false
        * Ensure:
        *   HasPlayedAgainst.Contains(player) = true
        *   AmountOfGames                     = old AmountOfGames + 1
        */
        void AddPlayedAgainst(IPlayer player);

        /*
        * Command 
        * Ensure:
        *   HasPlayedAgainst                  = new List<Player>()
        *   AmountOfGames                     = 0
        */
        void ResetHasPlayedAgainst();

        /*
        * Command 
        * Ensure:
        *   Pick                              = pick
        */
        void AssignPick(Pick pick);
    }
}
