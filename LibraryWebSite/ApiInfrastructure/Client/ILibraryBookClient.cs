
namespace LibraryWebSite.ApiInfrastructure.Client
{
    using System.Threading.Tasks;
    using Common.Models;
    using Common.Models.Api;
    using LibraryWebSite.ApiInfrastructure.Responses;

    public interface ILibraryBookClient
    {
        Task<LibraryBookResponse> GetLibraryBookByLibraryBookCode(string id);

        Task<LibraryBookPagedListResponse> GetLibraryBooksPaged(PagedBase filterParameters, bool listLostAndStolen);

        Task<APIItemsResponse> GetBookList(string search);

        Task<IntResponse> Insert(LibraryBookApiModel apiModel);

        Task<BoolResponse> Update(LibraryBookApiModel apiModel);

        Task<BoolResponse> Delete(string id);

    }
}
