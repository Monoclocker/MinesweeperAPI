using MinesweeperAPI.BusinessLogicLayer.DTOs;

namespace MinesweeperAPI.BusinessLogicLayer.Interfaces
{
    public interface IGameService
    {
        Task<GameInfoResponse> CreateGame(NewGameRequest gameParams);
        Task<GameInfoResponse> MakeTurn(GameTurnRequest turnParams);
    }
}
