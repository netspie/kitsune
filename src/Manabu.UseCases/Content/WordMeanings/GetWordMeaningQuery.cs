using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.WordMeanings;

public record GetWordMeaningQuery(
    string WordMeaningId) : IQuery<Result<GetWordMeaningQueryResponse>>;

public record GetWordMeaningQueryResponse(WordMeaningDetailsDTO Content);

public record WordMeaningDetailsDTO(
    string WordId,
    string WordMeaningId,
    string Value,
    string[] Translations,
    string[] PartOfSpeeches,
    string[] Properties,
    string PitchAccent,
    ReadingDTO[] Readings,
    bool KanjiWritingPreferred = true);

public record ReadingDTO(
    string Value,
    PersonaDTO[] Personas);

public record PersonaDTO(PersonaItemDTO[] Properties);
public record PersonaItemDTO(string Key, string Value);
