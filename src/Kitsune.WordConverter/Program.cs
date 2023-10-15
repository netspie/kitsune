using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Diagnostics;
using WanikaniTest.Models;

string jsonFilePath = "../../../words.json";
string jsonString = File.ReadAllText(jsonFilePath);

var words = JsonConvert.DeserializeObject<VocabularyItemDTO[]>(jsonString);

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
var mongoClient = new MongoClient(conn);
mongoConnection.Database = mongoClient.GetDatabase(mongoConnection.DatabaseName);
mongoConnection.Session = await mongoClient.StartSessionAsync();

var wordRepo = new MongoDbRepository<Word, WordId>(mongoConnection, Word.DefaultCollectionName);
var wordMeaningRepo = new MongoDbRepository<WordMeaning, WordMeaningId>(mongoConnection, WordMeaning.DefaultCollectionName);

var mm = mainVerbs.Where(m => m.Data.Readings.Length > 1).ToArray();
foreach (var verb in mainVerbs)
{
    var wordIdStr = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    foreach (var meaning in verb.Data.Meanings)
    {
        var wordMeaningIdStr = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var wordMeaning = new WordMeaning(new WordMeaningId(wordMeaningIdStr), 0, verb.Data.Slug, new List<string> { meaning.Meaning.ToLower() });
    }
    //var word = new Word(new WordId(base64Guid), 0, verb.Data.Slug, PartOfSpeech.Verb,)
}

public record WordCluster(string Type, VocabularyItemDTO[] Items)
{
    public override string ToString() => Type;
}

public static class PartOfSpeechExtensions
{
    public static PartOfSpeech? ToPartOfSpeech(this string str) => str switch
    {
        _ => null
    };
}
