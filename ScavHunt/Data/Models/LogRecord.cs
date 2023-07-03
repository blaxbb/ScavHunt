using ScavHunt.Data.Services;

namespace ScavHunt.Data.Models
{
    public class LogRecord
    {
        public long Id { get; set; }

        public enum RecordType
        {
            StartedQuestion = 0,
            CompletedQuestion = 1,
            Player = 2,
            IncorrectAnswer = 3,
            PointTransaction = 4,
            Prize = 5,
            Account = 6,
            NotFound = 7,
        }
        public RecordType Type { get; set; }

        public Question? Question { get; set; }
        public int? ResponseIndex { get; set; }
        public ScavhuntUser? User { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        public LogRecord()
        {

        }

        public static LogRecord QuestionLog(Question question, ScavhuntUser user, RecordType type, string message, int? responseIndex = null) => new LogRecord()
        {
            Type = type,
            Question = question,
            User = user,
            Message = message,
            Timestamp = DateTime.Now,
            ResponseIndex = responseIndex
        };

        public static LogRecord MessageLog(RecordType type, string message) => new LogRecord()
        {
            Type = type,
            Timestamp = DateTime.Now,
        };

        public static LogRecord PrizeLog(PrizeTransaction transaction) => new LogRecord()
        {
            Type = RecordType.Prize,
            User = transaction.User ?? transaction.CreatedBy,
            Message = $"{transaction.Prize.Type} prize {transaction.Prize.Name} was granted by {transaction.CreatedBy.UserName}{(transaction.User == null ? $" to badge {transaction.Badge}" : "" )}.",
            Timestamp = DateTime.Now,
        };

        public static LogRecord NotFoundLog(string path, ScavhuntUser user) => new LogRecord()
        {
            Type = RecordType.NotFound,
            Message = path,
            User = user,
            Timestamp = DateTime.Now
        };
            
    }
}
