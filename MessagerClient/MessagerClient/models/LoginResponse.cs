using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessagerClient.models
{
    public class LoginResponse : IModelBase<LoginResponse>
    {
        [property: JsonPropertyName("user")] public User? User { get; set; }
        [property: JsonPropertyName("sessionId")] public int? SessionId { get; set; }
        public bool IsNull()
        {
            return (User is null || User.IsNull()) && SessionId is null;
        }
    }
}
