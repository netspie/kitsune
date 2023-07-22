using Corelibs.Basic.DDD;
using Manabu.Entities.Plans;
using Manabu.Entities.Sessions;
using Manabu.Entities.SessionsAims;
using Manabu.Entities.Shared;

namespace Manabu.Entities.SessionAimControls;

public record SessionAimControlId(string Value) : EntityId(Value);

public class SessionAimControl : AimControl<Session, SessionId, SessionAim, SessionAimId, SessionAimControlId>, IAggregateRoot<SessionAimControlId>
{
    public const string DefaultCollectionName = "sessionAimControls";

    protected override SessionAim CreateAim(Session session, DateTime startedTime) =>
        new SessionAim(session.Id, startedTime);
}
