using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using KubernetesClient.Models;
using KubernetesClient.Responses;
using static KubernetesClient.WatcherDelegatingHandler;

namespace KubernetesClient
{
    public enum WatchEventType
    {
        /// <summary>Emitted when an object is created, modified to match a watch's filter, or when a watch is first opened.</summary>
        [EnumMember(Value = "ADDED")] Added,
        /// <summary>Emitted when an object is modified.</summary>
        [EnumMember(Value = "MODIFIED")] Modified,
        /// <summary>Emitted when an object is deleted or modified to no longer match a watch's filter.</summary>
        [EnumMember(Value = "DELETED")] Deleted,
        /// <summary>Emitted when an error occurs while watching resources. Most commonly, the error is 410 Gone which indicates that
        /// the watch resource version was outdated and events were probably lost. In that case, the watch should be restarted.
        /// </summary>
        [EnumMember(Value = "ERROR")] Error,
        /// <summary>Bookmarks may be emitted periodically to update the resource version. The object will
        /// contain only the resource version.
        /// </summary>
        [EnumMember(Value = "BOOKMARK")] Bookmark,
    }

    public class Watch<T> : IDisposable
    {
        private Func<Task<TextReader>> _streamReaderFactory;
        private TextReader _streamReader;

        private readonly CancellationTokenSource _cts;

        public bool Watching { get; private set; }
        /// <summary>
        /// Event raise when kubernetes api server change resource T.
        /// </summary>
        public event Action<WatchEventType, T, CancellationTokenSource> OnEvent = delegate { };
        /// <summary>
        /// Event raise when exception happen during watch.
        /// </summary>
        public event Action<Exception> OnError = delegate { };
        /// <summary>
        /// Event raise when close watch connection to kubernetes api server.
        /// </summary>
        public event Action OnClosed = delegate { };

        public Watch(
            Func<Task<TextReader>> streamReaderFactory,
            Action<WatchEventType, T, CancellationTokenSource> onEvent, Action<Exception> onError = null, Action onClosed = null,
            CancellationTokenSource cts = default)
        {
            _streamReaderFactory = streamReaderFactory;
            OnEvent += onEvent;
            OnError += onError;
            OnClosed += onClosed;

            _cts = cts;
        }

        public Watch(
            Func<Task<StreamReader>> streamReaderFactory,
            Action<WatchEventType, T, CancellationTokenSource> onEvent, Action<Exception> onError = null, Action onClosed = null, 
            CancellationTokenSource cts = default) 
            : this(async () => (TextReader)await streamReaderFactory().ConfigureAwait(false),
                    onEvent, onError, onClosed, cts)
        {
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _streamReader?.Dispose();
        }

        public async Task WatchLoop()
        {
            try
            {
                Watching = true;
                string line;
                _streamReader = await _streamReaderFactory().ConfigureAwait(false);

                // ReadLineAsync will return null when we've reached the end of the stream.
                while ((line = await _streamReader.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    if (_cts.IsCancellationRequested)
                        return;

                    try
                    {
                        var @event = JsonSerializer.Deserialize<WatchEvent>(line);

                        if (@event == null)
                        {
                            var statusEvent = JsonSerializer.Deserialize<Watch<V1Status>.WatchEvent>(line);
                            var exception = new KubernetesException(statusEvent.Object);
                            OnError?.Invoke(exception);
                        }
                        else
                        {
                            OnEvent?.Invoke(@event.Type, @event.Object, _cts);
                        }
                    }
                    catch (Exception ex)
                    {
                        // error if deserialized failed or OnEvent throws
                        OnError?.Invoke(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
            finally
            {
                Watching = false;
                OnClosed?.Invoke();
            }
        }

        public class WatchEvent
        {
            public WatchEventType Type { get; set; }
            [JsonPropertyName("object")]
            public T Object { get; set; }
        }
    }

    public static class WatchExtensions
    {
        public static Watch<T> Watch<T, TResponse>(this ValueTask<HttpResponse<TResponse>> responseTask,
            Action<WatchEventType, T, CancellationTokenSource> onEvent, Action<Exception> onError = null, Action onClosed = null, 
            CancellationTokenSource cts = default)
        {
            return new Watch<T>(async () =>
            {
                var response = await responseTask.ConfigureAwait(false);
                if (!(response.Response.Content is LineSeparatedHttpContent content))
                {
                    throw new KubernetesException("request is not watchable.");
                }
                return content.StreamReader;
            }, onEvent, onError, onClosed, cts);
        }

        public static Watch<T> Watch<T, TResponse>(this Task<HttpResponse<TResponse>> responseTask,
            Action<WatchEventType, T, CancellationTokenSource> onEvent, Action<Exception> onError = null, Action onClosed = null, 
            CancellationTokenSource cts = default)
        {
            return new Watch<T>(async () =>
            {
                var response = await responseTask.ConfigureAwait(false);
                if (!(response.Response.Content is LineSeparatedHttpContent content))
                {
                    throw new KubernetesException("request is not watchable.");
                }
                return content.StreamReader;
            }, onEvent, onError, onClosed, cts);
        }

        public static Watch<T> Watch<T, TResponse>(this HttpResponse<TResponse> response,
            Action<WatchEventType, T, CancellationTokenSource> onEvent, Action<Exception> onError = null, Action onClosed = null, 
            CancellationTokenSource cts = default)
        {
            return Watch(Task.FromResult(response), onEvent, onError, onClosed, cts);
        }
    }
}
