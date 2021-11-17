namespace Moerenuma.Test.UnitTesting
{
    using System;
    using System.Threading.Tasks;

    public static class TaskTFixture
    {
        public static Task<TResult> Completed<TResult>(TResult value)
        {
            return Task.FromResult(value);
        }

        public static Task<TResult> Pending<TResult>()
        {
            return new TaskCompletionSource<TResult>().Task;
        }

        public static Task<TResult> Faulted<TResult>(params Exception[] exceptions)
        {
            if (exceptions.Length == 0)
            {
                exceptions = new Exception[] { new ExpectedException() };
            }

            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exceptions);
            return tcs.Task;
        }

        public static Task<TResult> Canceled<TResult>()
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetCanceled();
            return tcs.Task;
        }
    }
}
