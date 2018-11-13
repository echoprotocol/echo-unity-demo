using System;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class MemoDataConverter : JsonCustomConverter<MemoData, JObject>
    {
        protected override MemoData Deserialize(JObject value, Type objectType) => MemoData.Create(value);

        protected override JObject Serialize(MemoData value) => JObject.Parse(value.ToString());
    }
}