
namespace Service.Email.ViewModel
{
    public class ScheduleJob
    {
        public string JobName { get; set; }
        public bool IsRepeatable { get; set; }
        public int JobInterval { get; set; }

    }
}
