namespace ScavHunt.Data.Models
{
    public class Alert
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public AlertType Type { get; set; }
        public enum AlertType
        {
            Info,
            Warning,
            Danger
        }

        /*
         *  New fields must be reflected in Pages/Admin/Alerts.razor
         */

        public bool IsActive()
        {
            if(StartTime == DateTime.MinValue)
            {
                return true;
            }

            var now = DateTime.Now;

            if(StartTime < now)
            {
                if(EndTime > now || EndTime == DateTime.MinValue)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
