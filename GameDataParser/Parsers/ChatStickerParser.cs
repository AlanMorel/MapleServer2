using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ChatStickerParser : Exporter<List<ChatStickerMetadata>>
    {
        public ChatStickerParser(MetadataResources resources) : base(resources, "chat-sticker") { }

        protected override List<ChatStickerMetadata> Parse()
        {
            // Iterate over preset objects to later reference while iterating over exported maps
            List<ChatStickerMetadata> chatStickers = new List<ChatStickerMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {

                if (!entry.Name.StartsWith("table/chatemoticon"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    ChatStickerMetadata metadata = new ChatStickerMetadata();

                    if (node.Name == "chatEmoticon")
                    {
                        metadata.StickerId = int.Parse(node.Attributes["id"].Value);
                        metadata.GroupId = byte.Parse(node.Attributes["group_id"].Value);
                        metadata.CategoryId = short.Parse(node.Attributes["category_id"].Value);
                    }

                    chatStickers.Add(metadata);
                }
            }
            return chatStickers;
        }
    }
}
