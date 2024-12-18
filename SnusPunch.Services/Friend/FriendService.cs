﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.NotificationService;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Notification;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.Services.Snus
{
    public class FriendService
    {
        private readonly ILogger<FriendService> mLogger;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;
        private readonly NotificationHub mNotificationHub;

        public FriendService(ILogger<FriendService> aLogger, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager, NotificationHub aNotificationHub)
        {
            mLogger = aLogger;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
            mNotificationHub = aNotificationHub;
        }

        public async Task<ResultModel> SendFriendRequest(string aSnusPunchUserName, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sUser2 = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aSnusPunchUserName);

                if (sUser2 == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Användaren \"{aSnusPunchUserName}\" hittades ej.");
                    return sResultModel;
                }

                if(await mSnusPunchRepository.GetFriendRequestModel(sUser.Id, sUser2.Id) != null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Du har redan skickat en vänförfrågan till {aSnusPunchUserName}");
                    return sResultModel;
                }

                SnusPunchFriendRequestModel sSnusPunchFriendRequestModel = new SnusPunchFriendRequestModel
                {
                    SnusPunchUserModelOneId = sUser.Id,
                    SnusPunchUserModelTwoId = sUser2.Id
                };

                await mSnusPunchRepository.AddFriendRequest(sSnusPunchFriendRequestModel);

                //Skicka notis
                await mNotificationHub.SendNotification(NotificationTypeEnum.FriendRequest ,$"Du har fått en vänförfrågan av {sUser.UserName}!", sUser2.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at SendFriendRequest in FriendService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveFriendRequest(string aSnusPunchUserName, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sUser2 = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aSnusPunchUserName);

                if (sUser2 == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Användaren \"{aSnusPunchUserName}\" hittades ej.");
                    return sResultModel;
                }

                await mSnusPunchRepository.RemoveFriendRequest(sUser.Id, sUser2.Id);

                //Skicka notis
                await mNotificationHub.SendNotification(NotificationTypeEnum.FriendRequestRemoved, $"Din vänförfrågan från {sUser.UserName} har tagits tillbaka :(", sUser2.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveFriendRequest in FriendService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> DenyFriendRequest(string aSnusPunchUserName, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sUser2 = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aSnusPunchUserName);

                if (sUser2 == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Användaren \"{aSnusPunchUserName}\" hittades ej.");
                    return sResultModel;
                }

                await mSnusPunchRepository.DenyFriendRequest(sUser.Id, sUser2.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at DenyFriendRequest in FriendService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveFriend(string aSnusPunchUserName, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sUser2 = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aSnusPunchUserName);

                if (sUser2 == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Användaren \"{aSnusPunchUserName}\" hittades ej.");
                    return sResultModel;
                }

                await mSnusPunchRepository.RemoveFriend(sUser.Id, sUser2.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveFriend in FriendService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> AcceptFriendRequest(string aSnusPunchUserName, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sUser2 = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aSnusPunchUserName);

                if (sUser2 == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Användaren \"{aSnusPunchUserName}\" hittades ej.");
                    return sResultModel;
                }

                var sFriendRequest = await mSnusPunchRepository.GetFriendRequestModel(sUser2.Id, sUser.Id);

                if(sFriendRequest == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Vänförfrågan existerar ej.");
                    return sResultModel;
                }

                SnusPunchFriendModel sSnusPunchFriendModel = new SnusPunchFriendModel
                {
                    SnusPunchUserModelOneId = sUser2.Id,
                    SnusPunchUserModelTwoId = sUser.Id
                };

                await mSnusPunchRepository.AddFriendship(sSnusPunchFriendModel);

                //Radera förfrågan
                await mSnusPunchRepository.RemoveFriendRequest(sUser2.Id, sUser.Id);

                //Radera den från andra hållet också om den finns
                var sFriendRequest2 = await mSnusPunchRepository.GetFriendRequestModel(sUser.Id, sUser2.Id);

                if(sFriendRequest2 != null)
                {
                    await mSnusPunchRepository.RemoveFriendRequest(sUser.Id, sUser2.Id);
                }
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at AcceptFriendRequest in FriendService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<PaginationResponse<SnusPunchUserDto>>> GetFriendsForUser(PaginationParameters aPaginationParameters, string aUserName)
        {
            ResultModel<PaginationResponse<SnusPunchUserDto>> sResultModel = new ResultModel<PaginationResponse<SnusPunchUserDto>>();

            try
            {
                var sUserToFetch = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aUserName);

                if (sUserToFetch == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetFriendsPaginated(aPaginationParameters, aUserName);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetFriendsForUser in FriendService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<List<FriendRequestDto>>> GetAllFriendRequests(ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<List<FriendRequestDto>> sResultModel = new ResultModel<List<FriendRequestDto>>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetAllFriendRequests(sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetAllFriendRequests in FriendService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
