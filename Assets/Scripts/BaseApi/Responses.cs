using System.Collections.Generic;
using Base.Data.Json;
using Base.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tools;


namespace Base.Responses
{
    public sealed class Response
    {
        private readonly string rawData;
        private readonly JObject jsonObject;


        private Response(string rawData)
        {
            jsonObject = JObject.Parse(rawData ?? string.Empty);
            this.rawData = rawData;
        }

        public static Response Parse(string data) => data.IsNullOrEmpty() ? null : new Response(data);

        public void SendResultData<T>(System.Action<T> resolve, System.Action<System.Exception> reject = null, bool isProcessed = true)
        {
            if (resolve != null && IsResult)
            {
                resolve.Invoke(jsonObject.ToObject<Result>().GetData<T>());
            }
            else
            if (reject != null && IsError)
            {
                reject.Invoke(jsonObject.ToObject<Error>().ToException());
            }
            IsProcessed = isProcessed;
        }

        public void SendNoticeData(System.Action<JToken[]> callback, bool isProcessed = true)
        {
            if (callback != null && IsNotice)
            {
                callback.Invoke(jsonObject.ToObject<Notice>().Results);
            }
            IsProcessed = isProcessed;
        }

        private bool IsError => Error.IsInstance(jsonObject);

        private bool IsResult => Result.IsInstance(jsonObject);

        private bool IsNotice => Notice.IsInstance(jsonObject);

        public int RequestId
        {
            get
            {
                if (IsError)
                {
                    return jsonObject.ToObject<Error>().ForRequestId;
                }
                if (IsResult)
                {
                    return jsonObject.ToObject<Result>().ForRequestId;
                }
                if (IsNotice)
                {
                    return jsonObject.ToObject<Notice>().SubscribeId;
                }
                return RequestIdentificator.INVALID_ID;
            }
        }

        public bool IsProcessed { get; private set; }

        public override string ToString() => rawData;

        public void PrintLog(string title)
        {
            if (IsError)
            {
                CustomTools.Console.DebugError(CustomTools.Console.SetYellowColor(title), CustomTools.Console.SetRedColor("<<<---"), CustomTools.Console.SetWhiteColor(ToString()));
            }
            else
            if (IsResult)
            {
                CustomTools.Console.DebugLog(CustomTools.Console.SetYellowColor(title), CustomTools.Console.SetRedColor("<<<---"), CustomTools.Console.SetWhiteColor(ToString()));
            }
            else
            if (IsNotice)
            {
                CustomTools.Console.DebugLog(CustomTools.Console.SetCyanColor(title), CustomTools.Console.SetRedColor("<<<---"), CustomTools.Console.SetWhiteColor(ToString()));
            }
        }

        public static Response Open(string url) => new Response(JsonConvert.SerializeObject(Result.Open(url)));

        public static Response Close(string reason) => new Response(JsonConvert.SerializeObject(Result.Close(reason)));
    }


    public sealed class Result
    {
        private const string ID_FIELD_KEY = "id";
        private const string RESULT_FIELD_KEY = "result";

        [JsonProperty(ID_FIELD_KEY)]
        private int id;
        [JsonProperty(RESULT_FIELD_KEY)]
        private JToken result;


        public int ForRequestId => id;

        public T GetData<T>() => result.ToObject<T>();

        public static Result Open(string url)
        {
            return new Result { id = RequestIdentificator.OPEN_ID, result = JToken.FromObject(url) };
        }

        public static Result Close(string reason)
        {
            return new Result { id = RequestIdentificator.CLOSE_ID, result = JToken.FromObject(reason) };
        }

        public static bool IsInstance(JObject jsonObject)
        {
            return new List<JProperty>(jsonObject.Properties()).Exists(p => RESULT_FIELD_KEY.Equals(p.Name));
        }
    }


    public sealed class Notice
    {
        private sealed class ParametersConverter : JsonCustomConverter<Parameters, JArray>
        {
            protected override Parameters Deserialize(JArray value, System.Type objectType)
            {
                if (value.IsNullOrEmpty() || value.Count != 2)
                {
                    return null;
                }
                var id = System.Convert.ToInt32(value.First);
                value = value.Last.ToObject<JArray>();
                if (!value.IsNullOrEmpty() && value.First.Type.Equals(JTokenType.Array))
                {
                    value = value.First.ToObject<JArray>();
                }
                return new Parameters(id, value.ToObject<JToken[]>());
            }

            protected override JArray Serialize(Parameters value) => new JArray(value.Id, value.Results);
        }


        [JsonConverter(typeof(ParametersConverter))]
        private sealed class Parameters
        {
            public int Id { get; private set; }
            public JToken[] Results { get; private set; }

            public Parameters(int id, JToken[] results)
            {
                Id = id;
                Results = results;
            }
        }


        private const string METHOD_FIELD_KEY = "method";
        private const string NOTICE_FIELD_KEY = "notice";
        private const string PARAMS_FIELD_KEY = "params";

        [JsonProperty(METHOD_FIELD_KEY)]
        private string method;
        [JsonProperty(PARAMS_FIELD_KEY)]
        private Parameters parameters;


        public int SubscribeId => parameters.Id;

        public JToken[] Results => parameters.Results;

        public static bool IsInstance(JObject jsonObject)
        {
            return new List<JProperty>(jsonObject.Properties()).Exists(p => METHOD_FIELD_KEY.Equals(p.Name) && NOTICE_FIELD_KEY.Equals(p.Value.ToString()));
        }
    }


    public sealed class Error
    {
        public class WrappedErrorException : System.Exception
        {
            public Error Error { get; private set; }

            public WrappedErrorException(Error error) : base(error.ToString())
            {
                Error = error;
            }
        }


        private const string ID_FIELD_KEY = "id";
        private const string ERROR_FIELD_KEY = "error";
        private const string DATA_FIELD_KEY = "data";

        [JsonProperty(ID_FIELD_KEY)]
        private int id;
        [JsonProperty(ERROR_FIELD_KEY)]
        private JObject data;


        public int ForRequestId => id;

        public WrappedErrorException ToException() => new WrappedErrorException(this);

        public string Data
        {
            get
            {
                var token = JToken.FromObject(string.Empty);
                return data.TryGetValue(DATA_FIELD_KEY, out token) ? token.ToString() : string.Empty;
            }
        }

        public override string ToString() => Data;

        public static bool IsInstance(JObject jsonObject)
        {
            return new List<JProperty>(jsonObject.Properties()).Exists(p => ERROR_FIELD_KEY.Equals(p.Name));
        }
    }
}