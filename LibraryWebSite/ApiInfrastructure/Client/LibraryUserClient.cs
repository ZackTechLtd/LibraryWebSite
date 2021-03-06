﻿using System;
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

    public class LibraryUserClient : ClientBase, ILibraryUserClient
    {
        public LibraryUserClient(IOptions<AppConfiguration> appConfiguration, IApiClient apiClient) : base(appConfiguration, apiClient)
        {
            _apiUrl = appConfiguration.Value.ApiUrl + "api/libraryUser/";
        }

        public async Task<LibraryUserResponse> GetLibraryUserByLibraryUserCode(string id)
        {
            return await GetJsonDecodedContent<LibraryUserResponse, LibraryUserApiModel>(_apiUrl + APIConstants.Id + id).ConfigureAwait(false);
        }

        public async Task<LibraryUserPagedListResponse> GetLibraryUsersPaged(PagedBase filterParameters)
        {
            List<KeyValuePair<string, string>> kvpList = KeyValuePairUtil.KeyValuePairFromPagedBase(filterParameters);
            return await GetJsonDecodedContent<LibraryUserPagedListResponse, LibraryUserPageApiModel>(_apiUrl + APIConstants.Paged, kvpList.ToArray()).ConfigureAwait(false);
        }

        public async Task<APIItemsResponse> GetLibraryUsers(string search)
        {
            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("search", search)
            };
            
            return await GetJsonDecodedContent<APIItemsResponse, ApiItemCollectionApiModel>(_apiUrl + APIConstants.List, kvpList.ToArray()).ConfigureAwait(false);
        }

        public async Task<IntResponse> Insert(LibraryUserApiModel apiModel)
        {
            var createBranchResponse = await PostEncodedContentWithSimpleResponse<IntResponse, LibraryUserApiModel>(_apiUrl /*+ APIConstants.Insert*/, apiModel);
            return createBranchResponse;
        }

        public async Task<BoolResponse> Update(LibraryUserApiModel apiModel)
        {
            var createBranchResponse = await PutEncodedContentWithSimpleBoolResponse<BoolResponse, LibraryUserApiModel>(_apiUrl /*+ APIConstants.Update*/, apiModel);
            return createBranchResponse;
        }

        public async Task<BoolResponse> Delete(string id)
        {
            return await DeleteContent<BoolResponse>(_apiUrl /*+ APIConstants.Delete*/ + "?id=" + id);
        }
    }
}
