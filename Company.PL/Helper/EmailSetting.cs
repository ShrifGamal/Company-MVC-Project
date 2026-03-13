using Company.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.PL.Helper
{
    public static class EmailSetting
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gamil.com", 587);
            client.Credentials = new NetworkCredential("Shrif2172002@gmail.com" , "ktwvokurearunpxm");

            client.Send("Shrif2172002@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
