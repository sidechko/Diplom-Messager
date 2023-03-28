using System;

namespace MessagerClient.models
{
    public interface IMessagerDateTimeJsonConverter
    {
        bool CanConvert(Type typeToConvert);
    }
}