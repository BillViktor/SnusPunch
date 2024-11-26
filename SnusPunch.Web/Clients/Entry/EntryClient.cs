﻿using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Net.Http.Json;
using System.Security.Claims;

namespace SnusPunch.Web.Clients.Snus
{
    public class EntryClient
    {
        private readonly IHttpClientFactory mHttpClientFactory;

        public EntryClient(IHttpClientFactory aHttpClientFactory)
        {
            mHttpClientFactory = aHttpClientFactory;
        }

        public async Task<ResultModel<PaginationResponse<EntryDto>>> GetEntriesPaginated(PaginationParameters aPaginationParameters, bool aFetchEmptyPunches, EntryFilterEnum aEntryFilterEnum)
        {
            ResultModel<PaginationResponse<EntryDto>> sResultModel = new ResultModel<PaginationResponse<EntryDto>>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Entry.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync($"GetEntriesPaginated/{aFetchEmptyPunches}/{aEntryFilterEnum}", aPaginationParameters);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at GetEntriesPaginated in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<PaginationResponse<EntryDto>>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<EntryDto>> AddEntry(AddEntryDto aAddEntryDto)
        {
            ResultModel<EntryDto> sResultModel = new ResultModel<EntryDto>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Entry.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("AddEntry", aAddEntryDto);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at AddEntry in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<EntryDto>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<EntryDto>> AddEntryWithImage(MultipartFormDataContent aMultipartFormDataContent)
        {
            ResultModel<EntryDto> sResultModel = new ResultModel<EntryDto>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Entry.ToString());
                var sResponse = await sHttpClient.PostAsync("AddEntryWithImage", aMultipartFormDataContent);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at AddEntryWithImage in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<EntryDto>>();
            }
            catch (Exception aException)
            {
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
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Entry.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"RemoveEntry/{aEntryModelId}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at RemoveEntry in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> AdminRemoveEntry(int aEntryModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Entry.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"AdminRemoveEntry/{aEntryModelId}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at AdminRemoveEntry in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }


        #region Likes
        public async Task<ResultModel> LikeEntry(int aEntryModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.EntryLike.ToString());
                var sResponse = await sHttpClient.PostAsync($"LikeEntry/{aEntryModelId}", null);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at LikeEntry in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> UnlikeEntry(int aEntryModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.EntryLike.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"UnlikeEntry/{aEntryModelId}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at UnlikeEntry in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
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
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.EntryLike.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync($"GetEntryLikesPaginated/{aEntryModelId}", aPaginationParameters);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at GetEntryLikesPaginated in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<PaginationResponse<EntryLikeDto>>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
        #endregion


        #region Comments
        public async Task<ResultModel<PaginationResponse<EntryCommentDto>>> GetEntryCommentsPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId)
        {
            ResultModel<PaginationResponse<EntryCommentDto>> sResultModel = new ResultModel<PaginationResponse<EntryCommentDto>>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.EntryComment.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync($"GetEntryCommentsPaginated/{aEntryCommentModelId}", aPaginationParameters);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at GetEntryCommentsPaginated in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<PaginationResponse<EntryCommentDto>>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<EntryCommentDto>> AddEntryComment(AddEntryCommentDto aAddEntryCommentDto)
        {
            ResultModel<EntryCommentDto> sResultModel = new ResultModel<EntryCommentDto>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.EntryComment.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("AddEntryComment", aAddEntryCommentDto);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at AddEntryComment in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<EntryCommentDto>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveEntryComment(int aEntryCommentModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.EntryComment.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"RemoveEntryComment/{aEntryCommentModelId}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at RemoveEntry in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> AdminRemoveEntryComment(int aEntryCommentModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.EntryComment.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"AdminRemoveEntryComment/{aEntryCommentModelId}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at AdminRemoveEntryComment in EntryClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
        #endregion
    }
}
