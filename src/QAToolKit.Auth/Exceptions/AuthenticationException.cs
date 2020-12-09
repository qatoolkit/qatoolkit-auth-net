using System;

namespace QAToolKit.Auth.Exceptions
{
    /// <summary>
    /// Keycloak exception
    /// </summary>
    [Serializable]
    public class AuthenticationException : Exception
    {
        /// <summary>
        /// Keycloak exception
        /// </summary>
        /// <param name="message"></param>
        public AuthenticationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Keycloak exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
