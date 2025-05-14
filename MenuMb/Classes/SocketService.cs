using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MenuMb.Classes
{
    internal class SocketService
    {
        private readonly ClientWebSocket _client = new();

        public async Task StartListeningAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
           
            await _client.ConnectAsync(new Uri(ConnectionServerSetings.WebSocketIp), CancellationToken.None);

            var buffer = new byte[256];
            while (_client.State == WebSocketState.Open)
            {
                var result = await _client.ReceiveAsync(buffer, CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                StatusUpdater.UpdateStatusBar(message);
            }
        }

        public async Task CloseAsync()
        {
            if (_client.State == WebSocketState.Open)
            {
                await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Завершение работы", CancellationToken.None);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}

