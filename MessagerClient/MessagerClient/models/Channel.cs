using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessagerClient.models
{
    public class Channel : IModelBase<Channel>
    {
        [property: JsonPropertyName("id")] public int? Id { get; set; }
        [property: JsonPropertyName("name")] public string? Name { get; set; }
        [property: JsonPropertyName("desc")] public string? Desc { get; set; }
        public bool IsNull()
        {
            return Id is null && Name is null && Desc is null;
        }

        public void Update(Channel? obj, bool soft = true)
        {
            if (obj is null)
                return;
            if (obj.Id is not null)
                this.Id = obj.Id;
            if (obj.Name is not null)
                this.Name = obj.Name;
            if (obj.Desc is not null)
                this.Desc = obj.Desc;
        }
    }
}
