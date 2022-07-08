using System.ComponentModel.DataAnnotations.Schema;

namespace ScavHunt.Data.Models
{
    public class Question
    {
        //
        // If SHORT_CODE_LENGTH changed, regex in Interop.js must also be changed!
        // function window.SetupScanner
        //

        public const int SHORT_CODE_LENGTH = 5;

        public long Id { get; set; }

        public string ShortCode { get; set; }
        public string? Vendor { get; set; }
        public string? Text { get; set; }
        public string? Title { get; set; }
        public string? HintText { get; set; }
        public string? SuccessText { get; set; }

        public List<string> Answers { get; set; } = new List<string>();

        public bool ShuffleAnswers { get; set; }

        public DateTime UnlockTime { get; set; }
        public DateTime LockTime { get; set; }

        public Question? ParentQuestion { get; set; }

        public List<LogRecord> Responses { get; set; }
        public List<PointTransaction> PointTransactions { get; set; }

        // QuestionAdminService -> Update() must be updated for additional fields
        // Admin/Questions.razor -> EditQuestion() must be updated for additional fields

        public bool IsCurrentlyLockedEarly()
        {
            if(UnlockTime != DateTime.MinValue && UnlockTime > DateTime.Now)
            {
                return true;
            }

            return false;
        }

        public bool IsCurrentlyLockedLate()
        {
            if (LockTime != DateTime.MinValue && LockTime < DateTime.Now)
            {
                return true;
            }

            return false;
        }

        public bool IsCurrentlyLockedTime()
        {
            return IsCurrentlyLockedEarly() || IsCurrentlyLockedLate();
        }

        public bool IsCompleted(Player player)
        {
            return player.PointTransactions.Any(t => (t?.Question?.Id ?? -1) == Id);
        }

        public bool HasAccess(Player player)
        {
            // Skip if no prerequisites
            if(ParentQuestion == default)
            {
                return true;
            }

            // Player has completed one of the parent tranasactions
            if(player.PointTransactions.Any(t => (t.Question?.Id ?? -1) == ParentQuestion.Id))
            {
                return true;
            }

            return false;
        }
    }
}
