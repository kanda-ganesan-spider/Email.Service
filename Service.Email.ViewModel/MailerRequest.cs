using System.Collections.Generic;

namespace Service.Email.ViewModel
{
    public class MailerRequest
    {
        public string From { get; set; }
        public IList<string> ToList { get; set; }
        public IList<string> CcList { get; set; }
        public IList<string> BccList { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public IList<MailerAttachment> Attachments { get; set; }
        public bool EnableSSL { get; set; }
        public string FromPassword { get; set; }
    }
}
