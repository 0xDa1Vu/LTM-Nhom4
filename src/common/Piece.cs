using System;

namespace CoTuongOnline.Common
{
    // Enum dùng để định nghĩa các loại quân cờ (giống như 1 danh sách cố định)
    public enum PieceType
    {
        General,   // Tướng
        Advisor,   // Sĩ
        Elephant,  // Tượng
        Horse,     // Mã
        Rook,      // Xe
        Cannon,    // Pháo
        Soldier    // Tốt
    }

    // Lớp đại diện cho 1 quân cờ
    public class Piece
    {
        public PieceType Type; // Loại quân (Xe, Mã, Tướng...)
        public bool IsRed;     // Màu quân: true = đỏ, false = đen
        public int Row;        // Vị trí hàng (0 → 9)
        public int Col;        // Vị trí cột (0 → 8)

        // Constructor: dùng để tạo 1 quân cờ mới
        public Piece(PieceType type, bool isRed, int row, int col)
        {
            // Gán loại quân
            Type = type;

            // Gán màu quân
            IsRed = isRed;

            // Gán vị trí ban đầu
            Row = row;
            Col = col;
        }

        // Hàm này dùng khi bạn in object ra (Console.WriteLine)
        public override string ToString()
        {
            // Trả về chuỗi mô tả quân cờ
            // Ví dụ: "Rook (Red) [9,0]"
            return $"{Type} ({(IsRed ? "Red" : "Black")}) [{Row},{Col}]";
        }
