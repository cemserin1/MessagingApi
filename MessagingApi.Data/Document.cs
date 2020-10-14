using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using MessagingApi.Data.Interfaces;
using MongoDB.Bson;

namespace MessagingApi.Data
{
    public abstract class Document : IDocument
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
