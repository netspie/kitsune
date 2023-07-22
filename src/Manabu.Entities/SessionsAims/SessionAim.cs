using Corelibs.Basic.DDD;
using Manabu.Entities.Sessions;
using Manabu.Entities.Shared;

namespace Manabu.Entities.SessionsAims;

public record SessionAimId(string Value) : EntityId(Value);

public class SessionAim : Aim<SessionId, SessionAimId>, IAggregateRoot<SessionAimId>
{
    public const string DefaultCollectionName = "sessionAims";

    public SessionAim(
        SessionId sessionId,
        DateTime startedTime) : base(sessionId, startedTime) { }
}
