#if V40 || V45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonExtensions.TaskExtensions
{
    public static partial class TaskExt
    {
        public static Task Delay(this Action action, int milliseconds)
        {
            var tcs = new TaskCompletionSource<object>();
            new System.Threading.Timer(_ => tcs.SetResult(null)).Change(milliseconds, System.Threading.Timeout.Infinite);
            return tcs.Task.ContinueWith(_ => action());
        }

        public static Task Delay<T>(this Action<T> action, int milliseconds, T input)
        {
            var tcs = new TaskCompletionSource<T>();
            new System.Threading.Timer(_ => tcs.SetResult(input)).Change(milliseconds, System.Threading.Timeout.Infinite);
            return tcs.Task.ContinueWith(task => action(task.Result));
        }
    }
}
#endif