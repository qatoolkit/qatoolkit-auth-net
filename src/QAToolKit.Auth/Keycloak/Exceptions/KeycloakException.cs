using System;

namespace QAToolKit.Auth.Exceptions
{
    /// <summary>
    /// Keycloak exception
    /// </summary>
    public class KeycloakException : Exception
    {
        /// <summary>
        /// Keycloak exception
        /// </summary>
        /// <param name="message"></param>
        public KeycloakException(string message) : base(message)
        {
        }

        /// <summary>
        /// Keycloak exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public KeycloakException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
