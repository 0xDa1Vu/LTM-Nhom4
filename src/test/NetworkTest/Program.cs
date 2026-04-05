using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Network Test";
        var client = new TcpClient();

        Console.Write("Nhập IP server (Enter = 127.0.0.1): ");
        string ip = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ip)) ip = "127.0.0.1";

        Console.WriteLine($"Đang kết nối tới {ip}:54000 ...");

        try
        {
            await client.ConnectAsync(ip, 54000);
            Console.WriteLine("Kết nối thành công! Nhập tin nhắn (gõ 'exit' để thoát):");

            var stream = client.GetStream();

            // Luồng nhận dữ liệu
            _ = Task.Run(async () =>
            {
                var buffer = new byte[8192];
                while (true)
                {
                    int read = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (read <= 0) break;
                    Console.WriteLine($"[SERVER]: {Encoding.UTF8.GetString(buffer, 0, read)}");
                }
            });

            // Luồng gửi dữ liệu
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit") break;
                byte[] data = Encoding.UTF8.GetBytes(input);
                await stream.WriteAsync(data, 0, data.Length);
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