using System;
using System.Xml;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class OXQuizMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public string Category;
        [XmlElement(Order = 3)]
        public string QuestionText;
        [XmlElement(Order = 4)]
        public string AnswerText;
        [XmlElement(Order = 5)]
        public bool Answer;

        public OXQuizMetadata() { }
    }
}
