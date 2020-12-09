using System;

namespace QAToolKit.Auth.Exceptions
{
    /// <summary>
    /// Keycloak unauthorized client exception
    /// </summary>
    [Serializable]
    public class UnauthorizedClientException : Exception
    {
        /// <summary>
        /// Keycloak unauthorized client exception
        /// </summary>
        /// <param name="message"></param>
        public UnauthorizedClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Keycloak unauthorized client exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnauthorizedClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
