using Corelibs.Basic.Blocks;
using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;

namespace Manabu.UseCases.Content;

public interface ILearningObjectToRehearseProcessor
{
    Task<Result> Process(UserId owner, LearningObjectId id);
}

public interface ILearningObjectToRehearseProcessor<TEntity, TId> : ILearningObjectToRehearseProcessor
    where TEntity : Entity<TId>
    where TId : EntityId
{
}

