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
        PlayerType PlayerType   { get; }
    }
}
