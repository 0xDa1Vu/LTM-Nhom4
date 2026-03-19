using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoTuongOnline.Client;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Client CờTướng";
        var client = new SocketClient();

        // Bắt sự kiện nhận dữ liệu
        client.DataReceived += (data) =>
        {
            string msg = Encoding.UTF8.GetString(data);
            Console.WriteLine($"[SERVER]: {msg}");
        };

        // Bắt sự kiện lỗi mạng
        client.ConnectionError += (ex) =>
        {
            Console.WriteLine($"[LỖI KẾT NỐI]: {ex.Message}");
        };

        Console.Write("Nhập IP server (Enter = 127.0.0.1): ");
        string ip = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ip)) ip = "127.0.0.1";

        Console.WriteLine($"Đang kết nối tới {ip}:54000 ...");

        try
        {
            await client.ConnectAsync(ip, 54000);
            client.StartReceiveLoop();
            Console.WriteLine("Kết nối thành công! Nhập tin nhắn (gõ 'exit' để thoát):");

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit") break;
                byte[] data = Encoding.UTF8.GetBytes(input);
                await client.SendAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }
        finally
        {
            client.Dispose();
            Console.WriteLine("Đã ngắt kết nối.");
        }

        Console.ReadLine();
    }
}