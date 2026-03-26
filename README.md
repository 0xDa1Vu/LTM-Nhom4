# ♟️ Trò Chơi Cờ Tướng 

> **Đồ Án Lập Trình Mạng** — Lớp [012012301305]

---

## 👥 Thông Tin Nhóm

| STT | Họ và Tên | MSSV | Vai trò |
|-----|-----------|------|---------|
| 1 | Nguyễn Trọng Nhân | 077206000056 | Nhóm trưởng|
| 2 | Mai Vũ Đại Vũ | 075206002454 | |
| 2 | Lý Đình Bảo | 094206004632 | |
| 2 | Lý Gia Bảo | 072206000828 | |
| 2 | Nguyễn Minh Nhựt | 095206003312 | |
| 2 | Ngô Hoàng Hiếu | 091206009023 | |

---

## 📌 Giới Thiệu Đề Tài

**Cờ Tướng** (hay còn gọi là Tượng Kỳ) là một trò chơi trí tuệ có nguồn gốc từ Trung Quốc, phổ biến rộng rãi tại Việt Nam và các nước châu Á. Trò chơi được chơi bởi hai người trên một bàn cờ gồm 9 cột × 10 hàng, mỗi bên có 16 quân với các vai trò khác nhau.

Dự án này xây dựng một **ứng dụng chơi Cờ Tướng qua mạng** cho phép:
- 🌐 Hai người chơi kết nối và đấu với nhau theo thời gian thực (real-time)
- 🤖 *(Dự kiến)* Chế độ chơi với máy (AI)
- 💬 Nhắn tin trong ván đấu (chat)
- ⏱️ Đồng hồ đếm giờ cho từng lượt đi
- 📜 Lưu lịch sử nước đi

---

## 🎮 Tính Năng Chính

| Tính năng | Mô tả |
|-----------|-------|
| Kết nối mạng P2P / Client-Server | Hai người chơi kết nối qua TCP/UDP |
| Giao diện đồ họa | Hiển thị bàn cờ và quân cờ trực quan |
| Kiểm tra nước đi hợp lệ | Luật di chuyển của từng loại quân |
| Phát hiện chiếu tướng / chiếu bí | Xác định điều kiện thắng/thua |
| Chat trong game | Nhắn tin giữa hai người chơi |
| Đồng hồ thi đấu | Giới hạn thời gian mỗi lượt |

---

## 🗂️ Cấu Trúc Repository

```
ChessGame_Online/
│
├── src/                              # Toàn bộ mã nguồn
│   ├── server/                       # Code phía Server
│   │   ├── Program.cs                # Điểm khởi động server
│   │   ├── ServerManager.cs          # Mở cổng, đón kết nối TCP
│   │   ├── ClientHandler.cs          # Xử lý từng client riêng biệt
│   │   ├── Logger.cs                 # Ghi log ra console và file
│   │   └── Server.csproj             # Cấu hình project server
│   │
│   ├── client/                       # Code phía Client
│   │   ├── UI/
│   │   │   ├── Form1.cs              # Giao diện bàn cờ WinForms
│   │   │   └── Form1.Designer.cs     # File tự sinh của Visual Studio
│   │   ├── Network/
│   │   │   └── SocketClient.cs       # Kết nối TCP, gửi/nhận dữ liệu
│   │   ├── Program.cs                # Khởi động ứng dụng WinForms
│   │   └── Client.csproj             # Cấu hình project client
│   │
│   ├── common/                       # Logic dùng chung
│   │   ├── Piece.cs                  # Định nghĩa quân cờ
│   │   ├── Board.cs                  # Định nghĩa bàn cờ 9x10
│   │   ├── Protocol.cs               # Giao thức gói tin TCP
│   │   └── Gametime.cs               # Đồng hồ và lịch sử nước đi
│   │
│   └── test/                         # Project test mạng
│       └── NetworkTest/
│           └── Program.cs            # Test kết nối TCP độc lập
│
├── docs/                             # Tài liệu
│   ├── Bao_cao.docx                  # Báo cáo Word
│   └── Thuyet_trinh.pptx             # PowerPoint thuyết trình
│
├── reports/                          # Bảng phân công nhiệm vụ
│   └── Phan_cong.xlsx
│
├── assets/                           # Hình ảnh
│   └── images/                       # Ảnh quân cờ PNG
│
├── .gitignore
└── README.md
```

---

## 🛠️ Công Nghệ Sử Dụng

> *(Cập nhật sau khi thống nhất công nghệ)*

| Thành phần | Công nghệ dự kiến |
|------------|-------------------|
| Ngôn ngữ | |
| Giao thức mạng |  |
| Giao diện |  |
| Giao tiếp |  |

---

## ▶️ Hướng Dẫn Chạy Chương Trình

> *(Sẽ cập nhật sau khi hoàn thiện code)*

```bash
# 1. Clone repository
git clone https://github.com/0xDa1Vu/LTM-Nhom4.git
cd ChessGame_Online

# 2. Khởi động Server
cd src/server
# [lệnh chạy server]

# 3. Khởi động Client (mỗi người chơi chạy trên máy của mình)
cd src/client
# [lệnh chạy client]
```

**Yêu cầu hệ thống:**
- [ ] [Môi trường / Runtime version]
- [ ] Kết nối mạng LAN hoặc Internet

---

*Đồ án môn Lập Trình Mạng — [Đại học Giao Thông Vận Tải Tp.Hồ Chí Minh]
