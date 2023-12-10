using Manabu.Entities.Content.Words;
using MongoDB.Driver;
using Newtonsoft.Json;
using WanikaniTest.Models;

namespace Kitsune.WordConverter;

internal class GetWordsByLessons
{
    public void Get() 
    {
        string jsonFilePath = "../../../../../../../words.json";
        string jsonString = File.ReadAllText(jsonFilePath);

        var wordDTOs = JsonConvert.DeserializeObject<VocabularyItemDTO[]>(jsonString);

        var conn = "";
        var client = new MongoClient(conn);
        var database = client.GetDatabase("Kitsune_dev");
        var wordCollection = database.GetCollection<Word>(Word.DefaultCollectionName);
        var wordQueryable = wordCollection.AsQueryable();

        int i = 1;
        foreach (var wordDTO in wordDTOs)
        {
            var word = wordQueryable.Where(word => word.Value == wordDTO.Data.Slug).FirstOrDefault();
            if (word is null)
            {
                Console.WriteLine($"{i} missing!");
                continue;
            }

            word.Level = wordDTO.Data.Level;
            wordCollection.ReplaceOne(Builders<Word>.Filter.Eq(x => x.Id, word.Id), word);

            Console.WriteLine($"Word {i} updated");
            i++;
        }
    }
}
