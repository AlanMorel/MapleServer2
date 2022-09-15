using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseOXQuizQuestion : DatabaseTable
{
    public DatabaseOXQuizQuestion() : base("ox_quiz_questions") { }

    public OXQuizQuestion GetRandomQuestion()
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get().ToList();
        dynamic singleResult = result.ElementAt(Random.Shared.Next(result.Count()));
        return ReadQuestion(singleResult);
    }

    private OXQuizQuestion ReadQuestion(dynamic data)
    {
        return new(data.category, data.question_text, data.answer_text, data.answer);
    }
}
