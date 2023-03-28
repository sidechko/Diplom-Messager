using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessagerClient.models
{
    public class UserChannelLink : IModelBase<UserChannelLink>
    {
        [property: JsonPropertyName("id")] public int? Id { get; set; }
        [property: JsonPropertyName("user")] public User? User { get; set; }
        [property: JsonPropertyName("channel")] public Channel? Channel { get; set; }


        public bool IsNull()
        {
            return Id is null && (Channel is null || Channel.IsNull()) && (User is null || User.IsNull());
        }
    }
}
