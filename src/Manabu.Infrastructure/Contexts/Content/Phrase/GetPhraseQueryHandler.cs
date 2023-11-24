using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Content.WordLexemes;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.Entities.Rehearse.RehearseEntities;
using Manabu.Entities.Shared;
using Manabu.UseCases.Content.Phrases;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.Contexts.Content.Phrases;

public class GetPhraseQueryHandler : IQueryHandler<GetPhraseQuery, Result<GetPhraseQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;

    public GetPhraseQueryHandler(
        MongoConnection mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetPhraseQueryResponse>> Handle(GetPhraseQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetPhraseQueryResponse>.Success();

        var collection = _mongoConnection.Database.GetCollection<Phrase>(Phrase.DefaultCollectionName);
        var queryable = collection.AsQueryable();
        var phrase = queryable.FirstOrDefault(x => x.Id.Value == query.PhraseId);

        var wordMeanings = queryable
            .Where(x => x.Id.Value == query.PhraseId)
            .SelectMany(x => x.WordMeanings)
            .GroupJoin(
                _mongoConnection.Database.GetCollection<WordMeaning>(WordMeaning.DefaultCollectionName).AsQueryable(),
                link => link.WordMeaningId.Value,
                wordMeaning => wordMeaning.Id.Value,
                (link, wordMeanings) => new { Link = link, Content = wordMeanings.First() })
            .GroupJoin(
                _mongoConnection.Database.GetCollection<Word>(Word.DefaultCollectionName).AsQueryable(),
                wordMeaning => wordMeaning.Content.WordId,
                word => word.Id,
                (wm, words) => new { Link = wm.Link, Content = wm.Content, Word = words.First() }
            )
            .GroupJoin(
                _mongoConnection.Database.GetCollection<WordLexeme>(WordLexeme.DefaultCollectionName).AsQueryable(),
                wordMeaning => wordMeaning.Word.Lexeme,
                lexeme => lexeme.Id,
                (wm, lexemes) => new { Link = wm.Link, Content = wm.Content, Word = wm.Word, Lexeme = lexemes.First() }
            )
            .ToList();

        var audios = queryable
            .Where(x => x.Id.Value == query.PhraseId)
            .SelectMany(x => x.Audios)
            .GroupJoin(
                _mongoConnection.Database.GetCollection<Audio>(Audio.DefaultCollectionName).AsQueryable(),
                audioId => audioId.Value,
                audio => audio.Id.Value,
                (audioId, audios) => new
                {
                    Id = audioId,
                    Audio = audios.First()
                })
            .ToList();

        var learningObjectId = new LearningObjectId(query.PhraseId);

        var rheCollection = _mongoConnection.Database.GetCollection<RehearseEntity>(RehearseEntity.DefaultCollectionName);
        var rehearseEntityCount = rheCollection.CountDocuments(Builders<RehearseEntity>.Filter.Eq("_id", learningObjectId));

        return result.With(
            new GetPhraseQueryResponse(
                new(query.PhraseId,
                    phrase.Original,
                    phrase.Translations.ToArrayOrEmpty(),
                    phrase.Contexts.ToArrayOrEmpty(),
                    Learned: rehearseEntityCount > 0,
                    audios!.SelectOrEmpty(a => new AudioDTO(a.Id.Value, a.Audio.Href)).ToArray(),
                    wordMeanings.SelectOrEmpty(w => 
                    {
                        var dictionaryForm = w.Content.Original;
                        var conjugatedForm = w.Link.WordInflectionId.SelectValue(
                            inflectionId =>
                            {
                                if (inflectionId.IsNullOrEmpty())
                                    return dictionaryForm;

                                var inflectionDef = inflectionId.Split(',').Select(i => i.Trim()).ToArray();
                                var inflectionType = new InflectionType(inflectionDef[0]);
                                var inflection = w.Lexeme.Inflections.FirstOrDefault(i => i.Type == inflectionType);

                                var inflectionForm = inflectionDef[1] == "formal" ? inflection?.Formal : inflection?.Informal;
                                return inflectionDef[2] == "negative" ? inflectionForm?.Negative?.Value : inflectionForm?.Positive.Value;
                            });

                        var targetWriting = w.Link.WritingMode.SelectValue(mode =>
                        {
                            if (mode == WritingMode.Hiragana)
                            {
                                return w.Content.HiraganaWritings.First().Value;
                            }

                            return conjugatedForm;
                        });

                        var reading = w.Content.HiraganaWritings.SelectValue(
                            readings =>
                            {
                                if (readings.Count == 1 && w.Link.Reading.IsNullOrEmpty())
                                {
                                    return readings.First().Value.Get(dictionaryForm, conjugatedForm);
                                }

                                return readings.FirstOrDefault(r => r.Value == w.Link.Reading).Value;
                            });

                        return new WordMeaningDTO(
                            w.Content.Id.Value,
                            dictionaryForm,
                            conjugatedForm,
                            targetWriting,
                            w.Content.Translations.First(),
                            w.Link.WordInflectionId,
                            reading,
                            w.Link.Reading,
                            w.Link.WritingMode?.Value,
                            w.Link.CustomWriting);
                    }).ToArray())));
    }

    public record PhraseProjection(
        PhraseId Id,
        string Original,
        string[] Translations,
        string[] Contexts,
        AudioId[] Audios,
        WordLink[] WordMeanings);

    public record PhraseWithAudioLookup(
        PhraseId Id,
        string Original,
        string[] Translations,
        string[] Contexts,
        AudioId[] Audios,
        WordLink[] WordMeanings,
        Audio[]? AudiosJoined,
        WordMeaning[]? WordMeaningsJoined);

    //public record Group(PhraseId Id, );
}
