using Microsoft.Skype.Bots.Media;
using System.Net.WebSockets;
using System.Runtime.InteropServices;

namespace TeamsMediaBot.Media;

public class AudioMediaHandler : IDisposable
{
    private readonly IAudioSocket _audioSocket;
    private readonly ClientWebSocket _webSocket = new();
    private readonly Uri _pythonWsUri;

    public AudioMediaHandler(IAudioSocket audioSocket, string pythonWsUrl)
    {
        _audioSocket = audioSocket;
        _pythonWsUri = new Uri(pythonWsUrl);
        _audioSocket.AudioMediaReceived += OnAudioMediaReceived;
    }

    public async Task ConnectToPythonPipelineAsync()
    {
        try
        {
            await _webSocket.ConnectAsync(_pythonWsUri, CancellationToken.None);
            Console.WriteLine("[Media] Connected to Python Audio Service.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Media] Python pipeline not ready: {ex.Message}");
        }
    }

    private async void OnAudioMediaReceived(object? sender, AudioMediaReceivedEventArgs e)
    {
        try
        {
            var buffer = e.Buffer;
            if (_webSocket.State == WebSocketState.Open && buffer.Data != IntPtr.Zero && buffer.Length > 0)
            {
                byte[] managedArray = new byte[buffer.Length];
                Marshal.Copy(buffer.Data, managedArray, 0, (int)buffer.Length);

                // Stream raw 16kHz uncompressed PCM audio chunk directly to Python
                await _webSocket.SendAsync(
                    new ArraySegment<byte>(managedArray),
                    WebSocketMessageType.Binary,
                    true,
                    CancellationToken.None);
            }
        }
        finally
        {
            e.Buffer.Dispose();
        }
    }

    public void Dispose()
    {
        _audioSocket.AudioMediaReceived -= OnAudioMediaReceived;
        _webSocket.Dispose();
    }
}