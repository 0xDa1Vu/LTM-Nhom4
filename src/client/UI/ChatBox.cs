using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoTuongOnline.Client
{
    public class ChatBox : Panel
    {
        private RichTextBox _log;    // hiển thị lịch sử chat
        private TextBox _input;      // ô nhập tin
        private Button _btnSend;

        // Event bắn ra khi có tin nhắn thường
        public event Action<string>? OnChatMessage;

        // Event bắn ra khi có lệnh, VD: ("exit", "") hoặc ("surrender", "")
        public event Action<string, string>? OnCommand;

        public ChatBox()
        {
            this.Size = new Size(170, 250);
            this.BorderStyle = BorderStyle.FixedSingle;

            // Vùng log chat
            _log = new RichTextBox
            {
                Location = new Point(0, 0),
                Size = new Size(170, 190),
                ReadOnly = true,
                BackColor = Color.WhiteSmoke,
                Font = new Font("Arial", 9),
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            // Ô nhập
            _input = new TextBox
            {
                Location = new Point(0, 195),
                Size = new Size(120, 25),
                Font = new Font("Arial", 9),
                PlaceholderText = "Nhắn tin hoặc /lệnh..."
            };

            // Nút gửi
            _btnSend = new Button
            {
                Text = "Gửi",
                Location = new Point(122, 193),
                Size = new Size(46, 27),
                Font = new Font("Arial", 8)
            };

            _btnSend.Click += (s, e) => Send();
            _input.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { Send(); e.SuppressKeyPress = true; }
            };

            this.Controls.Add(_log);
            this.Controls.Add(_input);
            this.Controls.Add(_btnSend);
        }

        private void Send()
        {
            string text = _input.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;
            _input.Clear();

            // ── Phân biệt lệnh và chat thường ──────────────────
            if (text.StartsWith("/"))
            {
                // Tách lệnh: "/surrender" → command="surrender", arg=""
                //            "/kick player1" → command="kick", arg="player1"
                string[] parts = text.Substring(1).Split(' ', 2);
                string command = parts[0].ToLower();
                string arg = parts.Length > 1 ? parts[1] : "";

                HandleCommand(command, arg);
            }
            else
            {
                // Tin nhắn bình thường
                AppendLog($"[Bạn]: {text}", Color.DarkBlue);
                OnChatMessage?.Invoke(text);
            }
        }

        private void HandleCommand(string command, string arg)
        {
            switch (command)
            {
                case "exit":
                    AppendLog("[Lệnh] Thoát game...", Color.Gray);
                    OnCommand?.Invoke("exit", arg);
                    break;

                case "surrender":
                    AppendLog("[Lệnh] Đầu hàng!", Color.OrangeRed);
                    OnCommand?.Invoke("surrender", arg);
                    break;

                case "help":
                    AppendLog("Lệnh có sẵn:", Color.DarkGreen);
                    AppendLog("  /exit       - Thoát", Color.DarkGreen);
                    AppendLog("  /surrender  - Đầu hàng", Color.DarkGreen);
                    AppendLog("  /help       - Xem lệnh", Color.DarkGreen);
                    break;

                default:
                    AppendLog($"[?] Lệnh /{command} không tồn tại. Gõ /help.", Color.Red);
                    break;
            }
        }

        // Hiển thị tin nhắn từ đối thủ
        public void AppendOpponentMessage(string message)
        {
            AppendLog($"[Đối thủ]: {message}", Color.DarkRed);
        }

        // Hàm tiện ích thêm dòng vào log
        public void AppendLog(string text, Color color)
        {
            _log.SelectionStart = _log.TextLength;
            _log.SelectionLength = 0;
            _log.SelectionColor = color;
            _log.AppendText(text + "\n");
            _log.ScrollToCaret();
        }
    }
}