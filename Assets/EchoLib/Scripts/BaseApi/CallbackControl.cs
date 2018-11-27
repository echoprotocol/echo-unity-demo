using System;
using System.Collections.Generic;
using Base.Requests;
using Base.Responses;


namespace Base.Eventing
{
    public sealed class CallbackControl : IDisposable
    {
        private readonly Dictionary<int, Action<Response>> requestCallbacks;
        private readonly Dictionary<int, Action<Response>> requestInitializers;
        private readonly Dictionary<int, Action<Response>> regularCallbacks;


        public CallbackControl()
        {
            requestCallbacks = new Dictionary<int, Action<Response>>();
            requestInitializers = new Dictionary<int, Action<Response>>();
            regularCallbacks = new Dictionary<int, Action<Response>>();
        }

        public void AddRegularCallback(int eventId, Action<Response> action)
        {
            regularCallbacks[eventId] = action;
        }

        public void RemoveRegularCallback(int eventId)
        {
            regularCallbacks.Remove(eventId);
        }

        public void SetRequestCallback(RequestAction request)
        {
            requestCallbacks[request.RequestId] = request.Callback;
            requestInitializers[request.RequestId] = request.Initializer;
        }

        public void ResetRequestCallback(int eventId)
        {
            requestCallbacks.Remove(eventId);
            requestInitializers.Remove(eventId);
        }

        public void Clear()
        {
            regularCallbacks.Clear();
            requestCallbacks.Clear();
            requestInitializers.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Clear();
        }

        public bool InvokeCallback(Response response)
        {
            if (regularCallbacks.ContainsKey(response.RequestId))
            {
                regularCallbacks[response.RequestId].Invoke(response);
                return true;
            }
            if (requestCallbacks.ContainsKey(response.RequestId))
            {
                requestCallbacks[response.RequestId].Invoke(response);
                if (response.IsProcessed)
                {
                    ResetRequestCallback(response.RequestId);
                }
                return true;
            }
            return false;
        }

        public bool InvokeInitializer(Response response)
        {
            if (requestInitializers.ContainsKey(response.RequestId))
            {
                requestInitializers[response.RequestId].Invoke(response);
                return true;
            }
            return false;
        }
    }
}