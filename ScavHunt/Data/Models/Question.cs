using Newtonsoft.Json;
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

        [JsonIgnore]
        public Question? ParentQuestion { get; set; }
        [JsonIgnore]
        public List<LogRecord> Responses { get; set; }
        [JsonIgnore]
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

        public override bool Equals(object? obj)
        {
            if (obj is Question question)
            {
                var lc = Answers.SequenceEqual(question.Answers);
                var ret = Id == question.Id &&
                       ShortCode == question.ShortCode &&
                       Vendor == question.Vendor &&
                       Text == question.Text &&
                       Title == question.Title &&
                       HintText == question.HintText &&
                       SuccessText == question.SuccessText &&
                       lc &&
                       ShuffleAnswers == question.ShuffleAnswers &&
                       UnlockTime == question.UnlockTime &&
                       LockTime == question.LockTime;
                return ret;
            }
            return false;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(ShortCode);
            hash.Add(Vendor);
            hash.Add(Text);
            hash.Add(Title);
            hash.Add(HintText);
            hash.Add(SuccessText);
            hash.Add(Answers);
            hash.Add(ShuffleAnswers);
            hash.Add(UnlockTime);
            hash.Add(LockTime);
            return hash.ToHashCode();
        }
    }
}
