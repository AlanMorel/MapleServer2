using MapleServer2.Constants;
using MapleServer2.Types;
using Newtonsoft.Json;

namespace MapleServer2.Data.Static
{
    public static class OXQuizMetadataStorage
    {
        private static readonly Dictionary<int, OXQuizQuestion> questions = new Dictionary<int, OXQuizQuestion>();

        public static void Init()
        {
            string json = File.ReadAllText($"{Paths.JSON}/OXQuizQuestions.json");
            List<OXQuizQuestion> items = JsonConvert.DeserializeObject<List<OXQuizQuestion>>(json);
            foreach (OXQuizQuestion item in items)
            {
                questions[item.Id] = item;
            }
        }

        public static OXQuizQuestion GetQuestion()
        {
            Random random = new Random();
            int index = random.Next(questions.Count);
            return questions[index];
        }
    }
}
