namespace Locators.Exceptions {
    /// <summary>
    /// Is thrown when the driver could not send the email
    /// </summary>
    /// <param name="message">Exception message</param>
    public class SendEmailFailedException(string message) : Exception(message) {
    }
}
