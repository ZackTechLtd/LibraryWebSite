using System;
namespace LibraryWebSite.ApiInfrastructure.Responses
{
    using System.Collections.Generic;

    public class ErrorStateResponse
    {
        public string Message { get; set; }
        public IDictionary<string, string[]> ModelState { get; set; }
    }

    public class ErrorResponceObject
    {
        public List<string> Error { get; set; }
    }
}
