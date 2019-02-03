using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InambeBlog.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _server;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;

        private readonly string _fromName;
        private readonly string _fromMail;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _server = _configuration["SMTP:Server"];
            _username = _configuration["SMTP:Username"];
            _password = _configuration["SMTP:Password"];
            _port = Convert.ToInt32(_configuration["SMTP:Port"]);

            _fromName = _configuration["Mailing:FromName"];
            _fromMail = _configuration["Mailing:FromMail"];
        }
        public void Send(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromMail));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_server, _port, false);
                client.Authenticate(_username, _password);
                client.Send(message);
                client.Disconnect(true);
            }
            //    var client = new ();

            //client.Host = _server;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential(_username, _password);
            //client.Port = _port;

            //var mailMessage = new MailMessage(from, to, subject, body);
            //client.Send(mailMessage);
        }
    }
}
