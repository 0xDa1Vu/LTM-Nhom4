using System;
using CoTuongOnline.Common;

namespace CoTuongOnline.Logic
{
    public static class ChessRules
    {
        // ================= KIỂM TRA CHIẾU =================
        public static bool IsCheck(Board board, bool isRed)
        {
            // Tìm tướng của bên đang xét
            Piece general = FindGeneral(board, isRed);
            if (general == null) return false;

            // Duyệt toàn bộ bàn cờ
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var p = board.grid[i, j];

                    // Nếu là quân đối phương
                    if (p != null && p.IsRed != isRed)
                    {
                        // Nếu nó ăn được tướng → bị chiếu
                        if (MoveValidator.IsValidMove(board, p, general.Row, general.Col))
                            return true;
                    }
                }
            }

            return false;
        }

        // ================= KIỂM TRA CHIẾU BÍ =================
        public static bool IsCheckmate(Board board, bool isRed)
        {
            // Nếu chưa bị chiếu → không phải chiếu bí
            if (!IsCheck(board, isRed))
                return false;

            // Duyệt tất cả quân của mình
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var p = board.grid[i, j];
                    if (p == null || p.IsRed != isRed) continue;

                    // Thử đi tới mọi ô
                    for (int r = 0; r < 10; r++)
                    {
                        for (int c = 0; c < 9; c++)
                        {
                            // Nếu nước đi không hợp lệ → bỏ
                            if (!MoveValidator.IsValidMove(board, p, r, c))
                                continue;

                            // ===== GIẢ LẬP NƯỚC ĐI =====
                            var temp = board.grid[r, c]; // lưu quân bị ăn
                            int oldR = p.Row, oldC = p.Col;

                            // Thực hiện nước đi
                            board.grid[r, c] = p;
                            board.grid[oldR, oldC] = null;
                            p.Row = r; 
                            p.Col = c;

                            // Kiểm tra còn bị chiếu không
                            bool stillCheck = IsCheck(board, isRed);

                            // ===== HOÀN TÁC (rollback) =====
                            board.grid[oldR, oldC] = p;
                            board.grid[r, c] = temp;
                            p.Row = oldR; 
                            p.Col = oldC;

                            // Nếu có ít nhất 1 cách thoát → không phải chiếu bí
                            if (!stillCheck)
                                return false;
                        }
                    }
                }
            }

            // Không còn cách nào thoát → chiếu bí
            return true;
        }

        // Tìm tướng trên bàn cờ
        private static Piece FindGeneral(Board board, bool isRed)
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 9; j++)
                {
                    var p = board.grid[i, j];

                    // Nếu là tướng và đúng màu
                    if (p != null && p.Type == PieceType.General && p.IsRed == isRed)
                        return p;
                }

            return null;
        }
    }
}