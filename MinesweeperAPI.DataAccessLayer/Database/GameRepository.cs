using MinesweeperAPI.DataAccessLayer.Entities;
using MinesweeperAPI.DataAccessLayer.Interfaces;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperAPI.DataAccessLayer.Database
{
    internal class GameRepository : IRepository<Game>
    {
        IMongoCollection<Game> games;

        public GameRepository(IMongoDatabase database) 
        {
            games = database.GetCollection<Game>("Games");
        }

        public Game? Get(Guid id)
        {
            return games.Find(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Game> GetAll()
        {
            return games.AsQueryable();
        }

        public void Create(Game entity)
        {
            games.InsertOne(entity);
        }

        public void Update(Game entity)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Where(x => x.Id == entity.Id);

            UpdateDefinition<Game> updater = Builders<Game>.Update
                .Set(x => x.Field, entity.Field)
                .Set(x => x.IsCompleted, entity.IsCompleted);

            games.UpdateOne(filter, updater);
        }

        public void Delete(Guid id)
        {
            FilterDefinition<Game> filter = Builders<Game>.Filter.Where(x => x.Id == id);

            games.DeleteOne(filter);
        }

    }
}
