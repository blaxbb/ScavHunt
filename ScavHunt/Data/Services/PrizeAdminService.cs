﻿using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class PrizeAdminService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;
        public PrizeAdminService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<List<Prize>> All()
        {
            var db = await dbFactory.CreateDbContextAsync();
            return await db.Prizes.ToListAsync();
        }

        public async Task<List<PrizeTransaction>> AllTransactions(string badge)
        {
            var db = await dbFactory.CreateDbContextAsync();
            return await db.PrizeTransactions.Include(p => p.Prize).Where(p => p.Badge == badge).ToListAsync();
        }

        public async Task<List<PrizeTransaction>> AllTransactions(ScavhuntUser user)
        {
            var db = await dbFactory.CreateDbContextAsync();
            return await db.PrizeTransactions.Include(p => p.Prize).Where(p => p.User == user).ToListAsync();
        }

        public async Task<Prize> New(Prize prize)
        {
            var db = await dbFactory.CreateDbContextAsync();
            var updated = db.Prizes.Add(prize);
            await db.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Prize> Update(Prize prize)
        {
            var db = await dbFactory.CreateDbContextAsync();
            var updated = db.Prizes.Update(prize);
            await db.SaveChangesAsync();
            return updated.Entity;
        }

        public async Task<PrizeTransaction> CreateTransaction(PrizeTransaction transaction)
        {
            var db = await dbFactory.CreateDbContextAsync();


            if(transaction.User != null)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == transaction.User.Id);
                transaction.User = user;
            }

            var prize = await db.Prizes.FirstOrDefaultAsync(p => p.Id == transaction.Prize.Id);
            if (prize == null)
            {
                throw new Exception("Prize not found!");
            }

            prize.Quantity -= 1;
            transaction.Prize = prize;
            
            var updated = db.PrizeTransactions.Add(transaction);

            db.Prizes.Update(prize);

            await db.SaveChangesAsync();
            return updated.Entity;
        }
    }
}