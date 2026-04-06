using System; 

namespace CoTuongOnline.Common 
<<<<<<< HEAD
{
    public class Board
=======
    public class Board 
>>>>>>> origin/main
    {
        // Khai báo mảng 2 chiều lưu bàn cờ
        // 10 hàng (Row) và 9 cột (Col) = 90 ô
        public Piece[,] grid = new Piece[10, 9];

        // Hàm khởi tạo bàn cờ với vị trí quân cờ ban đầu
        public void Init()
        {
            // ===== QUÂN ĐEN =====

            // Xe đen
            grid[0, 0] = new Piece(PieceType.Rook, false, 0, 0);
            grid[0, 8] = new Piece(PieceType.Rook, false, 0, 8);

            // Mã đen
            grid[0, 1] = new Piece(PieceType.Horse, false, 0, 1);
            grid[0, 7] = new Piece(PieceType.Horse, false, 0, 7);

            // Tượng đen
            grid[0, 2] = new Piece(PieceType.Elephant, false, 0, 2);
            grid[0, 6] = new Piece(PieceType.Elephant, false, 0, 6);

            // Sĩ đen
            grid[0, 3] = new Piece(PieceType.Advisor, false, 0, 3);
            grid[0, 5] = new Piece(PieceType.Advisor, false, 0, 5);

            // Tướng đen
            grid[0, 4] = new Piece(PieceType.General, false, 0, 4);

            // Pháo đen
            grid[2, 1] = new Piece(PieceType.Cannon, false, 2, 1);
            grid[2, 7] = new Piece(PieceType.Cannon, false, 2, 7);

            // Tốt đen (5 quân)
            // i += 2 để đặt ở các cột 0,2,4,6,8
            for (int i = 0; i < 9; i += 2)
                grid[3, i] = new Piece(PieceType.Soldier, false, 3, i);

            // ===== QUÂN ĐỎ =====

            // Xe đỏ
            grid[9, 0] = new Piece(PieceType.Rook, true, 9, 0);
            grid[9, 8] = new Piece(PieceType.Rook, true, 9, 8);

            // Mã đỏ
            grid[9, 1] = new Piece(PieceType.Horse, true, 9, 1);
            grid[9, 7] = new Piece(PieceType.Horse, true, 9, 7);

            // Tượng đỏ
            grid[9, 2] = new Piece(PieceType.Elephant, true, 9, 2);
            grid[9, 6] = new Piece(PieceType.Elephant, true, 9, 6);

            // Sĩ đỏ
            grid[9, 3] = new Piece(PieceType.Advisor, true, 9, 3);
            grid[9, 5] = new Piece(PieceType.Advisor, true, 9, 5);

            // Tướng đỏ
            grid[9, 4] = new Piece(PieceType.General, true, 9, 4);

            // Pháo đỏ
            grid[7, 1] = new Piece(PieceType.Cannon, true, 7, 1);
            grid[7, 7] = new Piece(PieceType.Cannon, true, 7, 7);

            // Tốt đỏ (5 quân)
            for (int i = 0; i < 9; i += 2)
                grid[6, i] = new Piece(PieceType.Soldier, true, 6, i);
        }

        // Hàm in bàn cờ ra console (dùng để test)
        public void PrintBoard()
        {
            for (int i = 0; i < 10; i++) // duyệt từng hàng
            {
                for (int j = 0; j < 9; j++) // duyệt từng cột
                {
                    // Nếu ô trống -> in dấu .
                    // Nếu có quân -> in X
                    Console.Write(grid[i, j] == null ? " . " : " X ");
                }
                Console.WriteLine(); // xuống dòng sau mỗi hàng
            }
        }

        // Hàm di chuyển quân cờ đơn giản (chưa kiểm tra luật cờ)
        public void Move(int fromRow, int fromCol, int toRow, int toCol)
        {
            // Kiểm tra nếu ô nguồn không có quân
            if (grid[fromRow, fromCol] == null)
            {
                Console.WriteLine("Không có quân!");
                return;
            }

            // Di chuyển quân cờ sang vị trí mới
            grid[toRow, toCol] = grid[fromRow, fromCol];

            // Xóa quân ở vị trí cũ
            grid[fromRow, fromCol] = null;
        }
    }
}