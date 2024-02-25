using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Work.Data.Base;

namespace Work.Models
{
    public class Skill 
    {
        [Key]
        [Required]
        [Display(Name = "Уровень")]
        [Range(1, 10, ErrorMessage = "Уровень навыка должен быть от 1 до 10!")]
        public byte Level { get; set; }

        [Required]
        [Display(Name = "ФИО")]
        public string Name { get; set; }
        public List<Skill_Person> Skill_Person {  get; set; }
    }

}
