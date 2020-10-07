using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Data
{
    public class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
