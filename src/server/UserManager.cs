using System;
using System.Collections.Generic;

namespace ChessServer
{
    public class User
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool IsOnline { get; set; }
}

    public class UserManager
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();

        // Đăng ký
        public bool Register(string username, string password)
        {
            if (users.ContainsKey(username))
                return false;

            users[username] = new User
            {
                Username = username,
                Password = password,
                IsOnline = false
            };

            return true;
        }

        // Đăng nhập
        public bool Login(string username, string password)
        {
            if (!users.ContainsKey(username))
                return false;

            var user = users[username];

            if (user.Password != password)
                return false;

            if (user.IsOnline)
                return false;

            user.IsOnline = true;
            Console.WriteLine($"[LOGIN] {username} online");
            return true;
        }

        // Đăng xuất
        public void Logout(string username)
        {
            if (users.ContainsKey(username))
            {
                users[username].IsOnline = false;
                Console.WriteLine($"[LOGOUT] {username} offline");
            }
        }

        // Danh sách online
        public List<string> GetOnlineUsers()
        {
            List<string> list = new List<string>();

            foreach (var u in users.Values)
            {
                if (u.IsOnline)
                    list.Add(u.Username);
            }

            return list;
        }
    }
}