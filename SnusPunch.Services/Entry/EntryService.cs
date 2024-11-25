using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.Services.Entry
{
    public class EntryService
    {
        private readonly ILogger<EntryService> mLogger;
        private readonly IConfiguration mConfiguration;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;

        public EntryService(ILogger<EntryService> aLogger, IConfiguration aConfiguration, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager)
        {
            mLogger = aLogger;
            mConfiguration = aConfiguration;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
        }

        public async Task<ResultModel<PaginationResponse<EntryDto>>> GetEntriesPaginated(PaginationParameters aPaginationParameters, bool aFetchEmptyPunches, EntryFilterEnum aEntryFilterEnum, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<PaginationResponse<EntryDto>> sResultModel = new ResultModel<PaginationResponse<EntryDto>>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if(sUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetEntriesPaginated(aPaginationParameters, aFetchEmptyPunches, aEntryFilterEnum, sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetEntriesPaginated in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<EntryDto>> AddEntry(AddEntryDto aAddEntryDto, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<EntryDto> sResultModel = new ResultModel<EntryDto>();

            try
            {
                var sUserId = mUserManager.GetUserId(aClaimsPrincipal);

                if(sUserId == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                EntryModel sEntryModel = new EntryModel
                {
                    Description = string.IsNullOrEmpty(aAddEntryDto.Description) ? null : aAddEntryDto.Description,
                    SnusId = aAddEntryDto.SnusId,
                    SnusPunchUserModelId = sUserId
                };
                
                //Spara bild om vi fick en sån
                //if (aAddEntryDto.FormFile != null)
                //{
                //    //Verifiera filen
                //    var sVerifyFileResult = ImageFileVerification.IsValidImage(aAddEntryDto.FormFile);
                //    if (!sVerifyFileResult.Success)
                //    {
                //        sResultModel.Errors = sVerifyFileResult.Errors;
                //        sResultModel.Success = false;
                //        return sResultModel;
                //    }

                //    //Fil OK, generera en Guid för filnamnet (för att undvika att användare gissar sig till en annans profilbild)
                //    Guid sFileName = Guid.NewGuid();

                //    string sFilePath = mConfiguration["PostPicturePath"] + $"{sFileName}.jpg";

                //    //Konvertera filen till jpg & spara på disk
                //    using (var sStream = new MemoryStream())
                //    {
                //        await aAddEntryDto.FormFile.CopyToAsync(sStream);

                //        using (var sImage = System.Drawing.Image.FromStream(sStream))
                //        {
                //            sImage.Save(sFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //        }
                //    }

                //    sEntryModel.PhotoUrl = sFileName + ".jpg";
                //}

                sResultModel.ResultObject = await mSnusPunchRepository.AddEntry(sEntryModel);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at AddEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveEntry(int aEntryModelId, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUserId = mUserManager.GetUserId(aClaimsPrincipal);

                if (sUserId == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sEntry = await mSnusPunchRepository.GetEntryById(aEntryModelId);

                //Ta bort bilden om den finns
                if(!string.IsNullOrEmpty(sEntry.PhotoUrl))
                {
                    string sFile = mConfiguration["PostPicturePath"] + $"/{sEntry.PhotoUrl}";

                    if (File.Exists(sFile))
                    {
                        File.Delete(sFile);
                    }
                }

                if (sEntry == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Inlägget hittades ej.");
                    return sResultModel;
                }

                //Verifiera konto
                if (sEntry.SnusPunchUserModelId != sUserId)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Du saknar behörighet för att radera detta inlägg.");
                    return sResultModel;
                }

                await mSnusPunchRepository.RemoveEntry(sEntry);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveEntry(int aEntryModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sEntry = await mSnusPunchRepository.GetEntryById(aEntryModelId);

                if (sEntry == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Inlägget hittades ej.");
                    return sResultModel;
                }

                await mSnusPunchRepository.RemoveEntry(sEntry);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        #region Likes
        public async Task<ResultModel> LikeEntry(int aEntryModelId, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if(sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sEntry = await mSnusPunchRepository.GetEntryById(aEntryModelId);

                if (sEntry == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Inlägget hittades ej.");
                    return sResultModel;
                }

                EntryLikeModel sEntryLikeModel = new EntryLikeModel
                {
                    EntryId = sEntry.Id,
                    SnusPunchUserModelId = sUser.Id
                };

                await mSnusPunchRepository.LikeEntry(sEntryLikeModel);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at LikeEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> UnlikeEntry(int aEntryModelId, ClaimsPrincipal aClaimsPrincipal)
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

                var sLike = await mSnusPunchRepository.GetEntryLike(aEntryModelId, sUser.Id);

                await mSnusPunchRepository.UnlikeEntry(sLike);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at UnlikeEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<PaginationResponse<EntryLikeDto>>> GetEntryLikesPaginated(PaginationParameters aPaginationParameters, int aEntryModelId)
        {
            ResultModel<PaginationResponse<EntryLikeDto>> sResultModel = new ResultModel<PaginationResponse<EntryLikeDto>>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetEntryLikesPaginated(aPaginationParameters, aEntryModelId);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at UnlikeEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
        #endregion
    }
}
