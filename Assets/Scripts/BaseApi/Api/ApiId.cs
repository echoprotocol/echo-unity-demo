using System;
using Base.Requests;
using Base.Responses;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Action;


namespace Base.Api
{
    public abstract class ApiId
    {
        private readonly ISender sender;


        protected int? Id { get; private set; }

        protected ApiId(int? id, ISender sender)
        {
            this.sender = sender;
            Id = id;
        }

        private Action<Response> GenerateRequestCallback<T>(Action<T> resolve, Action<Exception> reject, string responseTitle, bool debug, Action<Response> preProcessorCallback = null, Action<Response> postProcessorCallback = null)
        {
            return response =>
            {
                preProcessorCallback.SafeInvoke(response);
                if (debug)
                {
                    response.PrintLog(responseTitle);
                }
                response.SendResultData(resolve, reject);
                postProcessorCallback.SafeInvoke(response);
            };
        }

        protected void DoRequestVoid(int requestId, Parameters parameters, Action resolve, Action<Exception> reject, string title, bool debug)
        {
            DoRequest(requestId, parameters, resolve.IsNull() ? (Action<object>)null : result => resolve(), reject, title, debug);
        }

        protected void DoRequest<T>(int requestId, Parameters parameters, Action<T> resolve, Action<Exception> reject, string title, bool debug)
        {
            sender.Send((!resolve.IsNull() || !reject.IsNull()) ? new RequestCallback(requestId, parameters, GenerateRequestCallback(resolve, reject, title, debug), title, debug) : new Request(requestId, parameters, title, debug));
        }

        protected int GenerateNewId() => sender.Identificators.GenerateNewId();

        public bool IsInitialized => Id.HasValue;

        protected ApiId Init(int id)
        {
            Id = id;
            return this;
        }
    }
}