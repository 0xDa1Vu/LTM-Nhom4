using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

class Program
{
    // Header Protocol: 1 byte Type + 4 byte Length = 5 byte
    private const int HEADER_SIZE = 5;

    static async Task Main(string[] args)
    {
        Console.Title = "Network Test - Cờ Tướng";
        var client = new TcpClient();

        Console.Write("Nhập IP server (Enter = 127.0.0.1): ");
        string? ip = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ip)) ip = "127.0.0.1";

        Console.WriteLine($"Đang kết nối tới {ip}:54000 ...");

        try
        {
            await client.ConnectAsync(ip, 54000);
            Console.WriteLine("Kết nối thành công! Đang chờ server ghép cặp...");
            Console.WriteLine("(Gõ tin nhắn để chat, gõ 'exit' để thoát)\n");

            var stream = client.GetStream();

            // ===== LUỒNG NHẬN DỮ LIỆU — DECODE ĐÚNG PROTOCOL =====
            _ = Task.Run(async () =>
            {
                var headerBuf = new byte[HEADER_SIZE];
                while (true)
                {
                    try
                    {
                        // Đọc đúng 5 byte header
                        int totalRead = 0;
                        while (totalRead < HEADER_SIZE)
                        {
                            int read = await stream.ReadAsync(headerBuf, totalRead, HEADER_SIZE - totalRead);
                            if (read <= 0) return;
                            totalRead += read;
                        }

                        byte packetType = headerBuf[0];
                        int payloadLen = BitConverter.ToInt32(headerBuf, 1);

                        // Đọc payload
                        string payload = "";
                        if (payloadLen > 0 && payloadLen < 65536)
                        {
                            byte[] payloadBuf = new byte[payloadLen];
                            int payloadRead = 0;
                            while (payloadRead < payloadLen)
                            {
                                int r = await stream.ReadAsync(payloadBuf, payloadRead, payloadLen - payloadRead);
                                if (r <= 0) return;
                                payloadRead += r;
                            }
                            payload = Encoding.UTF8.GetString(payloadBuf);
                        }

                        // Hiển thị theo loại packet
                        switch (packetType)
                        {
                            case 50: // GameStart
                                string color = payload.Contains("RED") ? "RED" : "BLACK";
                                Console.ForegroundColor = color == "RED" ? ConsoleColor.Red : ConsoleColor.DarkCyan;
                                Console.WriteLine($"[SERVER]: START|{color}");
                                Console.ResetColor();
                                if (color == "RED")
                                    Console.WriteLine(">>> Bạn là quân ĐỎ — ĐI TRƯỚC <<<");
                                else
                                    Console.WriteLine(">>> Bạn là quân ĐEN — ĐI SAU <<<");
                                Console.WriteLine("Format nước đi: fromRow,fromCol,toRow,toCol (VD: 9,0,8,0)");
                                break;

                            case 31: // Move
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"[SERVER]: MOVE|{payload}");
                                Console.ResetColor();
                                break;

                            case 33: // Chat
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"[SERVER]: CHAT|{payload}");
                                Console.ResetColor();
                                break;

                            case 41: // MoveOK
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("[SERVER]: MOVE_OK");
                                Console.ResetColor();
                                break;

                            case 42: // MoveFail
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[SERVER]: MOVE_FAIL");
                                Console.ResetColor();
                                break;

                            case 60: // GameEnd
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine($"[SERVER]: GAME_END|{payload}");
                                Console.ResetColor();
                                return;

                            default:
                                if (!string.IsNullOrEmpty(payload))
                                    Console.WriteLine($"[SERVER]: TYPE={packetType}|{payload}");
                                else
                                    Console.WriteLine($"[SERVER]: TYPE={packetType}");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Lỗi nhận]: {ex.Message}");
                        break;
                    }
                }
            });

            // ===== LUỒNG GỬI DỮ LIỆU =====
            while (true)
            {
                string? input = Console.ReadLine();
                if (input == null || input == "exit") break;

                byte[] packet;

                // Nếu nhập theo format nước đi: "9,0,8,0"
                if (System.Text.RegularExpressions.Regex.IsMatch(input.Trim(), @"^\d+,\d+,\d+,\d+$"))
                {
                    // Tạo packet Move (type=31)
                    byte[] data = Encoding.UTF8.GetBytes(input.Trim());
                    packet = new byte[HEADER_SIZE + data.Length];
                    packet[0] = 31; // Move
                    BitConverter.GetBytes(data.Length).CopyTo(packet, 1);
                    data.CopyTo(packet, HEADER_SIZE);
                    Console.WriteLine($"[Gửi Move]: {input.Trim()}");
                }
                else
                {
                    // Tạo packet Chat (type=33)
                    byte[] data = Encoding.UTF8.GetBytes(input);
                    packet = new byte[HEADER_SIZE + data.Length];
                    packet[0] = 33; // Chat
                    BitConverter.GetBytes(data.Length).CopyTo(packet, 1);
                    data.CopyTo(packet, HEADER_SIZE);
                }

                await stream.WriteAsync(packet, 0, packet.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine("Đã ngắt kết nối.");
        }

        Console.ReadLine();
    }
}