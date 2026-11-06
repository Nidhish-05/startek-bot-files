namespace TeamsMediaBot.Configuration;
public class BotConfiguration
{
    public string AppId { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string BotBaseUrl { get; set; } = "https://g3k2lvfb-5000.aue.devtunnels.ms";
    public int SignalingPort { get; set; } = 5000;
    public string PythonAiServiceWebSocketUrl { get; set; } = "ws://10.0.2.2:8001/ws/audio";
}