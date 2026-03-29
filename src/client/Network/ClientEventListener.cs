using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoTuongOnline.Network;

namespace CoTuongOnline.Client
{
    /// <summary>
    /// Lắng nghe dữ liệu từ SocketClient, dùng PacketParser để giải mã,
    /// rồi fire các event tương ứng để Form1 cập nhật bàn cờ.
    /// </summary>
    public class ClientEventListener : IDisposable
    {
        private readonly SocketClient _socket;
        private readonly PacketParser _parser;
        private readonly Control _uiControl; // để Invoke về UI thread
        private bool _disposed;

        // ===== EVENTS cho UI đăng ký =====

        /// <summary>Game bắt đầu. bool = true nếu mình là quân Đỏ (đi trước).</summary>
        public event Action<bool>? OnGameStart;

        /// <summary>Nhận nước đi từ đối thủ: (fromRow, fromCol, toRow, toCol).</summary>
        public event Action<int, int, int, int>? OnOpponentMove;

        /// <summary>Server xác nhận nước đi của mình hợp lệ.</summary>
        public event Action? OnMoveAccepted;

        /// <summary>Server từ chối nước đi (di chuyển không hợp lệ).</summary>
        public event Action? OnMoveRejected;

        /// <summary>Nhận tin nhắn chat từ đối thủ.</summary>
        public event Action<string>? OnChatReceived;

        /// <summary>Game kết thúc. string = lý do (WIN/LOSE/DRAW).</summary>
        public event Action<string>? OnGameEnd;

        /// <summary>Đối thủ đầu hàng.</summary>
        public event Action? OnOpponentSurrender;

        /// <summary>Đối thủ sẵn sàng.</summary>
        public event Action? OnOpponentReady;

        /// <summary>Login thành công.</summary>
        public event Action? OnLoginSuccess;

        /// <summary>Login thất bại. string = lý do.</summary>
        public event Action<string>? OnLoginFailed;

        /// <summary>Lỗi kết nối.</summary>
        public event Action<string>? OnConnectionError;

        /// <param name="socket">SocketClient đã kết nối.</param>
        /// <param name="uiControl">Control (thường là Form1) để marshal event về UI thread.</param>
        public ClientEventListener(SocketClient socket, Control uiControl)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _uiControl = uiControl ?? throw new ArgumentNullException(nameof(uiControl));
            _parser = new PacketParser();

            // Đăng ký nhận data thô từ SocketClient
            _socket.DataReceived += OnDataReceived;
            _socket.ConnectionError += OnSocketError;

            // Đăng ký nhận packet đã parse
            _parser.OnPacketParsed += HandlePacket;
        }

        /// <summary>
        /// Kết nối tới server và bắt đầu lắng nghe.
        /// </summary>
        public async Task ConnectAndListenAsync(string serverIp, int port, CancellationToken ct = default)
        {
            await _socket.ConnectAsync(serverIp, port, ct);
            _socket.StartReceiveLoop(ct);
        }

        /// <summary>
        /// Gửi nước đi lên server.
        /// </summary>
        public async Task SendMoveAsync(int fromRow, int fromCol, int toRow, int toCol, CancellationToken ct = default)
        {
            byte[] packet = Protocol.CreateMove(fromRow, fromCol, toRow, toCol);
            await _socket.SendAsync(packet, ct);
        }

        /// <summary>
        /// Gửi tin nhắn chat.
        /// </summary>
        public async Task SendChatAsync(string message, CancellationToken ct = default)
        {
            byte[] packet = Protocol.CreateChat(message);
            await _socket.SendAsync(packet, ct);
        }

        /// <summary>
        /// Gửi trạng thái sẵn sàng.
        /// </summary>
        public async Task SendReadyAsync(CancellationToken ct = default)
        {
            byte[] packet = Protocol.CreateReady();
            await _socket.SendAsync(packet, ct);
        }

        /// <summary>
        /// Gửi đầu hàng.
        /// </summary>
        public async Task SendSurrenderAsync(CancellationToken ct = default)
        {
            byte[] packet = Protocol.CreateSurrender();
            await _socket.SendAsync(packet, ct);
        }

        // ===== XỬ LÝ DỮ LIỆU =====

        private void OnDataReceived(byte[] chunk)
        {
            _parser.ReceiveData(chunk);
        }

        private void OnSocketError(Exception ex)
        {
            InvokeOnUI(() => OnConnectionError?.Invoke(ex.Message));
        }

        /// <summary>
        /// Xử lý từng packet đã được PacketParser giải mã.
        /// Dispatch sang event tương ứng, marshal về UI thread.
        /// </summary>
        private void HandlePacket(PacketType type, string data)
        {
            switch (type)
            {
                case PacketType.GameStart:
                    if (PacketParser.ParseGameStart(data, out bool isRed))
                        InvokeOnUI(() => OnGameStart?.Invoke(isRed));
                    break;

                case PacketType.Move:
                    var move = PacketParser.ParseMove(data);
                    if (move.HasValue)
                    {
                        var m = move.Value;
                        InvokeOnUI(() => OnOpponentMove?.Invoke(m.fromRow, m.fromCol, m.toRow, m.toCol));
                    }
                    break;

                case PacketType.MoveOK:
                    InvokeOnUI(() => OnMoveAccepted?.Invoke());
                    break;

                case PacketType.MoveFail:
                    InvokeOnUI(() => OnMoveRejected?.Invoke());
                    break;

                case PacketType.Chat:
                    InvokeOnUI(() => OnChatReceived?.Invoke(data));
                    break;

                case PacketType.GameEnd:
                    InvokeOnUI(() => OnGameEnd?.Invoke(data));
                    break;

                case PacketType.SurrenderOK:
                    InvokeOnUI(() => OnOpponentSurrender?.Invoke());
                    break;

                case PacketType.ReadyOK:
                    InvokeOnUI(() => OnOpponentReady?.Invoke());
                    break;

                case PacketType.LoginOK:
                    InvokeOnUI(() => OnLoginSuccess?.Invoke());
                    break;

                case PacketType.LoginFail:
                    InvokeOnUI(() => OnLoginFailed?.Invoke(data));
                    break;

                case PacketType.Heartbeat:
                    // Không cần xử lý, chỉ giữ kết nối
                    break;
            }
        }

        /// <summary>
        /// Marshal callback về UI thread (WinForms).
        /// </summary>
        private void InvokeOnUI(Action action)
        {
            if (_uiControl.IsDisposed) return;

            if (_uiControl.InvokeRequired)
                _uiControl.BeginInvoke(action);
            else
                action();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            _socket.DataReceived -= OnDataReceived;
            _socket.ConnectionError -= OnSocketError;
            _parser.OnPacketParsed -= HandlePacket;
            _parser.Reset();
        }
    }
}
