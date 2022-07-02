using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class QuestionAdminService : QuestionService
    {
        public QuestionAdminService(IDbContextFactory<ApplicationDbContext> factory)
            : base(factory)
        {
            
        }

        public async Task<List<Question>> All()
        {
            using var db = dbFactory.CreateDbContext();
            return await db.Questions.Include(q => q.ParentQuestions).ToListAsync();
        }

        public async Task<Question?> Add(Question question)
        {
            using var db = dbFactory.CreateDbContext();

            var shortCode = await NewShortCode();
            if(shortCode == default)
            {
                // This should be nearly mathematically impossible for this application...
                Console.WriteLine("ERROR: No short code was able to be found!!!");
                return default;
            }

            question.ShortCode = shortCode;

            db.Attach(question);

            await db.Questions.AddAsync(question);
            await db.SaveChangesAsync();

            return question;
        }

        public async Task<Question?> Update(Question question)
        {
            using var db = dbFactory.CreateDbContext();

            try
            {
                var existing = await db.Questions.Include(q => q.ParentQuestions).FirstOrDefaultAsync(q => q.Id == question.Id);

                if(existing == default)
                {
                    return default;
                }
                var parents = question.ParentQuestions.Select(q => db.Questions.Find(q.Id)).Where(q => q != null);

                existing.Vendor = question.Vendor;
                existing.Text = question.Text;
                existing.ShortCode = question.ShortCode;
                existing.Answers = question.Answers;
                existing.Title = question.Title;
                existing.HintText = question.HintText;
                existing.SuccessText = question.SuccessText;
                existing.ShuffleAnswers = question.ShuffleAnswers;
                existing.UnlockTime = question.UnlockTime;
                existing.LockTime = question.LockTime;
                existing.ParentQuestions = parents.ToList() ?? new();

                db.Attach(existing);

                db.Questions.Update(existing);
                await db.SaveChangesAsync();
                return question;
            }
            catch(Exception e)
            {
                Console.WriteLine($"DB Error: {e.Message}");
                return default;
            }
        }

        public async Task<string?> NewShortCode()
        {
            for (int i = 0; i < 10; i++)
            {
                //
                // Generates SHORT_CODE_LENGTH digit codes ex: 5 => 53815
                //
                var check = Random.Shared.Next((int)Math.Pow(10, Question.SHORT_CODE_LENGTH - 1), (int)Math.Pow(10, Question.SHORT_CODE_LENGTH)).ToString();

                var existing = await GetFromShortCode(check);
                if (existing == default)
                {
                    return check;
                }

                Console.WriteLine($"We had a short code collision! {check}");
                //collision!....somehow - try again
            }

            return default;
        }
    }
}
