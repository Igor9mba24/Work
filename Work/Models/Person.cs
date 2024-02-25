using System.ComponentModel.DataAnnotations;
using Work.Data.Base;

namespace Work.Models
{
    public class Person : IEntityBase
        {
            [Key]
            public long Id { get; set; }

            //[Required]
            public string Name { get; set; }

            //[Required]
            public string DisplayName { get; set; }
        public List<Skill_Person> Skill_Person { get; set; }
    }
    }

