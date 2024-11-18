using Microsoft.Extensions.Logging;
using SnusPunch.Data.Repository;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Services.Snus
{
    public class UserService
    {
        private readonly ILogger<UserService> mLogger;
        private readonly SnusPunchRepository mSnusPunchRepository;

        public UserService(ILogger<UserService> aLogger, SnusPunchRepository aSnusPunchRepository)
        {
            mLogger = aLogger;
            mSnusPunchRepository = aSnusPunchRepository;
        }

        public async Task<ResultModel<PaginationResponse<SnusPunchUserDto>>> GetUsersPaginated(PaginationParameters aPaginationParameters)
        {
            ResultModel<PaginationResponse<SnusPunchUserDto>> sResultModel = new ResultModel<PaginationResponse<SnusPunchUserDto>>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetUsersPaginated(aPaginationParameters);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetUsersPaginated in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
