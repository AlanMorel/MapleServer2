namespace MapleServer2.Types
{
    public class OXQuizQuestion
    {
        public int Id;
        public string Category;
        public string QuestionText;
        public string AnswerText;
        public bool Answer;

        public OXQuizQuestion() { }

        public OXQuizQuestion(int id, string category, string questionText, string answerText, bool answer)
        {
            Id = id;
            Category = category;
            QuestionText = questionText;
            AnswerText = answerText;
            Answer = answer;
        }
    }
}
