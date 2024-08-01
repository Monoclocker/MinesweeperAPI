using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperAPI.BusinessLogicLayer.DTOs
{
    public class GameTurnRequest
    {
        public string game_id { get; set; } = default!;
        public int col { get; set; }
        public int row {  get; set; }
    }
}
