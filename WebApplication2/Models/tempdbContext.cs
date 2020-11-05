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
        public tempdbContext(DbContextOptions<tempdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Login> Login { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //    optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=tempdb;Trusted_Connection=True;");
            //}
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

                entity.Property(e => e.Id);
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.Property(e => e.id)
                    .HasDefaultValueSql("NEWID()");

                entity.Property(e => e.name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.pwd)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.role)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.lastlogin);
            });
            //modelBuilder.Entity<Login>().HasData(
            //    new { id = new Guid., name = "admin", pwd = "admin",lastlogin = Convert.ToDateTime("2020-01-01") }
            //);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        /// <summary>
        /// search user by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string name)
        {
            return await this.User.Where(t => t.Name.Equals(name)).FirstOrDefaultAsync();
        }
        /// <summary>
        /// get user with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(int id)
        {
            return await this.User.Where(t => t.Id.Equals(id)).FirstOrDefaultAsync();
        }
        /// <summary>
        /// get all user
        /// </summary>
        /// <returns></returns>
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
            this.SaveChanges();
        }

        public Login GetLogin(Login login)
        {
            var log = this.Login.Where(t=>t.name.Equals(login.name)).FirstOrDefault();
            log.lastlogin = DateTime.Now;
            //update login time
            this.Login.Update(log);
            this.SaveChanges();
            return log;
        }

        public void AddLogin(Login login) {
            this.Login.Add(login);
            this.SaveChanges();
        }
    }
}
