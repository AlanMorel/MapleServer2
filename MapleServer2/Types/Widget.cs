using Maple2.Trigger.Enum;

namespace MapleServer2.Types
{
    public class Widget
    {
        public int Id;
        public WidgetType Type;
        public string State;
        public string Arg; // unclear what this is
        public OXQuizQuestion OXQuizQuestion;

        public Widget(WidgetType type)
        {
            Type = type;
            Arg = "";
        }
    }

    public class OXQuizQuestion
    {
        public string Category;
        public string QuestionText;
        public string AnswerText;
        public bool Answer;

        public OXQuizQuestion(string category, string questionText, string answerText, bool answer)
        {
            Category = category;
            QuestionText = questionText;
            AnswerText = answerText;
            Answer = answer;
        }
    }
}
