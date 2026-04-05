using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CoTuongOnline.Client
{
    public partial class Form1 : Form
    {
        private TextBox txtStatus;
        private Point? selectedCell = null; // ô đang chọn
        private List<Point> validMoves = new List<Point>(); // ô có thể đi
        private bool myTurn = true; // lượt chơi (test trước)
        //  Cấu hình bàn cờ 
        private const int Cell = 60;  // kích thước 1 ô (px)
        private const int StartX = 30;  // lề trái
        private const int StartY = 30;  // lề trên
        // Bàn cờ: 9 cột (0-8) × 10 hàng (0-9)

        private readonly Dictionary<string, Image> _pieces = new Dictionary<string, Image>();

        private bool HasPieceAt(int col, int row)
        {
            // Quân đen
            if (row == 0 && (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 5 || col == 6 || col == 7 || col == 8))
                return true;

            if (row == 2 && (col == 1 || col == 7)) return true;

            if (row == 3 && (col % 2 == 0)) return true;

            // Quân đỏ
            if (row == 9 && (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 5 || col == 6 || col == 7 || col == 8))
                return true;

            if (row == 7 && (col == 1 || col == 7)) return true;

            if (row == 6 && (col % 2 == 0)) return true;

            return false;
        }

        public Form1()
        {
            InitializeComponent();

            this.MouseClick += Form1_MouseClick;

            this.ClientSize = new Size(StartX * 2 + 8 * Cell + 200,
                                            StartY * 2 + 9 * Cell);
            this.Text = "Cờ Tướng Online";
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.Paint += Form1_Paint;
            this.Load += Form1_Load;

            LoadImages();
        }

        //  VẼ BÀN CỜ + QUÂN
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền bàn cờ màu gỗ
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 222, 173)),
                            StartX, StartY, 8 * Cell, 9 * Cell);

            Pen pen = new Pen(Color.Black, 2);

            // Viền ngoài
            g.DrawRectangle(pen, StartX, StartY, 8 * Cell, 9 * Cell);

            // Đường ngang (1-8)
            for (int i = 1; i < 9; i++)
                g.DrawLine(pen,
                    StartX, StartY + i * Cell,
                    StartX + 8 * Cell, StartY + i * Cell);

            // Đường dọc: cột 0 & 8 đi thẳng; cột 1-7 bị ngắt ở sông
            for (int j = 0; j <= 8; j++)
            {
                if (j == 0 || j == 8)
                {
                    g.DrawLine(pen,
                        StartX + j * Cell, StartY,
                        StartX + j * Cell, StartY + 9 * Cell);
                }
                else
                {
                    g.DrawLine(pen,
                        StartX + j * Cell, StartY,
                        StartX + j * Cell, StartY + 4 * Cell);
                    g.DrawLine(pen,
                        StartX + j * Cell, StartY + 5 * Cell,
                        StartX + j * Cell, StartY + 9 * Cell);
                }
            }

            foreach (var p in validMoves)
            {
                int cx = StartX + p.X * Cell;
                int cy = StartY + p.Y * Cell;
                int dotSize = 20;
                g.FillEllipse(new SolidBrush(Color.Green),
                    cx - dotSize / 2,
                    cy - dotSize / 2,
                    dotSize, dotSize);
            }

            // Cung điện trên (hàng 0-2) và dưới (hàng 7-9)
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
            foreach (var m in marks)
                DrawMark(g, pen, m[0], m[1]);

            // Chữ sông
            Font fontRiver = new Font("SimSun", 24, FontStyle.Bold);
            g.DrawString("楚河", fontRiver, Brushes.DarkRed,
                         StartX + 1.4f * Cell, StartY + 4.15f * Cell);
            g.DrawString("漢界", fontRiver, Brushes.DarkRed,
                         StartX + 5.3f * Cell, StartY + 4.15f * Cell);

            //  Quân ĐEN (hàng 0-3, phía trên) 
            DrawPiece(g, "vua_den", 4, 0);
            DrawPiece(g, "si_den", 3, 0);
            DrawPiece(g, "si_den", 5, 0);
            DrawPiece(g, "tuong_den", 2, 0);
            DrawPiece(g, "tuong_den", 6, 0);
            DrawPiece(g, "xe_den", 0, 0);
            DrawPiece(g, "xe_den", 8, 0);
            DrawPiece(g, "ma_den", 1, 0);
            DrawPiece(g, "ma_den", 7, 0);
            DrawPiece(g, "phao_den", 1, 2);
            DrawPiece(g, "phao_den", 7, 2);
            DrawPiece(g, "tot_den", 0, 3);
            DrawPiece(g, "tot_den", 2, 3);
            DrawPiece(g, "tot_den", 4, 3);
            DrawPiece(g, "tot_den", 6, 3);
            DrawPiece(g, "tot_den", 8, 3);

            //  Quân ĐỎ (hàng 6-9, phía dưới)
            DrawPiece(g, "vua_do", 4, 9);
            DrawPiece(g, "si_do", 3, 9);
            DrawPiece(g, "si_do", 5, 9);
            DrawPiece(g, "tuong_do", 2, 9);
            DrawPiece(g, "tuong_do", 6, 9);
            DrawPiece(g, "xe_do", 0, 9);
            DrawPiece(g, "xe_do", 8, 9);
            DrawPiece(g, "ma_do", 1, 9);
            DrawPiece(g, "ma_do", 7, 9);
            DrawPiece(g, "phao_do", 1, 7);
            DrawPiece(g, "phao_do", 7, 7);
            DrawPiece(g, "tot_do", 0, 6);
            DrawPiece(g, "tot_do", 2, 6);
            DrawPiece(g, "tot_do", 4, 6);
            DrawPiece(g, "tot_do", 6, 6);
            DrawPiece(g, "tot_do", 8, 6);
        }

        //  VẼ 1 QUÂN CỜ – căn đúng tâm giao điểm (col, row)
        private void DrawPiece(Graphics g, string key, int col, int row)
        {
            if (!_pieces.ContainsKey(key)) return;

            int size = Cell - 6;
            int cx = StartX + col * Cell;
            int cy = StartY + row * Cell;

            g.DrawImage(_pieces[key], cx - size / 2, cy - size / 2, size, size);
        }

        //  DẤU GÓC (mark)
        private void DrawMark(Graphics g, Pen pen, int col, int row)
        {
            int x = StartX + col * Cell;
            int y = StartY + row * Cell;
            int arm = Cell / 6;
            int offset = Cell / 8;

            if (col > 0 && row > 0)
            {
                g.DrawLine(pen, x - offset, y - offset, x - offset - arm, y - offset);
                g.DrawLine(pen, x - offset, y - offset, x - offset, y - offset - arm);
            }
            if (col < 8 && row > 0)
            {
                g.DrawLine(pen, x + offset, y - offset, x + offset + arm, y - offset);
                g.DrawLine(pen, x + offset, y - offset, x + offset, y - offset - arm);
            }
            if (col > 0 && row < 9)
            {
                g.DrawLine(pen, x - offset, y + offset, x - offset - arm, y + offset);
                g.DrawLine(pen, x - offset, y + offset, x - offset, y + offset + arm);
            }
            if (col < 8 && row < 9)
            {
                g.DrawLine(pen, x + offset, y + offset, x + offset + arm, y + offset);
                g.DrawLine(pen, x + offset, y + offset, x + offset, y + offset + arm);
            }
        }

        //  NẠP ẢNH – khớp đúng tên file trong assets/images/
        private void LoadImages()
        {
            string basePath = Path.GetFullPath(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..",
                "assets", "images"
            ));

            var files = new Dictionary<string, string>
            {
                //  Quân đỏ
                { "vua_do",    "Vuado.png"    },  // Tướng đỏ
                { "tuong_do",  "Tuongdo1.png" },  // Tượng đỏ
                { "xe_do",     "Xedo1.png"    },
                { "ma_do",     "Mado1.png"    },
                { "phao_do",   "Phaodo1.png"  },
                { "si_do",     "Sido1.png"    },
                { "tot_do",    "Totdo1.png"   },
                //  Quân đen 
                { "vua_den",   "Vuaden.png"    },  // Tướng đen
                { "tuong_den", "Tuongden1.png" },  // Tượng đen
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

        //  LOAD FORM – nút Thách Đấu & Thoát
        private void Form1_Load(object sender, EventArgs e)
        {
            int btnX = StartX * 2 + 8 * Cell + 10;
            int formH = StartY * 2 + 9 * Cell;

            txtStatus = new TextBox
            {
                Text = "Đến Lượt Bạn",
                Location = new Point(StartX * 2 + 8 * Cell + 10, 25),
                Size = new Size(150, 100),
                Font = new Font("Arial", 15, FontStyle.Bold),

                ReadOnly = true,          // 👈 không cho nhập
                TextAlign = HorizontalAlignment.Center,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            txtStatus.TabStop = false;
            this.Controls.Add(txtStatus);

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

            Button btnThoat = new Button
            {
                Text = "Thoát",
                Size = new Size(150, 50),
                Location = new Point(btnX, formH / 2 + 10),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnThoat.Click += (s, ev) => Application.Exit();
            this.Controls.Add(btnThoat);

        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)

        {
            ShowGameResult(true);

            int col = (e.X - StartX + Cell / 2) / Cell;
            int row = (e.Y - StartY + Cell / 2) / Cell;

            if (col < 0 || col > 8 || row < 0 || row > 9) return;

            // 👇 BƯỚC 2 NẰM Ở ĐÂY
            if (!HasPieceAt(col, row))
            {
                SoundManager.PlayMove();

                selectedCell = null;
                validMoves.Clear();
                this.Invalidate();
                return;

            }
            {
                SoundManager.PlayCapture();
            
            }
            void AddMoveIfValid(int c, int r)
            {
                if (c >= 0 && c <= 8 && r >= 0 && r <= 9)
                    validMoves.Add(new Point(c, r));
            }

            // dùng:
            validMoves.Clear();

            AddMoveIfValid(col + 1, row);
            AddMoveIfValid(col - 1, row);
            AddMoveIfValid(col, row + 1);
            AddMoveIfValid(col, row - 1);

            myTurn = !myTurn;

            txtStatus.Text = myTurn ? "Đến Lượt Bạn" : "Đang Chờ Đối Thủ...";
            txtStatus.ForeColor = myTurn ? Color.DarkGreen : Color.Gray;

            this.Invalidate();
        }
        private void ShowGameResult(bool isWin)
        {
            MessageBox.Show("ĐÃ VÀO HÀM"); // 👈 test

            FormResult f = new FormResult(isWin);
            f.ShowDialog();
        }

        private void ResetGame()
        {
            selectedCell = null;
            validMoves.Clear();
            myTurn = true;

            txtStatus.Text = "Đến Lượt Bạn";
            txtStatus.ForeColor = Color.DarkGreen;

            this.Invalidate();
        }
    }
}