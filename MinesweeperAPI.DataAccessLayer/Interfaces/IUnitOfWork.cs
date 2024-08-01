using MinesweeperAPI.DataAccessLayer.Entities;

namespace MinesweeperAPI.DataAccessLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Game> Games { get; }
    }
}
