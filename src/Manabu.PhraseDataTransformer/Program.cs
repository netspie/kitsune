//// See https://aka.ms/new-console-template for more information
//using Corelibs.MongoDB;
//using Corelibs.Basic.Blocks;
//using LangGrinder.Infrastructure.Data;
//using Manabu.Entities.Audios;
//using Manabu.Entities.Conversations;
//using Manabu.Entities.Courses;
//using Manabu.Entities.Lessons;
//using Manabu.Entities.Phrases;
//using Manabu.Entities.Users;
//using MongoDB.Driver;
//using System.Text.Json;
//using Corelibs.Basic.Repository;

//string phrasesDir = @"C:\PhrasesDirectory"; // Replace with your phrases JSON directory
//string audioDir = @"C:\AudioDirectory"; // Replace with your audio files directory

//var conversationsData = ReadJsonFiles<ConversationsData>(
//    "D:\\git\\LangGrinder\\LangGrinder.Unity\\Assets\\Resources\\Jsons\\Conversations");

//// Load PhrasesData from JSON
//var phrasesData = ReadJsonFiles<PhrasesData>(
//    "D:\\git\\LangGrinder\\LangGrinder.Unity\\Assets\\Resources\\Jsons\\Phrases")
//    .SelectMany(p => p.Phrases)
//    .Select((p, i) => new { phrase = p, i })
//    .ToDictionary(kv => kv.phrase.Id);

//var mongoConnection = new MongoConnection("HackStudy_dev");
//var conn = Environment.GetEnvironmentVariable("HackStudyDatabaseConn");
//var mongoClient = new MongoClient(conn);
//mongoConnection.Database = mongoClient.GetDatabase(mongoConnection.DatabaseName);
//mongoConnection.Session = await mongoClient.StartSessionAsync();

//var courseRepo = new MongoDbRepository<Course, CourseId>(mongoConnection, Course.DefaultCollectionName);
//var lessonRepo = new MongoDbRepository<Lesson, LessonId>(mongoConnection, Lesson.DefaultCollectionName);
//var conversationRepo = new MongoDbRepository<Conversation, ConversationId>(mongoConnection, Conversation.DefaultCollectionName);
//var phraseRepo = new MongoDbRepository<Phrase, PhraseId>(mongoConnection, Phrase.DefaultCollectionName);
//var audioRepo = new MongoDbRepository<Audio, AudioId>(mongoConnection, Audio.DefaultCollectionName);

////mongoConnection.Session.StartTransaction();

//var userId = new UserId("f2a533eb-7c51-44d2-86e6-ce06dc806c73");

//var result = Result.Success();
//var course = new Course("Yuta Vocabulary", userId);
//course.AddModule("Level 1");

//foreach (var conversationContainer in conversationsData)
//{
//    var conversationData = conversationContainer.conversations[0];

//    var lesson = new Lesson(conversationData.Title, course.Id, userId);
//    if (!course.AddLesson(lesson.Id, 0, course.Modules[0].LessonIds.Count))
//    {
//        Console.WriteLine($"Could not add lesson {lesson.Id} to the course.");
//        return;
//    }


//    var conversation = new Conversation(conversationData.Title, lesson.Id, userId);
//    lesson.AddConversation(conversation.Id);

//    await lessonRepo.Save(lesson);
//    Console.WriteLine($"Lesson {lesson.Name} saved");

//    foreach (var phraseId in conversationData.PhraseIds)
//    {
//        var phraseData = phrasesData[phraseId];

//        var phrase = new Phrase(
//            userId,
//            original: phraseData.phrase.TranslatedPhrase,
//            conversation: conversation.Id);

//        phrase.Translations = new() {
//            phraseData.phrase.OriginalPhrase
//        };

//        var audio = new Audio($"{phraseData.phrase.Id}.mp3");
//        await audioRepo.Save(audio);

//        phrase.Audios = new() {
//            audio.Id
//        };

//        conversation.AddPhrase(phrase.Id);

//        await phraseRepo.Save(phrase);
//        Console.WriteLine($"Phrase {phrase.Original} saved. Nr {phraseData.i}.");
//    }

//    await conversationRepo.Save(conversation);
//    Console.WriteLine($"Conversation {conversation.Name} saved");
//}

//await courseRepo.Save(course);
//Console.WriteLine($"Course {course.Name} saved");
////mongoConnection.Session.CommitTransaction();
//Console.ReadKey();

//static T[] ReadJsonFiles<T>(string dir)
//{
//    string[] fileNames = Directory.GetFiles(dir, "*.json");

//    JsonSerializerOptions options = new JsonSerializerOptions
//    {
//        PropertyNameCaseInsensitive = true // Enable case-insensitive deserialization
//    };

//    return fileNames.Select(fn =>
//    {
//        try
//        {
//            string json = File.ReadAllText(fn);
//            return JsonSerializer.Deserialize<T>(json, options);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error reading {fn}: {ex.Message}");
//            return default(T);
//        }
//    })
//    .Where(obj => obj != null)
//    .ToArray();
//}
