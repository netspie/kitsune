using Corelibs.Basic.Collections;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using WanikaniTest.Models;
using MongoDB.Bson.Serialization;
using Kitsune.WordConverter;
using Manabu.Entities.Content.WordLexemes;

BsonClassMap.RegisterClassMap<VerbTransitivity>();
BsonClassMap.RegisterClassMap<VerbConjugationType>();
BsonClassMap.RegisterClassMap<AdjectiveConjugationType>();
BsonClassMap.RegisterClassMap<NounType>();

BsonClassMap.RegisterClassMap<Age>();
BsonClassMap.RegisterClassMap<Gender>();
BsonClassMap.RegisterClassMap<Dialect>();
BsonClassMap.RegisterClassMap<Formality>();

//string jsonFilePath = "../../../words.json";
//string jsonString = File.ReadAllText(jsonFilePath);

//var words = JsonConvert.DeserializeObject<VocabularyItemDTO[]>(jsonString);

//var Auxiliary_MeaningsMoreThan0 = words.Where(w => w.Data.Auxiliary_Meanings.Length > 0).ToList();
//var multipleMeaningAndReadingWords = words.Where(w => w.Data.Readings.Length > 1).ToList();
//var wordsGroupedByMeaningsLength = words.GroupBy(w => w.Data.Meanings.Length).OrderBy(w => w.Key).ToList();

//var partOfSpeechesNames = File.ReadAllLines("../../../partOfSpeeches.txt");
//var wordsByPartOfSpeeches = new List<WordCluster>();
//foreach (var partOfSpeech in partOfSpeechesNames)
//    wordsByPartOfSpeeches.Add(new(partOfSpeech, words.Where(w => w.Data.Parts_Of_Speech.Contains(partOfSpeech)).ToArray()));

var conn = "mongodb://localhost:27017/";
var client = new MongoClient(conn);
var database = client.GetDatabase("Kitsune_dev");
var wordCollection = database.GetCollection<Word>(Word.DefaultCollectionName);

var wordsFromDb = await wordCollection.Aggregate().ToListAsync();
var godanVerbs = wordsFromDb.Where(w => w.PartsOfSpeech is not null && w.Properties is not null && w.PartsOfSpeech.Contains(PartOfSpeech.Verb) && w.Properties.Contains(VerbConjugationType.Godan)).ToArray();
var ichidanVerbs = wordsFromDb.Where(w => w.PartsOfSpeech is not null && w.Properties is not null && w.PartsOfSpeech.Contains(PartOfSpeech.Verb) && w.Properties.Contains(VerbConjugationType.Ichidan)).ToArray();
return;

int i = 0;
foreach (var verb in godanVerbs)
{
    var inflections = Conjugator.ConjugateVerb(verb.Value, VerbConjugationType.Godan);
    if (inflections is null)
        continue;

    var wordLexemeIdStr = IdCreator.Create();
    var wordLexeme = new WordLexeme(new WordLexemeId(wordLexemeIdStr), verb.PartsOfSpeech.FirstOrDefault(), verb.Value, inflections);
    
    verb.Lexeme = wordLexeme.Id;
    var wordCol = database.GetCollection<Word>(Word.DefaultCollectionName);
    await wordCol.ReplaceOneAsync(Builders<Word>.Filter.Eq(x => x.Id, verb.Id), verb);

    var wordLexemeCol = database.GetCollection<WordLexeme>(WordLexeme.DefaultCollectionName);
    await wordLexemeCol.InsertOneAsync(wordLexeme);
    i++;
    Console.WriteLine($"{i}) {verb.Value}");
}

foreach (var verb in ichidanVerbs)
{
    var inflections = Conjugator.ConjugateVerb(verb.Value, VerbConjugationType.Ichidan);
    if (inflections is null)
        continue;

    var wordLexemeIdStr = IdCreator.Create();
    var wordLexeme = new WordLexeme(new WordLexemeId(wordLexemeIdStr), verb.PartsOfSpeech.FirstOrDefault(), verb.Value, inflections);

    verb.Lexeme = wordLexeme.Id;
    var wordCol = database.GetCollection<Word>(Word.DefaultCollectionName);
    await wordCol.ReplaceOneAsync(Builders<Word>.Filter.Eq(x => x.Id, verb.Id), verb);

    var wordLexemeCol = database.GetCollection<WordLexeme>(WordLexeme.DefaultCollectionName);
    await wordLexemeCol.InsertOneAsync(wordLexeme);
    // ...
    i++;
    Console.WriteLine($"{i}) {verb.Value}");
}

return;

//var wordEnts = await wordCollection.Aggregate().ToListAsync();
//var verbs = wordEnts
//    .Where(w => 
//        (!w.PartsOfSpeech.IsNullOrEmpty() && w.PartsOfSpeech.Contains(PartOfSpeech.Verb)) && 
//        (!w.Properties.IsNullOrEmpty() && !w.Properties.Contains(VerbConjugationType.Suru)))
//    .Select(w => w.Value)
//    .ToArray();

//File.WriteAllLines("../../../verbs.txt", verbs);
// --- VERBS ---
//var godanVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "godan verb");
//var transitiveVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "transitive verb");
//var intransitiveVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "intransitive verb");
//var ichidanVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "ichidan verb");
//var suruVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "する verb");

//var allVerbs = godanVerbs.Items.Concat(
//    transitiveVerbs.Items.Concat(
//        intransitiveVerbs.Items.Concat(
//            ichidanVerbs.Items.Concat(
//                suruVerbs.Items))));

//var allVerbsDistinct = allVerbs.Distinct();

//var mainVerbs = godanVerbs.Items.Concat(ichidanVerbs.Items.Concat(suruVerbs.Items));
//var transitiveVerbsRest = transitiveVerbs.Items.Where(v => !mainVerbs.Contains(v)).ToArray();
//var intransitiveVerbsRest = intransitiveVerbs.Items.Where(v => !mainVerbs.Contains(v)).ToArray();

//await Store(mainVerbs);

// --- NUMERALS ---
//var numerals = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "numeral");
//await Store(numerals.Items);
//var posDict = new Dictionary<string, List<VocabularyItemDataDTO>>();
//// --- NOUNS ---
//var nouns = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "noun").Items.Where(i => 
//{
//    foreach (var p in i.Data.Parts_Of_Speech)
//        if (p != "noun")
//            if (!posDict.TryAdd(p, new() { i.Data }))
//                posDict[p].Add(i.Data);

//    var ps = i.Data.Parts_Of_Speech;
//    if (ps.Length == 1)
//        return true;

//    if (!ps.Contains("な adjective") ||
//        !ps.Contains("の adjective") ||
//        !ps.Contains("verbal noun"))
//        return false;

//    return true;

//}).ToArray();
//var nounsProper = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "proper noun").Items.Where(i => !i.Data.Parts_Of_Speech.Contains("noun")).ToArray();
//var nounsVerbal  = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "verbal noun").Items.Where(i => !i.Data.Parts_Of_Speech.Contains("noun")).ToArray();

//await Store(nouns);

//var posDict = new Dictionary<string, List<VocabularyItemDataDTO>>();
//var adjectivesI = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "い adjective")?.Items;
//var adjectivesNa = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "な adjective")?.Items;
//var adjectivesNo = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "の adjective")?.Items;
//var adjectivesR = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "adjective")?.Items;

//var adjectivesAll = adjectivesI.Concat(adjectivesNa.Concat(adjectivesNo.Concat(adjectivesR))).ToArray();

//var adjectives = adjectivesAll.Where(i =>
//{
//    foreach (var p in i.Data.Parts_Of_Speech)
//        if (p != "adjective")
//            if (!posDict.TryAdd(p, new() { i.Data }))
//                posDict[p].Add(i.Data);

//    var ps = i.Data.Parts_Of_Speech;

//    if (ps.Contains("noun"))
//        return false;

//    if (!ps.Contains("な adjective") &&
//        !ps.Contains("い adjective"))
//        return false;

//    return true;

//}).ToArray();

//await Store(adjectives);

async static Task Store(IEnumerable<VocabularyItemDTO> words)
{
    var conn = "mongodb://localhost:27017/";

    var client = new MongoClient(conn);
    var database = client.GetDatabase("Kitsune_dev");

    var wordCol = database.GetCollection<Word>(Word.DefaultCollectionName);
    var wordMeaningsCol = database.GetCollection<WordMeaning>(WordMeaning.DefaultCollectionName);

    var mm = words.Where(m => m.Data.Readings.Length > 1).ToArray();

    foreach (var verb in words)
    {
        var meaning = verb.Data.Meanings.Where(m => m.Primary == true).FirstOrDefault();
        if (meaning is null)
            continue;

        var reading = verb.Data.Readings.Where(m => m.Primary == true).FirstOrDefault();
        if (reading is null)
            continue;

        var wordIdStr = IdCreator.Create();
        var wordMeaningIdStr = IdCreator.Create();

        var wordMeaning = new WordMeaning(
            id: new WordMeaningId(wordMeaningIdStr),
            wordId: new WordId(wordIdStr),
            original: verb.Data.Slug,
            translations: new List<string> { meaning.Meaning.ToLower() },
            hiraganaWritings: new() { new WordMeaning.HiraganaWriting(reading.Reading) });

        var partsOfSpeeches = verb.Data.Parts_Of_Speech.ToPartsOfSpeech();
        var word = new Word(
            id: new WordId(wordIdStr),
            value: verb.Data.Slug,
            partsOfSpeech: partsOfSpeeches.partOfSpeeches,
            meanings: new() { wordMeaning.Id },
            properties: partsOfSpeeches.properties,
            lexeme: null);

        try
        {
            await wordCol.InsertOneAsync(word);
            Console.WriteLine($"Written word: {wordMeaning.Translations[0]}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing word: {wordMeaning.Translations[0]}\n");
        }

        try
        {
            await wordMeaningsCol.InsertOneAsync(wordMeaning);
            Console.WriteLine($"Written word meaning: {wordMeaning.Translations[0]}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing word meaning: {wordMeaning.Translations[0]}\n");
        }
    }

    Console.WriteLine("Saved All");
}

public record WordCluster(string Type, VocabularyItemDTO[] Items)
{
    public override string ToString() => Type;
}

public static class IdCreator
{
    public static string Create() =>
        Regex.Replace(
            Convert.ToBase64String(
                Guid.NewGuid().ToByteArray()), 
            "[^a-zA-Z0-9]", 
            "");
}

public static class PartOfSpeechExtensions
{
    public static (List<PartOfSpeech> partOfSpeeches, List<WordProperty> properties) ToPartsOfSpeech(this string[] strs)
    {
        var partsOfSpeech = new List<PartOfSpeech>();
        var properties = new List<WordProperty>();

        //// VERBS
        //if (strs.Any(str => str.Contains("verb")))
        //    partsOfSpeech.Add(PartOfSpeech.Verb);

        //if (strs.Any(str => str.Contains("godan")))
        //    properties.Add(VerbConjugationType.Godan);
        //else
        //if (strs.Any(str => str.Contains("ichidan")))
        //    properties.Add(VerbConjugationType.Ichidan);
        //else
        //if (strs.Any(str => str.Contains("する")))
        //    properties.Add(VerbConjugationType.Suru);
        //else
        //    properties.Add(VerbConjugationType.Irregular);

        //if (strs.Any(str => str.Contains("transitive")))
        //    properties.Add(VerbTransitivity.Transitive);
        //else
        //if (strs.Any(str => str.Contains("intransitive")))
        //    properties.Add(VerbTransitivity.Intransitive);

        //// NUMERALS
        //if (strs.Any(str => str.Contains("numeral")))
        //    partsOfSpeech.Add(PartOfSpeech.Numeral);

        // ADJ
        if (strs.Any(str => str.Contains("adjective")))
            partsOfSpeech.Add(PartOfSpeech.Adjective);

        if (strs.Any(str => str.Contains("な adjective")))
            properties.Add(AdjectiveConjugationType.Na);
        if (strs.Any(str => str.Contains("の adjective")))
            properties.Add(AdjectiveConjugationType.No);
        if (strs.Any(str => str.Contains("い adjective")))
            properties.Add(AdjectiveConjugationType.I);

        return (partsOfSpeech, properties);
    }
}
