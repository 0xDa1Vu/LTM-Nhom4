using System;
using System.Text;
using CoTuongOnline.Network;

namespace CoTuongOnline.Client.Network
{
    /// <summary>
    /// Giải mã gói tin nhận được từ Server
    /// Đọc Header → xác định loại gói tin → trả về dữ liệu
    /// </summary>
    public class PacketParser
    {
        private const int HEADER_SIZE = 5;

        /// <summary>
        /// Kết quả sau khi parse 1 gói tin
        /// </summary>
        public class ParseResult
        {
            public PacketType Type { get; set; }
            public string Data { get; set; } = "";
            public bool IsValid { get; set; }
        }

        /// <summary>
        /// Parse dữ liệu thô byte[] từ Server thành ParseResult
        /// </summary>
        public static ParseResult Parse(byte[] buffer)
        {
            var result = new ParseResult { IsValid = false };

            // Kiểm tra buffer có đủ header không
            if (buffer == null || buffer.Length < HEADER_SIZE)
            {
                Console.WriteLine("[PARSER] Buffer quá ngắn hoặc null");
                return result;
            }

            try
            {
                // Đọc loại gói tin từ byte đầu tiên
                result.Type = (PacketType)buffer[0];

                // Đọc độ dài dữ liệu từ 4 bytes tiếp theo
                int dataLength = BitConverter.ToInt32(buffer, 1);

                // Kiểm tra buffer có đủ dữ liệu không
                if (buffer.Length < HEADER_SIZE + dataLength)
                {
                    Console.WriteLine("[PARSER] Buffer không đủ dữ liệu");
                    return result;
                }

                // Đọc dữ liệu
                result.Data = Encoding.UTF8.GetString(buffer, HEADER_SIZE, dataLength);
                result.IsValid = true;

                Console.WriteLine($"[PARSER] Loại: {result.Type} | Dữ liệu: {result.Data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PARSER] Lỗi parse: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Parse gói tin MOVE → trả về tọa độ (fromCol, fromRow, toCol, toRow)
        /// Định dạng: "x1,y1,x2,y2"
        /// </summary>
        public static (int fc, int fr, int tc, int tr) ParseMove(string data)
        {
            try
            {
                var parts = data.Split(',');
                if (parts.Length != 4)
                    return (-1, -1, -1, -1);

                return (
                    int.Parse(parts[0]),
                    int.Parse(parts[1]),
                    int.Parse(parts[2]),
                    int.Parse(parts[3])
                );
            }
            catch
            {
                return (-1, -1, -1, -1);
            }
        }

        /// <summary>
        /// Kiểm tra gói tin có phải loại cụ thể không
        /// </summary>
        public static bool IsType(byte[] buffer, PacketType type)
        {
            if (buffer == null || buffer.Length < 1) return false;
            return (PacketType)buffer[0] == type;
        }
    }
}