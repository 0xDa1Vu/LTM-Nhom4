using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test6
{
    public partial class Form1 : Form
    {
        int cell = 40;
        int startX = 20;
        int startY = 20;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 2);

            g.DrawRectangle(pen, startX, startY, 8 * cell, 9 * cell);

            for (int i = 1; i < 9; i++)
            {
                g.DrawLine(pen,
                    startX,
                    startY + i * cell,
                    startX + 8 * cell,
                    startY + i * cell);
            }

            for (int j = 0; j < 9; j++)
            {
                if (j == 0 || j == 8)
                {
                    g.DrawLine(pen,
                        startX + j * cell,
                        startY,
                        startX + j * cell,
                        startY + 9 * cell);
                }
                else
                {
                    g.DrawLine(pen,
                        startX + j * cell,
                        startY,
                        startX + j * cell,
                        startY + 4 * cell);

                    g.DrawLine(pen,
                        startX + j * cell,
                        startY + 5 * cell,
                        startX + j * cell,
                        startY + 9 * cell);

                }
            }
            g.DrawLine(pen, startX + 3 * cell, startY, startX + 5 * cell, startY + 2 * cell);
            g.DrawLine(pen, startX + 5 * cell, startY, startX + 3 * cell, startY + 2 * cell);

            g.DrawLine(pen, startX + 3 * cell, startY + 7 * cell, startX + 5 * cell, startY + 9 * cell);
            g.DrawLine(pen, startX + 5 * cell, startY + 7 * cell, startX + 3 * cell, startY + 9 * cell);

            DrawMark(g, pen, startX, startY, cell, 1, 2);
            DrawMark(g, pen, startX, startY, cell, 7, 2);
            DrawMark(g, pen, startX, startY, cell, 0, 3);
            DrawMark(g, pen, startX, startY, cell, 2, 3);
            DrawMark(g, pen, startX, startY, cell, 4, 3);
            DrawMark(g, pen, startX, startY, cell, 6, 3);
            DrawMark(g, pen, startX, startY, cell, 8, 3);

            DrawMark(g, pen, startX, startY, cell, 1, 7);
            DrawMark(g, pen, startX, startY, cell, 7, 7);
            DrawMark(g, pen, startX, startY, cell, 0, 6);
            DrawMark(g, pen, startX, startY, cell, 2, 6);
            DrawMark(g, pen, startX, startY, cell, 4, 6);
            DrawMark(g, pen, startX, startY, cell, 6, 6);
            DrawMark(g, pen, startX, startY, cell, 8, 6);

            Font font = new Font("Arial", 20, FontStyle.Bold);
            g.DrawString("楚河", font, Brushes.Black, startX + 1.3f * cell, startY + 4.2f * cell);
            g.DrawString("漢界", font, Brushes.Black, startX + 5f * cell, startY + 4.2f * cell);
        }
        void DrawMark(Graphics g, Pen pen, int startX, int startY, int cell, int col, int row)
        {
            int x = startX + col * cell;
            int y = startY + row * cell;

            int size = cell / 8;
            int offset = cell / 10;

            int maxCol = 8;
            int maxRow = 9;

            if (col > 0 && row > 0)
            {
                g.DrawLine(pen, x - offset, y - offset, x - offset - size, y - offset);
                g.DrawLine(pen, x - offset, y - offset, x - offset, y - offset - size);
            }

            if (col < maxCol && row > 0)
            {
                g.DrawLine(pen, x + offset, y - offset, x + offset + size, y - offset);
                g.DrawLine(pen, x + offset, y - offset, x + offset, y - offset - size);
            }

            if (col > 0 && row < maxRow)
            {
                g.DrawLine(pen, x - offset, y + offset, x - offset - size, y + offset);
                g.DrawLine(pen, x - offset, y + offset, x - offset, y + offset + size);
            }

            if (col < maxCol && row < maxRow)
            {
                g.DrawLine(pen, x + offset, y + offset, x + offset + size, y + offset);
                g.DrawLine(pen, x + offset, y + offset, x + offset, y + offset + size);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Button btnThachDau = new Button();
            btnThachDau.Text = "Thách Đấu";
            btnThachDau.Size = new Size(150, 50);
            btnThachDau.Location = new Point(350, 260);
            btnThachDau.Font = new Font("Arial", 12, FontStyle.Bold);
            btnThachDau.BackColor = Color.LightGray;
            btnThachDau.FlatStyle = FlatStyle.Flat;
            btnThachDau.Click += BtnThachDau_Click;
            this.Controls.Add(btnThachDau);

            Button btnThoat = new Button();
            btnThoat.Text = "Thoát";
            btnThoat.Size = new Size(150, 50);
            btnThoat.Location = new Point(350, 320);
            btnThoat.Font = new Font("Arial", 12, FontStyle.Bold);
            btnThoat.BackColor = Color.LightGray;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.Click += BtnThoat_Click;
            this.Controls.Add(btnThoat);
        }
        private void BtnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnThachDau_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bắt đầu thách đấu!");
        }
    }
}
