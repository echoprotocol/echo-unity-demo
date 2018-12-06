using System;
using Base.Data.Witnesses;
using Base.Data.Workers;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Array;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class VotesConverter : JsonCustomConverter<IdObject[], JArray>
    {
        protected override IdObject[] Deserialize(JArray value, Type objectType)
        {
            var result = new IdObject[value.Count];
            for (var i = 0; i < result.Length; i++)
            {
                var sample = value[i].ToObject<IdObject>();
                if (sample.Id.IsNullOrEmpty())
                {
                    CustomTools.Console.DebugWarning("Get unexpected vote object:", value.ToString());
                    result[i] = null;
                }
                else
                {
                    switch (sample.SpaceType)
                    {
                        case SpaceType.CommitteeMember:
                            result[i] = value[i].ToObject<CommitteeMemberObject>();
                            break;
                        case SpaceType.Witness:
                            result[i] = value[i].ToObject<WitnessObject>();
                            break;
                        case SpaceType.Worker:
                            result[i] = value[i].ToObject<WorkerObject>();
                            break;
                        default:
                            CustomTools.Console.DebugWarning("Get unexpected SpaceType for vote object:", CustomTools.Console.LogCyanColor(sample.SpaceType), sample.Id, '\n', value);
                            result[i] = null;
                            break;
                    }
                }
            }
            return result;
        }

        protected override JArray Serialize(IdObject[] value)
        {
            return new JArray(value.ConvertAll(item => item.IsNull() ? JToken.Parse("null") : JToken.FromObject(item)));
        }
    }
}