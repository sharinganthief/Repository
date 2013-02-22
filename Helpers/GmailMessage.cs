using System.Net;
using System.Net.Mail;


namespace Helpers
{

    public static class GmailMessage
    {
        private static readonly MailAddress FromAddress = new MailAddress("phillipjberglund@gmail.com", "From Name");
        private static readonly MailAddress ToAddress = new MailAddress("phillipjberglund@gmail.com", "To Name");
        private const string FromPassword = "amandabear123?";

        public static bool SendMessage(string messageToSend)
        {
            return SendMessage(messageToSend, string.Empty);
        }

        public static bool SendMessage(string messageToSend, string subject)
        {
            return SendMessage(messageToSend, subject, FromAddress, FromPassword);
        }

        private static bool SendMessage(string messageToSend, string subject, MailAddress fromAddress,
                                        string fromPassword)
        {
            return SendMessage(messageToSend, subject, fromAddress, fromPassword, ToAddress);
        }

        private static bool SendMessage(string messageToSend, string subject, MailAddress fromAddress,
                                        string fromPassword, MailAddress toAddress)
        {
            MailMessage message = new MailMessage(fromAddress, toAddress)
                                      {
                                          Subject = string.IsNullOrWhiteSpace(subject) ? "No subject supplied" : subject,
                                          Body = string.IsNullOrWhiteSpace(messageToSend) ? "No body supplied" : messageToSend
                                      };

            using (SmtpClient smtp = new SmtpClient
                                         {
                                             Host = "smtp.gmail.com",
                                             Port = 587,
                                             EnableSsl = true,
                                             DeliveryMethod = SmtpDeliveryMethod.Network,
                                             UseDefaultCredentials = false,
                                             Credentials =
                                                 new NetworkCredential(fromAddress.Address, fromPassword)
                                         })
            {
                smtp.Send(message);
            }

            return true;
        }
    }
}