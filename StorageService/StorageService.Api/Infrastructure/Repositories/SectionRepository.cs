using Microsoft.EntityFrameworkCore;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Data;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Api.Infrastructure.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly ApplicationDbContext _db;

        public SectionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Section> AddAsync(string code, string? description)
        {
            var section = new Section
            {
                Id = Guid.NewGuid(),
                Code = code,
                Description = description
            };

            _db.Sections.Add(section);
            await _db.SaveChangesAsync();

            return section;
        }

        public async Task<List<Section>> GetAllAsync()
        {
            return await _db.Sections.ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existSection = await _db.Sections.FindAsync(id);
            if (existSection == null) { throw new InvalidOperationException("No section for delete"); }

            _db.Sections.Remove(existSection);
            await _db.SaveChangesAsync();
        }

        public async Task<Section?> GetAsync(Guid id)
        {
            return await _db.Sections.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Section?> GetByCodeAsync(string code)
        {
            return await _db.Sections.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());
        }

        public async Task UpdateAsync(Section section)
        {
            var existSection = await GetAsync(section.Id);
            if(existSection == null) { throw new InvalidOperationException("No section for update"); }
            await UpdateAsync(section);
        }
    }
}
