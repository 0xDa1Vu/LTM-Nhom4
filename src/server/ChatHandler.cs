using System.Text.RegularExpressions;

namespace ChessServer
{
    public class ChatHandler
    {
        private Dictionary<string, DateTime> lastMessageTime = new Dictionary<string, DateTime>();

        public string SendMessage(string fromUser, string toUser, string message)
        {
            if (IsSpam(fromUser, message))
            {
                return "[SPAM BLOCKED]";
            }

            Console.WriteLine($"[CHAT] {fromUser} -> {toUser}: {message}");
            return message;
        }

        private bool IsSpam(string user, string message)
        {
            // gửi quá nhanh
            if (lastMessageTime.ContainsKey(user))
            {
                var diff = DateTime.Now - lastMessageTime[user];
                if (diff.TotalSeconds < 1)
                    return true;
            }

            lastMessageTime[user] = DateTime.Now;

            // lặp ký tự
            if (Regex.IsMatch(message, @"(.)\1{5,}"))
                return true;

            // từ cấm
            string[] banned = { "spam", "hack", "cheat" };
            foreach (var word in banned)
            {
                if (message.ToLower().Contains(word))
                    return true;
            }

            return false;
        }
    }
}