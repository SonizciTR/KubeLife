using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Data.S3.Model
{
    public class MinioReleaseableFileStreamModel : IDisposable
    {
        private readonly TaskCompletionSource _setStreamTaskCompletionSource = new();
        private readonly TaskCompletionSource _releaseTaskCompletionSource = new();

        private bool _set;
        private bool _disposedValue;

        public string FileName { get; init; } = null!;

        public string ContentType { get; init; } = null!;

        public Stream StreamHolder { get; set; } = null!;

        /// <summary>
        /// Set <see cref="StreamHolder"/>
        /// and return <see cref="Task"/> for awaiting of the Stream usage finish
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task SetStreamAsync(Stream stream, CancellationToken token)
        {
            if (StreamHolder != null)
            {
                throw new InvalidOperationException("Stream is already set.");
            }

            _set = true;
            StreamHolder = stream;
            
            _setStreamTaskCompletionSource.SetResult();
            return _releaseTaskCompletionSource.Task;
        }

        /// <summary>
        /// Set <see cref="StreamHolder"/>
        /// and return <see cref="Task"/> for awaiting of the Stream usage finish
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task SetStreamAsync(Stream stream)
            => SetStreamAsync(stream, CancellationToken.None);

        /// <summary>
        /// Release <see cref="StreamHolder"/>
        /// </summary>
        public void Release()
        {
            _releaseTaskCompletionSource.SetResult();
        }

        /// <summary>
        /// Process <paramref name="task"/>, that sets <see cref="StreamHolder"/>,
        /// and return <see cref="Task"/> that awaits setting of <see cref="StreamHolder"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public Task HandleAsync<T>(Task<T> task)
        {
            task.ContinueWith(
                HandleFail<T>(),
                CancellationToken.None,
                TaskContinuationOptions.AttachedToParent,
                TaskScheduler.Current);

            return StreamSetTask;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Release();
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// <see cref="Task"/> that awaits setting of <see cref="StreamHolder"/>
        /// </summary>
        private Task StreamSetTask => _setStreamTaskCompletionSource.Task;

        private Action<Task<T>> HandleFail<T>()
            => t
                =>
            {
                var ex = t.Exception;
                if (ex != null)
                {
                    _setStreamTaskCompletionSource.TrySetException(ex);
                    _releaseTaskCompletionSource.TrySetException(ex);
                }
                else
                {
                    if (!_set)
                    {
                        _setStreamTaskCompletionSource.TrySetException(new OperationCanceledException());
                    }

                    _releaseTaskCompletionSource.TrySetResult();
                }
            };
    }

}
