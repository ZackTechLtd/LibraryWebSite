using System;
using System.Collections.Generic;
using Common.Models.Api;
using LibraryWebSite.ApiInfrastructure.Responses;

namespace LibraryWebSite.ApiInfrastructure.Client
{
    public class BoolResponse : ApiResponse<bool>
    {
    }

    public class IntResponse : ApiResponse<int>
    {
    }

    public class StringResponse : ApiResponse<string>
    {
    }

    public class ApiStringResponse : ApiResponse<Apistring>
    {
    }

    public class ListStringResponse : ApiResponse<IEnumerable<string>>
    {
    }
}
