using System;
using CoTuongOnline.Common;

namespace CoTuongOnline.Logic
{
    // Class kiểm tra luật di chuyển của từng quân cờ
    public static class MoveValidator
    {
        //  TÌM VỊ TRÍ THẬT CỦA QUÂN TRÊN BOARD
        private static (int row, int col) FindPiece(Board board, Piece piece)
        {
            // Duyệt toàn bộ bàn cờ để tìm quân
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 9; j++)
                    if (board.grid[i, j] == piece)
                        return (i, j); // trả về vị trí tìm thấy

            return (-1, -1); // không tìm thấy
        }

        // Hàm kiểm tra nước đi hợp lệ
        public static bool IsValidMove(Board board, Piece piece, int toRow, int toCol)
        {
            if (piece == null) return false;

            // Tìm vị trí hiện tại của quân
            var (fromRow, fromCol) = FindPiece(board, piece);
            if (fromRow == -1) return false;

            var target = board.grid[toRow, toCol];

            // Không được ăn quân cùng màu
            if (target != null && target.IsRed == piece.IsRed)
                return false;

            // Kiểm tra theo từng loại quân
            switch (piece.Type)
            {
                case PieceType.Rook:     // Xe
                    return ValidateRook(board, fromRow, fromCol, toRow, toCol);

                case PieceType.Cannon:   // Pháo
                    return ValidateCannon(board, fromRow, fromCol, toRow, toCol);

                case PieceType.Horse:    // Mã
                    return ValidateHorse(board, fromRow, fromCol, toRow, toCol);

                case PieceType.Elephant: // Tượng
                    return ValidateElephant(board, piece, fromRow, fromCol, toRow, toCol);

                case PieceType.Advisor:  // Sĩ
                    return ValidateAdvisor(piece, fromRow, fromCol, toRow, toCol);

                case PieceType.General:  // Tướng
                    return ValidateGeneral(board, piece, fromRow, fromCol, toRow, toCol);

                case PieceType.Soldier:  // Tốt
                    return ValidateSoldier(piece, fromRow, fromCol, toRow, toCol);
            }

            return false;
        }

        // ===== XE =====
        private static bool ValidateRook(Board board, int fr, int fc, int tr, int tc)
        {
            // Xe chỉ đi ngang hoặc dọc
            if (fr != tr && fc != tc) return false;

            // Đi ngang
            if (fr == tr)
            {
                int step = tc > fc ? 1 : -1;
                for (int i = fc + step; i != tc; i += step)
                    if (board.grid[fr, i] != null) return false; // bị chặn
            }
            else // đi dọc
            {
                int step = tr > fr ? 1 : -1;
                for (int i = fr + step; i != tr; i += step)
                    if (board.grid[i, fc] != null) return false;
            }

            return true;
        }

        // ===== PHÁO =====
        private static bool ValidateCannon(Board board, int fr, int fc, int tr, int tc)
        {
            // Pháo cũng đi thẳng như Xe
            if (fr != tr && fc != tc) return false;

            int count = 0; // đếm số quân chắn

            if (fr == tr)
            {
                int step = tc > fc ? 1 : -1;
                for (int i = fc + step; i != tc; i += step)
                    if (board.grid[fr, i] != null) count++;
            }
            else
            {
                int step = tr > fr ? 1 : -1;
                for (int i = fr + step; i != tr; i += step)
                    if (board.grid[i, fc] != null) count++;
            }

            // Không ăn quân → không được có vật cản
            if (board.grid[tr, tc] == null)
                return count == 0;
            else // Ăn quân → phải có đúng 1 quân chắn
                return count == 1;
        }

        // ===== MÃ =====
        private static bool ValidateHorse(Board board, int fr, int fc, int tr, int tc)
        {
            int dr = tr - fr;
            int dc = tc - fc;

            // Các bước đi hợp lệ của mã
            int[,] moves = {
                {2,1},{2,-1},{-2,1},{-2,-1},
                {1,2},{1,-2},{-1,2},{-1,-2}
            };

            for (int i = 0; i < moves.GetLength(0); i++)
            {
                if (dr == moves[i, 0] && dc == moves[i, 1])
                {
                    // Kiểm tra "chặn chân"
                    int blockRow = fr + (moves[i, 0] == 2 || moves[i, 0] == -2 ? moves[i, 0] / 2 : 0);
                    int blockCol = fc + (moves[i, 1] == 2 || moves[i, 1] == -2 ? moves[i, 1] / 2 : 0);

                    return board.grid[blockRow, blockCol] == null;
                }
            }

            return false;
        }

        // ===== TƯỢNG =====
        private static bool ValidateElephant(Board board, Piece p, int fr, int fc, int tr, int tc)
        {
            int dr = tr - fr;
            int dc = tc - fc;

            // Đi chéo 2 ô
            if (Math.Abs(dr) != 2 || Math.Abs(dc) != 2) return false;

            // Không được qua sông
            if (p.IsRed && tr < 5) return false;
            if (!p.IsRed && tr > 4) return false;

            // Không bị chặn ở giữa
            if (board.grid[fr + dr / 2, fc + dc / 2] != null)
                return false;

            return true;
        }

        // ===== SĨ =====
        private static bool ValidateAdvisor(Piece p, int fr, int fc, int tr, int tc)
        {
            // Đi chéo 1 ô
            if (Math.Abs(tr - fr) != 1 || Math.Abs(tc - fc) != 1)
                return false;

            // Phải ở trong cung
            return InPalace(p.IsRed, tr, tc);
        }

        // ===== TƯỚNG =====
        private static bool ValidateGeneral(Board board, Piece p, int fr, int fc, int tr, int tc)
        {
            // Đi 1 ô (trên/dưới/trái/phải)
            if (Math.Abs(tr - fr) + Math.Abs(tc - fc) != 1)
                return false;

            // Phải ở trong cung
            if (!InPalace(p.IsRed, tr, tc))
                return false;

            // Kiểm tra 2 tướng không được nhìn nhau
            if (fc == tc)
            {
                int opponentRow = -1;

                // Tìm tướng đối phương
                for (int r = 0; r < 10; r++)
                {
                    var piece = board.grid[r, tc];
                    if (piece != null && piece.Type == PieceType.General && piece.IsRed != p.IsRed)
                    {
                        opponentRow = r;
                        break;
                    }
                }

                if (opponentRow != -1)
                {
                    int minRow = Math.Min(tr, opponentRow) + 1;
                    int maxRow = Math.Max(tr, opponentRow);

                    bool hasBlocker = false;

                    // Kiểm tra có quân chắn không
                    for (int r = minRow; r < maxRow; r++)
                    {
                        if (board.grid[r, tc] != null)
                        {
                            hasBlocker = true;
                            break;
                        }
                    }

                    // Nếu không có quân chắn → sai luật
                    if (!hasBlocker) return false;
                }
            }

            return true;
        }

        // ===== TỐT =====
        private static bool ValidateSoldier(Piece p, int fr, int fc, int tr, int tc)
        {
            int dr = tr - fr;
            int dc = Math.Abs(tc - fc);

            if (p.IsRed)
            {
                // Chưa qua sông → chỉ đi thẳng
                if (fr >= 5)
                    return dr == -1 && dc == 0;
                else // Qua sông → được đi ngang
                    return (dr == -1 && dc == 0) || (dr == 0 && dc == 1);
            }
            else
            {
                if (fr <= 4)
                    return dr == 1 && dc == 0;
                else
                    return (dr == 1 && dc == 0) || (dr == 0 && dc == 1);
            }
        }

        // Kiểm tra có nằm trong cung không
        private static bool InPalace(bool isRed, int r, int c)
        {
            if (c < 3 || c > 5) return false;
            return isRed ? (r >= 7 && r <= 9) : (r >= 0 && r <= 2);
        }
    }
}