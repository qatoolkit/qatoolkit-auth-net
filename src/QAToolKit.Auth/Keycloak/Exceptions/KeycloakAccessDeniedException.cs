using System;

namespace QAToolKit.Auth.Exceptions
{
    /// <summary>
    /// Keycloak access denied exception
    /// </summary>
    [Serializable]
    public class KeycloakAccessDeniedException : Exception
    {
        /// <summary>
        /// Keycloak access denied exception
        /// </summary>
        /// <param name="message"></param>
        public KeycloakAccessDeniedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Keycloak access denied exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public KeycloakAccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
