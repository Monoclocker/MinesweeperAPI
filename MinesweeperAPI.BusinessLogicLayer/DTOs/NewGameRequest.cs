using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperAPI.BusinessLogicLayer.DTOs
{
    public class NewGameRequest
    {
        public int width { get; set; }
        public int height { get; set; }
        public int mines_count {  get; set; }
    }
}
