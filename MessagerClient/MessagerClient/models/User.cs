using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessagerClient.models
{
    public class User : IModelBase<User>
    {
        [property: JsonPropertyName("id")] public int? Id { get; set; }
        [property: JsonPropertyName("login")] public string? Login { get; set; }
        [property: JsonPropertyName("password")] public string? Password { get; set; }
        [property: JsonPropertyName("regTime")] public DateTime? RegTime { get; set; }
        public bool IsNull()
        {
            return Id is null && Login is null && Password is null && RegTime is null;
        }

        public void Update(User? obj, bool soft = true)
        {
            if (obj is null)
                return;
            if (obj.Id is not null)
                this.Id = obj.Id;
            if (obj.Login is not null)
                this.Login = obj.Login;
            if (obj.Password is not null)
                this.Password = obj.Password;
            if (obj.RegTime is not null)
                this.RegTime = obj.RegTime;
        }
    }
}
