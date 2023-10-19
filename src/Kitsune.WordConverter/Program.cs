using Corelibs.Basic.Collections;
using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Manabu.Entities.Rehearse.RehearseItems;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using WanikaniTest.Models;

string jsonFilePath = "../../../words.json";
string jsonString = File.ReadAllText(jsonFilePath);

var words = JsonConvert.DeserializeObject<VocabularyItemDTO[]>(jsonString);

var Auxiliary_MeaningsMoreThan0 = words.Where(w => w.Data.Auxiliary_Meanings.Length > 0).ToList();
var multipleMeaningAndReadingWords = words.Where(w => w.Data.Readings.Length > 1).ToList();
var wordsGroupedByMeaningsLength = words.GroupBy(w => w.Data.Meanings.Length).OrderBy(w => w.Key).ToList();

var partOfSpeechesNames = File.ReadAllLines("../../../partOfSpeeches.txt");
var wordsByPartOfSpeeches = new List<WordCluster>();
foreach (var partOfSpeech in partOfSpeechesNames)
    wordsByPartOfSpeeches.Add(new(partOfSpeech, words.Where(w => w.Data.Parts_Of_Speech.Contains(partOfSpeech)).ToArray()));

var godanVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "godan verb");
var transitiveVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "transitive verb");
var intransitiveVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "intransitive verb");
var ichidanVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "ichidan verb");
var suruVerbs = wordsByPartOfSpeeches.FirstOrDefault(p => p.Type == "する verb");

var allVerbs = godanVerbs.Items.Concat(
    transitiveVerbs.Items.Concat(
        intransitiveVerbs.Items.Concat(
            ichidanVerbs.Items.Concat(
                suruVerbs.Items))));

var allVerbsDistinct = allVerbs.Distinct();

var mainVerbs = godanVerbs.Items.Concat(ichidanVerbs.Items.Concat(suruVerbs.Items));
var transitiveVerbsRest = transitiveVerbs.Items.Where(v => !mainVerbs.Contains(v)).ToArray();
var intransitiveVerbsRest = intransitiveVerbs.Items.Where(v => !mainVerbs.Contains(v)).ToArray();

Console.WriteLine($"{mainVerbs.Count()} x verbs");
Console.WriteLine($"{transitiveVerbsRest.Length} transitive verbs verbs");
Console.WriteLine($"{intransitiveVerbsRest.Length} intransitive verbs verbs");
Console.WriteLine($"{allVerbsDistinct.Count()} distinct verbs");

var mongoConnection = new MongoConnection("Kitsune_dev");
var conn = Environment.GetEnvironmentVariable("KitsuneDatabaseConn");

var client = new MongoClient(conn);
var database = client.GetDatabase("Kitsune_dev");

var wordCol = database.GetCollection<Word>(Word.DefaultCollectionName);
var wordMeaningsCol = database.GetCollection<WordMeaning>(WordMeaning.DefaultCollectionName);

var mm = mainVerbs.Where(m => m.Data.Readings.Length > 1).ToArray();
foreach (var verb in mainVerbs)
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

        // VERBS
        if (strs.Any(str => str.Contains("verb")))
            partsOfSpeech.Add(PartOfSpeech.Verb);

        if (strs.Any(str => str.Contains("godan")))
            properties.Add(VerbConjugationType.Godan);
        else
        if (strs.Any(str => str.Contains("ichidan")))
            properties.Add(VerbConjugationType.Ichidan);
        else
        if (strs.Any(str => str.Contains("する")))
            properties.Add(VerbConjugationType.Suru);
        else
            properties.Add(VerbConjugationType.Irregular);

        if (strs.Any(str => str.Contains("transitive")))
            properties.Add(VerbTransitivity.Transitive);
        else
        if (strs.Any(str => str.Contains("intransitive")))
            properties.Add(VerbTransitivity.Intransitive);

        return (partsOfSpeech, properties);
    }
}
