using Manabu.Entities.Flashcards;
using Mediator;

namespace Manabu.Entities.Content.Events;

public class LearningObjectAddedEvent : INotification
{
    public LearningObjectType Type { get; set; }
}
