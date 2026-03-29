using System;
using System.Text;
using CoTuongOnline.Network;
using CoTuongOnline.Client.Network;

namespace CoTuongOnline.Client
{
    /// <summary>
    /// Lắng nghe và xử lý sự kiện từ Server
    /// Kết nối giữa SocketClient (mạng) và Form1 (giao diện)
    /// </summary>
    public class ClientEventListener
    {
        private readonly SocketClient _socket;

        // Events để Form1 lắng nghe
        public event Action? OnWaiting;
        public event Action<bool>? OnGameStart;       // true = đỏ, false = đen
        public event Action<int, int, int, int>? OnMoveReceived;  // fc, fr, tc, tr
        public event Action<string>? OnChatReceived;
        public event Action? OnOpponentLeft;
        public event Action? OnGameEnd;

        public ClientEventListener(SocketClient socket)
        {
            _socket = socket;

            // Đăng ký lắng nghe dữ liệu từ SocketClient
            _socket.DataReceived += HandleData;
            _socket.ConnectionError += HandleError;
        }

        /// <summary>
        /// Xử lý dữ liệu thô nhận được từ Server
        /// </summary>
        private void HandleData(byte[] data)
        {
            // Thử parse theo Protocol (gói tin có header)
            var result = PacketParser.Parse(data);

            if (result.IsValid)
            {
                HandlePacket(result);
            }
            else
            {
                // Fallback: xử lý dạng text đơn giản (dùng khi test với NetworkTest)
                HandleTextMessage(Encoding.UTF8.GetString(data));
            }
        }

        /// <summary>
        /// Xử lý gói tin đã parse
        /// </summary>
        private void HandlePacket(PacketParser.ParseResult result)
        {
            switch (result.Type)
            {
                case PacketType.Move:
                    var (fc, fr, tc, tr) = PacketParser.ParseMove(result.Data);
                    if (fc != -1)
                        OnMoveReceived?.Invoke(fc, fr, tc, tr);
                    break;

                case PacketType.Chat:
                    OnChatReceived?.Invoke(result.Data);
                    break;

                case PacketType.GameStart:
                    OnGameStart?.Invoke(result.Data == "RED");
                    break;

                case PacketType.GameEnd:
                    OnGameEnd?.Invoke();
                    break;

                default:
                    Console.WriteLine($"[LISTENER] Gói tin không xử lý: {result.Type}");
                    break;
            }
        }

        /// <summary>
        /// Xử lý text message đơn giản (dùng khi test thủ công)
        /// Định dạng: "LOẠI|dữ liệu"
        /// </summary>
        private void HandleTextMessage(string message)
        {
            Console.WriteLine($"[LISTENER] Text: {message}");

            if (message.StartsWith("WAITING"))
                OnWaiting?.Invoke();

            else if (message.StartsWith("START|RED"))
                OnGameStart?.Invoke(true);

            else if (message.StartsWith("START|BLACK"))
                OnGameStart?.Invoke(false);

            else if (message.StartsWith("OPPONENT_LEFT"))
                OnOpponentLeft?.Invoke();

            else if (message.StartsWith("MOVE|"))
            {
                var data = message.Substring(5);
                var (fc, fr, tc, tr) = PacketParser.ParseMove(data);
                if (fc != -1)
                    OnMoveReceived?.Invoke(fc, fr, tc, tr);
            }

            else if (message.StartsWith("CHAT|"))
                OnChatReceived?.Invoke(message.Substring(5));
        }

        /// <summary>
        /// Xử lý lỗi kết nối
        /// </summary>
        private void HandleError(Exception ex)
        {
            Console.WriteLine($"[LISTENER] Lỗi kết nối: {ex.Message}");
            OnOpponentLeft?.Invoke();
        }
    }
}