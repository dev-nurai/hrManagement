using System.Net.Http.Headers;

namespace HR.LeaveManagement.BlazorUI
{
    public class HeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            if (request.RequestUri?.AbsolutePath.Contains("/authenicate") is not true)
            {
                
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }

}
