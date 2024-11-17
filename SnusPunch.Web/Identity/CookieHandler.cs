using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace SnusPunch.Web.Identity
{
    /// Handler to ensure cookie credentials are automatically sent over with each request.
    public class CookieHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage aHttpRequestMessage, CancellationToken aCancellationToken)
        {
            // include cookies!
            aHttpRequestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            aHttpRequestMessage.Headers.Add("X-Requested-With", ["XMLHttpRequest"]);

            return base.SendAsync(aHttpRequestMessage, aCancellationToken);
        }
    }
}
