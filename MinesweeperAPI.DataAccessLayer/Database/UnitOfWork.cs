using MinesweeperAPI.DataAccessLayer.Entities;
using MinesweeperAPI.DataAccessLayer.Interfaces;
using MongoDB.Driver;

namespace MinesweeperAPI.DataAccessLayer.Database
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IMongoDatabase database;
        private IRepository<Game> _Games = default!;

        public IRepository<Game> Games 
        { 
            get 
            {
                if (_Games is null)
                {
                    _Games = new GameRepository(database);
                }
                return _Games;
            } 
        }

        public UnitOfWork(IMongoClient client)
        {
            database = client.GetDatabase("minesweeper");
        }
    }
}
