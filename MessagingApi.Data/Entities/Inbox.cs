using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using SharpCompress.Compressors.Deflate;

namespace MessagingApi.Data.Entities
{
    [BsonCollection("Inbox")]

    public class Inbox : Document
    {
        public List<Conversation> ConversationList { get; set; }

        public Inbox()
        {
            ConversationList = new List<Conversation>();
        }
    }

    public class Conversation : Document
    {
        public string ConversationId { get; set; }
        public string FromUser { get; set; }
        public List<Message> Messages { get; set; }
    }
}
