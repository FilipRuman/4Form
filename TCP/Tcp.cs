namespace ForForm.Tcp
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Godot;

    public partial class Tcp : Node {
        [Export]
        TcpParser tcpParser;

        TcpClient client;
        NetworkStream stream;

        private Task readDataTask;

        float tcpConnectionCreationTimer = 0;
        const float TcpConnectionTimeout = .5f; //S

        public override void _Process(double delta) {
            if (stream == null) {
                tcpConnectionCreationTimer += (float)delta;
                if (tcpConnectionCreationTimer > TcpConnectionTimeout) {
                    tcpConnectionCreationTimer = 0;
                    CreateTcpConnection();
                }

                return;
            }
            if (
                readDataTask == null
                || readDataTask.Status == TaskStatus.RanToCompletion
                || readDataTask.Status == TaskStatus.Faulted
            ) {
                readDataTask?.Dispose();
                readDataTask = ReadDataAsync();
            }
            base._Process(delta);
        }

        private async Task ReadDataAsync() {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            // GD.Print("Received: " + message);
            tcpParser.ParseTcpDataString(data);
        }

        public async Task SendDataAsync(String stringToSend) {
            var bytesToSend = Encoding.UTF8.GetBytes(stringToSend);
            await stream.WriteAsync(bytesToSend);
        }

        private void CreateTcpConnection() {
            client = new TcpClient("127.0.0.1", 2137);
            stream = client.GetStream();
        }
    }
}
