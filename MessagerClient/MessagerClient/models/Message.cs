using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessagerClient.models
{
    public class Message : IModelBase<Message>
    {
        [property: JsonPropertyName("id")] public int? Id { get; set; }
        [property: JsonPropertyName("channel")] public Channel? Channel { get; set; }
        [property: JsonPropertyName("content")] public string? Content { get; set; }
        [property: JsonPropertyName("sender")] public User? Sender { get; set; }
        [property: JsonPropertyName("updateTime")] public DateTime? UpdTime { get; set; }
        [property: JsonPropertyName("sendTime")] public DateTime? SendTime { get; set; }
        public bool IsNull()
        {
            return Id is null && (Channel is null || Channel.IsNull()) && (Sender is null || Sender.IsNull()) && UpdTime is null && SendTime is null;
        }

        public void Update(Message? obj, bool soft = true)
        {
            if (obj is null)
                return;
            if (obj.Id is not null)
                this.Id = obj.Id;
            if(obj.Channel is not null && !obj.Channel.IsNull())
            {
                if (this.Channel is null)
                    this.Channel = obj.Channel;
                else if(soft)
                    this.Channel.Update(obj.Channel);
                else
                    this.Channel = obj.Channel;
            }
            if(obj.Sender is not null && !obj.Sender.IsNull())
            {
                if (this.Sender is null)
                    this.Sender = obj.Sender;
                else if (soft)
                    this.Sender.Update(obj.Sender);
                else
                    this.Sender = obj.Sender;
            }
            if (obj.UpdTime is not null)
                this.UpdTime = obj.UpdTime;
            if (obj.SendTime is not null)
                this.SendTime = obj.SendTime;
        }
    }
}
