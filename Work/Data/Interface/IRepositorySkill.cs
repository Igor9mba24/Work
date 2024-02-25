using Work.Models;

namespace Work.Data.Interface
{
    public interface IRepositorySkill
    {
        Task<IEnumerable<Skill>> GetAllSkillsAsync();
        Task<Skill> GetSkillByIdAsync(int id);
        Task AddSkillAsync(Skill skill);
        Task UpdateSkillAsync(Skill skill);
        Task DeleteSkillAsync(int id);
    }
}
