using Microsoft.EntityFrameworkCore;
using System;
using Work.Data.Interface;
using Work.Models;

namespace Work.Context
{
    public class ApplictaionDbContext : DbContext, IDbContext
    {
        public ApplictaionDbContext(DbContextOptions<ApplictaionDbContext> options)
       : base(options)
        {
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Skill_Person> Skill_Person { get; set; }
        public async Task AddAsync<T>(T entity) where T : class
        {
            await Set<T>().AddAsync(entity);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Skill_Person
            modelBuilder.Entity<Skill_Person>().HasKey(dg => new
            {
                dg.PersonId,
                dg.SkillId
            });
            modelBuilder.Entity<Skill_Person>().HasOne(g => g.Person).WithMany(dg => dg.Skill_Person).HasForeignKey(g => g.PersonId);
            modelBuilder.Entity<Skill_Person>().HasOne(g => g.Skill).WithMany(dg => dg.Skill_Person).HasForeignKey(g => g.SkillId);


            base.OnModelCreating(modelBuilder);
        }

    }
}
