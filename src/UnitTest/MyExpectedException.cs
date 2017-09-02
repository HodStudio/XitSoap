using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace HodStudio.XitSoap.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class MyExpectedException : ExpectedExceptionBaseAttribute
    {
        private readonly Type _expectedExceptionType;
        private readonly string _expectedExceptionMessage;

        public MyExpectedException(Type expectedExceptionType)
        {
            _expectedExceptionType = expectedExceptionType;
            _expectedExceptionMessage = string.Empty;
        }

        public MyExpectedException(Type expectedExceptionType, string expectedExceptionMessage)
        {
            _expectedExceptionType = expectedExceptionType;
            _expectedExceptionMessage = expectedExceptionMessage;
        }

        protected override void Verify(Exception exception)
        {
            Assert.IsNotNull(exception);

            Assert.IsInstanceOfType(exception, _expectedExceptionType, "Wrong type of exception was thrown.");

            if (!string.IsNullOrEmpty(_expectedExceptionMessage))
            {
                Assert.IsTrue(exception.Message.Contains(_expectedExceptionMessage), "Wrong exception message was returned.");
            }
        }
    }
}
