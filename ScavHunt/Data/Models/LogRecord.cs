namespace ScavHunt.Data.Models
{
    public class LogRecord
    {
        public long Id { get; set; }

        public enum RecordType
        {
            Started = 0,
            Completed = 1,
            Player = 2,
            IncorrectAnswer = 3,
            PointTransaction = 4,
        }
        public RecordType Type { get; set; }

        public Question? Question { get; set; }
        public Player? Player { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        public LogRecord()
        {

        }

        public static LogRecord QuestionLog(Question question, Player player, RecordType type, string message) => new LogRecord()
        {
            Type = type,
            Question = question,
            Player = player,
            Message = message,
            Timestamp = DateTime.Now,
        };

        public static LogRecord MessageLog(RecordType type, string message) => new LogRecord()
        {
            Type = type,
            Timestamp = DateTime.Now,
        };
    }
}
