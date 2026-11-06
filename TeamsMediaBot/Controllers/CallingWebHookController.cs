using Microsoft.AspNetCore.Mvc;
namespace TeamsMediaBot.Controllers;
[ApiController]
[Route("api/calling")]
public class CallingWebhookController : ControllerBase
{
    [HttpPost("callback")]
    public IActionResult OnIncomingNotification()
    {
        // Endpoint where Microsoft Teams posts call events (ringing, joined, media ready)
        Console.WriteLine("[Signaling] Received call notification from Teams.");
        return Ok();
    }
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { status = "Signaling endpoint is reachable" });
}