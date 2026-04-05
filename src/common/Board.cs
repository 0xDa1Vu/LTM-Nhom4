using System;

namespace CoTuongOnline.Common
{
    public class Board
    {
        // Tạo bàn cờ 2 chiều:
        // 10 hàng (0→9), 9 cột (0→8) = 90 ô
        // Mỗi ô chứa 1 quân cờ (Piece) hoặc null (ô trống)
        public Piece[,] grid = new Piece[10, 9];

        // Hàm khởi tạo trạng thái ban đầu của bàn cờ
        public void Init()
        {
            // ===== QUÂN ĐEN (ở phía trên) =====

            // Xe (Rook)
            grid[0, 0] = new Piece(PieceType.Rook, false, 0, 0);
            grid[0, 8] = new Piece(PieceType.Rook, false, 0, 8);

            // Mã (Horse)
            grid[0, 1] = new Piece(PieceType.Horse, false, 0, 1);
            grid[0, 7] = new Piece(PieceType.Horse, false, 0, 7);

            // Tượng (Elephant)
            grid[0, 2] = new Piece(PieceType.Elephant, false, 0, 2);
            grid[0, 6] = new Piece(PieceType.Elephant, false, 0, 6);

            // Sĩ (Advisor)
            grid[0, 3] = new Piece(PieceType.Advisor, false, 0, 3);
            grid[0, 5] = new Piece(PieceType.Advisor, false, 0, 5);

            // Tướng (General)
            grid[0, 4] = new Piece(PieceType.General, false, 0, 4);

            // Pháo (Cannon)
            grid[2, 1] = new Piece(PieceType.Cannon, false, 2, 1);
            grid[2, 7] = new Piece(PieceType.Cannon, false, 2, 7);

            // Tốt (Soldier) – đặt ở hàng 3, cách 1 ô đặt 1 quân
            for (int i = 0; i < 9; i += 2)
                grid[3, i] = new Piece(PieceType.Soldier, false, 3, i);

            // ===== QUÂN ĐỎ (ở phía dưới) =====

            // Xe
            grid[9, 0] = new Piece(PieceType.Rook, true, 9, 0);
            grid[9, 8] = new Piece(PieceType.Rook, true, 9, 8);

            // Mã
            grid[9, 1] = new Piece(PieceType.Horse, true, 9, 1);
            grid[9, 7] = new Piece(PieceType.Horse, true, 9, 7);

            // Tượng
            grid[9, 2] = new Piece(PieceType.Elephant, true, 9, 2);
            grid[9, 6] = new Piece(PieceType.Elephant, true, 9, 6);

            // Sĩ
            grid[9, 3] = new Piece(PieceType.Advisor, true, 9, 3);
            grid[9, 5] = new Piece(PieceType.Advisor, true, 9, 5);

            // Tướng
            grid[9, 4] = new Piece(PieceType.General, true, 9, 4);

            // Pháo
            grid[7, 1] = new Piece(PieceType.Cannon, true, 7, 1);
            grid[7, 7] = new Piece(PieceType.Cannon, true, 7, 7);

            // Tốt
            for (int i = 0; i < 9; i += 2)
                grid[6, i] = new Piece(PieceType.Soldier, true, 6, i);
        }

        // Hàm in bàn cờ ra console (chỉ để test)
        public void PrintBoard()
        {
            // Duyệt từng hàng
            for (int i = 0; i < 10; i++)
            {
                // Duyệt từng cột
                for (int j = 0; j < 9; j++)
                {
                    // Nếu ô trống → in "."
                    // Nếu có quân → in "X"
                    Console.Write(grid[i, j] == null ? " . " : " X ");
                }
                // Xuống dòng sau mỗi hàng
                Console.WriteLine();
            }
        }

        // Hàm di chuyển quân (đơn giản, CHƯA kiểm tra luật)
        public void Move(int fromRow, int fromCol, int toRow, int toCol)
        {
            // Kiểm tra ô xuất phát có quân không
            if (grid[fromRow, fromCol] == null)
            {
                Console.WriteLine("Không có quân!");
                return; // dừng nếu không có quân
            }

            // Di chuyển quân:
            // Gán quân từ vị trí cũ sang vị trí mới
            grid[toRow, toCol] = grid[fromRow, fromCol];

            // Xóa vị trí cũ (trở thành ô trống)
            grid[fromRow, fromCol] = null;
        }
    }
}