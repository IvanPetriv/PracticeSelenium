namespace Locators.Exceptions {
    /// <summary>
    /// Is thrown when the driver tries to send incorrect email
    /// </summary>
    /// <param name="message">Exception message</param>
    internal class IncorrectEmailException(string message) : Exception(message) {
    }
}
