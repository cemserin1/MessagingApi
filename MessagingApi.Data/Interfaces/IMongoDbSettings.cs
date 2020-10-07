using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Data
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
