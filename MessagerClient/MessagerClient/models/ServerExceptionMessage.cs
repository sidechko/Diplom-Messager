using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessagerClient.models
{
    public class ServerExceptionMessage : IModelBase<ServerExceptionMessage>
    {
        [property: JsonPropertyName("message")] public string Message { get; set; } = "nothing";

        public bool IsNull()
        {
            return false;
        }
    }
}
