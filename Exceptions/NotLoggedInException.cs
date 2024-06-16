namespace Locators.Exceptions {
    /// <summary>
    /// Is thrown when the driver starts executing tasks where log in is required
    /// </summary>
    /// <param name="message">Exception message</param>
    internal class NotLoggedInException(string message) : Exception(message) {
    }
}
