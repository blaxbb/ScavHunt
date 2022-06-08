namespace ScavHunt.Data.Models
{
    public class Question
    {
        public const int SHORT_CODE_LENGTH = 5;

        public long Id { get; set; }

        public string ShortCode { get; set; }
        public string Vendor { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string HintText { get; set; }
        public List<string> Answers { get; set; } = new List<string>();

        public List<LogRecord> Responses { get; set; }
        public List<PointTransaction> PointTransactions { get; set; }

        // QuestionAdminService -> Update() must be updated for additional fields
        // Admin/Questions.razor -> EditQuestion() must be updated for additional fields
    }
}
