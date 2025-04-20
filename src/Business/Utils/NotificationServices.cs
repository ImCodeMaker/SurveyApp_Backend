using MailKit.Net.Smtp;
using MimeKit;

    public class NotificacionServicio : INotificationsServices
    {
        private readonly UsersCRUDService _usersCRUDService;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;

        public NotificacionServicio(UsersCRUDService usersCRUDService)
        {
            _smtpHost = "smtp.gmail.com";
            _smtpPort = 587;
            _smtpUsername = "finalprojectsurveys@gmail.com";
            _smtpPassword = "sltlkuxofawtztvb";
            _fromEmail = "finalprojectsurveys@gmail.com";
            _usersCRUDService = usersCRUDService;
        }


        public async Task EnviarNotificacionAsync(string emailDestinatario, string asunto, string mensaje)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Survey Finale Project", _fromEmail));
                email.To.Add(new MailboxAddress("", emailDestinatario));
                email.Subject = asunto;

                var bodyBuilder = new BodyBuilder { TextBody = mensaje };
                email.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    // Desactivar la verificación de certificados (SOLUCIÓN TEMPORAL)
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                    await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar la notificación por correo: {ex.Message}", ex);
            }
        }

        public async Task SendAdminEmail( string mensaje)
        {
            try
            {
                var allusers = _usersCRUDService.GetUsers();
                string AdminEmail = ""; 
                foreach (var user in allusers)
                {
                    if(user.IsAdmin == true)
                    {
                        AdminEmail = user.Email;
                    }
                    
                }
                Console.WriteLine(AdminEmail);
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Surveys Automatic Responses", _fromEmail));
                email.To.Add(new MailboxAddress("", AdminEmail));
                email.Subject = "A user submitted it's response.";

                var bodyBuilder = new BodyBuilder { TextBody = mensaje };
                email.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                    await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar la notificación por correo: {ex.Message}", ex);
            }
        }
    }
