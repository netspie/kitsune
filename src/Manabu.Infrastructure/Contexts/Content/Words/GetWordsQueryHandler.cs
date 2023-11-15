﻿using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.UseCases.Content.Words;
using Mediator;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Manabu.Infrastructure.CQRS.Content.Words;

public class GetWordsQueryHandler : IQueryHandler<GetWordsQuery, Result<GetWordsQueryResponse>>
{
    private const int MaxItemLimit = 20;

    private readonly MongoConnection _mongoConnection;

    public GetWordsQueryHandler(
        MongoConnection mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async ValueTask<Result<GetWordsQueryResponse>> Handle(GetWordsQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetWordsQueryResponse>.Success();

        var wordsCollection = _mongoConnection.Database.GetCollection<Word>(Word.DefaultCollectionName);
        var wordsFilter = Builders<Word>.Filter.Empty;

        var wordsHintDict = new Dictionary<string, object>();
       // if (!query.SortArgs.IsNullOrEmpty())
            //wordsHintDict.Add(nameof(Word.Value), 1);

        var wordsHint = new BsonDocument(wordsHintDict);

        var range = query.Range ?? new RangeArg(0, MaxItemLimit);
        var limit = Math.Min(range.Limit, MaxItemLimit);

        var filter = Builders<Word>.Filter.Empty;
        //if (!query.FilterArgs.IsNullOrEmpty())
        //    foreach (var arg in query.FilterArgs)
        //        filter &= Builders<Word>.Filter.Eq(x => x.PartsOfSpeech[0].Value, arg.Value);

        //bool wasSort;
        //foreach (var arg in query.Modifiers.ToArrayOrEmpty())
        //{
        //    if (arg.IsSort)
        //    {

        //    }
        //    else
        //    {

        //    }
        //    filter &= Builders<Word>.Filter.Eq(x => x.PartsOfSpeech[0].Value, arg.Value);
        //}

        var wordsProjection = Builders<Word>.Projection.Include(x => x.Id).Include(x => x.Value).Include(x => x.PartsOfSpeech).Include(x => x.Meanings);
        var totalCount = await wordsCollection.CountDocumentsAsync(filter);
        var words = await wordsCollection
            .Aggregate(new AggregateOptions() { Hint = wordsHint })
            .Match(filter)
            .Project(wordsProjection)
            .Skip(range.Start)
            .Lookup<WordMeaning, LookupResult>(
                foreignCollectionName: WordMeaning.DefaultCollectionName,
                localField: nameof(Word.Meanings),
                foreignField: "_id",
                @as: nameof(LookupResult.MeaningsJoined))
            .Limit(limit)
            .ToListAsync();

        return result.With(new GetWordsQueryResponse(
            new WordsDTO(
                Words: words.Select(w => new WordDTO(
                    w.Id.Value, 
                    w.Value,
                    w.MeaningsJoined?.FirstOrDefault()?.Translations?.FirstOrDefault(),
                    w.PartsOfSpeech?.FirstOrDefault()?.Value,
                    w.MeaningsJoined?.FirstOrDefault()?.HiraganaWritings?.FirstOrDefault()?.Value)).ToArray(),
                Range: new RangeDTO((int) totalCount, range.Start, limit))));
    }

    public record WordProjection(WordId Id, string Value, List<PartOfSpeech> PartsOfSpeech, WordMeaningId[] Meanings);
    public record LookupResult(WordId Id, string Value, List<PartOfSpeech> PartsOfSpeech, WordMeaningId[] Meanings, WordMeaning[] MeaningsJoined);
}
