using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CoTuongOnline.Common;
using CoTuongOnline.Logic;
using CoTuongOnline.Final;


namespace CoTuongOnline.Client
{
    public partial class Form1 : Form
    {
        // Cấu hình bàn cờ 
        private const int Cell = 60;
        private const int StartX = 30;
        private const int StartY = 30;

        //  Trạng thái game
        private Board _board = new Board();
        private Point? _selectedCell = null;          // ô đang chọn
        private List<Point> _validMoves = new List<Point>(); // nước đi hợp lệ  
        private bool _isRedTurn = true;          // true = lượt đỏ

        // Đếm thời gian
        private GameTimer _gameTimer = new GameTimer();
        private Label lblTimerRed;   // hiển thị đếm ngược quân đỏ
        private Label lblTimerBlack; // hiển thị đếm ngược quân đen

        //─ UI 
        private TextBox txtStatus = null!;
        private readonly Dictionary<string, Image> _pieces = new Dictionary<string, Image>();

        // Map loại quân → key ảnh (dùng để vẽ từ Board.grid) 
        private static readonly Dictionary<PieceType, string> _pieceKeyMap = new()
        {
            { PieceType.General,  "vua"   },
            { PieceType.Advisor,  "si"    },
            { PieceType.Elephant, "tuong" },
            { PieceType.Horse,    "ma"    },
            { PieceType.Rook,     "xe"    },
            { PieceType.Cannon,   "phao"  },
            { PieceType.Soldier,  "tot"   },
        };

        //  CONSTRUCTOR
        public Form1()
        {
            InitializeComponent();

            this.ClientSize = new Size(StartX * 2 + 8 * Cell + 200, StartY * 2 + 9 * Cell);
            this.Text = "Cờ Tướng Online";
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.Paint += Form1_Paint;
            this.Load += Form1_Load;
            this.MouseClick += Form1_MouseClick;

            LoadImages();
            _board.Init(); // khởi tạo vị trí quân cờ ban đầu
        }

        //  VẼ BÀN CỜ + QUÂN (đọc từ _board.grid)
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //  Nền bàn cờ màu gỗ 
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 222, 173)),
                            StartX, StartY, 8 * Cell, 9 * Cell);

            Pen pen = new Pen(Color.Black, 2);

            // Viền ngoài 
            g.DrawRectangle(pen, StartX, StartY, 8 * Cell, 9 * Cell);

            // Đường ngang
            for (int i = 1; i < 9; i++)
                g.DrawLine(pen, StartX, StartY + i * Cell, StartX + 8 * Cell, StartY + i * Cell);

            // Đường dọc (ngắt ở sông hàng 4-5)
            for (int j = 0; j <= 8; j++)
            {
                if (j == 0 || j == 8)
                    g.DrawLine(pen, StartX + j * Cell, StartY, StartX + j * Cell, StartY + 9 * Cell);
                else
                {
                    g.DrawLine(pen, StartX + j * Cell, StartY, StartX + j * Cell, StartY + 4 * Cell);
                    g.DrawLine(pen, StartX + j * Cell, StartY + 5 * Cell, StartX + j * Cell, StartY + 9 * Cell);
                }
            }

            // Cung điện (chéo X) 
            g.DrawLine(pen, StartX + 3 * Cell, StartY, StartX + 5 * Cell, StartY + 2 * Cell);
            g.DrawLine(pen, StartX + 5 * Cell, StartY, StartX + 3 * Cell, StartY + 2 * Cell);
            g.DrawLine(pen, StartX + 3 * Cell, StartY + 7 * Cell, StartX + 5 * Cell, StartY + 9 * Cell);
            g.DrawLine(pen, StartX + 5 * Cell, StartY + 7 * Cell, StartX + 3 * Cell, StartY + 9 * Cell);

            // Dấu chấm vị trí Pháo và Tốt
            int[][] marks = {
                new[]{1,2}, new[]{7,2},
                new[]{0,3}, new[]{2,3}, new[]{4,3}, new[]{6,3}, new[]{8,3},
                new[]{1,7}, new[]{7,7},
                new[]{0,6}, new[]{2,6}, new[]{4,6}, new[]{6,6}, new[]{8,6}
            };
            foreach (var m in marks) DrawMark(g, pen, m[0], m[1]);

            // Chữ sông
            Font fontRiver = new Font("SimSun", 24, FontStyle.Bold);
            g.DrawString("楚河", fontRiver, Brushes.DarkRed, StartX + 1.4f * Cell, StartY + 4.15f * Cell);
            g.DrawString("漢界", fontRiver, Brushes.DarkRed, StartX + 4.8f * Cell, StartY + 4.15f * Cell);

            // Highlight ô đang chọn (vàng)
            if (_selectedCell.HasValue)
            {
                int hx = StartX + _selectedCell.Value.X * Cell - Cell / 2;
                int hy = StartY + _selectedCell.Value.Y * Cell - Cell / 2;
                g.FillRectangle(new SolidBrush(Color.FromArgb(120, 255, 215, 0)), hx, hy, Cell, Cell);
            }

            // Chấm xanh = nước đi hợp lệ
            foreach (var p in _validMoves)
            {
                int cx = StartX + p.X * Cell;
                int cy = StartY + p.Y * Cell;
                g.FillEllipse(new SolidBrush(Color.FromArgb(160, 0, 180, 0)), cx - 10, cy - 10, 20, 20);
            }

            // Vẽ quân từ _board.grid (KHÔNG hardcode nữa)
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Piece? p = _board.grid[row, col];
                    if (p == null) continue;

                    string color = p.IsRed ? "do" : "den";
                    string base_ = _pieceKeyMap[p.Type];
                    string key = $"{base_}_{color}";

                    DrawPiece(g, key, col, row);
                }
            }
        }

        //  XỬ LÝ CLICK CHUỘT
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Tính ô được click
            int col = (e.X - StartX + Cell / 2) / Cell;
            int row = (e.Y - StartY + Cell / 2) / Cell;

            // Bỏ qua nếu click ngoài bàn cờ
            if (col < 0 || col > 8 || row < 0 || row > 9) return;

            Piece? clickedPiece = _board.grid[row, col];

            // BƯỚC 1: Chưa chọn quân nào
            if (_selectedCell == null)
            {
                if (clickedPiece == null) return;           // ô trống
                if (clickedPiece.IsRed != _isRedTurn) return; // không phải lượt mình

                _selectedCell = new Point(col, row);
                _validMoves = GetValidMovesFor(row, col);

                SoundManager.PlayMove();
                this.Invalidate();
                return;
            }

            // BƯỚC 2: Đã chọn quân, xét click tiếp theo
            int fromCol = _selectedCell.Value.X;
            int fromRow = _selectedCell.Value.Y;

            // Click lại chính ô đó → bỏ chọn
            if (fromRow == row && fromCol == col)
            {
                _selectedCell = null;
                _validMoves.Clear();
                this.Invalidate();
                return;
            }

            // Click vào quân cùng màu khác → đổi sang chọn quân đó
            if (clickedPiece != null && clickedPiece.IsRed == _isRedTurn)
            {
                _selectedCell = new Point(col, row);
                _validMoves = GetValidMovesFor(row, col);
                this.Invalidate();
                return;
            }

            // BƯỚC 3: Thử di chuyển
            bool moved = FinalLogic.TryMove(_board, fromRow, fromCol, row, col, _isRedTurn);

            if (moved)
            {
                // Âm thanh: ăn quân hoặc đi thường
                if (clickedPiece != null)
                    SoundManager.PlayCapture();
                else
                    SoundManager.PlayMove();

                // Đổi lượt
                _isRedTurn = !_isRedTurn;

                // Reset đồng hồ cho lượt mới
                _gameTimer.StartTurn();

                // Cập nhật lại hiển thị đồng hồ
                lblTimerRed.Text = "⏱ Đỏ: 30s";
                lblTimerBlack.Text = "⏱ Đen: 30s";
                _selectedCell = null;
                _validMoves.Clear();

                // Cập nhật label trạng thái
                txtStatus.Text = _isRedTurn ? "Đến Lượt Bạn" : "Đang Chờ Đối Thủ...";
                txtStatus.ForeColor = _isRedTurn ? Color.DarkGreen : Color.Gray;

                // Kiểm tra chiếu bí → kết thúc game
                if (ChessRules.IsCheckmate(_board, _isRedTurn))
                {
                    // Bên vừa đi (_isRedTurn đã đổi) → bên kia thua
                    ShowGameResult(!_isRedTurn);
                }
            }
            else
            {
                // Nước đi không hợp lệ → bỏ chọn
                _selectedCell = null;
                _validMoves.Clear();
            }

            this.Invalidate();
        }

        //  TÍNH NƯỚC ĐI HỢP LỆ (dùng board clone để không ảnh hưởng thật)
        private List<Point> GetValidMovesFor(int row, int col)
        {
            var moves = new List<Point>();
            Piece? piece = _board.grid[row, col];
            if (piece == null) return moves;

            for (int r = 0; r < 10; r++)
                for (int c = 0; c < 9; c++)
                    if (FinalLogic.TryMove(CloneBoard(_board), row, col, r, c, piece.IsRed))
                        moves.Add(new Point(c, r));

            return moves;
        }

        // Clone board để thử nước đi mà không thay đổi board thật
        private Board CloneBoard(Board src)
        {
            var clone = new Board();
            for (int r = 0; r < 10; r++)
                for (int c = 0; c < 9; c++)
                {
                    var p = src.grid[r, c];
                    if (p != null)
                        clone.grid[r, c] = new Piece(p.Type, p.IsRed, p.Row, p.Col);
                }
            return clone;
        }

        //  VẼ 1 QUÂN CỜ – căn giữa tâm giao điểm (col, row)
        private void DrawPiece(Graphics g, string key, int col, int row)
        {
            if (!_pieces.ContainsKey(key)) return;

            int size = Cell - 6;
            int cx = StartX + col * Cell;
            int cy = StartY + row * Cell;

            g.DrawImage(_pieces[key], cx - size / 2, cy - size / 2, size, size);
        }

        //  DẤU GÓC (mark) tại giao điểm
        private void DrawMark(Graphics g, Pen pen, int col, int row)
        {
            int x = StartX + col * Cell;
            int y = StartY + row * Cell;
            int arm = Cell / 6;
            int offset = Cell / 8;

            if (col > 0 && row > 0)
            { g.DrawLine(pen, x - offset, y - offset, x - offset - arm, y - offset); g.DrawLine(pen, x - offset, y - offset, x - offset, y - offset - arm); }
            if (col < 8 && row > 0)
            { g.DrawLine(pen, x + offset, y - offset, x + offset + arm, y - offset); g.DrawLine(pen, x + offset, y - offset, x + offset, y - offset - arm); }
            if (col > 0 && row < 9)
            { g.DrawLine(pen, x - offset, y + offset, x - offset - arm, y + offset); g.DrawLine(pen, x - offset, y + offset, x - offset, y + offset + arm); }
            if (col < 8 && row < 9)
            { g.DrawLine(pen, x + offset, y + offset, x + offset + arm, y + offset); g.DrawLine(pen, x + offset, y + offset, x + offset, y + offset + arm); }
        }

        //  NẠP ẢNH
        private void LoadImages()
        {
            string basePath = Path.GetFullPath(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..",
                "assets", "images"
            ));

            var files = new Dictionary<string, string>
            {
                // Quân đỏ
                { "vua_do",    "Vuado.png"    },
                { "tuong_do",  "Tuongdo1.png" },
                { "xe_do",     "Xedo1.png"    },
                { "ma_do",     "Mado1.png"    },
                { "phao_do",   "Phaodo1.png"  },
                { "si_do",     "Sido1.png"    },
                { "tot_do",    "Totdo1.png"   },
                // Quân đen
                { "vua_den",   "Vuaden.png"    },
                { "tuong_den", "Tuongden1.png" },
                { "xe_den",    "Xeden1.png"    },
                { "ma_den",    "Maden1.png"    },
                { "phao_den",  "Phaoden1.png"  },
                { "si_den",    "Siden1.png"    },
                { "tot_den",   "Totden1.png"   },
            };

            foreach (var item in files)
            {
                string fullPath = Path.Combine(basePath, item.Value);
                if (File.Exists(fullPath))
                    _pieces[item.Key] = Image.FromFile(fullPath);
                else
                    Console.WriteLine($"Không tìm thấy ảnh: {fullPath}");
            }
        }

        //  LOAD FORM – tạo UI controls
        private void Form1_Load(object sender, EventArgs e)
        {
            int btnX = StartX * 2 + 8 * Cell + 10;
            int formH = StartY * 2 + 9 * Cell;

            // Label trạng thái lượt
            txtStatus = new TextBox
            {
                Text = "Đến Lượt Bạn",
                Location = new Point(btnX, 25),
                Size = new Size(150, 50),
                Font = new Font("Arial", 13, FontStyle.Bold),
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Center,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = Color.DarkGreen
            };
            txtStatus.TabStop = false;
            this.Controls.Add(txtStatus);

            // Nút Thách Đấu
            Button btnThachDau = new Button
            {
                Text = "Thách Đấu",
                Size = new Size(150, 50),
                Location = new Point(btnX, formH / 2 - 60),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnThachDau.Click += (s, ev) => MessageBox.Show("Bắt đầu thách đấu!");
            this.Controls.Add(btnThachDau);

            // Nút Chơi Lại
            Button btnReplay = new Button
            {
                Text = "Chơi Lại",
                Size = new Size(150, 50),
                Location = new Point(btnX, formH / 2),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            btnReplay.Click += (s, ev) => ResetGame();
            this.Controls.Add(btnReplay);

            // Nút Thoát
            Button btnThoat = new Button
            {
                Text = "Thoát",
                Size = new Size(150, 50),
                Location = new Point(btnX, formH / 2 + 60),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnThoat.Click += (s, ev) => Application.Exit();
            this.Controls.Add(btnThoat);

            int btnX = StartX * 2 + 8 * Cell + 10;

            // Label đồng hồ quân ĐỎ
            lblTimerRed = new Label
            {
                Text = "⏱ Đỏ: 30s",
                Location = new Point(btnX, formH / 2 + 80),
                Size = new Size(150, 30),
                Font = new Font("Arial", 13, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTimerRed);

            // Label đồng hồ quân ĐEN
            lblTimerBlack = new Label
            {
                Text = "⏱ Đen: 30s",
                Location = new Point(btnX, formH / 2 + 120),
                Size = new Size(150, 30),
                Font = new Font("Arial", 13, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTimerBlack);

            // Đăng ký event đếm ngược
            _gameTimer.CountdownChanged += (seconds) =>
            {
                // Phải invoke về UI thread vì timer chạy trên thread khác
                this.Invoke((Action)(() =>
                {
                    if (_isRedTurn)
                        lblTimerRed.Text = $"⏱ Đỏ: {seconds}s";
                    else
                        lblTimerBlack.Text = $"⏱ Đen: {seconds}s";
                }));
            };

            // Khi hết giờ → thua
            _gameTimer.CountdownEnd += () =>
            {
                this.Invoke((Action)(() =>
                {
                    MessageBox.Show(_isRedTurn ? "Đỏ hết giờ! Đen thắng!" : "Đen hết giờ! Đỏ thắng!");
                    ShowGameResult(!_isRedTurn); // bên còn lại thắng
                }));
            };

            // Bắt đầu game
            _gameTimer.StartGame();

            // Tạo ChatBox và đặt bên phải bàn cờ
            var chatBox = new ChatBox
            {
                Location = new Point(btnX, 170)
            };

            // Xử lý tin nhắn thường
            chatBox.OnChatMessage += (msg) =>
            {
                // TODO tuần 3: gửi qua network
                // _listener.SendChatAsync(msg);
                Console.WriteLine($"[CHAT] {msg}");
            };

            // Xử lý lệnh
            chatBox.OnCommand += (cmd, arg) =>
            {
                switch (cmd)
                {
                    case "exit":
                        Application.Exit();
                        break;
                    case "surrender":
                        ShowGameResult(false); // mình thua
                        break;
                }
            };

            this.Controls.Add(chatBox);
        }

    //  HIỂN THỊ KẾT QUẢ
    private void ShowGameResult(bool isWin)
        {
            FormResult f = new FormResult(isWin);
            if (f.ShowDialog() == DialogResult.OK && f.IsReplay)
                ResetGame();
        }

        //  RESET GAME
        private void ResetGame()
        {
            _board = new Board();
            _board.Init();
            _selectedCell = null;
            _validMoves.Clear();
            _isRedTurn = true;
            _gameTimer.StopAll();
            _gameTimer.StartGame();
            lblTimerRed.Text = "⏱ Đỏ: 30s";
            lblTimerBlack.Text = "⏱ Đen: 30s";


    txtStatus.Text = "Đến Lượt Bạn";
            txtStatus.ForeColor = Color.DarkGreen;

            this.Invalidate();
        }
    }
}