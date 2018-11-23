using System;
using CustomTools.Extensions.Core;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Json
{
    public abstract class PairConverter<TPair> : JsonCustomConverter<TPair, JArray> where TPair : class
    {
        protected override TPair Deserialize(JArray value, Type objectType)
        {
            return (value.IsNullOrEmpty() || value.Count != 2) ? null : ConvertFrom(value.First, value.Last);
        }

        protected override JArray Serialize(TPair value)
        {
            return value.IsNull() ? new JArray() : ConvertTo(value);
        }

        protected abstract TPair ConvertFrom(JToken key, JToken value);

        protected abstract JArray ConvertTo(TPair pair);
    }
}