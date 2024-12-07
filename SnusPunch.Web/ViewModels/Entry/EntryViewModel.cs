using Microsoft.AspNetCore.Components.Forms;
using SnusPunch.Shared.Constants;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Web.Clients.Snus;
using System.Net.Http.Headers;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class EntryViewModel : BaseViewModel
    {
        private readonly EntryClient mEntryClient;

        #region Fields
        private List<SnusPunchUserDto> mUsers = new List<SnusPunchUserDto>();
        #endregion


        #region Properties
        public List<SnusPunchUserDto> Snus { get { return mUsers; } set { SetValue(ref mUsers, value); } }
        #endregion

        public EntryViewModel(EntryClient aEntryClient)
        {
            mEntryClient = aEntryClient;
        }

        public async Task<PaginationResponse<EntryDto>> GetEntriesPaginated(PaginationParameters aPaginationParameters, bool aFetchEmptyPunches, EntryFilterEnum aEntryFilterEnum)
        {
            IsBusy = true;
            PaginationResponse<EntryDto> sPaginationResponse = new PaginationResponse<EntryDto>();

            var sResult = await mEntryClient.GetEntriesPaginated(aPaginationParameters, aFetchEmptyPunches, aEntryFilterEnum);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sPaginationResponse = sResult.ResultObject;
            }

            IsBusy = false;
            return sPaginationResponse;
        }

        public async Task<EntryDto> AddEntry(int aSnusId, string? aDescription, IBrowserFile aBrowserFile)
        {
            return aBrowserFile == null ? await AddEntry(aSnusId, aDescription) : await AddEntryWithImage(aSnusId, aDescription, aBrowserFile);
        }

        private async Task<EntryDto> AddEntryWithImage(int aSnusId, string? aDescription, IBrowserFile aBrowserFile)
        {
            IsBusy = true;
            EntryDto? sReturnEntry = null;

            try
            {
                if (aBrowserFile == null)
                {
                    throw new Exception("Ingen fil vald.");
                }

                if (!AllowedImageFileTypes.AllowedMimeTypes.Any(x => string.Equals(x, aBrowserFile.ContentType, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Ogiltigt filformat.");
                }

                if (aBrowserFile.Size > AllowedImageFileTypes.ImageMaximumBytes)
                {
                    throw new Exception("Filen är för stor.");
                }

                using var sContent = new MultipartFormDataContent();
                var sFileContent = new StreamContent(aBrowserFile.OpenReadStream(AllowedImageFileTypes.ImageMaximumBytes));
                sFileContent.Headers.ContentType = new MediaTypeHeaderValue(aBrowserFile.ContentType);

                sContent.Add(sFileContent, "FormFile", aBrowserFile.Name);
                if (aDescription != null)
                {
                    sContent.Add(new StringContent(aDescription), "Description");
                }
                sContent.Add(new StringContent(aSnusId.ToString()), "SnusId");

                var sResult = await mEntryClient.AddEntryWithImage(sContent);

                if (!sResult.Success)
                {
                    Errors.AddRange(sResult.Errors);
                }
                else
                {
                    sReturnEntry = sResult.ResultObject;
                    SuccessMessages.Add($"Din profilbild har uppdaterats");
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return null;
            }

            IsBusy = false;
            return sReturnEntry;
        }

        private async Task<EntryDto> AddEntry(int aSnusId, string? aDescription)
        {
            EntryDto? sReturnEntry = null;
            IsBusy = true;

            AddEntryDto sAddEntryDto = new AddEntryDto
            {
                SnusId = aSnusId,
                Description = aDescription
            };

            var sResult = await mEntryClient.AddEntry(sAddEntryDto);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sReturnEntry = sResult.ResultObject;
                SuccessMessages.Add("Stämpling genomförd!");
            }

            IsBusy = false;
            return sReturnEntry;
        }

        public async Task<bool> RemoveEntry(int aEntryModelId)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mEntryClient.RemoveEntry(aEntryModelId);

            if (!sResult.Success)
            {
                sSuccess = false;
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                SuccessMessages.Add("Stämpling raderad!");
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<bool> AdminRemoveEntry(int aEntryModelId)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mEntryClient.AdminRemoveEntry(aEntryModelId);

            if (!sResult.Success)
            {
                sSuccess = false;
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                SuccessMessages.Add("Stämpling raderad!");
            }

            IsBusy = false;
            return sSuccess;
        }


        #region Entry Likes
        public async Task<bool> LikeEntry(int aEntryModelId)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mEntryClient.LikeEntry(aEntryModelId);

            if (!sResult.Success)
            {
                sSuccess = false;
                Errors.AddRange(sResult.Errors);
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<bool> UnlikeEntry(int aEntryModelId)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mEntryClient.UnlikeEntry(aEntryModelId);

            if (!sResult.Success)
            {
                sSuccess = false;
                Errors.AddRange(sResult.Errors);
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<PaginationResponse<EntryLikeDto>> GetEntryLikesPaginated(PaginationParameters aPaginationParameters, int aEntryModelId)
        {
            IsBusy = true;
            PaginationResponse<EntryLikeDto> sPaginationResponse = new PaginationResponse<EntryLikeDto>();

            var sResult = await mEntryClient.GetEntryLikesPaginated(aPaginationParameters, aEntryModelId);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sPaginationResponse = sResult.ResultObject;
            }

            IsBusy = false;
            return sPaginationResponse;
        }
        #endregion


        #region Comments
        public async Task<PaginationResponse<EntryCommentDto>> GetEntryComments(PaginationParameters aPaginationParameters, int aEntryModelId)
        {
            PaginationResponse<EntryCommentDto> sList = new PaginationResponse<EntryCommentDto>();
            IsBusy = true;

            var sResult = await mEntryClient.GetEntryCommentsPaginated(aPaginationParameters, aEntryModelId);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sList = sResult.ResultObject;
            }

            IsBusy = false;
            return sList;
        }

        public async Task<PaginationResponse<EntryCommentDto>> GetEntryCommentRepliesPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId)
        {
            PaginationResponse<EntryCommentDto> sList = new PaginationResponse<EntryCommentDto>();
            IsBusy = true;

            var sResult = await mEntryClient.GetEntryCommentRepliesPaginated(aPaginationParameters, aEntryCommentModelId);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sList = sResult.ResultObject;
            }

            IsBusy = false;
            return sList;
        }

        public async Task<EntryCommentDto> AddEntryComment(AddEntryCommentDto sAddEntryCommentDto)
        {
            EntryCommentDto? sReturnEntryComment = null;
            IsBusy = true;

            var sResult = await mEntryClient.AddEntryComment(sAddEntryCommentDto);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sReturnEntryComment = sResult.ResultObject;
                SuccessMessages.Add("Kommentar postad!");
            }

            IsBusy = false;
            return sReturnEntryComment;
        }

        public async Task<bool> RemoveEntryComment(int aEntryCommentModelId)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mEntryClient.RemoveEntryComment(aEntryCommentModelId);

            if (!sResult.Success)
            {
                sSuccess = false;
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                SuccessMessages.Add("Kommentar raderad!");
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<bool> AdminRemoveEntryComment(int aEntryCommentModelId)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mEntryClient.AdminRemoveEntryComment(aEntryCommentModelId);

            if (!sResult.Success)
            {
                sSuccess = false;
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                SuccessMessages.Add("Kommentar raderad!");
            }

            IsBusy = false;
            return sSuccess;
        }
        #endregion


        #region Comment Likes
        public async Task<bool> LikeComment(int aEntryCommentModelId)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mEntryClient.LikeComment(aEntryCommentModelId);

            if (!sResult.Success)
            {
                sSuccess = false;
                Errors.AddRange(sResult.Errors);
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<bool> UnlikeComment(int aEntryCommentModelId)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mEntryClient.UnlikeComment(aEntryCommentModelId);

            if (!sResult.Success)
            {
                sSuccess = false;
                Errors.AddRange(sResult.Errors);
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<PaginationResponse<EntryLikeDto>> GetCommentLikesPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId)
        {
            IsBusy = true;
            PaginationResponse<EntryLikeDto> sPaginationResponse = new PaginationResponse<EntryLikeDto>();

            var sResult = await mEntryClient.GetCommentLikesPaginated(aPaginationParameters, aEntryCommentModelId);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sPaginationResponse = sResult.ResultObject;
            }

            IsBusy = false;
            return sPaginationResponse;
        }
        #endregion
    }
}
