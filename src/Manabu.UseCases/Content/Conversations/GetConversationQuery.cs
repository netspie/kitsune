using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Rehearse.RehearseEntities;
using Manabu.Entities.Shared;
using Mediator;

namespace Manabu.UseCases.Content.Conversations;


public record GetConversationQuery(
    string ConversationId,
    string PhrasesMode = null) : IQuery<Result<GetConversationQueryResponse>>;

public record GetConversationQueryResponse(ConversationDetailsDTO Content);

public record ConversationDetailsDTO(
    string Id,
    string Name,
    string Description,
    bool Learned,
    LessonDTO[] Lessons,
    PhraseDTO[] Phrases);

public record LessonDTO(
    string Id,
    string Name);

public record PhraseDTO(
    string Id,
    string Original,
    string? Speaker = null,
    string? SpeakerTranslation = null);
