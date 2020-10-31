using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2.Models
{
    public partial class tempdbContext : DbContext
    {
        public tempdbContext()
        {
        }

        public tempdbContext(DbContextOptions<tempdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=tempdb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public async Task<User> GetUserAsync(string name)
        {
            return await this.User.Where(t => t.Name.Equals(name)).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserAsync(int id)
        {
            return await this.User.Where(t => t.Id.Equals(id)).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await this.User.ToListAsync();
        }

        public async Task<EntityState> AddUser(User newUser) {
            EntityState res;
            try {
                res = this.User.Add(newUser).State;
                this.SaveChanges();
            }
            catch (Exception ex)
            {
                res = EntityState.Unchanged;
            }
            return res;
        }

        public void DeleteUser(int id) {
            var _ur = this.User.Where(t=>t.Id.Equals(id)).FirstOrDefault();
            //remove if not null
            this.User.Remove(_ur);
        }
    }
}
