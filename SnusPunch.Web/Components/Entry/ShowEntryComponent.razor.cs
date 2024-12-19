using Blazored.Modal.Services;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Components.Entry
{
    public partial class ShowEntryComponent
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Parameter] public EntryDto EntryDto { get; set; }
        [Inject] EntryViewModel EntryViewModel { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }

        private PaginationMetaData mPaginationMetaDataComments = null;
        private PaginationParameters mPaginationParametersComments = new PaginationParameters
        {
            SortPropertyName = "CommentLikes.Count",
            SortOrder = SortOrderEnum.Descending,
            PageSize = 5
        };

        private string mCommentPlaceholder = "Skriv en kommentar";
        private AddEntryCommentDto mAddEntryCommentDto = new AddEntryCommentDto();

        protected override async Task OnInitializedAsync()
        {
            mAddEntryCommentDto.EntryModelId = EntryDto.Id;
            await RefreshComments();
        }

        #region Comments
        private async Task OnCommentSortChange(ChangeEventArgs aChangeEventArgs)
        {
            var sSortProperty = aChangeEventArgs.Value.ToString();

            switch (sSortProperty)
            {
                case "Popular":
                    mPaginationParametersComments.SortPropertyName = "CommentLikes.Count";
                    mPaginationParametersComments.SortOrder = SortOrderEnum.Descending;
                    break;
                case "Oldest":
                    mPaginationParametersComments.SortPropertyName = "CreateDate";
                    mPaginationParametersComments.SortOrder = SortOrderEnum.Ascending;
                    break;
                case "Newest":
                default:
                    mPaginationParametersComments.SortPropertyName = "CreateDate";
                    mPaginationParametersComments.SortOrder = SortOrderEnum.Descending;
                    break;
            }

            await RefreshComments();
        }

        private async Task RefreshComments()
        {
            mPaginationParametersComments.PageNumber = 1;
            var sComments = await EntryViewModel.GetEntryComments(mPaginationParametersComments, EntryDto.Id);
            EntryDto.Comments = sComments.Items;
            mPaginationMetaDataComments = sComments.PaginationMetaData;
        }

        private async Task FetchMoreComments()
        {
            mPaginationParametersComments.PageNumber++;

            var sComments = await EntryViewModel.GetEntryComments(mPaginationParametersComments, EntryDto.Id);
            mPaginationMetaDataComments = sComments.PaginationMetaData;

            EntryDto.Comments.AddRange(sComments.Items);
            EntryDto.Comments = EntryDto.Comments.DistinctBy(x => x.Id).ToList();

            StateHasChanged();
        }

        private async Task AddEntryComment()
        {
            if (string.IsNullOrEmpty(mAddEntryCommentDto.Comment))
            {
                EntryViewModel.AddError("Lite tråkigt att skicka en tom kommentar?");
                return;
            }

            var sResult = await EntryViewModel.AddEntryComment(mAddEntryCommentDto);

            if (sResult != null)
            {
                if(mAddEntryCommentDto.ParentId == null)
                {
                    EntryDto.Comments.Insert(0, sResult);
                }
                else
                {
                    var sCommentRepliedto = EntryDto.Comments.FirstOrDefault(x => x.Id == mAddEntryCommentDto.ParentId);
                    sCommentRepliedto.Replies.Add(sResult);
                    sCommentRepliedto.ReplyCount++;
                }
                EntryDto.CommentCount++;

                mAddEntryCommentDto.Comment = "";
                mCommentPlaceholder = "Skriv en kommentar";
                mAddEntryCommentDto.ParentId = null;
                mAddEntryCommentDto.SnusPunchUserNameRepliedTo = null;

                await JSRuntime.InvokeVoidAsync("backToTop");
            }

            StateHasChanged();
        }

        private async Task Comment()
        {
            mAddEntryCommentDto.ParentId = null;
            mAddEntryCommentDto.SnusPunchUserNameRepliedTo = null;
            mCommentPlaceholder = "Skriv en kommentar";
            await JSRuntime.InvokeVoidAsync("focusElement", "commentInput");
        }

        private async Task ReplyToComment(EntryCommentDto aEntryCommentDto)
        {
            mAddEntryCommentDto.Comment = "";
            mAddEntryCommentDto.ParentId = aEntryCommentDto.ParentId ?? aEntryCommentDto.Id;
            mAddEntryCommentDto.SnusPunchUserNameRepliedTo = aEntryCommentDto.ParentId == null ? null : aEntryCommentDto.UserName;
            mCommentPlaceholder = $"Svarar {aEntryCommentDto.UserName}";
            await JSRuntime.InvokeVoidAsync("focusElement", "commentInput");
        }

        private async Task RemoveEntryComment(EntryCommentDto aEntryCommentDto)
        {
            if (!await ConfirmDeleteEntry()) return;

            if (await EntryViewModel.RemoveEntryComment(aEntryCommentDto.Id))
            {
                if (aEntryCommentDto.ParentId == null)
                {
                    EntryDto.Comments.Remove(aEntryCommentDto);
                }
                else
                {
                    var sParent = EntryDto.Comments.FirstOrDefault(x => x.Id == aEntryCommentDto.ParentId);
                    sParent.Replies.Remove(aEntryCommentDto);
                    sParent.ReplyCount--;
                }

                EntryDto.CommentCount--;
            }

            StateHasChanged();
        }

        private async Task AdminRemoveEntryComment(EntryCommentDto aEntryCommentDto)
        {
            if (!await ConfirmDeleteEntry()) return;

            if (await EntryViewModel.AdminRemoveEntryComment(aEntryCommentDto.Id))
            {
                EntryDto.Comments.Remove(aEntryCommentDto);
                EntryDto.CommentCount--;
            }

            StateHasChanged();
        }
        #endregion


        #region RemoveEntry
        private async Task RemoveEntry(EntryDto aEntryDto)
        {
            if (!await ConfirmDeleteEntry()) return;

            if (await EntryViewModel.RemoveEntry(aEntryDto.Id))
            {
                await BlazoredModal.CloseAsync();
            }
        }

        private async Task AdminRemoveEntry(EntryDto aEntryDto)
        {
            if (!await ConfirmDeleteEntry()) return;

            if (await EntryViewModel.AdminRemoveEntry(aEntryDto.Id))
            {
                await BlazoredModal.CloseAsync();
            }
        }

        private async Task<bool> ConfirmDeleteEntry()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sParameters = new ModalParameters { { "Message", $"Är du säker på att du vill radera detta inlägg? Det går inte att ångra!" } };
            var sModal = Modal.Show<ConfirmationComponent>("Bekräfta borttagning", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                return true;
            }

            return false;
        }
        #endregion


        #region Entry Likes
        private async Task ToggleLike(EntryDto aEntryDto)
        {
            if (aEntryDto.LikedByUser)
            {
                if (await EntryViewModel.UnlikeEntry(aEntryDto.Id))
                {
                    aEntryDto.Likes--;
                    aEntryDto.LikedByUser = false;
                }
            }
            else
            {
                if (await EntryViewModel.LikeEntry(aEntryDto.Id))
                {
                    aEntryDto.Likes++;
                    aEntryDto.LikedByUser = true;
                }
            }

            StateHasChanged();
        }

        private void ShowLikes(EntryDto aEntryDto)
        {
            if (aEntryDto.Likes == 0)
            {
                EntryViewModel.AddError("Inlägget har inga likes :(");
                return;
            }

            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = false,
                Size = ModalSize.Medium,
                Position = ModalPosition.Middle
            };

            var sParametes = new ModalParameters { { "EntryModelId", aEntryDto.Id } };

            Modal.Show<ShowEntryLikesComponent>("Likes", sParametes, sOptions);
        }
        #endregion


        #region Comment Likes
        private async Task ToggleLike(EntryCommentDto aEntryCommentDto)
        {
            if (aEntryCommentDto.LikedByUser)
            {
                if (await EntryViewModel.UnlikeComment(aEntryCommentDto.Id))
                {
                    aEntryCommentDto.Likes--;
                    aEntryCommentDto.LikedByUser = false;
                }
            }
            else
            {
                if (await EntryViewModel.LikeComment(aEntryCommentDto.Id))
                {
                    aEntryCommentDto.Likes++;
                    aEntryCommentDto.LikedByUser = true;
                }
            }

            StateHasChanged();
        }

        
        #endregion
    }
}
