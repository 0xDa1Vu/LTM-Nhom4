using System;

namespace CoTuongOnline.Common
{
    // Các loại quân cờ
    public enum PieceType
    {
        General, Advisor, Elephant, Horse, Rook, Cannon, Soldier
    }

    // Lớp quân cờ
    public class Piece
    {
        public PieceType Type; // loại quân
        public bool IsRed;     // đỏ hay đen
        public int Row;        // vị trí hàng
        public int Col;        // vị trí cột

        // Constructor
        public Piece(PieceType type, bool isRed, int row, int col)
        {
            Type = type;
            IsRed = isRed;
            Row = row;
            Col = col;
        }

        // Debug hiển thị
        public override string ToString()
        {
            return $"{Type} ({(IsRed ? "Red" : "Black")}) [{Row},{Col}]";
        }
    }
}