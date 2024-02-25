using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using System;
using Work.Models;

namespace Work.Context
{
    public class ApplicationDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplictaionDbContext>();

                dbContext.Database.EnsureCreated();

                // Platforms
                if (!dbContext.Skills.Any())
                {
                    dbContext.Skills.AddRange(new List<Skill>()
                    {
                        new Skill()
                        {
                            Name = "Навык1",
                            Level = 1
                        },
                        new Skill()
                        {
                            Name = "Навык2",
                            Level = 2
                            
                        },
                        new Skill()
                        {
                            Name = "Навык3",
                            Level = 3
                        },
                        new Skill()
                        {
                            Name = "Навык4",
                            Level = 4
                        },
                        new Skill()
                        {
                            Name = "Навык5",
                            Level = 5
                        },
                        new Skill()
                        {
                            Name = "Навык6",
                            Level = 6
                        },
                        new Skill()
                        {
                            Name = "Навык7",
                            Level = 7
                        },
                        new Skill()
                        {
                            Name = "Навы8",
                            Level = 8
                        },
                        new Skill()
                        {
                            Name = "Навык9",
                            Level = 9
                        },
                        new Skill()
                        {
                            Name = "Навык10",
                            Level = 10
                        },
                    });
                    dbContext.SaveChanges();
                }
                
            }
        }
    }
}
