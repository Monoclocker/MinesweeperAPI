using MinesweeperAPI.BusinessLogicLayer.DTOs;
using MinesweeperAPI.BusinessLogicLayer.Interfaces;
using MinesweeperAPI.DataAccessLayer.Entities;
using MinesweeperAPI.DataAccessLayer.Interfaces;

namespace MinesweeperAPI.BusinessLogicLayer.Services
{
    public class GameService: IGameService
    {
        IUnitOfWork context;

        public GameService(IUnitOfWork uow)
        {
            context = uow;
        }

        public async Task<GameInfoResponse> CreateGame(NewGameRequest gameParams)
        {

            Game newGame = new Game(gameParams.width, gameParams.height, gameParams.mines_count); 

            context.Games.Create(newGame);

            return await Task.FromResult(new GameInfoResponse()
            {
                completed = false,
                game_id = newGame.Id.ToString(),
                width = gameParams.width,
                height = gameParams.height,
                mines_count = gameParams.mines_count,
                field = newGame.Field,
            });
        }

        public async Task<GameInfoResponse> MakeTurn(GameTurnRequest turnParams)
        {
            int row = turnParams.row;
            int col = turnParams.col;
            Guid id;
            
            if (!Guid.TryParse(turnParams.game_id, out id))
            {
                throw new Exception("ID введён в некоректном формате");
            }

            Game? game = context.Games.Get(id);

            if (game is null)
            {
                throw new Exception("Игра с таким ID не найдена");
            }

            if (game!.IsCompleted)
            {
                throw new Exception("Игра завершена");
            }

            if (game.IsAlreadyOpened(row, col))
            {
                throw new Exception("Запрашиваемая клетка уже открыта");
            }

            if (game.CheckMine(row, col))
            {
                game.RevealAllField();
                game.CompleteGame();
            }
            else
            {
                game.RevealOneCell(row, col);
            }

            if (game.CheckWin())
            {
                game.CompleteGame();
            }


            context.Games.Update(game);

            GameInfoResponse newState = new GameInfoResponse()
            {
                game_id = id.ToString(),
                completed = game.IsCompleted,
                mines_count = game.MinesCoordinates.Length,
                height = game.Field.Length,
                width = game.Field[0].Length,
                field = game.Field,
            };

            return await Task.FromResult(newState);

        }
    }
}
