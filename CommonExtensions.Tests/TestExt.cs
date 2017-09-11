using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonExtensions.Tests
{
    public static partial class TestExt
    {
        public static Action AssertThrows(this Action code, Type expectedExceptionType = null, Action cleanup = null)
        {
            code.ThrowIfNull("code");
            if (expectedExceptionType != null && !typeof(Exception).IsAssignableFrom(expectedExceptionType))
                throw new ArgumentException("Type must be derived from System.Exception", "expectedExceptionType");

            try
            {
                code.Invoke();
                if (expectedExceptionType == null)
                    Assert.Fail("Expected an exception, but none was thrown.");
                else
                    Assert.Fail("Expected exception of type '{0}', but none was thrown.", expectedExceptionType.Name);
            }
            catch (Exception ex)
            {
                var exType = ex.GetType();
                if (exType.Equals(typeof(AssertFailedException))) throw ex;

                if (expectedExceptionType != null)
                {
                    Assert.IsTrue(exType.Equals(expectedExceptionType),
                        "Expected exception of type '{0}', but exception of type '{1}' was thrown instead.",
                        expectedExceptionType.Name, exType.Name);
                }
            }
            finally
            {
                if(cleanup != null)
                    cleanup.Invoke();
            }
            return code;
        }

        public static Action AssertThrows<TException>(this Action code, Action cleanup = null) where TException : Exception
        {
            return AssertThrows(code, typeof(TException), cleanup);       
        }

        public static Action AssertThrows(this Action code, IEnumerable<Type> expectedExceptionTypes, Action cleanup = null)
        {
            code.ThrowIfNull("code");
            expectedExceptionTypes.ThrowIfNull("expectedExceptionType");

            try
            {
                code.Invoke();
                Assert.Fail("Expected an exception, but none was thrown.");
            }
            catch (Exception ex)
            {
                var exType = ex.GetType();
                if (exType.Equals(typeof(AssertFailedException))) throw ex;

                Assert.IsTrue(expectedExceptionTypes.Contains(exType),
                    "Was expecting a known exception type, but exception of type '{0}' was thrown instead.",
                    exType.Name);
            }
            finally
            {
                if (cleanup != null)
                    cleanup.Invoke();
            }
            return code;
        }
    }   
}
