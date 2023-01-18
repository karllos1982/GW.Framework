using System.Net;
using System.Net.Mail;
using System.Text;
using GW.Common;
using GW.Core;

namespace GW.ApplicationHelpers
{

    public class MailSettings
    {
        public string SMTPServer { get; set; }

        public string SMTPUser { get; set; }

        public string SMTPPassword { get; set; }

        public string Port { get; set; }

        public bool IsBodyHtml { get; set; }

        public bool SSL { get; set; }

        public string EmailSender { get; set; }

        public string NameSender { get; set; }

        public Encoding ContentEncoding { get; set; }

    }
   

    public abstract class MailManager
    {

        private MailSettings _config;
        public MailSettings Config
        {
            get { return _config; }
            set { _config = value; }
        }


        public MailManager()
        {

        }

       
        private List<Destination> _destinations = new List<Destination>();

        public List<string> _attfiles { get; set; }

        public void AddDestination(string name, string email)
        {
            if (email.Trim().Length != 0)
            {
                Destination dest = new Destination(name, email);

                dest.Email = email;
                dest.Name = name;
                _destinations.Add(dest);
            }

        }

        public void AddAttachmentFile(string filename)
        {
            _attfiles.Add(filename);
        }

        public bool Send(string name, string email, string subject, string message)
        {
            bool ret = false;

            try
            {
                MailMessage oMail = new MailMessage();

                oMail.From = new MailAddress(_config.EmailSender, _config.NameSender);

                MailAddress item = new MailAddress(email, name);
                oMail.To.Add(item);

                if (_attfiles != null)
                {
                    foreach (string s in _attfiles)
                    {
                        oMail.Attachments.Add(new Attachment(s));
                    }
                }

                foreach (Destination dst in _destinations)
                {
                    MailAddress item2 = new MailAddress(dst.Email, dst.Name);
                    oMail.CC.Add(item2);
                }

                oMail.Subject = subject;
                oMail.IsBodyHtml = _config.IsBodyHtml;
                oMail.Body = message;
                oMail.BodyEncoding = _config.ContentEncoding;
                oMail.SubjectEncoding = _config.ContentEncoding;
                SmtpClient oSmtp = new SmtpClient(_config.SMTPServer, int.Parse(_config.Port));

                oSmtp.UseDefaultCredentials = false;

                oSmtp.Credentials = new NetworkCredential(_config.SMTPUser, _config.SMTPPassword);
                oSmtp.EnableSsl = _config.SSL;
                oSmtp.Send(oMail);

                oMail.Dispose();
                ret = true;
            }
            catch (Exception ex)
            {

            }

            return ret;
        }


    }

    public class Destination
    {
        private string _email;
        private string _name;

        public Destination(string name, string email)
        {
            _name = name;
            _email = email;
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
            }

        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
    }


}
