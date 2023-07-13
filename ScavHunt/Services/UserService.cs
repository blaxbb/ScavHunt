using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScavHunt.Data;
using ScavHunt.Data.Models;

namespace ScavHunt.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public int Count()
        {
            return db.Users.Count();
        }

        public DbSet<ScavhuntUser> Users => db.Users;

        public IEnumerable<string> GetRolesById(string id)
        {
            var user = Users.Where(u => u.Id == id).FirstOrDefault();
            if(user != null)
            {
                return GetRoles(user);
            }

            return new List<string>();
        }

        public IEnumerable<string> GetRoles(ScavhuntUser user)
        {
            var roles = db.UserRoles.Where(r => r.UserId == user.Id).Select(r => r.RoleId).ToList();
            return db.Roles.ToList().Where(r => roles.Contains(r.Id)).Select(r => r.Name);
        }

        public void AddToRole(ScavhuntUser user, string name)
        {
            var role = db.Roles.Where(r => r.Name == name).FirstOrDefault();
            if(role != null)
            {
                db.UserRoles.Add(new IdentityUserRole<string>() { RoleId = role.Id, UserId = user.Id });
            }
        }

        public void RemoveFromRole(ScavhuntUser user, string name)
        {
            var role = db.Roles.Where(r => r.Name == name).FirstOrDefault();
            if (role != null)
            {
                var roles = db.UserRoles.Where(r => r.UserId == user.Id && r.RoleId == role.Id);
                db.UserRoles.RemoveRange(roles);
                db.SaveChanges();

            }
        }

        public void ClearTracking()
        {
            db.ChangeTracker.Clear();
        }

        public async Task Update(ScavhuntUser user)
        {
            db.Update(user);
            await db.SaveChangesAsync();
        }
    }
}
