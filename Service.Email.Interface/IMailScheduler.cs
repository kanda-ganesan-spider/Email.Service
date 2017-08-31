using Service.Email.ViewModel;

namespace Service.Email.Interface
{
    public interface IMailScheduler
    {
        MailerResponse SendScheduledMail(MailerRequest mailerRequest, ScheduleJob scheduleJob = null);
    }
}
