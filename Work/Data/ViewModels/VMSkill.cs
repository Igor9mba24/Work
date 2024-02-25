using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Work.Data.ViewModels
{
    public class VMSkill
    {
        public byte Level { get; set; }
        
        public string Name { get; set; }
    }
}
