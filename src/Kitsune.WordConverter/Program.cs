using Corelibs.MongoDB;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using MongoDB.Driver;
using Newtonsoft.Json;
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
var mongoClient = new MongoClient(conn);
mongoConnection.Database = mongoClient.GetDatabase(mongoConnection.DatabaseName);
mongoConnection.Session = await mongoClient.StartSessionAsync();

var wordRepo = new MongoDbRepository<Word, WordId>(mongoConnection, Word.DefaultCollectionName);
var wordMeaningRepo = new MongoDbRepository<WordMeaning, WordMeaningId>(mongoConnection, WordMeaning.DefaultCollectionName);

var mm = mainVerbs.Where(m => m.Data.Readings.Length > 1).ToArray();
foreach (var verb in mainVerbs)
{
    var meaning = verb.Data.Meanings.Where(m => m.Primary == true).FirstOrDefault();
    if (meaning is null)
        continue;

    var reading = verb.Data.Meanings.Where(m => m.Primary == true).FirstOrDefault();
    if (reading is null)
        continue;

    var wordMeaningIdStr = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    var wordMeaning = new WordMeaning(
        id: new WordMeaningId(wordMeaningIdStr),
        original: verb.Data.Slug,
        translations: new List<string> { meaning.Meaning.ToLower() }, 
        personas: null,
        hiraganaWritings: new() { new WordMeaning.HiraganaWriting("reading") });

    var partsOfSpeeches = verb.Data.Parts_Of_Speech.ToPartsOfSpeech();
    var wordIdStr = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    var word = new Word(
        id: new WordId(wordIdStr),
        value: verb.Data.Slug,
        partsOfSpeech: partsOfSpeeches.partOfSpeeches,
        meanings: new() { wordMeaning.Id },
        properties: partsOfSpeeches.properties,
        lexeme: null);

    //var word = new Word(new WordId(base64Guid), 0, verb.Data.Slug, PartOfSpeech.Verb,)
}

public record WordCluster(string Type, VocabularyItemDTO[] Items)
{
    public override string ToString() => Type;
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
