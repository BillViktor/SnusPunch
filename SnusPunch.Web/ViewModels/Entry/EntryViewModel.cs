using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Clients.Snus;

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

        public async Task<PaginationResponse<EntryDto>> GetEntriesPaginated(PaginationParameters aPaginationParameters)
        {
            IsBusy = true;
            PaginationResponse<EntryDto> sPaginationResponse = new PaginationResponse<EntryDto>();

            var sResult = await mEntryClient.GetEntriesPaginated(aPaginationParameters);

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

        public async Task<EntryDto> AddEntry(int aSnusId, string? aDescription)
        {
            IsBusy = true;
            EntryDto? sReturnEntry = null;

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
    }
}
