using System;
using Service.Email.Interface;
using Service.Email.ViewModel;

namespace Service.Email
{
    public class MailScheduler : IMailScheduler
    {
        public MailerResponse SendScheduledMail(MailerRequest mailerRequest, ScheduleJob scheduleJob = null)
        {
            throw new NotImplementedException();
        }
    }
}
