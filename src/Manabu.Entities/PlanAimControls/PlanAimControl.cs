using Corelibs.Basic.DDD;
using Manabu.Entities.ExercisesAims;
using Manabu.Entities.Plans;
using Manabu.Entities.Shared;

namespace Manabu.Entities.PlanAimControls;

public record PlanAimControlId(string Value) : EntityId(Value);

public class PlanAimControl : AimControl<Plan, PlanId, PlanAim, PlanAimId, PlanAimControlId>, IAggregateRoot<PlanAimControlId>
{
    public const string DefaultCollectionName = "planAimControls";

    protected override PlanAim CreateAim(Plan plan, DateTime startedTime) =>
        new PlanAim(plan.Id, startedTime);
}
