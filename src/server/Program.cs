using System;

namespace ChessServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server ChessGame_Online";

            // Khởi tạo Server
            ServerManager server = new ServerManager();

            // Chạy server!
            server.Start();

            // Lệnh này giữ cho màn hình đen không bị tắt đi
            Console.ReadLine();
        }
    }
}