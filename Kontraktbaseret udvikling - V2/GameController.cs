using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___V2.DataModels;
using Kontraktbaseret_udvikling___V2.Enums;
using Kontraktbaseret_udvikling___V2.GameTypes;
using Kontraktbaseret_udvikling___V2.Interfaces;
using GameType = Kontraktbaseret_udvikling___V2.Enums.GameType;

namespace Kontraktbaseret_udvikling___V2
{
    public class GameController
    {
        private GameLogic _game;

        private GameType _gameType;
        
        public GameController()
        {
            this.GameLoop(this.Game);
        }

        private void Game()
        {
            this._game = new GameLogic();

            Output.Welcome();

            Output.PickGameType();

            this.PickGameType();

            Output.PickAmountOfPlayersBetween(2, 10);

            this.AssignPlayerAmount(this._game);

            this.AssignPlayerData(this._game);

            this.InitiateGameType();
        }

        private void GameLoop(Action toDo)
        {
            while (true)
            {
                toDo?.Invoke();
            }
        }

        private void InitiateGameType()
        {
            switch (this._gameType)
            {
                case GameType.Tournament:
                    this.PlayAgainDelegate(() => { new Tournament(this._game); }, this._game.ResetGameResults);
                    break;

                case GameType.Classic:
                    this.PlayAgainDelegate(() => { new Classic(this._game); });
                    break;

                case GameType.BestOf:
                    this.PlayAgainDelegate(() => { new BestOf(this._game); }, this._game.ResetGameResults);
                    break;
            }
        }

        private void PlayAgainDelegate(Action toDo, Action callBack = null)
        {
            while (true)
            {
                toDo();

                Output.PlayAgain();
                var result = Input.GetStringEqualTo(Output.PlayAgain, "y", "n");

                if (result == "n")
                    break;

                callBack?.Invoke();
            }
        }

        private void PickGameType()
        {
            this._gameType = (GameType)Input.GetNumberBetween(1, 3, () => Output.MustEnterNumberBetween(1, 3));
            Output.GameTypeChosen(this._gameType);
        }

        private void AssignPlayerAmount(GameLogic game)
        {
            var playerAmount = Input.GetNumberBetween(
                from:           2, 
                to:             10, 
                failCallBack:   () => Output.MustEnterNumberBetween(2, 10));

            game.SetCustomPlayerAmount(playerAmount);
        }

        private void AssignPlayerData(GameLogic game)
        {
            for (int i = 1; i <= game.PlayerLimit; i++)
            {
                Output.SubmitNameAndPlayerType(i);

                game.CreateNewPlayer(
                    name:   Input.GetStringMaxLength(15, Output.PlayerNameMustBeBetween),
                    type:   (PlayerType)Input.GetNumberBetween(
                        from:           1, 
                        to:             2, 
                        failCallBack:   Output.PickAValidPlayerType)
                );
            }
        }

        
    }
}
