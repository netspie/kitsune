using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Events;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.Entities.Shared;

namespace Manabu.Infrastructure.Contexts.Rehearse.EventHandlers;

public interface IEventHandler<TEvent>
{
    Task<Result> Handle(TEvent ev);
}

public class LearningObjectAddedForRehearseEventHandler : IEventHandler<LearningObjectAddedForRehearseEvent>
{
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IRepository<RehearseItemAsap, RehearseItemId> _rehearseItemAsapRepository;

    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;

    public LearningObjectAddedForRehearseEventHandler(
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

    public async Task<Result> Handle(
        LearningObjectAddedForRehearseEvent ev)
    {
        var result = Result.Success();

        //var itemIds = new List<ItemData>();

        //var id = ev.ObjectId;
        //var type = ev.ObjectType;
        //if (type.IsContainerItem())
        //{
        //    if (type == LearningContainerType.Lesson)
        //    {
        //        var phrases = await GetFlashcardListQueryHandler.GetPhrases(id, type, _lessonRepository, _conversationRepository);
        //        itemIds.AddRange(phrases.Select(id => new ItemData(
        //            new LearningObjectId(id.Value), LearningItemType.Phrase)));
        //    }
        //}

        //var rehearseItems = new List<RehearseItem>();
        //foreach (var item in itemIds)
        //{
        //    var modes = item.Type.GetLearningModes();
        //    foreach (var mode in modes)
        //    {
        //        var rehearseItemId = new RehearseItemId(ev.Owner, item.Id, mode);

        //        var getRhResult = Result.Success();
        //        var rehearseItem = await _rehearseItemRepository.Get(rehearseItemId, getRhResult);
        //        if (rehearseItem is not null)
        //            continue;

        //        rehearseItem = new RehearseItem(
        //            rehearseItemId, ev.Owner, item.Id, item.Type, mode);
        //        rehearseItems.Add(rehearseItem);
        //    }
        //}

        //result += await _rehearseItemRepository.Create(rehearseItems);
        //if (!result.IsSuccess)
        //    Console.WriteLine("Could not create rehearse items");

        return result;
    }

    record ItemData(LearningObjectId Id, LearningItemType Type);
}
