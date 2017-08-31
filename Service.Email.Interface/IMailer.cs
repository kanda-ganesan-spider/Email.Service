using Service.Email.ViewModel;

namespace Service.Email.Interface
{
    public interface IMailer
    {
        MailerResponse SendMail(MailerRequest mailerRequest);
    }
}
