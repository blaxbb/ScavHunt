using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class QuestionAdminService : QuestionService
    {
        public QuestionAdminService(ApplicationDbContext db)
            : base(db)
        {
            
        }

        public async Task<List<Question>> All()
        {
            return await db.Questions.ToListAsync();
        }

        public async Task<Question?> Add(Question question)
        {
            var shortCode = await NewShortCode();
            if(shortCode == default)
            {
                // This should be nearly mathematically impossible for this application...
                Console.WriteLine("ERROR: No short code was able to be found!!!");
                return default;
            }

            question.ShortCode = shortCode;

            await db.Questions.AddAsync(question);
            await db.SaveChangesAsync();

            return question;
        }

        public async Task<Question?> Update(Question question)
        {
            try
            {
                var existing = db.Questions.Find(question.Id);

                if(existing == default)
                {
                    return default;
                }

                existing.Vendor = question.Vendor;
                existing.Text = question.Text;
                existing.ShortCode = question.ShortCode;
                existing.Answers = question.Answers;

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
