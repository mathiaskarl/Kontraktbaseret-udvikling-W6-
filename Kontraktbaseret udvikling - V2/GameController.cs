using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___V2.DataModels;
using Kontraktbaseret_udvikling___V2.Enums;

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

        private void InitiateGameType()
        {
            switch (this._gameType)
            {
                case GameType.Tournament:
                    this.PlayAgainDelegate(() =>
                    {
                        var tournament = new Tournament(this.TournamentGame);
                        tournament.Start(this._game.Players);

                        Output.GameResults();

                        Output.CurrentResults(this._game.Players);

                        this._game.ResetGameResults();

                    });
                    break;

                case GameType.Classic:
                    this.PlayAgainDelegate(() =>
                    {
                        this._game.StartGame();

                        this.AssignPlayerPicks(this._game, Output.GameStarted);

                        this._game.DetermineGameResult();

                        Output.GameResults();

                        this.ShowGameResults(this._game);

                    });
                    break;

                case GameType.BestOf:
                    this.PlayAgainDelegate(() =>
                    {
                        Output.PickAmountOfRounds();
                        var amount = Input.GetNumberBetween(2, 100, () => Output.MustEnterNumberBetween(2, 100));

                        Output.GameStarted();

                        for (int i = 1; i <= amount; i++)
                        {
                            this._game.StartGame();

                            this.AssignPlayerPicks(this._game, () => Output.CurrentRoundInfo(i));

                            this._game.DetermineGameResult();

                            this.ShowGameResults(this._game);

                            Output.CurrentResults(this._game.Players);

                            if (i != amount)
                            {
                                Output.PressAnythingToContinue();
                                Input.PressAnything();
                            }
                        }

                        while (!this._game.IsWinnerFound())
                        {
                            var players = this._game.GetPlayersWithMaxWins();
                            Output.GameTiedExtension(players);

                            Output.PressAnythingToContinue();
                            Input.PressAnything();

                            var game = new GameLogic();
                            game.SetCustomPlayerAmount(players.Count);

                            foreach (var player in players)
                                game.AddExistingPlayer(player);

                            game.StartGame();

                            this.AssignPlayerPicks(game, Output.TieBreaker);

                            game.DetermineGameResult();

                            this.ShowGameResults(game);

                            Output.CurrentResults(game.Players);
                        }

                    }, this._game.ResetGameResults);
                    break;
            }
        }

        private void TournamentGame(Player player, Player enemy)
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

        private void AssignPlayerPicks(GameLogic game, Action callBack = null)
        {
            while (!game.IsPlayerQueueEmpty())
            {
                callBack?.Invoke();

                var currentPlayer = game.GetNextPlayer();

                Output.NextPlayerTurn(currentPlayer);

                var playerPick = currentPlayer.PlayerType == PlayerType.Human
                    ? (Pick) Input.GetNumberBetween(
                        from:           1, 
                        to:             3,
                        failCallBack:   Output.ChooseValidPick)
                    : Pick.Default;

                game.AssignPlayerPick(playerPick);
            }
        }

        private void ShowGameResults(GameLogic game)
        {
            if (game.GameResult == GameResult.Win)
                Output.Winner(game.GameWinners);
            else
                Output.Draw();

            Output.PlayerResult(game.Players);
        }
    }
}
