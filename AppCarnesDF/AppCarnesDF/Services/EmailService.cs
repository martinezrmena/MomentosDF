using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models;
using System;
using System.Threading.Tasks;

namespace AppCarnesDF.Services
{
    public class EmailService
    {
        public ParametizacionesModel Parametizaciones { get; set; }
        public LogMessageAttention Message = new LogMessageAttention();

        public void SetAtributes(ParametizacionesModel pParametizaciones)
        {
            Parametizaciones = pParametizaciones;
        }

        public async Task<int> SendEmail(string subject, string body, string recipient)
        {
            int result = 0;

            try
            {
                //SmtpClient SmtpServer = new SmtpClient(Parametizaciones.Client);
                //var mail = new MailMessage();
                //mail.From = new MailAddress(Parametizaciones.User);
                //mail.To.Add(recipient);
                //mail.Subject = subject ;
                //mail.IsBodyHtml = true;
                //mail.Body = body;
                //SmtpServer.Port = Parametizaciones.Port;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential(Parametizaciones.User, Parametizaciones.Pass);
                //SmtpServer.EnableSsl = true;
                //SmtpServer.Send(mail);
                //await Task.Delay(1000);
                return 1;
            }
            catch (Exception ex)
            {
                
            }

            return result;
        }

    }
}
