using System.Collections.Generic;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class OXQuizParser : Exporter<List<OXQuizMetadata>>
    {
        public OXQuizParser() : base(null, "ox-quiz") { }

        protected override List<OXQuizMetadata> Parse()
        {
            List<OXQuizMetadata> questions = new List<OXQuizMetadata>();

            OXQuizMetadata q1 = new OXQuizMetadata()
            {
                Id = 1,
                Category = "Culture/Art",
                QuestionText = "\"A Furtive Tear (Una furtiva lagrima)\" is an aria that appears in Puccini's opera \"Madama Butterfly.\"",
                AnswerText = "\"A Furtive Tear (Una furtiva lagrima)\" is an aria that appears in Gaetano Donizetti's opera \"L'elisir d'amore.\"",
                Answer = false
            };
            questions.Add(q1);

            OXQuizMetadata q2 = new OXQuizMetadata()
            {
                Id = 2,
                Category = "Culture/Art",
                QuestionText = "\"Allegretto\" is the tempo that's in between adagio and allegretto, which in Italian means \"to walk.\"",
                AnswerText = "Andante is the tempo that's in between Adagio and Allegretto.",
                Answer = false
            };
            questions.Add(q2);

            OXQuizMetadata q3 = new OXQuizMetadata()
            {
                Id = 3,
                Category = "Society",
                QuestionText = "\"Angry consumers\" refer to consumers who exploit the system to gain personal benefits wrongfully.",
                AnswerText = "They are called \"abusive consumers.\"",
                Answer = false
            };
            questions.Add(q3);

            OXQuizMetadata q4 = new OXQuizMetadata()
            {
                Id = 4,
                Category = "Culture/Art",
                QuestionText = "\"Bolero\" is Igor Stravinsky's work.",
                AnswerText = "\"Bolero\" is Maurice Ravel's work.",
                Answer = false
            };
            questions.Add(q4);

            OXQuizMetadata q5 = new OXQuizMetadata()
            {
                Id = 5,
                Category = "Culture/Art",
                QuestionText = "\"Call Me Ishmael\" is the first line of the book \"Tom Sawyer\".",
                AnswerText = "\"Call Me Ishmael\" is the first line of the book \"Moby Dick\"."
            };
            questions.Add(q5);

            OXQuizMetadata q6 = new OXQuizMetadata()
            {
                Id = 6,
                Category = "Culture/Art",
                QuestionText = "\"Carmen\" is a symphony.",
                AnswerText = "\"Carmen\" is an opera.",
                Answer = false
            };
            questions.Add(q6);

            OXQuizMetadata q7 = new OXQuizMetadata()
            {
                Id = 7,
                Category = "Culture/Art",
                QuestionText = "\"Carmen\" is about a romance between a gypsy girl and a detective.",
                AnswerText = "\"Carmen\" is about a romance between a gypsy girl and a petty officer.",
                Answer = false
            };
            questions.Add(q7);

            return questions;
        }
    }
}
