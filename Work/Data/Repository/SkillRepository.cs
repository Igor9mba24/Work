using Microsoft.EntityFrameworkCore;
using Work.Context;
using Work.Data.Interface;
using Work.Models;

namespace Work.Data.Repository
{
    public class SkillRepository : IRepositorySkill
    {
        private readonly ApplictaionDbContext _context;

        public SkillRepository(ApplictaionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            return await _context.Skills.ToListAsync();
        }

        public async Task<Skill> GetSkillByIdAsync(int id)
        {
            return await _context.Skills.FindAsync(id);
        }

        public async Task AddSkillAsync(Skill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateSkillAsync(Skill skill)
        {
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSkillAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
        }
    }
}
