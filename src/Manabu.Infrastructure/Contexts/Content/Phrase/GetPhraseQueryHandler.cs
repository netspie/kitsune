using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Content.WordMeanings;
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
                (link, wordMeanings) => new { Link = link, WordMeaning = wordMeanings.First() })
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
                        new WordMeaningDTO(
                            w.WordMeaning.Id.Value,
                            w.WordMeaning.Original,
                            w.WordMeaning.Translations.First(),
                            w.Link.WordInflectionId,
                            w.Link.Reading,
                            w.Link.WritingMode?.Value,
                            w.Link.CustomWriting)).ToArray())));
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
