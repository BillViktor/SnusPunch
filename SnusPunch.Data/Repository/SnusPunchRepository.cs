using Microsoft.EntityFrameworkCore;
using SnusPunch.Data.DbContexts;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Data.Repository
{
    public class SnusPunchRepository
    {
        private readonly SnusPunchDbContext mSnusPunchDbContext;

        public SnusPunchRepository(SnusPunchDbContext aSnusPunchDbContext)
        {
            mSnusPunchDbContext = aSnusPunchDbContext;
        }

        #region Snus
        public async Task<SnusModel> AddSnus(SnusModel aSnusModel)
        {
            await mSnusPunchDbContext.AddAsync(aSnusModel);
            await mSnusPunchDbContext.SaveChangesAsync();
            return aSnusModel;
        }

        public async Task<List<SnusModel>> GetSnus()
        {
            return await mSnusPunchDbContext.Snus.AsNoTracking().ToListAsync();
        }

        public async Task<SnusModel> UpdateSnus(SnusModel aSnusModel)
        {
            SnusModel sSnusModel = await mSnusPunchDbContext.Snus.AsNoTracking().FirstOrDefaultAsync(s => s.Id == aSnusModel.Id);
            mSnusPunchDbContext.Entry(sSnusModel).CurrentValues.SetValues(aSnusModel);
            await mSnusPunchDbContext.SaveChangesAsync();
            return sSnusModel;
        }

        public async Task RemoveSnus(int aSnusModelId)
        {
            SnusModel sSnusModel = await mSnusPunchDbContext.Snus.AsNoTracking().FirstOrDefaultAsync(s => s.Id == aSnusModelId);
            mSnusPunchDbContext.Remove(sSnusModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }
        #endregion
    }
}
