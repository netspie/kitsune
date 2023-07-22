using Corelibs.Basic.DDD;
using Manabu.Entities.Exercises;
using Manabu.Entities.ExercisesAims;
using Manabu.Entities.Shared;

namespace Manabu.Entities.ExercisesAimsControls;

public record ExerciseAimControlId(string Value) : EntityId(Value);

public class ExerciseAimControl : AimControl<Exercise, ExerciseId, ExerciseAim, ExerciseAimId, ExerciseAimControlId>, IAggregateRoot<ExerciseAimControlId>
{
    public const string DefaultCollectionName = "exerciseAimControls";

    protected override ExerciseAim CreateAim(Exercise exercise, DateTime startedTime) =>
        new ExerciseAim(exercise.Id, startedTime);
}
