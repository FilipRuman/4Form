namespace ForForm.Tcp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
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
        private static readonly string[] possibleRustBleHandlerAppFilePaths =
        {
            "./bluetooth_handler.exe",
            "./bluetooth_handler",
            "./bluetooth_handler.x86_64",
            "./../subomdules/4Form-BluetoothHandler/target/debug/bluetooth_handler.exe",
            "./../subomdules/4Form-BluetoothHandler/target/debug/bluetooth_handler",
            "./../subomdules/4Form-BluetoothHandler/target/debug/bluetooth_handler.x86_64",
        };
        public Process rustProcess;

        public override void _Ready() {
            var working_path = "";
            foreach (string path in possibleRustBleHandlerAppFilePaths) {
                if (File.Exists(path)) {
                    working_path = path;
                    break;
                }
            }

            if (working_path == "") {
                GD.PrintErr(
                    $"Bluetooth LE handling program was not found. Please compile it directly, or download it from release page https://github.com/FilipRuman/4Form/releases \n and place it in one of Following Directories: \n {ProjectSettings.GlobalizePath("./")} \n {ProjectSettings.GlobalizePath("./../subomdules/4Form-BluetoothHandler/target/debug/")} "
                );
            }
            GD.Print($"working file path to rust Ble handler:{working_path}");

            rustProcess = new Process();
            rustProcess.StartInfo.FileName = working_path;
            rustProcess.StartInfo.UseShellExecute = false;
            // for debugging
            rustProcess.StartInfo.CreateNoWindow = false;

            rustProcess.Start();
            tcpParser.Setup();

            base._Ready();
        }

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
                ReadDataAsync();
            }

            base._Process(delta);
        }

        private async Task ReadDataAsync() {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            string data_raw = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            var splittedData = data_raw.Split('\n');
            foreach (string data in splittedData) {
                tcpParser.ParseTcpDataString(data);
            }
        }

        public async Task SendDataAsync(String stringToSend) {
            stringToSend += '\n';
            var bytesToSend = Encoding.UTF8.GetBytes(stringToSend);
            await stream.WriteAsync(bytesToSend);
        }

        private void CreateTcpConnection() {
            client = new TcpClient("127.0.0.1", 2137);
            stream = client.GetStream();
        }
    }
}
