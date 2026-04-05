using System;
using CoTuongOnline.Common;

namespace CoTuongOnline.Logic
{
    // Class chứa các luật liên quan đến chiếu và chiếu bí
    public static class ChessRules
    {
        // ================= KIỂM TRA CHIẾU =================
        public static bool IsCheck(Board board, bool isRed)
        {
            // Tìm quân Tướng của bên đang xét (đỏ hoặc đen)
            Piece general = FindGeneral(board, isRed);

            // Nếu không tìm thấy tướng (trường hợp lỗi) → không xét
            if (general == null) return false;

            // Duyệt toàn bộ bàn cờ (10 hàng, 9 cột)
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Lấy quân tại vị trí (i, j)
                    var p = board.grid[i, j];

                    // Nếu có quân và là quân đối phương
                    if (p != null && p.IsRed != isRed)
                    {
                        // Kiểm tra: quân đó có thể ăn được tướng không
                        // (dùng luật di chuyển của từng quân)
                        if (MoveValidator.IsValidMove(board, p, general.Row, general.Col))
                            return true; // nếu ăn được → đang bị chiếu
                    }
                }
            }

            // Không có quân nào ăn được tướng → không bị chiếu
            return false;
        }

        // ================= KIỂM TRA CHIẾU BÍ =================
        public static bool IsCheckmate(Board board, bool isRed)
        {
            // Nếu chưa bị chiếu → chắc chắn không phải chiếu bí
            if (!IsCheck(board, isRed))
                return false;

            // Duyệt tất cả quân của bên mình
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Lấy quân tại ô (i, j)
                    var p = board.grid[i, j];

                    // Nếu ô trống hoặc không phải quân mình → bỏ qua
                    if (p == null || p.IsRed != isRed) continue;

                    // Thử di chuyển quân này tới mọi ô trên bàn cờ
                    for (int r = 0; r < 10; r++)
                    {
                        for (int c = 0; c < 9; c++)
                        {
                            // Nếu nước đi không hợp lệ → bỏ qua
                            if (!MoveValidator.IsValidMove(board, p, r, c))
                                continue;

                            // ===== GIẢ LẬP NƯỚC ĐI =====

                            // Lưu quân tại ô đích (có thể bị ăn)
                            var temp = board.grid[r, c];

                            // Lưu vị trí cũ của quân
                            int oldR = p.Row, oldC = p.Col;

                            // Thực hiện nước đi thử
                            board.grid[r, c] = p;        // đưa quân đến vị trí mới
                            board.grid[oldR, oldC] = null; // xóa vị trí cũ
                            p.Row = r;
                            p.Col = c;

                            // Sau khi đi thử → kiểm tra còn bị chiếu không
                            bool stillCheck = IsCheck(board, isRed);

                            // ===== HOÀN TÁC (rollback) =====

                            // Trả lại trạng thái ban đầu
                            board.grid[oldR, oldC] = p;
                            board.grid[r, c] = temp;
                            p.Row = oldR;
                            p.Col = oldC;

                            // Nếu tồn tại ít nhất 1 nước đi giúp thoát chiếu
                            if (!stillCheck)
                                return false; // → không phải chiếu bí
                        }
                    }
                }
            }

            // Không có cách nào thoát → chiếu bí
            return true;
        }

        // ================= TÌM TƯỚNG =================
        private static Piece FindGeneral(Board board, bool isRed)
        {
            // Duyệt toàn bộ bàn cờ để tìm quân Tướng
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 9; j++)
                {
                    var p = board.grid[i, j];

                    // Nếu là tướng và đúng màu → trả về
                    if (p != null && p.Type == PieceType.General && p.IsRed == isRed)
                        return p;
                }

            // Không tìm thấy → trả về null
            return null;
        }
    }
}