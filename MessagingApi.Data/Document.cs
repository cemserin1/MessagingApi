using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MessagingApi.Data
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
