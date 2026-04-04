using System.Text.RegularExpressions;

namespace Server
{
    public class ChatHandler
    {
        private Dictionary<string, DateTime> lastMessageTime = new Dictionary<string, DateTime>();
        private HashSet<string> onlineUsers = new HashSet<string>();

        // ===================== LOGIN =====================
        public bool Login(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            if (onlineUsers.Contains(username))
            {
                Console.WriteLine($"[LOGIN FAIL] {username} already online");
                return false;
            }

            onlineUsers.Add(username);
            Console.WriteLine($"[LOGIN SUCCESS] {username}");
            return true;
        }

        public void Logout(string username)
        {
            if (onlineUsers.Remove(username))
            {
                Console.WriteLine($"[LOGOUT] {username}");
            }
        }

        // ===================== CHAT RIÊNG =====================
        public string SendPrivateMessage(string fromUser, string toUser, string message)
        {
            if (!onlineUsers.Contains(fromUser))
                return "[ERROR] Bạn chưa đăng nhập";

            if (!onlineUsers.Contains(toUser))
                return "[ERROR] Người nhận không online";

            if (IsSpam(fromUser, message))
                return "[SPAM BLOCKED]";

            Console.WriteLine($"[PRIVATE] {fromUser} -> {toUser}: {message}");

            // Sau này chỗ này sẽ gửi qua socket
            return message;
        }

        // ===================== CHAT CHUNG =====================
        public string SendGlobalMessage(string fromUser, string message)
        {
            if (!onlineUsers.Contains(fromUser))
                return "[ERROR] Bạn chưa đăng nhập";

            if (IsSpam(fromUser, message))
                return "[SPAM BLOCKED]";

            Console.WriteLine($"[GLOBAL] {fromUser}: {message}");

            // Giả lập gửi cho tất cả user
            foreach (var user in onlineUsers)
            {
                if (user != fromUser)
                {
                    // Sau này thay bằng gửi thật qua socket
                    Console.WriteLine($"   -> gửi tới {user}");
                }
            }

            return message;
        }

        // ===================== ANTI SPAM =====================
        private bool IsSpam(string user, string message)
        {
            if (lastMessageTime.ContainsKey(user))
            {
                var diff = DateTime.Now - lastMessageTime[user];
                if (diff.TotalSeconds < 1)
                    return true;
            }

            lastMessageTime[user] = DateTime.Now;

            if (Regex.IsMatch(message, @"(.)\1{5,}"))
                return true;

            string[] banned = { "spam", "hack", "cheat" };
            foreach (var word in banned)
            {
                if (message.ToLower().Contains(word))
                    return true;
            }

            return false;
        }

        // ===================== STRESS TEST =====================
        public void RunStressTest()
        {
            Console.WriteLine("=== STRESS TEST START ===");

            // tạo user giả
            for (int i = 0; i < 50; i++)
            {
                Login($"user{i}");
            }

            Parallel.For(0, 50, i =>
            {
                string user = $"user{i}";

                for (int j = 0; j < 20; j++)
                {
                    // chat chung
                    SendGlobalMessage(user, $"Hello all {j}");

                    // chat riêng
                    string toUser = $"user{(i + 1) % 50}";
                    SendPrivateMessage(user, toUser, $"Hi {toUser}");
                }
            });

            Console.WriteLine("=== STRESS TEST END ===");
        }
    }
}
