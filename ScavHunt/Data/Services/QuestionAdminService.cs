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
            return await db.Questions.Include(q => q.ParentQuestion).ToListAsync();
        }

        public async Task<Question?> Add(Question question)
        {
            using var db = dbFactory.CreateDbContext();

            if(question.ShortCode == default)
            {
                var shortCode = await NewShortCode();
                if(shortCode == default)
                {
                    // This should be nearly mathematically impossible for this application...
                    Console.WriteLine("ERROR: No short code was able to be found!!!");
                    return default;
                }

                question.ShortCode = shortCode;
            }

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
                var existing = await db.Questions.Include(q => q.ParentQuestion).FirstOrDefaultAsync(q => q.Id == question.Id);

                if(existing == default)
                {
                    return default;
                }

                var parent = question.ParentQuestion == default ? default : await db.Questions.FirstOrDefaultAsync(q => q.Id == question.ParentQuestion.Id);

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
                existing.ParentQuestion = parent;

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

        public async Task Delete(Question question)
        {
            using var db = await dbFactory.CreateDbContextAsync();

            var existing = await db.Questions.Include(q => q.ParentQuestion).FirstOrDefaultAsync(q => q.Id == question.Id);

            if (existing == default)
            {
                return;
            }

            var children = await db.Questions.Where(q => q.ParentQuestion == existing).ToListAsync();
            if(existing.ParentQuestion != null)
            {
                children.ForEach(q => q.ParentQuestion = existing.ParentQuestion);
            }
            else
            {
                children.ForEach(q => q.ParentQuestion = null);
            }

            db.UpdateRange(children);

            var logs = await db.Log.Where(l => l.Question == existing).ToListAsync();
            logs.ForEach(l => l.Question = null);
            db.UpdateRange(logs);

            var points = await db.PointTransactions.Where(p => p.Question == existing).ToListAsync();
            foreach(var p in points)
            {
                p.Source = PointTransaction.PointSource.Manual;
                p.Question = null;
            }
            db.UpdateRange(points);

            db.Remove(existing);
            await db.SaveChangesAsync();
        }
    }
}
