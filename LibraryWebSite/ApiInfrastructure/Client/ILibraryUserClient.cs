
namespace LibraryWebSite.ApiInfrastructure.Client
{
    using System.Threading.Tasks;
    using Common.Models;
    using Common.Models.Api;
    using LibraryWebSite.ApiInfrastructure.Responses;

    public interface ILibraryUserClient
    {
        Task<LibraryUserResponse> GetLibraryUserByLibraryUserCode(string id);

        Task<LibraryUserPagedListResponse> GetLibraryUsersPaged(PagedBase filterParameters);

        Task<APIItemsResponse> GetLibraryUsers(string search);

        Task<IntResponse> Insert(LibraryUserApiModel apiModel);

        Task<BoolResponse> Update(LibraryUserApiModel apiModel);

        Task<BoolResponse> Delete(string id);

    }
}
