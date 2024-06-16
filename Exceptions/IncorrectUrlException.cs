namespace Locators.Exceptions {
    /// <summary>
    /// Is thrown when the driver starts executing not in the intended URL address
    /// </summary>
    /// <param name="message">Exception message</param>
    internal class IncorrectUrlException(string message) : Exception(message) {
    }
}
