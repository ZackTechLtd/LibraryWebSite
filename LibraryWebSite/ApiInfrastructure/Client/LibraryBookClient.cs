

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

    public class LibraryBookClient : ClientBase, ILibraryBookClient
    {
        public LibraryBookClient(IOptions<AppConfiguration> appConfiguration, IApiClient apiClient) : base(appConfiguration, apiClient)
        {
            _apiUrl = appConfiguration.Value.ApiUrl + "api/librarybook/";
        }

        public async Task<LibraryBookResponse> GetLibraryBookByLibraryBookCode(string id)
        {
            return await GetJsonDecodedContent<LibraryBookResponse, LibraryBookApiModel>(_apiUrl + APIConstants.Id + id).ConfigureAwait(false);
        }

        public async Task<LibraryBookPagedListResponse> GetLibraryBooksPaged(PagedBase filterParameters, bool listLostAndStolen)
        {
            List<KeyValuePair<string, string>> kvpList = KeyValuePairUtil.KeyValuePairFromPagedBase(filterParameters);
            kvpList.Add(new KeyValuePair<string, string>("listLostAndStolen", listLostAndStolen.ToString()));
            return await GetJsonDecodedContent<LibraryBookPagedListResponse, LibraryBookPageApiModel>(_apiUrl + APIConstants.Paged, kvpList.ToArray()).ConfigureAwait(false);
        }

        public async Task<APIItemsResponse> GetBookList(string search)
        {
            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("search", search)
            };

            return await GetJsonDecodedContent<APIItemsResponse, ApiItemCollectionApiModel>(_apiUrl + APIConstants.List, kvpList.ToArray()).ConfigureAwait(false);
        }

        public async Task<IntResponse> Insert(LibraryBookApiModel apiModel)
        {
            var createBranchResponse = await PostEncodedContentWithSimpleResponse<IntResponse, LibraryBookApiModel>(_apiUrl /*+ APIConstants.Insert*/, apiModel);
            return createBranchResponse;
        }

        public async Task<BoolResponse> Update(LibraryBookApiModel apiModel)
        {
            var createBranchResponse = await PutEncodedContentWithSimpleBoolResponse<BoolResponse, LibraryBookApiModel>(_apiUrl /*+ APIConstants.Update*/, apiModel);
            return createBranchResponse;
        }

        public async Task<BoolResponse> Delete(string id)
        {
            return await DeleteContent<BoolResponse>(_apiUrl /*+ APIConstants.Delete*/ + "?id=" + id);
        }
    }
}
