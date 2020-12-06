using System;

namespace QAToolKit.Auth.Exceptions
{
    /// <summary>
    /// Keycloak unauthorized client exception
    /// </summary>
    [Serializable]
    public class KeycloakUnauthorizedClientException : Exception
    {
        /// <summary>
        /// Keycloak unauthorized client exception
        /// </summary>
        /// <param name="message"></param>
        public KeycloakUnauthorizedClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Keycloak unauthorized client exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public KeycloakUnauthorizedClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
