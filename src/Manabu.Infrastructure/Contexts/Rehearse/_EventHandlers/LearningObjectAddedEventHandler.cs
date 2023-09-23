using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Events;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.Entities.Shared;
using Manabu.UseCases.Content.FlashcardLists;
using Mediator;

namespace Manabu.Infrastructure.Contexts.Rehearse.EventHandlers;

public class LearningObjectAddedEventHandler : INotificationHandler<LearningObjectAddedEvent>
{
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IRepository<RehearseItemAsap, RehearseItemId> _rehearseItemAsapRepository;

    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;

    public LearningObjectAddedEventHandler(
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository,
        IRepository<RehearseItemAsap, RehearseItemId> rehearseItemAsapRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository)
    {
        _rehearseItemRepository = rehearseItemRepository;
        _rehearseItemAsapRepository = rehearseItemAsapRepository;
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
    }

    public async ValueTask Handle(
        LearningObjectAddedEvent ev, CancellationToken ct)
    {
        var result = Result.Success();

        var itemIds = new List<LearningObjectId>();

        var id = ev.ObjectId;
        var type = ev.ObjectType;
        if (type.IsContainerItem())
        {
            if (type == LearningContainerType.Lesson)
            {
                var phrases = await GetFlashcardListQueryHandler.GetPhrases(id, type, _lessonRepository, _conversationRepository);
                itemIds.AddRange(phrases);
            }
        }

        var rehearseItems = new List<RehearseItem>();
        foreach (var itemId in itemIds)
        {
            var rehearseItemId = new RehearseItemId(ev.Owner, itemId);
            var rehearseItem = await _rehearseItemRepository.Get(rehearseItemId, result);
            if (rehearseItem is not null)
                continue;

            //rehearseItem = new RehearseItem(rehearseItemId, userId, command.ItemId, command.ItemType);
            //rehearseItems.Add(rehearseItem);
        }
    }
}
