using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Data
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
