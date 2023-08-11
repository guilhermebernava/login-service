using System.Net.Mail;
using System.Net;

namespace LoginMicroservice.Api.Utils;

public static class EmailSender
{
    public static void SendEmail(string email, string code, IConfiguration configuration)
    {
        try
        {

            Task.Run(() =>
            {
                MailMessage newMail = new();
                SmtpClient client = new("smtp.gmail.com");
                newMail.From = new MailAddress("guilhermebernava00@gmail.com", "NOT REPLY");
                newMail.To.Add(email);
                newMail.Subject = "Code to acess your account";
                newMail.IsBodyHtml = true; newMail.Body = $"<h1>Here is your code!</h1>\n<h2>Use this code to login in your account</h2>\n<h3>{code}</h3>";

                client.EnableSsl = true;
                client.Port = 587;
                client.Credentials = new NetworkCredential("guilhermebernava00@gmail.com", configuration["EmailPassword"]);
                client.Send(newMail);
            });

        }
        catch (Exception)
        {
            throw;
        }
    }
}
