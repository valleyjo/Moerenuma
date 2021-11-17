namespace Moerenuma.Test.UnitTesting
{
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        public static TaskAssertions<Task<TResult>> Should<TResult>(this Task<TResult> subject)
        {
            return new TaskAssertions<Task<TResult>>(subject);
        }

        public static TaskAssertions<Task> Should(this Task subject)
        {
            return new TaskAssertions<Task>(subject);
        }

        public static TResult NoWaitResult<TResult>(this Task<TResult> subject)
        {
            return subject.Should().BeCompletedSuccessfully().Which.Result;
        }
    }
}
