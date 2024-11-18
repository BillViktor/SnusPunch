using Microsoft.EntityFrameworkCore;
using SnusPunch.Data.DbContexts;
using SnusPunch.Data.Helpers;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using System.Linq;

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

        public async Task<SnusModel> GetSnusById(int aSnusModelId)
        {
            return await mSnusPunchDbContext.Snus.AsNoTracking().FirstOrDefaultAsync(x => x.Id == aSnusModelId);
        }

        public async Task<PaginationResponse<SnusModel>> GetSnusPaginated(PaginationParameters aPaginationParameters)
        {
            var sSnus = await mSnusPunchDbContext.Snus
                .SearchByProperty(aPaginationParameters.SearchPropertyName, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().ToListAsync();

            var sCount = await mSnusPunchDbContext.Snus
                .SearchByProperty(aPaginationParameters.SearchPropertyName, aPaginationParameters.SearchString)
                .CountAsync();

            return new PaginationResponse<SnusModel>(sSnus, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }

        public async Task<SnusModel> UpdateSnus(SnusModel aSnusModel)
        {
            SnusModel sSnusModel = await mSnusPunchDbContext.Snus.FirstOrDefaultAsync(s => s.Id == aSnusModel.Id);
            mSnusPunchDbContext.Entry(sSnusModel).CurrentValues.SetValues(aSnusModel);
            await mSnusPunchDbContext.SaveChangesAsync();
            return await mSnusPunchDbContext.Snus.AsNoTracking().FirstOrDefaultAsync(s => s.Id == aSnusModel.Id);
        }

        public async Task RemoveSnus(int aSnusModelId)
        {
            SnusModel sSnusModel = await mSnusPunchDbContext.Snus.AsNoTracking().FirstOrDefaultAsync(s => s.Id == aSnusModelId);
            mSnusPunchDbContext.Remove(sSnusModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task SetFavouriteSnus(string aUserId, int aSnusModelId)
        {
            SnusPunchUserModel sSnusPunchUserModel = await mSnusPunchDbContext.Users.FirstOrDefaultAsync(x => x.Id == aUserId);

            sSnusPunchUserModel.FavoriteSnusId = aSnusModelId;

            await mSnusPunchDbContext.SaveChangesAsync();
        }
        #endregion

        #region Users
        public async Task<PaginationResponse<SnusPunchUserDto>> GetUsersPaginated(PaginationParameters aPaginationParameters)
        {
            var sUsers = await mSnusPunchDbContext.Users
                .Include(s => s.FavoriteSnus)
                .SearchByProperty(aPaginationParameters.SearchPropertyName, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new SnusPunchUserDto
                {
                    UserName = x.UserName,
                    FavouriteSnus = x.FavoriteSnus.Name
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.Users
                .SearchByProperty(aPaginationParameters.SearchPropertyName, aPaginationParameters.SearchString)
                .CountAsync();

            return new PaginationResponse<SnusPunchUserDto>(sUsers, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }
        #endregion
    }
}
