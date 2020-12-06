﻿using QAToolKit.Auth.Exceptions;
using System;
using Xunit;

namespace QAToolKit.Auth.Test.Keycloak.Exceptions
{
    public class KeycloakAccessDeniedExceptionTests : Exception
    {
        [Fact]
        public void CreateExceptionTest_Successful()
        {
            var exception = new KeycloakAccessDeniedException("my error");

            Assert.Equal("my error", exception.Message);
        }

        [Fact]
        public void CreateExceptionWithInnerExceptionTest_Successful()
        {
            var innerException = new Exception("Inner");
            var exception = new KeycloakAccessDeniedException("my error", innerException);

            Assert.Equal("my error", exception.Message);
            Assert.Equal("Inner", innerException.Message);
        }
    }
}
