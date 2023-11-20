using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.Phrases;

public record GetPhraseQuery(
    string PhraseId) : IQuery<Result<GetPhraseQueryResponse>>;

public record GetPhraseQueryResponse(PhraseDetailsDTO Content);

public record PhraseDetailsDTO(
    string Id,
    string Original,
    string[] Translations,
    string[] Contexts,
    bool Learned,
    AudioDTO[] Audios,
    WordMeaningDTO[]? WordMeanings = null);

public record AudioDTO(string Id, string Href);

public record WordMeaningDTO(
    string Id,
    string Original,
    string Translation,
    string? WordInflectionId = null,
    string? Reading = null,
    string? WritingMode = null,
    string? CustomWriting = null);
