using System;
using System.Collections.Generic;
using System.Linq;
using Gomer.Models;

namespace Gomer.DataAccess.Implementation
{
    public class GameRepository : RepositoryBase<GameModel>, IGameRepository
    {
        public GameRepository(IDataContext context) : base(context)
        {
        }

        protected override ISet<GameModel> Set
        {
            get { return Context.Games; }
        }

        protected override IEnumerable<GameModel> DefaultSort(IEnumerable<GameModel> models)
        {
            return models.OrderBy(x => x.Name);
        }

        protected override GameModel PopulateSecondaryData(GameModel model)
        {
            return model;
        }

        protected override void OnBeforeAdd(GameModel entity)
        {
            entity.StatusHistory.Add(new StatusHistoryModel
            {
                Status = entity.Status,
                StatusDate = entity.AddedOn
            });
        }

        protected override void OnBeforeUpdate(GameModel entity, GameModel previousState)
        {
            if (entity.Status.Id != previousState.Status.Id)
            {
                entity.StatusHistory.Add(new StatusHistoryModel
                {
                    Status = entity.Status,
                    StatusDate = DateTime.Today
                });
            }
        }
    }
}