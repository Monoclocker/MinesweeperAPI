using Microsoft.AspNetCore.Mvc;
using MinesweeperAPI.BusinessLogicLayer.DTOs;
using MinesweeperAPI.BusinessLogicLayer.Interfaces;
using MinesweeperAPI.PresentationLayer.DTOs;

namespace MinesweeperAPI.PresentationLayer.Controllers
{
    [ApiController]
    public class GameController : ControllerBase
    {
        IGameService gameService;
        public GameController(IGameService gameService) 
        {
            this.gameService = gameService;
        }

        [HttpPost("/new")]
        public async Task<IActionResult> CreateGame(NewGameRequest dto)
        {
            if (dto.width <= 0 || 
                dto.height <= 0 || 
                dto.mines_count <= 0 || 
                dto.mines_count > (dto.width * dto.height - 1))
            {
                return BadRequest(new ErrorResponse() { error = "Неправильно введены параметры" });
            };

            return Ok(await gameService.CreateGame(dto));
        }

        [HttpPost("/turn")]
        public async Task<IActionResult> MakeNewTurn(GameTurnRequest dto)
        {
            try
            {
                return Ok(await gameService.MakeTurn(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { error = ex.Message });
            }
        }
    }
}
