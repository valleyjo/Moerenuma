namespace Moerenuma.Test.UnitTesting
{
    using System;
    using System.Threading.Tasks;

    public static class TaskFixture
    {
        public static Task Completed()
        {
            return TaskTFixture.Completed(false);
        }

        public static Task Pending()
        {
            return TaskTFixture.Pending<bool>();
        }

        public static Task Faulted(params Exception[] exceptions)
        {
            return TaskTFixture.Faulted<bool>(exceptions);
        }

        public static Task Canceled()
        {
            return TaskTFixture.Canceled<bool>();
        }
    }
}
