using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ExamManagementContext : DbContext
    {
        public ExamManagementContext(DbContextOptions<ExamManagementContext> options) : base(options)
        {

        }

        public ExamManagementContext()
        {
            this.ChangeTracker.AutoDetectChangesEnabled = false;
            this.ConfigureAwait(false);
        }

        #region Models
        public DbSet<ApiUser> ApiUsers { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Exams> Exams { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<Articles> Articles { get; set; }
        public DbSet<Questions> Questions { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApiUserConfiguration());
        }
    }
}
