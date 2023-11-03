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
    string WritingType);

public record WritingMode(string Value)
{
    public static readonly WritingMode Dictionary = new("dictionary");
    public static readonly WritingMode Hiragana = new("hiragana");
    public static readonly WritingMode Katakana = new("katakana");
    public static readonly WritingMode OtherWriting = new("otherWriting");
}
