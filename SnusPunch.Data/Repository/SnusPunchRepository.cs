using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SnusPunch.Data.DbContexts;
using SnusPunch.Data.Helpers;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using System.Linq;

namespace SnusPunch.Data.Repository
{
    public class SnusPunchRepository
    {
        private readonly IConfiguration mConfiguration;
        private readonly SnusPunchDbContext mSnusPunchDbContext;

        public SnusPunchRepository(IConfiguration aConfiguration, SnusPunchDbContext aSnusPunchDbContext)
        {
            mConfiguration = aConfiguration;
            mSnusPunchDbContext = aSnusPunchDbContext;
        }


        #region Snus
        public async Task<SnusModel> AddSnus(SnusModel aSnusModel)
        {
            await mSnusPunchDbContext.AddAsync(aSnusModel);
            await mSnusPunchDbContext.SaveChangesAsync();
            return aSnusModel;
        }

        public async Task<List<SnusDto>> GetSnusDto()
        {
            return await mSnusPunchDbContext.Snus
                .AsNoTracking()
                .Select(x => new SnusDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        }

        public async Task<SnusModel> GetSnusById(int aSnusModelId)
        {
            return await mSnusPunchDbContext.Snus.AsNoTracking().FirstOrDefaultAsync(x => x.Id == aSnusModelId);
        }

        public async Task<PaginationResponse<SnusModel>> GetSnusPaginated(PaginationParameters aPaginationParameters)
        {
            var sSnus = await mSnusPunchDbContext.Snus
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().ToListAsync();

            var sCount = await mSnusPunchDbContext.Snus
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
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


        #region Entries
        public async Task<PaginationResponse<EntryDto>> GetEntriesPaginated(PaginationParameters aPaginationParameters)
        {
            var sSnus = await mSnusPunchDbContext.Entries
                .Include(x => x.Snus)
                .Include(x => x.SnusPunchUserModel)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new EntryDto
                {
                    CreateDate = x.CreateDate,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl,
                    SnusName = x.Snus.Name,
                    UserName = x.SnusPunchUserModel.UserName,
                    UserProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}"
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.Entries
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .CountAsync();

            return new PaginationResponse<EntryDto>(sSnus, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }

        private async Task<EntryDto> GetEntryDtoById(int aEntryModelId)
        {
            var sEntry = await mSnusPunchDbContext.Entries
                .Include(x => x.Snus)
                .Include(x => x.SnusPunchUserModel)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == aEntryModelId);

            return new EntryDto
            {
                CreateDate = sEntry.CreateDate,
                Description = sEntry.Description,
                SnusName = sEntry.Snus.Name,
                UserName = sEntry.SnusPunchUserModel.UserName,
            };
        }

        public async Task<EntryDto> AddEntry(EntryModel aEntryModel)
        {
            await mSnusPunchDbContext.AddAsync(aEntryModel);
            await mSnusPunchDbContext.SaveChangesAsync();

            return await GetEntryDtoById(aEntryModel.Id);
        }
        #endregion


        #region Users
        public async Task<PaginationResponse<SnusPunchUserDto>> GetUsersPaginated(PaginationParameters aPaginationParameters)
        {
            var sUsers = await mSnusPunchDbContext.Users
                .Include(s => s.FavoriteSnus)
                .Include(x => x.Entries)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new SnusPunchUserDto
                {
                    UserName = x.UserName,
                    FavouriteSnus = x.FavoriteSnus.Name,
                    SnusPunches = x.Entries.Count,
                    ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.ProfilePicturePath ?? "default.jpg"}"
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.Users
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .CountAsync();

            return new PaginationResponse<SnusPunchUserDto>(sUsers, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }
        #endregion
    }
}
