using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Axiverse.Threading
{
    public class AsyncManualResetEvent
    {
        private volatile TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        public Task WaitAsync() { return taskCompletionSource.Task; }

        public void Set()
        {
            var tcs = taskCompletionSource;
            Task.Factory.StartNew(
                s => ((TaskCompletionSource<bool>)s).TrySetResult(true),
                tcs, CancellationToken.None,
                TaskCreationOptions.PreferFairness,
                TaskScheduler.Default);
            tcs.Task.Wait();
        }

        public void Reset()
        {
            while (true)
            {
                var tcs = taskCompletionSource;
                if (!tcs.Task.IsCompleted ||
                    Interlocked.CompareExchange(ref taskCompletionSource, new TaskCompletionSource<bool>(), tcs) == tcs)
                {
                    return;
                }
            }
        }
    }
}
