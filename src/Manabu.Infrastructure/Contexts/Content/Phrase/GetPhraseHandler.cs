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
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace Manabu.Infrastructure.Contexts.Content.Phrases;

public class GetPhraseHandler : IQueryHandler<GetPhraseQuery, Result<GetPhraseQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;

    public GetPhraseHandler(
        MongoConnection mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetPhraseQueryResponse>> Handle(GetPhraseQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetPhraseQueryResponse>.Success();

        var collection = _mongoConnection.Database.GetCollection<Phrase>(Phrase.DefaultCollectionName);
        var queryable = collection.AsQueryable();

        var wmCollection = _mongoConnection.Database.GetCollection<WordMeaning>(WordMeaning.DefaultCollectionName);
        var wmQueryable = wmCollection.AsQueryable();

        //var a = queryable
        //    .Where(x => x.Id.Value == query.PhraseId)
        //    .Select(x => new 
        //    { 
        //        Id = x.Id,
        //        WordMeanings = x.WordMeanings
        //            .GroupJoin(
        //                wmQueryable,
        //                meaning => meaning.WordMeaningId,
        //                wordMeaning => wordMeaning.Id,
        //                (link, wordMeanings) => new { Id = link.WordMeaningId, Values = wordMeanings })
        //    });
            //.GroupJoin(wmQueryable, 
                //phrase => phrase.WordMeanings);

        var filter = Builders<Phrase>.Filter.Eq(p => p.Id.Value, query.PhraseId);
        var projection = Builders<Phrase>.Projection
            .Exclude(p => p.Id)
            .Include(p => p.Original)
            .Include(p => p.Translations)
            .Include(p => p.Contexts)
            .Include(p => p.Audios)
            .Include(p => p.WordMeanings);

        //var proj = Builders<BsonDocument>.Projection
        //    .Include(nameof(Phrase.Original))
        //    .Include(nameof(Phrase.Translations))
        //    .Include(nameof(Phrase.Contexts))
        //    .Include(nameof(Phrase.Audios))
        //    .Include(nameof(Phrase.WordMeanings))
        //    .Exclude("_id");

        //var phraseX = await collection
        //    .Aggregate()
        //    .Match(filter)
        //    .Unwind("WordMeanings")
        //    .Project(proj)
        //    .Lookup<WordMeaning, PhraseWithAudioLookup>(
        //        foreignCollectionName: WordMeaning.DefaultCollectionName,
        //        localField: "WordMeanings.WordMeaningId",
        //        foreignField: "_id",
        //        @as: nameof(PhraseWithAudioLookup.WordMeaningsJoined))
        //    //.Group()
        //    .ToListAsync();

        var phrase = await collection
            .Aggregate()
            .Match(filter)
            .Project(projection)
            .Lookup<Audio, PhraseWithAudioLookup>(
                foreignCollectionName: Audio.DefaultCollectionName,
                localField: nameof(Phrase.Audios),
                foreignField: "_id",
                @as: nameof(PhraseWithAudioLookup.AudiosJoined))
            //.Lookup<WordMeaning, PhraseWithAudioLookup>(
            //    foreignCollectionName: WordMeaning.DefaultCollectionName,
            //    localField: "WordMeanings.WordMeaningId",
            //    foreignField: "_id",
            //    @as: nameof(PhraseWithAudioLookup.WordMeaningsJoined))
             .FirstOrDefaultAsync();

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
                    phrase.AudiosJoined!.SelectOrEmpty(a => new AudioDTO(a.Id.Value, a.Href)).ToArray())));
    }

    public record PhraseProjection(
        string Original,
        string[] Translations,
        string[] Contexts,
        AudioId[] Audios,
        WordLink[] WordMeanings);

    public record PhraseWithAudioLookup(
        string Original,
        string[] Translations,
        string[] Contexts,
        AudioId[] Audios,
        WordLink[] WordMeanings,
        Audio[]? AudiosJoined,
        WordMeaning[]? WordMeaningsJoined);

    //public record Group(PhraseId Id, );
}
