using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SnusPunch.Data.DbContexts;
using SnusPunch.Data.Helpers;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Shared.Models.Statistics;
using System.Xml.Linq;

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
        public async Task<PaginationResponse<EntryDto>> GetEntriesPaginated(PaginationParameters aPaginationParameters, bool aFetchEmptyPunches, EntryFilterEnum aEntryFilterEnum, string aSnusPunchUserModelId)
        {
            var sSnus = await mSnusPunchDbContext.Entries
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .FilterEntryHelper(aSnusPunchUserModelId, aEntryFilterEnum, aFetchEmptyPunches)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new EntryDto
                {
                    Id = x.Id,
                    CreateDate = x.CreateDate,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl != null ? mConfiguration["PostPicturePathFull"] + x.PhotoUrl : null,
                    SnusName = x.SnusName,
                    UserName = x.SnusPunchUserModel.UserName,
                    UserProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}",
                    Likes = x.Likes.Count,
                    CommentCount = x.Comments.Count,
                    LikedByUser = x.Likes.Any(x => x.SnusPunchUserModelId == aSnusPunchUserModelId)
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.Entries
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .FilterEntryHelper(aSnusPunchUserModelId, aEntryFilterEnum, aFetchEmptyPunches)
                .CountAsync();

            return new PaginationResponse<EntryDto>(sSnus, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }

        public async Task<PaginationResponse<EntryDto>> GetPhotoEntriesForUser(PaginationParameters aPaginationParameters, string aSnusPunchUserModelIdToFetch, string aSnusPunchUserModelId)
        {
            var sSnus = await mSnusPunchDbContext.Entries
                .Where(x => x.SnusPunchUserModelId == aSnusPunchUserModelIdToFetch && x.PhotoUrl != null)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new EntryDto
                {
                    Id = x.Id,
                    CreateDate = x.CreateDate,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl != null ? mConfiguration["PostPicturePathFull"] + x.PhotoUrl : null,
                    SnusName = x.SnusName,
                    UserName = x.SnusPunchUserModel.UserName,
                    UserProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}",
                    Likes = x.Likes.Count,
                    CommentCount = x.Comments.Count,
                    LikedByUser = x.Likes.Any(x => x.SnusPunchUserModelId == aSnusPunchUserModelId)
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.Entries
                .Where(x => x.SnusPunchUserModelId == aSnusPunchUserModelIdToFetch && x.PhotoUrl != null)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .CountAsync();

            return new PaginationResponse<EntryDto>(sSnus, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }

        public async Task<EntryModel> GetEntryById(int aEntryModelId)
        {
            return await mSnusPunchDbContext.Entries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == aEntryModelId);
        }

        public async Task<EntryDto> GetEntryDtoById(int aEntryModelId, string aUserId)
        {
            var sEntry = await mSnusPunchDbContext.Entries
                .Include(x => x.SnusPunchUserModel)
                .Include(x => x.Likes)
                .Include(x => x.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == aEntryModelId);

            return new EntryDto
            {
                Id = sEntry.Id,
                CreateDate = sEntry.CreateDate,
                Description = sEntry.Description,
                PhotoUrl = sEntry.PhotoUrl != null ? mConfiguration["PostPicturePathFull"] + sEntry.PhotoUrl : null,
                SnusName = sEntry.SnusName,
                UserName = sEntry.SnusPunchUserModel.UserName,
                UserProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{sEntry.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}",
                Likes = sEntry.Likes.Count,
                CommentCount = sEntry.Comments.Count,
                LikedByUser = sEntry.Likes.Any(x => x.SnusPunchUserModelId == aUserId)
            };
        }

        public async Task<EntryDto> AddEntry(EntryModel aEntryModel, string aUserId)
        {
            await mSnusPunchDbContext.AddAsync(aEntryModel);
            await mSnusPunchDbContext.SaveChangesAsync();

            return await GetEntryDtoById(aEntryModel.Id, aUserId);
        }

        public async Task RemoveEntry(EntryModel aEntryModel)
        {
            mSnusPunchDbContext.Remove(aEntryModel);

            await mSnusPunchDbContext.SaveChangesAsync();
        }
        #endregion


        #region Entry Likes
        public async Task LikeEntry(EntryLikeModel aEntryLikeModel)
        {
            await mSnusPunchDbContext.AddAsync(aEntryLikeModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task UnlikeEntry(EntryLikeModel aEntryLikeModel)
        {
            mSnusPunchDbContext.Remove(aEntryLikeModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task<EntryLikeModel> GetEntryLike(int aEntryModelId, string aSnusPunchUserModelId)
        {
            var sLike = await mSnusPunchDbContext.EntryLikes.FirstOrDefaultAsync(x => x.EntryId == aEntryModelId && x.SnusPunchUserModelId == aSnusPunchUserModelId);

            if(sLike == null)
            {
                throw new Exception("Liken hittades ej.");
            }

            return sLike;
        }

        public async Task<PaginationResponse<EntryLikeDto>> GetEntryLikesPaginated(PaginationParameters aPaginationParameters, int aEntryModelId)
        {
            var sLikes = await mSnusPunchDbContext.EntryLikes
                .Where(x => x.EntryId == aEntryModelId)
                .Include(x => x.SnusPunchUserModel)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty("CreateDate", SortOrderEnum.Ascending)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new EntryLikeDto
                {
                    UserName = x.SnusPunchUserModel.UserName ?? "Raderad användare",
                    ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}",
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.EntryLikes
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .Where(x => x.EntryId == aEntryModelId)
                .CountAsync();

            return new PaginationResponse<EntryLikeDto>(sLikes, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }
        #endregion


        #region Entry Comments
        public async Task<EntryCommentDto> AddEntryComment(EntryCommentModel aEntryCommentModel)
        {
            await mSnusPunchDbContext.AddAsync(aEntryCommentModel);
            await mSnusPunchDbContext.SaveChangesAsync();

            return await GetEntryCommentDtoById(aEntryCommentModel.Id);
        }

        public async Task RemoveEntryComment(EntryCommentModel aEntryCommentModel)
        {
            var sReplies = await mSnusPunchDbContext.EntryComments.Where(x => x.ParentCommentId == aEntryCommentModel.Id).ToListAsync();
            mSnusPunchDbContext.RemoveRange(sReplies);
            mSnusPunchDbContext.Remove(aEntryCommentModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task<EntryCommentModel> GetEntryCommentById(int aEntryCommentModelId)
        {
            return await mSnusPunchDbContext.EntryComments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == aEntryCommentModelId);
        }

        private async Task<EntryCommentDto> GetEntryCommentDtoById(int aEntryCommentModelId)
        {
            var sComment = await mSnusPunchDbContext.EntryComments
                .Include(x => x.SnusPunchUserModel)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == aEntryCommentModelId);

            return new EntryCommentDto
            {
                Id = sComment.Id,
                ParentId = sComment.ParentCommentId,
                CreateDate = sComment.CreateDate,
                Comment = sComment.Comment,
                UserName = sComment.SnusPunchUserModel.UserName,
                ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{sComment.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}",
            };
        }

        public async Task<PaginationResponse<EntryCommentDto>> GetEntryCommentsPaginated(PaginationParameters aPaginationParameters, int aEntryModelId, string aSnusPunchUserModelId)
        {
            var sLikes = await mSnusPunchDbContext.EntryComments
                .Where(x => x.EntryId == aEntryModelId && x.ParentCommentId == null) //Don't include comments with a parent (replies)
                .Include(x => x.SnusPunchUserModel)
                .Include(x => x.Replies)
                .Include(x => x.CommentLikes)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new EntryCommentDto
                {
                    UserName = x.SnusPunchUserModel.UserName,
                    ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}",
                    Comment = x.Comment,
                    CreateDate = x.CreateDate,
                    Id = x.Id,
                    LikedByUser = x.CommentLikes.Any(x => x.SnusPunchUserModelId == aSnusPunchUserModelId),
                    Likes = x.CommentLikes.Count,
                    ReplyCount = x.Replies.Count
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.EntryComments
                .Where(x => x.EntryId == aEntryModelId && x.ParentCommentId == null)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .CountAsync();

            return new PaginationResponse<EntryCommentDto>(sLikes, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }

        public async Task<PaginationResponse<EntryCommentDto>> GetEntryCommentRepliesPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId, string aSnusPunchUserModelId)
        {
            var sLikes = await mSnusPunchDbContext.EntryComments
                .Where(x => x.ParentCommentId == aEntryCommentModelId) //Don't include comments with a parent (replies)
                .Include(x => x.SnusPunchUserModel)
                .Include(x => x.Replies)
                .Include(x => x.CommentLikes)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new EntryCommentDto
                {
                    UserName = x.SnusPunchUserModel.UserName,
                    ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}",
                    Comment = x.Comment,
                    CreateDate = x.CreateDate,
                    Id = x.Id,
                    ParentId = x.ParentCommentId,
                    LikedByUser = x.CommentLikes.Any(x => x.SnusPunchUserModelId == aSnusPunchUserModelId),
                    Likes = x.CommentLikes.Count,
                    ReplyCount = x.Replies.Count
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.EntryComments
                .Where(x => x.ParentCommentId == aEntryCommentModelId && x.ParentCommentId == null)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .CountAsync();

            return new PaginationResponse<EntryCommentDto>(sLikes, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }
        #endregion


        #region Entry Comment Likes
        public async Task LikeComment(EntryCommentLikeModel aEntryCommentLikeModel)
        {
            await mSnusPunchDbContext.AddAsync(aEntryCommentLikeModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task UnlikeComment(EntryCommentLikeModel aEntryCommentLikeModel)
        {
            mSnusPunchDbContext.Remove(aEntryCommentLikeModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task<EntryCommentLikeModel> GetCommentLike(int aEntryCommentModelid, string aSnusPunchUserModelId)
        {
            var sLike = await mSnusPunchDbContext.EntryCommentLikes.FirstOrDefaultAsync(x => x.EntryCommentId == aEntryCommentModelid && x.SnusPunchUserModelId == aSnusPunchUserModelId);

            if (sLike == null)
            {
                throw new Exception("Liken hittades ej.");
            }

            return sLike;
        }

        public async Task<PaginationResponse<EntryLikeDto>> GetCommentLikesPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId)
        {
            var sLikes = await mSnusPunchDbContext.EntryCommentLikes
                .Where(x => x.EntryCommentId == aEntryCommentModelId)
                .Include(x => x.SnusPunchUserModel)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty("CreateDate", SortOrderEnum.Ascending)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new EntryLikeDto
                {
                    UserName = x.SnusPunchUserModel.UserName ?? "Raderad användare",
                    ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModel.ProfilePicturePath ?? "default.jpg"}",
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.EntryCommentLikes
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .Where(x => x.EntryCommentId == aEntryCommentModelId)
                .CountAsync();

            return new PaginationResponse<EntryLikeDto>(sLikes, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }
        #endregion


        #region Users
        public async Task<PaginationResponse<SnusPunchUserDto>> GetUsersPaginated(PaginationParameters aPaginationParameters)
        {
            var sUsers = await mSnusPunchDbContext.Users
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder)
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new SnusPunchUserDto
                {
                    UserName = x.UserName,
                    FavouriteSnus = x.FavoriteSnus.Name,
                    SnusPunches = x.Entries.Count,
                    ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.ProfilePicturePath ?? "default.jpg"}",
                    Friends = x.FriendsAddedByOthers.Count() + x.FriendsAddedByUser.Count()
                }).ToListAsync();

            var sCount = await mSnusPunchDbContext.Users
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .CountAsync();

            return new PaginationResponse<SnusPunchUserDto>(sUsers, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }

        public async Task<SnusPunchUserProfileDto> GetUserProfile(string aUserNameToFetch, string aUserIdToFetch, string aCurrentUserId)
        {
            return await mSnusPunchDbContext.Users
                .AsNoTracking().Select(x => new SnusPunchUserProfileDto
                {
                    UserName = x.UserName,
                    FavouriteSnus = x.FavoriteSnus.Name,
                    EntryCount = x.Entries.Count,
                    ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{x.ProfilePicturePath ?? "default.jpg"}",
                    FriendsCount = x.FriendsAddedByOthers.Count() + x.FriendsAddedByUser.Count(),
                    Friends = mSnusPunchDbContext.SnusPunchFriends
                        .Where(x => x.SnusPunchUserModelOneId == aUserIdToFetch || x.SnusPunchUserModelTwoId == aUserIdToFetch)
                        .OrderByDescending(x => x.CreateDate)
                        .Select(x => new SnusPunchUserDto
                        {
                            UserName = x.SnusPunchUserModelOneId == aUserIdToFetch ? x.SnusPunchUserModelTwo.UserName : x.SnusPunchUserModelOne.UserName,
                            ProfilePictureUrl = x.SnusPunchUserModelOneId == aUserIdToFetch ? $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModelTwo.ProfilePicturePath ?? "default.jpg"}" : $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModelOne.ProfilePicturePath ?? "default.jpg"}",
                        }).Take(9).ToList(),
                    PhotoEntries = x.Entries.Where(x => x.PhotoUrl != null).Select(x => new EntryDto
                    {
                        Id = x.Id,
                        PhotoUrl = mConfiguration["PostPicturePathFull"] + x.PhotoUrl
                    }).Take(9).ToList()
                }).FirstOrDefaultAsync(x => x.UserName == aUserNameToFetch);
        }

        public async Task<FriendshipStatusEnum> GetFriendshipStatus(string aUserIdToFetch, string aCurrentUserId)
        {
            if(await mSnusPunchDbContext.SnusPunchFriends.AnyAsync(x => (x.SnusPunchUserModelOneId == aUserIdToFetch && x.SnusPunchUserModelTwoId == aCurrentUserId) || (x.SnusPunchUserModelTwoId == aUserIdToFetch && x.SnusPunchUserModelOneId == aCurrentUserId)))
            {
                return FriendshipStatusEnum.Friends;
            }

            var sFriendRequestFromCurrentUser = await mSnusPunchDbContext.SnusPunchFriendRequests.FirstOrDefaultAsync(x => x.SnusPunchUserModelOneId == aCurrentUserId);

            if(sFriendRequestFromCurrentUser != null)
            {
                if(sFriendRequestFromCurrentUser.Denied)
                {
                    return FriendshipStatusEnum.Rejected;
                }

                return FriendshipStatusEnum.Pending;
            }

            var sFriendRequestFromUser = await mSnusPunchDbContext.SnusPunchFriendRequests.FirstOrDefaultAsync(x => x.SnusPunchUserModelOneId == aUserIdToFetch);

            if (sFriendRequestFromUser != null)
            {
                return FriendshipStatusEnum.Received;
            }


            return FriendshipStatusEnum.None;
        }
        #endregion


        #region Statistics
        public async Task<StatisticsTimePeriodResponseDto> GetStatisticsForTimePeriod(DateTime aStartDate, DateTime aEndDate, string aUserId)
        {
            //FromSql parameteriserar automatiskt FormattableString => Säkert mot SQL injections
            var sStats = await mSnusPunchDbContext.StatisticsTimePeriod
                .FromSql($@"
                    DECLARE @StartDate DATETIME = {aStartDate};
                    DECLARE @EndDate DATETIME = {aEndDate};
                    DECLARE @Days INT = 
                    CASE
	                    WHEN DATEDIFF(DAY, @StartDate, @EndDate) <= 1 THEN 1
	                    ELSE DATEDIFF(DAY, @StartDate, @EndDate)
                    END;

                    WITH MostUsedSnus AS
                    (
	                    SELECT TOP(1)
	                    [SnusName],
	                    COUNT(*) AS [UsedCount]

	                    FROM [tblEntry]
	
	                    WHERE
	                    [SnusPunchUserModelId] = {aUserId}
	                    AND [CreateDate] >= @StartDate
	                    AND [CreateDate] < @EndDate

	                    GROUP BY
	                    [SnusName]

	                    ORDER BY
	                    COUNT(*) DESC
                    )

                    SELECT
                    COUNT(*) AS 'SnusCount',
                    ISNULL(SUM([SnusPortionPriceInSek]), 0) AS 'TotalCostInSek',
                    ISNULL(SUM([SnusPortionNicotineInMg]), 0) AS 'TotalNicotineInMg',
                    ISNULL(CAST(COUNT(*) AS FLOAT)/@Days, 0) AS 'AvgSnusCountPerDay',
                    ISNULL(SUM([SnusPortionPriceInSek])/@Days, 0) AS 'AvgCostPerDayInSek',
                    ISNULL(SUM([SnusPortionNicotineInMg])/@Days, 0) AS 'AvgNicotinePerDayInMg',
                    (SELECT [SnusName] FROM MostUsedSnus) AS 'MostUsedSnus',
                    (SELECT [UsedCount] FROM MostUsedSnus) AS 'MostUsedSnusCount'

                    FROM [tblEntry]

                    WHERE
                    [SnusPunchUserModelId] = {aUserId}
                    AND [CreateDate] >= @StartDate
                    AND [CreateDate] < @EndDate")
                    .ToListAsync();

            return sStats.FirstOrDefault();
        }
        #endregion


        #region Friends
        public async Task AddFriendRequest(SnusPunchFriendRequestModel aSnusPunchFriendRequestModel)
        {
            await mSnusPunchDbContext.AddAsync(aSnusPunchFriendRequestModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task AddFriendship(SnusPunchFriendModel aSnusPunchFriendModel)
        {
            await mSnusPunchDbContext.AddAsync(aSnusPunchFriendModel);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task RemoveFriendRequest(string aSnusPunchUserModelIdOne, string aSnusPunchUserModelIdTwo)
        {
            var sFriendRequest = await GetFriendRequestModel(aSnusPunchUserModelIdOne, aSnusPunchUserModelIdTwo);

            if(sFriendRequest == null)
            {
                throw new Exception("Vänförfrågan existerar ej");
            }

            mSnusPunchDbContext.Remove(sFriendRequest);
            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task DenyFriendRequest(string aSnusPunchUserModelIdOne, string aSnusPunchUserModelIdTwo)
        {
            var sFriendRequest = await GetFriendRequestModel(aSnusPunchUserModelIdOne, aSnusPunchUserModelIdTwo);

            if (sFriendRequest == null)
            {
                throw new Exception("Vänförfrågan existerar ej");
            }

            sFriendRequest.Denied = true;

            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task<SnusPunchFriendRequestModel> GetFriendRequestModel(string aSnusPunchUserModelIdOne, string aSnusPunchUserModelIdTwo)
        {
            return await mSnusPunchDbContext.SnusPunchFriendRequests.FirstOrDefaultAsync(x => x.SnusPunchUserModelOneId == aSnusPunchUserModelIdOne && x.SnusPunchUserModelTwoId == aSnusPunchUserModelIdTwo);
        }

        public async Task RemoveFriend(string aSnusPunchUserModelIdOne, string aSnusPunchUserModelIdTwo)
        {
            var sFriendship = await mSnusPunchDbContext.SnusPunchFriends.FirstOrDefaultAsync(x => (x.SnusPunchUserModelOneId == aSnusPunchUserModelIdOne && x.SnusPunchUserModelTwoId == aSnusPunchUserModelIdTwo) || (x.SnusPunchUserModelTwoId == aSnusPunchUserModelIdOne && x.SnusPunchUserModelOneId == aSnusPunchUserModelIdTwo));

            if(sFriendship == null)
            {
                throw new Exception("Du är inte vän med denna användare.");
            }

            mSnusPunchDbContext.Remove(sFriendship);

            await mSnusPunchDbContext.SaveChangesAsync();
        }

        public async Task<PaginationResponse<SnusPunchUserDto>> GetFriendsPaginated(PaginationParameters aPaginationParameters, string aUserName)
        {
            var sQuery = mSnusPunchDbContext.SnusPunchFriends
                .Where(x => x.SnusPunchUserModelOne.UserName == aUserName || x.SnusPunchUserModelTwo.UserName == aUserName)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .OrderByProperty(aPaginationParameters.SortPropertyName, aPaginationParameters.SortOrder).AsQueryable();

            var sSearchString = aPaginationParameters.SearchString;

            if (!aPaginationParameters.SearchString.IsNullOrEmpty())
            {
                sQuery = sQuery.Where(x => (x.SnusPunchUserModelOne.UserName.Contains(sSearchString) && x.SnusPunchUserModelOne.UserName != aUserName) || (x.SnusPunchUserModelTwo.UserName.Contains(sSearchString) && x.SnusPunchUserModelTwo.UserName != aUserName));
            }

            var sFriends = await sQuery
                .Skip(aPaginationParameters.Skip)
                .Take(aPaginationParameters.PageSize)
                .AsNoTracking().Select(x => new SnusPunchUserDto
                {
                    UserName = x.SnusPunchUserModelOne.UserName == aUserName ? x.SnusPunchUserModelTwo.UserName : x.SnusPunchUserModelOne.UserName,
                    ProfilePictureUrl = x.SnusPunchUserModelOne.UserName == aUserName ? $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModelTwo.ProfilePicturePath ?? "default.jpg"}" : $"{mConfiguration["ProfilePicturePathFull"]}{x.SnusPunchUserModelOne.ProfilePicturePath ?? "default.jpg"}",
                }).ToListAsync();

            int sCount = 0;

            if (!aPaginationParameters.SearchString.IsNullOrEmpty())
            {
                sCount = await mSnusPunchDbContext.SnusPunchFriends
                .Where(x => x.SnusPunchUserModelOne.UserName == aUserName || x.SnusPunchUserModelTwo.UserName == aUserName)
                .Where(x => (x.SnusPunchUserModelOne.UserName.Contains(sSearchString) && x.SnusPunchUserModelOne.UserName != aUserName) || (x.SnusPunchUserModelTwo.UserName.Contains(sSearchString) && x.SnusPunchUserModelTwo.UserName != aUserName))
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .CountAsync();
            }
            else
            {
                sCount = await mSnusPunchDbContext.SnusPunchFriends
                .Where(x => x.SnusPunchUserModelOne.UserName == aUserName || x.SnusPunchUserModelTwo.UserName == aUserName)
                .SearchByProperty(aPaginationParameters.SearchPropertyNames, aPaginationParameters.SearchString)
                .CountAsync();
            }

            return new PaginationResponse<SnusPunchUserDto>(sFriends, sCount, aPaginationParameters.PageNumber, aPaginationParameters.PageSize);
        }
        #endregion
    }
}
