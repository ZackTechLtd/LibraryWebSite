using System;
namespace LibraryWebSite.ApiInfrastructure.Client
{
    using System.Threading.Tasks;
    using Common.Models;
    using Common.Models.Api;
    using LibraryWebSite.ApiInfrastructure.Responses;

    public interface ILibraryBookStatusClient
    {
        Task<LibraryBookStatusResponse> GetLibraryBookStatusByLibraryBookStatusCode(string id);

        Task<LibraryBookStatusPagedListResponse> GetLibraryBookStatusPaged(PagedBase filterParameters);

        Task<IntResponse> Insert(LibraryBookStatusApiModel apiModel);

        Task<BoolResponse> Update(LibraryBookStatusApiModel apiModel);

        Task<BoolResponse> Delete(string id);

    }
}
