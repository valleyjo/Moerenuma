namespace Moerenuma.Test.UnitTesting
{
    using System;

    [Serializable]
    public class ExpectedException : Exception
    {
        public ExpectedException()
            : base("This is an expected exception for the purposes of unit testing.")
        {
        }
    }
}
