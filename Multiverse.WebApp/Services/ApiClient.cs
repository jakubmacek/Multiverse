namespace Multiverse.WebApp.Services
{
    public partial class ApiClient
    {
        public IAuthorizationService AuthorizationService { get; set; }

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
        {
            if (AuthorizationService != null)
            {
                var token = AuthorizationService.Token;
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                AuthorizationService.Token = string.Empty;
        }
    }
}
