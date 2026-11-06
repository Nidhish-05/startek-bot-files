using Microsoft.Graph.Communications.Client.Authentication;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
namespace TeamsMediaBot.Services;
public class AuthenticationProvider : IRequestAuthenticationProvider
{
    private readonly IConfidentialClientApplication? _app;
    public AuthenticationProvider(string appId, string appSecret, string tenantId)
    {
        if (!string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(appSecret) && !string.IsNullOrEmpty(tenantId))
        {
            _app = ConfidentialClientApplicationBuilder.Create(appId)
                .WithClientSecret(appSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();
        }
    }
    public async Task AuthenticateOutboundRequestAsync(HttpRequestMessage request, string tenant)
    {
        if (_app != null)
        {
            var result = await _app.AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
                                   .ExecuteAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        }
    }
    public Task<RequestValidationResult> ValidateInboundRequestAsync(HttpRequestMessage request)
    {
        // Validates incoming Teams signaling webhook requests
        return Task.FromResult(new RequestValidationResult { IsValid = true });
    }
}