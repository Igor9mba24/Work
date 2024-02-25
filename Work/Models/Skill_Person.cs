namespace Work.Models
{
    public class Skill_Person
    {
        public long PersonId { get; set; }
        public Person Person { get; set; }

        public byte SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}
