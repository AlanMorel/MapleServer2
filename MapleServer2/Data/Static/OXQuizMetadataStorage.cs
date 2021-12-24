using Maple2Storage.Types;
using MapleServer2.Types;
using Newtonsoft.Json;

namespace MapleServer2.Data.Static;

public static class OXQuizMetadataStorage
{
    private static readonly Dictionary<int, OXQuizQuestion> Questions = new();

    public static void Init()
    {
        string json = File.ReadAllText($"{Paths.JSON_DIR}/OXQuizQuestions.json");
        List<OXQuizQuestion> items = JsonConvert.DeserializeObject<List<OXQuizQuestion>>(json);
        foreach (OXQuizQuestion item in items)
        {
            Questions[item.Id] = item;
        }
    }

    public static OXQuizQuestion GetQuestion()
    {
        Random random = new();
        int index = random.Next(Questions.Count);
        return Questions[index];
    }
}
