using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Corelibs.Basic.UseCases;
using FluentValidation;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Events;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Flashcards;
using Manabu.Entities.Rehearse.RehearseContainers;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.UseCases.Content.FlashcardLists;
using Mediator;
using System.Reflection;
using System.Security.Claims;

namespace Manabu.UseCases.Rehearse.RehearseItems;

public class AddLearningItemForRehearseCommandHandler : ICommandHandler<AddLearningItemForRehearseCommand, Result>
{
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IRepository<RehearseContainer, RehearseContainerId> _rehearseContainerRepository;
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;
    private readonly IPublisher _publisher;

    public AddLearningItemForRehearseCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository,
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<RehearseContainer, RehearseContainerId> rehearseContainerRepository,
        IPublisher publisher)
    {
        _userAccessor = userAccessor;
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
        _rehearseItemRepository = rehearseItemRepository;
        _lessonRepository = lessonRepository;
        _rehearseContainerRepository = rehearseContainerRepository;
        _publisher = publisher;
    }

    public async ValueTask<Result> Handle(AddLearningItemForRehearseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var userId = await _userAccessor.GetUserID<UserId>();

        var itemIds = new List<string>();

        await _publisher.Publish(new LearningObjectAddedEvent());

        var itemType = new LearningObjectType(command.ItemType);
        if (itemType.IsContainerItem())
        {
            if (itemType == LearningContainerType.Lesson)
            {
                var phrases = await GetFlashcardListQueryHandler.GetPhrases(command.ItemId, itemType, _lessonRepository, _conversationRepository);
                itemIds.AddRange(phrases.Select(p => p.Value).ToArray());

                var rehearseContainerId = new RehearseContainerId(userId.Value, command.ItemId);
                var rehearseContainer = new RehearseContainer(rehearseContainerId, userId, command.ItemId, command.ItemType);
                await _rehearseContainerRepository.Save(rehearseContainer, result);
            }
        }

        var rehearseItems = new List<RehearseItem>();
        foreach (var itemId in itemIds) 
        {
            var rehearseItemId = new RehearseItemId(userId.Value, itemId);
            var rehearseItem = await _rehearseItemRepository.Get(rehearseItemId, result);
            if (rehearseItem is not null)
                continue;

            //rehearseItem = new RehearseItem(rehearseItemId, userId, command.ItemId, command.ItemType);
            //rehearseItems.Add(rehearseItem);
        }

        result += await _rehearseItemRepository.Create(rehearseItems);

        return result;
    }

    public static IRepository[] GetAllRepositories(object instance)
    {
        var type = instance.GetType();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        var repositories = fields
            .Where(f => f.FieldType.IsGenericType &&
                        f.FieldType.GetGenericTypeDefinition() == typeof(IRepository<,>))
            .Select(f => (IRepository) f.GetValue(instance))
            .ToArray();

        return repositories;
    }
}

public record AddLearningItemForRehearseCommand(
    string ItemId,
    string ItemType) : ICommand<Result>;

public class AddLearningItemForRehearseCommandValidator : AbstractValidator<AddLearningItemForRehearseCommand> {}
