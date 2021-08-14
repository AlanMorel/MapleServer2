using System;
using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Types;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class OXQuizMetadataStorage
    {
        private static readonly Dictionary<int, OXQuizMetadata> questions = new Dictionary<int, OXQuizMetadata>();

        static OXQuizMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-ox-quiz-metadata");
            List<OXQuizMetadata> items = Serializer.Deserialize<List<OXQuizMetadata>>(stream);
            foreach (OXQuizMetadata item in items)
            {
                questions[item.Id] = item;
            }
        }

        private static OXQuizMetadata GetQuestion()
        {
            Random random = new Random();
            int index = random.Next(questions.Count);
            return questions[index];
        }
    }
}
