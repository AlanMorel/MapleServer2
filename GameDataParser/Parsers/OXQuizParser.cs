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
                QuestionText = "\"A Furtive Tear (Una furtiva lagrima)\" is an aria that appears in Puccini's opera \"Madama Butterfly.\"",
                Answer = false,
                AnswerText = "\"A Furtive Tear (Una furtiva lagrima)\" is an aria that appears in Gaetano Donizetti's opera \"L'elisir d'amore.\"",
                Category = "Culture/Art"
            };
            questions.Add(q1);

            OXQuizMetadata q2 = new OXQuizMetadata()
            {
                Id = 2,
                QuestionText = "\"Allegretto\" is the tempo that's in between adagio and allegretto, which in Italian means \"to walk.\"",
                Answer = false,
                AnswerText = "Andante is the tempo that's in between Adagio and Allegretto.",
                Category = "Culture/Art"
            };
            questions.Add(q2);

            OXQuizMetadata q3 = new OXQuizMetadata()
            {
                Id = 3,
                QuestionText = "\"Angry consumers\" refer to consumers who exploit the system to gain personal benefits wrongfully.",
                Answer = false,
                AnswerText = "They are called \"abusive consumers.\"",
                Category = "Society"
            };
            questions.Add(q3);

            OXQuizMetadata q4 = new OXQuizMetadata()
            {
                Id = 4,
                QuestionText = "\"Bolero\" is Igor Stravinsky's work.",
                Answer = false,
                AnswerText = "\"Bolero\" is Maurice Ravel's work.",
                Category = "Culture/Art"
            };
            questions.Add(q4);

            OXQuizMetadata q5 = new OXQuizMetadata()
            {
                Id = 5,
                QuestionText = "\"Call Me Ishmael\" is the first line of the book \"Tom Sawyer\".",
                Answer = false,
                AnswerText = "\"Call Me Ishmael\" is the first line of the book \"Moby Dick\".",
                Category = "Culture/Art"
            };
            questions.Add(q5);

            OXQuizMetadata q6 = new OXQuizMetadata()
            {
                Id = 6,
                QuestionText = "\"Carmen\" is a symphony.",
                Answer = false,
                AnswerText = "\"Carmen\" is an opera.",
                Category = "Culture/Art"
            };
            questions.Add(q6);

            OXQuizMetadata q7 = new OXQuizMetadata()
            {
                Id = 7,
                QuestionText = "\"Carmen\" is about a romance between a gypsy girl and a detective.",
                Answer = false,
                AnswerText = "\"Carmen\" is about a romance between a gypsy girl and a petty officer.",
                Category = "Culture/Art"
            };
            questions.Add(q7);

            OXQuizMetadata q8 = new OXQuizMetadata()
            {
                Id = 8,
                QuestionText = "\"Carmen\" is about a romance between a gypsy girl and a petty officer.",
                Answer = true,
                AnswerText = "\"Carmen\" is about a romance between a gypsy girl and a petty officer.",
                Category = "Culture/Art"
            };
            questions.Add(q8);

            OXQuizMetadata q9 = new OXQuizMetadata()
            {
                Id = 9,
                QuestionText = "\"Carmen\" is an opera.",
                Answer = true,
                AnswerText = "\"Carmen\" is an opera.",
                Category = "Culture/Art"
            };
            questions.Add(q9);

            OXQuizMetadata q10 = new OXQuizMetadata()
            {
                Id = 10,
                QuestionText = "\"Carnivals of the Animals\" is a work by Gabrielle Faure.",
                Answer = false,
                AnswerText = "\"Carnivals of the Animals\" is a work by Camille Saint Saens.",
                Category = "Culture/Art"
            };
            questions.Add(q10);

            OXQuizMetadata q11 = new OXQuizMetadata()
            {
                Id = 11,
                QuestionText = "\"Chorus,\" which means \"a church or in the manner of the chapel\" refers to a song that is unaccompanied by instruments.",
                Answer = false,
                AnswerText = "A capella refers to singing without instrumental accompaniment.",
                Category = "Culture/Art"
            };
            questions.Add(q11);

            OXQuizMetadata q12 = new OXQuizMetadata()
            {
                Id = 12,
                QuestionText = "\"Crime and Punishment\" is Tolstoy's work.",
                Answer = false,
                AnswerText = "\"Crime and Punishment\" is Fydor Dostoevsky's work.",
                Category = "Culture/Art"
            };
            questions.Add(q12);

            OXQuizMetadata q13 = new OXQuizMetadata()
            {
                Id = 13,
                QuestionText = "\"Die Forelle\" is a work by Franz Schubert.",
                Answer = true,
                AnswerText = "\"Die Forelle\" is a work by Franz Schubert.",
                Category = "Culture/Art"
            };
            questions.Add(q13);

            OXQuizMetadata q14 = new OXQuizMetadata()
            {
                Id = 14,
                QuestionText = "\"Die Forelle\" is a work by Mozart.",
                Answer = false,
                AnswerText = "\"Die Forelle\" is a work by Franz Schubert.",
                Category = "Culture/Art"
            };
            questions.Add(q14);

            OXQuizMetadata q15 = new OXQuizMetadata()
            {
                Id = 15,
                QuestionText = "\"Don Giovanni\" is an opera written by Beethoven.",
                Answer = false,
                AnswerText = "\"Don Giovanni\" is an opera written by Mozart.",
                Category = "Culture/Art"
            };
            questions.Add(q15);

            OXQuizMetadata q16 = new OXQuizMetadata()
            {
                Id = 16,
                QuestionText = "\"Erlkonig\" is a composition by Beethoven.",
                Answer = false,
                AnswerText = "\"Erlkönig\" is a composition by Giuseppe Verdi.",
                Category = "Culture/Art"
            };
            questions.Add(q16);

            OXQuizMetadata q17 = new OXQuizMetadata()
            {
                Id = 17,
                QuestionText = "\"Erlkonig\" is a poem by Wolfgang Goethe.",
                Answer = true,
                AnswerText = "\"Erlkönig\" is a poem by Wolfgang Goethe.",
                Category = "Culture/Art"
            };
            questions.Add(q17);

            OXQuizMetadata q18 = new OXQuizMetadata()
            {
                Id = 18,
                QuestionText = "\"Eskimo,\" which refers to a member of an indigenous people inhabiting the South Pole, means \"the eaters of raw meat.\"",
                Answer = false,
                AnswerText = "\"Eskimo,\" which refers to a member of an indigenous people inhabiting northern Alaska, means \"the eaters of raw meat.\"",
                Category = "General Knowledge"
            };
            questions.Add(q18);

            OXQuizMetadata q19 = new OXQuizMetadata()
            {
                Id = 19,
                QuestionText = "\"Eurydike\" is the oldest opera known to humankind.",
                Answer = true,
                AnswerText = "\"Eurydike\" is the oldest opera known to humankind.",
                Category = "Culture/Art"
            };
            questions.Add(q19);

            OXQuizMetadata q20 = new OXQuizMetadata()
            {
                Id = 20,
                QuestionText = "\"Four Seasons\" is the work of Wolfgang Mozart.",
                Answer = false,
                AnswerText = "\"Four Seasons\" is the work of Antonio Vivaldi.",
                Category = "Culture/Art"
            };
            questions.Add(q20);

            OXQuizMetadata q21 = new OXQuizMetadata()
            {
                Id = 21,
                QuestionText = "\"Four thirty-three,\" a song John cage composed in 1952, is an elaborate sonata accompanied by orchestra and piano.",
                Answer = false,
                AnswerText = "The song is composed of three movements which are 33 seconds, 2 minute 40 seconds and 1 minute 20 seconds of tacet (silence).",
                Category = "Culture/Art"
            };
            questions.Add(q21);

            OXQuizMetadata q22 = new OXQuizMetadata()
            {
                Id = 22,
                QuestionText = "\"Ghost in the Shell\" is based on the comic of the same name, which is drawn by Shirow Masamune.",
                Answer = true,
                AnswerText = "\"Ghost in the Shell\" is based on the comic of the same name, which is drawn by Shirow Masamune.",
                Category = "Culture/Art"
            };
            questions.Add(q22);

            OXQuizMetadata q23 = new OXQuizMetadata()
            {
                Id = 23,
                QuestionText = "\"Ghost in the Shell\" is based on the comic of the same name, which was drawn by Oshii Mamoru.",
                Answer = false,
                AnswerText = "\"Ghost in the Shell\" is based on the comic of the same name, which was drawn by Shirow Masamune.",
                Category = "Culture/Art"
            };
            questions.Add(q23);

            OXQuizMetadata q24 = new OXQuizMetadata()
            {
                Id = 24,
                QuestionText = "\"Gujiga\" whose lyrics contain \"Turtle, turtle push out your head\" is a shamanic song.",
                Answer = true,
                AnswerText = "It's the oldest shamanic song in Korean history.",
                Category = "Culture/Art"
            };
            questions.Add(q24);

            OXQuizMetadata q25 = new OXQuizMetadata()
            {
                Id = 25,
                QuestionText = "\"Jikji,\" which was made public in 1972, is the oldest extant book printed with movable metal type.",
                Answer = true,
                AnswerText = "\"Jikj\" is the oldest extant book printed with movable metal type.",
                Category = "History"
            };
            questions.Add(q25);

            OXQuizMetadata q26 = new OXQuizMetadata()
            {
                Id = 26,
                QuestionText = "\"L'elisir d'amore\" is an opera by Gaetano Donizetti.",
                Answer = true,
                AnswerText = "\"L'elisir d'amore\" is an opera by Gaetano Donizetti.",
                Category = "Culture/Art"
            };
            questions.Add(q26);

            OXQuizMetadata q27 = new OXQuizMetadata()
            {
                Id = 27,
                QuestionText = "\"L'elisir d'amore\" is an opera by Giuseppe Verdi.",
                Answer = false,
                AnswerText = "\"L'elisir d'amore\" is an opera by Gaetano Donizetti.",
                Category = "Culture/Art"
            };
            questions.Add(q27);

            OXQuizMetadata q28 = new OXQuizMetadata()
            {
                Id = 28,
                QuestionText = "\"La Triviata\" is a work by Mozart.",
                Answer = false,
                AnswerText = "\"La Triviata\" is a work by Giuseppe Verdi.",
                Category = "Culture/Art"
            };
            questions.Add(q28);

            OXQuizMetadata q29 = new OXQuizMetadata()
            {
                Id = 29,
                QuestionText = "\"Les Miserables\" is Guy de Maupassant's work.",
                Answer = false,
                AnswerText = "\"Les Miserables\" is Victor Hugo's work.",
                Category = "Culture/Art"
            };
            questions.Add(q29);

            OXQuizMetadata q30 = new OXQuizMetadata()
            {
                Id = 30,
                QuestionText = "\"Macbeth\" is an opera written by Giuseppe Verdi.",
                Answer = true,
                AnswerText = "\"Macbeth\" is an opera written by Giuseppe Verdi.",
                Category = "Culture/Art"
            };
            questions.Add(q30);

            OXQuizMetadata q31 = new OXQuizMetadata()
            {
                Id = 31,
                QuestionText = "\"Macbeth\" is an opera written by Mozart.",
                Answer = false,
                AnswerText = "\"Macbeth\" is an opera written by Giuseppe Verdi.",
                Category = "Culture/Art"
            };
            questions.Add(q31);

            OXQuizMetadata q32 = new OXQuizMetadata()
            {
                Id = 32,
                QuestionText = "\"Madama Fly\" is by Puccini.",
                Answer = false,
                AnswerText = "It's one of his three masterpieces: \"Madama Butterfly,\" \"La Bohème,\" and \"Tosca.\"",
                Category = "Culture/Art"
            };
            questions.Add(q32);

            OXQuizMetadata q33 = new OXQuizMetadata()
            {
                Id = 33,
                QuestionText = "\"Mesopotamia\" means the extensive open grassland where the sun rises.",
                Answer = false,
                AnswerText = "\"Mesopotamia\" means the land between the two rivers.",
                Category = "Culture/Art"
            };
            questions.Add(q33);

            OXQuizMetadata q34 = new OXQuizMetadata()
            {
                Id = 34,
                QuestionText = "\"Mesopotamia\" means wide open grassland where the sun rises.",
                Answer = false,
                AnswerText = "\"Mesopotamia\" means land between two rivers.",
                Category = "Culture/Art"
            };
            questions.Add(q34);

            OXQuizMetadata q35 = new OXQuizMetadata()
            {
                Id = 35,
                QuestionText = "\"Moulin Rouge\" in French means yellow mill.",
                Answer = false,
                AnswerText = "\"Moulin Rouge\" in French means red mill.",
                Category = "Culture/Art"
            };
            questions.Add(q35);

            OXQuizMetadata q36 = new OXQuizMetadata()
            {
                Id = 36,
                QuestionText = "\"O Sole Mio\" is a song composed by Giacomo Puccini.",
                Answer = false,
                AnswerText = "\"O Sole Mio\" is a song composed by Giovanni Capurro.",
                Category = "Culture/Art"
            };
            questions.Add(q36);

            OXQuizMetadata q37 = new OXQuizMetadata()
            {
                Id = 37,
                QuestionText = "\"One Thousand and One Nights\" is well-known Latin literature.",
                Answer = false,
                AnswerText = "Saracen culture, based on Islam and Hellenism, is an Arabic culture.",
                Category = "Culture/Art"
            };
            questions.Add(q37);

            OXQuizMetadata q38 = new OXQuizMetadata()
            {
                Id = 38,
                QuestionText = "\"Paradise Lost,\" an epic poem written by English poet John Milton, is a trilogy. The three chapters are \"Hell,\" \"Purgatory,\" and \"Paradise.\"",
                Answer = false,
                AnswerText = "\"Paradise Lost\" is an epic poem composed of 12 acts. Its main theme is about the redemption of man's original sins as relate to Adam and Eve.",
                Category = "Culture/Art"
            };
            questions.Add(q38);

            OXQuizMetadata q39 = new OXQuizMetadata()
            {
                Id = 39,
                QuestionText = "\"Rapunzel\" is a story included in Andersen's fairy tales.",
                Answer = false,
                AnswerText = "It's in the Grimm brothers fairy tales.",
                Category = "Culture/Art"
            };
            questions.Add(q39);

            OXQuizMetadata q40 = new OXQuizMetadata()
            {
                Id = 40,
                QuestionText = "\"Rapunzel\" is a story included in the Grimm brothers' fairy tales.",
                Answer = true,
                AnswerText = "\"Rapunzel\" is a story included in the Grimm brothers' fairy tales.",
                Category = "Culture/Art"
            };
            questions.Add(q40);

            OXQuizMetadata q41 = new OXQuizMetadata()
            {
                Id = 41,
                QuestionText = "\"Romeo and Juliet\" is considered one of Shakespeare's four great tragedies.",
                Answer = false,
                AnswerText = "The four Shakespearean tragedies are Othello, King Lear, Macbeth, and Hamlet.",
                Category = "Culture/Art"
            };
            questions.Add(q41);

            OXQuizMetadata q42 = new OXQuizMetadata()
            {
                Id = 42,
                QuestionText = "\"Sangnoksu,\" a 1936 novel by Korean writer Sim Hun, had been hugely influenced by Russia's Narodniks movement (khozhdenie v narod).",
                Answer = true,
                AnswerText = "The Narodniks were a politically conscious movement of the Russian middle class.",
                Category = "Culture/Art"
            };
            questions.Add(q42);

            OXQuizMetadata q43 = new OXQuizMetadata()
            {
                Id = 43,
                QuestionText = "\"Tannhauser\" is a work by Beethoven.",
                Answer = false,
                AnswerText = "\"Tannhäuser\" is a work by Richard Wagner.",
                Category = "Culture/Art"
            };
            questions.Add(q43);

            OXQuizMetadata q44 = new OXQuizMetadata()
            {
                Id = 44,
                QuestionText = "\"Tannhauser\" is the only opera Beethoven ever composed.",
                Answer = false,
                AnswerText = "Beethoven left only one opera: \"Fidelio.\" It is divided into two acts.",
                Category = "Culture/Art"
            };
            questions.Add(q44);

            OXQuizMetadata q45 = new OXQuizMetadata()
            {
                Id = 45,
                QuestionText = "\"The Last Supper\" is Leonardo Da Vinci's painting.",
                Answer = true,
                AnswerText = "Leonardo da Vinci was the epitome of a Renaissance main. As a leading artist, sculptor, architect, mathematician, and philosopher, he left profound masterpieces.",
                Category = "General Knowledge"
            };
            questions.Add(q45);

            OXQuizMetadata q46 = new OXQuizMetadata()
            {
                Id = 46,
                QuestionText = "\"The Snow Queen\" isn't a story by Hans Christian Andersen.",
                Answer = false,
                AnswerText = "Hans Christian Andersen wrote \"The Snow Queen.\"",
                Category = "Culture/Art"
            };
            questions.Add(q46);

            OXQuizMetadata q47 = new OXQuizMetadata()
            {
                Id = 47,
                QuestionText = "\"The Ugly Duckling\" is a story included in Andersen's fairy tales.",
                Answer = true,
                AnswerText = "\"The Ugly Duckling\" is a story included in Andersen's fairy tales.",
                Category = "Culture/Art"
            };
            questions.Add(q47);

            OXQuizMetadata q48 = new OXQuizMetadata()
            {
                Id = 48,
                QuestionText = "\"The Ugly Duckling\" is a story included in Grimm brothers' fairy tales.",
                Answer = false,
                AnswerText = "It's in Andersen's fairy tales.",
                Category = "Culture/Art"
            };
            questions.Add(q48);

            OXQuizMetadata q49 = new OXQuizMetadata()
            {
                Id = 49,
                QuestionText = "\"The Ugly Duckling\" isn't a story by Hans Christian Andersen.",
                Answer = false,
                AnswerText = "Hans Christian Andersen wrote \"The Ugly Duckling.\"",
                Category = "Culture/Art"
            };
            questions.Add(q49);

            OXQuizMetadata q50 = new OXQuizMetadata()
            {
                Id = 50,
                QuestionText = "\"Thumbelina\" isn't a story by Hans Christian Andersen.",
                Answer = false,
                AnswerText = "Hans Christian Andersen wrote \"Thumbelina.\"",
                Category = "Culture/Art"
            };
            questions.Add(q50);

            OXQuizMetadata q51 = new OXQuizMetadata()
            {
                Id = 51,
                QuestionText = "\"Titanic\" is a name of an airplane.",
                Answer = false,
                AnswerText = "\"Titanic\" is a name of a ship.",
                Category = "Culture/Art"
            };
            questions.Add(q51);

            OXQuizMetadata q52 = new OXQuizMetadata()
            {
                Id = 52,
                QuestionText = "\"Tristan und Isolde\" is Igor Stravinsky's work.",
                Answer = false,
                AnswerText = "\"Tristan und Isolde\" is from Richard Wagner.",
                Category = "Culture/Art"
            };
            questions.Add(q52);

            OXQuizMetadata q53 = new OXQuizMetadata()
            {
                Id = 53,
                QuestionText = "\"Un bel di, vedremo\" is an aria from the opera \"La Boheme.\"",
                Answer = false,
                AnswerText = "\"Un bel di, vedremo\" is an aria from the opera \"Madama Butterfly.\"",
                Category = "Culture/Art"
            };
            questions.Add(q53);

            OXQuizMetadata q54 = new OXQuizMetadata()
            {
                Id = 54,
                QuestionText = "\"War and Peace\" is Fyodor Dostoyevsky's signature work.",
                Answer = false,
                AnswerText = "\"War and Peace\" is Tolstoy's work.",
                Category = "Culture/Art"
            };
            questions.Add(q54);

            OXQuizMetadata q55 = new OXQuizMetadata()
            {
                Id = 55,
                QuestionText = "\"Winterreise\" is Beethoven's work.",
                Answer = false,
                AnswerText = "\"Winterreise\" is Franz Schubert's work.",
                Category = "Culture/Art"
            };
            questions.Add(q55);

            OXQuizMetadata q56 = new OXQuizMetadata()
            {
                Id = 56,
                QuestionText = "\"Yalario,\" the salutation of the Allicari merchants in Queenstown, means \"old friend.\"",
                Answer = false,
                AnswerText = "\"Yalario,\" the salutation of the Allicari merchants, means \"new friend.\"",
                Category = "MapleStory 2"
            };
            questions.Add(q56);

            OXQuizMetadata q57 = new OXQuizMetadata()
            {
                Id = 57,
                QuestionText = "1 byte equals 1024 bits.",
                Answer = false,
                AnswerText = "1 byte equals 8 bits.",
                Category = "IT"
            };
            questions.Add(q57);

            OXQuizMetadata q58 = new OXQuizMetadata()
            {
                Id = 58,
                QuestionText = "1 byte equals 4 bits.",
                Answer = false,
                AnswerText = "1 byte equals 8 bits.",
                Category = "IT"
            };
            questions.Add(q58);

            OXQuizMetadata q59 = new OXQuizMetadata()
            {
                Id = 59,
                QuestionText = "1 byte equals 8 bits.",
                Answer = true,
                AnswerText = "1 byte equals 8 bits.",
                Category = "IT"
            };
            questions.Add(q59);

            OXQuizMetadata q60 = new OXQuizMetadata()
            {
                Id = 60,
                QuestionText = "1 gigabyte is 1024 kilobytes.",
                Answer = false,
                AnswerText = "1 megabyte is 1024 kilobytes.",
                Category = "IT"
            };
            questions.Add(q60);

            OXQuizMetadata q61 = new OXQuizMetadata()
            {
                Id = 61,
                QuestionText = "1 kgf = 9.8N",
                Answer = true,
                AnswerText = "1 kgf = 9.8N",
                Category = "Math"
            };
            questions.Add(q61);

            OXQuizMetadata q62 = new OXQuizMetadata()
            {
                Id = 62,
                QuestionText = "1 MB is 1024 GB.",
                Answer = false,
                AnswerText = "1 TB is 1024 GB.",
                Category = "IT"
            };
            questions.Add(q62);

            OXQuizMetadata q63 = new OXQuizMetadata()
            {
                Id = 63,
                QuestionText = "1 megabyte is 1024 kilobytes.",
                Answer = true,
                AnswerText = "One megabyte is 1024 kilobytes.",
                Category = "IT"
            };
            questions.Add(q63);

            OXQuizMetadata q64 = new OXQuizMetadata()
            {
                Id = 64,
                QuestionText = "1 megabyte is used to measure 1024 meterbytes.",
                Answer = false,
                AnswerText = "One megabyte is the unit used to measure 1024 kilobytes.",
                Category = "IT"
            };
            questions.Add(q64);

            OXQuizMetadata q65 = new OXQuizMetadata()
            {
                Id = 65,
                QuestionText = "10 quadrillion is 1,000 times 100 million.",
                Answer = false,
                AnswerText = "10 quadrillion is 100,000000 times 100 million.",
                Category = "Math"
            };
            questions.Add(q65);

            OXQuizMetadata q66 = new OXQuizMetadata()
            {
                Id = 66,
                QuestionText = "1024 byte is called 1 meterbyte.",
                Answer = false,
                AnswerText = "One kilobyte is 1024 bytes.",
                Category = "IT"
            };
            questions.Add(q66);

            OXQuizMetadata q67 = new OXQuizMetadata()
            {
                Id = 67,
                QuestionText = "1024 bytes is 1 kilobyte.",
                Answer = true,
                AnswerText = "One kilobyte is 1024 bytes.",
                Category = "IT"
            };
            questions.Add(q67);

            OXQuizMetadata q68 = new OXQuizMetadata()
            {
                Id = 68,
                QuestionText = "1024 GB is 1 TB",
                Answer = true,
                AnswerText = "1024 GB is 1 TB",
                Category = "IT"
            };
            questions.Add(q68);

            OXQuizMetadata q69 = new OXQuizMetadata()
            {
                Id = 69,
                QuestionText = "1024 megabytes is 1 gigabyte.",
                Answer = true,
                AnswerText = "1 gigabyte equals 1024 megabytes.",
                Category = "IT"
            };
            questions.Add(q69);

            OXQuizMetadata q70 = new OXQuizMetadata()
            {
                Id = 70,
                QuestionText = "1024 megabytes is 1 kilobyte.",
                Answer = false,
                AnswerText = "1024 megabytes is also known as 1 gigabyte.",
                Category = "IT"
            };
            questions.Add(q70);

            OXQuizMetadata q71 = new OXQuizMetadata()
            {
                Id = 71,
                QuestionText = "1024 megabytes is also called 10 gigabytes.",
                Answer = false,
                AnswerText = "1 gigabyte equals 1024 megabytes.",
                Category = "IT"
            };
            questions.Add(q71);

            OXQuizMetadata q72 = new OXQuizMetadata()
            {
                Id = 72,
                QuestionText = "13 players comprise a rugby team.",
                Answer = false,
                AnswerText = "In rugby, a team is composed of 7 or 15 players.",
                Category = "Culture/Art"
            };
            questions.Add(q72);

            OXQuizMetadata q73 = new OXQuizMetadata()
            {
                Id = 73,
                QuestionText = "14 + 6 X 2 = 26",
                Answer = true,
                AnswerText = "14 + 6 X 2 = 26",
                Category = "Math"
            };
            questions.Add(q73);

            OXQuizMetadata q74 = new OXQuizMetadata()
            {
                Id = 74,
                QuestionText = "14 + 6 X 2 = 40",
                Answer = false,
                AnswerText = "14 + 6 X 2 = 26",
                Category = "Math"
            };
            questions.Add(q74);

            OXQuizMetadata q75 = new OXQuizMetadata()
            {
                Id = 75,
                QuestionText = "14 X 6 = 84",
                Answer = true,
                AnswerText = "14 X 6 = 84",
                Category = "Math"
            };
            questions.Add(q75);

            OXQuizMetadata q76 = new OXQuizMetadata()
            {
                Id = 76,
                QuestionText = "14 X 6 = 86",
                Answer = false,
                AnswerText = "14 X 6 = 84",
                Category = "Math"
            };
            questions.Add(q76);

            OXQuizMetadata q77 = new OXQuizMetadata()
            {
                Id = 77,
                QuestionText = "19 X 19 = 361",
                Answer = true,
                AnswerText = "19 X 19 = 361",
                Category = "Math"
            };
            questions.Add(q77);

            OXQuizMetadata q78 = new OXQuizMetadata()
            {
                Id = 78,
                QuestionText = "19 X 19 = 381",
                Answer = false,
                AnswerText = "19 X 19 = 361",
                Category = "Math"
            };
            questions.Add(q78);

            OXQuizMetadata q79 = new OXQuizMetadata()
            {
                Id = 79,
                QuestionText = "194 nations participated in the 2008 Beijing Olympics.",
                Answer = false,
                AnswerText = "204 nations participated.",
                Category = "General Knowledge"
            };
            questions.Add(q79);

            OXQuizMetadata q80 = new OXQuizMetadata()
            {
                Id = 80,
                QuestionText = "2 + 2 = 4",
                Answer = true,
                AnswerText = "2 + 2 = 4",
                Category = "Math"
            };
            questions.Add(q80);

            OXQuizMetadata q81 = new OXQuizMetadata()
            {
                Id = 81,
                QuestionText = "24 X 4 = 86",
                Answer = false,
                AnswerText = "24 X 4 = 96",
                Category = "Math"
            };
            questions.Add(q81);

            OXQuizMetadata q82 = new OXQuizMetadata()
            {
                Id = 82,
                QuestionText = "24 X 4 = 96",
                Answer = true,
                AnswerText = "24 X 4 = 96",
                Category = "Math"
            };
            questions.Add(q82);

            OXQuizMetadata q83 = new OXQuizMetadata()
            {
                Id = 83,
                QuestionText = "29 X 29 = 581",
                Answer = false,
                AnswerText = "29 X 29 = 841",
                Category = "Math"
            };
            questions.Add(q83);

            OXQuizMetadata q84 = new OXQuizMetadata()
            {
                Id = 84,
                QuestionText = "29 X 29 = 821",
                Answer = false,
                AnswerText = "29 X 29 = 841",
                Category = "Math"
            };
            questions.Add(q84);

            OXQuizMetadata q85 = new OXQuizMetadata()
            {
                Id = 85,
                QuestionText = "29 X 29 = 831",
                Answer = false,
                AnswerText = "29 X 29 = 841",
                Category = "Math"
            };
            questions.Add(q85);

            OXQuizMetadata q86 = new OXQuizMetadata()
            {
                Id = 86,
                QuestionText = "29 X 29 = 841",
                Answer = true,
                AnswerText = "29 X 29 = 841",
                Category = "Math"
            };
            questions.Add(q86);

            OXQuizMetadata q87 = new OXQuizMetadata()
            {
                Id = 87,
                QuestionText = "2PM, the unit that describes a concentration of a substance in water or soil, is usually used to measure pollution.",
                Answer = false,
                AnswerText = "PPM, the unit that describes a concentration of a substance in water or soil, is the most widely used measure of pollution.",
                Category = "Science"
            };
            questions.Add(q87);

            OXQuizMetadata q88 = new OXQuizMetadata()
            {
                Id = 88,
                QuestionText = "3 + 5 X 18 = 144",
                Answer = false,
                AnswerText = "3 + 5 X 18 = 93",
                Category = "Math"
            };
            questions.Add(q88);

            OXQuizMetadata q89 = new OXQuizMetadata()
            {
                Id = 89,
                QuestionText = "4 + 3 = 7",
                Answer = true,
                AnswerText = "4 + 3 = 7",
                Category = "Math"
            };
            questions.Add(q89);

            OXQuizMetadata q90 = new OXQuizMetadata()
            {
                Id = 90,
                QuestionText = "4 + 6 X 2 = 16",
                Answer = true,
                AnswerText = "4 + 6 X 2 = 16",
                Category = "Math"
            };
            questions.Add(q90);

            OXQuizMetadata q91 = new OXQuizMetadata()
            {
                Id = 91,
                QuestionText = "4 + 6 X 2 = 20",
                Answer = false,
                AnswerText = "4 + 6 X 2 = 16",
                Category = "Math"
            };
            questions.Add(q91);

            OXQuizMetadata q92 = new OXQuizMetadata()
            {
                Id = 92,
                QuestionText = "4 X 6 + 2 = 26",
                Answer = true,
                AnswerText = "4 X 6 + 2 = 26",
                Category = "Math"
            };
            questions.Add(q92);

            OXQuizMetadata q93 = new OXQuizMetadata()
            {
                Id = 93,
                QuestionText = "4 X 6 = 26",
                Answer = false,
                AnswerText = "4 X 6 = 24",
                Category = "Math"
            };
            questions.Add(q93);

            OXQuizMetadata q94 = new OXQuizMetadata()
            {
                Id = 94,
                QuestionText = "42 X 2 = 84",
                Answer = true,
                AnswerText = "42 X 2 = 84",
                Category = "Math"
            };
            questions.Add(q94);

            OXQuizMetadata q95 = new OXQuizMetadata()
            {
                Id = 95,
                QuestionText = "9 X 9 = 80",
                Answer = false,
                AnswerText = "9 X 9 = 81",
                Category = "Math"
            };
            questions.Add(q95);

            OXQuizMetadata q96 = new OXQuizMetadata()
            {
                Id = 96,
                QuestionText = "9 X 9 = 81",
                Answer = true,
                AnswerText = "9 X 9 = 81",
                Category = "Math"
            };
            questions.Add(q96);

            OXQuizMetadata q97 = new OXQuizMetadata()
            {
                Id = 97,
                QuestionText = "A bald person's head doesn't smell, even if one doesn't wash it.",
                Answer = false,
                AnswerText = "The smell comes from the scalp, not the hair.",
                Category = "General Knowledge"
            };
            questions.Add(q97);

            OXQuizMetadata q98 = new OXQuizMetadata()
            {
                Id = 98,
                QuestionText = "A black box flight recorder is colored black.",
                Answer = false,
                AnswerText = "A black box is orange.",
                Category = "General Knowledge"
            };
            questions.Add(q98);

            OXQuizMetadata q99 = new OXQuizMetadata()
            {
                Id = 99,
                QuestionText = "A boy, who took the ball in his arms and ran with it amid a football match inspired the creation of rugby.",
                Answer = true,
                AnswerText = "William Webb Ellis is the alleged inventor of rugby football. According to legend, he picked up the ball and ran with it during a school football match in 1823, thus creating the rugby-style of play.",
                Category = "Culture/Art"
            };
            questions.Add(q99);

            OXQuizMetadata q100 = new OXQuizMetadata()
            {
                Id = 100,
                QuestionText = "A British songwriter composed \"Jeanie with the Light Brown Hair.\"",
                Answer = false,
                AnswerText = "An American songwriter, Stephen Foster, composed \"Jeanie with the Light Brown Hair.\"",
                Category = "Culture/Art"
            };
            questions.Add(q100);

            OXQuizMetadata q101 = new OXQuizMetadata()
            {
                Id = 101,
                QuestionText = "A camel has humps on its back. They are filled with water.",
                Answer = false,
                AnswerText = "They are filled with fat.",
                Category = "Science"
            };
            questions.Add(q101);

            OXQuizMetadata q102 = new OXQuizMetadata()
            {
                Id = 102,
                QuestionText = "A camel's humps are filled with water.",
                Answer = false,
                AnswerText = "Camel humps consist of fat.",
                Category = "General Knowledge"
            };
            questions.Add(q102);

            OXQuizMetadata q103 = new OXQuizMetadata()
            {
                Id = 103,
                QuestionText = "A capella is a religious musical movement from the Renaissance, and refers to songs accompanied by the organ. It is usually played in churches and cathedrals.",
                Answer = false,
                AnswerText = "A capella is a religious music sung in churches and cathedrals and refers to singing without instrumental accompaniment.",
                Category = "Culture/Art"
            };
            questions.Add(q103);

            OXQuizMetadata q104 = new OXQuizMetadata()
            {
                Id = 104,
                QuestionText = "A capella is religious music from the Renaissance, and refers to singing without instrumental accompaniment.",
                Answer = true,
                AnswerText = "A capella is religious music sung in churches and cathedrals and refers to singing without instrumental accompaniment.",
                Category = "Culture/Art"
            };
            questions.Add(q104);

            OXQuizMetadata q105 = new OXQuizMetadata()
            {
                Id = 105,
                QuestionText = "A carrot's root is the part of a carrot popularly consumed.",
                Answer = true,
                AnswerText = "A carrot is a root vegetable. Most people consume the root part of a carrot.",
                Category = "General Knowledge"
            };
            questions.Add(q105);

            OXQuizMetadata q106 = new OXQuizMetadata()
            {
                Id = 106,
                QuestionText = "A carrot's stem is the part of a carrot popularly consumed.",
                Answer = false,
                AnswerText = "Carrots are a root vegetable. Most people consume the root part of a carrot.",
                Category = "General Knowledge"
            };
            questions.Add(q106);

            OXQuizMetadata q107 = new OXQuizMetadata()
            {
                Id = 107,
                QuestionText = "A cold game in baseball refers to the situation in which no opposing players reaches a base until the end.",
                Answer = false,
                AnswerText = "A perfect game in baseball refers to the situation in which the pitcher doesn't allow a single hit until the end of the game.",
                Category = "Culture/Art"
            };
            questions.Add(q107);

            OXQuizMetadata q108 = new OXQuizMetadata()
            {
                Id = 108,
                QuestionText = "A day is 28 hours long.",
                Answer = false,
                AnswerText = "A day is 24 hours long.",
                Category = "General Knowledge"
            };
            questions.Add(q108);

            OXQuizMetadata q109 = new OXQuizMetadata()
            {
                Id = 109,
                QuestionText = "A die is a cube, which each of its six faces showing a different number from 1 to 6. The opposite sides of a die always add up to 8.",
                Answer = false,
                AnswerText = "The opposite sides of dice always add up to 7.",
                Category = "General Knowledge"
            };
            questions.Add(q109);

            OXQuizMetadata q110 = new OXQuizMetadata()
            {
                Id = 110,
                QuestionText = "A duet refers to a company of three voices or instruments.",
                Answer = false,
                AnswerText = "A duet refers to a company of two voices or instruments.",
                Category = "Culture/Art"
            };
            questions.Add(q110);

            OXQuizMetadata q111 = new OXQuizMetadata()
            {
                Id = 111,
                QuestionText = "A frog falls asleep if its tummy is rubbed.",
                Answer = true,
                AnswerText = "A frog whose tummy is rubbed feels relieved and falls asleep.",
                Category = "General Knowledge"
            };
            questions.Add(q111);

            OXQuizMetadata q112 = new OXQuizMetadata()
            {
                Id = 112,
                QuestionText = "A frog wakes up from sleep if its tummy is rubbed.",
                Answer = false,
                AnswerText = "A frog whose tummy is rubbed feels relieved and falls asleep.",
                Category = "General Knowledge"
            };
            questions.Add(q112);

            OXQuizMetadata q113 = new OXQuizMetadata()
            {
                Id = 113,
                QuestionText = "A German pianist won the 19th International Chopin Piano Competition, a competition that is held in Warsaw, Poland.",
                Answer = false,
                AnswerText = "The 1990 International Chopin Piano Competition was the 12th. Also, it had no winner.",
                Category = "Culture/Art"
            };
            questions.Add(q113);

            OXQuizMetadata q114 = new OXQuizMetadata()
            {
                Id = 114,
                QuestionText = "A Giant Turtle appears at Beachway 666.",
                Answer = false,
                AnswerText = "A Giant Turtle appears at Beachway 111.",
                Category = "MapleStory 2"
            };
            questions.Add(q114);

            OXQuizMetadata q115 = new OXQuizMetadata()
            {
                Id = 115,
                QuestionText = "A guild is an association of artisans and merchants.",
                Answer = true,
                AnswerText = "A guild is an association of artisans and merchants. The development of cities and urban economies made guilds a possibility.",
                Category = "History"
            };
            questions.Add(q115);

            OXQuizMetadata q116 = new OXQuizMetadata()
            {
                Id = 116,
                QuestionText = "A guild is an association of medieval lords.",
                Answer = false,
                AnswerText = "Guilds began in the 12th century as associations of artisans and merchants. The development of cities and urban economy made guilds a possibility.",
                Category = "History"
            };
            questions.Add(q116);

            OXQuizMetadata q117 = new OXQuizMetadata()
            {
                Id = 117,
                QuestionText = "A guitar is a chordophone.",
                Answer = true,
                AnswerText = "A guitar is a chordophone.",
                Category = "Culture/Art"
            };
            questions.Add(q117);

            OXQuizMetadata q118 = new OXQuizMetadata()
            {
                Id = 118,
                QuestionText = "A guitar is a percussion instrument.",
                Answer = false,
                AnswerText = "A guitar is a chordophone.",
                Category = "Culture/Art"
            };
            questions.Add(q118);

            OXQuizMetadata q119 = new OXQuizMetadata()
            {
                Id = 119,
                QuestionText = "A hearing aid is an electronic device a person with hearing loss uses.",
                Answer = true,
                AnswerText = "A hearing aid is an electronic device a person with hearing loss uses.",
                Category = "General Knowledge"
            };
            questions.Add(q119);

            OXQuizMetadata q120 = new OXQuizMetadata()
            {
                Id = 120,
                QuestionText = "A lese drama refers to dramas specifically written for reading.",
                Answer = true,
                AnswerText = "Å lese drama refers to dramas specifically written for reading.",
                Category = "Culture/Art"
            };
            questions.Add(q120);

            OXQuizMetadata q121 = new OXQuizMetadata()
            {
                Id = 121,
                QuestionText = "A lion is a herbivore.",
                Answer = false,
                AnswerText = "A lion is a carnivore.",
                Category = "General Knowledge"
            };
            questions.Add(q121);

            OXQuizMetadata q122 = new OXQuizMetadata()
            {
                Id = 122,
                QuestionText = "A lunar eclipse happens because the moon blocks the sun.",
                Answer = false,
                AnswerText = "A lunar eclipse happens because the earth's shadow blocks the moon.",
                Category = "Science"
            };
            questions.Add(q122);

            OXQuizMetadata q123 = new OXQuizMetadata()
            {
                Id = 123,
                QuestionText = "A magnet is composed of X and S poles.",
                Answer = false,
                AnswerText = "A magnet is composed of N and S poles.",
                Category = "Science"
            };
            questions.Add(q123);

            OXQuizMetadata q124 = new OXQuizMetadata()
            {
                Id = 124,
                QuestionText = "A male flea is bigger than a female flea.",
                Answer = false,
                AnswerText = "A female flea is bigger than a male flea.",
                Category = "General Knowledge"
            };
            questions.Add(q124);

            OXQuizMetadata q125 = new OXQuizMetadata()
            {
                Id = 125,
                QuestionText = "A Mandarin fish eats fish its size.",
                Answer = true,
                AnswerText = "A Mandarin fish eats fish its size.",
                Category = "Science"
            };
            questions.Add(q125);

            OXQuizMetadata q126 = new OXQuizMetadata()
            {
                Id = 126,
                QuestionText = "A Mandarin fish eats only the fish that are smaller than itself.",
                Answer = false,
                AnswerText = "A Mandarin fish eats fish its size.",
                Category = "Science"
            };
            questions.Add(q126);

            OXQuizMetadata q127 = new OXQuizMetadata()
            {
                Id = 127,
                QuestionText = "A marathon course is 40.185 km long.",
                Answer = false,
                AnswerText = "A marathon course is 42.195 km long.",
                Category = "General Knowledge"
            };
            questions.Add(q127);

            OXQuizMetadata q128 = new OXQuizMetadata()
            {
                Id = 128,
                QuestionText = "A marathon course is 42.195km long.",
                Answer = true,
                AnswerText = "A marathon course is 42.195km long.",
                Category = "Culture/Art"
            };
            questions.Add(q128);

            OXQuizMetadata q129 = new OXQuizMetadata()
            {
                Id = 129,
                QuestionText = "A musical is an art form that follows the musical play style of the late 16th century Italy and whose texts, some or all, are represented as songs.",
                Answer = false,
                AnswerText = "It's an opera.",
                Category = "Culture/Art"
            };
            questions.Add(q129);

            OXQuizMetadata q130 = new OXQuizMetadata()
            {
                Id = 130,
                QuestionText = "A newly born hedgehog doesn't yet have spines on its back.",
                Answer = false,
                AnswerText = "A newly born hedgehog does have spines, but they are soft.",
                Category = "General Knowledge"
            };
            questions.Add(q130);

            OXQuizMetadata q131 = new OXQuizMetadata()
            {
                Id = 131,
                QuestionText = "A paradox refers to an imitation of the style of a particular writer, artist, or genre with deliberate exaggeration for comic effect.",
                Answer = false,
                AnswerText = "Parody is used widely in songs, movies, advertisements, etc.",
                Category = "General Knowledge"
            };
            questions.Add(q131);

            OXQuizMetadata q132 = new OXQuizMetadata()
            {
                Id = 132,
                QuestionText = "A perfect game in baseball refers to the situation in which no opposing players reach a base until the end.",
                Answer = true,
                AnswerText = "A perfect game in baseball refers to the situation in which the pitcher doesn't allow a single hit until the end of the game.",
                Category = "Culture/Art"
            };
            questions.Add(q132);

            OXQuizMetadata q133 = new OXQuizMetadata()
            {
                Id = 133,
                QuestionText = "A person with type O blood can provide blood to a person with type A blood.",
                Answer = true,
                AnswerText = "A person with type O blood can provide blood to all blood types.",
                Category = "Science"
            };
            questions.Add(q133);

            OXQuizMetadata q134 = new OXQuizMetadata()
            {
                Id = 134,
                QuestionText = "A person with type O blood can't provide blood to a person with type A blood.",
                Answer = false,
                AnswerText = "A person with type O blood can provide blood to anyone.",
                Category = "Science"
            };
            questions.Add(q134);

            OXQuizMetadata q135 = new OXQuizMetadata()
            {
                Id = 135,
                QuestionText = "A person with type O blood can't provide blood to a person with type AB blood.",
                Answer = false,
                AnswerText = "A person with type O blood can provide blood to anyone.",
                Category = "Science"
            };
            questions.Add(q135);

            OXQuizMetadata q136 = new OXQuizMetadata()
            {
                Id = 136,
                QuestionText = "A polar bear's skin color is black.",
                Answer = true,
                AnswerText = "A polar bear's skin is black.",
                Category = "Science"
            };
            questions.Add(q136);

            OXQuizMetadata q137 = new OXQuizMetadata()
            {
                Id = 137,
                QuestionText = "A polar bear's skin color is white.",
                Answer = false,
                AnswerText = "A polar bear's skin is black.",
                Category = "Science"
            };
            questions.Add(q137);

            OXQuizMetadata q138 = new OXQuizMetadata()
            {
                Id = 138,
                QuestionText = "A polar bear's skin is black.",
                Answer = true,
                AnswerText = "A polar bear's skin is black.",
                Category = "Science"
            };
            questions.Add(q138);

            OXQuizMetadata q139 = new OXQuizMetadata()
            {
                Id = 139,
                QuestionText = "A prosecutor can issue an arrest warrant.",
                Answer = false,
                AnswerText = "A local judge decides whether to issue warrants based on the request from a prosecutor or a judicial police officer.",
                Category = "Society"
            };
            questions.Add(q139);

            OXQuizMetadata q140 = new OXQuizMetadata()
            {
                Id = 140,
                QuestionText = "A quilt is a multi-layered textile, composed by sewing layers of fabrics.",
                Answer = true,
                AnswerText = "A quilt is a multi-layered textile, composed by sewing layers of fabrics.",
                Category = "Culture/Art"
            };
            questions.Add(q140);

            OXQuizMetadata q141 = new OXQuizMetadata()
            {
                Id = 141,
                QuestionText = "A seesaw spins on its center.",
                Answer = false,
                AnswerText = "Seesaws go up and down.",
                Category = "General Knowledge"
            };
            questions.Add(q141);

            OXQuizMetadata q142 = new OXQuizMetadata()
            {
                Id = 142,
                QuestionText = "A semiconductor is made of germanium, silicon, cadmium sulfide, etc.",
                Answer = true,
                AnswerText = "Mixtures and compounds of germanium, silicon, cadmium sulfide, etc. are used to make semiconductors. Semiconductors devices -- transistors and thyristor to name a few -- are used widely in many fields including",
                Category = "IT"
            };
            questions.Add(q142);

            OXQuizMetadata q143 = new OXQuizMetadata()
            {
                Id = 143,
                QuestionText = "A shrimp's heart is at the tip of its head.",
                Answer = true,
                AnswerText = "A shrimp's heart is at the tip of its head.",
                Category = "General Knowledge"
            };
            questions.Add(q143);

            OXQuizMetadata q144 = new OXQuizMetadata()
            {
                Id = 144,
                QuestionText = "A shrimp's heart is at the tip of its tail.",
                Answer = false,
                AnswerText = "A shrimp's heart is at the tip of its head.",
                Category = "General Knowledge"
            };
            questions.Add(q144);

            OXQuizMetadata q145 = new OXQuizMetadata()
            {
                Id = 145,
                QuestionText = "A sigil is the stamp that one usually imprints on one's writing or drawing. It usually bears one's name or pseudonym.",
                Answer = true,
                AnswerText = "A sigil is the stamp that one usually imprints on one's writing or drawing. It usually bears one's name or pseudonym.",
                Category = "General Knowledge"
            };
            questions.Add(q145);

            OXQuizMetadata q146 = new OXQuizMetadata()
            {
                Id = 146,
                QuestionText = "A snail can crawl over a razor blade without being cut.",
                Answer = true,
                AnswerText = "A snail can crawl over a razor blade without being cut thanks to its mucus lining.",
                Category = "General Knowledge"
            };
            questions.Add(q146);

            OXQuizMetadata q147 = new OXQuizMetadata()
            {
                Id = 147,
                QuestionText = "A snail can't crawl over a razor blade without being cut.",
                Answer = false,
                AnswerText = "A snail can crawl over a razor blade without being cut thanks to its mucus lining.",
                Category = "General Knowledge"
            };
            questions.Add(q147);

            OXQuizMetadata q148 = new OXQuizMetadata()
            {
                Id = 148,
                QuestionText = "A soccer team consists of 11 players, a basketball team consists of 5 players, and a handball team consists of 7 players.",
                Answer = true,
                AnswerText = "A soccer team consists of 11 players, a basketball team consists of 5 players, and a handball team consists of 7 players.",
                Category = "Culture/Art"
            };
            questions.Add(q148);

            OXQuizMetadata q149 = new OXQuizMetadata()
            {
                Id = 149,
                QuestionText = "A soccer team consists of 11 players, a basketball team consists of 5 players, and a handball team consists of 9 players.",
                Answer = false,
                AnswerText = "The number of players in each team is 11, 5 and 7 respectively.",
                Category = "General Knowledge"
            };
            questions.Add(q149);

            OXQuizMetadata q150 = new OXQuizMetadata()
            {
                Id = 150,
                QuestionText = "A soprano's vocal range is high, while an alto's vocal range is low.",
                Answer = true,
                AnswerText = "From high to low, the three female voices are classified as soprano, mezzo-soprano, and alto.",
                Category = "Culture/Art"
            };
            questions.Add(q150);

            OXQuizMetadata q151 = new OXQuizMetadata()
            {
                Id = 151,
                QuestionText = "A soprano's vocal range is low, while an alto's vocal range is high.",
                Answer = false,
                AnswerText = "From high to low, the female voices are classified as soprano, mezzo-soprano, and alto.",
                Category = "Culture/Art"
            };
            questions.Add(q151);

            OXQuizMetadata q152 = new OXQuizMetadata()
            {
                Id = 152,
                QuestionText = "A speaker is not an output device.",
                Answer = false,
                AnswerText = "Speakers are output devices which make sounds.",
                Category = "General Knowledge"
            };
            questions.Add(q152);

            OXQuizMetadata q153 = new OXQuizMetadata()
            {
                Id = 153,
                QuestionText = "A spider is an eight-legged insect.",
                Answer = false,
                AnswerText = "A spider is an arthropod.",
                Category = "General Knowledge"
            };
            questions.Add(q153);

            OXQuizMetadata q154 = new OXQuizMetadata()
            {
                Id = 154,
                QuestionText = "A spider is classified as an insect.",
                Answer = false,
                AnswerText = "A spider is classified as an arthropod.",
                Category = "Science"
            };
            questions.Add(q154);

            OXQuizMetadata q155 = new OXQuizMetadata()
            {
                Id = 155,
                QuestionText = "A spider is classified as an insect.",
                Answer = false,
                AnswerText = "An insect has 6 legs. A spider has 8.",
                Category = "Science"
            };
            questions.Add(q155);

            OXQuizMetadata q156 = new OXQuizMetadata()
            {
                Id = 156,
                QuestionText = "A squid has 9 tentacles.",
                Answer = false,
                AnswerText = "A squid has 10 tentacles.",
                Category = "General Knowledge"
            };
            questions.Add(q156);

            OXQuizMetadata q157 = new OXQuizMetadata()
            {
                Id = 157,
                QuestionText = "A stethoscope is an electronic device a person with hearing loss uses.",
                Answer = false,
                AnswerText = "The device is called a hearing aid.",
                Category = "General Knowledge"
            };
            questions.Add(q157);

            OXQuizMetadata q158 = new OXQuizMetadata()
            {
                Id = 158,
                QuestionText = "A stone is heavier to lift underwater than out in the air.",
                Answer = false,
                AnswerText = "Because of buoyancy, stones are lighter to lift underwater.",
                Category = "General Knowledge"
            };
            questions.Add(q158);

            OXQuizMetadata q159 = new OXQuizMetadata()
            {
                Id = 159,
                QuestionText = "A team of 10 players can play in a game of Gateball.",
                Answer = false,
                AnswerText = "A team of 5 to 7 players can play in a game of Gateball.",
                Category = "Culture/Art"
            };
            questions.Add(q159);

            OXQuizMetadata q160 = new OXQuizMetadata()
            {
                Id = 160,
                QuestionText = "A tongue doesn't get fat.",
                Answer = false,
                AnswerText = "A tongue does get fat.",
                Category = "Science"
            };
            questions.Add(q160);

            OXQuizMetadata q161 = new OXQuizMetadata()
            {
                Id = 161,
                QuestionText = "A turtle has a tail.",
                Answer = true,
                AnswerText = "A turtle has a tail.",
                Category = "Science"
            };
            questions.Add(q161);

            OXQuizMetadata q162 = new OXQuizMetadata()
            {
                Id = 162,
                QuestionText = "A turtle lacks a tail.",
                Answer = false,
                AnswerText = "A turtle has a tail.",
                Category = "General Knowledge"
            };
            questions.Add(q162);

            OXQuizMetadata q163 = new OXQuizMetadata()
            {
                Id = 163,
                QuestionText = "A typical symphony doesn't include piano.",
                Answer = true,
                AnswerText = "Because a piano lacks tonality, it's excluded from a symphony.",
                Category = "Culture/Art"
            };
            questions.Add(q163);

            OXQuizMetadata q164 = new OXQuizMetadata()
            {
                Id = 164,
                QuestionText = "A typical symphony includes piano.",
                Answer = false,
                AnswerText = "Because a piano lacks tonality, it's excluded from a symphony.",
                Category = "Culture/Art"
            };
            questions.Add(q164);

            OXQuizMetadata q165 = new OXQuizMetadata()
            {
                Id = 165,
                QuestionText = "A violin has 7 strings.",
                Answer = false,
                AnswerText = "A violin has 4 strings.",
                Category = "Culture/Art"
            };
            questions.Add(q165);

            OXQuizMetadata q166 = new OXQuizMetadata()
            {
                Id = 166,
                QuestionText = "A VISA is a conditional authorization granted by a country to a foreigner, allowing them to enter.",
                Answer = true,
                AnswerText = "A VISA is a conditional authorization granted by a country to a foreigner, allowing them to enter.",
                Category = "Society"
            };
            questions.Add(q166);

            OXQuizMetadata q167 = new OXQuizMetadata()
            {
                Id = 167,
                QuestionText = "A VISA is a conditional sanction placed by a country on a foreigner, banning them from entering.",
                Answer = false,
                AnswerText = "A VISA is a conditional authorization granted by a country to a foreigner, allowing them to enter.",
                Category = "Society"
            };
            questions.Add(q167);

            OXQuizMetadata q168 = new OXQuizMetadata()
            {
                Id = 168,
                QuestionText = "A wart is a group of skin cells that have grown rapidly due to human papillomavirus (HPV) infection.",
                Answer = true,
                AnswerText = "A wart is an infectious, viral disease caused by human papillomavirus (HPV).",
                Category = "Science"
            };
            questions.Add(q168);

            OXQuizMetadata q169 = new OXQuizMetadata()
            {
                Id = 169,
                QuestionText = "A watermill is a device for spinning thread or yarn from cotton or furs.",
                Answer = false,
                AnswerText = "A spinning wheel is a device for spinning thread or yarn from cotton or furs.",
                Category = "General Knowledge"
            };
            questions.Add(q169);

            OXQuizMetadata q170 = new OXQuizMetadata()
            {
                Id = 170,
                QuestionText = "A wild turkey's head can change into 7 different colors.",
                Answer = false,
                AnswerText = "A wild turkey's head can change into 3 different colors.",
                Category = "General Knowledge"
            };
            questions.Add(q170);

            OXQuizMetadata q171 = new OXQuizMetadata()
            {
                Id = 171,
                QuestionText = "A4 paper is bigger than A3.",
                Answer = false,
                AnswerText = "A3 is bigger.",
                Category = "General Knowledge"
            };
            questions.Add(q171);

            OXQuizMetadata q172 = new OXQuizMetadata()
            {
                Id = 172,
                QuestionText = "A5 paper is half the size of A0 paper.",
                Answer = false,
                AnswerText = "A1 is half the size of A0 paper size.",
                Category = "General Knowledge"
            };
            questions.Add(q172);

            OXQuizMetadata q173 = new OXQuizMetadata()
            {
                Id = 173,
                QuestionText = "AAAA, the smallest cylindrical battery, and D, the largest cylindrical battery, have the same voltage.",
                Answer = true,
                AnswerText = "Both AAAA-type and D-type batteries have 1.5 V.",
                Category = "General Knowledge"
            };
            questions.Add(q173);

            OXQuizMetadata q174 = new OXQuizMetadata()
            {
                Id = 174,
                QuestionText = "According to ABO blood group system, type-A mother and type-A father can bear a type-O offspring.",
                Answer = true,
                AnswerText = "According to the ABO blood group system, type-AO mother and type-AO father can bear a type-OO offspring.",
                Category = "Science"
            };
            questions.Add(q174);

            OXQuizMetadata q175 = new OXQuizMetadata()
            {
                Id = 175,
                QuestionText = "According to ABO blood group system, type-A mother and type-B father can bear a type-O offspring.",
                Answer = true,
                AnswerText = "According to the ABO blood group system, type-AO mother and type-BO father can bear a type-OO offspring.",
                Category = "Science"
            };
            questions.Add(q175);

            OXQuizMetadata q176 = new OXQuizMetadata()
            {
                Id = 176,
                QuestionText = "According to ABO blood group system, type-B mother and type-O father can bear a type-O offspring.",
                Answer = true,
                AnswerText = "According to the ABO blood group system, type-BO mother and type-OO father can bear a type-OO offspring.",
                Category = "Science"
            };
            questions.Add(q176);

            OXQuizMetadata q177 = new OXQuizMetadata()
            {
                Id = 177,
                QuestionText = "According to ABO blood group system, type-O mother and type-AB father can bear a type-B offspring.",
                Answer = true,
                AnswerText = "According to the ABO blood group system, type-OO mother and type-AB father can bear a type-BO offspring.",
                Category = "Science"
            };
            questions.Add(q177);

            OXQuizMetadata q178 = new OXQuizMetadata()
            {
                Id = 178,
                QuestionText = "According to both ABO blood group system and Rh blood group system, a person with Rh- blood can give blood to another individual with the same ABO-type blood but has Rh+ blood.",
                Answer = true,
                AnswerText = "Because Rh- blood lacks antigens, a person with Rh- blood can give blood to another individual with the same ABO-type blood but has Rh+ blood.",
                Category = "Science"
            };
            questions.Add(q178);

            OXQuizMetadata q179 = new OXQuizMetadata()
            {
                Id = 179,
                QuestionText = "Actions learned through emotes disappear eventually.",
                Answer = false,
                AnswerText = "Actions learned by emote are permanent.",
                Category = "MapleStory 2"
            };
            questions.Add(q179);

            OXQuizMetadata q180 = new OXQuizMetadata()
            {
                Id = 180,
                QuestionText = "Actors Michael Douglas, Charlie Sheen, and Kiefer Sutherland are all sons of renowned actors.",
                Answer = true,
                AnswerText = "Their fathers are Kirk Douglas, Martin Sheen, and Donald Sutherland.",
                Category = "Culture/Art"
            };
            questions.Add(q180);

            OXQuizMetadata q181 = new OXQuizMetadata()
            {
                Id = 181,
                QuestionText = "Ad-lib refers to improvised solo parts in jazz and impromptu delivery in plays and movies.",
                Answer = true,
                AnswerText = "Ad-lib is an abbreviation of ad libitum es.",
                Category = "Culture/Art"
            };
            questions.Add(q181);

            OXQuizMetadata q182 = new OXQuizMetadata()
            {
                Id = 182,
                QuestionText = "Agatha Christie wrote the detective novel \"Sherlock Holmes.\"",
                Answer = false,
                AnswerText = "Arthur Conan Doyle wrote the detective novel \"Sherlock Holmes.\"",
                Category = "Culture/Art"
            };
            questions.Add(q182);

            OXQuizMetadata q183 = new OXQuizMetadata()
            {
                Id = 183,
                QuestionText = "Air is one of five basic elements in Five Elements Theory.",
                Answer = false,
                AnswerText = "The five elements are fire, water, earth, metal, and wood.",
                Category = "General Knowledge"
            };
            questions.Add(q183);

            OXQuizMetadata q184 = new OXQuizMetadata()
            {
                Id = 184,
                QuestionText = "Albert Schweitzer composed \"Erlkonig,\" whose lyrics are from a poem by Wolfgang Goethe.",
                Answer = false,
                AnswerText = "Franz Schubert composed \"Erlkönig,\" whose lyrics are from a poem by Wolfgang Goethe.",
                Category = "Culture/Art"
            };
            questions.Add(q184);

            OXQuizMetadata q185 = new OXQuizMetadata()
            {
                Id = 185,
                QuestionText = "Albinism in humans is a congenital disorder characterized by the complete or partial absence of pigment in the skin, hair, and eyes.",
                Answer = true,
                AnswerText = "Deficiency in melanin causes it.",
                Category = "Science"
            };
            questions.Add(q185);

            OXQuizMetadata q186 = new OXQuizMetadata()
            {
                Id = 186,
                QuestionText = "Alikar Prison's male uniforms can be washed once a year.",
                Answer = false,
                AnswerText = "Alikar Prison's male uniforms can be washed twice a year.",
                Category = "MapleStory 2"
            };
            questions.Add(q186);

            OXQuizMetadata q187 = new OXQuizMetadata()
            {
                Id = 187,
                QuestionText = "All bees can sting only once.",
                Answer = false,
                AnswerText = "Hornets and queen bees can sting multiple times.",
                Category = "Science"
            };
            questions.Add(q187);

            OXQuizMetadata q188 = new OXQuizMetadata()
            {
                Id = 188,
                QuestionText = "All gear can be enchanted for better performance.",
                Answer = false,
                AnswerText = "Only weapons, suits, tops, boots, gloves, and bottoms at level 20 or above can be enchanted.",
                Category = "MapleStory 2"
            };
            questions.Add(q188);

            OXQuizMetadata q189 = new OXQuizMetadata()
            {
                Id = 189,
                QuestionText = "All monkey buttocks are red.",
                Answer = false,
                AnswerText = "Only some monkey buttocks are red. Orangutans and gorillas have dark faces and buttocks.",
                Category = "General Knowledge"
            };
            questions.Add(q189);

            OXQuizMetadata q190 = new OXQuizMetadata()
            {
                Id = 190,
                QuestionText = "All the 10-player dungeons have the same BGM.",
                Answer = false,
                AnswerText = "Not all 10-player dungeons have the same BGM.",
                Category = "MapleStory 2"
            };
            questions.Add(q190);

            OXQuizMetadata q191 = new OXQuizMetadata()
            {
                Id = 191,
                QuestionText = "Alvin Toffler is a futurist who wrote \"Future Shock,\" \"The Third Wave,\" and \"Powershift.\"",
                Answer = true,
                AnswerText = "Alvin Toffler is a futurist who wrote \"Future Shock,\" \"The Third Wave,\" and \"Powershift.\"",
                Category = "Culture/Art"
            };
            questions.Add(q191);

            OXQuizMetadata q192 = new OXQuizMetadata()
            {
                Id = 192,
                QuestionText = "Amadeus is a biographical movie about Niccolo Paganini.",
                Answer = false,
                AnswerText = "Amadeus is a biographical movie about Wolfgang Mozart.",
                Category = "Culture/Art"
            };
            questions.Add(q192);

            OXQuizMetadata q193 = new OXQuizMetadata()
            {
                Id = 193,
                QuestionText = "Amadeus is a biographical movie about Wolfgang Mozart.",
                Answer = true,
                AnswerText = "Amadeus is a biographical movie about Wolfgang Mozart.",
                Category = "Culture/Art"
            };
            questions.Add(q193);

            OXQuizMetadata q194 = new OXQuizMetadata()
            {
                Id = 194,
                QuestionText = "Amazon jackets are jackets that help hunters endure hot climate.",
                Answer = false,
                AnswerText = "Safari jackets, considered sporty fashion, are used for hunting and traveling in Africa. Made of practical fibers, they have many uses.",
                Category = "General Knowledge"
            };
            questions.Add(q194);

            OXQuizMetadata q195 = new OXQuizMetadata()
            {
                Id = 195,
                QuestionText = "America is the continent with the largest population.",
                Answer = false,
                AnswerText = "Asia is the continent with the largest population.",
                Category = "General Knowledge"
            };
            questions.Add(q195);

            OXQuizMetadata q196 = new OXQuizMetadata()
            {
                Id = 196,
                QuestionText = "Among Asian nations, China has won the highest number of gold medals in Winter Olympics.",
                Answer = false,
                AnswerText = "So far among Asian nations, Korea has won the highest number of gold medals in the Winter Olympics.",
                Category = "General Knowledge"
            };
            questions.Add(q196);

            OXQuizMetadata q197 = new OXQuizMetadata()
            {
                Id = 197,
                QuestionText = "Ampere is a unit used to measure the size of the electric potential difference.",
                Answer = false,
                AnswerText = "Ampere, a unit used to measure the apparent power in the electrical circuit, is named after the French Physics physicist André-Marie Ampère.",
                Category = "General Knowledge"
            };
            questions.Add(q197);

            OXQuizMetadata q198 = new OXQuizMetadata()
            {
                Id = 198,
                QuestionText = "Amperes is a unit used to measure the apparent power in an electrical circuit.",
                Answer = true,
                AnswerText = "Ampere is named after the French physicist André-Marie Ampère.",
                Category = "General Knowledge"
            };
            questions.Add(q198);

            OXQuizMetadata q199 = new OXQuizMetadata()
            {
                Id = 199,
                QuestionText = "An American first invented TV.",
                Answer = false,
                AnswerText = "John Logie Baird from Scotland first invented a primitive TV.",
                Category = "History"
            };
            questions.Add(q199);

            OXQuizMetadata q200 = new OXQuizMetadata()
            {
                Id = 200,
                QuestionText = "An American songwriter composed \"Jeanie with the Light Brown Hair.\"",
                Answer = true,
                AnswerText = "An American songwriter, Stephen Foster, composed \"Jeanie with the Light Brown Hair.\"",
                Category = "Culture/Art"
            };
            questions.Add(q200);

            OXQuizMetadata q201 = new OXQuizMetadata()
            {
                Id = 201,
                QuestionText = "An apple falling from a tree taught Archimedes the concept of gravity.",
                Answer = false,
                AnswerText = "An apple falling from a tree inspired Newton to describe the concept of gravity.",
                Category = "History"
            };
            questions.Add(q201);

            OXQuizMetadata q202 = new OXQuizMetadata()
            {
                Id = 202,
                QuestionText = "An assassin killed Abraham Lincoln.",
                Answer = true,
                AnswerText = "On April 14th, Abraham Lincoln was assassinated in a theater.",
                Category = "History"
            };
            questions.Add(q202);

            OXQuizMetadata q203 = new OXQuizMetadata()
            {
                Id = 203,
                QuestionText = "An example of Newton's Third Law is a rocket engine, as the engine accelerates forward by discharging fuel.",
                Answer = false,
                AnswerText = "Rocket engines use the law of action and reaction.",
                Category = "Science"
            };
            questions.Add(q203);

            OXQuizMetadata q204 = new OXQuizMetadata()
            {
                Id = 204,
                QuestionText = "An idea that best contrasts communism is democracy.",
                Answer = false,
                AnswerText = "An idea that best contrasts communism is capitalism.",
                Category = "Society"
            };
            questions.Add(q204);

            OXQuizMetadata q205 = new OXQuizMetadata()
            {
                Id = 205,
                QuestionText = "Ancien Regime is the political and social system of France after the French Revolution.",
                Answer = false,
                AnswerText = "Ancien Régime refers to the French monarchy prior to the French revolution.",
                Category = "History"
            };
            questions.Add(q205);

            OXQuizMetadata q206 = new OXQuizMetadata()
            {
                Id = 206,
                QuestionText = "Ancien Regime was the political and social system of France before the French revolution.",
                Answer = true,
                AnswerText = "Ancien Régime refers to the French monarchy prior to the French Revolution.",
                Category = "History"
            };
            questions.Add(q206);

            OXQuizMetadata q207 = new OXQuizMetadata()
            {
                Id = 207,
                QuestionText = "Andante means to play at a slow tempo.",
                Answer = true,
                AnswerText = "Andante means to play at a slow tempo.",
                Category = "Culture/Art"
            };
            questions.Add(q207);

            OXQuizMetadata q208 = new OXQuizMetadata()
            {
                Id = 208,
                QuestionText = "Andante means to play in a fast tempo.",
                Answer = false,
                AnswerText = "Andante means to play in a slow tempo.",
                Category = "Culture/Art"
            };
            questions.Add(q208);

            OXQuizMetadata q209 = new OXQuizMetadata()
            {
                Id = 209,
                QuestionText = "Andre Gide wrote \"Demian.\"",
                Answer = false,
                AnswerText = "Hermann Hesse wrote \"Demian.\"",
                Category = "Culture/Art"
            };
            questions.Add(q209);

            OXQuizMetadata q210 = new OXQuizMetadata()
            {
                Id = 210,
                QuestionText = "Angelina Jolie plays Lara Croft in the film \"Tomb Raider.\"",
                Answer = true,
                AnswerText = "The role fits her image.",
                Category = "Culture/Art"
            };
            questions.Add(q210);

            OXQuizMetadata q211 = new OXQuizMetadata()
            {
                Id = 211,
                QuestionText = "Anne Frank, the owner of the famous diary was Jewish.",
                Answer = true,
                AnswerText = "Her diary depicts the lives of the Jews at the time.",
                Category = "History"
            };
            questions.Add(q211);

            OXQuizMetadata q212 = new OXQuizMetadata()
            {
                Id = 212,
                QuestionText = "Antonin Dvorak, the composer of \"New World Symphony,\" is from the Czech Republic.",
                Answer = true,
                AnswerText = "Antonin Dvorak, the composer of \"New World Symphony,\" is from the Czech Republic.",
                Category = "Culture/Art"
            };
            questions.Add(q212);

            OXQuizMetadata q213 = new OXQuizMetadata()
            {
                Id = 213,
                QuestionText = "Antonin Dvorak, the composer of \"New World Symphony,\" is from Yugoslavia.",
                Answer = false,
                AnswerText = "Antonin Dvorak, the composer of \"New World Symphony,\" is from the Czech Republic.",
                Category = "Culture/Art"
            };
            questions.Add(q213);

            OXQuizMetadata q214 = new OXQuizMetadata()
            {
                Id = 214,
                QuestionText = "Antonio de Saint-Exupery wrote The Flowers of Evil.",
                Answer = false,
                AnswerText = "Charles Baudelaire wrote The Flowers of Evil.",
                Category = "Culture/Art"
            };
            questions.Add(q214);

            OXQuizMetadata q215 = new OXQuizMetadata()
            {
                Id = 215,
                QuestionText = "Antonio Gaudi, who had been active during the 18th century, was a Swiss architect.",
                Answer = false,
                AnswerText = "Antonio Gaudi, who had been active from the end of 19th century to the early 20th century, was a Spanish architect.",
                Category = "General Knowledge"
            };
            questions.Add(q215);

            OXQuizMetadata q216 = new OXQuizMetadata()
            {
                Id = 216,
                QuestionText = "Antonio Salieri and Wolfgang Mozart are contemporaries.",
                Answer = true,
                AnswerText = "Antonio Salieri and Wolfgang Mozart are contemporaries.",
                Category = "Culture/Art"
            };
            questions.Add(q216);

            OXQuizMetadata q217 = new OXQuizMetadata()
            {
                Id = 217,
                QuestionText = "Antonio Salieri and Wolfgang Mozart are not contemporaries.",
                Answer = false,
                AnswerText = "Antonio Salieri and Wolfgang Mozart are contemporaries.",
                Category = "Culture/Art"
            };
            questions.Add(q217);

            OXQuizMetadata q218 = new OXQuizMetadata()
            {
                Id = 218,
                QuestionText = "Ants have eight legs.",
                Answer = false,
                AnswerText = "Ants are insects. They have six legs.",
                Category = "Science"
            };
            questions.Add(q218);

            OXQuizMetadata q219 = new OXQuizMetadata()
            {
                Id = 219,
                QuestionText = "Any citizen can make an indictment, which is a formal accusation.",
                Answer = false,
                AnswerText = "An indictment is a formal accusation made to the court by a prosecutor.",
                Category = "General Knowledge"
            };
            questions.Add(q219);

            OXQuizMetadata q220 = new OXQuizMetadata()
            {
                Id = 220,
                QuestionText = "Any house password set up through the Surveillance Camera is canceled after 24 hours.",
                Answer = true,
                AnswerText = "Any house password set up through the Surveillance Camera is canceled after 24 hours.",
                Category = "MapleStory 2"
            };
            questions.Add(q220);

            OXQuizMetadata q221 = new OXQuizMetadata()
            {
                Id = 221,
                QuestionText = "Arabesque architecture is an architectural style characterized by horizontal lines and Roman architecture styles.",
                Answer = false,
                AnswerText = "Romanesque Architecture is an architectural style of Medieval Europe that is thought to have started from France in the late 10th century, and spread to Western Europe by the mid 12th century.",
                Category = "Culture/Art"
            };
            questions.Add(q221);

            OXQuizMetadata q222 = new OXQuizMetadata()
            {
                Id = 222,
                QuestionText = "Arabesque means \"Arabic\" and is used as the title for the compositions with fanciful ornamentation of the melody.",
                Answer = true,
                AnswerText = "Arabesque means \"Arabic\" and is used as the title for the compositions with fanciful ornamentation of the melody.",
                Category = "Culture/Art"
            };
            questions.Add(q222);

            OXQuizMetadata q223 = new OXQuizMetadata()
            {
                Id = 223,
                QuestionText = "Archmage Asimov is an important figure in Ellinia.",
                Answer = true,
                AnswerText = "Archmage Asimov is an important figure in Ellinia.",
                Category = "MapleStory 2"
            };
            questions.Add(q223);

            OXQuizMetadata q224 = new OXQuizMetadata()
            {
                Id = 224,
                QuestionText = "Argentina's capital is Buenos Aires.",
                Answer = true,
                AnswerText = "Argentina's capital is Buenos Aires.",
                Category = "General Knowledge"
            };
            questions.Add(q224);

            OXQuizMetadata q225 = new OXQuizMetadata()
            {
                Id = 225,
                QuestionText = "Argentina's capital is not Buenos Aires.",
                Answer = false,
                AnswerText = "Argentina's capital is Buenos Aires.",
                Category = "General Knowledge"
            };
            questions.Add(q225);

            OXQuizMetadata q226 = new OXQuizMetadata()
            {
                Id = 226,
                QuestionText = "Arguably one of the most successful composers of our time, Andrew Lloyd Webber's works includes \"An American in Paris.\"",
                Answer = false,
                AnswerText = "An American in Paris is an orchestra music by George Gershwin.",
                Category = "Culture/Art"
            };
            questions.Add(q226);

            OXQuizMetadata q227 = new OXQuizMetadata()
            {
                Id = 227,
                QuestionText = "Arpeggio, which refers to the tempo in between andante and largo, means \"at ease.\"",
                Answer = false,
                AnswerText = "Adagio, which refers to the tempo in between andante and largo, means \"at ease.\"",
                Category = "Culture/Art"
            };
            questions.Add(q227);

            OXQuizMetadata q228 = new OXQuizMetadata()
            {
                Id = 228,
                QuestionText = "As kimchi ferments, its PH value increases.",
                Answer = false,
                AnswerText = "Kimchi turns more acidic as it ferments. Its PH value decreases.",
                Category = "Science"
            };
            questions.Add(q228);

            OXQuizMetadata q229 = new OXQuizMetadata()
            {
                Id = 229,
                QuestionText = "As of August 2014, male idol group EXO broke the previous record set by TVXQ by having more than 940 thousand fan club members.",
                Answer = true,
                AnswerText = "On August 2014, male idol group EXO's fan club exceeded 945 thousand members and broke the previous Guinness record set by TVXQ.",
                Category = "History"
            };
            questions.Add(q229);

            OXQuizMetadata q230 = new OXQuizMetadata()
            {
                Id = 230,
                QuestionText = "Asians, during their invasion of India, introduced the rigid social class hierarchy called the caste system.",
                Answer = false,
                AnswerText = "Around 1300 BC, the Aryans invaded India. Their invasion introduced India the rigid social class hierarchy called the caste system.",
                Category = "History"
            };
            questions.Add(q230);

            OXQuizMetadata q231 = new OXQuizMetadata()
            {
                Id = 231,
                QuestionText = "Assistant Growlie goes on strike if he's not paid.",
                Answer = true,
                AnswerText = "He goes on strike.",
                Category = "MapleStory 2"
            };
            questions.Add(q231);

            OXQuizMetadata q232 = new OXQuizMetadata()
            {
                Id = 232,
                QuestionText = "Astronomy first started in the Babylonian civilization of Mesopotamia.",
                Answer = true,
                AnswerText = "Babylonia's astronomy led to the advancement of mathematical models and scientific applications. Some of the examples are sexagesimal (base 60) and the use of lunar calendars.",
                Category = "History"
            };
            questions.Add(q232);

            OXQuizMetadata q233 = new OXQuizMetadata()
            {
                Id = 233,
                QuestionText = "Audiences can't hear the instrumentals during the playing of \"Four thirty-three,\" a song John Cage composed in 1952.",
                Answer = true,
                AnswerText = "The song is composed of three movements which are 33 seconds, 2 minute 40 seconds and 1 minute 20 seconds of tacet (silence).",
                Category = "Culture/Art"
            };
            questions.Add(q233);

            OXQuizMetadata q234 = new OXQuizMetadata()
            {
                Id = 234,
                QuestionText = "Auguste Rodin, the father of contemporary sculpting, left many magnificent sculptures including The Gates of Hell, The Age of Bronze, and The Burghers of Calais.",
                Answer = true,
                AnswerText = "Auguste Rodin, the father of contemporary sculpting, left many magnificent sculptures including The Gates of Hell, The Age of Bronze, and The Burghers of Calais.",
                Category = "History"
            };
            questions.Add(q234);

            OXQuizMetadata q235 = new OXQuizMetadata()
            {
                Id = 235,
                QuestionText = "Auguste Rodin's \"The Thinker\" rests his chin on his left hand.",
                Answer = false,
                AnswerText = "Auguste Rodin's \"The Thinker\" rests his chin on his right hand.",
                Category = "History"
            };
            questions.Add(q235);

            OXQuizMetadata q236 = new OXQuizMetadata()
            {
                Id = 236,
                QuestionText = "Auguste Rodin's the Thinker sits with his legs crossed.",
                Answer = false,
                AnswerText = "Auguste Rodin's the Thinker doesn't sit with his legs crossed, but has his chin resting on one hand.",
                Category = "History"
            };
            questions.Add(q236);

            OXQuizMetadata q237 = new OXQuizMetadata()
            {
                Id = 237,
                QuestionText = "Australopithecus's scientific name is Austoralopithecus thalensis.",
                Answer = false,
                AnswerText = "Australopithecus's scientific name is Australopithecus afarensis.",
                Category = "History"
            };
            questions.Add(q237);

            OXQuizMetadata q238 = new OXQuizMetadata()
            {
                Id = 238,
                QuestionText = "Australopithecus's scientific name is Australiapithecus afarensis.",
                Answer = false,
                AnswerText = "Australopithecus's scientific name is Australopithecus afarensis.",
                Category = "History"
            };
            questions.Add(q238);

            OXQuizMetadata q239 = new OXQuizMetadata()
            {
                Id = 239,
                QuestionText = "Australopithecus's scientific name is Australiapithecus thalensis.",
                Answer = false,
                AnswerText = "Australopithecus's scientific name is Australopithecus afarensis.",
                Category = "History"
            };
            questions.Add(q239);

            OXQuizMetadata q240 = new OXQuizMetadata()
            {
                Id = 240,
                QuestionText = "Australopithecus's scientific name is Australopithecus afarensis.",
                Answer = true,
                AnswerText = "Australopithecus's scientific name is Australopithecus afarensis.",
                Category = "History"
            };
            questions.Add(q240);

            OXQuizMetadata q241 = new OXQuizMetadata()
            {
                Id = 241,
                QuestionText = "Australopithecus's scientific name is Australopithecus thalensis.",
                Answer = false,
                AnswerText = "Australopithecus's scientific name is Australopithecus afarensis.",
                Category = "History"
            };
            questions.Add(q241);

            OXQuizMetadata q242 = new OXQuizMetadata()
            {
                Id = 242,
                QuestionText = "Australopithecus's scientific name is Austriapithecus afarensis.",
                Answer = false,
                AnswerText = "Australopithecus's scientific name is Australopithecus afarensis.",
                Category = "History"
            };
            questions.Add(q242);

            OXQuizMetadata q243 = new OXQuizMetadata()
            {
                Id = 243,
                QuestionText = "Babies have fewer bones than adults.",
                Answer = false,
                AnswerText = "The bones merge and become fewer in number.",
                Category = "General Knowledge"
            };
            questions.Add(q243);

            OXQuizMetadata q244 = new OXQuizMetadata()
            {
                Id = 244,
                QuestionText = "Babies have more bones than adults.",
                Answer = true,
                AnswerText = "The bones merge and become fewer in number as babies grow.",
                Category = "General Knowledge"
            };
            questions.Add(q244);

            OXQuizMetadata q245 = new OXQuizMetadata()
            {
                Id = 245,
                QuestionText = "Bald People don't experience dandruff.",
                Answer = false,
                AnswerText = "Even the bald experience dandruff.",
                Category = "General Knowledge"
            };
            questions.Add(q245);

            OXQuizMetadata q246 = new OXQuizMetadata()
            {
                Id = 246,
                QuestionText = "Bangkok is the capital of Thailand.",
                Answer = true,
                AnswerText = "Bangkok is the capital of Thailand.",
                Category = "General Knowledge"
            };
            questions.Add(q246);

            OXQuizMetadata q247 = new OXQuizMetadata()
            {
                Id = 247,
                QuestionText = "Bart Simpson's best friend is Milhouse.",
                Answer = true,
                AnswerText = "Bart and Milhouse are usually causing mischief around Springfield.",
                Category = "Culture/Art"
            };
            questions.Add(q247);

            OXQuizMetadata q248 = new OXQuizMetadata()
            {
                Id = 248,
                QuestionText = "Baseball was dropped from the Olympics after the 2008 Beijing Olympics.",
                Answer = true,
                AnswerText = "Baseball was dropped from the Olympics after the 2008 Beijing Olympics.",
                Category = "Culture/Art"
            };
            questions.Add(q248);

            OXQuizMetadata q249 = new OXQuizMetadata()
            {
                Id = 249,
                QuestionText = "Bass is one of the instruments in a piano trio.",
                Answer = false,
                AnswerText = "The piano trio are piano, cello, and violin.",
                Category = "Culture/Art"
            };
            questions.Add(q249);

            OXQuizMetadata q250 = new OXQuizMetadata()
            {
                Id = 250,
                QuestionText = "Batman's butler is named Albert.",
                Answer = false,
                AnswerText = "Batman's butler is named Alfred.",
                Category = "Culture/Art"
            };
            questions.Add(q250);

            OXQuizMetadata q251 = new OXQuizMetadata()
            {
                Id = 251,
                QuestionText = "Beauty Street's BGM has 120 BPM.",
                Answer = false,
                AnswerText = "Beauty Street's BGM has 130 BPM.",
                Category = "MapleStory 2"
            };
            questions.Add(q251);

            OXQuizMetadata q252 = new OXQuizMetadata()
            {
                Id = 252,
                QuestionText = "Because of its light and playful tone, the viola is often compared to Sopranos.",
                Answer = false,
                AnswerText = "Viola has an alto tone that's dark, but warm and full.",
                Category = "General Knowledge"
            };
            questions.Add(q252);

            OXQuizMetadata q253 = new OXQuizMetadata()
            {
                Id = 253,
                QuestionText = "Because they are high in saturated fat and satiating, peanuts and walnuts are good for diets.",
                Answer = false,
                AnswerText = "Because they are high in unsaturated fat and satiating, peanuts and walnuts are good for a diet.",
                Category = "General Knowledge"
            };
            questions.Add(q253);

            OXQuizMetadata q254 = new OXQuizMetadata()
            {
                Id = 254,
                QuestionText = "Bedrich Smetana is Russia's famous composer.",
                Answer = false,
                AnswerText = "Bedrich Smetana is Czechoslovakia's famous composer.",
                Category = "Culture/Art"
            };
            questions.Add(q254);

            OXQuizMetadata q255 = new OXQuizMetadata()
            {
                Id = 255,
                QuestionText = "Beethoven and Joseph Haydn are two exemplary musicians of the classical era.",
                Answer = true,
                AnswerText = "Beethoven and Joseph Haydn are two exemplary musicians of the classical era.",
                Category = "Culture/Art"
            };
            questions.Add(q255);

            OXQuizMetadata q256 = new OXQuizMetadata()
            {
                Id = 256,
                QuestionText = "Beethoven and Joseph Haydn are two exemplary musicians of the Renaissance era.",
                Answer = false,
                AnswerText = "Beethoven and Joseph Haydn are two exemplary musicians of classicism era.",
                Category = "Culture/Art"
            };
            questions.Add(q256);

            OXQuizMetadata q257 = new OXQuizMetadata()
            {
                Id = 257,
                QuestionText = "Beethoven was the first person to use a metronome.",
                Answer = true,
                AnswerText = "Beethoven was the first person to use a metronome.",
                Category = "Culture/Art"
            };
            questions.Add(q257);

            OXQuizMetadata q258 = new OXQuizMetadata()
            {
                Id = 258,
                QuestionText = "Beethoven's piece \"Eroica\" is related to Alexander the Great.",
                Answer = false,
                AnswerText = "Beethoven's piece \"Eroica\" is related to Napoleon.",
                Category = "Culture/Art"
            };
            questions.Add(q258);

            OXQuizMetadata q259 = new OXQuizMetadata()
            {
                Id = 259,
                QuestionText = "Beethoven's piece \"Eroica\" is related to Napoleon.",
                Answer = true,
                AnswerText = "Beethoven's piece \"Eroica\" is related to Napoleon.",
                Category = "Culture/Art"
            };
            questions.Add(q259);

            OXQuizMetadata q260 = new OXQuizMetadata()
            {
                Id = 260,
                QuestionText = "Before playing a piece, orchestras use the oboe's sound to tune their instruments.",
                Answer = true,
                AnswerText = "The instrument, even when stored in different environments, rarely changes its tune.",
                Category = "Culture/Art"
            };
            questions.Add(q260);

            OXQuizMetadata q261 = new OXQuizMetadata()
            {
                Id = 261,
                QuestionText = "Before playing a piece, orchestras use the piano's sound to tune their instruments.",
                Answer = false,
                AnswerText = "They use the oboe, which rarely changes its tune.",
                Category = "Culture/Art"
            };
            questions.Add(q261);

            OXQuizMetadata q262 = new OXQuizMetadata()
            {
                Id = 262,
                QuestionText = "Benelux refers to the three neighboring monarchies: Belgium, the Netherlands, and Luxembourg.",
                Answer = true,
                AnswerText = "Benelux refers to the three neighboring monarchies: Belgium, the Netherlands, and Luxembourg.",
                Category = "General Knowledge"
            };
            questions.Add(q262);

            OXQuizMetadata q263 = new OXQuizMetadata()
            {
                Id = 263,
                QuestionText = "Benelux refers to three neighboring monarchies: Belgium, Liechtenstein, and Luxembourg.",
                Answer = false,
                AnswerText = "Benelux refers to the three neighboring monarchies: Belgium, the Netherlands, and Luxembourg.",
                Category = "General Knowledge"
            };
            questions.Add(q263);

            OXQuizMetadata q264 = new OXQuizMetadata()
            {
                Id = 264,
                QuestionText = "Bill Clinton, a former President of United States, served a single term of presidency.",
                Answer = false,
                AnswerText = "Bill Clinton served two terms, in 1992 and 1996.",
                Category = "General Knowledge"
            };
            questions.Add(q264);

            OXQuizMetadata q265 = new OXQuizMetadata()
            {
                Id = 265,
                QuestionText = "Bill Clinton, a former President of United States, served two terms of presidency.",
                Answer = true,
                AnswerText = "Bill Clinton served two terms, in 1992 and 1996.",
                Category = "General Knowledge"
            };
            questions.Add(q265);

            OXQuizMetadata q266 = new OXQuizMetadata()
            {
                Id = 266,
                QuestionText = "Black Eye has tattoos on the backs of his hands.",
                Answer = false,
                AnswerText = "Black Eye has tattoos on his arms.",
                Category = "MapleStory 2"
            };
            questions.Add(q266);

            OXQuizMetadata q267 = new OXQuizMetadata()
            {
                Id = 267,
                QuestionText = "Black is a color that absorbs sunlight well.",
                Answer = true,
                AnswerText = "Black absorbs sunlight better than other colors. That's why it's black.",
                Category = "Science"
            };
            questions.Add(q267);

            OXQuizMetadata q268 = new OXQuizMetadata()
            {
                Id = 268,
                QuestionText = "Black is a color that reflects sunlight well.",
                Answer = false,
                AnswerText = "Black absorbs sunlight better than other colors. That's why it's black.",
                Category = "Science"
            };
            questions.Add(q268);

            OXQuizMetadata q269 = new OXQuizMetadata()
            {
                Id = 269,
                QuestionText = "Blake is currently leading two different lives.",
                Answer = true,
                AnswerText = "Blake is working as a singer and an assistant at the same time.",
                Category = "MapleStory 2"
            };
            questions.Add(q269);

            OXQuizMetadata q270 = new OXQuizMetadata()
            {
                Id = 270,
                QuestionText = "Blizzard Entertainment developed StarCraft.",
                Answer = true,
                AnswerText = "StarCraft is a real-time strategy game developed by Blizzard Entertainment.",
                Category = "General Knowledge"
            };
            questions.Add(q270);

            OXQuizMetadata q271 = new OXQuizMetadata()
            {
                Id = 271,
                QuestionText = "Blood platelets are tiny blood cells that help our body form clots to stop bleeding, if necessary.",
                Answer = true,
                AnswerText = "Platelets (a type of blood cell) and proteins in your plasma (the liquid part of blood) work together to stop the bleeding by forming a clot over the injury.",
                Category = "Science"
            };
            questions.Add(q271);

            OXQuizMetadata q272 = new OXQuizMetadata()
            {
                Id = 272,
                QuestionText = "Blood platelets, a component of our blood, don't have nuclei.",
                Answer = true,
                AnswerText = "Blood platelets are cells about 2 to 4 micrometers in size. They are made from bone marrow and lack nuclei.",
                Category = "Science"
            };
            questions.Add(q272);

            OXQuizMetadata q273 = new OXQuizMetadata()
            {
                Id = 273,
                QuestionText = "BMW is an American auto manufacturer.",
                Answer = false,
                AnswerText = "BMW is a German auto manufacturer.",
                Category = "General Knowledge"
            };
            questions.Add(q273);

            OXQuizMetadata q274 = new OXQuizMetadata()
            {
                Id = 274,
                QuestionText = "Bossa Nova is a genre of Mexican music.",
                Answer = false,
                AnswerText = "Bossa Nova is a genre of Brazilian music.",
                Category = "Culture/Art"
            };
            questions.Add(q274);

            OXQuizMetadata q275 = new OXQuizMetadata()
            {
                Id = 275,
                QuestionText = "Both Chairman Goldus and Mayor Marco have a secretary.",
                Answer = true,
                AnswerText = "Both Chairman Goldus and Mayor Marco have a secretary.",
                Category = "MapleStory 2"
            };
            questions.Add(q275);

            OXQuizMetadata q276 = new OXQuizMetadata()
            {
                Id = 276,
                QuestionText = "Boxing ring is in a circular shape.",
                Answer = false,
                AnswerText = "It's a square.",
                Category = "General Knowledge"
            };
            questions.Add(q276);

            OXQuizMetadata q277 = new OXQuizMetadata()
            {
                Id = 277,
                QuestionText = "Boyle's Law states that at a constant temperature for a fixed mass, the absolute pressure and the volume of a gas are inversely proportional.",
                Answer = true,
                AnswerText = "Boyle's Law states that at a constant temperature for a fixed mass, the absolute pressure and the volume of a gas are inversely proportional.",
                Category = "Science"
            };
            questions.Add(q277);

            OXQuizMetadata q278 = new OXQuizMetadata()
            {
                Id = 278,
                QuestionText = "Brasilia is the capital of Brazil.",
                Answer = true,
                AnswerText = "Brasilia is the capital of Brazil.",
                Category = "General Knowledge"
            };
            questions.Add(q278);

            OXQuizMetadata q279 = new OXQuizMetadata()
            {
                Id = 279,
                QuestionText = "Brazil won the 1930 World Cup in Uruguay.",
                Answer = false,
                AnswerText = "Uruguay won the 1930 World Cup Uruguay.",
                Category = "Culture/Art"
            };
            questions.Add(q279);

            OXQuizMetadata q280 = new OXQuizMetadata()
            {
                Id = 280,
                QuestionText = "Brazilians use Portuguese as their primary language.",
                Answer = true,
                AnswerText = "Because of the Portuguese invasion of Brazil, Brazilians use Portuguese as their primary language.",
                Category = "General Knowledge"
            };
            questions.Add(q280);

            OXQuizMetadata q281 = new OXQuizMetadata()
            {
                Id = 281,
                QuestionText = "Brazilians use Spanish as their primary language.",
                Answer = false,
                AnswerText = "Because of the Portuguese invasion of Brazil, Brazilians use Portuguese as their primary language.",
                Category = "General Knowledge"
            };
            questions.Add(q281);

            OXQuizMetadata q282 = new OXQuizMetadata()
            {
                Id = 282,
                QuestionText = "Bread in Korean is \"ppang.\" The word is purely domestic.",
                Answer = false,
                AnswerText = "Ppang is a loanword borrowed from Portuguese.",
                Category = "General Knowledge"
            };
            questions.Add(q282);

            OXQuizMetadata q283 = new OXQuizMetadata()
            {
                Id = 283,
                QuestionText = "British currency shows Queen Elizabeth as she ages.",
                Answer = true,
                AnswerText = "British currency shows Queen Elizabeth as she ages.",
                Category = "General Knowledge"
            };
            questions.Add(q283);

            OXQuizMetadata q284 = new OXQuizMetadata()
            {
                Id = 284,
                QuestionText = "Bruce Wayne is Superman's name.",
                Answer = false,
                AnswerText = "It's Batman's name.",
                Category = "Culture/Art"
            };
            questions.Add(q284);

            OXQuizMetadata q285 = new OXQuizMetadata()
            {
                Id = 285,
                QuestionText = "Bruce Willis plays Dr. Jones in the film \"Indiana Jones.\"",
                Answer = false,
                AnswerText = "Harrison Ford plays the role.",
                Category = "Culture/Art"
            };
            questions.Add(q285);

            OXQuizMetadata q286 = new OXQuizMetadata()
            {
                Id = 286,
                QuestionText = "Bulgarian scientist Alessandro Volta invented the world's first electric battery.",
                Answer = false,
                AnswerText = "In 1800, Italian scientist Alessandro Volta invented the world's first electric battery.",
                Category = "Science"
            };
            questions.Add(q286);

            OXQuizMetadata q287 = new OXQuizMetadata()
            {
                Id = 287,
                QuestionText = "Buzz Lightyear is a character from Toy Story.",
                Answer = true,
                AnswerText = "Buzz Lightyear is a character from Toy Story.",
                Category = "General Knowledge"
            };
            questions.Add(q287);

            OXQuizMetadata q288 = new OXQuizMetadata()
            {
                Id = 288,
                QuestionText = "C-3PO is fluent in over six million forms of communication.",
                Answer = true,
                AnswerText = "When being sold, C-3PO says he is fluent in over six million forms of communication.",
                Category = "General Knowledge"
            };
            questions.Add(q288);

            OXQuizMetadata q289 = new OXQuizMetadata()
            {
                Id = 289,
                QuestionText = "Calt Disney founded Disney.",
                Answer = false,
                AnswerText = "It's Walt Disney.",
                Category = "General Knowledge"
            };
            questions.Add(q289);

            OXQuizMetadata q290 = new OXQuizMetadata()
            {
                Id = 290,
                QuestionText = "Camel humps consist of flesh.",
                Answer = false,
                AnswerText = "Camel humps consist of fat.",
                Category = "General Knowledge"
            };
            questions.Add(q290);

            OXQuizMetadata q291 = new OXQuizMetadata()
            {
                Id = 291,
                QuestionText = "Canada's 100 dollar bill smells like maple syrup.",
                Answer = false,
                AnswerText = "The bill smells like paper despite the rumor.",
                Category = "General Knowledge"
            };
            questions.Add(q291);

            OXQuizMetadata q292 = new OXQuizMetadata()
            {
                Id = 292,
                QuestionText = "Cantata is a vocal music usually sung with choir, instrumental accompaniment and in solo.",
                Answer = true,
                AnswerText = "Cantata is a narrative piece of music for voices with instrumental accompaniment, typically with solos, chorus, and orchestra. Its name came from the Italian word cantare, meaning to sing.",
                Category = "Culture/Art"
            };
            questions.Add(q292);

            OXQuizMetadata q293 = new OXQuizMetadata()
            {
                Id = 293,
                QuestionText = "Cantatas lack lyrics.",
                Answer = false,
                AnswerText = "Cantata is a vocal music.",
                Category = "Culture/Art"
            };
            questions.Add(q293);

            OXQuizMetadata q294 = new OXQuizMetadata()
            {
                Id = 294,
                QuestionText = "Canzone is a Portuguese folk song.",
                Answer = false,
                AnswerText = "Canzone is an Italian folk song. Its lyrics are mostly romantic and straightforward and its melody, which is joyous, is easy to sing.",
                Category = "Culture/Art"
            };
            questions.Add(q294);

            OXQuizMetadata q295 = new OXQuizMetadata()
            {
                Id = 295,
                QuestionText = "Captain Jack Sparrow is played by Robert Downey Jr.",
                Answer = false,
                AnswerText = "Captain Jack Sparrow is played by Johnny Depp.",
                Category = "Culture/Art"
            };
            questions.Add(q295);

            OXQuizMetadata q296 = new OXQuizMetadata()
            {
                Id = 296,
                QuestionText = "Captain Moc holds a cannon in one hand.",
                Answer = true,
                AnswerText = "Captain Moc holds a cannon in one hand.",
                Category = "MapleStory 2"
            };
            questions.Add(q296);

            OXQuizMetadata q297 = new OXQuizMetadata()
            {
                Id = 297,
                QuestionText = "Cats dream in their sleep.",
                Answer = true,
                AnswerText = "Having the part of the brain that dreams, cats dream in their sleep.",
                Category = "Science"
            };
            questions.Add(q297);

            OXQuizMetadata q298 = new OXQuizMetadata()
            {
                Id = 298,
                QuestionText = "Caused by algal blooms, red ocean refers to a phenomenon during which overt population of algae discolors coastal waters.",
                Answer = false,
                AnswerText = "The fish near the area affected by red tide suffocate to death because the algae consume all the dissolved oxygen.",
                Category = "Science"
            };
            questions.Add(q298);

            OXQuizMetadata q299 = new OXQuizMetadata()
            {
                Id = 299,
                QuestionText = "Cell phones can explode.",
                Answer = true,
                AnswerText = "Cell phones can explode if put in an environment over 170 °C or if the battery is stored with a key or coin.",
                Category = "General Knowledge"
            };
            questions.Add(q299);

            OXQuizMetadata q300 = new OXQuizMetadata()
            {
                Id = 300,
                QuestionText = "Cellos cannot exceed a single octave range.",
                Answer = false,
                AnswerText = "Cellos have a wide range of tones that span 4 octaves.",
                Category = "General Knowledge"
            };
            questions.Add(q300);

            OXQuizMetadata q301 = new OXQuizMetadata()
            {
                Id = 301,
                QuestionText = "Celsius defines 0 as the freezing point of water at 1 atm pressure and 100 as the boiling point of water at 1 atm pressure. The two points are divided into 100 degrees to serve as a scale.",
                Answer = true,
                AnswerText = "Swedish scientist Anders Celsius created a temperature scale where 0 represented the freezing point of water, and 100 represented the boiling point of water, and the two points are divided into 100 degrees as a",
                Category = "Science"
            };
            questions.Add(q301);

            OXQuizMetadata q302 = new OXQuizMetadata()
            {
                Id = 302,
                QuestionText = "Celsius defines 32 degrees as the freezing point of water at 1 atm pressure and 180 as the boiling point of water at 1 atm pressure. The two points are divided into 180 degrees to serve as a scale.",
                Answer = false,
                AnswerText = "Celsius is the temperature scale where 0 represented the freezing point of water and 100 represented the boiling point of water, and the two points are divided into 100 degrees as a scale.",
                Category = "Science"
            };
            questions.Add(q302);

            OXQuizMetadata q303 = new OXQuizMetadata()
            {
                Id = 303,
                QuestionText = "Chanson is an Italian song.",
                Answer = false,
                AnswerText = "Chanson is a French song.",
                Category = "Culture/Art"
            };
            questions.Add(q303);

            OXQuizMetadata q304 = new OXQuizMetadata()
            {
                Id = 304,
                QuestionText = "Chaos Onyx Crystals can be found from dismantled Gear items.",
                Answer = true,
                AnswerText = "Chaos Onyx Crystals can be found from dismantled Gear items.",
                Category = "MapleStory 2"
            };
            questions.Add(q304);

            OXQuizMetadata q305 = new OXQuizMetadata()
            {
                Id = 305,
                QuestionText = "Charizard is the highest evolved form of Bulbasaur.",
                Answer = false,
                AnswerText = "Charizard is the highest evolved form of Charmander.",
                Category = "Culture/Art"
            };
            questions.Add(q305);

            OXQuizMetadata q306 = new OXQuizMetadata()
            {
                Id = 306,
                QuestionText = "Charles Baudelaire wrote \"The Decameron.\"",
                Answer = false,
                AnswerText = "Giovanni Boccaccio, an Italian writer, wrote \"The Decameron.\"",
                Category = "Culture/Art"
            };
            questions.Add(q306);

            OXQuizMetadata q307 = new OXQuizMetadata()
            {
                Id = 307,
                QuestionText = "Charles Darwin is the first person to propose the theory of degeneration.",
                Answer = false,
                AnswerText = "Charles Darwin is an English biologist best known for his contributions to the science of evolution.",
                Category = "General Knowledge"
            };
            questions.Add(q307);

            OXQuizMetadata q308 = new OXQuizMetadata()
            {
                Id = 308,
                QuestionText = "Charles Darwin is the first person to propose the theory of evolution.",
                Answer = true,
                AnswerText = "Charles Darwin is an English biologist best known for his contributions to the science of evolution.",
                Category = "General Knowledge"
            };
            questions.Add(q308);

            OXQuizMetadata q309 = new OXQuizMetadata()
            {
                Id = 309,
                QuestionText = "Cherry Blossom Hill is accessible from the Boulderwhite Mountains.",
                Answer = false,
                AnswerText = "Cherry Blossom Hill is accessible from the Sylvan Woods trail.",
                Category = "MapleStory 2"
            };
            questions.Add(q309);

            OXQuizMetadata q310 = new OXQuizMetadata()
            {
                Id = 310,
                QuestionText = "Chickens can be left-footed or right-footed.",
                Answer = true,
                AnswerText = "Chickens can be left-footed or right-footed.",
                Category = "General Knowledge"
            };
            questions.Add(q310);

            OXQuizMetadata q311 = new OXQuizMetadata()
            {
                Id = 311,
                QuestionText = "Chicks lack belly buttons.",
                Answer = false,
                AnswerText = "Chicks have belly buttons.",
                Category = "General Knowledge"
            };
            questions.Add(q311);

            OXQuizMetadata q312 = new OXQuizMetadata()
            {
                Id = 312,
                QuestionText = "Chile is in South America.",
                Answer = true,
                AnswerText = "South America is home to many countries such as Chile, Brazil, and Argentina.",
                Category = "General Knowledge"
            };
            questions.Add(q312);

            OXQuizMetadata q313 = new OXQuizMetadata()
            {
                Id = 313,
                QuestionText = "China is the country that triggered the Japanese invasion of Korea (1592).",
                Answer = false,
                AnswerText = "It was Japan.",
                Category = "History"
            };
            questions.Add(q313);

            OXQuizMetadata q314 = new OXQuizMetadata()
            {
                Id = 314,
                QuestionText = "China uses yuan as its currency, and so does Hong Kong.",
                Answer = false,
                AnswerText = "Hong Kong uses Hong Kong dollars.",
                Category = "General Knowledge"
            };
            questions.Add(q314);

            OXQuizMetadata q315 = new OXQuizMetadata()
            {
                Id = 315,
                QuestionText = "Chinese characters are phonograms.",
                Answer = false,
                AnswerText = "Chinese characters are logograms.",
                Category = "General Knowledge"
            };
            questions.Add(q315);

            OXQuizMetadata q316 = new OXQuizMetadata()
            {
                Id = 316,
                QuestionText = "Chopin is one of the composers who used the word Ballade in the name of instrumental songs.",
                Answer = true,
                AnswerText = "Chopin is the first composers to use the word Ballade in the name of instrumental songs.",
                Category = "Culture/Art"
            };
            questions.Add(q316);

            OXQuizMetadata q317 = new OXQuizMetadata()
            {
                Id = 317,
                QuestionText = "Christmas is the day of Jesus's marriage.",
                Answer = false,
                AnswerText = "Christmas is the day that celebrates the birth of baby Jesus.",
                Category = "Culture/Art"
            };
            questions.Add(q317);

            OXQuizMetadata q318 = new OXQuizMetadata()
            {
                Id = 318,
                QuestionText = "Christopher Columbus coined the name \"Pacific Ocean.\"",
                Answer = false,
                AnswerText = "Ferdinand Magellan coined the name \"Oceano Pacifico,\" which means \"a quiet sea.\"",
                Category = "General Knowledge"
            };
            questions.Add(q318);

            OXQuizMetadata q319 = new OXQuizMetadata()
            {
                Id = 319,
                QuestionText = "Christopher Columbus discovered the continent of Africa.",
                Answer = false,
                AnswerText = "Christopher Columbus discovered the continent of America.",
                Category = "History"
            };
            questions.Add(q319);

            OXQuizMetadata q320 = new OXQuizMetadata()
            {
                Id = 320,
                QuestionText = "Christopher Nolan is the director of the 2008 film The Dark Knight.",
                Answer = true,
                AnswerText = "Christopher Nolan is the director of the 2008 film The Dark Knight.",
                Category = "Culture/Art"
            };
            questions.Add(q320);

            OXQuizMetadata q321 = new OXQuizMetadata()
            {
                Id = 321,
                QuestionText = "Cicada larva are butterfly larva.",
                Answer = false,
                AnswerText = "Cicada larva are cicada larva.",
                Category = "General Knowledge"
            };
            questions.Add(q321);

            OXQuizMetadata q322 = new OXQuizMetadata()
            {
                Id = 322,
                QuestionText = "Cicada larva are spider's larva.",
                Answer = false,
                AnswerText = "Cicada larva is a cicada's larva.",
                Category = "General Knowledge"
            };
            questions.Add(q322);

            OXQuizMetadata q323 = new OXQuizMetadata()
            {
                Id = 323,
                QuestionText = "Classicism refers to the following of ancient Greek or Roman styles in art and literature, generally associated with harmony and adherence to standard forms.",
                Answer = true,
                AnswerText = "Classicism refers to the following of ancient Greek or Roman styles in art and literature, and it sprang up in between the 17th and 18th centures across Europe.",
                Category = "Culture/Art"
            };
            questions.Add(q323);

            OXQuizMetadata q324 = new OXQuizMetadata()
            {
                Id = 324,
                QuestionText = "Claude Debussy composed the tone poem \"Finlandia.\"",
                Answer = false,
                AnswerText = "Finlandia is a song by Jean Sibelius.",
                Category = "Culture/Art"
            };
            questions.Add(q324);

            OXQuizMetadata q325 = new OXQuizMetadata()
            {
                Id = 325,
                QuestionText = "Claude Debussy is a Belgian composer.",
                Answer = false,
                AnswerText = "Claude Debussy is a French composer.",
                Category = "Culture/Art"
            };
            questions.Add(q325);

            OXQuizMetadata q326 = new OXQuizMetadata()
            {
                Id = 326,
                QuestionText = "Claude Debussy is a French composer.",
                Answer = true,
                AnswerText = "Claude Debussy is a composer from Paris, France.",
                Category = "Culture/Art"
            };
            questions.Add(q326);

            OXQuizMetadata q327 = new OXQuizMetadata()
            {
                Id = 327,
                QuestionText = "Claude Monet painted \"The Luncheon on the Grass.\"",
                Answer = false,
                AnswerText = "Édouard Manet painted \"The Luncheon on the Grass.\"",
                Category = "Culture/Art"
            };
            questions.Add(q327);

            OXQuizMetadata q328 = new OXQuizMetadata()
            {
                Id = 328,
                QuestionText = "CO2 is lighter than O2.",
                Answer = false,
                AnswerText = "CO2 is heavier than O2.",
                Category = "Science"
            };
            questions.Add(q328);

            OXQuizMetadata q329 = new OXQuizMetadata()
            {
                Id = 329,
                QuestionText = "Cockroaches and ants are known to cohabite peacefully in most cases.",
                Answer = false,
                AnswerText = "Ants and cockroaches are known for their animosity. Ants are even known to feast on cockroach eggs.",
                Category = "Science"
            };
            questions.Add(q329);

            OXQuizMetadata q330 = new OXQuizMetadata()
            {
                Id = 330,
                QuestionText = "Cockroaches can't survive in a place where ants thrive.",
                Answer = true,
                AnswerText = "It's because ants feast on cockroach eggs.",
                Category = "Science"
            };
            questions.Add(q330);

            OXQuizMetadata q331 = new OXQuizMetadata()
            {
                Id = 331,
                QuestionText = "Cold water freezes faster than warm water.",
                Answer = false,
                AnswerText = "Warm water freezes faster than cold water.",
                Category = "Science"
            };
            questions.Add(q331);

            OXQuizMetadata q332 = new OXQuizMetadata()
            {
                Id = 332,
                QuestionText = "Cold-blooded animals have a constant body temperature.",
                Answer = false,
                AnswerText = "Warm-blooded animals have constant body temperature.",
                Category = "Science"
            };
            questions.Add(q332);

            OXQuizMetadata q333 = new OXQuizMetadata()
            {
                Id = 333,
                QuestionText = "Commonly used in TV programs, ABS is shorthand for Automatic Response System.",
                Answer = false,
                AnswerText = "It's called Automatic Response System (ARS).",
                Category = "IT"
            };
            questions.Add(q333);

            OXQuizMetadata q334 = new OXQuizMetadata()
            {
                Id = 334,
                QuestionText = "Commonly used in TV programs, ARS is a shorthand for Automatic Response System.",
                Answer = true,
                AnswerText = "ARS is a shorthand for Automatic Response System.",
                Category = "IT"
            };
            questions.Add(q334);

            OXQuizMetadata q335 = new OXQuizMetadata()
            {
                Id = 335,
                QuestionText = "Composed by Giacomo Puccini, \"La Boheme\" is an opera whose plot depicts the romance between a U.S. Navy officer and a Japanese woman.",
                Answer = false,
                AnswerText = "\"La Boheme\" is an opera whose plot depicts the life of four cohabitants -- three artists and one philosopher -- in an attic.",
                Category = "Culture/Art"
            };
            questions.Add(q335);

            OXQuizMetadata q336 = new OXQuizMetadata()
            {
                Id = 336,
                QuestionText = "Computer input devices are limited to mouse and keyboard.",
                Answer = false,
                AnswerText = "Beside mice and keyboards there are many more input devices such as scanners and mics.",
                Category = "IT"
            };
            questions.Add(q336);

            OXQuizMetadata q337 = new OXQuizMetadata()
            {
                Id = 337,
                QuestionText = "Computers use binaries, 0 and 1, to express data.",
                Answer = true,
                AnswerText = "Computers use binaries, 0 and 1, to express data.",
                Category = "IT"
            };
            questions.Add(q337);

            OXQuizMetadata q338 = new OXQuizMetadata()
            {
                Id = 338,
                QuestionText = "Concave lenses converge light.",
                Answer = false,
                AnswerText = "Concave lenses diverge light, while convex lenses converge light.",
                Category = "Science"
            };
            questions.Add(q338);

            OXQuizMetadata q339 = new OXQuizMetadata()
            {
                Id = 339,
                QuestionText = "Contrabass is the largest string instrument.",
                Answer = true,
                AnswerText = "Contrabass is the largest string instrument.",
                Category = "Culture/Art"
            };
            questions.Add(q339);

            OXQuizMetadata q340 = new OXQuizMetadata()
            {
                Id = 340,
                QuestionText = "Copenhagen is the capital of Denmark.",
                Answer = true,
                AnswerText = "Copenhagen is the capital of Denmark.",
                Category = "Culture/Art"
            };
            questions.Add(q340);

            OXQuizMetadata q341 = new OXQuizMetadata()
            {
                Id = 341,
                QuestionText = "Cordelia the pampered feline used to live in the Tria Palace.",
                Answer = true,
                AnswerText = "Born in the Tria Palace, she's now looking for a butler from the outside of the palace.",
                Category = "MapleStory 2"
            };
            questions.Add(q341);

            OXQuizMetadata q342 = new OXQuizMetadata()
            {
                Id = 342,
                QuestionText = "Coronavirus is a TNA virus that causes infections in respiratory and digestive systems.",
                Answer = false,
                AnswerText = "Coronavirus is an RNA virus that causes infections in respiratory and digestive systems.",
                Category = "Science"
            };
            questions.Add(q342);

            OXQuizMetadata q343 = new OXQuizMetadata()
            {
                Id = 343,
                QuestionText = "Cotton and flax are examples of natural fibers.",
                Answer = true,
                AnswerText = "Both are linens made from natural fibers. Cotton is from the cotton plant and flax is a member of the genus Linum.",
                Category = "General Knowledge"
            };
            questions.Add(q343);

            OXQuizMetadata q344 = new OXQuizMetadata()
            {
                Id = 344,
                QuestionText = "Coubertin is regarded as the father of the Olympics.",
                Answer = true,
                AnswerText = "Coubertin of France is regarded as the father of the Olympics.",
                Category = "Culture/Art"
            };
            questions.Add(q344);

            OXQuizMetadata q345 = new OXQuizMetadata()
            {
                Id = 345,
                QuestionText = "Cows don't have teeth.",
                Answer = false,
                AnswerText = "Cows do have teeth.",
                Category = "General Knowledge"
            };
            questions.Add(q345);

            OXQuizMetadata q346 = new OXQuizMetadata()
            {
                Id = 346,
                QuestionText = "Cows have teeth.",
                Answer = true,
                AnswerText = "Cows do have teeth.",
                Category = "General Knowledge"
            };
            questions.Add(q346);

            OXQuizMetadata q347 = new OXQuizMetadata()
            {
                Id = 347,
                QuestionText = "CPUs are composed of three main units: a control unit, an arithmetic logic unit, and a memory management unit.",
                Answer = true,
                AnswerText = "CPUs are composed of three main units: a control unit, an arithmetic logic unit, and a memory management unit.",
                Category = "IT"
            };
            questions.Add(q347);

            OXQuizMetadata q348 = new OXQuizMetadata()
            {
                Id = 348,
                QuestionText = "Crabs have 6 legs.",
                Answer = false,
                AnswerText = "Crabs have 10 legs.",
                Category = "General Knowledge"
            };
            questions.Add(q348);

            OXQuizMetadata q349 = new OXQuizMetadata()
            {
                Id = 349,
                QuestionText = "Crabs have 8 legs.",
                Answer = false,
                AnswerText = "Crabs have 10 legs.",
                Category = "General Knowledge"
            };
            questions.Add(q349);

            OXQuizMetadata q350 = new OXQuizMetadata()
            {
                Id = 350,
                QuestionText = "Crabs have 9 legs.",
                Answer = false,
                AnswerText = "Crabs have 10 legs.",
                Category = "General Knowledge"
            };
            questions.Add(q350);

            OXQuizMetadata q351 = new OXQuizMetadata()
            {
                Id = 351,
                QuestionText = "Crickets make noises through their mouths.",
                Answer = false,
                AnswerText = "Crickets make noises by rubbing their wings.",
                Category = "Science"
            };
            questions.Add(q351);

            OXQuizMetadata q352 = new OXQuizMetadata()
            {
                Id = 352,
                QuestionText = "Croquis drawing is a quick sketch of a live model in motion.",
                Answer = true,
                AnswerText = "Croquis drawing is a quick sketch of a live model in motion. It's used to draw animals on the move or humans in balanced poses.",
                Category = "Culture/Art"
            };
            questions.Add(q352);

            OXQuizMetadata q353 = new OXQuizMetadata()
            {
                Id = 353,
                QuestionText = "Croquis is a technique of capturing the short video of a live model in motion.",
                Answer = false,
                AnswerText = "Croquis drawing is a quick sketch of a live model in motion. It's used to draw animals on the move or humans in balanced poses.",
                Category = "Culture/Art"
            };
            questions.Add(q353);

            OXQuizMetadata q354 = new OXQuizMetadata()
            {
                Id = 354,
                QuestionText = "Croquis is the artistic technique that exaggerates a remarkable trait of a person or a thing to create a comic or satiric effect.",
                Answer = false,
                AnswerText = "Caricature is the artistic technique that exaggerates a remarkable trait of a person or a thing to create a comic or satiric effect.",
                Category = "Culture/Art"
            };
            questions.Add(q354);

            OXQuizMetadata q355 = new OXQuizMetadata()
            {
                Id = 355,
                QuestionText = "Crystal Fragments can only be purchased from merchants.",
                Answer = false,
                AnswerText = "They can also be found from treasure chests and rewarded from quests.",
                Category = "MapleStory 2"
            };
            questions.Add(q355);

            OXQuizMetadata q356 = new OXQuizMetadata()
            {
                Id = 356,
                QuestionText = "CTRL+C is the default shortcut to copy text.",
                Answer = true,
                AnswerText = "CTRL+C is the default shortcut to copy text.",
                Category = "IT"
            };
            questions.Add(q356);

            OXQuizMetadata q357 = new OXQuizMetadata()
            {
                Id = 357,
                QuestionText = "Ctrl+G is the default hotkey to copy text.",
                Answer = false,
                AnswerText = "Ctrl+C is the default hotkey to copy text.",
                Category = "IT"
            };
            questions.Add(q357);

            OXQuizMetadata q358 = new OXQuizMetadata()
            {
                Id = 358,
                QuestionText = "Currently, 12 TB is the world's largest SSD, a storage device that uses semiconductors.",
                Answer = false,
                AnswerText = "On August 2015, Samsung Electronics unveiled their 16TB SSD.",
                Category = "IT"
            };
            questions.Add(q358);

            OXQuizMetadata q359 = new OXQuizMetadata()
            {
                Id = 359,
                QuestionText = "Dark Wind HQ is located in Henesys.",
                Answer = false,
                AnswerText = "Dark Wind HQ is located in Kerning City.",
                Category = "MapleStory 2"
            };
            questions.Add(q359);

            OXQuizMetadata q360 = new OXQuizMetadata()
            {
                Id = 360,
                QuestionText = "Darth Vader was Luke Skywalker's father.",
                Answer = true,
                AnswerText = "In Star Wars V: Empire Strikes Back, it is revealed that Darth Vader is Luke's father.",
                Category = "Culture/Art"
            };
            questions.Add(q360);

            OXQuizMetadata q361 = new OXQuizMetadata()
            {
                Id = 361,
                QuestionText = "David Livingston set out to Africa to provide medical services.",
                Answer = false,
                AnswerText = "David Livingston set out to Africa to do missionary work.",
                Category = "General Knowledge"
            };
            questions.Add(q361);

            OXQuizMetadata q362 = new OXQuizMetadata()
            {
                Id = 362,
                QuestionText = "Debugging in computer science terms refers to the fixing of the errors created during the translation of source code into object code.",
                Answer = true,
                AnswerText = "Debugging refers to the process of identifying and removing errors from either computer hardware or software.",
                Category = "IT"
            };
            questions.Add(q362);

            OXQuizMetadata q363 = new OXQuizMetadata()
            {
                Id = 363,
                QuestionText = "Decibel is a unit to measure the intensity of pollution.",
                Answer = false,
                AnswerText = "Decibel is a unit to measure the intensity of noise.",
                Category = "Science"
            };
            questions.Add(q363);

            OXQuizMetadata q364 = new OXQuizMetadata()
            {
                Id = 364,
                QuestionText = "Deer and humans possess the same number of neck bones.",
                Answer = true,
                AnswerText = "All mammals have the same number of neck bones.",
                Category = "General Knowledge"
            };
            questions.Add(q364);

            OXQuizMetadata q365 = new OXQuizMetadata()
            {
                Id = 365,
                QuestionText = "Devilin Warriors appear on the North Royal Road.",
                Answer = false,
                AnswerText = "Devilin Warriors appear on the South Royal Road.",
                Category = "MapleStory 2"
            };
            questions.Add(q365);

            OXQuizMetadata q366 = new OXQuizMetadata()
            {
                Id = 366,
                QuestionText = "Devilin Warriors appear on the South Royal Road.",
                Answer = true,
                AnswerText = "Devilin Warriors appear on the South Royal Road.",
                Category = "MapleStory 2"
            };
            questions.Add(q366);

            OXQuizMetadata q367 = new OXQuizMetadata()
            {
                Id = 367,
                QuestionText = "Diastase transforms starch into maltose, then into glucose.",
                Answer = true,
                AnswerText = "Diastase transforms starch into maltose, then into glucose.",
                Category = "Science"
            };
            questions.Add(q367);

            OXQuizMetadata q368 = new OXQuizMetadata()
            {
                Id = 368,
                QuestionText = "Dietary fiber is the primary nutrition meat, eggs, fish, and beans provide.",
                Answer = false,
                AnswerText = "They provide protein.",
                Category = "General Knowledge"
            };
            questions.Add(q368);

            OXQuizMetadata q369 = new OXQuizMetadata()
            {
                Id = 369,
                QuestionText = "Dogs have 6 legs.",
                Answer = false,
                AnswerText = "Dogs have 4 legs.",
                Category = "General Knowledge"
            };
            questions.Add(q369);

            OXQuizMetadata q370 = new OXQuizMetadata()
            {
                Id = 370,
                QuestionText = "Doric is the oldest of the three classical architectural orders of Greece: Doric, Ionic, and Corinthian.",
                Answer = true,
                AnswerText = "Doric is the oldest of the three classical architectural orders of Greece: Doric, Ionic, and Corinthian.",
                Category = "Culture/Art"
            };
            questions.Add(q370);

            OXQuizMetadata q371 = new OXQuizMetadata()
            {
                Id = 371,
                QuestionText = "Doughnut effect refers to the phenomenon where a metropolitan city's residents, due to pollution and rise in real estate price, relocate to the city's peripheries.",
                Answer = true,
                AnswerText = "Doughnut effect refers to the phenomenon where a metropolitan city's population becomes hollowed out at the center. Several factors pressure the residents to relocate to the city's peripheries, while only the",
                Category = "Society"
            };
            questions.Add(q371);

            OXQuizMetadata q372 = new OXQuizMetadata()
            {
                Id = 372,
                QuestionText = "Drag, in computing terms, means a saved sequence of commands that can be stored and then recalled with a single command.",
                Answer = false,
                AnswerText = "Macro, in computing terms, means a saved sequence of commands that can be stored and then recalled with a single command.",
                Category = "IT"
            };
            questions.Add(q372);

            OXQuizMetadata q373 = new OXQuizMetadata()
            {
                Id = 373,
                QuestionText = "Dragonflies have many eyes.",
                Answer = true,
                AnswerText = "They have two large compound eyes on the sides of their heads and three simple lens eyes in the center.",
                Category = "General Knowledge"
            };
            questions.Add(q373);

            OXQuizMetadata q374 = new OXQuizMetadata()
            {
                Id = 374,
                QuestionText = "Dragonflies have more than two eyes.",
                Answer = true,
                AnswerText = "They have two large compound eyes on the sides of their heads and three simple lens eyes in the center.",
                Category = "General Knowledge"
            };
            questions.Add(q374);

            OXQuizMetadata q375 = new OXQuizMetadata()
            {
                Id = 375,
                QuestionText = "Dragonflies have one eye.",
                Answer = false,
                AnswerText = "They have two large compound eyes on the sides of their heads and three simple lens eyes in the center.",
                Category = "General Knowledge"
            };
            questions.Add(q375);

            OXQuizMetadata q376 = new OXQuizMetadata()
            {
                Id = 376,
                QuestionText = "Dragonflies have over twenty thousand eyes.",
                Answer = true,
                AnswerText = "They do have at least twenty thousand eyes.",
                Category = "General Knowledge"
            };
            questions.Add(q376);

            OXQuizMetadata q377 = new OXQuizMetadata()
            {
                Id = 377,
                QuestionText = "Dragonflies have over two hundred million eyes.",
                Answer = false,
                AnswerText = "They do have about twenty thousand eyes.",
                Category = "General Knowledge"
            };
            questions.Add(q377);

            OXQuizMetadata q378 = new OXQuizMetadata()
            {
                Id = 378,
                QuestionText = "Dragonflies lay their eggs under the ground.",
                Answer = false,
                AnswerText = "Dragonflies lay their eggs under the water.",
                Category = "Science"
            };
            questions.Add(q378);

            OXQuizMetadata q379 = new OXQuizMetadata()
            {
                Id = 379,
                QuestionText = "Drama refers to films or shows that are meant to document phenomena.",
                Answer = false,
                AnswerText = "Such films and shows are documentaries, not dramas.",
                Category = "General Knowledge"
            };
            questions.Add(q379);

            OXQuizMetadata q380 = new OXQuizMetadata()
            {
                Id = 380,
                QuestionText = "Dry ice is colder than regular ice.",
                Answer = true,
                AnswerText = "Dry ice is at -78 °C, much colder than regular ice.",
                Category = "Science"
            };
            questions.Add(q380);

            OXQuizMetadata q381 = new OXQuizMetadata()
            {
                Id = 381,
                QuestionText = "Dry ice is hotter than regular ice.",
                Answer = false,
                AnswerText = "Dry ice, which is at -78 °C, is much colder than regular ice.",
                Category = "Science"
            };
            questions.Add(q381);

            OXQuizMetadata q382 = new OXQuizMetadata()
            {
                Id = 382,
                QuestionText = "Dry ice is solid carbon monoxide.",
                Answer = false,
                AnswerText = "Dry ice is solid carbon dioxide.",
                Category = "Science"
            };
            questions.Add(q382);

            OXQuizMetadata q383 = new OXQuizMetadata()
            {
                Id = 383,
                QuestionText = "DuBose Heyward wrote the novel \"Uncle Tom\"s Cabin.\"",
                Answer = false,
                AnswerText = "Harriet Beecher Stowe wrote the novel \"Uncle Tom\"s Cabin.\"",
                Category = "Culture/Art"
            };
            questions.Add(q383);

            OXQuizMetadata q384 = new OXQuizMetadata()
            {
                Id = 384,
                QuestionText = "During the 13-day Copenhagen diet, boiled eggs are eaten with salt.",
                Answer = false,
                AnswerText = "Boiled eggs are eaten plain.",
                Category = "General Knowledge"
            };
            questions.Add(q384);

            OXQuizMetadata q385 = new OXQuizMetadata()
            {
                Id = 385,
                QuestionText = "During the day, trees take in CO and provide O2.",
                Answer = false,
                AnswerText = "During the day, trees take in CO2 and provide O2.",
                Category = "Science"
            };
            questions.Add(q385);

            OXQuizMetadata q386 = new OXQuizMetadata()
            {
                Id = 386,
                QuestionText = "Eating dark chocolate is healthy for dogs.",
                Answer = false,
                AnswerText = "A substance called theobromine in chocolate can kill a dog.",
                Category = "General Knowledge"
            };
            questions.Add(q386);

            OXQuizMetadata q387 = new OXQuizMetadata()
            {
                Id = 387,
                QuestionText = "Edouard Manet is known as the mother of Impressionism.",
                Answer = false,
                AnswerText = "Édouard Manet is known as the father of Impressionism.",
                Category = "Culture/Art"
            };
            questions.Add(q387);

            OXQuizMetadata q388 = new OXQuizMetadata()
            {
                Id = 388,
                QuestionText = "Edvard Grieg is a Finnish musician.",
                Answer = false,
                AnswerText = "Edvard Grieg is a Norwegian musician.",
                Category = "Culture/Art"
            };
            questions.Add(q388);

            OXQuizMetadata q389 = new OXQuizMetadata()
            {
                Id = 389,
                QuestionText = "Edvard Grieg is a Norwegian musician.",
                Answer = true,
                AnswerText = "Edvard Grieg is a Norwegian musician.",
                Category = "Culture/Art"
            };
            questions.Add(q389);

            OXQuizMetadata q390 = new OXQuizMetadata()
            {
                Id = 390,
                QuestionText = "Electric Bass used in rock bands has only 5 strings.",
                Answer = false,
                AnswerText = "There are 4-stringed, 5-stringed, and 6-stringed Electric Bass.",
                Category = "Culture/Art"
            };
            questions.Add(q390);

            OXQuizMetadata q391 = new OXQuizMetadata()
            {
                Id = 391,
                QuestionText = "Elephants and humans possess the same number of neck bones.",
                Answer = true,
                AnswerText = "All mammals have the same number of neck bones.",
                Category = "General Knowledge"
            };
            questions.Add(q391);

            OXQuizMetadata q392 = new OXQuizMetadata()
            {
                Id = 392,
                QuestionText = "Elizabeth I of England was a married woman.",
                Answer = false,
                AnswerText = "Elizabeth I of England was single. She claimed that she was married to her country and stayed devoted her country.",
                Category = "History"
            };
            questions.Add(q392);

            OXQuizMetadata q393 = new OXQuizMetadata()
            {
                Id = 393,
                QuestionText = "Ellinia and Decor Haven play the same BGM.",
                Answer = true,
                AnswerText = "Decor Haven and Ellinia play the same BGM.",
                Category = "MapleStory 2"
            };
            questions.Add(q393);

            OXQuizMetadata q394 = new OXQuizMetadata()
            {
                Id = 394,
                QuestionText = "Emile Boirac was one of the first to use the term \"deja vu.\"",
                Answer = true,
                AnswerText = "Émile Boirac was one of the first to use the term \"déjà vu.\"",
                Category = "General Knowledge"
            };
            questions.Add(q394);

            OXQuizMetadata q395 = new OXQuizMetadata()
            {
                Id = 395,
                QuestionText = "Emotion changes tear concentration.",
                Answer = true,
                AnswerText = "When one is emotionally aroused, the sympathetic nervous system excretes tears that are high in sodium chloride.",
                Category = "Science"
            };
            questions.Add(q395);

            OXQuizMetadata q396 = new OXQuizMetadata()
            {
                Id = 396,
                QuestionText = "Emotion changes the temperature of tears.",
                Answer = false,
                AnswerText = "One's emotion has nothing to do with the temperature of one's tears.",
                Category = "General Knowledge"
            };
            questions.Add(q396);

            OXQuizMetadata q397 = new OXQuizMetadata()
            {
                Id = 397,
                QuestionText = "Emperor Napoleon I ordered the construction of the Triumphal Arch.",
                Answer = true,
                AnswerText = "It was built from 1806 to 1836 to celebrate the victories of France.",
                Category = "General Knowledge"
            };
            questions.Add(q397);

            OXQuizMetadata q398 = new OXQuizMetadata()
            {
                Id = 398,
                QuestionText = "England has never won a World Cup.",
                Answer = false,
                AnswerText = "England won the 1966 World Cup England.",
                Category = "History"
            };
            questions.Add(q398);

            OXQuizMetadata q399 = new OXQuizMetadata()
            {
                Id = 399,
                QuestionText = "Engle's coefficient measures the income distribution of a nation's residents.",
                Answer = false,
                AnswerText = "Gini coefficient measures the income distribution of a nation's residents.",
                Category = "General Knowledge"
            };
            questions.Add(q399);

            OXQuizMetadata q400 = new OXQuizMetadata()
            {
                Id = 400,
                QuestionText = "Equal suffrage means every citizen gets one vote.",
                Answer = true,
                AnswerText = "Equal suffrage means that every citizen, regardless of one's social standings, gets one vote.",
                Category = "Society"
            };
            questions.Add(q400);

            OXQuizMetadata q401 = new OXQuizMetadata()
            {
                Id = 401,
                QuestionText = "Equal suffrage means that every citizen, regardless of social standings, gets one vote.",
                Answer = true,
                AnswerText = "Equal suffrage means that every citizen, regardless of social standings, gets one vote.",
                Category = "Society"
            };
            questions.Add(q401);

            OXQuizMetadata q402 = new OXQuizMetadata()
            {
                Id = 402,
                QuestionText = "Equipment Merchant Surmony wears an eye patch over his left eye.",
                Answer = false,
                AnswerText = "Surmony does not wear an eye patch.",
                Category = "MapleStory 2"
            };
            questions.Add(q402);

            OXQuizMetadata q403 = new OXQuizMetadata()
            {
                Id = 403,
                QuestionText = "Eros is the nephew of Aphrodite.",
                Answer = false,
                AnswerText = "Eros is the son of Aphrodite.",
                Category = "General Knowledge"
            };
            questions.Add(q403);

            OXQuizMetadata q404 = new OXQuizMetadata()
            {
                Id = 404,
                QuestionText = "Eros is the son of Aphrodite.",
                Answer = true,
                AnswerText = "Eros is the son of Aphrodite.",
                Category = "General Knowledge"
            };
            questions.Add(q404);

            OXQuizMetadata q405 = new OXQuizMetadata()
            {
                Id = 405,
                QuestionText = "Erwin Schrodinger first proposed the concept of the Higgs particle.",
                Answer = false,
                AnswerText = "Peter Higgs proposed the date.",
                Category = "History"
            };
            questions.Add(q405);

            OXQuizMetadata q406 = new OXQuizMetadata()
            {
                Id = 406,
                QuestionText = "Eugene O'Neill wrote the play \"Death of a Salesman.\"",
                Answer = false,
                AnswerText = "Arthur Miller wrote the play \"Death of a Salesman.\"",
                Category = "Culture/Art"
            };
            questions.Add(q406);

            OXQuizMetadata q407 = new OXQuizMetadata()
            {
                Id = 407,
                QuestionText = "Evansville and Queenstown play the same BGM.",
                Answer = true,
                AnswerText = "Evansville and Queenstown, both located near Tria, play the same BGM.",
                Category = "MapleStory 2"
            };
            questions.Add(q407);

            OXQuizMetadata q408 = new OXQuizMetadata()
            {
                Id = 408,
                QuestionText = "Eve and Katvan are in love.",
                Answer = false,
                AnswerText = "Eve and Lennon are in love.",
                Category = "MapleStory 2"
            };
            questions.Add(q408);

            OXQuizMetadata q409 = new OXQuizMetadata()
            {
                Id = 409,
                QuestionText = "Every monster in the Testing Grounds is an illusion created by Shaman Onrung.",
                Answer = true,
                AnswerText = "They are illusions created by Shaman Onrung for training purposes. This is how the warriors of Perion trained in the past.",
                Category = "MapleStory 2"
            };
            questions.Add(q409);

            OXQuizMetadata q410 = new OXQuizMetadata()
            {
                Id = 410,
                QuestionText = "Everything in the novel \"A Christmas Carol\" takes place in the span of three days.",
                Answer = false,
                AnswerText = "The entire plot of \"A Christmas Carol\" takes place in the span of two days: the eve and the day of Christmas.",
                Category = "Culture/Art"
            };
            questions.Add(q410);

            OXQuizMetadata q411 = new OXQuizMetadata()
            {
                Id = 411,
                QuestionText = "Everything in the novel \"A Christmas Carol\" takes place in the span of two days.",
                Answer = true,
                AnswerText = "The entire plot of \"A Christmas Carol\" takes place in the span of two days: the eve and the day of Christmas.",
                Category = "Culture/Art"
            };
            questions.Add(q411);

            OXQuizMetadata q412 = new OXQuizMetadata()
            {
                Id = 412,
                QuestionText = "Exceptional or better items generate Chaos Onyx Crystals when dismantled.",
                Answer = false,
                AnswerText = "Epic or better items generate Chaos Onyx Crystals when dismantled.",
                Category = "MapleStory 2"
            };
            questions.Add(q412);

            OXQuizMetadata q413 = new OXQuizMetadata()
            {
                Id = 413,
                QuestionText = "F-1, a type of auto racing, is shorthand for Force-1.",
                Answer = false,
                AnswerText = "F-1 stands for Formula-1.",
                Category = "General Knowledge"
            };
            questions.Add(q413);

            OXQuizMetadata q414 = new OXQuizMetadata()
            {
                Id = 414,
                QuestionText = "Facing one's head upward is an effective way to stop one's nose from bleeding.",
                Answer = false,
                AnswerText = "It doesn't help with the bleeding.",
                Category = "General Knowledge"
            };
            questions.Add(q414);

            OXQuizMetadata q415 = new OXQuizMetadata()
            {
                Id = 415,
                QuestionText = "Fahrenheit defines 0 as the freezing point of water at 1 atm pressure and 100 as the boiling point of water at 1 atm pressure. The two points are divided into 100 degrees to serve as a scale.",
                Answer = false,
                AnswerText = "Celsius is the temperature scale where 0 represented the freezing point of water and 100 represented the boiling point of water, and the two points are divided into 100 degrees as a scale.",
                Category = "Science"
            };
            questions.Add(q415);

            OXQuizMetadata q416 = new OXQuizMetadata()
            {
                Id = 416,
                QuestionText = "Fahrenheit defines 32 as the freezing point of water at 1 atm pressure and 212 as the boiling point of water at 1 atm pressure. The two points are divided into 180 degrees to serve as a scale.",
                Answer = true,
                AnswerText = "German scientist Daniel Gabrielle Fahrenheit created a temperature scale where 32 represented the freezing point of water, and 212 represented the boiling point of water, and the two points are divided into 180 degrees.",
                Category = "Science"
            };
            questions.Add(q416);

            OXQuizMetadata q417 = new OXQuizMetadata()
            {
                Id = 417,
                QuestionText = "Falling water is a signature work of architect Frank Lloyd Wright.",
                Answer = true,
                AnswerText = "Falling water is a signature work of architect Frank Lloyd Wright, who is a popular 20th-century American architect.",
                Category = "General Knowledge"
            };
            questions.Add(q417);

            OXQuizMetadata q418 = new OXQuizMetadata()
            {
                Id = 418,
                QuestionText = "Falling water is a signature work of architect Le Corbusier.",
                Answer = false,
                AnswerText = "Falling Water is a signature work of Frank Lloyd Wright, who is a popular 20th-century American architect.",
                Category = "General Knowledge"
            };
            questions.Add(q418);

            OXQuizMetadata q419 = new OXQuizMetadata()
            {
                Id = 419,
                QuestionText = "Famous composers Wolfgang Mozart and Beethoven are relatives.",
                Answer = false,
                AnswerText = "Famous composers Wolfgang Mozart and Beethoven are not relatives.",
                Category = "Culture/Art"
            };
            questions.Add(q419);

            OXQuizMetadata q420 = new OXQuizMetadata()
            {
                Id = 420,
                QuestionText = "Famous for \"Four Seasons,\" Antonio Vivaldi is an Austrian.",
                Answer = false,
                AnswerText = "Antonio Vivaldi is an Italian.",
                Category = "Culture/Art"
            };
            questions.Add(q420);

            OXQuizMetadata q421 = new OXQuizMetadata()
            {
                Id = 421,
                QuestionText = "Famous for \"Four Seasons,\" Antonio Vivaldi was Italian.",
                Answer = true,
                AnswerText = "Famous for \"Four Seasons,\" Antonio Vivaldi was Italian.",
                Category = "Culture/Art"
            };
            questions.Add(q421);

            OXQuizMetadata q422 = new OXQuizMetadata()
            {
                Id = 422,
                QuestionText = "Fat-free milk has absolutely no fat.",
                Answer = false,
                AnswerText = "A jar of milk has less than 0.6% fat can be labelled as fat-free milk. In other words, fat-free milk often does have a bit of fat in it.",
                Category = "General Knowledge"
            };
            questions.Add(q422);

            OXQuizMetadata q423 = new OXQuizMetadata()
            {
                Id = 423,
                QuestionText = "Female mantises, after mating, eat their male partners.",
                Answer = true,
                AnswerText = "Female mantises prey of their partners to gain the energy required for the hatching process.",
                Category = "General Knowledge"
            };
            questions.Add(q423);

            OXQuizMetadata q424 = new OXQuizMetadata()
            {
                Id = 424,
                QuestionText = "Ferdinand Magellan coined the name \"Pacific Ocean.\"",
                Answer = true,
                AnswerText = "Ferdinand Magellan coined the name \"Oceano Pacifico,\" which means \"a quiet sea.\"",
                Category = "Culture/Art"
            };
            questions.Add(q424);

            OXQuizMetadata q425 = new OXQuizMetadata()
            {
                Id = 425,
                QuestionText = "Fevernova was the official ball for the 2002 World Cup in Korea-Japan.",
                Answer = true,
                AnswerText = "Fevernova was the official ball for the 2002 World Cup in Korea-Japan.",
                Category = "Culture/Art"
            };
            questions.Add(q425);

            OXQuizMetadata q426 = new OXQuizMetadata()
            {
                Id = 426,
                QuestionText = "Fidelio is the only opera Beethoven ever composed.",
                Answer = true,
                AnswerText = "Beethoven composed only one opera: Fidelio. It is divded into two acts.",
                Category = "Culture/Art"
            };
            questions.Add(q426);

            OXQuizMetadata q427 = new OXQuizMetadata()
            {
                Id = 427,
                QuestionText = "Figurative art is a subgenre of abstract art.",
                Answer = false,
                AnswerText = "Figurative art is the opposite of abstract art.",
                Category = "Culture/Art"
            };
            questions.Add(q427);

            OXQuizMetadata q428 = new OXQuizMetadata()
            {
                Id = 428,
                QuestionText = "Fish and chips is a classic American working class dish.",
                Answer = false,
                AnswerText = "Fish and chips, a dish of fried fish and potatoes, is a signature English dish.",
                Category = "General Knowledge"
            };
            questions.Add(q428);

            OXQuizMetadata q429 = new OXQuizMetadata()
            {
                Id = 429,
                QuestionText = "Fish breathe through their mouths.",
                Answer = false,
                AnswerText = "Fish breathe through their gills.",
                Category = "General Knowledge"
            };
            questions.Add(q429);

            OXQuizMetadata q430 = new OXQuizMetadata()
            {
                Id = 430,
                QuestionText = "Fish sleep with their eyes closed.",
                Answer = false,
                AnswerText = "Fish sleep with their eyes open because they don't have eyelids.",
                Category = "General Knowledge"
            };
            questions.Add(q430);

            OXQuizMetadata q431 = new OXQuizMetadata()
            {
                Id = 431,
                QuestionText = "Fleas have kidneys.",
                Answer = false,
                AnswerText = "Fleas lack kidneys.",
                Category = "General Knowledge"
            };
            questions.Add(q431);

            OXQuizMetadata q432 = new OXQuizMetadata()
            {
                Id = 432,
                QuestionText = "Fleas lack kidneys.",
                Answer = true,
                AnswerText = "Fleas lack kidneys.",
                Category = "General Knowledge"
            };
            questions.Add(q432);

            OXQuizMetadata q433 = new OXQuizMetadata()
            {
                Id = 433,
                QuestionText = "Footnote, in a word processor, is a function that adds additional notes at the bottom of a document.",
                Answer = true,
                AnswerText = "Footnote is a function that adds additional notes at the bottom of a document.",
                Category = "IT"
            };
            questions.Add(q433);

            OXQuizMetadata q434 = new OXQuizMetadata()
            {
                Id = 434,
                QuestionText = "Forsythia is a genus of flower that blooms in autumn.",
                Answer = false,
                AnswerText = "It is a flower that blooms in the spring.",
                Category = "General Knowledge"
            };
            questions.Add(q434);

            OXQuizMetadata q435 = new OXQuizMetadata()
            {
                Id = 435,
                QuestionText = "Forsythia is a genus of flower that blooms in spring.",
                Answer = true,
                AnswerText = "It is a flower that blooms in spring.",
                Category = "General Knowledge"
            };
            questions.Add(q435);

            OXQuizMetadata q436 = new OXQuizMetadata()
            {
                Id = 436,
                QuestionText = "Four CPUs are called a quad core.",
                Answer = false,
                AnswerText = "Quad core refers not to 4 CPUs but 4 cores.",
                Category = "IT"
            };
            questions.Add(q436);

            OXQuizMetadata q437 = new OXQuizMetadata()
            {
                Id = 437,
                QuestionText = "Francisco Goya was a Spanish court painter.",
                Answer = true,
                AnswerText = "Francisco Goya was a Spanish court painter.",
                Category = "Culture/Art"
            };
            questions.Add(q437);

            OXQuizMetadata q438 = new OXQuizMetadata()
            {
                Id = 438,
                QuestionText = "Fraternal twins have the same blood type.",
                Answer = false,
                AnswerText = "Fraternal twins can have different blood types.",
                Category = "General Knowledge"
            };
            questions.Add(q438);

            OXQuizMetadata q439 = new OXQuizMetadata()
            {
                Id = 439,
                QuestionText = "Frederic Chopin and Franz Liszt are renowned piano virtuosos and composers of their time.",
                Answer = true,
                AnswerText = "Frederic Chopin and Franz Liszt are renowned piano virtuosos and composers of their time.",
                Category = "Culture/Art"
            };
            questions.Add(q439);

            OXQuizMetadata q440 = new OXQuizMetadata()
            {
                Id = 440,
                QuestionText = "Frederic Chopin and Franz Liszt are renowned violin virtuosos and composers of their time.",
                Answer = false,
                AnswerText = "Frederic Chopin and Franz Liszt are renowned piano virtuosos and composers of their time.",
                Category = "Culture/Art"
            };
            questions.Add(q440);

            OXQuizMetadata q441 = new OXQuizMetadata()
            {
                Id = 441,
                QuestionText = "Frederic Chopin had participated in the International Chopin Piano Competition, a competition that is held in Warsaw, Poland.",
                Answer = false,
                AnswerText = "Frédéric Chopin died in 1849, and the first international Chopin Competition began in 1927.",
                Category = "Culture/Art"
            };
            questions.Add(q441);

            OXQuizMetadata q442 = new OXQuizMetadata()
            {
                Id = 442,
                QuestionText = "Frogs have navels (belly buttons).",
                Answer = false,
                AnswerText = "Because frogs are hatched from eggs, they lack navels.",
                Category = "General Knowledge"
            };
            questions.Add(q442);

            OXQuizMetadata q443 = new OXQuizMetadata()
            {
                Id = 443,
                QuestionText = "Gaboshi heels are shoes with a wedge-shaped sole.",
                Answer = false,
                AnswerText = "In vogue since the 1940s, wedge heels are shoes with a sole in the form of a wedge.",
                Category = "General Knowledge"
            };
            questions.Add(q443);

            OXQuizMetadata q444 = new OXQuizMetadata()
            {
                Id = 444,
                QuestionText = "Gaining weight fattens one's tongue.",
                Answer = true,
                AnswerText = "A tongue does get fat.",
                Category = "General Knowledge"
            };
            questions.Add(q444);

            OXQuizMetadata q445 = new OXQuizMetadata()
            {
                Id = 445,
                QuestionText = "Gastric juice is high alkaline.",
                Answer = false,
                AnswerText = "Gastric juice is highly acidic and prevents the food in the stomach from decaying.",
                Category = "Science"
            };
            questions.Add(q445);

            OXQuizMetadata q446 = new OXQuizMetadata()
            {
                Id = 446,
                QuestionText = "Gastric juice is highly acidic.",
                Answer = true,
                AnswerText = "Gastric juice is highly acidic and prevents the food in the stomach from decaying.",
                Category = "Science"
            };
            questions.Add(q446);

            OXQuizMetadata q447 = new OXQuizMetadata()
            {
                Id = 447,
                QuestionText = "GATT trade round focuses on creating multi-national trade rules that aim to preserve the environment.",
                Answer = true,
                AnswerText = "GATT trade round focuses on preserving the environment. It tries to create multi-national trade rules that impose high tariffs on environmentally detrimental goods.",
                Category = "Society"
            };
            questions.Add(q447);

            OXQuizMetadata q448 = new OXQuizMetadata()
            {
                Id = 448,
                QuestionText = "GDP is the shorthand for \"gross national product.\"",
                Answer = false,
                AnswerText = "GNP is the shorthand for \"gross national product.\"",
                Category = "General Knowledge"
            };
            questions.Add(q448);

            OXQuizMetadata q449 = new OXQuizMetadata()
            {
                Id = 449,
                QuestionText = "Gear at Level 20 or above can be dismantled.",
                Answer = true,
                AnswerText = "Gear at Level 20 or above can be dismantled to gain Onyx Crystals.",
                Category = "MapleStory 2"
            };
            questions.Add(q449);

            OXQuizMetadata q450 = new OXQuizMetadata()
            {
                Id = 450,
                QuestionText = "George Gershwin is an American composer known for \"Rhapsody in Blue,\" a song for solo piano and jazz bands.",
                Answer = true,
                AnswerText = "He is also known for \"Porgy and Bess.\"",
                Category = "Culture/Art"
            };
            questions.Add(q450);

            OXQuizMetadata q451 = new OXQuizMetadata()
            {
                Id = 451,
                QuestionText = "George Washington served as the first President of the United States.",
                Answer = true,
                AnswerText = "On April 30, 1789, George Washington was inaugurated as the first President of the United States.",
                Category = "General Knowledge"
            };
            questions.Add(q451);

            OXQuizMetadata q452 = new OXQuizMetadata()
            {
                Id = 452,
                QuestionText = "Georges Bizet, who composed \"L'Arlesienne,\" is a Norwegian composer.",
                Answer = false,
                AnswerText = "Georges Bizet is a French composer.",
                Category = "Culture/Art"
            };
            questions.Add(q452);

            OXQuizMetadata q453 = new OXQuizMetadata()
            {
                Id = 453,
                QuestionText = "Germany won the 2006 World Cup in Germany.",
                Answer = false,
                AnswerText = "Italy won the 2006 World Cup Germany.",
                Category = "Culture/Art"
            };
            questions.Add(q453);

            OXQuizMetadata q454 = new OXQuizMetadata()
            {
                Id = 454,
                QuestionText = "Giant Turtle is a Spirit monster.",
                Answer = false,
                AnswerText = "Giant Turtle is a Divine monster.",
                Category = "MapleStory 2"
            };
            questions.Add(q454);

            OXQuizMetadata q455 = new OXQuizMetadata()
            {
                Id = 455,
                QuestionText = "Gioachino Rossini is a German composer.",
                Answer = false,
                AnswerText = "Gioachino Rossini is an Italian composer.",
                Category = "Culture/Art"
            };
            questions.Add(q455);

            OXQuizMetadata q456 = new OXQuizMetadata()
            {
                Id = 456,
                QuestionText = "Giraffes and humans possess the same number of neck bones.",
                Answer = true,
                AnswerText = "All mammals have the same number of neck bones.",
                Category = "General Knowledge"
            };
            questions.Add(q456);

            OXQuizMetadata q457 = new OXQuizMetadata()
            {
                Id = 457,
                QuestionText = "Giuseppe Verdi was the first composer to use a metronome.",
                Answer = false,
                AnswerText = "Beethoven was the first notable composer to use a metronome.",
                Category = "Culture/Art"
            };
            questions.Add(q457);

            OXQuizMetadata q458 = new OXQuizMetadata()
            {
                Id = 458,
                QuestionText = "Given that external pressure is 1 atm, a substance with a boiling point above room temperature (25 °C) and melting point below room temperature must be a liquid.",
                Answer = true,
                AnswerText = "Given that external pressure is 1 atm, a substance such as water whose boiling point is at 100 °C and melting point is at 0 °C would be in the liquid state.",
                Category = "Science"
            };
            questions.Add(q458);

            OXQuizMetadata q459 = new OXQuizMetadata()
            {
                Id = 459,
                QuestionText = "Given that external pressure is 1 atm, a substance with a boiling point and melting point both above room temperature (25 °C) must be a gas.",
                Answer = false,
                AnswerText = "Given that external pressure is 1 atm, a substance such as naphthalene whose boiling point is at 218 °C and melting point is at 81 °C would be in the solid state.",
                Category = "Science"
            };
            questions.Add(q459);

            OXQuizMetadata q460 = new OXQuizMetadata()
            {
                Id = 460,
                QuestionText = "Given that external pressure is 1 atm, a substance with a boiling point and melting point both above room temperature (25 °C) must be a solid.",
                Answer = true,
                AnswerText = "Given that external pressure is 1 atm, a substance such as iron whose boiling point is at 2750 °C and melting point is at 1535 °C would be in the solid state.",
                Category = "Science"
            };
            questions.Add(q460);

            OXQuizMetadata q461 = new OXQuizMetadata()
            {
                Id = 461,
                QuestionText = "Given that external pressure is 1 atm, a substance with a boiling point below room temperature (25 °C) must be a gas.",
                Answer = true,
                AnswerText = "Given that external pressure is 1 atm, substances such as water and nitrogen whose boiling points are below room temperature (25 °C) would be in the gas state.",
                Category = "Science"
            };
            questions.Add(q461);

            OXQuizMetadata q462 = new OXQuizMetadata()
            {
                Id = 462,
                QuestionText = "Given that external pressure is 1 atm, a substance with a boiling point below room temperature (25 °C) must be a solid.",
                Answer = false,
                AnswerText = "Given that external pressure is 1 atm, substances such as water and nitrogen whose boiling points are below room temperature (25 °C) would be in the gas state.",
                Category = "Science"
            };
            questions.Add(q462);

            OXQuizMetadata q463 = new OXQuizMetadata()
            {
                Id = 463,
                QuestionText = "Given that external pressure is constant, a pure substance's boiling point and freezing point are the same.",
                Answer = true,
                AnswerText = "A substance maintains its temperature during the boiling process and melting process. At 1 atm, water freezes at 0 °C and ice melts at 0 °C.",
                Category = "Science"
            };
            questions.Add(q463);

            OXQuizMetadata q464 = new OXQuizMetadata()
            {
                Id = 464,
                QuestionText = "Given that external pressure is constant, a pure substance's boiling point depends on its amount.",
                Answer = false,
                AnswerText = "Different substances boil at a different temperatures. It's a useful property to identify and categorize different substances.",
                Category = "Science"
            };
            questions.Add(q464);

            OXQuizMetadata q465 = new OXQuizMetadata()
            {
                Id = 465,
                QuestionText = "Given that external pressure is constant, a pure substance's boiling point doesn't depend on its amount.",
                Answer = true,
                AnswerText = "Sheer volume won't change the boiling point. It will alter the amount of time and heat required to boil, though.",
                Category = "Science"
            };
            questions.Add(q465);

            OXQuizMetadata q466 = new OXQuizMetadata()
            {
                Id = 466,
                QuestionText = "Given that external pressure is constant, a pure substance's evaporation process happens at constant temperature.",
                Answer = true,
                AnswerText = "The boiling point of a substance is the temperature at which the liquid changes into a vapor while boiling at a constant temperature.",
                Category = "Science"
            };
            questions.Add(q466);

            OXQuizMetadata q467 = new OXQuizMetadata()
            {
                Id = 467,
                QuestionText = "Given that the external pressure is 1 atm, a substance with a boiling point below room temperature (25°C) must be a gas.",
                Answer = true,
                AnswerText = "Given that the external pressure is 1 atm, substances such as water and nitrogen whose boiling points are below room temperature (25°C) would be in the gas state.",
                Category = "Science"
            };
            questions.Add(q467);

            OXQuizMetadata q468 = new OXQuizMetadata()
            {
                Id = 468,
                QuestionText = "Go (weiqi) is an official event of 2010 Asian Games in Guangzhou.",
                Answer = true,
                AnswerText = "Go (weiqi) became an official event of the 2010 Asian Games Guangzhou. In 2010 Asian Games, Korea won three types of Go events.",
                Category = "History"
            };
            questions.Add(q468);

            OXQuizMetadata q469 = new OXQuizMetadata()
            {
                Id = 469,
                QuestionText = "Goldus Group has an official song for Goldus.",
                Answer = true,
                AnswerText = "The company uses this song for its chairman as a pre-employment test.",
                Category = "MapleStory 2"
            };
            questions.Add(q469);

            OXQuizMetadata q470 = new OXQuizMetadata()
            {
                Id = 470,
                QuestionText = "Graham Bill invented the telephone.",
                Answer = false,
                AnswerText = "Graham Bell invented the telephone.",
                Category = "General Knowledge"
            };
            questions.Add(q470);

            OXQuizMetadata q471 = new OXQuizMetadata()
            {
                Id = 471,
                QuestionText = "Greece lost the Trojan War.",
                Answer = false,
                AnswerText = "Odysseus used a Trojan horse and scores a great victory for his homeland of Greece.",
                Category = "History"
            };
            questions.Add(q471);

            OXQuizMetadata q472 = new OXQuizMetadata()
            {
                Id = 472,
                QuestionText = "Greece won the Trojan war.",
                Answer = true,
                AnswerText = "Odysseus used a Trojan horse and scored a great victory for his homeland of Greece.",
                Category = "History"
            };
            questions.Add(q472);

            OXQuizMetadata q473 = new OXQuizMetadata()
            {
                Id = 473,
                QuestionText = "Greenland is part of the Netherlands.",
                Answer = false,
                AnswerText = "Greenland is an autonomous constituent country within the Kingdom of Denmark.",
                Category = "History"
            };
            questions.Add(q473);

            OXQuizMetadata q474 = new OXQuizMetadata()
            {
                Id = 474,
                QuestionText = "Greenpeace are international environmental terrorists.",
                Answer = false,
                AnswerText = "Greenpeace is an international environmental activist organization.",
                Category = "General Knowledge"
            };
            questions.Add(q474);

            OXQuizMetadata q475 = new OXQuizMetadata()
            {
                Id = 475,
                QuestionText = "Greenpeace is an international environmental activist organization.",
                Answer = true,
                AnswerText = "Greenpeace is an international environmental activist organization.",
                Category = "General Knowledge"
            };
            questions.Add(q475);

            OXQuizMetadata q476 = new OXQuizMetadata()
            {
                Id = 476,
                QuestionText = "Greenpeace is an organization aiming to free Palestine.",
                Answer = false,
                AnswerText = "Greenpeace is an international environmental activist organization.",
                Category = "General Knowledge"
            };
            questions.Add(q476);

            OXQuizMetadata q477 = new OXQuizMetadata()
            {
                Id = 477,
                QuestionText = "Gregor Mendel, in his 1866 paper on pea plant experimentation, discovered the dominance and recessivity in genes.",
                Answer = true,
                AnswerText = "Mendel in 1866 found the principles of inheritance.",
                Category = "Science"
            };
            questions.Add(q477);

            OXQuizMetadata q478 = new OXQuizMetadata()
            {
                Id = 478,
                QuestionText = "Grooving refers to a cat licking and cleaning its fur.",
                Answer = false,
                AnswerText = "Grooming refers to a cat's licking and cleaning of its fur.",
                Category = "General Knowledge"
            };
            questions.Add(q478);

            OXQuizMetadata q479 = new OXQuizMetadata()
            {
                Id = 479,
                QuestionText = "Guard Boss Chaw appears in the Kerning Waste Facility.",
                Answer = false,
                AnswerText = "Guard Boss Chaw appears in Kerning Interchange.",
                Category = "MapleStory 2"
            };
            questions.Add(q479);

            OXQuizMetadata q480 = new OXQuizMetadata()
            {
                Id = 480,
                QuestionText = "Guatemala City is not the capital of Guatemala.",
                Answer = false,
                AnswerText = "Guatemala City is the capital of Guatemala.",
                Category = "General Knowledge"
            };
            questions.Add(q480);

            OXQuizMetadata q481 = new OXQuizMetadata()
            {
                Id = 481,
                QuestionText = "Guava City is the capital of Guatemala.",
                Answer = false,
                AnswerText = "Guatemala City is the capital of Guatemala.",
                Category = "General Knowledge"
            };
            questions.Add(q481);

            OXQuizMetadata q482 = new OXQuizMetadata()
            {
                Id = 482,
                QuestionText = "Guggenheim museum is a signature work of architect Antonio Gaudi.",
                Answer = false,
                AnswerText = "The Guggenheim Museum is Frank Lloyd Wright's signature work.",
                Category = "General Knowledge"
            };
            questions.Add(q482);

            OXQuizMetadata q483 = new OXQuizMetadata()
            {
                Id = 483,
                QuestionText = "Guggenheim Museum is a signature work of architect Frank Lloyd Wright.",
                Answer = true,
                AnswerText = "The Guggenheim Museum is a well-known work of Frank Lloyd Wright who's regarded as the 20th century's most famous American architect.",
                Category = "General Knowledge"
            };
            questions.Add(q483);

            OXQuizMetadata q484 = new OXQuizMetadata()
            {
                Id = 484,
                QuestionText = "Gustav Klimt was born in 1861.",
                Answer = false,
                AnswerText = "Gustav Klimt was born in 1862.",
                Category = "Culture/Art"
            };
            questions.Add(q484);

            OXQuizMetadata q485 = new OXQuizMetadata()
            {
                Id = 485,
                QuestionText = "Gustav Klimt, an Austrian painter, was born in 1863.",
                Answer = false,
                AnswerText = "Gustav Klimt was born in 1862.",
                Category = "Culture/Art"
            };
            questions.Add(q485);

            OXQuizMetadata q486 = new OXQuizMetadata()
            {
                Id = 486,
                QuestionText = "Gustave Eiffel designed the steel frame of the Statue of Liberty.",
                Answer = true,
                AnswerText = "Gustave Eiffel, the designer of Eiffel tower, designed the steel frame of the Statue of Liberty.",
                Category = "General Knowledge"
            };
            questions.Add(q486);

            OXQuizMetadata q487 = new OXQuizMetadata()
            {
                Id = 487,
                QuestionText = "Gustavia is the capital of Saint Barthelemy.",
                Answer = true,
                AnswerText = "Gustavia is the capital of Saint Barthélemy.",
                Category = "General Knowledge"
            };
            questions.Add(q487);

            OXQuizMetadata q488 = new OXQuizMetadata()
            {
                Id = 488,
                QuestionText = "Hair is composed of carbohydrates.",
                Answer = false,
                AnswerText = "Hair is composed of proteins.",
                Category = "General Knowledge"
            };
            questions.Add(q488);

            OXQuizMetadata q489 = new OXQuizMetadata()
            {
                Id = 489,
                QuestionText = "Han Solo hides the Millennium Falcon on the hull of another star ship to evade capture in The Empire Strikes Back.",
                Answer = true,
                AnswerText = "Han Solo hides the Millennium Falcon on the hull of another star ship to evade capture in The Empire Strikes Back.",
                Category = "Culture/Art"
            };
            questions.Add(q489);

            OXQuizMetadata q490 = new OXQuizMetadata()
            {
                Id = 490,
                QuestionText = "Handball is an indoor team sport where nine players play on a team.",
                Answer = false,
                AnswerText = "Seven players play on a team.",
                Category = "Culture/Art"
            };
            questions.Add(q490);

            OXQuizMetadata q491 = new OXQuizMetadata()
            {
                Id = 491,
                QuestionText = "Hans Andes wrote \"The Little Mermaid.\"",
                Answer = false,
                AnswerText = "Hans Christian Andersen wrote \"The Little Mermaid.\"",
                Category = "Culture/Art"
            };
            questions.Add(q491);

            OXQuizMetadata q492 = new OXQuizMetadata()
            {
                Id = 492,
                QuestionText = "Hans Christian Andersen is Danish.",
                Answer = true,
                AnswerText = "Thanks to Hans Christian Andersen, Denmark is known to be the land of the fairy tales.",
                Category = "General Knowledge"
            };
            questions.Add(q492);

            OXQuizMetadata q493 = new OXQuizMetadata()
            {
                Id = 493,
                QuestionText = "Hans Christian Andersen is Swedish.",
                Answer = false,
                AnswerText = "Thanks to Hans Christian Andersen, Denmark is known to be the land of the fairy tales.",
                Category = "General Knowledge"
            };
            questions.Add(q493);

            OXQuizMetadata q494 = new OXQuizMetadata()
            {
                Id = 494,
                QuestionText = "Hans Christian Andersen was a prominent French entomologist who wrote Souvenirs Entomologiques.",
                Answer = false,
                AnswerText = "The author of Souvenirs Entomologiques was Jean-Henri Fabre.",
                Category = "Culture/Art"
            };
            questions.Add(q494);

            OXQuizMetadata q495 = new OXQuizMetadata()
            {
                Id = 495,
                QuestionText = "Hans Christian Andersen wrote the Little Mermaid.",
                Answer = true,
                AnswerText = "Hans Christian Anderson wrote the Little Mermaid.",
                Category = "Culture/Art"
            };
            questions.Add(q495);

            OXQuizMetadata q496 = new OXQuizMetadata()
            {
                Id = 496,
                QuestionText = "Harry Potter went to the magic school Hogwarts.",
                Answer = true,
                AnswerText = "Harry Potter went to Hogwarts starting at the age of 11.",
                Category = "Culture/Art"
            };
            questions.Add(q496);

            OXQuizMetadata q497 = new OXQuizMetadata()
            {
                Id = 497,
                QuestionText = "Heat haze, which usually happens in spring, is caused by refraction of light.",
                Answer = true,
                AnswerText = "The refraction of light causes it.",
                Category = "Science"
            };
            questions.Add(q497);

            OXQuizMetadata q498 = new OXQuizMetadata()
            {
                Id = 498,
                QuestionText = "Heat haze, which usually happens in spring, is caused by refraction of sound.",
                Answer = false,
                AnswerText = "The refraction of light causes it.",
                Category = "Science"
            };
            questions.Add(q498);

            OXQuizMetadata q499 = new OXQuizMetadata()
            {
                Id = 499,
                QuestionText = "Heath Ledger played the role of Joker in the movie The Dark Knight.",
                Answer = true,
                AnswerText = "Heath Ledger played the role of Joker in the movie The Dark Knight.",
                Category = "Culture/Art"
            };
            questions.Add(q499);

            OXQuizMetadata q500 = new OXQuizMetadata()
            {
                Id = 500,
                QuestionText = "Heavy rain has no effects onlandslides.",
                Answer = false,
                AnswerText = "Heavy rain causes landslides.",
                Category = "General Knowledge"
            };
            questions.Add(q500);

            OXQuizMetadata q501 = new OXQuizMetadata()
            {
                Id = 501,
                QuestionText = "Heavy rain strongly affects landslides.",
                Answer = true,
                AnswerText = "Heavy rain causes landslides.",
                Category = "General Knowledge"
            };
            questions.Add(q501);

            OXQuizMetadata q502 = new OXQuizMetadata()
            {
                Id = 502,
                QuestionText = "Hellenism significantly influenced Gandhara art.",
                Answer = true,
                AnswerText = "That's why Gandhara art focuses on depicting humans as realistically as possible.",
                Category = "Culture/Art"
            };
            questions.Add(q502);

            OXQuizMetadata q503 = new OXQuizMetadata()
            {
                Id = 503,
                QuestionText = "Hemokan is higher level than Pamokan.",
                Answer = false,
                AnswerText = "Hemokan is lower level than Pamokan.",
                Category = "MapleStory 2"
            };
            questions.Add(q503);

            OXQuizMetadata q504 = new OXQuizMetadata()
            {
                Id = 504,
                QuestionText = "Hemokan is lower level than Pamokan.",
                Answer = true,
                AnswerText = "Hemokan is lower level than Pamokan.",
                Category = "MapleStory 2"
            };
            questions.Add(q504);

            OXQuizMetadata q505 = new OXQuizMetadata()
            {
                Id = 505,
                QuestionText = "Hemokan lurks within the dungeon at Lava Springs.",
                Answer = false,
                AnswerText = "The dungeon at Lava Springs is Ignicore, home to Pamokan.",
                Category = "MapleStory 2"
            };
            questions.Add(q505);

            OXQuizMetadata q506 = new OXQuizMetadata()
            {
                Id = 506,
                QuestionText = "Henri Matisse is a Fauve.",
                Answer = true,
                AnswerText = "Fauvism is a radical method of painting, which values individual expression over those taught in institutions.",
                Category = "Culture/Art"
            };
            questions.Add(q506);

            OXQuizMetadata q507 = new OXQuizMetadata()
            {
                Id = 507,
                QuestionText = "Hens must incubate their eggs for three weeks.",
                Answer = true,
                AnswerText = "The egg incubation period is three weeks.",
                Category = "Science"
            };
            questions.Add(q507);

            OXQuizMetadata q508 = new OXQuizMetadata()
            {
                Id = 508,
                QuestionText = "Hens must incubate their eggs for two weeks.",
                Answer = false,
                AnswerText = "Eggs hatch in three weeks.",
                Category = "Science"
            };
            questions.Add(q508);

            OXQuizMetadata q509 = new OXQuizMetadata()
            {
                Id = 509,
                QuestionText = "Homo neanderthalensis, whose traces lead back 3.9 million years ago, is the first species of humans.",
                Answer = false,
                AnswerText = "Australopithecus is the first known human.",
                Category = "History"
            };
            questions.Add(q509);

            OXQuizMetadata q510 = new OXQuizMetadata()
            {
                Id = 510,
                QuestionText = "Homo sapiens (humans) appeared 1.8 million years ago and left cave paintings.",
                Answer = false,
                AnswerText = "Homo sapiens (humans) appeared nearly 200,000 years ago.",
                Category = "History"
            };
            questions.Add(q510);

            OXQuizMetadata q511 = new OXQuizMetadata()
            {
                Id = 511,
                QuestionText = "Horticulture refers to the farming of crops such as cabbage and bracken in a high-altitude environment.",
                Answer = false,
                AnswerText = "Highland agriculture refers to the farming of crops in a high-altitude environment.",
                Category = "General Knowledge"
            };
            questions.Add(q511);

            OXQuizMetadata q512 = new OXQuizMetadata()
            {
                Id = 512,
                QuestionText = "Horus is the leader of the griffins.",
                Answer = true,
                AnswerText = "Horus is ranked highest among the three griffins that awoke at the top of the holy Vayar Mountain.",
                Category = "MapleStory 2"
            };
            questions.Add(q512);

            OXQuizMetadata q513 = new OXQuizMetadata()
            {
                Id = 513,
                QuestionText = "Hugh Jackman played the role of Wolverine.",
                Answer = true,
                AnswerText = "Hugh Jackman played the role of Wolverine.",
                Category = "Culture/Art"
            };
            questions.Add(q513);

            OXQuizMetadata q514 = new OXQuizMetadata()
            {
                Id = 514,
                QuestionText = "Humanism is at the foundation of the Renaissance.",
                Answer = true,
                AnswerText = "Humanist beliefs stress the potential value and goodness of human beings, emphasize common human needs, and seek solely rational ways of solving human problems.",
                Category = "Culture/Art"
            };
            questions.Add(q514);

            OXQuizMetadata q515 = new OXQuizMetadata()
            {
                Id = 515,
                QuestionText = "Humanitarianism is a philosophy of thinking based on the existence of God.",
                Answer = false,
                AnswerText = "Humanitarianism is a philosophy of thinking based on the promotion of human welfare.",
                Category = "Culture/Art"
            };
            questions.Add(q515);

            OXQuizMetadata q516 = new OXQuizMetadata()
            {
                Id = 516,
                QuestionText = "Humans are animals.",
                Answer = true,
                AnswerText = "Humans are animals.",
                Category = "General Knowledge"
            };
            questions.Add(q516);

            OXQuizMetadata q517 = new OXQuizMetadata()
            {
                Id = 517,
                QuestionText = "Humans are living organisms.",
                Answer = true,
                AnswerText = "Humans are living organisms and fall under the subcategory of animals.",
                Category = "General Knowledge"
            };
            questions.Add(q517);

            OXQuizMetadata q518 = new OXQuizMetadata()
            {
                Id = 518,
                QuestionText = "Humans are microorganisms.",
                Answer = false,
                AnswerText = "Humans aren't microorganisms.",
                Category = "General Knowledge"
            };
            questions.Add(q518);

            OXQuizMetadata q519 = new OXQuizMetadata()
            {
                Id = 519,
                QuestionText = "Humans are plants.",
                Answer = false,
                AnswerText = "Humans are classified as animals.",
                Category = "General Knowledge"
            };
            questions.Add(q519);

            OXQuizMetadata q520 = new OXQuizMetadata()
            {
                Id = 520,
                QuestionText = "Humans have 23 chromosomes.",
                Answer = false,
                AnswerText = "Humans have 23 pairs of chromosomes, 46 total.",
                Category = "Science"
            };
            questions.Add(q520);

            OXQuizMetadata q521 = new OXQuizMetadata()
            {
                Id = 521,
                QuestionText = "Humans have one kidney.",
                Answer = false,
                AnswerText = "Humans have two kidneys.",
                Category = "Science"
            };
            questions.Add(q521);

            OXQuizMetadata q522 = new OXQuizMetadata()
            {
                Id = 522,
                QuestionText = "Hurdling is a field event in track-and-field contests.",
                Answer = false,
                AnswerText = "Hurdling is a track event in track-and-field contests.",
                Category = "Culture/Art"
            };
            questions.Add(q522);

            OXQuizMetadata q523 = new OXQuizMetadata()
            {
                Id = 523,
                QuestionText = "Hurum, the guardian of Fairy Tree Lake, is capable of speaking human.",
                Answer = true,
                AnswerText = "Press the Spacebar to talk to him.",
                Category = "MapleStory 2"
            };
            questions.Add(q523);

            OXQuizMetadata q524 = new OXQuizMetadata()
            {
                Id = 524,
                QuestionText = "IC is shorthand for Integrated Circuit.",
                Answer = true,
                AnswerText = "IC is shorthand for Integrated Circuit.",
                Category = "IT"
            };
            questions.Add(q524);

            OXQuizMetadata q525 = new OXQuizMetadata()
            {
                Id = 525,
                QuestionText = "Ice skates are oblong footwear worn on snow.",
                Answer = false,
                AnswerText = "Those are called skis.",
                Category = "Culture/Art"
            };
            questions.Add(q525);

            OXQuizMetadata q526 = new OXQuizMetadata()
            {
                Id = 526,
                QuestionText = "IDC is the shorthand for \"International Olympic Committee.\"",
                Answer = false,
                AnswerText = "IOC is the shorthand for \"International Olympic Committee.\"",
                Category = "General Knowledge"
            };
            questions.Add(q526);

            OXQuizMetadata q527 = new OXQuizMetadata()
            {
                Id = 527,
                QuestionText = "If a mamushi accidentally bites its tongue, it dies from its own poison.",
                Answer = false,
                AnswerText = "A mamushi won't die of its poison even if it accidentally bites its tongue. It is resistant to its poison.",
                Category = "General Knowledge"
            };
            questions.Add(q527);

            OXQuizMetadata q528 = new OXQuizMetadata()
            {
                Id = 528,
                QuestionText = "If a mini-game opens and you click the Event pop-up window, you will have to pay a taxi fare to join the event.",
                Answer = false,
                AnswerText = "If a mini-game opens and you click the Event pop-up window, you can move to the event map free of charge.",
                Category = "MapleStory 2"
            };
            questions.Add(q528);

            OXQuizMetadata q529 = new OXQuizMetadata()
            {
                Id = 529,
                QuestionText = "If hit by an earthquake, one must climb on top of a desk.",
                Answer = false,
                AnswerText = "If hit by an earthquake, one should hide under a desk.",
                Category = "General Knowledge"
            };
            questions.Add(q529);

            OXQuizMetadata q530 = new OXQuizMetadata()
            {
                Id = 530,
                QuestionText = "If measured, one's weight is increased in a descending elevator.",
                Answer = false,
                AnswerText = "Because of gravity, one's weight is decreased in a descending elevator.",
                Category = "Science"
            };
            questions.Add(q530);

            OXQuizMetadata q531 = new OXQuizMetadata()
            {
                Id = 531,
                QuestionText = "If spinach is simmered in an open pot, its color remains unchanged.",
                Answer = true,
                AnswerText = "If spinach is simmered in an open pot, its color remains unchanged.",
                Category = "Science"
            };
            questions.Add(q531);

            OXQuizMetadata q532 = new OXQuizMetadata()
            {
                Id = 532,
                QuestionText = "If you buy a UGC item and the item gets banned by the Operation Team, you will get your money back.",
                Answer = true,
                AnswerText = "You will get your money back in your Mailbox.",
                Category = "MapleStory 2"
            };
            questions.Add(q532);

            OXQuizMetadata q533 = new OXQuizMetadata()
            {
                Id = 533,
                QuestionText = "ILO is the shorthand for \"International Labour Organization.\"",
                Answer = true,
                AnswerText = "ILO is \"International Labour Organization.\"",
                Category = "Society"
            };
            questions.Add(q533);

            OXQuizMetadata q534 = new OXQuizMetadata()
            {
                Id = 534,
                QuestionText = "ILQ is the shorthand for \"International Labour Organization.\"",
                Answer = false,
                AnswerText = "ILO is the shorthand for \"International Labour Organization.\"",
                Category = "Society"
            };
            questions.Add(q534);

            OXQuizMetadata q535 = new OXQuizMetadata()
            {
                Id = 535,
                QuestionText = "In \"Alice and the Wonderland,\" the animal that leads Alice into a land of mystery is a rabbit.",
                Answer = true,
                AnswerText = "It was the rabbit.",
                Category = "Culture/Art"
            };
            questions.Add(q535);

            OXQuizMetadata q536 = new OXQuizMetadata()
            {
                Id = 536,
                QuestionText = "In \"Harry Potter,\" Malfoy is the name of Harry's best friend.",
                Answer = false,
                AnswerText = "In \"Harry Potter,\" Ron and Hermione are Harry's best friends. Malfoy is a target of hatred.",
                Category = "Culture/Art"
            };
            questions.Add(q536);

            OXQuizMetadata q537 = new OXQuizMetadata()
            {
                Id = 537,
                QuestionText = "In \"Snow White,\" the dwarves are farmers.",
                Answer = false,
                AnswerText = "In \"Snow White,\" the dwarfs are miners.",
                Category = "Culture/Art"
            };
            questions.Add(q537);

            OXQuizMetadata q538 = new OXQuizMetadata()
            {
                Id = 538,
                QuestionText = "In 1453, the Ottoman Empire officially renamed Constantinople, which had been Byzantine Empire's capital, to Istanbul.",
                Answer = false,
                AnswerText = "Only in the 1930s did Istanbul become the city's official name.",
                Category = "History"
            };
            questions.Add(q538);

            OXQuizMetadata q539 = new OXQuizMetadata()
            {
                Id = 539,
                QuestionText = "In 1927, USA's Paramount Pictures invented a talkie, which started the generation of movies with sounds.",
                Answer = false,
                AnswerText = "Warner Brothers started the generation of movies with sounds.",
                Category = "Culture/Art"
            };
            questions.Add(q539);

            OXQuizMetadata q540 = new OXQuizMetadata()
            {
                Id = 540,
                QuestionText = "In 1927, USA's Warner Brothers invented a talkie, which started the generation of movies with sounds.",
                Answer = true,
                AnswerText = "Warner Brothers started the generation of movies with sounds.",
                Category = "Culture/Art"
            };
            questions.Add(q540);

            OXQuizMetadata q541 = new OXQuizMetadata()
            {
                Id = 541,
                QuestionText = "In 1948, at the third United Nations General Assembly, the \"Universal Declaration of Human Rights\" was chosen as the human rights declaration.",
                Answer = true,
                AnswerText = "The \"Universal Declaration of Human Rights\" specifies individual's basic rights and his or her rights to labor and life.",
                Category = "History"
            };
            questions.Add(q541);

            OXQuizMetadata q542 = new OXQuizMetadata()
            {
                Id = 542,
                QuestionText = "In 1988 Seoul Olympics, China won the overall number one spot.",
                Answer = false,
                AnswerText = "In the 1988 Seoul Olympics, it was the Soviet Union that won the overall number one spot.",
                Category = "General Knowledge"
            };
            questions.Add(q542);

            OXQuizMetadata q543 = new OXQuizMetadata()
            {
                Id = 543,
                QuestionText = "In 2008, Korean male idol group TVXQ broke a new Guinness record for having more than 1 million fan club members.",
                Answer = false,
                AnswerText = "In 2008, Korean male idol group TVXQ broke a new Guinness record for having more than 800 thousand fan club members.",
                Category = "General Knowledge"
            };
            questions.Add(q543);

            OXQuizMetadata q544 = new OXQuizMetadata()
            {
                Id = 544,
                QuestionText = "In a game of ice hockey, 8 members from a team of 22 persons play.",
                Answer = false,
                AnswerText = "6 members are on a team.",
                Category = "General Knowledge"
            };
            questions.Add(q544);

            OXQuizMetadata q545 = new OXQuizMetadata()
            {
                Id = 545,
                QuestionText = "In a golf course, a depression filled with impediments such as sand is called a trunk.",
                Answer = false,
                AnswerText = "One of the obstacles in golf, the bunker is a depression filled with impediments such as sand.",
                Category = "Culture/Art"
            };
            questions.Add(q545);

            OXQuizMetadata q546 = new OXQuizMetadata()
            {
                Id = 546,
                QuestionText = "In a hotel, Double Bedroom refers to a room that has two single-sized beds.",
                Answer = false,
                AnswerText = "A room with two beds is called a twin bedroom.",
                Category = "General Knowledge"
            };
            questions.Add(q546);

            OXQuizMetadata q547 = new OXQuizMetadata()
            {
                Id = 547,
                QuestionText = "In a traditional artist's color wheel, the primary colors are red, green, and blue.",
                Answer = true,
                AnswerText = "The primary colors are red, yellow, and blue.",
                Category = "General Knowledge"
            };
            questions.Add(q547);

            OXQuizMetadata q548 = new OXQuizMetadata()
            {
                Id = 548,
                QuestionText = "In Alexandre Dumas's novel, \"The Three Musketeers,\" the musketeers are Athos, Aramis, and Porthos.",
                Answer = true,
                AnswerText = "After D'Artagnan joined, the trio became four musketeers.",
                Category = "Culture/Art"
            };
            questions.Add(q548);

            OXQuizMetadata q549 = new OXQuizMetadata()
            {
                Id = 549,
                QuestionText = "In all events, local anesthesia is safer than general anesthesia.",
                Answer = false,
                AnswerText = "Local Anesthesia isn't always safer than general anesthesia.",
                Category = "Science"
            };
            questions.Add(q549);

            OXQuizMetadata q550 = new OXQuizMetadata()
            {
                Id = 550,
                QuestionText = "In anatomy, what separates the head from the face is forehead.",
                Answer = false,
                AnswerText = "In anatomy, what separates the head from the face is eyebrows.",
                Category = "General Knowledge"
            };
            questions.Add(q550);

            OXQuizMetadata q551 = new OXQuizMetadata()
            {
                Id = 551,
                QuestionText = "In architecture, the Doric order is considered feminine while the Ionic order is seen as masculine.",
                Answer = false,
                AnswerText = "In architecture, the Doric order is considered masculine while the Ionic order is seen as feminine.",
                Category = "Culture/Art"
            };
            questions.Add(q551);

            OXQuizMetadata q552 = new OXQuizMetadata()
            {
                Id = 552,
                QuestionText = "In autumn, the leaves of the trees in the northern region turn red first before those in the southern region.",
                Answer = true,
                AnswerText = "In autumn, the leaves of the trees in the northern region turn red first before those in the southern region.",
                Category = "Science"
            };
            questions.Add(q552);

            OXQuizMetadata q553 = new OXQuizMetadata()
            {
                Id = 553,
                QuestionText = "In Autumn, the leaves of the trees in the southern region turn red first before those in the northern region.",
                Answer = false,
                AnswerText = "In Autumn, the leaves of the trees in the northern regions turn red before those in the southern regions.",
                Category = "Science"
            };
            questions.Add(q553);

            OXQuizMetadata q554 = new OXQuizMetadata()
            {
                Id = 554,
                QuestionText = "In badminton, the server must keep the whole shuttle above one's waist and serve with an overarm hitting action.",
                Answer = false,
                AnswerText = "Some of the fouls that occur during a serve include overhand, over waist, etc.",
                Category = "Culture/Art"
            };
            questions.Add(q554);

            OXQuizMetadata q555 = new OXQuizMetadata()
            {
                Id = 555,
                QuestionText = "In badminton, the server must keep the whole shuttle below one's waist and serve with an underarm hitting action.",
                Answer = true,
                AnswerText = "Some of the fouls that occur during a serve include overhand, over waist, etc.",
                Category = "Culture/Art"
            };
            questions.Add(q555);

            OXQuizMetadata q556 = new OXQuizMetadata()
            {
                Id = 556,
                QuestionText = "In baseball, \"hot corner\" refers to the third base.",
                Answer = true,
                AnswerText = "In baseball, \"hot corner\" refers to the third base.",
                Category = "Culture/Art"
            };
            questions.Add(q556);

            OXQuizMetadata q557 = new OXQuizMetadata()
            {
                Id = 557,
                QuestionText = "In baseball, a grand slam means that a player made all four types of hits (single, double, triple, and home run) in a game.",
                Answer = false,
                AnswerText = "\"Hitting for the cycle\" is the term.",
                Category = "Culture/Art"
            };
            questions.Add(q557);

            OXQuizMetadata q558 = new OXQuizMetadata()
            {
                Id = 558,
                QuestionText = "In baseball, the grand slam means a home run hit with all three bases loaded.",
                Answer = true,
                AnswerText = "In baseball, the grand slam means a home run hit with all three bases loaded.",
                Category = "Culture/Art"
            };
            questions.Add(q558);

            OXQuizMetadata q559 = new OXQuizMetadata()
            {
                Id = 559,
                QuestionText = "In baseball, the grand slam means a solo home run.",
                Answer = false,
                AnswerText = "In baseball, a grand slam means a home run hit with all three bases loaded.",
                Category = "Culture/Art"
            };
            questions.Add(q559);

            OXQuizMetadata q560 = new OXQuizMetadata()
            {
                Id = 560,
                QuestionText = "In bowling, a kingpin refers to the number 1 pin.",
                Answer = false,
                AnswerText = "In bowling, a kingpin refers to the number 5 pin.",
                Category = "Culture/Art"
            };
            questions.Add(q560);

            OXQuizMetadata q561 = new OXQuizMetadata()
            {
                Id = 561,
                QuestionText = "In computing, BUS is a communication system that transfers data between components.",
                Answer = true,
                AnswerText = "In computing, BUS is a communication system that transfers data between components.",
                Category = "IT"
            };
            questions.Add(q561);

            OXQuizMetadata q562 = new OXQuizMetadata()
            {
                Id = 562,
                QuestionText = "In computing, TAXI is a communication system that transfers data between components.",
                Answer = false,
                AnswerText = "In computing, BUS is a communication system that transfers data between CPU, memory, and input/output devices.",
                Category = "IT"
            };
            questions.Add(q562);

            OXQuizMetadata q563 = new OXQuizMetadata()
            {
                Id = 563,
                QuestionText = "In fencing, epee is the only sword that can score points from the edge of its blade.",
                Answer = false,
                AnswerText = "In fencing, sabre is the only sword that can score points from the edge of its blade.",
                Category = "Culture/Art"
            };
            questions.Add(q563);

            OXQuizMetadata q564 = new OXQuizMetadata()
            {
                Id = 564,
                QuestionText = "In golf jargon, \"referee\" refers to a group of spectators.",
                Answer = false,
                AnswerText = "In golf jargon, \"gallery\" refers to a group of spectators.",
                Category = "Culture/Art"
            };
            questions.Add(q564);

            OXQuizMetadata q565 = new OXQuizMetadata()
            {
                Id = 565,
                QuestionText = "In golf, birdie means scoring two under par.",
                Answer = false,
                AnswerText = "In golf, eagle means scoring two under par.",
                Category = "Culture/Art"
            };
            questions.Add(q565);

            OXQuizMetadata q566 = new OXQuizMetadata()
            {
                Id = 566,
                QuestionText = "In Greek and Roman mythology, Zeus wedded Hera.",
                Answer = true,
                AnswerText = "In Greek and Roman mythology, Zeus wedded Hera.",
                Category = "Culture/Art"
            };
            questions.Add(q566);

            OXQuizMetadata q567 = new OXQuizMetadata()
            {
                Id = 567,
                QuestionText = "In Greek and Roman mythology, Zeus wedded Hestia.",
                Answer = false,
                AnswerText = "Zeus and Hera are married.",
                Category = "Culture/Art"
            };
            questions.Add(q567);

            OXQuizMetadata q568 = new OXQuizMetadata()
            {
                Id = 568,
                QuestionText = "In Greek mythology, Apollo is the god of wine and winemaking.",
                Answer = false,
                AnswerText = "Dionysus is the god of wine.",
                Category = "Culture/Art"
            };
            questions.Add(q568);

            OXQuizMetadata q569 = new OXQuizMetadata()
            {
                Id = 569,
                QuestionText = "In Greek mythology, Athene is the goddess of love.",
                Answer = false,
                AnswerText = "Aphrodite is the goddess of love.",
                Category = "General Knowledge"
            };
            questions.Add(q569);

            OXQuizMetadata q570 = new OXQuizMetadata()
            {
                Id = 570,
                QuestionText = "In Greek mythology, Megaera is the goddess who personifies discord and conflict.",
                Answer = false,
                AnswerText = "In Greek mythology, Eris is the goddess who personifies discord and conflict.",
                Category = "General Knowledge"
            };
            questions.Add(q570);

            OXQuizMetadata q571 = new OXQuizMetadata()
            {
                Id = 571,
                QuestionText = "In Greek mythology, the Minotaur is a creature with the head of a pig.",
                Answer = false,
                AnswerText = "The Minotaur is a creature with the head of a bull.",
                Category = "Culture/Art"
            };
            questions.Add(q571);

            OXQuizMetadata q572 = new OXQuizMetadata()
            {
                Id = 572,
                QuestionText = "In Harry Potter, Hagrid opened the Chamber of Secrets.",
                Answer = false,
                AnswerText = "Tom Riddle opened the Chamber of Secrets.",
                Category = "Culture/Art"
            };
            questions.Add(q572);

            OXQuizMetadata q573 = new OXQuizMetadata()
            {
                Id = 573,
                QuestionText = "In horse racing, a dark horse refers to a horse whose performances are little known.",
                Answer = true,
                AnswerText = "\"Dark Horse\" has gained popularity and is used in other competitions such as election, games, etc.",
                Category = "Culture/Art"
            };
            questions.Add(q573);

            OXQuizMetadata q574 = new OXQuizMetadata()
            {
                Id = 574,
                QuestionText = "In India's caste system, brahmins are the warrior class.",
                Answer = false,
                AnswerText = "In India's caste system, Kshatriya is the warrior class.",
                Category = "General Knowledge"
            };
            questions.Add(q574);

            OXQuizMetadata q575 = new OXQuizMetadata()
            {
                Id = 575,
                QuestionText = "In India's caste system, Kshatriya is the warrior class.",
                Answer = true,
                AnswerText = "In India's caste system, Kshatriya is the warrior class.",
                Category = "General Knowledge"
            };
            questions.Add(q575);

            OXQuizMetadata q576 = new OXQuizMetadata()
            {
                Id = 576,
                QuestionText = "In labor union terms, 3Ds refer to jobs that are dirty, different, and dangerous.",
                Answer = false,
                AnswerText = "3Ds refer to jobs that are shunned because they are dirty, difficult, and dangerous.",
                Category = "Society"
            };
            questions.Add(q576);

            OXQuizMetadata q577 = new OXQuizMetadata()
            {
                Id = 577,
                QuestionText = "In labor union terms, 3Ds refer to jobs that are dirty, different, and difficult.",
                Answer = false,
                AnswerText = "3Ds refer to jobs that are shunned because they are dirty, difficult, and dangerous.",
                Category = "Society"
            };
            questions.Add(q577);

            OXQuizMetadata q578 = new OXQuizMetadata()
            {
                Id = 578,
                QuestionText = "In labor union terms, 3Ds refer to jobs that are dirty, difficult, and dangerous.",
                Answer = true,
                AnswerText = "3Ds refer to jobs that are shunned because they are dirty, difficult, and dangerous.",
                Category = "General Knowledge"
            };
            questions.Add(q578);

            OXQuizMetadata q579 = new OXQuizMetadata()
            {
                Id = 579,
                QuestionText = "In labor union terms, 3Ds refer to jobs that are Dirty, Difficult, and Different.",
                Answer = false,
                AnswerText = "3Ds refer to jobs that are shunned because they are Dirty, Difficult, and Dangerous.",
                Category = "General Knowledge"
            };
            questions.Add(q579);

            OXQuizMetadata q580 = new OXQuizMetadata()
            {
                Id = 580,
                QuestionText = "In Maplestory 2, the act of improving your gear is called Refining.",
                Answer = false,
                AnswerText = "It's Enchanting, not Refining.",
                Category = "MapleStory 2"
            };
            questions.Add(q580);

            OXQuizMetadata q581 = new OXQuizMetadata()
            {
                Id = 581,
                QuestionText = "In MapleStory 2, the Mage classes are Wizard and Priest.",
                Answer = true,
                AnswerText = "In MapleStory 2, the Mage classes are Wizard and Priest.",
                Category = "MapleStory 2"
            };
            questions.Add(q581);

            OXQuizMetadata q582 = new OXQuizMetadata()
            {
                Id = 582,
                QuestionText = "In Maplestory 2, the South and North Royal Roads have been broken apart.",
                Answer = true,
                AnswerText = "The Royal Road in Maple World has been torn apart. If you want to move from the south side of the road to the north side, you must take a detour through the Hushwood Vale and the Crooked Canyon.",
                Category = "MapleStory 2"
            };
            questions.Add(q582);

            OXQuizMetadata q583 = new OXQuizMetadata()
            {
                Id = 583,
                QuestionText = "In MapleStory 2, you can mail Mesos to your friends.",
                Answer = false,
                AnswerText = "Mesos and items cannot be mailed.",
                Category = "MapleStory 2"
            };
            questions.Add(q583);

            OXQuizMetadata q584 = new OXQuizMetadata()
            {
                Id = 584,
                QuestionText = "In Maplestory, even the monsters have levels.",
                Answer = true,
                AnswerText = "Monster stats differ based on their levels.",
                Category = "MapleStory"
            };
            questions.Add(q584);

            OXQuizMetadata q585 = new OXQuizMetadata()
            {
                Id = 585,
                QuestionText = "In MapleStory, the monsters don't have levels.",
                Answer = false,
                AnswerText = "Monster stats differ based on their levels.",
                Category = "General Knowledge"
            };
            questions.Add(q585);

            OXQuizMetadata q586 = new OXQuizMetadata()
            {
                Id = 586,
                QuestionText = "In MapleStory, the monsters don't use skills.",
                Answer = false,
                AnswerText = "The monsters also use skills.",
                Category = "General Knowledge"
            };
            questions.Add(q586);

            OXQuizMetadata q587 = new OXQuizMetadata()
            {
                Id = 587,
                QuestionText = "In MapleStory, there's a boss monster named Jr. Balrog.",
                Answer = true,
                AnswerText = "In MapleStory, there's a boss monster named Jr. Balrog.",
                Category = "MapleStory"
            };
            questions.Add(q587);

            OXQuizMetadata q588 = new OXQuizMetadata()
            {
                Id = 588,
                QuestionText = "In Medieval Europe, the broad sword was a type of sword made for stabbing.",
                Answer = false,
                AnswerText = "The rapier is the type of swords designed for stabbing. It's used in fencing as well.",
                Category = "Culture/Art"
            };
            questions.Add(q588);

            OXQuizMetadata q589 = new OXQuizMetadata()
            {
                Id = 589,
                QuestionText = "In medieval Europe, the peerage rank Marquess is above Baron.",
                Answer = true,
                AnswerText = "Marquess is above Baron.",
                Category = "Culture/Art"
            };
            questions.Add(q589);

            OXQuizMetadata q590 = new OXQuizMetadata()
            {
                Id = 590,
                QuestionText = "In mobile communications, LTE stands for Long Time Evolution.",
                Answer = false,
                AnswerText = "LTE stands for Long Term Evolution.",
                Category = "IT"
            };
            questions.Add(q590);

            OXQuizMetadata q591 = new OXQuizMetadata()
            {
                Id = 591,
                QuestionText = "In music, \"on point\" is a technique of finding musical lines that are harmonically interdependent yet independent in rhythm and contour.",
                Answer = false,
                AnswerText = "In music, \"counterpoint\" is a technique of finding musical lines that are harmonically interdependent yet independent in rhythm and contour.",
                Category = "Society"
            };
            questions.Add(q591);

            OXQuizMetadata q592 = new OXQuizMetadata()
            {
                Id = 592,
                QuestionText = "In music, a repeat sign indicates a section should be repeated.",
                Answer = true,
                AnswerText = "In music, a repeat sign indicates a section should be repeated.",
                Category = "Culture/Art"
            };
            questions.Add(q592);

            OXQuizMetadata q593 = new OXQuizMetadata()
            {
                Id = 593,
                QuestionText = "In music, cantabile means to play on beat.",
                Answer = false,
                AnswerText = "In music, cantabile means to play like it's sung.",
                Category = "Culture/Art"
            };
            questions.Add(q593);

            OXQuizMetadata q594 = new OXQuizMetadata()
            {
                Id = 594,
                QuestionText = "In music, staccato is a tricky part of a classical music piece played by a soloist, usually in improvision.",
                Answer = false,
                AnswerText = "In music, cadenza is a tricky solo part of a classical music piece that is usually improvised.",
                Category = "Culture/Art"
            };
            questions.Add(q594);

            OXQuizMetadata q595 = new OXQuizMetadata()
            {
                Id = 595,
                QuestionText = "In music, the cadenza is a tricky solo part of a classical music piece that is usually improvised.",
                Answer = true,
                AnswerText = "In music, the cadenza is a tricky solo part of a classical music piece that is usually improvised.",
                Category = "Culture/Art"
            };
            questions.Add(q595);

            OXQuizMetadata q596 = new OXQuizMetadata()
            {
                Id = 596,
                QuestionText = "In music, the counterpoint is a technique of finding musical lines that are harmonically interdependent yet independent in rhythm and contour.",
                Answer = true,
                AnswerText = "In music, the counterpoint is a technique of finding musical lines that are harmonically interdependent yet independent in rhythm and contour.",
                Category = "Society"
            };
            questions.Add(q596);

            OXQuizMetadata q597 = new OXQuizMetadata()
            {
                Id = 597,
                QuestionText = "In music, the prelude refers to a musical finale.",
                Answer = false,
                AnswerText = "In music, the prelude refers to an introductory piece of music.",
                Category = "Culture/Art"
            };
            questions.Add(q597);

            OXQuizMetadata q598 = new OXQuizMetadata()
            {
                Id = 598,
                QuestionText = "In Ninja Turtles, the names of the turtles are inspired by famous musicians.",
                Answer = false,
                AnswerText = "Leonardo, Michaelangelo, Raphael, and Donatello are all artists.",
                Category = "General Knowledge"
            };
            questions.Add(q598);

            OXQuizMetadata q599 = new OXQuizMetadata()
            {
                Id = 599,
                QuestionText = "In Pokemon, Raichu evolved from Raddish.",
                Answer = false,
                AnswerText = "In Pokemon: Raichu evolved from Pikachu.",
                Category = "Culture/Art"
            };
            questions.Add(q599);

            OXQuizMetadata q600 = new OXQuizMetadata()
            {
                Id = 600,
                QuestionText = "In professional baseball, the home plate's shape is a hexagon.",
                Answer = false,
                AnswerText = "In professional baseball, the home plate's shape is a pentagon.",
                Category = "Culture/Art"
            };
            questions.Add(q600);

            OXQuizMetadata q601 = new OXQuizMetadata()
            {
                Id = 601,
                QuestionText = "In Roman mythology, Venus is the goddess who deems family the top priority.",
                Answer = false,
                AnswerText = "Venus is the goddess of love.",
                Category = "General Knowledge"
            };
            questions.Add(q601);

            OXQuizMetadata q602 = new OXQuizMetadata()
            {
                Id = 602,
                QuestionText = "In soccer, 10 players make up a team.",
                Answer = false,
                AnswerText = "In soccer, 11 players make up a team.",
                Category = "Culture/Art"
            };
            questions.Add(q602);

            OXQuizMetadata q603 = new OXQuizMetadata()
            {
                Id = 603,
                QuestionText = "In soccer, a referee declares handling any time a ball hits an arm or hand of a player.",
                Answer = false,
                AnswerText = "Handling cannot be issued unless there was a deliberate attempt to play the ball with one's hand or arm.",
                Category = "Culture/Art"
            };
            questions.Add(q603);

            OXQuizMetadata q604 = new OXQuizMetadata()
            {
                Id = 604,
                QuestionText = "In space, a feather and a cannon ball have the same weight.",
                Answer = true,
                AnswerText = "Because there is no gravity in the space, both objects have 0 weight.",
                Category = "Science"
            };
            questions.Add(q604);

            OXQuizMetadata q605 = new OXQuizMetadata()
            {
                Id = 605,
                QuestionText = "In tennis, a volley is a shot in which the ball is struck after it bounces on the ground.",
                Answer = false,
                AnswerText = "In tennis, a volley is a shot in which the ball is struck before it bounces on the ground.",
                Category = "Culture/Art"
            };
            questions.Add(q605);

            OXQuizMetadata q606 = new OXQuizMetadata()
            {
                Id = 606,
                QuestionText = "In tennis, a volley is a shot in which the ball is struck before it bounces on the ground.",
                Answer = true,
                AnswerText = "In tennis, a volley is a shot in which the ball is struck before it bounces on the ground.",
                Category = "Culture/Art"
            };
            questions.Add(q606);

            OXQuizMetadata q607 = new OXQuizMetadata()
            {
                Id = 607,
                QuestionText = "In tennis, the score of 0 is called zero.",
                Answer = false,
                AnswerText = "In tennis, the score of 0 is called love.",
                Category = "General Knowledge"
            };
            questions.Add(q607);

            OXQuizMetadata q608 = new OXQuizMetadata()
            {
                Id = 608,
                QuestionText = "In Thailand, soldiers are drafted using the luck of the draw.",
                Answer = true,
                AnswerText = "Thailand's military uses conscription to recruit soldiers. In Thailand, however, soldiers are drafted using the luck of the draw.",
                Category = "Society"
            };
            questions.Add(q608);

            OXQuizMetadata q609 = new OXQuizMetadata()
            {
                Id = 609,
                QuestionText = "In the 4th dimension, gravity is a dimension added on top of the 3D world.",
                Answer = false,
                AnswerText = "In the 4th dimension, time and the 3D world aren't separate elements. They are continuous and integrated.",
                Category = "Science"
            };
            questions.Add(q609);

            OXQuizMetadata q610 = new OXQuizMetadata()
            {
                Id = 610,
                QuestionText = "In the 4th dimension, time is a dimension added to the 3D world.",
                Answer = true,
                AnswerText = "In the 4th dimension, time and the 3D world aren't separate elements. They are continuous and integrated.",
                Category = "Science"
            };
            questions.Add(q610);

            OXQuizMetadata q611 = new OXQuizMetadata()
            {
                Id = 611,
                QuestionText = "In the cartoon \"Teenage Mutant Ninja Turtles,\" the turtles' favorite food is ice cream.",
                Answer = false,
                AnswerText = "In the cartoon \"Teenage Mutant Ninja Turtles,\" the turtles' favorite food is pizza.",
                Category = "Culture/Art"
            };
            questions.Add(q611);

            OXQuizMetadata q612 = new OXQuizMetadata()
            {
                Id = 612,
                QuestionText = "In the Crusades, there were child soldiers.",
                Answer = true,
                AnswerText = "In the Crusades, there was a unit solely composed of child soldiers.",
                Category = "History"
            };
            questions.Add(q612);

            OXQuizMetadata q613 = new OXQuizMetadata()
            {
                Id = 613,
                QuestionText = "In the equation 438 X ( ) = 540930, 1235 is the number that fills the ().",
                Answer = true,
                AnswerText = "In the equation 438 X ( ) = 540930, 1235 is the number that fills the ().",
                Category = "Math"
            };
            questions.Add(q613);

            OXQuizMetadata q614 = new OXQuizMetadata()
            {
                Id = 614,
                QuestionText = "In the equation 438 X ( ) = 540930, 1255 is the number that fills the ().",
                Answer = false,
                AnswerText = "In the equation 438 X ( ) = 540930, 1235 is the number that fills the ().",
                Category = "Math"
            };
            questions.Add(q614);

            OXQuizMetadata q615 = new OXQuizMetadata()
            {
                Id = 615,
                QuestionText = "In the equation 438 X ( ) = 540930, 1265 is the number that fills the ().",
                Answer = false,
                AnswerText = "In the equation 438 X ( ) = 540930, 1235 is the number that fills the ().",
                Category = "Math"
            };
            questions.Add(q615);

            OXQuizMetadata q616 = new OXQuizMetadata()
            {
                Id = 616,
                QuestionText = "In the equation 438 X ( ) = 540930, 1275 is the number that fills the ().",
                Answer = false,
                AnswerText = "In the equation 438 X ( ) = 540930, 1235 is the number that fills the ().",
                Category = "Math"
            };
            questions.Add(q616);

            OXQuizMetadata q617 = new OXQuizMetadata()
            {
                Id = 617,
                QuestionText = "In the equation 438 X ( ) = 540930, 1285 is the number that fills the ().",
                Answer = false,
                AnswerText = "In the equation 438 X ( ) = 540930, 1235 is the number that fills the ().",
                Category = "Math"
            };
            questions.Add(q617);

            OXQuizMetadata q618 = new OXQuizMetadata()
            {
                Id = 618,
                QuestionText = "In the equation 439 X ( ) = 540930, 1325 is the number that fills the ().",
                Answer = false,
                AnswerText = "In the equation 439 X ( ) = 540930, 1235 is the number that fills the ().",
                Category = "Math"
            };
            questions.Add(q618);

            OXQuizMetadata q619 = new OXQuizMetadata()
            {
                Id = 619,
                QuestionText = "In the equation 735 X ( ) = 65415, 87 is the number that fills the ().",
                Answer = false,
                AnswerText = "In the equation 735 X ( ) = 65415, 89 is the number that fills the ().",
                Category = "Math"
            };
            questions.Add(q619);

            OXQuizMetadata q620 = new OXQuizMetadata()
            {
                Id = 620,
                QuestionText = "In the flag of the United States, \"The Stars and Stripes,\" the stars represents the number of the states.",
                Answer = true,
                AnswerText = "In the American flag, \"The Stars and Stripes,\" the stars represents the number of the states.",
                Category = "General Knowledge"
            };
            questions.Add(q620);

            OXQuizMetadata q621 = new OXQuizMetadata()
            {
                Id = 621,
                QuestionText = "In the game \"Super Mario Brothers,\" the lead character Mario is a plumber.",
                Answer = true,
                AnswerText = "Mario is a plumber.",
                Category = "Culture/Art"
            };
            questions.Add(q621);

            OXQuizMetadata q622 = new OXQuizMetadata()
            {
                Id = 622,
                QuestionText = "In the movie \"Die Hard,\" Bruce Willis stars as Detective McClane.",
                Answer = true,
                AnswerText = "The role became what he is best known for.",
                Category = "Culture/Art"
            };
            questions.Add(q622);

            OXQuizMetadata q623 = new OXQuizMetadata()
            {
                Id = 623,
                QuestionText = "In the movie \"ET,\" ET and Elliott fly in a light airplane.",
                Answer = false,
                AnswerText = "They flew on a bicycle.",
                Category = "Culture/Art"
            };
            questions.Add(q623);

            OXQuizMetadata q624 = new OXQuizMetadata()
            {
                Id = 624,
                QuestionText = "In the movie \"Independence Day\" aliens invade Earth.",
                Answer = true,
                AnswerText = "The movie describes an epic rise of humans against the alien invasion.",
                Category = "Culture/Art"
            };
            questions.Add(q624);

            OXQuizMetadata q625 = new OXQuizMetadata()
            {
                Id = 625,
                QuestionText = "In the movie \"Indiana Jones,\" Dr. Jones's favorite weapon is a lightsaber.",
                Answer = false,
                AnswerText = "He uses a whip.",
                Category = "Culture/Art"
            };
            questions.Add(q625);

            OXQuizMetadata q626 = new OXQuizMetadata()
            {
                Id = 626,
                QuestionText = "In the movie \"Jurassic Park,\" the scientists extracted dinosaur DNA from reptile excretions.",
                Answer = false,
                AnswerText = "The scientists extracted dinosaur DNA from a mosquito.",
                Category = "Culture/Art"
            };
            questions.Add(q626);

            OXQuizMetadata q627 = new OXQuizMetadata()
            {
                Id = 627,
                QuestionText = "In the musical \"Cats,\" there's an aria called \"Memory.\"",
                Answer = true,
                AnswerText = "\"Memory\" is the most famous aria of the \"Cats\" arias.",
                Category = "Culture/Art"
            };
            questions.Add(q627);

            OXQuizMetadata q628 = new OXQuizMetadata()
            {
                Id = 628,
                QuestionText = "In the novel Don Quixote, what the main character believes to be a giant and duels is a clock tower.",
                Answer = false,
                AnswerText = "He fights a windmill.",
                Category = "Culture/Art"
            };
            questions.Add(q628);

            OXQuizMetadata q629 = new OXQuizMetadata()
            {
                Id = 629,
                QuestionText = "In the Olympics, the last country to enter the opening ceremony is Greece, the country where Olympics was founded.",
                Answer = false,
                AnswerText = "In the Olympics, the last country to enter the opening ceremony is the host country.",
                Category = "Culture/Art"
            };
            questions.Add(q629);

            OXQuizMetadata q630 = new OXQuizMetadata()
            {
                Id = 630,
                QuestionText = "In the Olympics, the last country to enter the opening ceremony is the host country.",
                Answer = true,
                AnswerText = "In the Olympics, the last country to enter the opening ceremony is the host country.",
                Category = "Culture/Art"
            };
            questions.Add(q630);

            OXQuizMetadata q631 = new OXQuizMetadata()
            {
                Id = 631,
                QuestionText = "In the Punic Wars, Rome lost to Carthage.",
                Answer = false,
                AnswerText = "Rome won the Punic Wars.",
                Category = "Culture/Art"
            };
            questions.Add(q631);

            OXQuizMetadata q632 = new OXQuizMetadata()
            {
                Id = 632,
                QuestionText = "In the United States, soldiers are drafted using luck of the draw.",
                Answer = false,
                AnswerText = "Thailand's military uses conscription to recruit soldiers. In Thailand, however, soldiers are drafted using the luck of the draw.",
                Category = "Society"
            };
            questions.Add(q632);

            OXQuizMetadata q633 = new OXQuizMetadata()
            {
                Id = 633,
                QuestionText = "In the West, the West End, along with New York's Broadway, is an area known for top-notch theater performances.",
                Answer = true,
                AnswerText = "Many theaters are built in Victorian form.",
                Category = "Culture/Art"
            };
            questions.Add(q633);

            OXQuizMetadata q634 = new OXQuizMetadata()
            {
                Id = 634,
                QuestionText = "In theater, convention is the unspoken agreement made between the audience and the actors.",
                Answer = true,
                AnswerText = "In theater, convention is the unspoken agreement made between the audience and the actors.",
                Category = "Culture/Art"
            };
            questions.Add(q634);

            OXQuizMetadata q635 = new OXQuizMetadata()
            {
                Id = 635,
                QuestionText = "In track and field, anchor leg refers to the first position in the relay race.",
                Answer = false,
                AnswerText = "In track and field, anchor leg refers to the last position in a relay race.",
                Category = "Culture/Art"
            };
            questions.Add(q635);

            OXQuizMetadata q636 = new OXQuizMetadata()
            {
                Id = 636,
                QuestionText = "In volleyball, the position specialized in defensive plays is called the sweeper.",
                Answer = false,
                AnswerText = "The position is called libero.",
                Category = "Culture/Art"
            };
            questions.Add(q636);

            OXQuizMetadata q637 = new OXQuizMetadata()
            {
                Id = 637,
                QuestionText = "In weight lifting, the narrow grip means that the hands are placed on a barbell or other type of bar a grip wider than shoulder width.",
                Answer = false,
                AnswerText = "The wide grip means that the hands are placed on a barbell or other type of bar a grip wider than shoulder width.",
                Category = "General Knowledge"
            };
            questions.Add(q637);

            OXQuizMetadata q638 = new OXQuizMetadata()
            {
                Id = 638,
                QuestionText = "Indians used the sexagesimal (base 60) as the standard system of measurement.",
                Answer = false,
                AnswerText = "Mesopotamians used sexagesimal (base 60) as the standardized system of measurement.",
                Category = "Society"
            };
            questions.Add(q638);

            OXQuizMetadata q639 = new OXQuizMetadata()
            {
                Id = 639,
                QuestionText = "Infants have more bones than adults.",
                Answer = true,
                AnswerText = "Infants have about 360 bones while adults have about 360.",
                Category = "Science"
            };
            questions.Add(q639);

            OXQuizMetadata q640 = new OXQuizMetadata()
            {
                Id = 640,
                QuestionText = "IOC is the shorthand for \"International Olympic Committee.\"",
                Answer = true,
                AnswerText = "IOC is the shorthand for \"International Olympic Committee.\"",
                Category = "General Knowledge"
            };
            questions.Add(q640);

            OXQuizMetadata q641 = new OXQuizMetadata()
            {
                Id = 641,
                QuestionText = "Ionic is the oldest of the three classical architectural orders of Greece, Doric, Ionic, and Corinthian.",
                Answer = false,
                AnswerText = "Doric is the oldest of the three classical architectural orders of Greece, Doric, Ionic, and Corinthian.",
                Category = "Culture/Art"
            };
            questions.Add(q641);

            OXQuizMetadata q642 = new OXQuizMetadata()
            {
                Id = 642,
                QuestionText = "Israel's capital is Jerusalem.",
                Answer = true,
                AnswerText = "Israel's capital is Jerusalem.",
                Category = "General Knowledge"
            };
            questions.Add(q642);

            OXQuizMetadata q643 = new OXQuizMetadata()
            {
                Id = 643,
                QuestionText = "It always rains in Chronoff Train Station.",
                Answer = false,
                AnswerText = "It snows in Chronoff Train Station.",
                Category = "MapleStory 2"
            };
            questions.Add(q643);

            OXQuizMetadata q644 = new OXQuizMetadata()
            {
                Id = 644,
                QuestionText = "It was Emperor Hadrian who built the Parthenon.",
                Answer = false,
                AnswerText = "Marcus Vipsanius Agrippa started building the Pantheon. Publius Aelius Hadrianus remodeled it.",
                Category = "General Knowledge"
            };
            questions.Add(q644);

            OXQuizMetadata q645 = new OXQuizMetadata()
            {
                Id = 645,
                QuestionText = "Italian scientist Alessandro Volta invented the world's first electric battery.",
                Answer = true,
                AnswerText = "In 1800, Italian scientist Alessandro Volta invented the world's first electric battery.",
                Category = "Science"
            };
            questions.Add(q645);

            OXQuizMetadata q646 = new OXQuizMetadata()
            {
                Id = 646,
                QuestionText = "Italy won the 1982 World Cup in Spain.",
                Answer = true,
                AnswerText = "Italy won the 1982 World Cup Spain.",
                Category = "Culture/Art"
            };
            questions.Add(q646);

            OXQuizMetadata q647 = new OXQuizMetadata()
            {
                Id = 647,
                QuestionText = "Italy won the 1990 World Cup in Italy.",
                Answer = false,
                AnswerText = "West Germany won the 1990 World Cup Italy.",
                Category = "Culture/Art"
            };
            questions.Add(q647);

            OXQuizMetadata q648 = new OXQuizMetadata()
            {
                Id = 648,
                QuestionText = "Items cannot be hidden from display.",
                Answer = false,
                AnswerText = "The Transparency Badge can be used to hide items from display.",
                Category = "MapleStory 2"
            };
            questions.Add(q648);

            OXQuizMetadata q649 = new OXQuizMetadata()
            {
                Id = 649,
                QuestionText = "Items purchased from the Black Market arrive in the Mailbox in 48 hours.",
                Answer = false,
                AnswerText = "Items purchased from the Black Market immediately arrive in the Mailbox.",
                Category = "MapleStory 2"
            };
            questions.Add(q649);

            OXQuizMetadata q650 = new OXQuizMetadata()
            {
                Id = 650,
                QuestionText = "Items stored with Goldus Bank are shared among your characters on the same account.",
                Answer = true,
                AnswerText = "Items and mesos stored with Goldus Bank are shared among your characters on the same account.",
                Category = "MapleStory 2"
            };
            questions.Add(q650);

            OXQuizMetadata q651 = new OXQuizMetadata()
            {
                Id = 651,
                QuestionText = "Jabulani was the official ball for the 2014 Brazil World Cup.",
                Answer = false,
                AnswerText = "Brazuca was the official ball for the 2014 Brazil World Cup.",
                Category = "Culture/Art"
            };
            questions.Add(q651);

            OXQuizMetadata q652 = new OXQuizMetadata()
            {
                Id = 652,
                QuestionText = "Jan Yeong-Sil developed the light bulb and phonograph.",
                Answer = false,
                AnswerText = "Thomas Edison developed the light bulb and phonograph.",
                Category = "Science"
            };
            questions.Add(q652);

            OXQuizMetadata q653 = new OXQuizMetadata()
            {
                Id = 653,
                QuestionText = "Jane Austen, famous for her novel \"Pride and Prejudice,\" left the famous quote \"Love is merely a madness.\"",
                Answer = false,
                AnswerText = "\"Love is merely a madness\" is a quote by William Shakespeare.",
                Category = "General Knowledge"
            };
            questions.Add(q653);

            OXQuizMetadata q654 = new OXQuizMetadata()
            {
                Id = 654,
                QuestionText = "Jang Yeong-Sil developed the light bulb and phonograph.",
                Answer = false,
                AnswerText = "Thomas Edison developed the light bulb and phonograph.",
                Category = "General Knowledge"
            };
            questions.Add(q654);

            OXQuizMetadata q655 = new OXQuizMetadata()
            {
                Id = 655,
                QuestionText = "Japan won the second spot in the World Baseball Softball Confederation (WSBC) Premiere 12, which was held in 2015.",
                Answer = false,
                AnswerText = "South Korea won first place, the United States won second, and Japan won third.",
                Category = "General Knowledge"
            };
            questions.Add(q655);

            OXQuizMetadata q656 = new OXQuizMetadata()
            {
                Id = 656,
                QuestionText = "Japan won the third spot in the World Baseball Softball Confederation (WSBC) Premiere 12, which was held in 2015.",
                Answer = true,
                AnswerText = "South Korea won first place, the United States won second, and Japan won third.",
                Category = "General Knowledge"
            };
            questions.Add(q656);

            OXQuizMetadata q657 = new OXQuizMetadata()
            {
                Id = 657,
                QuestionText = "Japan won the World Baseball Softball Confederation (WSBC) Premiere 12, which was held in 2015.",
                Answer = false,
                AnswerText = "Japan, the host of the World Baseball Softball Confederation (WSBC) Premiere 12, won third place.",
                Category = "General Knowledge"
            };
            questions.Add(q657);

            OXQuizMetadata q658 = new OXQuizMetadata()
            {
                Id = 658,
                QuestionText = "Jean Sibelius is a Russian composer.",
                Answer = false,
                AnswerText = "Jean Sibelius is from Finland.",
                Category = "Culture/Art"
            };
            questions.Add(q658);

            OXQuizMetadata q659 = new OXQuizMetadata()
            {
                Id = 659,
                QuestionText = "Jekyll & Hide is a musical whose plot is set in London.",
                Answer = true,
                AnswerText = "Jekyll & Hide is a musical whose plot is set in London.",
                Category = "Culture/Art"
            };
            questions.Add(q659);

            OXQuizMetadata q660 = new OXQuizMetadata()
            {
                Id = 660,
                QuestionText = "Jeremy Bentham is the philosopher who suggested utilitarianism, which upholds maximization of overall happiness as a value.",
                Answer = true,
                AnswerText = "Jeremy Bentham asserted that utilitarianism, which upholds maximization of overall happiness as moral, should be the legal norm.",
                Category = "History"
            };
            questions.Add(q660);

            OXQuizMetadata q661 = new OXQuizMetadata()
            {
                Id = 661,
                QuestionText = "Johann Electric Bach is a family member of Johann Sebastian Bach.",
                Answer = false,
                AnswerText = "Johann Electric Bach and Johann Sebastian Bach are unrelated.",
                Category = "Culture/Art"
            };
            questions.Add(q661);

            OXQuizMetadata q662 = new OXQuizMetadata()
            {
                Id = 662,
                QuestionText = "Johann Sebastian Bach's \"The Goldberg Variation\" has catalog number of BWV 987.",
                Answer = false,
                AnswerText = "\"The Goldberg Variations\" is a work written for harpsichord by Johann Sebastian Bach, consisting of an aria and a set of 30 variations. Its catalog number is BWV 988.",
                Category = "Culture/Art"
            };
            questions.Add(q662);

            OXQuizMetadata q663 = new OXQuizMetadata()
            {
                Id = 663,
                QuestionText = "Johann Sebastian Bach's BWV 244, \"St Matthew Passion,\" first premiered on the Saturday of April 15, 1729.",
                Answer = false,
                AnswerText = "Johann Sebastion Bach's BWV 244, \"St Matthew Passion,\" first premiered on Good Friday of April 15, 1729.",
                Category = "Culture/Art"
            };
            questions.Add(q663);

            OXQuizMetadata q664 = new OXQuizMetadata()
            {
                Id = 664,
                QuestionText = "Johannesburg is one of the capitals of the Republic of South Africa.",
                Answer = false,
                AnswerText = "The Republic of South Africa has three capitals: Pretoria, Bloemfontein, and Cape Town.",
                Category = "General Knowledge"
            };
            questions.Add(q664);

            OXQuizMetadata q665 = new OXQuizMetadata()
            {
                Id = 665,
                QuestionText = "Johannesburg is the capital of the Republic of South Africa.",
                Answer = false,
                AnswerText = "Pretoria is the capital of the Republic of South Africa.",
                Category = "General Knowledge"
            };
            questions.Add(q665);

            OXQuizMetadata q666 = new OXQuizMetadata()
            {
                Id = 666,
                QuestionText = "John Milton wrote the novel Utopia, which greatly influenced 18th-century British satires.",
                Answer = false,
                AnswerText = "Thomas More wrote the novel Utopia.",
                Category = "Culture/Art"
            };
            questions.Add(q666);

            OXQuizMetadata q667 = new OXQuizMetadata()
            {
                Id = 667,
                QuestionText = "Junk food refers to food that is high in calories but low in nutritional value.",
                Answer = true,
                AnswerText = "Junk food refers to food that is high in calories but low in nutritional value.",
                Category = "General Knowledge"
            };
            questions.Add(q667);

            OXQuizMetadata q668 = new OXQuizMetadata()
            {
                Id = 668,
                QuestionText = "Just like humans, monkeys have blood types.",
                Answer = true,
                AnswerText = "Just like humans, monkeys do have blood types. RH, a term used to classify human's blood, comes from rhesus, the name of a species of red-furred monkey.",
                Category = "General Knowledge"
            };
            questions.Add(q668);

            OXQuizMetadata q669 = new OXQuizMetadata()
            {
                Id = 669,
                QuestionText = "Kart Rider is a racing game.",
                Answer = true,
                AnswerText = "Kart Rider is a car racing game.",
                Category = "General Knowledge"
            };
            questions.Add(q669);

            OXQuizMetadata q670 = new OXQuizMetadata()
            {
                Id = 670,
                QuestionText = "Katvan's Gloves can be found in the Root of Darkness.",
                Answer = false,
                AnswerText = "Katvan's Gloves do not exist.",
                Category = "MapleStory 2"
            };
            questions.Add(q670);

            OXQuizMetadata q671 = new OXQuizMetadata()
            {
                Id = 671,
                QuestionText = "King Tabo appears in the Andrea Barony.",
                Answer = false,
                AnswerText = "The elite monster in the Andrea Barony is Varaha.",
                Category = "MapleStory 2"
            };
            questions.Add(q671);

            OXQuizMetadata q672 = new OXQuizMetadata()
            {
                Id = 672,
                QuestionText = "Known for its local squids, Ulleung-do, an island in Korea, is part of Gyeongsangnam-do district.",
                Answer = false,
                AnswerText = "Ulleung-do is part of the Gyeongsangbuk-do district.",
                Category = "General Knowledge"
            };
            questions.Add(q672);

            OXQuizMetadata q673 = new OXQuizMetadata()
            {
                Id = 673,
                QuestionText = "Korean Orcas and American Orcas can communicate with one another.",
                Answer = false,
                AnswerText = "They can't because whales have regional dialects.",
                Category = "General Knowledge"
            };
            questions.Add(q673);

            OXQuizMetadata q674 = new OXQuizMetadata()
            {
                Id = 674,
                QuestionText = "L. L. Zamenhof invented the international auxiliary language, Esperanto.",
                Answer = true,
                AnswerText = "Believing that the conflicts among Poland, Germany, and Jews lie in language, L. L. Zamenhof developed Esperanto in the 19th century.",
                Category = "General Knowledge"
            };
            questions.Add(q674);

            OXQuizMetadata q675 = new OXQuizMetadata()
            {
                Id = 675,
                QuestionText = "Lack of calcium causes anemia.",
                Answer = false,
                AnswerText = "Lack of iron causes anemia.",
                Category = "Science"
            };
            questions.Add(q675);

            OXQuizMetadata q676 = new OXQuizMetadata()
            {
                Id = 676,
                QuestionText = "Lame duck refers to the diminishing power of an elected official who is reaching the end of one's term.",
                Answer = true,
                AnswerText = "Lame duck refers to the diminishing power of an elected official who is reaching the end of one's term.",
                Category = "General Knowledge"
            };
            questions.Add(q676);

            OXQuizMetadata q677 = new OXQuizMetadata()
            {
                Id = 677,
                QuestionText = "Largo is a term used to describe dynamics in music.",
                Answer = false,
                AnswerText = "Largo is a term used to describe tempo in music.",
                Category = "Culture/Art"
            };
            questions.Add(q677);

            OXQuizMetadata q678 = new OXQuizMetadata()
            {
                Id = 678,
                QuestionText = "Largo is a term used to describe tempo in music.",
                Answer = true,
                AnswerText = "Largo is a term used to describe tempo in music.",
                Category = "Culture/Art"
            };
            questions.Add(q678);

            OXQuizMetadata q679 = new OXQuizMetadata()
            {
                Id = 679,
                QuestionText = "Lava Eye has 3 eyes.",
                Answer = false,
                AnswerText = "Lava Eye has 1 eye.",
                Category = "MapleStory 2"
            };
            questions.Add(q679);

            OXQuizMetadata q680 = new OXQuizMetadata()
            {
                Id = 680,
                QuestionText = "Lennon has a scar across his cheek.",
                Answer = true,
                AnswerText = "Lennon has a scar across his cheek.",
                Category = "MapleStory 2"
            };
            questions.Add(q680);

            OXQuizMetadata q681 = new OXQuizMetadata()
            {
                Id = 681,
                QuestionText = "Lernos has 5 heads.",
                Answer = false,
                AnswerText = "Lernos has 3 heads.",
                Category = "MapleStory 2"
            };
            questions.Add(q681);

            OXQuizMetadata q682 = new OXQuizMetadata()
            {
                Id = 682,
                QuestionText = "Lerta in the Frostember Void is a Divine monster.",
                Answer = false,
                AnswerText = "Lerta is a Beast monster.",
                Category = "MapleStory 2"
            };
            questions.Add(q682);

            OXQuizMetadata q683 = new OXQuizMetadata()
            {
                Id = 683,
                QuestionText = "Libya's capital is Tripoli.",
                Answer = true,
                AnswerText = "Libya's capital is Tripoli.",
                Category = "General Knowledge"
            };
            questions.Add(q683);

            OXQuizMetadata q684 = new OXQuizMetadata()
            {
                Id = 684,
                QuestionText = "Light travels 1km/sec in vaccum.",
                Answer = false,
                AnswerText = "The speed of light in a vacuum is 299,792,458 m/s.",
                Category = "Science"
            };
            questions.Add(q684);

            OXQuizMetadata q685 = new OXQuizMetadata()
            {
                Id = 685,
                QuestionText = "Link is the main character in the video game \"Zelda\".",
                Answer = true,
                AnswerText = "Link is the main character in the video game \"Zelda\".",
                Category = "General Knowledge"
            };
            questions.Add(q685);

            OXQuizMetadata q686 = new OXQuizMetadata()
            {
                Id = 686,
                QuestionText = "Liquid's boiling point is affected by external pressure.",
                Answer = true,
                AnswerText = "The higher the external pressure, the higher the boiling point. Likewise, the lower the external pressure, the lower the boiling point.",
                Category = "Science"
            };
            questions.Add(q686);

            OXQuizMetadata q687 = new OXQuizMetadata()
            {
                Id = 687,
                QuestionText = "Liquid's boiling point isn't affected by external pressure.",
                Answer = false,
                AnswerText = "The higher the external pressure, the higher the boiling point. Likewise, the lower the external pressure, the lower the boiling point.",
                Category = "Science"
            };
            questions.Add(q687);

            OXQuizMetadata q688 = new OXQuizMetadata()
            {
                Id = 688,
                QuestionText = "Lisa Simpson plays the clarinet in \"The Simpsons.\"",
                Answer = false,
                AnswerText = "Lisa Simpson plays the saxophone in \"The Simpsons.\" Her inspiration was Bleeding Gums Murphy.",
                Category = "Culture/Art"
            };
            questions.Add(q688);

            OXQuizMetadata q689 = new OXQuizMetadata()
            {
                Id = 689,
                QuestionText = "Lithographs use the immiscibility of oil and water.",
                Answer = true,
                AnswerText = "Lithographs use the immiscibility of oil and water.",
                Category = "Culture/Art"
            };
            questions.Add(q689);

            OXQuizMetadata q690 = new OXQuizMetadata()
            {
                Id = 690,
                QuestionText = "Lo and Moomoo are Divine monsters who appear at the Baum Tree.",
                Answer = true,
                AnswerText = "Lo and Moomoo are Divine monsters found at the Baum Tree.",
                Category = "MapleStory 2"
            };
            questions.Add(q690);

            OXQuizMetadata q691 = new OXQuizMetadata()
            {
                Id = 691,
                QuestionText = "Lo and Moomoo are Divine monsters who appear at the Baun Tree.",
                Answer = false,
                AnswerText = "Lo and Moomoo are Divine monsters found at the Baum Tree.",
                Category = "MapleStory 2"
            };
            questions.Add(q691);

            OXQuizMetadata q692 = new OXQuizMetadata()
            {
                Id = 692,
                QuestionText = "Lotions are effective at preventing static electricity.",
                Answer = true,
                AnswerText = "Lotions are effective at preventing static electricity.",
                Category = "Science"
            };
            questions.Add(q692);

            OXQuizMetadata q693 = new OXQuizMetadata()
            {
                Id = 693,
                QuestionText = "Lotions are ineffective at preventing static electricity.",
                Answer = false,
                AnswerText = "Lotions are effective at preventing static electricity.",
                Category = "Science"
            };
            questions.Add(q693);

            OXQuizMetadata q694 = new OXQuizMetadata()
            {
                Id = 694,
                QuestionText = "Lovecraft, an American author who is known for his influential works of romance fiction, is the pseudonym of Howard Philip.",
                Answer = false,
                AnswerText = "H. P. Lovecraft is famous for creating the Cthulhu Mythos.",
                Category = "Culture/Art"
            };
            questions.Add(q694);

            OXQuizMetadata q695 = new OXQuizMetadata()
            {
                Id = 695,
                QuestionText = "Low-calorie ramen is a thing that exists.",
                Answer = true,
                AnswerText = "The noodles of low-calorie ramen's aren't cooked in oil.",
                Category = "General Knowledge"
            };
            questions.Add(q695);

            OXQuizMetadata q696 = new OXQuizMetadata()
            {
                Id = 696,
                QuestionText = "LTV stands for Loan to Value Ratio.",
                Answer = true,
                AnswerText = "LTV stands for Loan to Value Ratio.",
                Category = "General Knowledge"
            };
            questions.Add(q696);

            OXQuizMetadata q697 = new OXQuizMetadata()
            {
                Id = 697,
                QuestionText = "LTV stands for the financial term Debt to Income Ratio.",
                Answer = false,
                AnswerText = "DTI stands for Debt to Income Ratio.",
                Category = "General Knowledge"
            };
            questions.Add(q697);

            OXQuizMetadata q698 = new OXQuizMetadata()
            {
                Id = 698,
                QuestionText = "Lupin was the first composer to use the word Ballade in the name of an instrumental.",
                Answer = false,
                AnswerText = "Chopin was the first composer to use the word Ballade in the name of an instrumental.",
                Category = "Culture/Art"
            };
            questions.Add(q698);

            OXQuizMetadata q699 = new OXQuizMetadata()
            {
                Id = 699,
                QuestionText = "Luwak coffee is made out of a cat's feces.",
                Answer = true,
                AnswerText = "The part-digested coffee cherries eaten and defecated by a cat are called kopi luwak, used in luwak coffee.",
                Category = "General Knowledge"
            };
            questions.Add(q699);

            OXQuizMetadata q700 = new OXQuizMetadata()
            {
                Id = 700,
                QuestionText = "Luxembourg's capital is Luxembourg city.",
                Answer = true,
                AnswerText = "Luxembourg's capital is Luxembourg City.",
                Category = "General Knowledge"
            };
            questions.Add(q700);

            OXQuizMetadata q701 = new OXQuizMetadata()
            {
                Id = 701,
                QuestionText = "Madama Butterfly is by Puccini.",
                Answer = true,
                AnswerText = "It's one of his three masterpieces: Madama Butterfly, La Bohème, and Tosca.",
                Category = "Culture/Art"
            };
            questions.Add(q701);

            OXQuizMetadata q702 = new OXQuizMetadata()
            {
                Id = 702,
                QuestionText = "Madonette appears in Flora Avenue.",
                Answer = true,
                AnswerText = "Madonette appears in Flora Avenue.",
                Category = "MapleStory 2"
            };
            questions.Add(q702);

            OXQuizMetadata q703 = new OXQuizMetadata()
            {
                Id = 703,
                QuestionText = "Magatma Gandhi from India is the first Asian person to win a Nobel Prize.",
                Answer = false,
                AnswerText = "Rabindranath Tagore, an Indian, is the first Asian person to win a Nobel Prize.",
                Category = "Culture/Art"
            };
            questions.Add(q703);

            OXQuizMetadata q704 = new OXQuizMetadata()
            {
                Id = 704,
                QuestionText = "Male lions are the main providers of a lion pride.",
                Answer = false,
                AnswerText = "Female lions are the primary providers of a lion pack.",
                Category = "General Knowledge"
            };
            questions.Add(q704);

            OXQuizMetadata q705 = new OXQuizMetadata()
            {
                Id = 705,
                QuestionText = "Male mantises, after mating, eat their female partners.",
                Answer = false,
                AnswerText = "Female mantises, prey on their partners to gain the energy required for the hatching process.",
                Category = "Science"
            };
            questions.Add(q705);

            OXQuizMetadata q706 = new OXQuizMetadata()
            {
                Id = 706,
                QuestionText = "Mambrina in Katramus steals HP from her targets.",
                Answer = true,
                AnswerText = "Mambrina in Katramus steals HP from her targets.",
                Category = "MapleStory 2"
            };
            questions.Add(q706);

            OXQuizMetadata q707 = new OXQuizMetadata()
            {
                Id = 707,
                QuestionText = "Mandolin is a type of keyboard instrument.",
                Answer = false,
                AnswerText = "Mandolin is a stringed instrument, which is similar to the lute.",
                Category = "Culture/Art"
            };
            questions.Add(q707);

            OXQuizMetadata q708 = new OXQuizMetadata()
            {
                Id = 708,
                QuestionText = "Mannequins, when set up inside your house, enable you to change to any clothes, even those for the opposite gender.",
                Answer = false,
                AnswerText = "You can only change to the clothes for your gender.",
                Category = "MapleStory 2"
            };
            questions.Add(q708);

            OXQuizMetadata q709 = new OXQuizMetadata()
            {
                Id = 709,
                QuestionText = "Mano is a married male snail appearing in Greenia Falls.",
                Answer = true,
                AnswerText = "Mano's wife is Moya, who can be found in Fungeeburg Stump.",
                Category = "MapleStory 2"
            };
            questions.Add(q709);

            OXQuizMetadata q710 = new OXQuizMetadata()
            {
                Id = 710,
                QuestionText = "Many computer programs including games and worc processors are classified as hardware.",
                Answer = false,
                AnswerText = "They are called software, not hardware.",
                Category = "IT"
            };
            questions.Add(q710);

            OXQuizMetadata q711 = new OXQuizMetadata()
            {
                Id = 711,
                QuestionText = "Maplestory 2 is computer hardware.",
                Answer = false,
                AnswerText = "Maplestory 2 is computer software. Computer hardware is the collection of physical parts of a computer system.",
                Category = "MapleStory 2"
            };
            questions.Add(q711);

            OXQuizMetadata q712 = new OXQuizMetadata()
            {
                Id = 712,
                QuestionText = "MapleStory is a mini-game.",
                Answer = false,
                AnswerText = "MapleStory is an online game with a continuing storyline. It's not a mini-game.",
                Category = "MapleStory"
            };
            questions.Add(q712);

            OXQuizMetadata q713 = new OXQuizMetadata()
            {
                Id = 713,
                QuestionText = "Marathon is the name of the runner who won an endurance running in ancient Olympics.",
                Answer = false,
                AnswerText = "Marathon is the name of an expansive open grassland near Athens.",
                Category = "History"
            };
            questions.Add(q713);

            OXQuizMetadata q714 = new OXQuizMetadata()
            {
                Id = 714,
                QuestionText = "Mariah Carey is the heroine of and sang the theme song of the movie \"The Bodyguard.\"",
                Answer = false,
                AnswerText = "It was Whitney Houston.",
                Category = "Culture/Art"
            };
            questions.Add(q714);

            OXQuizMetadata q715 = new OXQuizMetadata()
            {
                Id = 715,
                QuestionText = "Marie Antoinette is the spouse of Louis the Fourteenth.",
                Answer = false,
                AnswerText = "Marie Antoinette is the spouse of Louis the Sixteenth.",
                Category = "History"
            };
            questions.Add(q715);

            OXQuizMetadata q716 = new OXQuizMetadata()
            {
                Id = 716,
                QuestionText = "Marie Antoinette, the spouse of Louis the Sixteenth, is the youngest daughter of English queen Maria Theresia.",
                Answer = false,
                AnswerText = "Marie Antoinette, the spouse of Louis the Sixteenth, is the youngest daughter of Austrian queen Maria Theresia.",
                Category = "History"
            };
            questions.Add(q716);

            OXQuizMetadata q717 = new OXQuizMetadata()
            {
                Id = 717,
                QuestionText = "Marinda rights are what mandate the police to warn the suspects of their right to remain silent.",
                Answer = false,
                AnswerText = "Before a police officer arrests and interrogates the suspect, he or she must read the Miranda rights.",
                Category = "Society"
            };
            questions.Add(q717);

            OXQuizMetadata q718 = new OXQuizMetadata()
            {
                Id = 718,
                QuestionText = "Mario's pet dinosaur is named Bowser.",
                Answer = false,
                AnswerText = "Mario's pet dinosaur is named Yoshi.",
                Category = "Culture/Art"
            };
            questions.Add(q718);

            OXQuizMetadata q719 = new OXQuizMetadata()
            {
                Id = 719,
                QuestionText = "Marksman refers to a person who's skilled in swordsmanship.",
                Answer = false,
                AnswerText = "Marksman refers to a person who's skilled at shooting.",
                Category = "Culture/Art"
            };
            questions.Add(q719);

            OXQuizMetadata q720 = new OXQuizMetadata()
            {
                Id = 720,
                QuestionText = "Maurice Ravel is a Russian composer.",
                Answer = false,
                AnswerText = "Maurice Ravel is French.",
                Category = "Culture/Art"
            };
            questions.Add(q720);

            OXQuizMetadata q721 = new OXQuizMetadata()
            {
                Id = 721,
                QuestionText = "Maurice Ravel's \"Bolero\" is an opera.",
                Answer = false,
                AnswerText = "Maurice Ravel's \"Bolero\" is a ballet.",
                Category = "Culture/Art"
            };
            questions.Add(q721);

            OXQuizMetadata q722 = new OXQuizMetadata()
            {
                Id = 722,
                QuestionText = "Mazurka is a Polish folk song with 2/4 beats.",
                Answer = false,
                AnswerText = "Mazurka is Polish folk dance song with 3/4 beats.",
                Category = "Culture/Art"
            };
            questions.Add(q722);

            OXQuizMetadata q723 = new OXQuizMetadata()
            {
                Id = 723,
                QuestionText = "Mazurka is a Polish folk song with a 3/4 beat.",
                Answer = true,
                AnswerText = "Mazurka is a Polish folk dance song in triple meter, usually at a lively tempo.",
                Category = "Culture/Art"
            };
            questions.Add(q723);

            OXQuizMetadata q724 = new OXQuizMetadata()
            {
                Id = 724,
                QuestionText = "Mechanical is one of the three forms of power as described in Alvin Toffler's Powershift.",
                Answer = false,
                AnswerText = "The three forms of power are violence, wealth, and knowledge.",
                Category = "Culture/Art"
            };
            questions.Add(q724);

            OXQuizMetadata q725 = new OXQuizMetadata()
            {
                Id = 725,
                QuestionText = "Mel Gibson is the director and lead actor of \"Million Dollar Baby.\"",
                Answer = false,
                AnswerText = "Clint Eastwood won the Academy's best director award for this movie.",
                Category = "Culture/Art"
            };
            questions.Add(q725);

            OXQuizMetadata q726 = new OXQuizMetadata()
            {
                Id = 726,
                QuestionText = "Melamin is the skin pigment responsible for tanning and freckles.",
                Answer = false,
                AnswerText = "The skin pigment's name is melanin.",
                Category = "General Knowledge"
            };
            questions.Add(q726);

            OXQuizMetadata q727 = new OXQuizMetadata()
            {
                Id = 727,
                QuestionText = "Melvin Toffler is a futurist who wrote \"Future Shock,\" \"The Third Wave,\" and \"Powershift.\"",
                Answer = false,
                AnswerText = "Alvin Toffler is a futurist who wrote \"Future Shock,\" \"The Third Wave,\" and \"Powershift.\"",
                Category = "Society"
            };
            questions.Add(q727);

            OXQuizMetadata q728 = new OXQuizMetadata()
            {
                Id = 728,
                QuestionText = "Memory loss due to aging occurs because old age decreases intelligence.",
                Answer = false,
                AnswerText = "Old age decreases memory capacity but doesn't necessarily decrease intelligence.",
                Category = "Science"
            };
            questions.Add(q728);

            OXQuizMetadata q729 = new OXQuizMetadata()
            {
                Id = 729,
                QuestionText = "Men don't suffer from osteoporosis.",
                Answer = false,
                AnswerText = "Even men, without proper physical activity, lose bone density.",
                Category = "General Knowledge"
            };
            questions.Add(q729);

            OXQuizMetadata q730 = new OXQuizMetadata()
            {
                Id = 730,
                QuestionText = "Meso Eye Drop increases your Mesos pickup range.",
                Answer = false,
                AnswerText = "Meso Eye Drop increases your Meso drop rate.",
                Category = "MapleStory 2"
            };
            questions.Add(q730);

            OXQuizMetadata q731 = new OXQuizMetadata()
            {
                Id = 731,
                QuestionText = "Mesopotamians used sexagesimal (base 60) as the standardized system of measurement.",
                Answer = true,
                AnswerText = "Mesopotamians used sexagesimal (base 60) as the standardized system of measurement.",
                Category = "Science"
            };
            questions.Add(q731);

            OXQuizMetadata q732 = new OXQuizMetadata()
            {
                Id = 732,
                QuestionText = "Metal balls, if heated, shrink in volume.",
                Answer = false,
                AnswerText = "Metal, if heated, expands in volume.",
                Category = "Science"
            };
            questions.Add(q732);

            OXQuizMetadata q733 = new OXQuizMetadata()
            {
                Id = 733,
                QuestionText = "Michael Jackson's album 'Thriller' is recorded in the Guinness world records as the most sold album in the world.",
                Answer = true,
                AnswerText = "Michael Jackson's 'Thriller,' the most sold album in the Guinness records, has sold 58 million copies worldwide.",
                Category = "Culture/Art"
            };
            questions.Add(q733);

            OXQuizMetadata q734 = new OXQuizMetadata()
            {
                Id = 734,
                QuestionText = "Michael Jordan is usually associated with basketball.",
                Answer = true,
                AnswerText = "Michael Jordan has won the NBA Championships 6 times and is a 14 time NBA All Star.",
                Category = "Culture/Art"
            };
            questions.Add(q734);

            OXQuizMetadata q735 = new OXQuizMetadata()
            {
                Id = 735,
                QuestionText = "Michaelangelo painted the Mona Lisa.",
                Answer = false,
                AnswerText = "Leonardo Da Vinci painted Mona Lisa.",
                Category = "Culture/Art"
            };
            questions.Add(q735);

            OXQuizMetadata q736 = new OXQuizMetadata()
            {
                Id = 736,
                QuestionText = "Michaelangelo's sculpture \"Moses\" is located in St. Peter's Basilica in Geneva.",
                Answer = false,
                AnswerText = "Michaelangelo's sculpture \"Moses\" is located in St. Peter's Basilica in Rome.",
                Category = "Culture/Art"
            };
            questions.Add(q736);

            OXQuizMetadata q737 = new OXQuizMetadata()
            {
                Id = 737,
                QuestionText = "Michelangelo painted \"The Last Supper.\"",
                Answer = false,
                AnswerText = "\"The Last Supper\" is Leonardo Da Vinci's painting.",
                Category = "General Knowledge"
            };
            questions.Add(q737);

            OXQuizMetadata q738 = new OXQuizMetadata()
            {
                Id = 738,
                QuestionText = "Michelangelo, the father of contemporary sculpting, left many magnificent sculptures including The Gates of Hell, The Age of Bronze, and The Burghers of Calais.",
                Answer = false,
                AnswerText = "Auguste Rodin, the father of contemporary sculpting, left many magnificent sculptures including The Gates of Hell, The Thinker, and The Burghers of Calais.",
                Category = "History"
            };
            questions.Add(q738);

            OXQuizMetadata q739 = new OXQuizMetadata()
            {
                Id = 739,
                QuestionText = "Miguel de Cervantes invented the International auxiliary language, Esperanto.",
                Answer = false,
                AnswerText = "Believing that the conflicts among Poland, Germany, and Jews lie in language, L. L. Zamenhof developed Esperanto in the 19th century.",
                Category = "General Knowledge"
            };
            questions.Add(q739);

            OXQuizMetadata q740 = new OXQuizMetadata()
            {
                Id = 740,
                QuestionText = "Milk Road is the road that connects the ancient civilizations of the East and the West.",
                Answer = false,
                AnswerText = "The route is called the Silk Road.",
                Category = "History"
            };
            questions.Add(q740);

            OXQuizMetadata q741 = new OXQuizMetadata()
            {
                Id = 741,
                QuestionText = "Minister Karl's surname is Vidrem.",
                Answer = true,
                AnswerText = "The Minister's full name is Karl Vidrem.",
                Category = "MapleStory 2"
            };
            questions.Add(q741);

            OXQuizMetadata q742 = new OXQuizMetadata()
            {
                Id = 742,
                QuestionText = "Miranda rights are what mandate the police to warn the suspects of their right to remain silent.",
                Answer = true,
                AnswerText = "Before a police officer arrests and interrogates the suspect, he or she must warn them that they have the right to remain silent and the right to an attorney.",
                Category = "Society"
            };
            questions.Add(q742);

            OXQuizMetadata q743 = new OXQuizMetadata()
            {
                Id = 743,
                QuestionText = "Mixing the three primary colors of pigment creates black.",
                Answer = false,
                AnswerText = "The resulting color is white.",
                Category = "General Knowledge"
            };
            questions.Add(q743);

            OXQuizMetadata q744 = new OXQuizMetadata()
            {
                Id = 744,
                QuestionText = "MK 52 Beta can be found in Sunset Tower.",
                Answer = true,
                AnswerText = "MK 52 Beta can be found in Sunset Tower.",
                Category = "MapleStory 2"
            };
            questions.Add(q744);

            OXQuizMetadata q745 = new OXQuizMetadata()
            {
                Id = 745,
                QuestionText = "Molding, giving shape using a malleable substance, is one of the primary techniques in sculpture.",
                Answer = true,
                AnswerText = "Molding, giving shape using a malleable substance, is one of the primary techniques in sculpture.",
                Category = "Culture/Art"
            };
            questions.Add(q745);

            OXQuizMetadata q746 = new OXQuizMetadata()
            {
                Id = 746,
                QuestionText = "Molds belong to the kingdom Plantae.",
                Answer = false,
                AnswerText = "Molds, like mushrooms, belong to the kingdom Fungi.",
                Category = "Science"
            };
            questions.Add(q746);

            OXQuizMetadata q747 = new OXQuizMetadata()
            {
                Id = 747,
                QuestionText = "Mon Petit Chouchou Hotel is the name of the building on the cliff where Trauros appears.",
                Answer = true,
                AnswerText = "Mon Petit Chouchou Hotel is situated on the Kolopua Crag.",
                Category = "MapleStory 2"
            };
            questions.Add(q747);

            OXQuizMetadata q748 = new OXQuizMetadata()
            {
                Id = 748,
                QuestionText = "Mon Petit Chouchou Hotel's Mademoiselle Rue is a member of the Vidrem family.",
                Answer = false,
                AnswerText = "She's an Andrea.",
                Category = "MapleStory 2"
            };
            questions.Add(q748);

            OXQuizMetadata q749 = new OXQuizMetadata()
            {
                Id = 749,
                QuestionText = "Monica is the daughter of Patrick at the Fellowstone Construction Site.",
                Answer = true,
                AnswerText = "Monica is the daughter of Patrick at the Fellowstone Construction Site.",
                Category = "MapleStory 2"
            };
            questions.Add(q749);

            OXQuizMetadata q750 = new OXQuizMetadata()
            {
                Id = 750,
                QuestionText = "Monkeys don't have blood types.",
                Answer = false,
                AnswerText = "Just like humans, monkeys do have blood types. RH, a term used to classify human's blood, comes from rhesus, the name of a species of red-furred monkey.",
                Category = "General Knowledge"
            };
            questions.Add(q750);

            OXQuizMetadata q751 = new OXQuizMetadata()
            {
                Id = 751,
                QuestionText = "Monkeys have the second highest IQ, next to humans.",
                Answer = false,
                AnswerText = "Dolphins have the second highest IQ, next to humans.",
                Category = "General Knowledge"
            };
            questions.Add(q751);

            OXQuizMetadata q752 = new OXQuizMetadata()
            {
                Id = 752,
                QuestionText = "Montesquieu is the political thinker who officially established the idea of separation of powers: legislative, executive, and judicial.",
                Answer = true,
                AnswerText = "Montesquieu is the political thinker who officially established the idea of separation of powers: legislative, executive, and judicial.",
                Category = "Society"
            };
            questions.Add(q752);

            OXQuizMetadata q753 = new OXQuizMetadata()
            {
                Id = 753,
                QuestionText = "Moses Mendelssohn worked as a cartoonist as well.",
                Answer = false,
                AnswerText = "Moses Mendelssohn worked as a painter as well.",
                Category = "Culture/Art"
            };
            questions.Add(q753);

            OXQuizMetadata q754 = new OXQuizMetadata()
            {
                Id = 754,
                QuestionText = "Moses Mendelssohn worked as a painter as well.",
                Answer = true,
                AnswerText = "Moses Mendelssohn worked as a painter as well.",
                Category = "Culture/Art"
            };
            questions.Add(q754);

            OXQuizMetadata q755 = new OXQuizMetadata()
            {
                Id = 755,
                QuestionText = "Mosquito bites itch because of the mosquito's saliva.",
                Answer = true,
                AnswerText = "Because blood coagulates quickly, mosquitoes inject their saliva to prevent clotting.",
                Category = "General Knowledge"
            };
            questions.Add(q755);

            OXQuizMetadata q756 = new OXQuizMetadata()
            {
                Id = 756,
                QuestionText = "Mosquito bites itch because of the mosquito's sweat.",
                Answer = false,
                AnswerText = "Because blood coagulates quickly, mosquitoes inject their saliva to prevent the clotting.",
                Category = "General Knowledge"
            };
            questions.Add(q756);

            OXQuizMetadata q757 = new OXQuizMetadata()
            {
                Id = 757,
                QuestionText = "Most of the Grand Canyon in the US is located in Nevada.",
                Answer = false,
                AnswerText = "The Grand Canyon is mostly in Arizona and touches Nevada and Utah.",
                Category = "General Knowledge"
            };
            questions.Add(q757);

            OXQuizMetadata q758 = new OXQuizMetadata()
            {
                Id = 758,
                QuestionText = "Most trees in tropical regions have growth rings.",
                Answer = false,
                AnswerText = "Because tropical regions lack seasons, most of their trees lack growth rings.",
                Category = "General Knowledge"
            };
            questions.Add(q758);

            OXQuizMetadata q759 = new OXQuizMetadata()
            {
                Id = 759,
                QuestionText = "Most trees in tropical regions lack growth rings.",
                Answer = true,
                AnswerText = "Most trees in tropical regions lack growth rings because they don't experience apparent seasonal climate change.",
                Category = "General Knowledge"
            };
            questions.Add(q759);

            OXQuizMetadata q760 = new OXQuizMetadata()
            {
                Id = 760,
                QuestionText = "Most trees in tropical regions lack prominent growth rings.",
                Answer = true,
                AnswerText = "Because tropical regions lack seasons, most of their trees lack prominent growth rings.",
                Category = "General Knowledge"
            };
            questions.Add(q760);

            OXQuizMetadata q761 = new OXQuizMetadata()
            {
                Id = 761,
                QuestionText = "Motherboards are composed of three main units: a control unit, an arithmetic logic unit, and a memory management unit.",
                Answer = false,
                AnswerText = "CPUs are composed of three main units: a control unit, an arithmetic logic unit, and a memory management unit.",
                Category = "IT"
            };
            questions.Add(q761);

            OXQuizMetadata q762 = new OXQuizMetadata()
            {
                Id = 762,
                QuestionText = "Moulin Rouge in French means red mill.",
                Answer = true,
                AnswerText = "Moulin Rouge in French means red mill.",
                Category = "General Knowledge"
            };
            questions.Add(q762);

            OXQuizMetadata q763 = new OXQuizMetadata()
            {
                Id = 763,
                QuestionText = "Mount Rushmore is where the faces of China's past presidents are engraved.",
                Answer = false,
                AnswerText = "Nestled in the Black Hills of South Dakota, Mouth Rushmore is the site of four gigantic carved sculptures depicting the faces of U.S. Presidents.",
                Category = "History"
            };
            questions.Add(q763);

            OXQuizMetadata q764 = new OXQuizMetadata()
            {
                Id = 764,
                QuestionText = "Mount Rushmore is where the faces of past U.S. presidents are engraved.",
                Answer = true,
                AnswerText = "Nestled in the Black Hills of South Dakota, Mouth Rushmore is the site of four gigantic carved sculptures depicting the faces of U.S. Presidents.",
                Category = "History"
            };
            questions.Add(q764);

            OXQuizMetadata q765 = new OXQuizMetadata()
            {
                Id = 765,
                QuestionText = "Mountain berries belong to the rose family.",
                Answer = true,
                AnswerText = "Mountain berries belong to the rose family.",
                Category = "General Knowledge"
            };
            questions.Add(q765);

            OXQuizMetadata q766 = new OXQuizMetadata()
            {
                Id = 766,
                QuestionText = "Movies, plays, and architecture are all included in plastic arts.",
                Answer = false,
                AnswerText = "Plastic arts include art forms that involve modeling or molding, such as sculpture and ceramics, or art involving the representation of solid objects with three-dimensional effects.",
                Category = "Culture/Art"
            };
            questions.Add(q766);

            OXQuizMetadata q767 = new OXQuizMetadata()
            {
                Id = 767,
                QuestionText = "Mozart composed operas as well.",
                Answer = true,
                AnswerText = "The Marriage of Figaro is an opera by Wolfgang Mozart.",
                Category = "Culture/Art"
            };
            questions.Add(q767);

            OXQuizMetadata q768 = new OXQuizMetadata()
            {
                Id = 768,
                QuestionText = "Mozart didn't compose a single opera.",
                Answer = false,
                AnswerText = "The Marriage of Figaro is an opera by Wolfgang Mozart.",
                Category = "Culture/Art"
            };
            questions.Add(q768);

            OXQuizMetadata q769 = new OXQuizMetadata()
            {
                Id = 769,
                QuestionText = "Mt. Rushmore is located in South Dakota.",
                Answer = true,
                AnswerText = "Mt. Rushmore is located in Keystone, South Dakota.",
                Category = "Culture/Art"
            };
            questions.Add(q769);

            OXQuizMetadata q770 = new OXQuizMetadata()
            {
                Id = 770,
                QuestionText = "Muscle pain relief sprays can erase manicures.",
                Answer = true,
                AnswerText = "Muscle pain relief sprays contain alcohol, which is effective at erasing manicures.",
                Category = "General Knowledge"
            };
            questions.Add(q770);

            OXQuizMetadata q771 = new OXQuizMetadata()
            {
                Id = 771,
                QuestionText = "Myocardial infarction is a disease that occurs when blood flow stops to a part of the heart causing damage to the heart muscle.",
                Answer = true,
                AnswerText = "It's a disease that occurs when blood flow stops to a part of the heart, causing damage to the heart muscle.",
                Category = "Science"
            };
            questions.Add(q771);

            OXQuizMetadata q772 = new OXQuizMetadata()
            {
                Id = 772,
                QuestionText = "Nails have roots.",
                Answer = true,
                AnswerText = "Nails have roots from which nails grow.",
                Category = "Science"
            };
            questions.Add(q772);

            OXQuizMetadata q773 = new OXQuizMetadata()
            {
                Id = 773,
                QuestionText = "Nanosecond refers to one-billionth of a second.",
                Answer = true,
                AnswerText = "Because a contemporary supercomputer can perform one arithmetic operation under 1 nanosecond, it can perform 10 billion of such operations.",
                Category = "IT"
            };
            questions.Add(q773);

            OXQuizMetadata q774 = new OXQuizMetadata()
            {
                Id = 774,
                QuestionText = "Nelph has a total of 3 documents stuck in his waistband.",
                Answer = false,
                AnswerText = "Nelph has a total of 2 documents stuck in his waistband.",
                Category = "MapleStory 2"
            };
            questions.Add(q774);

            OXQuizMetadata q775 = new OXQuizMetadata()
            {
                Id = 775,
                QuestionText = "Nelph was raised by a single father.",
                Answer = false,
                AnswerText = "Nelph's father died when he was young and he was raised by his mother.",
                Category = "MapleStory 2"
            };
            questions.Add(q775);

            OXQuizMetadata q776 = new OXQuizMetadata()
            {
                Id = 776,
                QuestionText = "Nelph works as the Royal Court Manager.",
                Answer = true,
                AnswerText = "Nelph is the Royal Court Manager working at the Tria Royal Office.",
                Category = "MapleStory 2"
            };
            questions.Add(q776);

            OXQuizMetadata q777 = new OXQuizMetadata()
            {
                Id = 777,
                QuestionText = "Nelson Mandela, in his 1866 paper on pea plant experimentation, discovered the dominance and recessivity in genes.",
                Answer = false,
                AnswerText = "Mendel in 1866 found the principles of inheritance.",
                Category = "Science"
            };
            questions.Add(q777);

            OXQuizMetadata q778 = new OXQuizMetadata()
            {
                Id = 778,
                QuestionText = "Newcastle disease is a contagious viral swine disease.",
                Answer = false,
                AnswerText = "Newcastle disease is a contagious viral avian disease.",
                Category = "General Knowledge"
            };
            questions.Add(q778);

            OXQuizMetadata q779 = new OXQuizMetadata()
            {
                Id = 779,
                QuestionText = "Niccolo Paganini has composed only five songs in his entire life.",
                Answer = false,
                AnswerText = "Niccolo Paganini left many songs: 21 sonatas, 24 caprices, 3 quartets, and 2 concertos, among others.",
                Category = "Culture/Art"
            };
            questions.Add(q779);

            OXQuizMetadata q780 = new OXQuizMetadata()
            {
                Id = 780,
                QuestionText = "Niccolo Paganini is a famous cello virtuoso.",
                Answer = false,
                AnswerText = "Niccolò Paganini is a famous violin virtuoso.",
                Category = "Culture/Art"
            };
            questions.Add(q780);

            OXQuizMetadata q781 = new OXQuizMetadata()
            {
                Id = 781,
                QuestionText = "Nigeria's capital is Abuja.",
                Answer = true,
                AnswerText = "Nigeria's capital is Abuja.",
                Category = "General Knowledge"
            };
            questions.Add(q781);

            OXQuizMetadata q782 = new OXQuizMetadata()
            {
                Id = 782,
                QuestionText = "Night and day are caused by the Earth's rotation.",
                Answer = true,
                AnswerText = "Earth's rotation causes night and day, and its rotation around the sun causes the four seasons.",
                Category = "Science"
            };
            questions.Add(q782);

            OXQuizMetadata q783 = new OXQuizMetadata()
            {
                Id = 783,
                QuestionText = "NIMBY refers to people who want controversial and dangerous facilities, such as landfill sites, in their neighborhoods.",
                Answer = false,
                AnswerText = "NIMBY refers to people who don't want controversial and dangerous facilities, such as landfill sites, in their neighborhoods.",
                Category = "Society"
            };
            questions.Add(q783);

            OXQuizMetadata q784 = new OXQuizMetadata()
            {
                Id = 784,
                QuestionText = "Ninecap Copper is the elite monster at the Chronoff Subway Station.",
                Answer = false,
                AnswerText = "The correct map name is Chronoff Train Station.",
                Category = "MapleStory 2"
            };
            questions.Add(q784);

            OXQuizMetadata q785 = new OXQuizMetadata()
            {
                Id = 785,
                QuestionText = "Ninecap Copper is the elite monster at the Chronoff Train Station.",
                Answer = true,
                AnswerText = "Ninecap Copper is the elite monster at the Chronoff Train Station.",
                Category = "MapleStory 2"
            };
            questions.Add(q785);

            OXQuizMetadata q786 = new OXQuizMetadata()
            {
                Id = 786,
                QuestionText = "No fish change their sex during their lifetime.",
                Answer = false,
                AnswerText = "Originally, blackhead seabreams are males that are about 10cm long. But after they lay eggs, they turn into females and get enlarged.",
                Category = "Science"
            };
            questions.Add(q786);

            OXQuizMetadata q787 = new OXQuizMetadata()
            {
                Id = 787,
                QuestionText = "No insect that go through complete metamorphosis can move during the pupa state.",
                Answer = false,
                AnswerText = "Some insects can move during the pupa state.",
                Category = "General Knowledge"
            };
            questions.Add(q787);

            OXQuizMetadata q788 = new OXQuizMetadata()
            {
                Id = 788,
                QuestionText = "Nocturne is a choral composition.",
                Answer = false,
                AnswerText = "Nocturne means a musical composition that is inspired by, or evocative of, the night.",
                Category = "Culture/Art"
            };
            questions.Add(q788);

            OXQuizMetadata q789 = new OXQuizMetadata()
            {
                Id = 789,
                QuestionText = "Nocturne means a musical composition that is inspired by, or evocative of, the night.",
                Answer = true,
                AnswerText = "Nocturne means a musical composition that is inspired by, or evocative of, the night.",
                Category = "Culture/Art"
            };
            questions.Add(q789);

            OXQuizMetadata q790 = new OXQuizMetadata()
            {
                Id = 790,
                QuestionText = "Northeasterly trade winds are dry and hot.",
                Answer = true,
                AnswerText = "Northeasterly trade winds are dry and hot.",
                Category = "General Knowledge"
            };
            questions.Add(q790);

            OXQuizMetadata q791 = new OXQuizMetadata()
            {
                Id = 791,
                QuestionText = "Notre-Dame Cathedral is in France.",
                Answer = true,
                AnswerText = "Notre-Dame Cathedral, which is in France, served a pivotal role in French history. It's the place where French monarchs were coronated.",
                Category = "General Knowledge"
            };
            questions.Add(q791);

            OXQuizMetadata q792 = new OXQuizMetadata()
            {
                Id = 792,
                QuestionText = "Notre-Dame Cathedral is in Provence.",
                Answer = false,
                AnswerText = "Notre-Dame Cathedral, which is in France, served a pivotal role in French history. It's the place where French monarchs were coronated.",
                Category = "General Knowledge"
            };
            questions.Add(q792);

            OXQuizMetadata q793 = new OXQuizMetadata()
            {
                Id = 793,
                QuestionText = "O. Henry wrote The Little Prince.",
                Answer = false,
                AnswerText = "Antoine de Saint-Exupéry wrote the Little Prince.",
                Category = "Culture/Art"
            };
            questions.Add(q793);

            OXQuizMetadata q794 = new OXQuizMetadata()
            {
                Id = 794,
                QuestionText = "Oakland is the capital of New Zealand.",
                Answer = false,
                AnswerText = "The capital of New Zealand is Wellington.",
                Category = "Culture/Art"
            };
            questions.Add(q794);

            OXQuizMetadata q795 = new OXQuizMetadata()
            {
                Id = 795,
                QuestionText = "Odin is the Allfather and father to Thor.",
                Answer = true,
                AnswerText = "Odin is the King of Asgard and Thor is his son.",
                Category = "Culture/Art"
            };
            questions.Add(q795);

            OXQuizMetadata q796 = new OXQuizMetadata()
            {
                Id = 796,
                QuestionText = "Odysseus had a role in the Trojan War.",
                Answer = true,
                AnswerText = "Odysseus had a role in the Trojan War.",
                Category = "History"
            };
            questions.Add(q796);

            OXQuizMetadata q797 = new OXQuizMetadata()
            {
                Id = 797,
                QuestionText = "Odysseus has nothing to do with the Trojan War.",
                Answer = false,
                AnswerText = "Odysseus has a role in the Trojan War.",
                Category = "History"
            };
            questions.Add(q797);

            OXQuizMetadata q798 = new OXQuizMetadata()
            {
                Id = 798,
                QuestionText = "OECD refers to the Organization for Economic Cooperation and Development.",
                Answer = true,
                AnswerText = "OECD is an organization that promotes economic growth, prosperity, and sustainable growth of economically less developed nations.",
                Category = "General Knowledge"
            };
            questions.Add(q798);

            OXQuizMetadata q799 = new OXQuizMetadata()
            {
                Id = 799,
                QuestionText = "Of the components present in blood, it is the red blood cells that carry CO2.",
                Answer = true,
                AnswerText = "Red blood cells carry both CO2 and O2.",
                Category = "Science"
            };
            questions.Add(q799);

            OXQuizMetadata q800 = new OXQuizMetadata()
            {
                Id = 800,
                QuestionText = "Of the five oceans, the largest is the Atlantic Ocean.",
                Answer = false,
                AnswerText = "The biggest ocean is the Pacific Ocean.",
                Category = "General Knowledge"
            };
            questions.Add(q800);

            OXQuizMetadata q801 = new OXQuizMetadata()
            {
                Id = 801,
                QuestionText = "Of the following three types of liquids -- water, alcohol and cooking oil -- water evaporates first.",
                Answer = false,
                AnswerText = "The order of evaporation is alcohol, water, cooking oil.",
                Category = "General Knowledge"
            };
            questions.Add(q801);

            OXQuizMetadata q802 = new OXQuizMetadata()
            {
                Id = 802,
                QuestionText = "Of the international tennis tournaments, Wimbledon has the longest history.",
                Answer = true,
                AnswerText = "Of the international tennis tournaments, Wimbledon has the longest history.",
                Category = "History"
            };
            questions.Add(q802);

            OXQuizMetadata q803 = new OXQuizMetadata()
            {
                Id = 803,
                QuestionText = "Of the many ancient civilizations, the Aegean was the first one to develop along the coasts.",
                Answer = true,
                AnswerText = "Of the many ancient civilizations, the Aegean was the first one to develop along the coasts.",
                Category = "History"
            };
            questions.Add(q803);

            OXQuizMetadata q804 = new OXQuizMetadata()
            {
                Id = 804,
                QuestionText = "On a golf course, a depression filled with impediments such as sand is called a bunker.",
                Answer = true,
                AnswerText = "One of the obstacles in golf, the bunker, is a depression filled with impediments such as sand.",
                Category = "General Knowledge"
            };
            questions.Add(q804);

            OXQuizMetadata q805 = new OXQuizMetadata()
            {
                Id = 805,
                QuestionText = "On a golf course, a depression filled with impediments such as sand is called a water hazard.",
                Answer = false,
                AnswerText = "On a golf course, a depression filled with impediments such as sand is called a bunker.",
                Category = "General Knowledge"
            };
            questions.Add(q805);

            OXQuizMetadata q806 = new OXQuizMetadata()
            {
                Id = 806,
                QuestionText = "On a traditional artist's color wheel, the primary colors are red, yellow, and green.",
                Answer = false,
                AnswerText = "The primary colors on a color wheel are red, yellow, and blue.",
                Category = "General Knowledge"
            };
            questions.Add(q806);

            OXQuizMetadata q807 = new OXQuizMetadata()
            {
                Id = 807,
                QuestionText = "On Beauty Street, you can get two-toned hair colors.",
                Answer = true,
                AnswerText = "At Rosetta Beauty Salon, you can use custom color to dye your hair two tones.",
                Category = "MapleStory 2"
            };
            questions.Add(q807);

            OXQuizMetadata q808 = new OXQuizMetadata()
            {
                Id = 808,
                QuestionText = "On rainy days, thunder strikes first before the lighting.",
                Answer = false,
                AnswerText = "The lightning strikes first. Thunder travels at the speed of sound while lightning travels at the speed of light.",
                Category = "Science"
            };
            questions.Add(q808);

            OXQuizMetadata q809 = new OXQuizMetadata()
            {
                Id = 809,
                QuestionText = "One can repel mosquitoes by pasting cinnamon onto one's body.",
                Answer = true,
                AnswerText = "Cinnamons have components in them that can repel mosquitoes.",
                Category = "General Knowledge"
            };
            questions.Add(q809);

            OXQuizMetadata q810 = new OXQuizMetadata()
            {
                Id = 810,
                QuestionText = "One way to properly store a guitar is by tightening its strings.",
                Answer = false,
                AnswerText = "If one plans on leaving a guitar unused for an extended duration, one must remove the attached strings. Over time the strings can warp the neck of the guitar.",
                Category = "Culture/Art"
            };
            questions.Add(q810);

            OXQuizMetadata q811 = new OXQuizMetadata()
            {
                Id = 811,
                QuestionText = "Only Guard Zekk gives out Alikar Prison Brochures.",
                Answer = false,
                AnswerText = "All the guards give the brochure.",
                Category = "MapleStory 2"
            };
            questions.Add(q811);

            OXQuizMetadata q812 = new OXQuizMetadata()
            {
                Id = 812,
                QuestionText = "Only in-game screenshots can be used as profile pictures.",
                Answer = false,
                AnswerText = "Images other than in-game screenshots can be used as profile pictures.",
                Category = "MapleStory 2"
            };
            questions.Add(q812);

            OXQuizMetadata q813 = new OXQuizMetadata()
            {
                Id = 813,
                QuestionText = "OPEC means the Warsaw Treaty Organization.",
                Answer = false,
                AnswerText = "OPEC refers to the Organization of Petroleum Exporting Countries.",
                Category = "General Knowledge"
            };
            questions.Add(q813);

            OXQuizMetadata q814 = new OXQuizMetadata()
            {
                Id = 814,
                QuestionText = "Pablo Picasso never made engravings.",
                Answer = false,
                AnswerText = "Pablo Picasso had made engravings.",
                Category = "Culture/Art"
            };
            questions.Add(q814);

            OXQuizMetadata q815 = new OXQuizMetadata()
            {
                Id = 815,
                QuestionText = "Pak Chiwon wrote the travelogue \"The Jehol Diary.\"",
                Answer = true,
                AnswerText = "The diary records various aspects of Qing dynasty's advanced civilization. Some of the topics included are Qing's culture, economics, military, and literature.",
                Category = "Culture/Art"
            };
            questions.Add(q815);

            OXQuizMetadata q816 = new OXQuizMetadata()
            {
                Id = 816,
                QuestionText = "Paleolithic men lived mostly around valleys and foothills.",
                Answer = false,
                AnswerText = "Paleolithic men lived mostly in caves and along river banks.",
                Category = "Society"
            };
            questions.Add(q816);

            OXQuizMetadata q817 = new OXQuizMetadata()
            {
                Id = 817,
                QuestionText = "Pampas refers to fertile South American lowlands.",
                Answer = true,
                AnswerText = "The Pampas are vast plains extending westward across central Argentina up to Uruguay.",
                Category = "General Knowledge"
            };
            questions.Add(q817);

            OXQuizMetadata q818 = new OXQuizMetadata()
            {
                Id = 818,
                QuestionText = "Pampas refers to the fertile African lowlands.",
                Answer = false,
                AnswerText = "The Pampas, also called the Pampa, Spanish La Pampa, are vast plains extending westward across central Argentina to Uruguay.",
                Category = "General Knowledge"
            };
            questions.Add(q818);

            OXQuizMetadata q819 = new OXQuizMetadata()
            {
                Id = 819,
                QuestionText = "Pantomime is a comedy show in which performers express meaning through gestures instead of words.",
                Answer = true,
                AnswerText = "Pantomime is a comedy show in which performers express meaning through gestures instead of words.",
                Category = "General Knowledge"
            };
            questions.Add(q819);

            OXQuizMetadata q820 = new OXQuizMetadata()
            {
                Id = 820,
                QuestionText = "Pantomime is a kind of spoken comedy show.",
                Answer = false,
                AnswerText = "Pantomime is a comedy show in which performers express meaning through gestures, not words.",
                Category = "General Knowledge"
            };
            questions.Add(q820);

            OXQuizMetadata q821 = new OXQuizMetadata()
            {
                Id = 821,
                QuestionText = "Parody refers to an imitation of the style of a particular writer, artist, or genre with deliberate exaggeration for comic effect.",
                Answer = true,
                AnswerText = "Parody is used widely in songs, movies, advertisements, etc.",
                Category = "General Knowledge"
            };
            questions.Add(q821);

            OXQuizMetadata q822 = new OXQuizMetadata()
            {
                Id = 822,
                QuestionText = "Paul Gauguin drew \"Potato Eaters.\"",
                Answer = false,
                AnswerText = "It is Vincent Van Gogh's work.",
                Category = "Culture/Art"
            };
            questions.Add(q822);

            OXQuizMetadata q823 = new OXQuizMetadata()
            {
                Id = 823,
                QuestionText = "Paul Gauguin was the brother of Vincent Van Gogh.",
                Answer = false,
                AnswerText = "Vincent Van Gogh and Paul Gauguin were not brothers.",
                Category = "Culture/Art"
            };
            questions.Add(q823);

            OXQuizMetadata q824 = new OXQuizMetadata()
            {
                Id = 824,
                QuestionText = "Payments for Black Market items arrive in the Mailbox in 48 hours.",
                Answer = false,
                AnswerText = "Payments for Black Market items immediately arrive in the Mailbox.",
                Category = "MapleStory 2"
            };
            questions.Add(q824);

            OXQuizMetadata q825 = new OXQuizMetadata()
            {
                Id = 825,
                QuestionText = "PDF is a document file type developed by Adobe Systems of the United States.",
                Answer = true,
                AnswerText = "PDF is the format suitable for the digital publication of media such as e-books and CDs.",
                Category = "IT"
            };
            questions.Add(q825);

            OXQuizMetadata q826 = new OXQuizMetadata()
            {
                Id = 826,
                QuestionText = "Peanuts and walnuts are composed of fat.",
                Answer = true,
                AnswerText = "Peanuts and walnuts are composed of fat.",
                Category = "General Knowledge"
            };
            questions.Add(q826);

            OXQuizMetadata q827 = new OXQuizMetadata()
            {
                Id = 827,
                QuestionText = "Peanuts and walnuts have fat in them.",
                Answer = false,
                AnswerText = "Peanuts and walnuts are composed of fat.",
                Category = "General Knowledge"
            };
            questions.Add(q827);

            OXQuizMetadata q828 = new OXQuizMetadata()
            {
                Id = 828,
                QuestionText = "Peking opera is a Korean opera.",
                Answer = false,
                AnswerText = "Peking opera is a Chinese opera.",
                Category = "Culture/Art"
            };
            questions.Add(q828);

            OXQuizMetadata q829 = new OXQuizMetadata()
            {
                Id = 829,
                QuestionText = "Penguin's teeth grow only up to 5 cm.",
                Answer = false,
                AnswerText = "Penguins don't have teeth.",
                Category = "General Knowledge"
            };
            questions.Add(q829);

            OXQuizMetadata q830 = new OXQuizMetadata()
            {
                Id = 830,
                QuestionText = "Penguin's teeth grow up to 5 mm.",
                Answer = false,
                AnswerText = "Penguins don't have teeth.",
                Category = "General Knowledge"
            };
            questions.Add(q830);

            OXQuizMetadata q831 = new OXQuizMetadata()
            {
                Id = 831,
                QuestionText = "Penguin's teeth grow up to 55 mm.",
                Answer = false,
                AnswerText = "Penguins don't have teeth.",
                Category = "General Knowledge"
            };
            questions.Add(q831);

            OXQuizMetadata q832 = new OXQuizMetadata()
            {
                Id = 832,
                QuestionText = "Perfect Guard completely negates incoming damage.",
                Answer = true,
                AnswerText = "Perfect Guard perfectly shields against incoming damage.",
                Category = "MapleStory 2"
            };
            questions.Add(q832);

            OXQuizMetadata q833 = new OXQuizMetadata()
            {
                Id = 833,
                QuestionText = "Perion Chief Wolf Heart is a Murpagoth.",
                Answer = true,
                AnswerText = "Perion Chief Wolf Heart is a Murpagoth.",
                Category = "MapleStory 2"
            };
            questions.Add(q833);

            OXQuizMetadata q834 = new OXQuizMetadata()
            {
                Id = 834,
                QuestionText = "Perion's BGM is 3 minutes and 25 seconds long.",
                Answer = false,
                AnswerText = "It's 1 minute and 36 seconds long.",
                Category = "MapleStory 2"
            };
            questions.Add(q834);

            OXQuizMetadata q835 = new OXQuizMetadata()
            {
                Id = 835,
                QuestionText = "Perion's wolf girl Tina rides a brown wolf.",
                Answer = false,
                AnswerText = "Perion's wolf girl Tina rides a white wolf.",
                Category = "MapleStory 2"
            };
            questions.Add(q835);

            OXQuizMetadata q836 = new OXQuizMetadata()
            {
                Id = 836,
                QuestionText = "Peter Pan syndrome refer to adults who are socially immature.",
                Answer = true,
                AnswerText = "Peter Pan syndrome refer to adults who, even as they age, remain socially immature.",
                Category = "General Knowledge"
            };
            questions.Add(q836);

            OXQuizMetadata q837 = new OXQuizMetadata()
            {
                Id = 837,
                QuestionText = "Petroleum is made from the energy that was stored in giant ancient plants.",
                Answer = false,
                AnswerText = "Petroleum is from animals. Coal is from plants.",
                Category = "General Knowledge"
            };
            questions.Add(q837);

            OXQuizMetadata q838 = new OXQuizMetadata()
            {
                Id = 838,
                QuestionText = "Phantom Destroyer in the Phantoma Cyborg Center is an Inanimate monster.",
                Answer = false,
                AnswerText = "Phantom Destroyer in the Phantoma Cyborg Center is a Mechanical monster.",
                Category = "MapleStory 2"
            };
            questions.Add(q838);

            OXQuizMetadata q839 = new OXQuizMetadata()
            {
                Id = 839,
                QuestionText = "Physical exercise causes hunger and leads to weight gain.",
                Answer = false,
                AnswerText = "Physical exercises do arouse hunger but don't necessarily lead to weight gain.",
                Category = "General Knowledge"
            };
            questions.Add(q839);

            OXQuizMetadata q840 = new OXQuizMetadata()
            {
                Id = 840,
                QuestionText = "Pickles are an ingredient in gum.",
                Answer = false,
                AnswerText = "Chicles are an ingredient in gum.",
                Category = "General Knowledge"
            };
            questions.Add(q840);

            OXQuizMetadata q841 = new OXQuizMetadata()
            {
                Id = 841,
                QuestionText = "Pierre Bourdieu is the political thinker who officially established the idea of separation of powers between legislative, executive, and judicial.",
                Answer = false,
                AnswerText = "Montesquieu is the political thinker who officially established the idea of separation of powers between legislative, executive, and judicial.",
                Category = "Society"
            };
            questions.Add(q841);

            OXQuizMetadata q842 = new OXQuizMetadata()
            {
                Id = 842,
                QuestionText = "Pigs don't sleep standing up.",
                Answer = true,
                AnswerText = "Like most animals, pigs sleep lying down.",
                Category = "General Knowledge"
            };
            questions.Add(q842);

            OXQuizMetadata q843 = new OXQuizMetadata()
            {
                Id = 843,
                QuestionText = "Pigs sleep standing up.",
                Answer = false,
                AnswerText = "Just like other animals, pigs sleep lying down.",
                Category = "General Knowledge"
            };
            questions.Add(q843);

            OXQuizMetadata q844 = new OXQuizMetadata()
            {
                Id = 844,
                QuestionText = "Pikachu evolves into Bulbasaur.",
                Answer = false,
                AnswerText = "Pikachu evolves into Raichu.",
                Category = "Culture/Art"
            };
            questions.Add(q844);

            OXQuizMetadata q845 = new OXQuizMetadata()
            {
                Id = 845,
                QuestionText = "Plants have adaptive immune systems.",
                Answer = false,
                AnswerText = "Plants do not have adaptive immune systems, although they do have other ways to protect themselves from disease.",
                Category = "Science"
            };
            questions.Add(q845);

            OXQuizMetadata q846 = new OXQuizMetadata()
            {
                Id = 846,
                QuestionText = "Plants only photosynthesize in the morning.",
                Answer = false,
                AnswerText = "They photosynthesize whenever they can.",
                Category = "Science"
            };
            questions.Add(q846);

            OXQuizMetadata q847 = new OXQuizMetadata()
            {
                Id = 847,
                QuestionText = "Pluto is the smallest planet in the solar system.",
                Answer = false,
                AnswerText = "Pluto is not a planet.",
                Category = "Science"
            };
            questions.Add(q847);

            OXQuizMetadata q848 = new OXQuizMetadata()
            {
                Id = 848,
                QuestionText = "Polar bears live at the South Pole.",
                Answer = false,
                AnswerText = "Polar bears live at the North Pole.",
                Category = "General Knowledge"
            };
            questions.Add(q848);

            OXQuizMetadata q849 = new OXQuizMetadata()
            {
                Id = 849,
                QuestionText = "Polliwogs grow their forelegs first.",
                Answer = false,
                AnswerText = "Polliwogs grow their hind legs first, then their forelegs.",
                Category = "Science"
            };
            questions.Add(q849);

            OXQuizMetadata q850 = new OXQuizMetadata()
            {
                Id = 850,
                QuestionText = "Polliwogs grow their hind legs first.",
                Answer = true,
                AnswerText = "Polliwogs grow their hind legs first, then their forelegs.",
                Category = "Science"
            };
            questions.Add(q850);

            OXQuizMetadata q851 = new OXQuizMetadata()
            {
                Id = 851,
                QuestionText = "Ponte Vecchio is the oldest bridge on the Arno River in Florence (Firenze).",
                Answer = true,
                AnswerText = "Ponte Vecchio was rebuilt in 1345. It's the last bridge constructed by the Roman Empire.",
                Category = "General Knowledge"
            };
            questions.Add(q851);

            OXQuizMetadata q852 = new OXQuizMetadata()
            {
                Id = 852,
                QuestionText = "Potassium's symbol is Ca.",
                Answer = false,
                AnswerText = "Potassium's symbol is K. Calcium's symbol is Ca.",
                Category = "General Knowledge"
            };
            questions.Add(q852);

            OXQuizMetadata q853 = new OXQuizMetadata()
            {
                Id = 853,
                QuestionText = "Potatoes grow out of roots.",
                Answer = false,
                AnswerText = "Potatoes are transformed roots.",
                Category = "General Knowledge"
            };
            questions.Add(q853);

            OXQuizMetadata q854 = new OXQuizMetadata()
            {
                Id = 854,
                QuestionText = "Potatoes taste sweeter when eaten with salt than with sugar.",
                Answer = true,
                AnswerText = "Ironically, the salty taste of salt heightens sweet tastes in contrast.",
                Category = "Science"
            };
            questions.Add(q854);

            OXQuizMetadata q855 = new OXQuizMetadata()
            {
                Id = 855,
                QuestionText = "PPM, the unit that describes a concentration of a substance in water or soil, is usually used to measure pollution.",
                Answer = true,
                AnswerText = "PPM, the unit that describes a concentration of a substance in water or soil, is usually used to measure pollution.",
                Category = "Science"
            };
            questions.Add(q855);

            OXQuizMetadata q856 = new OXQuizMetadata()
            {
                Id = 856,
                QuestionText = "Press the period key (.) and a Move key to walk slowly.",
                Answer = false,
                AnswerText = "Press the period key (.) and a move key to crawl.",
                Category = "MapleStory 2"
            };
            questions.Add(q856);

            OXQuizMetadata q857 = new OXQuizMetadata()
            {
                Id = 857,
                QuestionText = "Pressing Tab enlarges the Minimap.",
                Answer = true,
                AnswerText = "Press Tab to enlarge the Minimap.",
                Category = "MapleStory 2"
            };
            questions.Add(q857);

            OXQuizMetadata q858 = new OXQuizMetadata()
            {
                Id = 858,
                QuestionText = "Pretoria is the capital of the Republic of South Africa.",
                Answer = true,
                AnswerText = "Pretoria is the capital of the Republic of South Africa.",
                Category = "General Knowledge"
            };
            questions.Add(q858);

            OXQuizMetadata q859 = new OXQuizMetadata()
            {
                Id = 859,
                QuestionText = "Prince, a famous American musician, is regarded as the \"King of Pop.\"",
                Answer = false,
                AnswerText = "The \"King of Pop\" usually refers to Michael Jackson.",
                Category = "Culture/Art"
            };
            questions.Add(q859);

            OXQuizMetadata q860 = new OXQuizMetadata()
            {
                Id = 860,
                QuestionText = "Princess Maker 4 is an online game.",
                Answer = false,
                AnswerText = "Princess Maker 4 is not an online game.",
                Category = "General Knowledge"
            };
            questions.Add(q860);

            OXQuizMetadata q861 = new OXQuizMetadata()
            {
                Id = 861,
                QuestionText = "Pyramids can only be found in Egypt.",
                Answer = false,
                AnswerText = "Ancient civilizations in Mexico also built pyramids.",
                Category = "History"
            };
            questions.Add(q861);

            OXQuizMetadata q862 = new OXQuizMetadata()
            {
                Id = 862,
                QuestionText = "Pyrros Fard can be found in Pyratalos.",
                Answer = true,
                AnswerText = "Pyrros Fard can be found in Pyratalos.",
                Category = "MapleStory 2"
            };
            questions.Add(q862);

            OXQuizMetadata q863 = new OXQuizMetadata()
            {
                Id = 863,
                QuestionText = "Pyrros Fard's Belt can be found from Pyrros Fard.",
                Answer = false,
                AnswerText = "Pyrros Fard's Belt does not exist.",
                Category = "MapleStory 2"
            };
            questions.Add(q863);

            OXQuizMetadata q864 = new OXQuizMetadata()
            {
                Id = 864,
                QuestionText = "Quenta Silmarillion is another popular title of J.R.R. Tolkien, who is best known for the Lord of the Rings.",
                Answer = true,
                AnswerText = "Quenta Silmarillion is a title by J.R.R. Tolkien.",
                Category = "Culture/Art"
            };
            questions.Add(q864);

            OXQuizMetadata q865 = new OXQuizMetadata()
            {
                Id = 865,
                QuestionText = "R&B, in music, is a shorthand for rhythm and ballade.",
                Answer = false,
                AnswerText = "R&B, in music, is a shorthand for rhythm and blues.",
                Category = "Culture/Art"
            };
            questions.Add(q865);

            OXQuizMetadata q866 = new OXQuizMetadata()
            {
                Id = 866,
                QuestionText = "R2D2 is the name of the hairy beast in the movie \"Star Wars.\"",
                Answer = false,
                AnswerText = "The hairy beast is called Chewbacca. R2D2 is the name of a robot.",
                Category = "Culture/Art"
            };
            questions.Add(q866);

            OXQuizMetadata q867 = new OXQuizMetadata()
            {
                Id = 867,
                QuestionText = "Rabbit eyes appear red because they reflect sunlight.",
                Answer = false,
                AnswerText = "Rabbit eyes appear red because they are translucent and thus expose the inner blood vessels.",
                Category = "General Knowledge"
            };
            questions.Add(q867);

            OXQuizMetadata q868 = new OXQuizMetadata()
            {
                Id = 868,
                QuestionText = "Rabindranath Tagore is the first Asian person to win a Nobel Prize.",
                Answer = true,
                AnswerText = "Rabindranath Tagore, an Indian, is the first Asian person to win a Nobel Prize.",
                Category = "Culture/Art"
            };
            questions.Add(q868);

            OXQuizMetadata q869 = new OXQuizMetadata()
            {
                Id = 869,
                QuestionText = "Raccoons and humans possess the same number of neck bones.",
                Answer = true,
                AnswerText = "All mammals have the same number of neck bones.",
                Category = "General Knowledge"
            };
            questions.Add(q869);

            OXQuizMetadata q870 = new OXQuizMetadata()
            {
                Id = 870,
                QuestionText = "Raccoons are most closely related to cats.",
                Answer = false,
                AnswerText = "Raccoons are more closely related to dogs than to cats.",
                Category = "General Knowledge"
            };
            questions.Add(q870);

            OXQuizMetadata q871 = new OXQuizMetadata()
            {
                Id = 871,
                QuestionText = "Range Rover and Land Rover are two separate car brands.",
                Answer = false,
                AnswerText = "Range Rover is a car model of Land Rover.",
                Category = "General Knowledge"
            };
            questions.Add(q871);

            OXQuizMetadata q872 = new OXQuizMetadata()
            {
                Id = 872,
                QuestionText = "Ratatouille is a movie about talking toys that get lost and try to find their way back to their owner.",
                Answer = false,
                AnswerText = "Ratatouille is a movie about a rat who becomes a chef.",
                Category = "Culture/Art"
            };
            questions.Add(q872);

            OXQuizMetadata q873 = new OXQuizMetadata()
            {
                Id = 873,
                QuestionText = "Real estate owners and debtors benefit from inflation.",
                Answer = true,
                AnswerText = "One who owns non-financial assets will be better off than the one who owns financial assets.",
                Category = "General Knowledge"
            };
            questions.Add(q873);

            OXQuizMetadata q874 = new OXQuizMetadata()
            {
                Id = 874,
                QuestionText = "Real estate owners and debtors suffer losses from inflation.",
                Answer = false,
                AnswerText = "One who owns non-financial assets will be better off than one who owns financial assets.",
                Category = "General Knowledge"
            };
            questions.Add(q874);

            OXQuizMetadata q875 = new OXQuizMetadata()
            {
                Id = 875,
                QuestionText = "Red blood cells in our blood act as vessels that carry oxygen to the cells in various parts of our body.",
                Answer = true,
                AnswerText = "Red blood cells, the main component of our blood, is a blood cell specialized in carrying oxygen.",
                Category = "Science"
            };
            questions.Add(q875);

            OXQuizMetadata q876 = new OXQuizMetadata()
            {
                Id = 876,
                QuestionText = "Red, white, and blue are the colors of the American flag.",
                Answer = true,
                AnswerText = "Red, white, and blue are the colors of the American Flag.",
                Category = "General Knowledge"
            };
            questions.Add(q876);

            OXQuizMetadata q877 = new OXQuizMetadata()
            {
                Id = 877,
                QuestionText = "Renaissance means \"revival and rebirth of the classical art and intellect.\"",
                Answer = true,
                AnswerText = "Renaissance literally means \"revival and rebirth.\" Colloquially, it refers to the era of the Renaissance.",
                Category = "History"
            };
            questions.Add(q877);

            OXQuizMetadata q878 = new OXQuizMetadata()
            {
                Id = 878,
                QuestionText = "Rene Descartes is the first person to use the word \"absurd.\"",
                Answer = false,
                AnswerText = "Albert Camus coined the term through his work, the literature of the absurd.",
                Category = "General Knowledge"
            };
            questions.Add(q878);

            OXQuizMetadata q879 = new OXQuizMetadata()
            {
                Id = 879,
                QuestionText = "Rene Magritte painted The Persistence of Memory.",
                Answer = false,
                AnswerText = "Salvador Dali painted The Persistence of Memory.",
                Category = "Culture/Art"
            };
            questions.Add(q879);

            OXQuizMetadata q880 = new OXQuizMetadata()
            {
                Id = 880,
                QuestionText = "Requiem is a musical composition in honor of the dead.",
                Answer = true,
                AnswerText = "Requiem is a musical composition in honor of the dead.",
                Category = "Culture/Art"
            };
            questions.Add(q880);

            OXQuizMetadata q881 = new OXQuizMetadata()
            {
                Id = 881,
                QuestionText = "Retroreflectivity describes how light is reflected from a surface and returned to its source.",
                Answer = true,
                AnswerText = "Retroreflectives are widely applied in traffic signs.",
                Category = "Science"
            };
            questions.Add(q881);

            OXQuizMetadata q882 = new OXQuizMetadata()
            {
                Id = 882,
                QuestionText = "Revenant Zombie appears in the Abandoned Charnel House.",
                Answer = true,
                AnswerText = "Revenant Zombie appears in the Abandoned Charnel House.",
                Category = "MapleStory 2"
            };
            questions.Add(q882);

            OXQuizMetadata q883 = new OXQuizMetadata()
            {
                Id = 883,
                QuestionText = "Rice, when cooked at high altitude, tends to get half-cooked because water boils at a lower temperature at higher altitudes.",
                Answer = true,
                AnswerText = "Rice, when cooked at high altitude, tends to get half-cooked because water boils at a lower temperature at higher altitudes.",
                Category = "Science"
            };
            questions.Add(q883);

            OXQuizMetadata q884 = new OXQuizMetadata()
            {
                Id = 884,
                QuestionText = "Rice, when cooked at high altitude, tends to get half-cooked because water boils at higher temperature at higher altitudes.",
                Answer = false,
                AnswerText = "All liquids start to boil when their vapor pressure equals the air pressure. Because the atmospheric pressure is low at high altitude surroundings, water boils at a lower temperature in high altitude.",
                Category = "Science"
            };
            questions.Add(q884);

            OXQuizMetadata q885 = new OXQuizMetadata()
            {
                Id = 885,
                QuestionText = "Riot Games developed StarCraft.",
                Answer = false,
                AnswerText = "StarCraft is a real-time strategy game developed by Blizzard Entertainment.",
                Category = "IT"
            };
            questions.Add(q885);

            OXQuizMetadata q886 = new OXQuizMetadata()
            {
                Id = 886,
                QuestionText = "Ro and Noonoo are Divine monsters who appear at the Baun Tree.",
                Answer = false,
                AnswerText = "Lo and Moomoo are Divine monsters found at the Baum Tree.",
                Category = "MapleStory 2"
            };
            questions.Add(q886);

            OXQuizMetadata q887 = new OXQuizMetadata()
            {
                Id = 887,
                QuestionText = "Romanesque architecture is an architectural style characterized by horizontal lines and Roman architecture styles.",
                Answer = true,
                AnswerText = "Romanesque architecture is an architectural style of Medieval Europe that is thought to have started in France in the late 10th century, and spread to Western Europe through the mid 12th century.",
                Category = "Culture/Art"
            };
            questions.Add(q887);

            OXQuizMetadata q888 = new OXQuizMetadata()
            {
                Id = 888,
                QuestionText = "Romanticist writers and poets include Lord Byron, John Keats, and William Wordsworth.",
                Answer = true,
                AnswerText = "Romanticism is a movement in the arts and literature that was in vogue during the late 18th century and the early 19th century. It originated from Germany, England, and France and then spread to the rest of Europe.",
                Category = "Culture/Art"
            };
            questions.Add(q888);

            OXQuizMetadata q889 = new OXQuizMetadata()
            {
                Id = 889,
                QuestionText = "Rondo is a musical term that denotes tempo.",
                Answer = false,
                AnswerText = "In rondo form, a principal theme alternates with one or more contrasting themes.",
                Category = "Culture/Art"
            };
            questions.Add(q889);

            OXQuizMetadata q890 = new OXQuizMetadata()
            {
                Id = 890,
                QuestionText = "Rotterdam is the capital of the Netherlands.",
                Answer = false,
                AnswerText = "Amsterdam is the capital of the Netherlands.",
                Category = "General Knowledge"
            };
            questions.Add(q890);

            OXQuizMetadata q891 = new OXQuizMetadata()
            {
                Id = 891,
                QuestionText = "Rudyard Coupling is the author of \"The Jungle Book.\"",
                Answer = false,
                AnswerText = "Kipling through \"The Jungle Book\" provided food for thought regarding the relationships of individuals in society.",
                Category = "Culture/Art"
            };
            questions.Add(q891);

            OXQuizMetadata q892 = new OXQuizMetadata()
            {
                Id = 892,
                QuestionText = "Rudyard Kipling is the author of \"The Jungle Book.\"",
                Answer = true,
                AnswerText = "He provided food for thought on the individual's roles in society.",
                Category = "Culture/Art"
            };
            questions.Add(q892);

            OXQuizMetadata q893 = new OXQuizMetadata()
            {
                Id = 893,
                QuestionText = "Rue is a currency obtainable from Daily quests.",
                Answer = true,
                AnswerText = "Rue is a currency obtainable from Daily quests.",
                Category = "MapleStory 2"
            };
            questions.Add(q893);

            OXQuizMetadata q894 = new OXQuizMetadata()
            {
                Id = 894,
                QuestionText = "Rum is a Russian alcohol.",
                Answer = false,
                AnswerText = "Rum is not Russian. Vodka is.",
                Category = "General Knowledge"
            };
            questions.Add(q894);

            OXQuizMetadata q895 = new OXQuizMetadata()
            {
                Id = 895,
                QuestionText = "Russia's virtuoso, contemporary artist Wassily Kandinsky has been a professor at Bauhaus.",
                Answer = true,
                AnswerText = "Wassily Kandinsky began teaching in Bauhaus in June 1922.",
                Category = "Culture/Art"
            };
            questions.Add(q895);

            OXQuizMetadata q896 = new OXQuizMetadata()
            {
                Id = 896,
                QuestionText = "Safari Jackets are jackets that help hunters endure hot climates.",
                Answer = true,
                AnswerText = "Safari jackets, considered sporty fashion, are used for hunting and traveling in Africa. Made of practical fibers, they are comfortable and are high in utility.",
                Category = "General Knowledge"
            };
            questions.Add(q896);

            OXQuizMetadata q897 = new OXQuizMetadata()
            {
                Id = 897,
                QuestionText = "Saint Basil's Cathedral is in India.",
                Answer = false,
                AnswerText = "Saint Basil's Cathedral is in Russia.",
                Category = "Society"
            };
            questions.Add(q897);

            OXQuizMetadata q898 = new OXQuizMetadata()
            {
                Id = 898,
                QuestionText = "Salt doesn't dissolve in water.",
                Answer = false,
                AnswerText = "Salt dissolves in water.",
                Category = "General Knowledge"
            };
            questions.Add(q898);

            OXQuizMetadata q899 = new OXQuizMetadata()
            {
                Id = 899,
                QuestionText = "Santa Cruz Island is among the Galapagos islands.",
                Answer = true,
                AnswerText = "There is an island called Santa Cruz Island in the Galapagos Islands (Ecuador).",
                Category = "General Knowledge"
            };
            questions.Add(q899);

            OXQuizMetadata q900 = new OXQuizMetadata()
            {
                Id = 900,
                QuestionText = "Santa Lucia is a Swiss folk song.",
                Answer = false,
                AnswerText = "Santa Lucia is an Italian folk song.",
                Category = "Culture/Art"
            };
            questions.Add(q900);

            OXQuizMetadata q901 = new OXQuizMetadata()
            {
                Id = 901,
                QuestionText = "Santa Lucia is an Italian folk song.",
                Answer = true,
                AnswerText = "Santa Lucia is an Italian folk song.",
                Category = "Culture/Art"
            };
            questions.Add(q901);

            OXQuizMetadata q902 = new OXQuizMetadata()
            {
                Id = 902,
                QuestionText = "Scaling in dentistry produces whiter teeth.",
                Answer = false,
                AnswerText = "Scaling in dentistry removes plaque and tartar.",
                Category = "General Knowledge"
            };
            questions.Add(q902);

            OXQuizMetadata q903 = new OXQuizMetadata()
            {
                Id = 903,
                QuestionText = "Scanners are devices that can input complex photos and maps.",
                Answer = true,
                AnswerText = "Scanners are devices that can read complex figures and shapes on a paper and convert that information into computer graphics.",
                Category = "IT"
            };
            questions.Add(q903);

            OXQuizMetadata q904 = new OXQuizMetadata()
            {
                Id = 904,
                QuestionText = "Schwanda is a sculptor, whose artwork can be seen along the streets of Tria.",
                Answer = false,
                AnswerText = "Schwanda is a painter, and his paintings are in his studio in Tria.",
                Category = "MapleStory 2"
            };
            questions.Add(q904);

            OXQuizMetadata q905 = new OXQuizMetadata()
            {
                Id = 905,
                QuestionText = "Scissors, hammers, and toe clippers use the principles of leverage.",
                Answer = true,
                AnswerText = "They all employ the principles of leverage.",
                Category = "Science"
            };
            questions.Add(q905);

            OXQuizMetadata q906 = new OXQuizMetadata()
            {
                Id = 906,
                QuestionText = "Scotland's traditional clothing for both genders is bell-bottoms.",
                Answer = false,
                AnswerText = "Scotland's traditional clothing is the kilt.",
                Category = "General Knowledge"
            };
            questions.Add(q906);

            OXQuizMetadata q907 = new OXQuizMetadata()
            {
                Id = 907,
                QuestionText = "Scotland's traditional clothing, for both genders, is a kilt.",
                Answer = true,
                AnswerText = "Scotland's traditional clothing is a kilt.",
                Category = "General Knowledge"
            };
            questions.Add(q907);

            OXQuizMetadata q908 = new OXQuizMetadata()
            {
                Id = 908,
                QuestionText = "Sea water doesn't freeze.",
                Answer = false,
                AnswerText = "Sea waters are more resistant to freezing than fresh water, but the former do freeze. The glaciers in the North Pole are frozen sea water.",
                Category = "General Knowledge"
            };
            questions.Add(q908);

            OXQuizMetadata q909 = new OXQuizMetadata()
            {
                Id = 909,
                QuestionText = "Sea water freezes.",
                Answer = true,
                AnswerText = "Sea waters are more resistant to freezing than fresh water, but the former do freeze. The glaciers in the North Pole are frozen sea water.",
                Category = "General Knowledge"
            };
            questions.Add(q909);

            OXQuizMetadata q910 = new OXQuizMetadata()
            {
                Id = 910,
                QuestionText = "Sepak Takraw is a ball game.",
                Answer = true,
                AnswerText = "Sepak Takraw is a ball game, played by three players. It uses a rattan ball, slightly larger than a man's fist and with a hole in it.",
                Category = "Culture/Art"
            };
            questions.Add(q910);

            OXQuizMetadata q911 = new OXQuizMetadata()
            {
                Id = 911,
                QuestionText = "Sepak Takraw is a combat sport.",
                Answer = false,
                AnswerText = "Sepak Takraw is a ball game, played by three players. It uses a rattan ball, a ball that is slightly larger than a man's fist and has a hole in it.",
                Category = "Culture/Art"
            };
            questions.Add(q911);

            OXQuizMetadata q912 = new OXQuizMetadata()
            {
                Id = 912,
                QuestionText = "Separation of Powers is a recent political concept that prevents one group from gaining excessive power and promotes civil rights and freedom.",
                Answer = true,
                AnswerText = "The United States is the first country to adopt Separation of Powers in 1787. South Korea, too, applied this theory to its constitution.",
                Category = "Society"
            };
            questions.Add(q912);

            OXQuizMetadata q913 = new OXQuizMetadata()
            {
                Id = 913,
                QuestionText = "Sergei Rachmaninoff composed \"The Nutcracker.\"",
                Answer = false,
                AnswerText = "Pyotr Ilyich Tchaikovsky composed \"The Nutcracker.\"",
                Category = "Culture/Art"
            };
            questions.Add(q913);

            OXQuizMetadata q914 = new OXQuizMetadata()
            {
                Id = 914,
                QuestionText = "Sergei Rachmaninoff composed Pathetique.",
                Answer = false,
                AnswerText = "Pathetique is one of Pyotr Tchaikovsky's signature works.",
                Category = "Culture/Art"
            };
            questions.Add(q914);

            OXQuizMetadata q915 = new OXQuizMetadata()
            {
                Id = 915,
                QuestionText = "Sewage systems carry drinking water through pipes.",
                Answer = false,
                AnswerText = "Water supply carries drinking water through pipes.",
                Category = "General Knowledge"
            };
            questions.Add(q915);

            OXQuizMetadata q916 = new OXQuizMetadata()
            {
                Id = 916,
                QuestionText = "Shamaness Nahmed is a Kaka.",
                Answer = false,
                AnswerText = "Shamaness Nahmed is a Pigming.",
                Category = "MapleStory 2"
            };
            questions.Add(q916);

            OXQuizMetadata q917 = new OXQuizMetadata()
            {
                Id = 917,
                QuestionText = "Shark's fin is there to reduce resistance in underwater maneuvers.",
                Answer = false,
                AnswerText = "A shark uses its fin to balance.",
                Category = "Science"
            };
            questions.Add(q917);

            OXQuizMetadata q918 = new OXQuizMetadata()
            {
                Id = 918,
                QuestionText = "Sherlock Holmes is a real person.",
                Answer = false,
                AnswerText = "Sherlock Holmes is a fictional character.",
                Category = "Culture/Art"
            };
            questions.Add(q918);

            OXQuizMetadata q919 = new OXQuizMetadata()
            {
                Id = 919,
                QuestionText = "Shih Tzu is a Korean dog breed.",
                Answer = false,
                AnswerText = "Shih Tzu is a Chinese dog breed.",
                Category = "General Knowledge"
            };
            questions.Add(q919);

            OXQuizMetadata q920 = new OXQuizMetadata()
            {
                Id = 920,
                QuestionText = "Shoo and Booboo dwell inside Bluefrost Cave.",
                Answer = false,
                AnswerText = "Shoo and Booboo dwell inside Blueshade Cave.",
                Category = "MapleStory 2"
            };
            questions.Add(q920);

            OXQuizMetadata q921 = new OXQuizMetadata()
            {
                Id = 921,
                QuestionText = "Silicon is a key material used for making semiconductors.",
                Answer = true,
                AnswerText = "Silicon and germanium are the primary materials that make up semi-conductors.",
                Category = "General Knowledge"
            };
            questions.Add(q921);

            OXQuizMetadata q922 = new OXQuizMetadata()
            {
                Id = 922,
                QuestionText = "Silk and wool are examples of natural plant fibers.",
                Answer = false,
                AnswerText = "Silk and wool are examples of natural animal fibers.",
                Category = "General Knowledge"
            };
            questions.Add(q922);

            OXQuizMetadata q923 = new OXQuizMetadata()
            {
                Id = 923,
                QuestionText = "Sir Francis Bacon left the famous quote \"Man is only a reed, the weakest in nature, but he is a thinking reed.\"",
                Answer = false,
                AnswerText = "Pascal wrote in his book that men are weak like reeds. But he also noted that because we can think, we are superior to any other species.",
                Category = "General Knowledge"
            };
            questions.Add(q923);

            OXQuizMetadata q924 = new OXQuizMetadata()
            {
                Id = 924,
                QuestionText = "Skunks, taxonomically, belong to the weasel family.",
                Answer = false,
                AnswerText = "Skunks belong to the family Mephitidae.",
                Category = "General Knowledge"
            };
            questions.Add(q924);

            OXQuizMetadata q925 = new OXQuizMetadata()
            {
                Id = 925,
                QuestionText = "Snails are insects.",
                Answer = false,
                AnswerText = "Snails are mollusks.",
                Category = "General Knowledge"
            };
            questions.Add(q925);

            OXQuizMetadata q926 = new OXQuizMetadata()
            {
                Id = 926,
                QuestionText = "Snails have 2 genders.",
                Answer = false,
                AnswerText = "A snail is a hermaphrodite, an organism that has reproductive organs normally associated with both male and female sexes.",
                Category = "General Knowledge"
            };
            questions.Add(q926);

            OXQuizMetadata q927 = new OXQuizMetadata()
            {
                Id = 927,
                QuestionText = "Snails have teeth.",
                Answer = true,
                AnswerText = "Snails have teeth.",
                Category = "General Knowledge"
            };
            questions.Add(q927);

            OXQuizMetadata q928 = new OXQuizMetadata()
            {
                Id = 928,
                QuestionText = "Snakes can crawl backward.",
                Answer = false,
                AnswerText = "Snakes can either turn their body or head to look back, but can't crawl back.",
                Category = "General Knowledge"
            };
            questions.Add(q928);

            OXQuizMetadata q929 = new OXQuizMetadata()
            {
                Id = 929,
                QuestionText = "Socrates left the famous quote \"Knowledge is Power.\"",
                Answer = false,
                AnswerText = "Sir Francis Bacon left the quote.",
                Category = "General Knowledge"
            };
            questions.Add(q929);

            OXQuizMetadata q930 = new OXQuizMetadata()
            {
                Id = 930,
                QuestionText = "Soda is an effective ant repellent.",
                Answer = false,
                AnswerText = "Salt is an effective ant repellant.",
                Category = "General Knowledge"
            };
            questions.Add(q930);

            OXQuizMetadata q931 = new OXQuizMetadata()
            {
                Id = 931,
                QuestionText = "Some fish change their sex.",
                Answer = true,
                AnswerText = "Some fish change their sex.",
                Category = "General Knowledge"
            };
            questions.Add(q931);

            OXQuizMetadata q932 = new OXQuizMetadata()
            {
                Id = 932,
                QuestionText = "Some fishing rods shorten fishing time.",
                Answer = true,
                AnswerText = "Chubby Whale Fishing Pole and other fishing rods shorten fishing time.",
                Category = "MapleStory 2"
            };
            questions.Add(q932);

            OXQuizMetadata q933 = new OXQuizMetadata()
            {
                Id = 933,
                QuestionText = "Sonic bomb refers to the phenomenon that occurs when a flying object exceeds the speed of sound.",
                Answer = false,
                AnswerText = "It's called a sonic boom.",
                Category = "General Knowledge"
            };
            questions.Add(q933);

            OXQuizMetadata q934 = new OXQuizMetadata()
            {
                Id = 934,
                QuestionText = "Sonic, the main character of \"Sonic the Hedgehog,\" is a mole.",
                Answer = false,
                AnswerText = "Sonic is a hedgehog.",
                Category = "Culture/Art"
            };
            questions.Add(q934);

            OXQuizMetadata q935 = new OXQuizMetadata()
            {
                Id = 935,
                QuestionText = "South Korea has a Novel Laureate.",
                Answer = true,
                AnswerText = "Kim Dae-Jung, a former president of South Korea, received a Nobel Peace Prize.",
                Category = "General Knowledge"
            };
            questions.Add(q935);

            OXQuizMetadata q936 = new OXQuizMetadata()
            {
                Id = 936,
                QuestionText = "South Korea lacks a Nobel Laureate.",
                Answer = false,
                AnswerText = "Kim Dae-Jung, a former president of South Korea, received a Novel Peace Prize.",
                Category = "General Knowledge"
            };
            questions.Add(q936);

            OXQuizMetadata q937 = new OXQuizMetadata()
            {
                Id = 937,
                QuestionText = "South Korea won the World Baseball Softball Confederation (WSBC) Premiere 12, which was held in 2015.",
                Answer = true,
                AnswerText = "South Korea won the World Baseball Softball Confederation (WSBC) Premiere 12.",
                Category = "General Knowledge"
            };
            questions.Add(q937);

            OXQuizMetadata q938 = new OXQuizMetadata()
            {
                Id = 938,
                QuestionText = "South Korea's country dialing code is 81.",
                Answer = false,
                AnswerText = "South Korea's country dialing code is 82.",
                Category = "General Knowledge"
            };
            questions.Add(q938);

            OXQuizMetadata q939 = new OXQuizMetadata()
            {
                Id = 939,
                QuestionText = "South Royal Road only spawns monsters at Level 9 or below.",
                Answer = false,
                AnswerText = "Devilin Warriors and Chiefs are above Level 10.",
                Category = "MapleStory 2"
            };
            questions.Add(q939);

            OXQuizMetadata q940 = new OXQuizMetadata()
            {
                Id = 940,
                QuestionText = "Spain won the 2002 World Cup in Korea and Japan.",
                Answer = false,
                AnswerText = "Brazil won the 2002 World Cup in Korea and Japan.",
                Category = "Culture/Art"
            };
            questions.Add(q940);

            OXQuizMetadata q941 = new OXQuizMetadata()
            {
                Id = 941,
                QuestionText = "Spain won the 2010 World Cup in South Africa.",
                Answer = true,
                AnswerText = "Spain won the 2010 World Cup South Africa.",
                Category = "Culture/Art"
            };
            questions.Add(q941);

            OXQuizMetadata q942 = new OXQuizMetadata()
            {
                Id = 942,
                QuestionText = "Spain's capital is Madrid, and Canada's capital is Ottawa.",
                Answer = true,
                AnswerText = "Spain's capital is Madrid, and Canada's capital is Ottawa.",
                Category = "General Knowledge"
            };
            questions.Add(q942);

            OXQuizMetadata q943 = new OXQuizMetadata()
            {
                Id = 943,
                QuestionText = "Spain's capital is Madrid, and Canada's capital is Toronto.",
                Answer = false,
                AnswerText = "Spain's capital is Madrid, and Canada's capital is Ottawa.",
                Category = "General Knowledge"
            };
            questions.Add(q943);

            OXQuizMetadata q944 = new OXQuizMetadata()
            {
                Id = 944,
                QuestionText = "Speed = Time/Meters",
                Answer = false,
                AnswerText = "Speed = Meters/Time",
                Category = "Science"
            };
            questions.Add(q944);

            OXQuizMetadata q945 = new OXQuizMetadata()
            {
                Id = 945,
                QuestionText = "Spider-Man is related to Spiders.",
                Answer = true,
                AnswerText = "The comic is about Spiderman, a hero who got his power from a bite of a genetically engineered spider and fends off villains.",
                Category = "Culture/Art"
            };
            questions.Add(q945);

            OXQuizMetadata q946 = new OXQuizMetadata()
            {
                Id = 946,
                QuestionText = "Spiderman got his powers by getting hit by gamma rays.",
                Answer = false,
                AnswerText = "Spiderman got his powers when he got bit by a radioactive spider.",
                Category = "Culture/Art"
            };
            questions.Add(q946);

            OXQuizMetadata q947 = new OXQuizMetadata()
            {
                Id = 947,
                QuestionText = "Spiderman is related to Spiders.",
                Answer = true,
                AnswerText = "Spiderman's character was bitten by spiders.",
                Category = "Culture/Art"
            };
            questions.Add(q947);

            OXQuizMetadata q948 = new OXQuizMetadata()
            {
                Id = 948,
                QuestionText = "Squids have 7 tentacles.",
                Answer = false,
                AnswerText = "Squids have 10 tentacles.",
                Category = "General Knowledge"
            };
            questions.Add(q948);

            OXQuizMetadata q949 = new OXQuizMetadata()
            {
                Id = 949,
                QuestionText = "Squids lack bones.",
                Answer = false,
                AnswerText = "An octopus doesn't have bones, but a squid does.",
                Category = "General Knowledge"
            };
            questions.Add(q949);

            OXQuizMetadata q950 = new OXQuizMetadata()
            {
                Id = 950,
                QuestionText = "SSD, a computer storage device, stands for Super Speed Drive.",
                Answer = false,
                AnswerText = "SSD stands for Solid State Drive.",
                Category = "IT"
            };
            questions.Add(q950);

            OXQuizMetadata q951 = new OXQuizMetadata()
            {
                Id = 951,
                QuestionText = "Staccato is a musical term that means to rush through the notes.",
                Answer = false,
                AnswerText = "Staccato is a musical term that means to read each note with sharp distinction.",
                Category = "General Knowledge"
            };
            questions.Add(q951);

            OXQuizMetadata q952 = new OXQuizMetadata()
            {
                Id = 952,
                QuestionText = "Stevie Wonder, an American musician, lost his sight at the age of 14 due to a car accident.",
                Answer = false,
                AnswerText = "Stevie Wonder lost his sight when he was an infant.",
                Category = "Culture/Art"
            };
            questions.Add(q952);

            OXQuizMetadata q953 = new OXQuizMetadata()
            {
                Id = 953,
                QuestionText = "Stone is lighter to lift underwater than out in the air.",
                Answer = true,
                AnswerText = "It's because of buoyancy.",
                Category = "Science"
            };
            questions.Add(q953);

            OXQuizMetadata q954 = new OXQuizMetadata()
            {
                Id = 954,
                QuestionText = "Strawberries belong to the lily family.",
                Answer = false,
                AnswerText = "Strawberries belong to the rose family.",
                Category = "General Knowledge"
            };
            questions.Add(q954);

            OXQuizMetadata q955 = new OXQuizMetadata()
            {
                Id = 955,
                QuestionText = "Strawberries belong to the rhododendron family.",
                Answer = false,
                AnswerText = "Strawberries belong to the rose family.",
                Category = "General Knowledge"
            };
            questions.Add(q955);

            OXQuizMetadata q956 = new OXQuizMetadata()
            {
                Id = 956,
                QuestionText = "Strawberries belong to the rose family.",
                Answer = true,
                AnswerText = "Strawberries belong to the rose family.",
                Category = "Science"
            };
            questions.Add(q956);

            OXQuizMetadata q957 = new OXQuizMetadata()
            {
                Id = 957,
                QuestionText = "Summer Olympic sports include Swimming, Track and Field, Gymnastics, and Ice Skating.",
                Answer = false,
                AnswerText = "Summer Olympics do not include Ice Skating.",
                Category = "Culture/Art"
            };
            questions.Add(q957);

            OXQuizMetadata q958 = new OXQuizMetadata()
            {
                Id = 958,
                QuestionText = "Summer solstice refers to the day when daytime and nighttime are equal in length.",
                Answer = false,
                AnswerText = "Summer solstice refers to the day when daytime is the longest. Winter solstice refers to the day when nighttime is the longest.",
                Category = "General Knowledge"
            };
            questions.Add(q958);

            OXQuizMetadata q959 = new OXQuizMetadata()
            {
                Id = 959,
                QuestionText = "Summoner Latun is a rabbit in a red hood.",
                Answer = false,
                AnswerText = "Summoner Latun is the Elite monster in a dark green hood.",
                Category = "MapleStory 2"
            };
            questions.Add(q959);

            OXQuizMetadata q960 = new OXQuizMetadata()
            {
                Id = 960,
                QuestionText = "Sunflowers don't have pollen.",
                Answer = false,
                AnswerText = "Sunflowers do have pollen.",
                Category = "General Knowledge"
            };
            questions.Add(q960);

            OXQuizMetadata q961 = new OXQuizMetadata()
            {
                Id = 961,
                QuestionText = "Sunflowers have seeds.",
                Answer = true,
                AnswerText = "One can extract oil from sunflower seeds.",
                Category = "General Knowledge"
            };
            questions.Add(q961);

            OXQuizMetadata q962 = new OXQuizMetadata()
            {
                Id = 962,
                QuestionText = "Superman is Earth-born.",
                Answer = false,
                AnswerText = "He's an alien.",
                Category = "Culture/Art"
            };
            questions.Add(q962);

            OXQuizMetadata q963 = new OXQuizMetadata()
            {
                Id = 963,
                QuestionText = "Superman's alter ego is Clark Kent.",
                Answer = true,
                AnswerText = "Clark Kent is a reporter for the Daily Planet and is Superman.",
                Category = "Culture/Art"
            };
            questions.Add(q963);

            OXQuizMetadata q964 = new OXQuizMetadata()
            {
                Id = 964,
                QuestionText = "Surmony's brother, who runs a gear shop in Perion, is called Slacky.",
                Answer = true,
                AnswerText = "Surmony's brother, who runs a gear shop, is called Slacky.",
                Category = "MapleStory 2"
            };
            questions.Add(q964);

            OXQuizMetadata q965 = new OXQuizMetadata()
            {
                Id = 965,
                QuestionText = "Suvi is the capital of Fiji.",
                Answer = false,
                AnswerText = "The capital of Fiji is Suva, not Suvi.",
                Category = "Culture/Art"
            };
            questions.Add(q965);

            OXQuizMetadata q966 = new OXQuizMetadata()
            {
                Id = 966,
                QuestionText = "Swallowing pills with soda has no downside.",
                Answer = false,
                AnswerText = "In some cases, phosphoric acid in soft drinks must be avoided when taking pills.",
                Category = "General Knowledge"
            };
            questions.Add(q966);

            OXQuizMetadata q967 = new OXQuizMetadata()
            {
                Id = 967,
                QuestionText = "Swaraj is an anti-British movement that China's Lu Xun started.",
                Answer = false,
                AnswerText = "Swaraj is an anti-British movement that started in 1906 in India.",
                Category = "History"
            };
            questions.Add(q967);

            OXQuizMetadata q968 = new OXQuizMetadata()
            {
                Id = 968,
                QuestionText = "Swaraj is an anti-British movement that Gandhi started.",
                Answer = true,
                AnswerText = "Swaraj is an anti-British movement that started in 1906, in India.",
                Category = "History"
            };
            questions.Add(q968);

            OXQuizMetadata q969 = new OXQuizMetadata()
            {
                Id = 969,
                QuestionText = "Sydney is the capital of Australia.",
                Answer = false,
                AnswerText = "Canberra is the capital of Austrailia.",
                Category = "General Knowledge"
            };
            questions.Add(q969);

            OXQuizMetadata q970 = new OXQuizMetadata()
            {
                Id = 970,
                QuestionText = "Symphony no. 6, \"Pastoral,\" is Joseph Haydn's masterpiece.",
                Answer = false,
                AnswerText = "Symphony no. 6, \"Pastoral,\" is Beethoven's masterpiece.",
                Category = "Culture/Art"
            };
            questions.Add(q970);

            OXQuizMetadata q971 = new OXQuizMetadata()
            {
                Id = 971,
                QuestionText = "Symphony No. 94 \"Surprise\" is Beethoven's work.",
                Answer = false,
                AnswerText = "Symphony No. 94 \"Surprise\" is Joseph Haydn's work.",
                Category = "Culture/Art"
            };
            questions.Add(q971);

            OXQuizMetadata q972 = new OXQuizMetadata()
            {
                Id = 972,
                QuestionText = "Tablets are devices that can input complex photos and maps.",
                Answer = false,
                AnswerText = "Scanners are devices that can read complex figures and shapes on a paper and convert that information into computer data.",
                Category = "General Knowledge"
            };
            questions.Add(q972);

            OXQuizMetadata q973 = new OXQuizMetadata()
            {
                Id = 973,
                QuestionText = "Taeguk, the Korean national flag, uses yellow colors.",
                Answer = false,
                AnswerText = "Taeguk uses white, red, blue, and black.",
                Category = "General Knowledge"
            };
            questions.Add(q973);

            OXQuizMetadata q974 = new OXQuizMetadata()
            {
                Id = 974,
                QuestionText = "Taekwondo became an official Olympic Sport from the Seoul Olympics.",
                Answer = false,
                AnswerText = "Taekwondo became an official Olympic Sport starting at the Sydney Olympics.",
                Category = "General Knowledge"
            };
            questions.Add(q974);

            OXQuizMetadata q975 = new OXQuizMetadata()
            {
                Id = 975,
                QuestionText = "Taepyeongso, a Korean double reed wind instrument, is also called 'Hojok' or 'Ballbari.'",
                Answer = false,
                AnswerText = "Taepyeongso, a Korean double reed wind instrument, is also called 'Hojok,' Nallari,' or 'Saenap.'",
                Category = "History"
            };
            questions.Add(q975);

            OXQuizMetadata q976 = new OXQuizMetadata()
            {
                Id = 976,
                QuestionText = "Taliskar was built before Kerning City.",
                Answer = false,
                AnswerText = "Taliskar is the city built by fringe scientists from Kerning City.",
                Category = "MapleStory 2"
            };
            questions.Add(q976);

            OXQuizMetadata q977 = new OXQuizMetadata()
            {
                Id = 977,
                QuestionText = "Tango is a French dance.",
                Answer = false,
                AnswerText = "Tango is a Spanish dance.",
                Category = "Culture/Art"
            };
            questions.Add(q977);

            OXQuizMetadata q978 = new OXQuizMetadata()
            {
                Id = 978,
                QuestionText = "Tango is a South American music.",
                Answer = true,
                AnswerText = "Tango is a South American music that originated from Argentina.",
                Category = "Culture/Art"
            };
            questions.Add(q978);

            OXQuizMetadata q979 = new OXQuizMetadata()
            {
                Id = 979,
                QuestionText = "Tango is a Spanish style of music.",
                Answer = false,
                AnswerText = "Tango is a South American music that originated from Argentina.",
                Category = "Culture/Art"
            };
            questions.Add(q979);

            OXQuizMetadata q980 = new OXQuizMetadata()
            {
                Id = 980,
                QuestionText = "TCP/IP is a protocol related to the internet.",
                Answer = true,
                AnswerText = "It's closely related to the internet.",
                Category = "IT"
            };
            questions.Add(q980);

            OXQuizMetadata q981 = new OXQuizMetadata()
            {
                Id = 981,
                QuestionText = "Tear concentration can't be used to reveal whether a person is shedding tears of sadness or joy.",
                Answer = false,
                AnswerText = "When one is angry, the sympathetic nervous system excretes tears that are high in sodium chloride.",
                Category = "Science"
            };
            questions.Add(q981);

            OXQuizMetadata q982 = new OXQuizMetadata()
            {
                Id = 982,
                QuestionText = "Tear concentration reveals whether a person is shedding tears of sadness or joy.",
                Answer = true,
                AnswerText = "When one is angry, one's sympathetic nervous system excretes tears that are high in sodium chloride.",
                Category = "Science"
            };
            questions.Add(q982);

            OXQuizMetadata q983 = new OXQuizMetadata()
            {
                Id = 983,
                QuestionText = "Telstar was the official ball for the 1970 World Cup in Mexico.",
                Answer = true,
                AnswerText = "Telstar was the official ball for the 1970 World Cup Mexico.",
                Category = "Culture/Art"
            };
            questions.Add(q983);

            OXQuizMetadata q984 = new OXQuizMetadata()
            {
                Id = 984,
                QuestionText = "Thanos is a major villain in the Avengers.",
                Answer = true,
                AnswerText = "Thanos is a major villain in the Avengers.",
                Category = "Culture/Art"
            };
            questions.Add(q984);

            OXQuizMetadata q985 = new OXQuizMetadata()
            {
                Id = 985,
                QuestionText = "The \"Divine Comedy\" is a trilogy. The three books are \"Hell,\" \"Purgatory,\" and \"Paradise.\"",
                Answer = false,
                AnswerText = "The \"Divine Comedy\" is a single long narrative poem that depicts humanity's redemption from sins.",
                Category = "Culture/Art"
            };
            questions.Add(q985);

            OXQuizMetadata q986 = new OXQuizMetadata()
            {
                Id = 986,
                QuestionText = "The 1934 World Cup was held in Sweden.",
                Answer = false,
                AnswerText = "The 1934 World Cup was held in Italy.",
                Category = "Culture/Art"
            };
            questions.Add(q986);

            OXQuizMetadata q987 = new OXQuizMetadata()
            {
                Id = 987,
                QuestionText = "The 1994 World Cup was held in Canada.",
                Answer = false,
                AnswerText = "The 1994 World Cup was held in the United States.",
                Category = "Culture/Art"
            };
            questions.Add(q987);

            OXQuizMetadata q988 = new OXQuizMetadata()
            {
                Id = 988,
                QuestionText = "The 2016 Summer Olympics were held in Rio De Janeiro.",
                Answer = true,
                AnswerText = "Rio De Janeiro was the host of the 2016 Olympics. It was the first year since 2000 that the Summer Olympics were held in the Southern Hemisphere.",
                Category = "Culture/Art"
            };
            questions.Add(q988);

            OXQuizMetadata q989 = new OXQuizMetadata()
            {
                Id = 989,
                QuestionText = "The ABO blood group system follows Mendel's Laws.",
                Answer = true,
                AnswerText = "Mendelian inheritance is divided into three laws: the law of segregation of genes, the law of independent assortment, and the law of dominance. Mendelian inheritance can explain the ABO blood group system.",
                Category = "General Knowledge"
            };
            questions.Add(q989);

            OXQuizMetadata q990 = new OXQuizMetadata()
            {
                Id = 990,
                QuestionText = "The Achilles heel is at the elbow joint.",
                Answer = false,
                AnswerText = "The Achilles heel is at the heel.",
                Category = "General Knowledge"
            };
            questions.Add(q990);

            OXQuizMetadata q991 = new OXQuizMetadata()
            {
                Id = 991,
                QuestionText = "The albatross is a mythical beast.",
                Answer = false,
                AnswerText = "The albatross is not a mythological beast.",
                Category = "General Knowledge"
            };
            questions.Add(q991);

            OXQuizMetadata q992 = new OXQuizMetadata()
            {
                Id = 992,
                QuestionText = "The assassin's Mark of Death is activated when cast.",
                Answer = false,
                AnswerText = "Mark of Death is a passive skill that creates a chance of marking enemies with 30% or lower HP when the Assassin attacks them.",
                Category = "MapleStory 2"
            };
            questions.Add(q992);

            OXQuizMetadata q993 = new OXQuizMetadata()
            {
                Id = 993,
                QuestionText = "The Australian Open, of the international tennis tournaments, has the longest history.",
                Answer = false,
                AnswerText = "Wimbledon, of the international tennis tournaments, has the longest history.",
                Category = "General Knowledge"
            };
            questions.Add(q993);

            OXQuizMetadata q994 = new OXQuizMetadata()
            {
                Id = 994,
                QuestionText = "The big intestine is the longer of the two intestines.",
                Answer = false,
                AnswerText = "The small intestine is the longer of the two intestines.",
                Category = "General Knowledge"
            };
            questions.Add(q994);

            OXQuizMetadata q995 = new OXQuizMetadata()
            {
                Id = 995,
                QuestionText = "The biggest country in Africa is Algeria.",
                Answer = true,
                AnswerText = "The biggest country in Africa is Algeria.",
                Category = "General Knowledge"
            };
            questions.Add(q995);

            OXQuizMetadata q996 = new OXQuizMetadata()
            {
                Id = 996,
                QuestionText = "The birthstone for those born in December is garnet.",
                Answer = false,
                AnswerText = "The birthstone for those born in December is turquoise.",
                Category = "General Knowledge"
            };
            questions.Add(q996);

            OXQuizMetadata q997 = new OXQuizMetadata()
            {
                Id = 997,
                QuestionText = "The blood pressure in the left hand and that in the right hand are the same.",
                Answer = false,
                AnswerText = "They are different by around 20mmHg.",
                Category = "Science"
            };
            questions.Add(q997);

            OXQuizMetadata q998 = new OXQuizMetadata()
            {
                Id = 998,
                QuestionText = "The Blue House is where the senate gathers and debates policy.",
                Answer = false,
                AnswerText = "It's the house of Congress.",
                Category = "Society"
            };
            questions.Add(q998);

            OXQuizMetadata q999 = new OXQuizMetadata()
            {
                Id = 999,
                QuestionText = "The boss monster of Frostheart is Griffina.",
                Answer = false,
                AnswerText = "It's Griffin.",
                Category = "MapleStory 2"
            };
            questions.Add(q999);

            OXQuizMetadata q1000 = new OXQuizMetadata()
            {
                Id = 1000,
                QuestionText = "The boss monster of Trinian Crossing is Griffina.",
                Answer = true,
                AnswerText = "It's Griffina.",
                Category = "MapleStory 2"
            };
            questions.Add(q1000);

            OXQuizMetadata q1001 = new OXQuizMetadata()
            {
                Id = 1001,
                QuestionText = "The boss monster of Twilight Moon Castle is Lernos.",
                Answer = true,
                AnswerText = "The boss monster of Twilight Moon Castle is Lernos.",
                Category = "MapleStory 2"
            };
            questions.Add(q1001);

            OXQuizMetadata q1002 = new OXQuizMetadata()
            {
                Id = 1002,
                QuestionText = "The boss monster of Twilite Moon Castle is Lernos.",
                Answer = false,
                AnswerText = "Lernos appears at the Twilight Moon Castle.",
                Category = "MapleStory 2"
            };
            questions.Add(q1002);

            OXQuizMetadata q1003 = new OXQuizMetadata()
            {
                Id = 1003,
                QuestionText = "The British Open is the oldest of the four major championships in professional golf.",
                Answer = true,
                AnswerText = "The British Open Championship was founded in 1860, the Masters Tournament in 1931, and the PGA Championship in 1916.",
                Category = "Culture/Art"
            };
            questions.Add(q1003);

            OXQuizMetadata q1004 = new OXQuizMetadata()
            {
                Id = 1004,
                QuestionText = "The cabinet system is the political system in which the governing party creates and maintains a cabinet.",
                Answer = true,
                AnswerText = "It was born in the 18th-century England after the Glorious Revolution, amid the fight between the parliament and the monarch.",
                Category = "Society"
            };
            questions.Add(q1004);

            OXQuizMetadata q1005 = new OXQuizMetadata()
            {
                Id = 1005,
                QuestionText = "The cabinet system originated with George Washington in the United States.",
                Answer = false,
                AnswerText = "The cabinet system of government originated in Great Britain in the late 17th and early 18th centuries.",
                Category = "Society"
            };
            questions.Add(q1005);

            OXQuizMetadata q1006 = new OXQuizMetadata()
            {
                Id = 1006,
                QuestionText = "The Cairo Conference gave birth to the Cold War.",
                Answer = false,
                AnswerText = "The Yalta Conference gave birth to the Cold War.",
                Category = "Society"
            };
            questions.Add(q1006);

            OXQuizMetadata q1007 = new OXQuizMetadata()
            {
                Id = 1007,
                QuestionText = "The cantata is the name of a lyrical musical piece sung by an opera's main character.",
                Answer = false,
                AnswerText = "The aria is the name of a lyrical musical piece sung by an opera's main character.",
                Category = "Culture/Art"
            };
            questions.Add(q1007);

            OXQuizMetadata q1008 = new OXQuizMetadata()
            {
                Id = 1008,
                QuestionText = "The caste system is a class structure in India.",
                Answer = true,
                AnswerText = "The caste system is a class structure in India.",
                Category = "Culture/Art"
            };
            questions.Add(q1008);

            OXQuizMetadata q1009 = new OXQuizMetadata()
            {
                Id = 1009,
                QuestionText = "The caste system is a judicial system in India.",
                Answer = false,
                AnswerText = "The caste system is a class structure in India.",
                Category = "Culture/Art"
            };
            questions.Add(q1009);

            OXQuizMetadata q1010 = new OXQuizMetadata()
            {
                Id = 1010,
                QuestionText = "The cello is an abbreviation of the violoncello.",
                Answer = false,
                AnswerText = "The cello is an abbreviation of the violoncello.",
                Category = "Culture/Art"
            };
            questions.Add(q1010);

            OXQuizMetadata q1011 = new OXQuizMetadata()
            {
                Id = 1011,
                QuestionText = "The character specializing in the fire, ice, and lightning schools of magic is the Priest.",
                Answer = false,
                AnswerText = "The character specializing in the fire, ice, and lightning schools of magic is the Wizard.",
                Category = "MapleStory 2"
            };
            questions.Add(q1011);

            OXQuizMetadata q1012 = new OXQuizMetadata()
            {
                Id = 1012,
                QuestionText = "The clash between France's border policy and Russia's policy to move South caused the Fashoda Incident.",
                Answer = false,
                AnswerText = "The Fashoda Incident or Crisis was the climax of imperial territorial disputes between Britain and France in Eastern Africa, occurring in 1898 in Fashoda.",
                Category = "General Knowledge"
            };
            questions.Add(q1012);

            OXQuizMetadata q1013 = new OXQuizMetadata()
            {
                Id = 1013,
                QuestionText = "The closer the light source is to an object, the smaller the size of its shadow.",
                Answer = false,
                AnswerText = "The closer the light source is to an object, the larger its shadow gets.",
                Category = "Science"
            };
            questions.Add(q1013);

            OXQuizMetadata q1014 = new OXQuizMetadata()
            {
                Id = 1014,
                QuestionText = "The color black reflects light.",
                Answer = false,
                AnswerText = "Black color absorbs light.",
                Category = "General Knowledge"
            };
            questions.Add(q1014);

            OXQuizMetadata q1015 = new OXQuizMetadata()
            {
                Id = 1015,
                QuestionText = "The countries that started the Second World War were Germany, Austria, and Japan.",
                Answer = false,
                AnswerText = "The Axis is Germany, Italy, and Japan.",
                Category = "History"
            };
            questions.Add(q1015);

            OXQuizMetadata q1016 = new OXQuizMetadata()
            {
                Id = 1016,
                QuestionText = "The Cream War is the name of the war that Nightingale took part in.",
                Answer = false,
                AnswerText = "It's the Crimean War.",
                Category = "History"
            };
            questions.Add(q1016);

            OXQuizMetadata q1017 = new OXQuizMetadata()
            {
                Id = 1017,
                QuestionText = "The creature featured in the movie Jaws is a rabbit.",
                Answer = false,
                AnswerText = "The movie is about the great white shark.",
                Category = "Culture/Art"
            };
            questions.Add(q1017);

            OXQuizMetadata q1018 = new OXQuizMetadata()
            {
                Id = 1018,
                QuestionText = "The Dark Knight is a character in World of Warcraft.",
                Answer = false,
                AnswerText = "The Dark Knight is a movie from Christopher Nolan about Batman.",
                Category = "Culture/Art"
            };
            questions.Add(q1018);

            OXQuizMetadata q1019 = new OXQuizMetadata()
            {
                Id = 1019,
                QuestionText = "The Declaration of the Rights of Man and of Citizens is one of the three British constitutional documents.",
                Answer = false,
                AnswerText = "The three British constitutional documents are the Bill of Rights, the Petition of Right, and the Magna Carta.",
                Category = "History"
            };
            questions.Add(q1019);

            OXQuizMetadata q1020 = new OXQuizMetadata()
            {
                Id = 1020,
                QuestionText = "The default hotkey for Crawl is the period key (.).",
                Answer = true,
                AnswerText = "The default hotkey for Crawl is the period key (.), and the hotkey for Walk is the comma key (,).",
                Category = "MapleStory 2"
            };
            questions.Add(q1020);

            OXQuizMetadata q1021 = new OXQuizMetadata()
            {
                Id = 1021,
                QuestionText = "The default hotkey to maximize the Minimap is the Tab key.",
                Answer = true,
                AnswerText = "The default hotkey to maximize the Minimap is the Tab key.",
                Category = "MapleStory 2"
            };
            questions.Add(q1021);

            OXQuizMetadata q1022 = new OXQuizMetadata()
            {
                Id = 1022,
                QuestionText = "The destination of \"The Way of St. James\" is Jerusalem.",
                Answer = false,
                AnswerText = "Spain's Cathedral of Santiago de Compostela is at the end of the way.",
                Category = "Culture/Art"
            };
            questions.Add(q1022);

            OXQuizMetadata q1023 = new OXQuizMetadata()
            {
                Id = 1023,
                QuestionText = "The distance light travels in a year is about 9.46 billion km.",
                Answer = false,
                AnswerText = "A light year is approximately 10 trillion km.",
                Category = "General Knowledge"
            };
            questions.Add(q1023);

            OXQuizMetadata q1024 = new OXQuizMetadata()
            {
                Id = 1024,
                QuestionText = "The East End, along with New York's Broadway, is an area known for top-notch theater performances.",
                Answer = false,
                AnswerText = "The West End is the name of the street that is packed with Victorian style theaters.",
                Category = "Culture/Art"
            };
            questions.Add(q1024);

            OXQuizMetadata q1025 = new OXQuizMetadata()
            {
                Id = 1025,
                QuestionText = "The Eiffel Tower was built in 1945 to celebrate the end of World War 2.",
                Answer = false,
                AnswerText = "The Eiffel Tower was built in 1889.",
                Category = "General Knowledge"
            };
            questions.Add(q1025);

            OXQuizMetadata q1026 = new OXQuizMetadata()
            {
                Id = 1026,
                QuestionText = "The Elite monster appearing in Ludition Carnival is Pierre de Carro.",
                Answer = false,
                AnswerText = "The Elite monster appearing in Ludition Carnival is Monty de Carro.",
                Category = "MapleStory 2"
            };
            questions.Add(q1026);

            OXQuizMetadata q1027 = new OXQuizMetadata()
            {
                Id = 1027,
                QuestionText = "The Elite monster appearing in Torhara Spring is Dark Girant.",
                Answer = true,
                AnswerText = "The Elite monster appearing in Torhara Spring is Dark Girant.",
                Category = "MapleStory 2"
            };
            questions.Add(q1027);

            OXQuizMetadata q1028 = new OXQuizMetadata()
            {
                Id = 1028,
                QuestionText = "The elite monsters of Fairy Tree Lake are Nyxie and Epi.",
                Answer = true,
                AnswerText = "The elite monsters of Fairy Tree Lake are Nyxie and Epi.",
                Category = "MapleStory 2"
            };
            questions.Add(q1028);

            OXQuizMetadata q1029 = new OXQuizMetadata()
            {
                Id = 1029,
                QuestionText = "The elite monsters of Fairy Tree Lake are Pixie and Epi.",
                Answer = false,
                AnswerText = "The elite monsters of Fairy Tree Lake are Nyxie and Epi.",
                Category = "MapleStory 2"
            };
            questions.Add(q1029);

            OXQuizMetadata q1030 = new OXQuizMetadata()
            {
                Id = 1030,
                QuestionText = "The emblem of Audi, a German car brand, symbolizes success, integrity, reputation, and victory.",
                Answer = false,
                AnswerText = "The Audi emblem each represent one of four car companies that banded together to create Audi's predecessor company.",
                Category = "General Knowledge"
            };
            questions.Add(q1030);

            OXQuizMetadata q1031 = new OXQuizMetadata()
            {
                Id = 1031,
                QuestionText = "The emblem of Mercedez Benz, a German car brand, symbolizes sky, land, and sea.",
                Answer = true,
                AnswerText = "Benz had intended on using its engines not just in cars, but also in planes and ships.",
                Category = "General Knowledge"
            };
            questions.Add(q1031);

            OXQuizMetadata q1032 = new OXQuizMetadata()
            {
                Id = 1032,
                QuestionText = "The Emmy Awards, widely regarded as the Academy Awards for live theater performances, recognizes excellence in live Broadway theater.",
                Answer = false,
                AnswerText = "Tony Awards awards the plays that are performed in live Broadway theaters.",
                Category = "Culture/Art"
            };
            questions.Add(q1032);

            OXQuizMetadata q1033 = new OXQuizMetadata()
            {
                Id = 1033,
                QuestionText = "The entrance to the Twinkling Path dungeon is in Orne Town.",
                Answer = true,
                AnswerText = "The entrance to the Twinkling Path dungeon is in southern Orne Town.",
                Category = "MapleStory 2"
            };
            questions.Add(q1033);

            OXQuizMetadata q1034 = new OXQuizMetadata()
            {
                Id = 1034,
                QuestionText = "The experts rated the cucumber as the healthiest food of the 21st century.",
                Answer = false,
                AnswerText = "The experts rated the tomato as the healthiest food of the 21st century.",
                Category = "General Knowledge"
            };
            questions.Add(q1034);

            OXQuizMetadata q1035 = new OXQuizMetadata()
            {
                Id = 1035,
                QuestionText = "The field boss at Fractured Canyon is Pekanos.",
                Answer = true,
                AnswerText = "The field boss of Fractured Canyon is Pekanos.",
                Category = "MapleStory 2"
            };
            questions.Add(q1035);

            OXQuizMetadata q1036 = new OXQuizMetadata()
            {
                Id = 1036,
                QuestionText = "The field boss of Precipice Fortress is called the Vayar Warden.",
                Answer = false,
                AnswerText = "The field boss of Precipice Fortress is called the Vayar Gatekeeper.",
                Category = "MapleStory 2"
            };
            questions.Add(q1036);

            OXQuizMetadata q1037 = new OXQuizMetadata()
            {
                Id = 1037,
                QuestionText = "The final Boss monster in the Watchtower Rampart dungeon is King Tabo.",
                Answer = true,
                AnswerText = "The final Boss monster in the Watchtower Rampart dungeon is King Tabo.",
                Category = "MapleStory 2"
            };
            questions.Add(q1037);

            OXQuizMetadata q1038 = new OXQuizMetadata()
            {
                Id = 1038,
                QuestionText = "The first city to hold the Olympics was Athens and the first city to hold the Asian Games was New Delhi.",
                Answer = true,
                AnswerText = "The first city to hold the Olympics was Athens and the first city to hold the Asian Games was New Delhi.",
                Category = "Culture/Art"
            };
            questions.Add(q1038);

            OXQuizMetadata q1039 = new OXQuizMetadata()
            {
                Id = 1039,
                QuestionText = "The first city to hold the Olympics was Athens, and the first city to hold Asian Games was Bangkok.",
                Answer = false,
                AnswerText = "The first city to hold the Olympics was Athens and the first city to hold the Asian Games was New Delhi.",
                Category = "Culture/Art"
            };
            questions.Add(q1039);

            OXQuizMetadata q1040 = new OXQuizMetadata()
            {
                Id = 1040,
                QuestionText = "The first country to win the World Cup was Brazil.",
                Answer = false,
                AnswerText = "Uruguay is the first country to both host and win the World Cup.",
                Category = "Culture/Art"
            };
            questions.Add(q1040);

            OXQuizMetadata q1041 = new OXQuizMetadata()
            {
                Id = 1041,
                QuestionText = "The first country to win the World Cup was Paraguay.",
                Answer = false,
                AnswerText = "Uruguay is the first country to both host and win the World Cup.",
                Category = "Culture/Art"
            };
            questions.Add(q1041);

            OXQuizMetadata q1042 = new OXQuizMetadata()
            {
                Id = 1042,
                QuestionText = "The first FIFA Women's World Cup was held in the United States.",
                Answer = false,
                AnswerText = "The first FIFA Women's World Cup was held in China.",
                Category = "Culture/Art"
            };
            questions.Add(q1042);

            OXQuizMetadata q1043 = new OXQuizMetadata()
            {
                Id = 1043,
                QuestionText = "The first president of United States is George Washington, the second is John Adams, and the third is Thomas Jefferson.",
                Answer = true,
                AnswerText = "The first president of United States is George Washington, the second is John Adams, and the third is Thomas Jefferson.",
                Category = "General Knowledge"
            };
            questions.Add(q1043);

            OXQuizMetadata q1044 = new OXQuizMetadata()
            {
                Id = 1044,
                QuestionText = "The first president of United States is George Washington, the second is Thomas Jefferson, and the third is John Adams.",
                Answer = false,
                AnswerText = "The first president of United States is George Washington, the second is John Adams, and the third is Thomas Jefferson.",
                Category = "General Knowledge"
            };
            questions.Add(q1044);

            OXQuizMetadata q1045 = new OXQuizMetadata()
            {
                Id = 1045,
                QuestionText = "The first Star Wars movie, A New Hope (1977), is the fourth episode of the Star Wars saga.",
                Answer = true,
                AnswerText = "Star Wars: A New Hope (1977) is the fourth episode.",
                Category = "Culture/Art"
            };
            questions.Add(q1045);

            OXQuizMetadata q1046 = new OXQuizMetadata()
            {
                Id = 1046,
                QuestionText = "The first voice actor for Mickey Mouse was Walt Disney.",
                Answer = true,
                AnswerText = "The producer, Walt Disney, acted out Mickey Mouse's voice himself.",
                Category = "Culture/Art"
            };
            questions.Add(q1046);

            OXQuizMetadata q1047 = new OXQuizMetadata()
            {
                Id = 1047,
                QuestionText = "The first Winter Olympics were held in Chamonix, France.",
                Answer = true,
                AnswerText = "In 1924, the first Winter Olympics were held in Chamonix, France.",
                Category = "History"
            };
            questions.Add(q1047);

            OXQuizMetadata q1048 = new OXQuizMetadata()
            {
                Id = 1048,
                QuestionText = "The flavoring ingredient in cola is from the cola tree.",
                Answer = true,
                AnswerText = "In the late 19th century, John Pemberton, an American inventor, began using the extracts from cola tree leaves and seeds.",
                Category = "General Knowledge"
            };
            questions.Add(q1048);

            OXQuizMetadata q1049 = new OXQuizMetadata()
            {
                Id = 1049,
                QuestionText = "The Forgotten Vayar can be found in the Baraska Ice Cave.",
                Answer = true,
                AnswerText = "The Forgotten Vayar can be found in the Baraska Ice Cave.",
                Category = "MapleStory 2"
            };
            questions.Add(q1049);

            OXQuizMetadata q1050 = new OXQuizMetadata()
            {
                Id = 1050,
                QuestionText = "The four great tragedies of Shakespeare are Hamlet, Othello, King Lear, and Twelfth Night.",
                Answer = false,
                AnswerText = "Twelfth Night is a comedy.",
                Category = "Culture/Art"
            };
            questions.Add(q1050);

            OXQuizMetadata q1051 = new OXQuizMetadata()
            {
                Id = 1051,
                QuestionText = "The Four Kings, a group of four formidable enemies, lie in wait on the 1st underground floor of the Golden Tower.",
                Answer = false,
                AnswerText = "The Four Kings are on the 2nd underground floor of the tower.",
                Category = "MapleStory 2"
            };
            questions.Add(q1051);

            OXQuizMetadata q1052 = new OXQuizMetadata()
            {
                Id = 1052,
                QuestionText = "The French Revolution, Puritan Revolution, and Xinhai Revolution are the three modern-day democratic revolutions.",
                Answer = false,
                AnswerText = "The French Revolution, Puritan Revolution, and American Revolution are the three modern day democratic revolutions.",
                Category = "History"
            };
            questions.Add(q1052);

            OXQuizMetadata q1053 = new OXQuizMetadata()
            {
                Id = 1053,
                QuestionText = "The Ganges River flows through the Iranian Plateau.",
                Answer = false,
                AnswerText = "The Ganges River flows through India.",
                Category = "General Knowledge"
            };
            questions.Add(q1053);

            OXQuizMetadata q1054 = new OXQuizMetadata()
            {
                Id = 1054,
                QuestionText = "The Glorious Revolution in Great Britain resulted in the creation of Bill of Rights.",
                Answer = true,
                AnswerText = "During the Glorious Revolution, the Tories and the Whigs joined forces and overthrew King James II of England.",
                Category = "History"
            };
            questions.Add(q1054);

            OXQuizMetadata q1055 = new OXQuizMetadata()
            {
                Id = 1055,
                QuestionText = "The Glorious Revolution in Great Britain resulted in the creation of the Fill of Rights.",
                Answer = false,
                AnswerText = "During the Glorious Revolution, the Tories and the Whigs overthrew King James II of England and created the Bill of Rights.",
                Category = "History"
            };
            questions.Add(q1055);

            OXQuizMetadata q1056 = new OXQuizMetadata()
            {
                Id = 1056,
                QuestionText = "The Golden Tower is a dungeon accessible from Kerning City.",
                Answer = false,
                AnswerText = "The Golden Tower is accessible from Kerning Interchange.",
                Category = "MapleStory 2"
            };
            questions.Add(q1056);

            OXQuizMetadata q1057 = new OXQuizMetadata()
            {
                Id = 1057,
                QuestionText = "The gravitational attraction of the moon causes waves.",
                Answer = false,
                AnswerText = "The gravitational attraction of the moon causes the tides while the wind creates waves.",
                Category = "Science"
            };
            questions.Add(q1057);

            OXQuizMetadata q1058 = new OXQuizMetadata()
            {
                Id = 1058,
                QuestionText = "The Great Wall is in Japan.",
                Answer = false,
                AnswerText = "The Great Wall is in China.",
                Category = "General Knowledge"
            };
            questions.Add(q1058);

            OXQuizMetadata q1059 = new OXQuizMetadata()
            {
                Id = 1059,
                QuestionText = "The griffins can be found in Horus's Nest.",
                Answer = true,
                AnswerText = "The griffins can be found in Horus's Nest. Horus is their leader.",
                Category = "MapleStory 2"
            };
            questions.Add(q1059);

            OXQuizMetadata q1060 = new OXQuizMetadata()
            {
                Id = 1060,
                QuestionText = "The Guggenheim Museum is a renowned New York structure that looks like a white ribbon wrapped in circles.",
                Answer = true,
                AnswerText = "On its opening day, its striking design aroused both accolades and criticisms.",
                Category = "Culture/Art"
            };
            questions.Add(q1060);

            OXQuizMetadata q1061 = new OXQuizMetadata()
            {
                Id = 1061,
                QuestionText = "The Guggenheim Museum is a renowned Paris structure that looks like a white ribbon wrapped in circles.",
                Answer = false,
                AnswerText = "The Guggenheim Museum in New York has garnered both praise and criticism for its striking appearance.",
                Category = "Culture/Art"
            };
            questions.Add(q1061);

            OXQuizMetadata q1062 = new OXQuizMetadata()
            {
                Id = 1062,
                QuestionText = "The headquarters of International Olympic Committee is in Geneva.",
                Answer = false,
                AnswerText = "The headquarters of the International Olympic Committee is in Lausanne, Switzerland.",
                Category = "General Knowledge"
            };
            questions.Add(q1062);

            OXQuizMetadata q1063 = new OXQuizMetadata()
            {
                Id = 1063,
                QuestionText = "The headquarters of the Barrota Trading Company, the union of merchants, is located at Barrota Shore Landfill.",
                Answer = false,
                AnswerText = "The headquarters of the Barrota Trading Company is located in Barrota Trading Port.",
                Category = "MapleStory 2"
            };
            questions.Add(q1063);

            OXQuizMetadata q1064 = new OXQuizMetadata()
            {
                Id = 1064,
                QuestionText = "The Helium Balloon item lasts for 3 minutes.",
                Answer = true,
                AnswerText = "The Helium Balloon item lasts for 3 minutes.",
                Category = "MapleStory 2"
            };
            questions.Add(q1064);

            OXQuizMetadata q1065 = new OXQuizMetadata()
            {
                Id = 1065,
                QuestionText = "The Henesys Clock Tower and the Forest of Life play the same BGM.",
                Answer = true,
                AnswerText = "The Henesys Clock Tower and the Forest of Life play the same BGM.",
                Category = "MapleStory 2"
            };
            questions.Add(q1065);

            OXQuizMetadata q1066 = new OXQuizMetadata()
            {
                Id = 1066,
                QuestionText = "The high your Closeness with an Assistant, the greater the variety of items he or she can create.",
                Answer = true,
                AnswerText = "The high your Closeness with an Assistant, the greater the variety of items he or she can create.",
                Category = "MapleStory 2"
            };
            questions.Add(q1066);

            OXQuizMetadata q1067 = new OXQuizMetadata()
            {
                Id = 1067,
                QuestionText = "The higher the sun's altitude, the shorter the shadows it casts.",
                Answer = true,
                AnswerText = "The higher the sun's altitude, the shorter the shadows it casts.",
                Category = "Science"
            };
            questions.Add(q1067);

            OXQuizMetadata q1068 = new OXQuizMetadata()
            {
                Id = 1068,
                QuestionText = "The Hollywood movie \"Godzilla\" is the original film.",
                Answer = false,
                AnswerText = "Originally published in Japan in 1954, the movie was remade by Hollywood.",
                Category = "Culture/Art"
            };
            questions.Add(q1068);

            OXQuizMetadata q1069 = new OXQuizMetadata()
            {
                Id = 1069,
                QuestionText = "The human brain can feel pain.",
                Answer = false,
                AnswerText = "The human brain cannot feel pain. Muscles surrounding the brain causes headaches.",
                Category = "General Knowledge"
            };
            questions.Add(q1069);

            OXQuizMetadata q1070 = new OXQuizMetadata()
            {
                Id = 1070,
                QuestionText = "The humiliation of Canossa refers to King Henry IV's loss to Pope Gregory VII.",
                Answer = true,
                AnswerText = "The humiliation of Canossa refers specifically to King Henry IV's plea for forgiveness to Pope Gregory VII.",
                Category = "History"
            };
            questions.Add(q1070);

            OXQuizMetadata q1071 = new OXQuizMetadata()
            {
                Id = 1071,
                QuestionText = "The humiliation of Canossa refers to King Henry XIV's loss to Pope Gregory VII.",
                Answer = false,
                AnswerText = "The humiliation of Canossa refers to King Henry IV's plea for forgiveness from Pope Gregory VII.",
                Category = "History"
            };
            questions.Add(q1071);

            OXQuizMetadata q1072 = new OXQuizMetadata()
            {
                Id = 1072,
                QuestionText = "The IDC is an organization that decides where to hold the Olympics.",
                Answer = false,
                AnswerText = "IOC is an organization that makes decisions for the operations of the Olympics. It decides where to host the Olympics and the official sports and programs, etc.",
                Category = "History"
            };
            questions.Add(q1072);

            OXQuizMetadata q1073 = new OXQuizMetadata()
            {
                Id = 1073,
                QuestionText = "The Indus River is the cradle of the Egyptian civilization.",
                Answer = false,
                AnswerText = "The Nile River is the cradle of the Egyptian civilization.",
                Category = "History"
            };
            questions.Add(q1073);

            OXQuizMetadata q1074 = new OXQuizMetadata()
            {
                Id = 1074,
                QuestionText = "The International Atomic Energy Agency's acronym is IAEA.",
                Answer = true,
                AnswerText = "International Atomic Energy Agency is IAEA.",
                Category = "Society"
            };
            questions.Add(q1074);

            OXQuizMetadata q1075 = new OXQuizMetadata()
            {
                Id = 1075,
                QuestionText = "The International Atomic Energy Agency's acronym is LAEA.",
                Answer = false,
                AnswerText = "International Atomic Energy Agency's acronym is IAEA.",
                Category = "Society"
            };
            questions.Add(q1075);

            OXQuizMetadata q1076 = new OXQuizMetadata()
            {
                Id = 1076,
                QuestionText = "The International Chopin Piano Competition first started in 1927, and its first winner was a Russian pianist.",
                Answer = true,
                AnswerText = "Lev Nikolayevich Oborin, a Russian pianist, was the winner of the first International Chopin Piano Competition in 1927.",
                Category = "Culture/Art"
            };
            questions.Add(q1076);

            OXQuizMetadata q1077 = new OXQuizMetadata()
            {
                Id = 1077,
                QuestionText = "The International Court of Justice is located in London.",
                Answer = false,
                AnswerText = "The International Court of Justice is located at the Hague in the Netherlands.",
                Category = "General Knowledge"
            };
            questions.Add(q1077);

            OXQuizMetadata q1078 = new OXQuizMetadata()
            {
                Id = 1078,
                QuestionText = "The International Press Institute's acronym is FIFA.",
                Answer = false,
                AnswerText = "Federation Internationale de Football Association's acronym is FIFA.",
                Category = "General Knowledge"
            };
            questions.Add(q1078);

            OXQuizMetadata q1079 = new OXQuizMetadata()
            {
                Id = 1079,
                QuestionText = "The International Table Tennis Federation banned the use of speed glue because they worry about the health of table tennis players.",
                Answer = true,
                AnswerText = "Speed glue contains toxic solvents.",
                Category = "Culture/Art"
            };
            questions.Add(q1079);

            OXQuizMetadata q1080 = new OXQuizMetadata()
            {
                Id = 1080,
                QuestionText = "The IOC is an organization that decides where to hold the Olympics.",
                Answer = true,
                AnswerText = "IOC is an organization that makes decisions for the operations of the Olympics. It decides where to host the Olympics and the official sports and programs, etc.",
                Category = "General Knowledge"
            };
            questions.Add(q1080);

            OXQuizMetadata q1081 = new OXQuizMetadata()
            {
                Id = 1081,
                QuestionText = "The items that change the appearances of other items are called Skins.",
                Answer = true,
                AnswerText = "Skin items change the appearances of your items displayed on the screen.",
                Category = "MapleStory 2"
            };
            questions.Add(q1081);

            OXQuizMetadata q1082 = new OXQuizMetadata()
            {
                Id = 1082,
                QuestionText = "The Japanese invasion of Korea lasted for 6 years.",
                Answer = true,
                AnswerText = "The Japanese invasion of Korea lasted for 6 years.",
                Category = "History"
            };
            questions.Add(q1082);

            OXQuizMetadata q1083 = new OXQuizMetadata()
            {
                Id = 1083,
                QuestionText = "The July Revolution occurred in Russia.",
                Answer = false,
                AnswerText = "The July Revolution occurred in France.",
                Category = "History"
            };
            questions.Add(q1083);

            OXQuizMetadata q1084 = new OXQuizMetadata()
            {
                Id = 1084,
                QuestionText = "The language most used in terms of population is English.",
                Answer = false,
                AnswerText = "The most used language by population is Chinese. English is the most widely used language.",
                Category = "Culture/Art"
            };
            questions.Add(q1084);

            OXQuizMetadata q1085 = new OXQuizMetadata()
            {
                Id = 1085,
                QuestionText = "The Last Judgement is a fresco by Leonardo Da Vinci.",
                Answer = false,
                AnswerText = "The Last Judgement, which is painted in the Sistine Chapel, is a fresco by Michelangelo.",
                Category = "General Knowledge"
            };
            questions.Add(q1085);

            OXQuizMetadata q1086 = new OXQuizMetadata()
            {
                Id = 1086,
                QuestionText = "The Last Judgement is a fresco by Michelangelo.",
                Answer = true,
                AnswerText = "The Last Judgement, which is painted in the Sistine Chapel, is a fresco by Michelangelo.",
                Category = "General Knowledge"
            };
            questions.Add(q1086);

            OXQuizMetadata q1087 = new OXQuizMetadata()
            {
                Id = 1087,
                QuestionText = "The last letter in the Greek alphabet is gamma.",
                Answer = false,
                AnswerText = "The last letter in the Greek alphabet is omega.",
                Category = "General Knowledge"
            };
            questions.Add(q1087);

            OXQuizMetadata q1088 = new OXQuizMetadata()
            {
                Id = 1088,
                QuestionText = "The leader of the Autobots is named Bumblebee.",
                Answer = false,
                AnswerText = "The leader of the Autobots is named Optimus Prime.",
                Category = "Culture/Art"
            };
            questions.Add(q1088);

            OXQuizMetadata q1089 = new OXQuizMetadata()
            {
                Id = 1089,
                QuestionText = "The leader of the Henesys-based vigilante group, the Green Hoods, is Oska.",
                Answer = true,
                AnswerText = "The current leader of the Green Hoods is Oska.",
                Category = "MapleStory 2"
            };
            questions.Add(q1089);

            OXQuizMetadata q1090 = new OXQuizMetadata()
            {
                Id = 1090,
                QuestionText = "The least number of bits needed to express a decimal digit is 8 bits.",
                Answer = false,
                AnswerText = "The least number of bits needed to express a decimal digit is 4 bits.",
                Category = "IT"
            };
            questions.Add(q1090);

            OXQuizMetadata q1091 = new OXQuizMetadata()
            {
                Id = 1091,
                QuestionText = "The letters \"BMV\" are initials of Beethoven's composition number.",
                Answer = false,
                AnswerText = "The letters \"BMV\" are initials of Johann Sebastian Bach's composition number.",
                Category = "Culture/Art"
            };
            questions.Add(q1091);

            OXQuizMetadata q1092 = new OXQuizMetadata()
            {
                Id = 1092,
                QuestionText = "The Lost Vayar Guardian is a Divine monster.",
                Answer = false,
                AnswerText = "The Lost Vayar Guardian is a Spirit monster.",
                Category = "MapleStory 2"
            };
            questions.Add(q1092);

            OXQuizMetadata q1093 = new OXQuizMetadata()
            {
                Id = 1093,
                QuestionText = "The lowest-leveled Elite monster in MapleStory 2 is Urza.",
                Answer = false,
                AnswerText = "Turtcoli is the lowest-leveled Elite monster.",
                Category = "MapleStory 2"
            };
            questions.Add(q1093);

            OXQuizMetadata q1094 = new OXQuizMetadata()
            {
                Id = 1094,
                QuestionText = "The lowest-leveled field boss monster is Dondon.",
                Answer = false,
                AnswerText = "The lowest-leveled field boss monster is Doondun.",
                Category = "MapleStory 2"
            };
            questions.Add(q1094);

            OXQuizMetadata q1095 = new OXQuizMetadata()
            {
                Id = 1095,
                QuestionText = "The lowest-leveled field boss monster is Doondun.",
                Answer = true,
                AnswerText = "The lowest-leveled field boss monster is Doondun.",
                Category = "MapleStory 2"
            };
            questions.Add(q1095);

            OXQuizMetadata q1096 = new OXQuizMetadata()
            {
                Id = 1096,
                QuestionText = "The lunar calendar was first developed in Egyptian civilization.",
                Answer = false,
                AnswerText = "The lunar calendar was first developed in Mesopotamia.",
                Category = "History"
            };
            questions.Add(q1096);

            OXQuizMetadata q1097 = new OXQuizMetadata()
            {
                Id = 1097,
                QuestionText = "The main instrument used for the Calibre Island BGM is piano.",
                Answer = false,
                AnswerText = "It's the Uilleann pipes, an Irish musical instrument.",
                Category = "MapleStory 2"
            };
            questions.Add(q1097);

            OXQuizMetadata q1098 = new OXQuizMetadata()
            {
                Id = 1098,
                QuestionText = "The Maori people stick out their tongues to greet strangers.",
                Answer = false,
                AnswerText = "The Maoris stick out their tongues to intimidate their enemies.",
                Category = "Culture/Art"
            };
            questions.Add(q1098);

            OXQuizMetadata q1099 = new OXQuizMetadata()
            {
                Id = 1099,
                QuestionText = "The maximum capacity for parties is 10.",
                Answer = true,
                AnswerText = "Each party can contain up to 10 players.",
                Category = "MapleStory 2"
            };
            questions.Add(q1099);

            OXQuizMetadata q1100 = new OXQuizMetadata()
            {
                Id = 1100,
                QuestionText = "The melon is the fruit that got its name for resembling grapes.",
                Answer = false,
                AnswerText = "Grapefruits are the fruit that got their name because premature grapefruits looks similar in shape to unripe green grapes.",
                Category = "General Knowledge"
            };
            questions.Add(q1100);

            OXQuizMetadata q1101 = new OXQuizMetadata()
            {
                Id = 1101,
                QuestionText = "The members of the legendary rock band Beatles are Paul McCartney, John Lennon, George Harrison, and Chris Martin.",
                Answer = false,
                AnswerText = "Chris Martin is a member of Coldplay.",
                Category = "Culture/Art"
            };
            questions.Add(q1101);

            OXQuizMetadata q1102 = new OXQuizMetadata()
            {
                Id = 1102,
                QuestionText = "The microbiologist Marburg discovered the Ebola virus in the Nile River.",
                Answer = false,
                AnswerText = "The Ebola virus was discovered near the Ebola River.",
                Category = "Science"
            };
            questions.Add(q1102);

            OXQuizMetadata q1103 = new OXQuizMetadata()
            {
                Id = 1103,
                QuestionText = "The milk used in coffee has fat in it.",
                Answer = true,
                AnswerText = "All milk has fat in it.",
                Category = "General Knowledge"
            };
            questions.Add(q1103);

            OXQuizMetadata q1104 = new OXQuizMetadata()
            {
                Id = 1104,
                QuestionText = "The Millenium Falcon made the Kessel Run in 14 parsecs.",
                Answer = false,
                AnswerText = "The Millenium Falcon made the Kessel Run in less than 12 parsecs.",
                Category = "Culture/Art"
            };
            questions.Add(q1104);

            OXQuizMetadata q1105 = new OXQuizMetadata()
            {
                Id = 1105,
                QuestionText = "The Missouri River is the longest river in the USA.",
                Answer = true,
                AnswerText = "The Missouri River is longer than the Mississippi River.",
                Category = "Culture/Art"
            };
            questions.Add(q1105);

            OXQuizMetadata q1106 = new OXQuizMetadata()
            {
                Id = 1106,
                QuestionText = "The modern exonym \"Korea\" came from Goryeo.",
                Answer = true,
                AnswerText = "The trade relationship between Goryeo and Arabian merchants is where the West learned the current national name \"Korea.\"",
                Category = "General Knowledge"
            };
            questions.Add(q1106);

            OXQuizMetadata q1107 = new OXQuizMetadata()
            {
                Id = 1107,
                QuestionText = "The Molten Knight appears in Berrysweet Castle.",
                Answer = true,
                AnswerText = "The Molten Knight appears in Berrysweet Castle.",
                Category = "MapleStory 2"
            };
            questions.Add(q1107);

            OXQuizMetadata q1108 = new OXQuizMetadata()
            {
                Id = 1108,
                QuestionText = "The Molten Knight appears in Fort Macaroon.",
                Answer = false,
                AnswerText = "The Molten Knight appears in Berrysweet Castle.",
                Category = "MapleStory 2"
            };
            questions.Add(q1108);

            OXQuizMetadata q1109 = new OXQuizMetadata()
            {
                Id = 1109,
                QuestionText = "The moon generates light on its own.",
                Answer = false,
                AnswerText = "The moon merely reflects sunlight.",
                Category = "Science"
            };
            questions.Add(q1109);

            OXQuizMetadata q1110 = new OXQuizMetadata()
            {
                Id = 1110,
                QuestionText = "The moon is bigger than the sun.",
                Answer = false,
                AnswerText = "The moon appears larger than the sun because the former is much closer to the earth than the latter.",
                Category = "Science"
            };
            questions.Add(q1110);

            OXQuizMetadata q1111 = new OXQuizMetadata()
            {
                Id = 1111,
                QuestionText = "The mosquito's pre-metamorphosis form is called diptera larva.",
                Answer = false,
                AnswerText = "Mosquito larvae, which live underwater, are mosquito's pre-metamorphosis form.",
                Category = "General Knowledge"
            };
            questions.Add(q1111);

            OXQuizMetadata q1112 = new OXQuizMetadata()
            {
                Id = 1112,
                QuestionText = "The mosquito's pre-metamorphosis form is called larva.",
                Answer = true,
                AnswerText = "Mosquito larvae, which live underwater, are mosquito's pre-metamorphosis form.",
                Category = "General Knowledge"
            };
            questions.Add(q1112);

            OXQuizMetadata q1113 = new OXQuizMetadata()
            {
                Id = 1113,
                QuestionText = "The most fundamental principle in democracy is the rights of the individual.",
                Answer = true,
                AnswerText = "The most fundamental principle in democracy is the rights of the individual.",
                Category = "Society"
            };
            questions.Add(q1113);

            OXQuizMetadata q1114 = new OXQuizMetadata()
            {
                Id = 1114,
                QuestionText = "The mysterious creature in the basement of the Ludibrium Clock Tower is Papulatus.",
                Answer = true,
                AnswerText = "The mysterious creature in the basement of the Ludibrium Clock Tower is Papulatus.",
                Category = "MapleStory 2"
            };
            questions.Add(q1114);

            OXQuizMetadata q1115 = new OXQuizMetadata()
            {
                Id = 1115,
                QuestionText = "The mysterious creature in the basement of the Ludibrium Clock Tower is Popalatus.",
                Answer = false,
                AnswerText = "Papulatus is the correct name.",
                Category = "MapleStory 2."
            };
            questions.Add(q1115);

            OXQuizMetadata q1116 = new OXQuizMetadata()
            {
                Id = 1116,
                QuestionText = "The mysterious creature in the basement of the Ludibrium Clock Tower is Populatus.",
                Answer = false,
                AnswerText = "Papulatus is the correct name.",
                Category = "MapleStory 2"
            };
            questions.Add(q1116);

            OXQuizMetadata q1117 = new OXQuizMetadata()
            {
                Id = 1117,
                QuestionText = "The name \"Bridge of Sighs\" originated from the nearby dungeon's death chamber.",
                Answer = false,
                AnswerText = "The name \"Bridge of Sighs\" comes from the suggestion that prisoners would sigh at their final view of beautiful Venice through the window before being taken down to their cells.",
                Category = "General Knowledge"
            };
            questions.Add(q1117);

            OXQuizMetadata q1118 = new OXQuizMetadata()
            {
                Id = 1118,
                QuestionText = "The name of Henesys's Elder is Manovich.",
                Answer = true,
                AnswerText = "The name of Henesys's Elder is Manovich.",
                Category = "MapleStory 2"
            };
            questions.Add(q1118);

            OXQuizMetadata q1119 = new OXQuizMetadata()
            {
                Id = 1119,
                QuestionText = "The name of Kerning City's mayor is Arco.",
                Answer = false,
                AnswerText = "The name of Kerning City's mayor is Marco.",
                Category = "MapleStory 2"
            };
            questions.Add(q1119);

            OXQuizMetadata q1120 = new OXQuizMetadata()
            {
                Id = 1120,
                QuestionText = "The name of Mrs. Ibelin's son is Allon.",
                Answer = false,
                AnswerText = "The name of Mrs. Ibelin's son is Nelph.",
                Category = "MapleStory 2"
            };
            questions.Add(q1120);

            OXQuizMetadata q1121 = new OXQuizMetadata()
            {
                Id = 1121,
                QuestionText = "The name of the big flower in the center of the Toxic Garden is Nogarcia.",
                Answer = false,
                AnswerText = "The name of the big flower in the center of the Toxic Garden is Norcacias.",
                Category = "MapleStory 2"
            };
            questions.Add(q1121);

            OXQuizMetadata q1122 = new OXQuizMetadata()
            {
                Id = 1122,
                QuestionText = "The name of the buff awarded to the winners of the mini-game events is Ereve's Happiness.",
                Answer = false,
                AnswerText = "Blessing of Ereve is the correct name of the buff.",
                Category = "MapleStory 2"
            };
            questions.Add(q1122);

            OXQuizMetadata q1123 = new OXQuizMetadata()
            {
                Id = 1123,
                QuestionText = "The name of the city where Empress Ereve resides is Travia.",
                Answer = false,
                AnswerText = "Empress Ereve resides in Tria.",
                Category = "MapleStory 2"
            };
            questions.Add(q1123);

            OXQuizMetadata q1124 = new OXQuizMetadata()
            {
                Id = 1124,
                QuestionText = "The name of the deliveryman in Tria is Boogie.",
                Answer = false,
                AnswerText = "The name of the deliveryman in Tria is Bogie.",
                Category = "MapleStory 2"
            };
            questions.Add(q1124);

            OXQuizMetadata q1125 = new OXQuizMetadata()
            {
                Id = 1125,
                QuestionText = "The name of the Elite monster in Sweet Tooth Valley is Stump.",
                Answer = false,
                AnswerText = "The name of the Elite monster in Sweet Tooth Valley is Stumpy.",
                Category = "MapleStory 2"
            };
            questions.Add(q1125);

            OXQuizMetadata q1126 = new OXQuizMetadata()
            {
                Id = 1126,
                QuestionText = "The name of the merchant that sells items for Valor Tokens is Beautiful Arc.",
                Answer = true,
                AnswerText = "The name of the merchant that sells items for Valor is Beautiful Arc.",
                Category = "MapleStory 2"
            };
            questions.Add(q1126);

            OXQuizMetadata q1127 = new OXQuizMetadata()
            {
                Id = 1127,
                QuestionText = "The name of the person who enchants gear in MapleStory 2 is Ophelia.",
                Answer = true,
                AnswerText = "The name of the person who enchants gear in MapleStory 2 is Ophelia.",
                Category = "MapleStory 2"
            };
            questions.Add(q1127);

            OXQuizMetadata q1128 = new OXQuizMetadata()
            {
                Id = 1128,
                QuestionText = "The name of the tribe concerned about the princess abducted from Cherry Blossom Hill is Boroboro.",
                Answer = true,
                AnswerText = "The Boroboro fairfolk are worried about the abducted Princess Yomi.",
                Category = "MapleStory 2"
            };
            questions.Add(q1128);

            OXQuizMetadata q1129 = new OXQuizMetadata()
            {
                Id = 1129,
                QuestionText = "The name of the Trophy earned from Nyxie and Epi in Fairy Tree Lake is Twin Robbers.",
                Answer = false,
                AnswerText = "The name of the Trophy found from Nyxie and Epi in Fairy Tree Lake is Twin Thieves.",
                Category = "MapleStory 2"
            };
            questions.Add(q1129);

            OXQuizMetadata q1130 = new OXQuizMetadata()
            {
                Id = 1130,
                QuestionText = "The name of the violent bear appearing at Kernel Energy Research is Urska.",
                Answer = true,
                AnswerText = "Urska is the Elite monster appearing at Kernel Energy Research.",
                Category = "MapleStory 2"
            };
            questions.Add(q1130);

            OXQuizMetadata q1131 = new OXQuizMetadata()
            {
                Id = 1131,
                QuestionText = "The name Thursday comes from the Nordic god of the tree.",
                Answer = false,
                AnswerText = "The name Thursday comes from the Nordic god of thunder.",
                Category = "History"
            };
            questions.Add(q1131);

            OXQuizMetadata q1132 = new OXQuizMetadata()
            {
                Id = 1132,
                QuestionText = "The Neolithic Revolution is the framework behind the modern societal structure.",
                Answer = false,
                AnswerText = "The Industrial Revolution is what triggered the shift of modern society to how it is today.",
                Category = "History"
            };
            questions.Add(q1132);

            OXQuizMetadata q1133 = new OXQuizMetadata()
            {
                Id = 1133,
                QuestionText = "The North Pole has a lower average temperature than the South Pole.",
                Answer = false,
                AnswerText = "The South Pole is colder than the North Pole.",
                Category = "Science"
            };
            questions.Add(q1133);

            OXQuizMetadata q1134 = new OXQuizMetadata()
            {
                Id = 1134,
                QuestionText = "The northeasterly trade winds are hot and humid.",
                Answer = false,
                AnswerText = "The traits of northeasterly trade winds are dry, hot wind.",
                Category = "Science"
            };
            questions.Add(q1134);

            OXQuizMetadata q1135 = new OXQuizMetadata()
            {
                Id = 1135,
                QuestionText = "The NPCs that give Daily quests in Queenstown belong to the Alleyoop Merchant Society.",
                Answer = false,
                AnswerText = "The NPCs that give Daily quests in Queenstown belong to the Allicari Merchant Society.",
                Category = "MapleStory 2"
            };
            questions.Add(q1135);

            OXQuizMetadata q1136 = new OXQuizMetadata()
            {
                Id = 1136,
                QuestionText = "The number of children playing hide-and-seek on the Twinkling Path is 6.",
                Answer = true,
                AnswerText = "Merin was playing hide-and-seek with his friends when he ran into danger.",
                Category = "MapleStory 2"
            };
            questions.Add(q1136);

            OXQuizMetadata q1137 = new OXQuizMetadata()
            {
                Id = 1137,
                QuestionText = "The OECD refers to an Organization of the Petroleum Exporting Countries.",
                Answer = false,
                AnswerText = "The OECD, or Organization for Economic Co-operation and Development, is an organization that promotes economic growth, prosperity, and sustainable growth of economically less developed nations.",
                Category = "General Knowledge"
            };
            questions.Add(q1137);

            OXQuizMetadata q1138 = new OXQuizMetadata()
            {
                Id = 1138,
                QuestionText = "The official height of the men's volleyball net is 2.24 m.",
                Answer = false,
                AnswerText = "It's 2.43 m for men.",
                Category = "Culture/Art"
            };
            questions.Add(q1138);

            OXQuizMetadata q1139 = new OXQuizMetadata()
            {
                Id = 1139,
                QuestionText = "The Operation Hen Rescue is a dungeon accessible from Andrea Barony.",
                Answer = true,
                AnswerText = "The Operation Hen Rescue is a dungeon accessible from Andrea Barony.",
                Category = "MapleStory 2"
            };
            questions.Add(q1139);

            OXQuizMetadata q1140 = new OXQuizMetadata()
            {
                Id = 1140,
                QuestionText = "The Opium Wars granted Great Britain rule over Hong Kong.",
                Answer = true,
                AnswerText = "The Opium Wars granted Great Britain rule over Hong Kong.",
                Category = "History"
            };
            questions.Add(q1140);

            OXQuizMetadata q1141 = new OXQuizMetadata()
            {
                Id = 1141,
                QuestionText = "The opposite sides of dice always add up to 6.",
                Answer = false,
                AnswerText = "The opposite sides of dice always add up to 7.",
                Category = "General Knowledge"
            };
            questions.Add(q1141);

            OXQuizMetadata q1142 = new OXQuizMetadata()
            {
                Id = 1142,
                QuestionText = "The opposite sides of dice always add up to 7.",
                Answer = true,
                AnswerText = "The opposite sides of dice always add up to 7.",
                Category = "General Knowledge"
            };
            questions.Add(q1142);

            OXQuizMetadata q1143 = new OXQuizMetadata()
            {
                Id = 1143,
                QuestionText = "The Palace of Versailles is a prime example of Rococo style architecture.",
                Answer = false,
                AnswerText = "The Palace of Versailles is a prime example of Baroque style architecture.",
                Category = "Culture/Art"
            };
            questions.Add(q1143);

            OXQuizMetadata q1144 = new OXQuizMetadata()
            {
                Id = 1144,
                QuestionText = "The part of a potato most commonly eaten is its root.",
                Answer = false,
                AnswerText = "The part of a potato most commonly eaten is the tuber.",
                Category = "Science"
            };
            questions.Add(q1144);

            OXQuizMetadata q1145 = new OXQuizMetadata()
            {
                Id = 1145,
                QuestionText = "The part of a potato most commonly eaten is its stem.",
                Answer = true,
                AnswerText = "The part of a potato most commonly eaten is its tuber.",
                Category = "Science"
            };
            questions.Add(q1145);

            OXQuizMetadata q1146 = new OXQuizMetadata()
            {
                Id = 1146,
                QuestionText = "The Peace of Westphalia formally recognized Switzerland's and Austria's independence.",
                Answer = false,
                AnswerText = "The treaty granted sovereignty to Switzerland and the Netherlands.",
                Category = "History"
            };
            questions.Add(q1146);

            OXQuizMetadata q1147 = new OXQuizMetadata()
            {
                Id = 1147,
                QuestionText = "The Peace of Westphalia was the treaty signed because of the Thirty Years' War.",
                Answer = true,
                AnswerText = "It was signed in 1648. It accepted Lutheranism and Calvinism as a form of religion and granted sovereignty to Switzerland and Netherlands.",
                Category = "History"
            };
            questions.Add(q1147);

            OXQuizMetadata q1148 = new OXQuizMetadata()
            {
                Id = 1148,
                QuestionText = "The Peace of Worstphalia was the treaty signed because of the Thirty Years' War.",
                Answer = false,
                AnswerText = "The Peace of Westphalia was signed in 1648. It accepted Lutherans and Calvinists as a form of religion and granted sovereignty to Switzerland and the Netherlands.",
                Category = "History"
            };
            questions.Add(q1148);

            OXQuizMetadata q1149 = new OXQuizMetadata()
            {
                Id = 1149,
                QuestionText = "The Persistence of Memory is a painting by Rene Magritte.",
                Answer = false,
                AnswerText = "The Persistence of Memory is a painting by Salvador Dali.",
                Category = "Culture/Art"
            };
            questions.Add(q1149);

            OXQuizMetadata q1150 = new OXQuizMetadata()
            {
                Id = 1150,
                QuestionText = "The piano is not an abbreviation of the pianoforte.",
                Answer = false,
                AnswerText = "The piano is an abbreviation of the pianoforte.",
                Category = "Culture/Art"
            };
            questions.Add(q1150);

            OXQuizMetadata q1151 = new OXQuizMetadata()
            {
                Id = 1151,
                QuestionText = "The pig king that threatens the castle walls at the West Watchtower is called Pabo.",
                Answer = false,
                AnswerText = "The pig king that threatens the castle walls in West Watchtower in called Tabo.",
                Category = "MapleStory 2"
            };
            questions.Add(q1151);

            OXQuizMetadata q1152 = new OXQuizMetadata()
            {
                Id = 1152,
                QuestionText = "The practice of enclosure, a cause of the industrial revolution, was an economic trend. Wealthy farmers bought land from small farmers to realize economies of scale.",
                Answer = true,
                AnswerText = "European landlords used enclosure to turn large swath of land to profitable ventures, such as wool production. This practice endured from the end of the 15th century to the early 17th century.",
                Category = "History"
            };
            questions.Add(q1152);

            OXQuizMetadata q1153 = new OXQuizMetadata()
            {
                Id = 1153,
                QuestionText = "The Priest can summon a rabbit with the Celestial Guardian skill.",
                Answer = false,
                AnswerText = "The Priest can summon an angel with the Celestial Guardian skill.",
                Category = "MapleStory 2"
            };
            questions.Add(q1153);

            OXQuizMetadata q1154 = new OXQuizMetadata()
            {
                Id = 1154,
                QuestionText = "The Priest can use a skill to restore the spirit of their allies.",
                Answer = true,
                AnswerText = "Holy Symbol restores the spirit of allies.",
                Category = "MapleStory 2"
            };
            questions.Add(q1154);

            OXQuizMetadata q1155 = new OXQuizMetadata()
            {
                Id = 1155,
                QuestionText = "The Priest places their right knee on the ground to use the Heal skill.",
                Answer = false,
                AnswerText = "It's their left knee.",
                Category = "MapleStory 2"
            };
            questions.Add(q1155);

            OXQuizMetadata q1156 = new OXQuizMetadata()
            {
                Id = 1156,
                QuestionText = "The primary cause of the dispute that led to American Civil War was slavery.",
                Answer = true,
                AnswerText = "The North opposed slavery while the South advocated slavery. The two opposing views led to war.",
                Category = "History"
            };
            questions.Add(q1156);

            OXQuizMetadata q1157 = new OXQuizMetadata()
            {
                Id = 1157,
                QuestionText = "The primary colors of light are red, green, and white.",
                Answer = false,
                AnswerText = "The primary colors are red, green, and blue.",
                Category = "Culture/Art"
            };
            questions.Add(q1157);

            OXQuizMetadata q1158 = new OXQuizMetadata()
            {
                Id = 1158,
                QuestionText = "The Prism Falls are 12 meters tall.",
                Answer = false,
                AnswerText = "The falls are 15 meters tall.",
                Category = "MapleStory 2"
            };
            questions.Add(q1158);

            OXQuizMetadata q1159 = new OXQuizMetadata()
            {
                Id = 1159,
                QuestionText = "The programming language C grew out of B.",
                Answer = true,
                AnswerText = "The programming language C grew out of B.",
                Category = "IT"
            };
            questions.Add(q1159);

            OXQuizMetadata q1160 = new OXQuizMetadata()
            {
                Id = 1160,
                QuestionText = "The reason cranes stand on one leg at a time is to balance its center of gravity.",
                Answer = false,
                AnswerText = "The reason cranes stand on one leg at a time is to regulate its body temperature.",
                Category = "General Knowledge"
            };
            questions.Add(q1160);

            OXQuizMetadata q1161 = new OXQuizMetadata()
            {
                Id = 1161,
                QuestionText = "The recorder, a musical instrument, is a 20th-century invention.",
                Answer = false,
                AnswerText = "Recorders have been in use since the 11th century.",
                Category = "General Knowledge"
            };
            questions.Add(q1161);

            OXQuizMetadata q1162 = new OXQuizMetadata()
            {
                Id = 1162,
                QuestionText = "The Renaissance began in Italy.",
                Answer = true,
                AnswerText = "The Renaissance, which began in Italy during the 14th century, was a birth of new culture through the rebirth of Greek and Roman culture.",
                Category = "History"
            };
            questions.Add(q1162);

            OXQuizMetadata q1163 = new OXQuizMetadata()
            {
                Id = 1163,
                QuestionText = "The Renaissance began in Russia.",
                Answer = false,
                AnswerText = "Renaissance, which began in Italy during the 14th century, means a birth of new culture through the rebirth of Greek and Roman culture.",
                Category = "History"
            };
            questions.Add(q1163);

            OXQuizMetadata q1164 = new OXQuizMetadata()
            {
                Id = 1164,
                QuestionText = "The Renaissance was based on pragmatism.",
                Answer = false,
                AnswerText = "The Renaissance was based on humanism.",
                Category = "History"
            };
            questions.Add(q1164);

            OXQuizMetadata q1165 = new OXQuizMetadata()
            {
                Id = 1165,
                QuestionText = "The revolution of the Earth refers to the earth's revolving around the sun over the period of 1 year.",
                Answer = true,
                AnswerText = "The Earth takes one year to revolve around the sun. The process is called the revolution of the Earth.",
                Category = "Science"
            };
            questions.Add(q1165);

            OXQuizMetadata q1166 = new OXQuizMetadata()
            {
                Id = 1166,
                QuestionText = "The Rh blood group system doesn't follow Mendel's Laws.",
                Answer = false,
                AnswerText = "Mendelian inheritance is divided into three laws: the law of segregation of genes, the law of independent assortment, and the law of dominance. Mendelian inheritance can't explain the RH blood group system.",
                Category = "Science"
            };
            questions.Add(q1166);

            OXQuizMetadata q1167 = new OXQuizMetadata()
            {
                Id = 1167,
                QuestionText = "The Rh blood group system follows Mendel's Laws.",
                Answer = true,
                AnswerText = "Mendelian inheritance is divided into three laws: the law of segregation of genes, the law of independent assortment, and the law of dominance. Mendelian inheritance can't explain the RH blood group system.",
                Category = "Science"
            };
            questions.Add(q1167);

            OXQuizMetadata q1168 = new OXQuizMetadata()
            {
                Id = 1168,
                QuestionText = "The rise in currency decreases the volume of exports but increases profits.",
                Answer = false,
                AnswerText = "Devaluation increases the volume of exports, but it can also cause inflation by raising the price of imported goods.",
                Category = "General Knowledge"
            };
            questions.Add(q1168);

            OXQuizMetadata q1169 = new OXQuizMetadata()
            {
                Id = 1169,
                QuestionText = "The rise in currency increases the volume of exports but decreases profits.",
                Answer = true,
                AnswerText = "Devaluation increases the volume of exports, but it can also cause inflation by raising the price of imported goods.",
                Category = "General Knowledge"
            };
            questions.Add(q1169);

            OXQuizMetadata q1170 = new OXQuizMetadata()
            {
                Id = 1170,
                QuestionText = "The royal guards of Tria are identical siblings.",
                Answer = false,
                AnswerText = "The rumor about the royal guards of Tria being identical siblings is not true.",
                Category = "MapleStory 2"
            };
            questions.Add(q1170);

            OXQuizMetadata q1171 = new OXQuizMetadata()
            {
                Id = 1171,
                QuestionText = "The Sagrada Familia church in Barcelona is taking longer to build than the pyramids.",
                Answer = true,
                AnswerText = "The Sagrada Familia church started construction in 1882 and is still not finished.",
                Category = "General Knowledge"
            };
            questions.Add(q1171);

            OXQuizMetadata q1172 = new OXQuizMetadata()
            {
                Id = 1172,
                QuestionText = "The Sagrada Familia, a large Roman Catholic church in Barcelona, Spain, is a signature work of Spanish architect Antonio Gaudi.",
                Answer = true,
                AnswerText = "The Sagrada Familia, a large Roman Catholic church in Barcelona, Spain, is a signature work of Spanish architect Antonio Gaudi.",
                Category = "General Knowledge"
            };
            questions.Add(q1172);

            OXQuizMetadata q1173 = new OXQuizMetadata()
            {
                Id = 1173,
                QuestionText = "The same poles of a magnet attract each other.",
                Answer = false,
                AnswerText = "The opposites attract.",
                Category = "Science"
            };
            questions.Add(q1173);

            OXQuizMetadata q1174 = new OXQuizMetadata()
            {
                Id = 1174,
                QuestionText = "The saxophone is a brass instrument.",
                Answer = false,
                AnswerText = "The saxophone is a woodwind instrument.",
                Category = "Culture/Art"
            };
            questions.Add(q1174);

            OXQuizMetadata q1175 = new OXQuizMetadata()
            {
                Id = 1175,
                QuestionText = "The scent of bamboo is pleasant.",
                Answer = true,
                AnswerText = "Bamboo has a pleasant scent to it.",
                Category = "General Knowledge"
            };
            questions.Add(q1175);

            OXQuizMetadata q1176 = new OXQuizMetadata()
            {
                Id = 1176,
                QuestionText = "The Second World War resulted in British rule of Hong Kong.",
                Answer = false,
                AnswerText = "The Opium Wars led to Great Britain's rule over Hong Kong.",
                Category = "History"
            };
            questions.Add(q1176);

            OXQuizMetadata q1177 = new OXQuizMetadata()
            {
                Id = 1177,
                QuestionText = "The Silk Road is the road that connects the ancient civilization of the East and the West.",
                Answer = true,
                AnswerText = "The route is called Silk Road.",
                Category = "History"
            };
            questions.Add(q1177);

            OXQuizMetadata q1178 = new OXQuizMetadata()
            {
                Id = 1178,
                QuestionText = "The Simpson's family dog is named Santa's Little Helper.",
                Answer = true,
                AnswerText = "The Simpson's family dog is named Santa's Little Helper.",
                Category = "Culture/Art"
            };
            questions.Add(q1178);

            OXQuizMetadata q1179 = new OXQuizMetadata()
            {
                Id = 1179,
                QuestionText = "The Simpsons live on Everblue Terrace.",
                Answer = false,
                AnswerText = "The Simpsons live on Evergreen Terrace.",
                Category = "Culture/Art"
            };
            questions.Add(q1179);

            OXQuizMetadata q1180 = new OXQuizMetadata()
            {
                Id = 1180,
                QuestionText = "The slowest animal in the animal kingdom is a turtle.",
                Answer = false,
                AnswerText = "Sloths travel about 30 meters an hour.",
                Category = "General Knowledge"
            };
            questions.Add(q1180);

            OXQuizMetadata q1181 = new OXQuizMetadata()
            {
                Id = 1181,
                QuestionText = "The small intestine is the longer of the two intestines.",
                Answer = true,
                AnswerText = "The small intestine is the longer of the two intestines.",
                Category = "General Knowledge"
            };
            questions.Add(q1181);

            OXQuizMetadata q1182 = new OXQuizMetadata()
            {
                Id = 1182,
                QuestionText = "The Snowy sells Gear items.",
                Answer = false,
                AnswerText = "The Snowy, even though he looks identical to the yeti merchants, is not a merchant.",
                Category = "MapleStory 2"
            };
            questions.Add(q1182);

            OXQuizMetadata q1183 = new OXQuizMetadata()
            {
                Id = 1183,
                QuestionText = "The song \"Grand March\" is part of the musical \"Amida.\"",
                Answer = false,
                AnswerText = "The song \"Grand March\" is part of the musical \"Aida.\"",
                Category = "Culture/Art"
            };
            questions.Add(q1183);

            OXQuizMetadata q1184 = new OXQuizMetadata()
            {
                Id = 1184,
                QuestionText = "The song \"Grand March\" is part of the opera \"Aida.\"",
                Answer = true,
                AnswerText = "The song \"Grand March\" is part of the opera \"Aida.\"",
                Category = "Culture/Art"
            };
            questions.Add(q1184);

            OXQuizMetadata q1185 = new OXQuizMetadata()
            {
                Id = 1185,
                QuestionText = "The Sound of Music takes place in Australia.",
                Answer = false,
                AnswerText = "The Sound of Music takes place in Austria.",
                Category = "Culture/Art"
            };
            questions.Add(q1185);

            OXQuizMetadata q1186 = new OXQuizMetadata()
            {
                Id = 1186,
                QuestionText = "The Sphinx has a body of a snake and a face of a man.",
                Answer = false,
                AnswerText = "The Sphinx has a body of a lion and a face of a man.",
                Category = "General Knowledge"
            };
            questions.Add(q1186);

            OXQuizMetadata q1187 = new OXQuizMetadata()
            {
                Id = 1187,
                QuestionText = "The Statue of Liberty holds a torch in her left hand and the Declaration of Independence in her right.",
                Answer = false,
                AnswerText = "The Statue of Liberty holds a torch in her right hand and the Declaration of Independence in her left.",
                Category = "General Knowledge"
            };
            questions.Add(q1187);

            OXQuizMetadata q1188 = new OXQuizMetadata()
            {
                Id = 1188,
                QuestionText = "The Statue of Liberty in New York is barefooted.",
                Answer = false,
                AnswerText = "The Statue of Liberty is not barefooted.",
                Category = "General Knowledge"
            };
            questions.Add(q1188);

            OXQuizMetadata q1189 = new OXQuizMetadata()
            {
                Id = 1189,
                QuestionText = "The Statue of Liberty was added as a UNESCO World Heritage site in 1984.",
                Answer = true,
                AnswerText = "The Statue of Liberty was added as a UNESCO World Heritage site in 1984.",
                Category = "General Knowledge"
            };
            questions.Add(q1189);

            OXQuizMetadata q1190 = new OXQuizMetadata()
            {
                Id = 1190,
                QuestionText = "The strongest enemy in the Cusp of Life is Einos.",
                Answer = false,
                AnswerText = "The strongest enemy in the Cusp of Life is Pathos.",
                Category = "MapleStory 2"
            };
            questions.Add(q1190);

            OXQuizMetadata q1191 = new OXQuizMetadata()
            {
                Id = 1191,
                QuestionText = "The Suez Canal is a waterway that connects the Mediterranean Sea to the Atlantic Ocean.",
                Answer = false,
                AnswerText = "The Suez Canal is a waterway that connects the Mediterranean Sea to the Red Sea.",
                Category = "General Knowledge"
            };
            questions.Add(q1191);

            OXQuizMetadata q1192 = new OXQuizMetadata()
            {
                Id = 1192,
                QuestionText = "The sum of the interior angles of a pentagon is 470 degrees.",
                Answer = false,
                AnswerText = "The sum of the interior angles of a pentagon is 540 degrees.",
                Category = "Science"
            };
            questions.Add(q1192);

            OXQuizMetadata q1193 = new OXQuizMetadata()
            {
                Id = 1193,
                QuestionText = "The sum of the natural numbers from 1 to 10 is 50.",
                Answer = false,
                AnswerText = "The answer is 55.",
                Category = "Math"
            };
            questions.Add(q1193);

            OXQuizMetadata q1194 = new OXQuizMetadata()
            {
                Id = 1194,
                QuestionText = "The superstring theory describes how the universe expanded from a single point.",
                Answer = false,
                AnswerText = "It's the big bang theory.",
                Category = "General Knowledge"
            };
            questions.Add(q1194);

            OXQuizMetadata q1195 = new OXQuizMetadata()
            {
                Id = 1195,
                QuestionText = "The symbol @ in English means \"and.\"",
                Answer = false,
                AnswerText = "The symbol @ means \"at.\"",
                Category = "IT"
            };
            questions.Add(q1195);

            OXQuizMetadata q1196 = new OXQuizMetadata()
            {
                Id = 1196,
                QuestionText = "The tallest mountain on the Korean peninsula is Paektu Mountain.",
                Answer = true,
                AnswerText = "Paektu Mountain is 2,744m from above sea level.",
                Category = "General Knowledge"
            };
            questions.Add(q1196);

            OXQuizMetadata q1197 = new OXQuizMetadata()
            {
                Id = 1197,
                QuestionText = "The term aunt is used to refer to a mother's sister.",
                Answer = true,
                AnswerText = "Mom's sister is called aunt.",
                Category = "General Knowledge"
            };
            questions.Add(q1197);

            OXQuizMetadata q1198 = new OXQuizMetadata()
            {
                Id = 1198,
                QuestionText = "The term bluebird syndrome refers to a state in which a person puts happiness in reality over the yearning for an ideal world.",
                Answer = false,
                AnswerText = "The term bluebird syndrome refers to a state in which a person puts the yearning for an ideal world over happiness in reality, just like the main character from The Blue Bird by Maurice Maeterlinck.",
                Category = "Culture/Art"
            };
            questions.Add(q1198);

            OXQuizMetadata q1199 = new OXQuizMetadata()
            {
                Id = 1199,
                QuestionText = "The term bluebird syndrome refers to a state in which a person puts the yearning for an ideal world over happiness in reality.",
                Answer = true,
                AnswerText = "The term bluebird syndrome refers to a state in which a person puts the yearning for an ideal world over happiness in reality, just like the main character from The Blue Bird by Maurice Maeterlinck.",
                Category = "Culture/Art"
            };
            questions.Add(q1199);

            OXQuizMetadata q1200 = new OXQuizMetadata()
            {
                Id = 1200,
                QuestionText = "The term chick refers to a baby pheasant.",
                Answer = true,
                AnswerText = "The term chick refers to a baby pheasant.",
                Category = "General Knowledge"
            };
            questions.Add(q1200);

            OXQuizMetadata q1201 = new OXQuizMetadata()
            {
                Id = 1201,
                QuestionText = "The three primary colors are red, yellow, and green.",
                Answer = false,
                AnswerText = "The three primary colors are red, yellow, and blue.",
                Category = "General Knowledge"
            };
            questions.Add(q1201);

            OXQuizMetadata q1202 = new OXQuizMetadata()
            {
                Id = 1202,
                QuestionText = "The title creature in the movie Jaws is a shark.",
                Answer = true,
                AnswerText = "The movie is about the great white shark.",
                Category = "Culture/Art"
            };
            questions.Add(q1202);

            OXQuizMetadata q1203 = new OXQuizMetadata()
            {
                Id = 1203,
                QuestionText = "The Tony Awards, widely regarded as the Academy Awards for live theater performances, recognizes excellence in live Broadway theater.",
                Answer = true,
                AnswerText = "The Tony Awards are for plays that are performed in live Broadway theaters.",
                Category = "Culture/Art"
            };
            questions.Add(q1203);

            OXQuizMetadata q1204 = new OXQuizMetadata()
            {
                Id = 1204,
                QuestionText = "The top 4 fashion shows are held in New York, Milano, Paris, and Rome.",
                Answer = false,
                AnswerText = "The top 4 fashion shows are held in New York, Paris, London, and Milano.",
                Category = "General Knowledge"
            };
            questions.Add(q1204);

            OXQuizMetadata q1205 = new OXQuizMetadata()
            {
                Id = 1205,
                QuestionText = "The top of the Eiffel Tower leans away from the sun.",
                Answer = true,
                AnswerText = "Metal expands with the heat from the sun, so the side facing the sun it's always longer than the side in the shadow.",
                Category = "Culture/Art"
            };
            questions.Add(q1205);

            OXQuizMetadata q1206 = new OXQuizMetadata()
            {
                Id = 1206,
                QuestionText = "The torso refers to the body, not the limbs.",
                Answer = true,
                AnswerText = "The torso refers to the body, not the limbs.",
                Category = "Culture/Art"
            };
            questions.Add(q1206);

            OXQuizMetadata q1207 = new OXQuizMetadata()
            {
                Id = 1207,
                QuestionText = "The Transparency Badge makes your weapon invisible.",
                Answer = false,
                AnswerText = "The Transparency Badge cannot hide weapons from display.",
                Category = "MapleStory 2"
            };
            questions.Add(q1207);

            OXQuizMetadata q1208 = new OXQuizMetadata()
            {
                Id = 1208,
                QuestionText = "The Treaty of Nanjing placed Hong Kong under British rule.",
                Answer = true,
                AnswerText = "It is the treaty that ended the first Opium War. China paid the British an indemnity, ceded the territory of Hong Kong, and agreed to establish a \"fair and reasonable\" tariff on British goods. As a side effect of the treaty,",
                Category = "History"
            };
            questions.Add(q1208);

            OXQuizMetadata q1209 = new OXQuizMetadata()
            {
                Id = 1209,
                QuestionText = "The Trigger Controller helps you control the actions of all items inside your house.",
                Answer = false,
                AnswerText = "Only Furnishing items can be controlled by the Trigger Controller.",
                Category = "MapleStory 2"
            };
            questions.Add(q1209);

            OXQuizMetadata q1210 = new OXQuizMetadata()
            {
                Id = 1210,
                QuestionText = "The Triple Point refers to the temperature and pressure at which the solid, liquid, and vapor phases of a pure substance can coexist in equilibrium.",
                Answer = true,
                AnswerText = "The Triple Point refers to the temperature and pressure at which the solid, liquid, and vapor phases of a pure substance can coexist in equilibrium.",
                Category = "Science"
            };
            questions.Add(q1210);

            OXQuizMetadata q1211 = new OXQuizMetadata()
            {
                Id = 1211,
                QuestionText = "The trumpet is an aerophone, which produces sound by creating vibrations in the air.",
                Answer = true,
                AnswerText = "The trumpet is an aerophone, which produces sound by creating vibrations in the air.",
                Category = "Culture/Art"
            };
            questions.Add(q1211);

            OXQuizMetadata q1212 = new OXQuizMetadata()
            {
                Id = 1212,
                QuestionText = "The two ideas of Jean-Jacques Rousseau's political philosophy are a direct democracy and empowering citizens.",
                Answer = true,
                AnswerText = "The two ideas of Jean-Jacques Rousseau's political philosophy are a direct democracy and empowering citizens.",
                Category = "Society"
            };
            questions.Add(q1212);

            OXQuizMetadata q1213 = new OXQuizMetadata()
            {
                Id = 1213,
                QuestionText = "The United Kingdom and Qing Dynasty agreed on The Treaty of Beijing right after the Opium Wars.",
                Answer = false,
                AnswerText = "The United Kingdom and Qing Dynasty agreed on The Treaty of Nanjing right after the Opium Wars.",
                Category = "History"
            };
            questions.Add(q1213);

            OXQuizMetadata q1214 = new OXQuizMetadata()
            {
                Id = 1214,
                QuestionText = "The United Kingdom and Qing Dynasty agreed on the Treaty of Nanjing right after the Opium Wars.",
                Answer = true,
                AnswerText = "The United Kingdom and Qing Dynasty agreed on The Treaty of Nanjing right after the Opium Wars.",
                Category = "History"
            };
            questions.Add(q1214);

            OXQuizMetadata q1215 = new OXQuizMetadata()
            {
                Id = 1215,
                QuestionText = "The United Kingdom is a fully independent sovereign state made up of the 3 countries in Great Britain (England, Scotland, and Waves) plus Ireland.",
                Answer = false,
                AnswerText = "The United Kingdom is a fully independent sovereign state made up of the 3 countries in Great Britain (England, Scotland, and Wales) plus Northern Ireland. Ireland is a separate nation.",
                Category = "History"
            };
            questions.Add(q1215);

            OXQuizMetadata q1216 = new OXQuizMetadata()
            {
                Id = 1216,
                QuestionText = "The United States has won the most gold medals in the Winter Olympics.",
                Answer = false,
                AnswerText = "Norway has won the highest number of gold medals in the Winter Olympics.",
                Category = "General Knowledge"
            };
            questions.Add(q1216);

            OXQuizMetadata q1217 = new OXQuizMetadata()
            {
                Id = 1217,
                QuestionText = "The United States is the country that won the most FIFA Women's World Cups.",
                Answer = true,
                AnswerText = "The United States, which have won three times, is the country that won the most FIFA Women's World Cups.",
                Category = "Culture/Art"
            };
            questions.Add(q1217);

            OXQuizMetadata q1218 = new OXQuizMetadata()
            {
                Id = 1218,
                QuestionText = "The universe is expanding at a decreasing rate.",
                Answer = false,
                AnswerText = "It's rather expanding at an increasing rate.",
                Category = "Science"
            };
            questions.Add(q1218);

            OXQuizMetadata q1219 = new OXQuizMetadata()
            {
                Id = 1219,
                QuestionText = "The Unyo Incident ignited The Second Opium War.",
                Answer = false,
                AnswerText = "The Arrow Incident started the Second Opium War.",
                Category = "History"
            };
            questions.Add(q1219);

            OXQuizMetadata q1220 = new OXQuizMetadata()
            {
                Id = 1220,
                QuestionText = "The US Open is the oldest of the four major championships in professional golf.",
                Answer = false,
                AnswerText = "The British Open Championship was founded in 1860, the Masters Tournament in 1930, the US Open Championship in 1931, and the PGA Championship in 1916.",
                Category = "History"
            };
            questions.Add(q1220);

            OXQuizMetadata q1221 = new OXQuizMetadata()
            {
                Id = 1221,
                QuestionText = "The velodrome is an arena for track cycling.",
                Answer = true,
                AnswerText = "The velodrome is an arena for track cycling.",
                Category = "Culture/Art"
            };
            questions.Add(q1221);

            OXQuizMetadata q1222 = new OXQuizMetadata()
            {
                Id = 1222,
                QuestionText = "The velodrome is an ice rink designed for figure skating.",
                Answer = false,
                AnswerText = "The velodrome is an arena for track cycling.",
                Category = "Culture/Art"
            };
            questions.Add(q1222);

            OXQuizMetadata q1223 = new OXQuizMetadata()
            {
                Id = 1223,
                QuestionText = "The violin was first invented in the 6th century.",
                Answer = false,
                AnswerText = "Violin was first invented in the 16th century.",
                Category = "Culture/Art"
            };
            questions.Add(q1223);

            OXQuizMetadata q1224 = new OXQuizMetadata()
            {
                Id = 1224,
                QuestionText = "The warmest part of a human body is the ears.",
                Answer = false,
                AnswerText = "Because the ears have fewer blood vessels than other organs do, they are the coolest part of the human body.",
                Category = "Science"
            };
            questions.Add(q1224);

            OXQuizMetadata q1225 = new OXQuizMetadata()
            {
                Id = 1225,
                QuestionText = "The Wars of the Roses were a series of wars waged over the control of the throne of France (1445-85).",
                Answer = false,
                AnswerText = "The Wars of the Roses were a series of wars waged by the two powerhouses -- Lancaster and York -- over the throne of England.",
                Category = "History"
            };
            questions.Add(q1225);

            OXQuizMetadata q1226 = new OXQuizMetadata()
            {
                Id = 1226,
                QuestionText = "The Weimar Constitution is the first constitution to state fundamental human rights.",
                Answer = true,
                AnswerText = "To protect the right to live like a human, the Weimar Constitution states fundamental human rights.",
                Category = "Society"
            };
            questions.Add(q1226);

            OXQuizMetadata q1227 = new OXQuizMetadata()
            {
                Id = 1227,
                QuestionText = "The western trend in literature shifted from classicism to naturalism to romanticism.",
                Answer = false,
                AnswerText = "The western trend in literature shifted from classicism to romanticism to naturalism",
                Category = "Culture/Art"
            };
            questions.Add(q1227);

            OXQuizMetadata q1228 = new OXQuizMetadata()
            {
                Id = 1228,
                QuestionText = "The word \"audition,\" in broadcasting, refers to the third parties' tipping of sensitive information, under the agreement of confidentiality.",
                Answer = false,
                AnswerText = "The phase \"off the record\" refers to third parties' tipping of sensitive information, under the agreement of confidentiality.",
                Category = "General Knowledge"
            };
            questions.Add(q1228);

            OXQuizMetadata q1229 = new OXQuizMetadata()
            {
                Id = 1229,
                QuestionText = "The word \"eskimo,\" which refers to a member of an indigenous people inhabiting northern Alaska, means \"eater of raw meat.\"",
                Answer = true,
                AnswerText = "\"Eskimo\" means \"the eater of raw meat.\"",
                Category = "General Knowledge"
            };
            questions.Add(q1229);

            OXQuizMetadata q1230 = new OXQuizMetadata()
            {
                Id = 1230,
                QuestionText = "The word \"gondola\" in Italian means \"roll, rock.\"",
                Answer = true,
                AnswerText = "The word \"gondola\" in Italian means \"roll, rock.\"",
                Category = "General Knowledge"
            };
            questions.Add(q1230);

            OXQuizMetadata q1231 = new OXQuizMetadata()
            {
                Id = 1231,
                QuestionText = "The word baroque is derived from French.",
                Answer = false,
                AnswerText = "The word comes from the Portuguese word \"barroco\" meaning \"imperfect pearl.\"",
                Category = "Culture/Art"
            };
            questions.Add(q1231);

            OXQuizMetadata q1232 = new OXQuizMetadata()
            {
                Id = 1232,
                QuestionText = "The word baroque is derived from Portuguese.",
                Answer = true,
                AnswerText = "The word comes from the Portuguese word \"barroco,\" meaning \"imperfect pearl.\"",
                Category = "Culture/Art"
            };
            questions.Add(q1232);

            OXQuizMetadata q1233 = new OXQuizMetadata()
            {
                Id = 1233,
                QuestionText = "The word Love is a term used in boxing.",
                Answer = false,
                AnswerText = "The word Love in tennis refers to the score of zero.",
                Category = "Culture/Art"
            };
            questions.Add(q1233);

            OXQuizMetadata q1234 = new OXQuizMetadata()
            {
                Id = 1234,
                QuestionText = "The word love is a term used in tennis.",
                Answer = true,
                AnswerText = "The world love in tennis refers to the score of zero.",
                Category = "Culture/Art"
            };
            questions.Add(q1234);

            OXQuizMetadata q1235 = new OXQuizMetadata()
            {
                Id = 1235,
                QuestionText = "The World Cup is held every 2 years.",
                Answer = false,
                AnswerText = "The World Cup happens every 4 years.",
                Category = "General Knowledge"
            };
            questions.Add(q1235);

            OXQuizMetadata q1236 = new OXQuizMetadata()
            {
                Id = 1236,
                QuestionText = "The World Cup is held every 3 years.",
                Answer = false,
                AnswerText = "The World Cup is held every 4 years.",
                Category = "Culture/Art"
            };
            questions.Add(q1236);

            OXQuizMetadata q1237 = new OXQuizMetadata()
            {
                Id = 1237,
                QuestionText = "The world's first scented stamp was developed in Paris, France.",
                Answer = false,
                AnswerText = "The world's first scented stamp was developed in Germany, in 1945.",
                Category = "Culture/Art"
            };
            questions.Add(q1237);

            OXQuizMetadata q1238 = new OXQuizMetadata()
            {
                Id = 1238,
                QuestionText = "The www at the beginning of a URL is a shorthand for World Wide Web.",
                Answer = true,
                AnswerText = "The www at the beginning of a URL is a shorthand for World Wide Web.",
                Category = "IT"
            };
            questions.Add(q1238);

            OXQuizMetadata q1239 = new OXQuizMetadata()
            {
                Id = 1239,
                QuestionText = "The xylophone is a percussion instrument.",
                Answer = true,
                AnswerText = "The xylophone is a percussion instrument that has a keyboard.",
                Category = "Culture/Art"
            };
            questions.Add(q1239);

            OXQuizMetadata q1240 = new OXQuizMetadata()
            {
                Id = 1240,
                QuestionText = "The xylophone is a string instrument.",
                Answer = false,
                AnswerText = "The xylophone is a percussion instrument with a keyboard.",
                Category = "Culture/Art"
            };
            questions.Add(q1240);

            OXQuizMetadata q1241 = new OXQuizMetadata()
            {
                Id = 1241,
                QuestionText = "The year-long rotation of the earth around the sun is called Earth's rotation.",
                Answer = false,
                AnswerText = "The Earth takes one year to revolve around the sun. The process is called the revolution of the Earth.",
                Category = "General Knowledge"
            };
            questions.Add(q1241);

            OXQuizMetadata q1242 = new OXQuizMetadata()
            {
                Id = 1242,
                QuestionText = "There are 2 different colors--white and yellow--for the swim ring that your character wears when in the water.",
                Answer = false,
                AnswerText = "There are yellow and orange swim rings.",
                Category = "MapleStory 2"
            };
            questions.Add(q1242);

            OXQuizMetadata q1243 = new OXQuizMetadata()
            {
                Id = 1243,
                QuestionText = "There are 4 black piano keys in one octave.",
                Answer = false,
                AnswerText = "There are 5 or 6 black pianos keys in one octave.",
                Category = "Culture/Art"
            };
            questions.Add(q1243);

            OXQuizMetadata q1244 = new OXQuizMetadata()
            {
                Id = 1244,
                QuestionText = "There are 51 stars on the flag of the United States.",
                Answer = false,
                AnswerText = "There are 50 stars on the flag of the United States.",
                Category = "General Knowledge"
            };
            questions.Add(q1244);

            OXQuizMetadata q1245 = new OXQuizMetadata()
            {
                Id = 1245,
                QuestionText = "There are 7 black piano keys in one octave.",
                Answer = false,
                AnswerText = "There are 5 black pianos keys in one octave.",
                Category = "Culture/Art"
            };
            questions.Add(q1245);

            OXQuizMetadata q1246 = new OXQuizMetadata()
            {
                Id = 1246,
                QuestionText = "There are no speed limits for airplanes.",
                Answer = false,
                AnswerText = "There are speed limits near the control tower and other zones specified by each nation's minister of transport.",
                Category = "General Knowledge"
            };
            questions.Add(q1246);

            OXQuizMetadata q1247 = new OXQuizMetadata()
            {
                Id = 1247,
                QuestionText = "There are three stars in Singapore's national flag.",
                Answer = false,
                AnswerText = "There are five stars in Singapore's national flag.",
                Category = "General Knowledge"
            };
            questions.Add(q1247);

            OXQuizMetadata q1248 = new OXQuizMetadata()
            {
                Id = 1248,
                QuestionText = "There is a dialectical model named for Jean-Jacques Rousseau.",
                Answer = false,
                AnswerText = "There is a dialectical model named for G. W. F. Hegel.",
                Category = "Science"
            };
            questions.Add(q1248);

            OXQuizMetadata q1249 = new OXQuizMetadata()
            {
                Id = 1249,
                QuestionText = "There's a broadcasting station in MapleStory 2.",
                Answer = true,
                AnswerText = "Maple TV Newsroom, accessible from Silverstone Bridge, is the broadcasting station in MapleStory 2.",
                Category = "MapleStory 2"
            };
            questions.Add(q1249);

            OXQuizMetadata q1250 = new OXQuizMetadata()
            {
                Id = 1250,
                QuestionText = "There's a bus on the Silverstone Bridge that can be picked up and thrown.",
                Answer = false,
                AnswerText = "There's no bus on the Silverstone Bridge that can be picked up.",
                Category = "MapleStory 2"
            };
            questions.Add(q1250);

            OXQuizMetadata q1251 = new OXQuizMetadata()
            {
                Id = 1251,
                QuestionText = "There's a pair of binoculars at Revoldic Dam.",
                Answer = false,
                AnswerText = "There's no pair of binoculars at Revoldic Dam.",
                Category = "MapleStory 2"
            };
            questions.Add(q1251);

            OXQuizMetadata q1252 = new OXQuizMetadata()
            {
                Id = 1252,
                QuestionText = "There's a pair of binoculars in Misty Temple.",
                Answer = false,
                AnswerText = "There's no pair of binoculars in Misty Temple.",
                Category = "MapleStory 2"
            };
            questions.Add(q1252);

            OXQuizMetadata q1253 = new OXQuizMetadata()
            {
                Id = 1253,
                QuestionText = "There's a pair of binoculars on the Skyreach Pass.",
                Answer = true,
                AnswerText = "There's a pair of binoculars on the Skyreach Pass.",
                Category = "MapleStory 2"
            };
            questions.Add(q1253);

            OXQuizMetadata q1254 = new OXQuizMetadata()
            {
                Id = 1254,
                QuestionText = "There's a phone booth in Rose Castle that can be picked up.",
                Answer = true,
                AnswerText = "There's no phone booth in Rose Castle that can be picked up.",
                Category = "MapleStory 2"
            };
            questions.Add(q1254);

            OXQuizMetadata q1255 = new OXQuizMetadata()
            {
                Id = 1255,
                QuestionText = "There's a total of 13 huts in the Crow's Eye Village.",
                Answer = true,
                AnswerText = "There's a total of 13 huts in the Crow's Eye Village.",
                Category = "MapleStory 2"
            };
            questions.Add(q1255);

            OXQuizMetadata q1256 = new OXQuizMetadata()
            {
                Id = 1256,
                QuestionText = "There's a total of 292 trees in the Spectrumwood.",
                Answer = false,
                AnswerText = "There's a total of 288 trees.",
                Category = "MapleStory 2"
            };
            questions.Add(q1256);

            OXQuizMetadata q1257 = new OXQuizMetadata()
            {
                Id = 1257,
                QuestionText = "There's a total of 3 Allicari merchants in Queenstown.",
                Answer = true,
                AnswerText = "There's a total of 3 Allicari merchants—Aliyar, Mayar, and Godar—in Queenstown.",
                Category = "MapleStory 2"
            };
            questions.Add(q1257);

            OXQuizMetadata q1258 = new OXQuizMetadata()
            {
                Id = 1258,
                QuestionText = "There's a total of 7 golden treasure chests in the Trinket Woods.",
                Answer = true,
                AnswerText = "There's a total of 7 golden treasure chests in the Trinket Woods.",
                Category = "MapleStory 2"
            };
            questions.Add(q1258);

            OXQuizMetadata q1259 = new OXQuizMetadata()
            {
                Id = 1259,
                QuestionText = "There's a utility pole in Kerning City that can be uprooted and swung.",
                Answer = false,
                AnswerText = "There's no utility pole in Kerning City that can be uprooted and swung.",
                Category = "MapleStory 2"
            };
            questions.Add(q1259);

            OXQuizMetadata q1260 = new OXQuizMetadata()
            {
                Id = 1260,
                QuestionText = "There's a utility pole in Woodbury that can be uprooted and swung.",
                Answer = true,
                AnswerText = "There's a utility pole in Woodbury that can be uprooted and swung.",
                Category = "MapleStory 2"
            };
            questions.Add(q1260);

            OXQuizMetadata q1261 = new OXQuizMetadata()
            {
                Id = 1261,
                QuestionText = "There's a way to find out whether an egg is boiled or not without breaking it.",
                Answer = true,
                AnswerText = "One can spin it and find out whether the egg is boiled.",
                Category = "General Knowledge"
            };
            questions.Add(q1261);

            OXQuizMetadata q1262 = new OXQuizMetadata()
            {
                Id = 1262,
                QuestionText = "There's a wedding hall in MapleStory 2.",
                Answer = true,
                AnswerText = "The Amore Wedding Hall, accessible from Tria, is an elegant wedding hall with a warm atmosphere.",
                Category = "MapleStory 2"
            };
            questions.Add(q1262);

            OXQuizMetadata q1263 = new OXQuizMetadata()
            {
                Id = 1263,
                QuestionText = "There's no pair of binoculars by Fairy Tree Lake.",
                Answer = true,
                AnswerText = "There's no pair of binoculars by Fairy Tree Lake.",
                Category = "MapleStory 2"
            };
            questions.Add(q1263);

            OXQuizMetadata q1264 = new OXQuizMetadata()
            {
                Id = 1264,
                QuestionText = "There's no pair of binoculars in the Magma Gorge.",
                Answer = true,
                AnswerText = "There's no pair of binoculars in the Magma Gorge.",
                Category = "MapleStory 2"
            };
            questions.Add(q1264);

            OXQuizMetadata q1265 = new OXQuizMetadata()
            {
                Id = 1265,
                QuestionText = "There's no picture of Nelph inside Mrs. Ibelin's House.",
                Answer = true,
                AnswerText = "There's no picture of Nelph inside Mrs. Ibelin's House.",
                Category = "MapleStory 2"
            };
            questions.Add(q1265);

            OXQuizMetadata q1266 = new OXQuizMetadata()
            {
                Id = 1266,
                QuestionText = "There's no runway in Maplestory 2.",
                Answer = false,
                AnswerText = "Victoria Runway, accessible from Tria, is a modern fashion runway with a cruise ship theme.",
                Category = "MapleStory 2"
            };
            questions.Add(q1266);

            OXQuizMetadata q1267 = new OXQuizMetadata()
            {
                Id = 1267,
                QuestionText = "Third baseman's position number is six.",
                Answer = false,
                AnswerText = "Short stop's position number is six.",
                Category = "Culture/Art"
            };
            questions.Add(q1267);

            OXQuizMetadata q1268 = new OXQuizMetadata()
            {
                Id = 1268,
                QuestionText = "Thomas Jefferson served as the first President of the United States.",
                Answer = false,
                AnswerText = "On April 30, 1789, George Washington was inaugurated as the first President of the United States.",
                Category = "History"
            };
            questions.Add(q1268);

            OXQuizMetadata q1269 = new OXQuizMetadata()
            {
                Id = 1269,
                QuestionText = "Tigers, the king of predators, also eat fruit.",
                Answer = true,
                AnswerText = "They eat durians.",
                Category = "General Knowledge"
            };
            questions.Add(q1269);

            OXQuizMetadata q1270 = new OXQuizMetadata()
            {
                Id = 1270,
                QuestionText = "Tinnitus is the perception of noise or ringing in the ears.",
                Answer = true,
                AnswerText = "Because it's a disease based on the patient's perception, it's treatment isn't clearly established.",
                Category = "Science"
            };
            questions.Add(q1270);

            OXQuizMetadata q1271 = new OXQuizMetadata()
            {
                Id = 1271,
                QuestionText = "To find Giant Lava Eye, you must carefully search the Hototot River.",
                Answer = true,
                AnswerText = "There's the entrance to Lava Eye Nest in Hototot River, and Giant Lava Eye appears inside the nest.",
                Category = "MapleStory 2"
            };
            questions.Add(q1271);

            OXQuizMetadata q1272 = new OXQuizMetadata()
            {
                Id = 1272,
                QuestionText = "To invite a character into your guild, the character must be online.",
                Answer = true,
                AnswerText = "Offline characters cannot be invited to guilds.",
                Category = "MapleStory 2"
            };
            questions.Add(q1272);

            OXQuizMetadata q1273 = new OXQuizMetadata()
            {
                Id = 1273,
                QuestionText = "To join the Alikar Prison tour, you must pay 100 Mesos to Oarsman Seamus.",
                Answer = false,
                AnswerText = "The prison tour costs 1,000 Mesos.",
                Category = "MapleStory 2"
            };
            questions.Add(q1273);

            OXQuizMetadata q1274 = new OXQuizMetadata()
            {
                Id = 1274,
                QuestionText = "To send a Friend request to a character, the character must be online.",
                Answer = false,
                AnswerText = "Friend requests can be sent to characters regardless of their online status.",
                Category = "MapleStory 2"
            };
            questions.Add(q1274);

            OXQuizMetadata q1275 = new OXQuizMetadata()
            {
                Id = 1275,
                QuestionText = "To use the Black Market, you must be in the Black Market map.",
                Answer = false,
                AnswerText = "You can use the Black Market button in the game menu to use the Black Market at any time.",
                Category = "MapleStory 2"
            };
            questions.Add(q1275);

            OXQuizMetadata q1276 = new OXQuizMetadata()
            {
                Id = 1276,
                QuestionText = "To visit Alikar Prison by boat, speak to Pesca in Lith Harbor.",
                Answer = false,
                AnswerText = "Pesca is a bait merchant. To visit Alikar Prison, you must meet Oarsman Simus.",
                Category = "MapleStory 2"
            };
            questions.Add(q1276);

            OXQuizMetadata q1277 = new OXQuizMetadata()
            {
                Id = 1277,
                QuestionText = "To visit Alikar Prison, you must get on the ship to the prison at Lith Harbor.",
                Answer = true,
                AnswerText = "Oarsman Seamus in Lith Harbor lets you get on the Alikar Prison cruise.",
                Category = "MapleStory 2"
            };
            questions.Add(q1277);

            OXQuizMetadata q1278 = new OXQuizMetadata()
            {
                Id = 1278,
                QuestionText = "Toh and Googoo are the field boss monsters of Whistler Cliffs.",
                Answer = true,
                AnswerText = "Toh and Googoo are the field boss monsters of Whistler Cliffs.",
                Category = "MapleStory 2"
            };
            questions.Add(q1278);

            OXQuizMetadata q1279 = new OXQuizMetadata()
            {
                Id = 1279,
                QuestionText = "Toilet, bathroom, and restroom all refer to the same thing.",
                Answer = true,
                AnswerText = "Toilet, bathroom, and restroom all refer to the same thing.",
                Category = "General Knowledge"
            };
            questions.Add(q1279);

            OXQuizMetadata q1280 = new OXQuizMetadata()
            {
                Id = 1280,
                QuestionText = "Tokyo is not Japan's capital.",
                Answer = false,
                AnswerText = "Japan's capital is Tokyo.",
                Category = "General Knowledge"
            };
            questions.Add(q1280);

            OXQuizMetadata q1281 = new OXQuizMetadata()
            {
                Id = 1281,
                QuestionText = "Tony Stark is a billionaire who lost his parents in a dark alley.",
                Answer = false,
                AnswerText = "Bruce Wayne lost his parents in a dark alley.",
                Category = "Culture/Art"
            };
            questions.Add(q1281);

            OXQuizMetadata q1282 = new OXQuizMetadata()
            {
                Id = 1282,
                QuestionText = "Too much vitamin C causes scurvy.",
                Answer = false,
                AnswerText = "Taking vitamin C can cure scurvy.",
                Category = "Science"
            };
            questions.Add(q1282);

            OXQuizMetadata q1283 = new OXQuizMetadata()
            {
                Id = 1283,
                QuestionText = "Toothpaste can kill cockroaches.",
                Answer = true,
                AnswerText = "Fluorine, an ingredient in toothpaste, is widely used in pesticides.",
                Category = "General Knowledge"
            };
            questions.Add(q1283);

            OXQuizMetadata q1284 = new OXQuizMetadata()
            {
                Id = 1284,
                QuestionText = "Toothpaste can't kill cockroaches.",
                Answer = false,
                AnswerText = "Fluorine, an ingredient in toothpaste, is widely used in pesticides.",
                Category = "General Knowledge"
            };
            questions.Add(q1284);

            OXQuizMetadata q1285 = new OXQuizMetadata()
            {
                Id = 1285,
                QuestionText = "Torso refers to human limbs.",
                Answer = false,
                AnswerText = "Torso refers to the body, not the limbs.",
                Category = "Culture/Art"
            };
            questions.Add(q1285);

            OXQuizMetadata q1286 = new OXQuizMetadata()
            {
                Id = 1286,
                QuestionText = "Treble Clef is also called C Clef.",
                Answer = false,
                AnswerText = "Treble Clef is called G Clef.",
                Category = "Culture/Art"
            };
            questions.Add(q1286);

            OXQuizMetadata q1287 = new OXQuizMetadata()
            {
                Id = 1287,
                QuestionText = "Turkey's capital is Istanbul.",
                Answer = false,
                AnswerText = "Ankara is the capital of Turkey.",
                Category = "General Knowledge"
            };
            questions.Add(q1287);

            OXQuizMetadata q1288 = new OXQuizMetadata()
            {
                Id = 1288,
                QuestionText = "Turning the volume up on a television will use more electricity.",
                Answer = true,
                AnswerText = "Loud volumes or bright TV screens can increase electricity consumption.",
                Category = "General Knowledge"
            };
            questions.Add(q1288);

            OXQuizMetadata q1289 = new OXQuizMetadata()
            {
                Id = 1289,
                QuestionText = "Turtles are slower than snails.",
                Answer = false,
                AnswerText = "Snails are slower.",
                Category = "General Knowledge"
            };
            questions.Add(q1289);

            OXQuizMetadata q1290 = new OXQuizMetadata()
            {
                Id = 1290,
                QuestionText = "Twingos swing shovels.",
                Answer = false,
                AnswerText = "Twingoos swing shovels.",
                Category = "MapleStory 2"
            };
            questions.Add(q1290);

            OXQuizMetadata q1291 = new OXQuizMetadata()
            {
                Id = 1291,
                QuestionText = "Unequal suffrage means every citizen gets one vote.",
                Answer = false,
                AnswerText = "Equal suffrage means that every citizen, regardless of social standings, gets one vote.",
                Category = "Society"
            };
            questions.Add(q1291);

            OXQuizMetadata q1292 = new OXQuizMetadata()
            {
                Id = 1292,
                QuestionText = "UNESCO is an international organization founded to strengthen the ties between nations and societies by sharing education, science, and culture.",
                Answer = true,
                AnswerText = "UNESCO is the United Nations Educational, Scientific and Cultural Organization.",
                Category = "Society"
            };
            questions.Add(q1292);

            OXQuizMetadata q1293 = new OXQuizMetadata()
            {
                Id = 1293,
                QuestionText = "UNICEF is an international organization founded to strengthen the ties among nations and societies by sharing education, science, and culture.",
                Answer = false,
                AnswerText = "UNESCO is the United Nations Educational, Scientific and Cultural Organization.",
                Category = "Society"
            };
            questions.Add(q1293);

            OXQuizMetadata q1294 = new OXQuizMetadata()
            {
                Id = 1294,
                QuestionText = "Urpanda in Rainbow Mountain carries bamboo stalks on its shoulders.",
                Answer = true,
                AnswerText = "Urpanda carries gigantic bamboo stalks on its shoulders.",
                Category = "MapleStory 2"
            };
            questions.Add(q1294);

            OXQuizMetadata q1295 = new OXQuizMetadata()
            {
                Id = 1295,
                QuestionText = "Urpanda in Rainbow Mountain carries pine trees on its shoulders.",
                Answer = false,
                AnswerText = "Urpanda carries gigantic bamboo stalks on its shoulders.",
                Category = "MapleStory 2"
            };
            questions.Add(q1295);

            OXQuizMetadata q1296 = new OXQuizMetadata()
            {
                Id = 1296,
                QuestionText = "Urpanda's Spin Kick skill stuns its target for 2 seconds.",
                Answer = false,
                AnswerText = "Urpanda's Spin Kick stuns for 3 seconds.",
                Category = "MapleStory 2"
            };
            questions.Add(q1296);

            OXQuizMetadata q1297 = new OXQuizMetadata()
            {
                Id = 1297,
                QuestionText = "Valor is a currency obtainable from field battles.",
                Answer = false,
                AnswerText = "Valor is a currency obtainable through Guild Championships.",
                Category = "MapleStory 2"
            };
            questions.Add(q1297);

            OXQuizMetadata q1298 = new OXQuizMetadata()
            {
                Id = 1298,
                QuestionText = "Varaha appears in the Andrea Barony.",
                Answer = true,
                AnswerText = "The elite monster in the Andrea Barony is Varaha.",
                Category = "MapleStory 2"
            };
            questions.Add(q1298);

            OXQuizMetadata q1299 = new OXQuizMetadata()
            {
                Id = 1299,
                QuestionText = "VDT syndrome refers to the eye strain, headache, etc. caused by computer and visual display terminals.",
                Answer = true,
                AnswerText = "VDT syndrome refers to the eye strain, headache, etc. caused by computer and visual display terminals.",
                Category = "General Knowledge"
            };
            questions.Add(q1299);

            OXQuizMetadata q1300 = new OXQuizMetadata()
            {
                Id = 1300,
                QuestionText = "Vincent Van Gogh draw \"Potato Eaters.\"",
                Answer = true,
                AnswerText = "It is Vincent Van Gogh's work.",
                Category = "Culture/Art"
            };
            questions.Add(q1300);

            OXQuizMetadata q1301 = new OXQuizMetadata()
            {
                Id = 1301,
                QuestionText = "Vincent Van Gogh is a post-impressionist painter from the Netherlands.",
                Answer = true,
                AnswerText = "Born in the Netherlands, Gogh painted in France.",
                Category = "Culture/Art"
            };
            questions.Add(q1301);

            OXQuizMetadata q1302 = new OXQuizMetadata()
            {
                Id = 1302,
                QuestionText = "Vincent Van Gogh is a post-impressionist painter from the Neverlands.",
                Answer = false,
                AnswerText = "Born in the Netherlands, Gogh painted in France.",
                Category = "Culture/Art"
            };
            questions.Add(q1302);

            OXQuizMetadata q1303 = new OXQuizMetadata()
            {
                Id = 1303,
                QuestionText = "Violin is composed of four notes: C (Do), G (Sol), D (Re), and A (La).",
                Answer = false,
                AnswerText = "Violin is composed of four notes: G (Sol), D (Re), A (La), and E (Mi).",
                Category = "Culture/Art"
            };
            questions.Add(q1303);

            OXQuizMetadata q1304 = new OXQuizMetadata()
            {
                Id = 1304,
                QuestionText = "Violin was first invented in the 16th century.",
                Answer = true,
                AnswerText = "Violin was first invented in the 16th century.",
                Category = "Culture/Art"
            };
            questions.Add(q1304);

            OXQuizMetadata q1305 = new OXQuizMetadata()
            {
                Id = 1305,
                QuestionText = "Violin's sound hole is C-shaped.",
                Answer = false,
                AnswerText = "Violin's sound hole is F-Shaped.",
                Category = "Culture/Art"
            };
            questions.Add(q1305);

            OXQuizMetadata q1306 = new OXQuizMetadata()
            {
                Id = 1306,
                QuestionText = "Vitamin D deficiency causes night blindness.",
                Answer = false,
                AnswerText = "Vitamin A deficiency causes night blindness.",
                Category = "Science"
            };
            questions.Add(q1306);

            OXQuizMetadata q1307 = new OXQuizMetadata()
            {
                Id = 1307,
                QuestionText = "VOD is a means of communication.",
                Answer = false,
                AnswerText = "Video On Demand (VOD) is a movie renting system.",
                Category = "General Knowledge"
            };
            questions.Add(q1307);

            OXQuizMetadata q1308 = new OXQuizMetadata()
            {
                Id = 1308,
                QuestionText = "Volts is a unit used to measure the apparent power in the electrical circuit.",
                Answer = false,
                AnswerText = "Ampere is a unit used to measure the apparent power in the electrical circuit.",
                Category = "General Knowledge"
            };
            questions.Add(q1308);

            OXQuizMetadata q1309 = new OXQuizMetadata()
            {
                Id = 1309,
                QuestionText = "W. Somerset Maugham's \"Moon and Six Pence\" is a novel based on the post-impressionist painter Paul Gauguin.",
                Answer = true,
                AnswerText = "W. Somerset Maugham's \"Moon and Six Pence\" is a novel based on the post-impressionist painter Paul Gauguin.",
                Category = "Culture/Art"
            };
            questions.Add(q1309);

            OXQuizMetadata q1310 = new OXQuizMetadata()
            {
                Id = 1310,
                QuestionText = "W. Somerset Maugham's \"Moon and Six Pence\" is a novel based on the post-impressionist painter Pierre-Auguste Renoir.",
                Answer = false,
                AnswerText = "W. Somerset Maugham's \"Moon and Six Pence\" is a novel based on the post-impressionist painter Paul Gauguin.",
                Category = "Culture/Art"
            };
            questions.Add(q1310);

            OXQuizMetadata q1311 = new OXQuizMetadata()
            {
                Id = 1311,
                QuestionText = "Walt Disney designed Mickey Mouse.",
                Answer = false,
                AnswerText = "Ub Iwerks designed Mickey Mouse.",
                Category = "Culture/Art"
            };
            questions.Add(q1311);

            OXQuizMetadata q1312 = new OXQuizMetadata()
            {
                Id = 1312,
                QuestionText = "Walter Adolf Gropius started functionalism by founding a Bauhaus research school at Weimar, Germany in 1919. The school tried to combine crafts and fine arts.",
                Answer = true,
                AnswerText = "Bauhaus is a research school at Weimar, Germany. It greatly influenced modern arts.",
                Category = "Culture/Art"
            };
            questions.Add(q1312);

            OXQuizMetadata q1313 = new OXQuizMetadata()
            {
                Id = 1313,
                QuestionText = "Walter Adolf Gropius started functionalism by founding a Bouncehouse research school at Weimar, Germany. The school tried to combine crafts and fine arts.",
                Answer = false,
                AnswerText = "Bauhaus is a research school at Weimar, Germany. It greatly influenced modern thought.",
                Category = "Culture/Art"
            };
            questions.Add(q1313);

            OXQuizMetadata q1314 = new OXQuizMetadata()
            {
                Id = 1314,
                QuestionText = "Warts on human skin are infectious.",
                Answer = true,
                AnswerText = "Warts caused by viruses are infectious.",
                Category = "General Knowledge"
            };
            questions.Add(q1314);

            OXQuizMetadata q1315 = new OXQuizMetadata()
            {
                Id = 1315,
                QuestionText = "Wassily Kandinsky was born in Poland.",
                Answer = false,
                AnswerText = "Wassily Kandinsky was born in Russia.",
                Category = "Culture/Art"
            };
            questions.Add(q1315);

            OXQuizMetadata q1316 = new OXQuizMetadata()
            {
                Id = 1316,
                QuestionText = "Water, when frozen, shrinks in volume.",
                Answer = false,
                AnswerText = "Water, when frozen, expands in volume.",
                Category = "Science"
            };
            questions.Add(q1316);

            OXQuizMetadata q1317 = new OXQuizMetadata()
            {
                Id = 1317,
                QuestionText = "Watermelons are not vegetables.",
                Answer = false,
                AnswerText = "Watermelons are classified both as fruits and vegetables.",
                Category = "General Knowledge"
            };
            questions.Add(q1317);

            OXQuizMetadata q1318 = new OXQuizMetadata()
            {
                Id = 1318,
                QuestionText = "Weightlifting can be divided into two subcategories: singles and doubles.",
                Answer = false,
                AnswerText = "Weightlifting can be divided into two subcategories: snatch & clean and jerks.",
                Category = "General Knowledge"
            };
            questions.Add(q1318);

            OXQuizMetadata q1319 = new OXQuizMetadata()
            {
                Id = 1319,
                QuestionText = "Whales are fish.",
                Answer = false,
                AnswerText = "Whales look like fish but are mammals. They give live birth, breathe with their lungs, and are warm-blooded.",
                Category = "General Knowledge"
            };
            questions.Add(q1319);

            OXQuizMetadata q1320 = new OXQuizMetadata()
            {
                Id = 1320,
                QuestionText = "Whales are mammals.",
                Answer = true,
                AnswerText = "Whales look like fish but are mammals. They give birth to their progeny, breathe with their lungs, and are warm-blooded.",
                Category = "General Knowledge"
            };
            questions.Add(q1320);

            OXQuizMetadata q1321 = new OXQuizMetadata()
            {
                Id = 1321,
                QuestionText = "When cooking oil and water are mixed, they separate and the oil sinks.",
                Answer = false,
                AnswerText = "The oil always floats to the top because it is less dense than water.",
                Category = "General Knowledge"
            };
            questions.Add(q1321);

            OXQuizMetadata q1322 = new OXQuizMetadata()
            {
                Id = 1322,
                QuestionText = "When one's eyes are irritated, it's a good idea to wash them with salt water.",
                Answer = false,
                AnswerText = "The saline solution used in ophthalmology is disinfected. Never wash your eyes with salt water.",
                Category = "Science"
            };
            questions.Add(q1322);

            OXQuizMetadata q1323 = new OXQuizMetadata()
            {
                Id = 1323,
                QuestionText = "White blood cells carry oxygen.",
                Answer = false,
                AnswerText = "Red blood cells carry oxygen.",
                Category = "Science"
            };
            questions.Add(q1323);

            OXQuizMetadata q1324 = new OXQuizMetadata()
            {
                Id = 1324,
                QuestionText = "WHO stands for International Monetary Fund.",
                Answer = false,
                AnswerText = "IMF stands for International Monetary Fund.",
                Category = "General Knowledge"
            };
            questions.Add(q1324);

            OXQuizMetadata q1325 = new OXQuizMetadata()
            {
                Id = 1325,
                QuestionText = "WI-FI is a technology that allows one to use wireless internet around the area where a Wireless Access Point is installed.",
                Answer = true,
                AnswerText = "Wi-Fi is a technology that enables local wireless connectivity via radio signals or infrared rays.",
                Category = "IT"
            };
            questions.Add(q1325);

            OXQuizMetadata q1326 = new OXQuizMetadata()
            {
                Id = 1326,
                QuestionText = "William Shakespeare wrote the original \"Romeo and Juliet.\"",
                Answer = true,
                AnswerText = "It's one of his well-known dramas.",
                Category = "Culture/Art"
            };
            questions.Add(q1326);

            OXQuizMetadata q1327 = new OXQuizMetadata()
            {
                Id = 1327,
                QuestionText = "Wind Mystic Lapin in the Caustic Garden is a white rabbit.",
                Answer = true,
                AnswerText = "Wind Mystic Lapin in the Caustic Garden is a whtie rabbit.",
                Category = "MapleStory 2"
            };
            questions.Add(q1327);

            OXQuizMetadata q1328 = new OXQuizMetadata()
            {
                Id = 1328,
                QuestionText = "Wolfgang Amadeus Mozart and Johann Sebastian Bach have met each other.",
                Answer = false,
                AnswerText = "Wolfgang Amadeus Mozart (1-27-1756 - 12-5-1791) and Johann Sebastian Bach (3-21-1685 - 7-28-1750) have never met each other.",
                Category = "Culture/Art"
            };
            questions.Add(q1328);

            OXQuizMetadata q1329 = new OXQuizMetadata()
            {
                Id = 1329,
                QuestionText = "World of Warcraft is an MMORPG developed by Blizzard Entertainment.",
                Answer = true,
                AnswerText = "World of Warcraft is an MMORPG developed by Blizzard Entertainment.",
                Category = "Culture/Art"
            };
            questions.Add(q1329);

            OXQuizMetadata q1330 = new OXQuizMetadata()
            {
                Id = 1330,
                QuestionText = "World War II is what led to the Treaty of Versailles.",
                Answer = false,
                AnswerText = "World War I is what led to the Treaty of Versailles.",
                Category = "History"
            };
            questions.Add(q1330);

            OXQuizMetadata q1331 = new OXQuizMetadata()
            {
                Id = 1331,
                QuestionText = "X-rays can penetrate muscles and tissues, but not bones and metals.",
                Answer = true,
                AnswerText = "X-rays can penetrate thick papers, but not bone and metals.",
                Category = "General Knowledge"
            };
            questions.Add(q1331);

            OXQuizMetadata q1332 = new OXQuizMetadata()
            {
                Id = 1332,
                QuestionText = "You can apply to enter the Personal Arena at Level 40 or above.",
                Answer = false,
                AnswerText = "You can apply to enter the Personal Arena at Level 50 or above.",
                Category = "MapleStory 2"
            };
            questions.Add(q1332);

            OXQuizMetadata q1333 = new OXQuizMetadata()
            {
                Id = 1333,
                QuestionText = "You can change input keys during the Tutorial.",
                Answer = false,
                AnswerText = "You cannot change input keys during the Tutorial.",
                Category = "MapleStory 2"
            };
            questions.Add(q1333);

            OXQuizMetadata q1334 = new OXQuizMetadata()
            {
                Id = 1334,
                QuestionText = "You can create UGC items that look just like the costumes of your favorite animated characters.",
                Answer = false,
                AnswerText = "You will get Belma's Warning if the items are deemed to violate copyrights.",
                Category = "MapleStory 2"
            };
            questions.Add(q1334);

            OXQuizMetadata q1335 = new OXQuizMetadata()
            {
                Id = 1335,
                QuestionText = "You can create your own clothing designs for your character.",
                Answer = true,
                AnswerText = "You can create your own designs by using Design Templates sold at the Meret Market.",
                Category = "MapleStory 2"
            };
            questions.Add(q1335);

            OXQuizMetadata q1336 = new OXQuizMetadata()
            {
                Id = 1336,
                QuestionText = "You can equip weapons for other jobs.",
                Answer = true,
                AnswerText = "Some characters can be equipped with weapons for different jobs.",
                Category = "MapleStory 2"
            };
            questions.Add(q1336);

            OXQuizMetadata q1337 = new OXQuizMetadata()
            {
                Id = 1337,
                QuestionText = "You can find Balrog at the Temple of Immortals.",
                Answer = true,
                AnswerText = "You can find Balrog at the Temple of Immortals on the Darkmist Path.",
                Category = "MapleStory 2"
            };
            questions.Add(q1337);

            OXQuizMetadata q1338 = new OXQuizMetadata()
            {
                Id = 1338,
                QuestionText = "You can nominate a Star Architect for multiple players a day.",
                Answer = false,
                AnswerText = "You can only nominate one player as Star Architect a day.",
                Category = "MapleStory 2"
            };
            questions.Add(q1338);

            OXQuizMetadata q1339 = new OXQuizMetadata()
            {
                Id = 1339,
                QuestionText = "You can temporarily grow bigger when attacked by King Slime.",
                Answer = false,
                AnswerText = "Being attacked by King Slime does not enlarge you.",
                Category = "MapleStory 2"
            };
            questions.Add(q1339);

            OXQuizMetadata q1340 = new OXQuizMetadata()
            {
                Id = 1340,
                QuestionText = "You can temporarily grow smaller when attacked by King Slime.",
                Answer = true,
                AnswerText = "A certain attack from King Slime shrinks its target.",
                Category = "MapleStory 2"
            };
            questions.Add(q1340);

            OXQuizMetadata q1341 = new OXQuizMetadata()
            {
                Id = 1341,
                QuestionText = "You can update your profile only in the Character window.",
                Answer = false,
                AnswerText = "You can update your profile in the Character window or Profile Camera in the main screen.",
                Category = "MapleStory 2"
            };
            questions.Add(q1341);

            OXQuizMetadata q1342 = new OXQuizMetadata()
            {
                Id = 1342,
                QuestionText = "You can use pictures of your face to create UGC items.",
                Answer = false,
                AnswerText = "No images of real people's faces--yours or your friends--can be used for UGC.",
                Category = "MapleStory 2"
            };
            questions.Add(q1342);

            OXQuizMetadata q1343 = new OXQuizMetadata()
            {
                Id = 1343,
                QuestionText = "You cannot enter dungeons if your Dungeon Reward count is 0.",
                Answer = false,
                AnswerText = "You can enter dungeons regardless of your Dungeon Reward count.",
                Category = "MapleStory 2"
            };
            questions.Add(q1343);

            OXQuizMetadata q1344 = new OXQuizMetadata()
            {
                Id = 1344,
                QuestionText = "You have to pay a service fee to list an item you want to sell at the Black Market.",
                Answer = true,
                AnswerText = "You have to pay 1% of your item price as a security deposit to sell at the Black Market.",
                Category = "MapleStory 2"
            };
            questions.Add(q1344);

            OXQuizMetadata q1345 = new OXQuizMetadata()
            {
                Id = 1345,
                QuestionText = "You must attack the Hat of Misdirection when you find it.",
                Answer = false,
                AnswerText = "The Hat of Misdirection cannot be attacked.",
                Category = "MapleStory 2"
            };
            questions.Add(q1345);

            OXQuizMetadata q1346 = new OXQuizMetadata()
            {
                Id = 1346,
                QuestionText = "You must be at least level 47 to enter the Testing Grounds.",
                Answer = true,
                AnswerText = "You must be at least level 47 to enter the Testing Grounds.",
                Category = "MapleStory 2"
            };
            questions.Add(q1346);

            OXQuizMetadata q1347 = new OXQuizMetadata()
            {
                Id = 1347,
                QuestionText = "You must be at least level 47 to enter Training Grounds.",
                Answer = true,
                AnswerText = "You must be at least level 47 to enter Training Grounds.",
                Category = "MapleStory 2"
            };
            questions.Add(q1347);

            OXQuizMetadata q1348 = new OXQuizMetadata()
            {
                Id = 1348,
                QuestionText = "Your mother's sister is your aunt.",
                Answer = true,
                AnswerText = "An aunt is the sister of either parent.",
                Category = "General Knowledge"
            };
            questions.Add(q1348);

            OXQuizMetadata q1349 = new OXQuizMetadata()
            {
                Id = 1349,
                QuestionText = "Zero-to-60 refers to the subminiature airbags installed in a car.",
                Answer = false,
                AnswerText = "Zero-to-60 refers to the time it takes to accelerate a car from 0 to 60 mph.",
                Category = "General Knowledge"
            };
            questions.Add(q1349);

            OXQuizMetadata q1350 = new OXQuizMetadata()
            {
                Id = 1350,
                QuestionText = "Zeus the supreme god in Greek and Roman mythology, is a womanizer.",
                Answer = true,
                AnswerText = "Zeus is a notorious womanizer. His liking for women creates a lot of side stories.",
                Category = "Culture/Art"
            };
            questions.Add(q1350);

            OXQuizMetadata q1351 = new OXQuizMetadata()
            {
                Id = 1351,
                QuestionText = "Zobek manages the Troy Inn.",
                Answer = false,
                AnswerText = "Zobek manages Mon Petit Chouchou Hotel.",
                Category = "MapleStory 2"
            };
            questions.Add(q1351);

            OXQuizMetadata q1352 = new OXQuizMetadata()
            {
                Id = 1352,
                QuestionText = "Zombie Mushmom has a name tag stuck to her forehead.",
                Answer = false,
                AnswerText = "It's a paper charm that is stuck to Zombie Mushmom's forehead.",
                Category = "MapleStory 2"
            };
            questions.Add(q1352);

            OXQuizMetadata q1353 = new OXQuizMetadata()
            {
                Id = 1353,
                QuestionText = "Zurich is the capital of Switzerland.",
                Answer = false,
                AnswerText = "Bern is the capital of Switzerland.",
                Category = "Society"
            };
            questions.Add(q1353);

            return questions;
        }
    }
}
