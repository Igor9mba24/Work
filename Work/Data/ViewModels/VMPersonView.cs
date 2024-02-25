using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Work.Data.ViewModels
{
    public class VMPersonView
    {
        
        public long Id { get; set; }
        [Display(Name = "ФИО")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Отображающее ФИО")]
        [Required]
        public string DisplayName { get; set; }
        [Display(Name = "Навык")]
        [Required]
        public List<VMSkilView> SkillId { get; set; }
    }
}
