# ♟️ Trò Chơi Cờ Tướng Online

> **Đồ Án Lập Trình Mạng** — Lớp 012012301305  
> Đại học Giao Thông Vận Tải TP. Hồ Chí Minh

![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![C#](https://img.shields.io/badge/C%23-WinForms-blue)
![TCP](https://img.shields.io/badge/Protocol-TCP%2FIP-green)
![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey)

---

## 👥 Thông Tin Nhóm

| STT | Họ và Tên | MSSV | Vai trò | Sản phẩm bàn giao |
|-----|-----------|------|---------|-------------------|
| 1 | Nguyễn Trọng Nhân | 077206000056 | Nhóm trưởng / Server | `ServerManager.cs`, `RoomManager.cs`, `GameRoom.cs` |
| 2 | Mai Vũ Đại Vũ | 075206002454 | Client Network | `SocketClient.cs`, `PacketParser.cs`, `ConnectionGuard.cs` |
| 3 | Lý Đình Bảo | 094206004632 | GUI Designer | `Form1.cs`, `FormMenu.cs`, `FormResult.cs`, `SoundManager.cs` |
| 4 | Lý Gia Bảo | 072206000828 | Game Logic | `ChessRules.cs`, `MoveValidator.cs`, `FinalLogic.cs` |
| 5 | Nguyễn Minh Nhựt | 095206003312 | Protocol & Timer | `Protocol.cs`, `GameTimer.cs` |
| 6 | Ngô Hoàng Hiếu | 091206009023 | Logic & Security | `UserManager.cs`, `ChatHandler.cs` |

---

## 📌 Giới Thiệu Đề Tài

**Cờ Tướng** (Tượng Kỳ) là trò chơi trí tuệ có nguồn gốc từ Trung Quốc, phổ biến rộng rãi tại Việt Nam và các nước châu Á. Trò chơi được chơi bởi hai người trên bàn cờ **9 cột × 10 hàng**, mỗi bên có **16 quân** với các vai trò khác nhau.

Dự án xây dựng ứng dụng **chơi Cờ Tướng qua mạng** theo mô hình **Client-Server** sử dụng giao thức **TCP thuần** với giao thức tự định nghĩa.

---

## 🎮 Tính Năng Chính

| Tính năng | Mô tả | Trạng thái |
|-----------|-------|-----------|
| Kết nối mạng TCP | Hai người chơi kết nối qua LAN hoặc localhost | ✅ Hoàn thành |
| Ghép cặp tự động | Server tự ghép 2 người vào 1 phòng chơi | ✅ Hoàn thành |
| Giao diện đồ họa | Bàn cờ GDI+, ảnh quân cờ PNG, highlight nước đi | ✅ Hoàn thành |
| Luật di chuyển | Đầy đủ 7 loại quân: Xe, Pháo, Mã, Tượng, Sĩ, Tướng, Tốt | ✅ Hoàn thành |
| Chiếu tướng / chiếu bí | Phát hiện và kết thúc game tự động | ✅ Hoàn thành |
| Đồng hồ đếm ngược | 30 giây mỗi lượt, hết giờ thua | ✅ Hoàn thành |
| Chat trong game | Nhắn tin + lệnh `/help`, `/surrender`, `/exit` | ✅ Hoàn thành |
| Lọc spam | Rate limit, lặp ký tự, từ cấm | ✅ Hoàn thành |
| Auto-reconnect | Tự động kết nối lại khi mất mạng (exponential backoff) | ✅ Hoàn thành |
| Âm thanh | Di chuyển quân và ăn quân | ✅ Hoàn thành |
| Menu chính | Màn hình bắt đầu và kết thúc | ✅ Hoàn thành |

---

## 🛠️ Công Nghệ Sử Dụng

| Thành phần | Công nghệ |
|------------|-----------|
| Ngôn ngữ | C# / .NET 10.0 |
| Giao thức mạng | TCP — `TcpListener` / `TcpClient` — Cổng **54000** |
| Giao diện | WinForms + GDI+ |
| Giao tiếp | Custom Protocol (`PacketType` Enum + `PacketHeader` 5 byte) |
| Đa luồng | `Task` / `async-await` |
| IDE | Visual Studio 2022 |
| Version control | Git / GitHub |

---

## 🗂️ Cấu Trúc Repository

```
ChessGame_Online/
│
├── src/
│   ├── server/                   # Code phía Server
│   │   ├── Program.cs            # Điểm khởi động server
│   │   ├── ServerManager.cs      # Mở cổng TCP, đón kết nối
│   │   ├── RoomManager.cs        # Ghép cặp người chơi (Singleton)
│   │   ├── GameRoom.cs           # Relay nước đi giữa 2 người chơi
│   │   ├── ClientHandler.cs      # Xử lý từng client
│   │   ├── UserManager.cs        # Đăng nhập / đăng xuất
│   │   ├── ChatHandler.cs        # Chat + spam filter + stress test
│   │   ├── Logger.cs             # Ghi log ra console và file
│   │   └── Server.csproj
│   │
│   ├── client/                   # Code phía Client
│   │   ├── UI/
│   │   │   ├── Form1.cs          # Giao diện bàn cờ chính
│   │   │   ├── FormMenu.cs       # Menu chính
│   │   │   ├── FormResult.cs     # Màn hình thắng/thua
│   │   │   ├── ChatBox.cs        # Khung chat + lệnh
│   │   │   └── SoundManager.cs   # Phát âm thanh
│   │   ├── Network/
│   │   │   ├── SocketClient.cs        # Kết nối TCP
│   │   │   ├── PacketParser.cs        # Giải mã gói tin (TCP framing)
│   │   │   ├── ClientEventListener.cs # Bridge mạng ↔ UI
│   │   │   └── ConnectionGuard.cs     # Auto-reconnect
│   │   ├── Program.cs
│   │   └── Client.csproj
│   │
│   ├── common/                   # Dùng chung Server & Client
│   │   ├── Protocol.cs           # Giao thức gói tin TCP
│   │   ├── Piece.cs              # Định nghĩa quân cờ
│   │   ├── Board.cs              # Bàn cờ 9×10
│   │   ├── ChessRules.cs         # Kiểm tra chiếu / chiếu bí
│   │   ├── MoveValidator.cs      # Luật di chuyển 7 loại quân
│   │   ├── FinalLogic.cs         # Logic nước đi hoàn chỉnh
│   │   └── GameTimer.cs          # Đồng hồ đếm ngược
│   │
│   └── test/NetworkTest/         # Test kết nối TCP độc lập
│
├── assets/images/                # Ảnh quân cờ PNG (32 quân)
├── assets/sounds/                # Âm thanh move.wav, capture.wav
├── docs/
│   ├── Bao_cao.docx              # Báo cáo Word
│   └── Thuyet_trinh.pptx         # PowerPoint thuyết trình
├── reports/Phan_cong.xlsx        # Bảng phân công nhiệm vụ
├── .gitignore
└── README.md
```

---

## ⚙️ Yêu Cầu Hệ Thống

- **OS:** Windows 10/11
- **Runtime:** [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- **IDE:** Visual Studio 2022 (khuyến nghị) hoặc VS Code
- **Mạng:** Kết nối LAN hoặc localhost

---

## ▶️ Hướng Dẫn Chạy Chương Trình

### Bước 1 — Clone repository

```bash
git clone https://github.com/0xDa1Vu/LTM-Nhom4.git
cd LTM-Nhom4
```

### Bước 2 — Khởi động Server

```bash
cd src/server
dotnet run
```

Kết quả mong đợi:
```
=== STRESS TEST END ===
=== STRESS TEST HOÀN THÀNH! ===

Nhấn Enter để khởi động Server...
[dd/MM/yyyy HH:mm:ss] === SERVER CỜ TƯỚNG ĐÃ MỞ TẠI CỔNG 54000 ===
```

### Bước 3 — Khởi động Client (mỗi người chơi chạy trên máy của mình)

```bash
cd src/client
dotnet run
```

> **Lưu ý:** Nếu chơi qua LAN, người chơi 2 cần biết **địa chỉ IP** của máy chạy Server.  
> Tìm IP bằng lệnh: `ipconfig` → xem dòng **IPv4 Address**

### Bước 4 — Bắt đầu chơi

1. Cả 2 người bấm **"Thách Đấu"**
2. Server tự động ghép cặp
3. Người được gán quân **Đỏ** đi trước
4. Mỗi lượt có **30 giây** để đi — hết giờ thua

---

## 🎯 Hướng Dẫn Chơi

### Di chuyển quân cờ
- **Click chuột** vào quân của mình → hiện các ô hợp lệ (chấm xanh)
- Click vào ô muốn di chuyển → quân di chuyển

### Lệnh trong Chat
| Lệnh | Chức năng |
|------|-----------|
| `/help` | Xem danh sách lệnh |
| `/surrender` | Đầu hàng |
| `/exit` | Thoát game |
| (nhắn bình thường) | Gửi tin nhắn cho đối thủ |

---

## 🔌 Giao Thức Tự Định Nghĩa

Mỗi gói tin gồm **Header 5 byte** + **Payload**:

```
[Type: 1 byte] [Length: 4 byte] [Data: N byte]
```

| PacketType | Giá trị | Ý nghĩa |
|------------|---------|---------|
| Login | 10 | Đăng nhập |
| LoginOK | 11 | Đăng nhập thành công |
| LoginFail | 12 | Đăng nhập thất bại |
| Ready | 30 | Sẵn sàng chơi |
| Move | 31 | Nước đi: `"x1,y1,x2,y2"` |
| Surrender | 32 | Đầu hàng |
| Chat | 33 | Tin nhắn chat |
| MoveOK | 41 | Server xác nhận nước đi hợp lệ |
| MoveFail | 42 | Server từ chối nước đi |
| GameStart | 50 | Bắt đầu ván đấu |
| GameEnd | 60 | Kết thúc ván đấu |
| Heartbeat | 99 | Giữ kết nối |

---

## 🧪 Kết Quả Kiểm Thử

### Stress Test ChatHandler
- **50 user** giả đăng nhập đồng thời
- **1000 tin nhắn** gửi song song (Parallel)
- Spam filter hoạt động đúng (rate limit 1s, lặp ký tự, từ cấm)
- Kết quả: `=== STRESS TEST END ===` — không crash

### Test kết nối TCP
- Server ↔ Client trên cùng máy (localhost): ✅
- Server ↔ Client qua LAN: ✅
- Ghép cặp 2 người chơi tự động: ✅

---

## 📋 Phân Công Nhiệm Vụ Chi Tiết

| Tuần | Thành viên | Nhiệm vụ | Sản phẩm |
|------|-----------|----------|----------|
| 1 | Nguyễn Trọng Nhân | Tạo solution, ServerManager, Logger, ClientHandler | Server chạy được |
| 1 | Mai Vũ Đại Vũ | SocketClient kết nối TCP | SocketClient.cs |
| 1 | Lý Đình Bảo | Giao diện WinForms, ảnh quân cờ | Form1.cs, assets/ |
| 1 | Lý Gia Bảo | Piece, Board, khởi tạo 32 quân | Piece.cs, Board.cs |
| 1 | Nguyễn Minh Nhựt | Protocol enum, PacketHeader, GameTimer | Protocol.cs, GameTimer.cs |
| 1 | Ngô Hoàng Hiếu | Báo cáo tuần 1, kịch bản test | Bao_cao.docx |
| 2 | Nguyễn Trọng Nhân | RoomManager (ghép cặp), GameRoom (relay) | RoomManager.cs |
| 2 | Mai Vũ Đại Vũ | PacketParser (TCP framing), ClientEventListener | PacketParser.cs |
| 2 | Lý Đình Bảo | Click chọn quân, highlight nước đi | Form1.cs nâng cấp |
| 2 | Lý Gia Bảo | ChessRules (chiếu/chiếu bí), MoveValidator | ChessRules.cs |
| 2 | Nguyễn Minh Nhựt | GameTimer countdown 30s, Protocol v2 | GameTimer.cs nâng cấp |
| 2 | Ngô Hoàng Hiếu | UserManager, ChatHandler + spam filter | UserManager.cs |
| 3 | Nguyễn Trọng Nhân | Tối ưu server, video demo | Server ổn định |
| 3 | Mai Vũ Đại Vũ | ConnectionGuard auto-reconnect | ConnectionGuard.cs |
| 3 | Lý Đình Bảo | FormMenu, FormResult, SoundManager, ChatBox | Giao diện hoàn chỉnh |
| 3 | Lý Gia Bảo | FinalLogic tích hợp toàn bộ luật | FinalLogic.cs |
| 3 | Nguyễn Minh Nhựt | Đồng bộ Timer với UI | GameTimer.cs final |
| 3 | Ngô Hoàng Hiếu | Stress test, hoàn thiện báo cáo | ChatHandler.cs |

---

*Đồ án môn Lập Trình Mạng — Đại học Giao Thông Vận Tải TP. Hồ Chí Minh*
