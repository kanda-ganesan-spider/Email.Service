using System;
using Service.Email.Interface;
using Service.Email.ViewModel;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Linq;

namespace Service.Email
{
    public class Mailer : IMailer
    {
        MailerResponse IMailer.SendMail(MailerRequest mailerRequest) {
            return SendEmail(
                mailerRequest.ToList,
                mailerRequest.From,
                mailerRequest.Subject,
                mailerRequest.Message,
                mailerRequest.Attachments,
                mailerRequest.CcList,
                mailerRequest.BccList,
                mailerRequest.EnableSSL,
                mailerRequest.FromPassword);
        }

        private static MailerResponse SendEmail(
            IList<string> toList,
            string from,
            string subject,
            string message,
            IList<MailerAttachment> attachments = null,
            IList<string> ccList = null,
            IList<string> bccList = null,
            bool enableSSL = false,
            string fromPassword = null)
        {
            var mailerResponse = new MailerResponse() { IsSuccess = true, ErrorCode = Constants.Success };
            var mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from),
                Subject = subject,
                Body = message
            };

            try
            {
                SetToAddress(toList, mailMessage);

                SetCcAddress(ccList, mailMessage);

                SetAttachments(attachments, mailMessage);

                NetworkCredential networkCredential;
                if (string.IsNullOrEmpty(fromPassword))
                {
                    networkCredential = CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    networkCredential = new NetworkCredential(from, fromPassword);
                }

                var smtpClient = GetSmtpClient(enableSSL, networkCredential);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex) {
                mailerResponse.IsSuccess = false;
                mailerResponse.ErrorCode = Constants.Error;
                mailerResponse.ErrorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return mailerResponse;
            }

            return mailerResponse;
        }

        private static void SetAttachments(IEnumerable<MailerAttachment> attachments, MailMessage mailMessage)
        {
            if (attachments == null) return;
            foreach (var attachment in attachments)
            {
                var memoryStream = new MemoryStream(attachment.Attachment);
                mailMessage.Attachments.Add(new Attachment(memoryStream, attachment.Name));
            }
        }

        private static void SetCcAddress(IEnumerable<string> ccList, MailMessage mailMessage)
        {
            if (ccList == null) return;
            foreach (var mailCc in ccList.Where(cc => !string.IsNullOrEmpty(cc.Trim())))
                mailMessage.CC.Add(mailCc);
        }

        private static void SetToAddress(IEnumerable<string> toList, MailMessage mailMessage)
        {
            foreach (var mailTo in toList.Where(to => !string.IsNullOrEmpty(to.Trim())))
                mailMessage.To.Add(mailTo);
        }

        private static SmtpClient GetSmtpClient(bool enableSSL, NetworkCredential networkCredential)
        {
            return new SmtpClient
            {
                Host = ConfigurationManager.AppSettings[Constants.SmtpNetworkHost],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SmtpNetworkPort]),
                Credentials = networkCredential,
                EnableSsl = enableSSL
            };
        }
    }
}
