namespace Locators.Exceptions {
    /// <summary>
    /// Is thrown when the driver cannot log in the service
    /// </summary>
    /// <param name="message">Exception message</param>
    public class LoginFailedException(string message) : Exception(message) {
    }
}
