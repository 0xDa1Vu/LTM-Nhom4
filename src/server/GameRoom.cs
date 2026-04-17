using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using CoTuongOnline.Network;

namespace ChessServer
{
    /// <summary>
    /// 1 phòng chơi = 2 người chơi
    /// Nhận nước đi từ người này → gửi sang người kia
    /// </summary>
    public class GameRoom
    {
        private readonly TcpClient _player1;
        private readonly TcpClient _player2;
        private readonly string _roomId;
        private bool _gameRunning = true;

        public GameRoom(TcpClient player1, TcpClient player2)
        {
            _player1 = player1;
            _player2 = player2;
            _roomId = Guid.NewGuid().ToString()[..8];
        }

        public async Task StartAsync()
        {
            Logger.WriteLog($"[ROOM {_roomId}] Ván đấu bắt đầu!");

            // Thông báo vai trò cho 2 người chơi (dùng Protocol binary format)
            SendBytes(_player1, Protocol.CreateGameStart("RED"));
            SendBytes(_player2, Protocol.CreateGameStart("BLACK"));

            // Chạy 2 luồng lắng nghe song song
            Task t1 = Task.Run(() => ListenFrom(_player1, _player2, "Đỏ"));
            Task t2 = Task.Run(() => ListenFrom(_player2, _player1, "Đen"));

            await Task.WhenAll(t1, t2);

            Logger.WriteLog($"[ROOM {_roomId}] Ván đấu kết thúc.");
        }

        /// <summary>
        /// Lắng nghe từ sender → chuyển tiếp raw bytes sang receiver
        /// (giữ nguyên binary protocol, không convert sang string)
        /// </summary>
        private void ListenFrom(TcpClient sender, TcpClient receiver, string role)
        {
            byte[] buffer = new byte[4096];

            try
            {
                NetworkStream stream = sender.GetStream();

                while (_gameRunning)
                {
                    int byteRead = stream.Read(buffer, 0, buffer.Length);
                    if (byteRead == 0) break;

                    Logger.WriteLog($"[ROOM {_roomId}] {role}: nhận {byteRead} bytes");

                    // Chuyển tiếp raw bytes sang đối thủ (giữ nguyên binary protocol)
                    try
                    {
                        receiver.GetStream().Write(buffer, 0, byteRead);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog($"[ROOM {_roomId}] Lỗi gửi tới đối thủ: {ex.Message}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"[ROOM {_roomId}] {role} mất kết nối: {ex.Message}");
            }
            finally
            {
                _gameRunning = false;

                // Báo người còn lại biết đối thủ đã thoát (dùng Protocol binary format)
                SendBytes(receiver, Protocol.CreateGameEnd("Đối thủ đã thoát khỏi phòng."));

                sender.Close();
                Logger.WriteLog($"[ROOM {_roomId}] {role} đã ngắt kết nối.");
            }
        }

        private static void SendBytes(TcpClient client, byte[] data)
        {
            try
            {
                client.GetStream().Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"[ROOM] Lỗi gửi: {ex.Message}");
            }
        }
    }
}