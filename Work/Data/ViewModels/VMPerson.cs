using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Work.Models;

namespace Work.Data.ViewModels
{
    public class VMPerson
    {
        [JsonIgnore]
        public long Id { get; set; }
        [Display(Name = "ФИО")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Отображающее ФИО")]
        [Required]
        public string DisplayName { get; set; }
        [Display(Name = "Навык")]
        [Required]
        public List<VMSkill> SkillId { get; set; }
    }
}
