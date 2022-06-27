namespace MapleServer2.Database.Types;

public class OXQuizQuestion
{
    public readonly string Category;
    public readonly string QuestionText;
    public readonly string AnswerText;
    public readonly bool Answer;

    public OXQuizQuestion() { }

    public OXQuizQuestion(string category, string questionText, string answerText, bool answer)
    {
        Category = category;
        QuestionText = questionText;
        AnswerText = answerText;
        Answer = answer;
    }
}
