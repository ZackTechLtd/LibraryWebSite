using System;
namespace LibraryWebSite.ApiInfrastructure.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Models;
    using Common.Models.Api;
    using LibraryWebSite.ApiInfrastructure.Responses;
    using LibraryWebSite.Util;
    using Microsoft.Extensions.Options;

    public class LibraryBookStatusClient : ClientBase, ILibraryBookStatusClient
    {
        public LibraryBookStatusClient(IOptions<AppConfiguration> appConfiguration, IApiClient apiClient) : base(appConfiguration, apiClient)
        {
            _apiUrl = appConfiguration.Value.ApiUrl + "api/librarybookstatus/";
        }

        public async Task<LibraryBookStatusResponse> GetLibraryBookStatusByLibraryBookStatusCode(string id)
        {
            return await GetJsonDecodedContent<LibraryBookStatusResponse, LibraryBookStatusApiModel>(_apiUrl + APIConstants.Id + id).ConfigureAwait(false);
        }

        public async Task<LibraryBookStatusPagedListResponse> GetLibraryBookStatusPaged(PagedBase filterParameters)
        {
            List<KeyValuePair<string, string>> kvpList = KeyValuePairUtil.KeyValuePairFromPagedBase(filterParameters);
            return await GetJsonDecodedContent<LibraryBookStatusPagedListResponse, LibraryBookStatusPageApiModel>(_apiUrl + APIConstants.Paged, kvpList.ToArray()).ConfigureAwait(false);
        }

        public async Task<IntResponse> Insert(LibraryBookStatusApiModel apiModel)
        {
            var createBranchResponse = await PostEncodedContentWithSimpleResponse<IntResponse, LibraryBookStatusApiModel>(_apiUrl /*+ APIConstants.Insert*/, apiModel);
            return createBranchResponse;
        }

        public async Task<BoolResponse> Update(LibraryBookStatusApiModel apiModel)
        {
            var createBranchResponse = await PutEncodedContentWithSimpleBoolResponse<BoolResponse, LibraryBookStatusApiModel>(_apiUrl /*+ APIConstants.Update*/, apiModel);
            return createBranchResponse;
        }

        public async Task<BoolResponse> Delete(string id)
        {
            return await DeleteContent<BoolResponse>(_apiUrl /*+ APIConstants.Delete*/ + "?=" + id);
        }
    }
}
