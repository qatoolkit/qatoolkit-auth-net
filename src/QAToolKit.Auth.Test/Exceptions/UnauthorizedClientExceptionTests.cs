﻿using QAToolKit.Auth.Exceptions;
using System;
using Xunit;

namespace QAToolKit.Auth.Test.Exceptions
{
    public class UnauthorizedClientExceptionTests : Exception
    {
        [Fact]
        public void CreateExceptionTest_Successful()
        {
            var exception = new UnauthorizedClientException("my error");

            Assert.Equal("my error", exception.Message);
        }

        [Fact]
        public void CreateExceptionWithInnerExceptionTest_Successful()
        {
            var innerException = new Exception("Inner");
            var exception = new UnauthorizedClientException("my error", innerException);

            Assert.Equal("my error", exception.Message);
            Assert.Equal("Inner", innerException.Message);
        }
    }
}
