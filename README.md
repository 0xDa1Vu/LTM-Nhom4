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
co-tuong-online/
│
├── src/                        # Toàn bộ mã nguồn
│   ├── server/                 # Code phía Server
│   │   └── ...
│   ├── client/                 # Code phía Client
│   │   └── ...
│   └── common/                 # Logic dùng chung (luật cờ, v.v.)
│       └── ...
│
├── docs/                       # Tài liệu
│   ├── Bao_cao.docx             # Báo cáo Word
│   └── Thuyet_trinh.pptx  # PowerPoint thuyết trình
│
├── reports/                    # Bảng phân công nhiệm vụ
│   └── Phan_cong.xlsx
│
├── assets/                     # Hình ảnh quân cờ, bàn cờ
│   └── images/
│
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
git clone https://github.com/[username]/co-tuong-online.git
cd co-tuong-online

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

## 📋 Phân Công Nhiệm Vụ

| Thành viên | Nhiệm vụ | Tiến độ |
|------------|----------|---------|
| ... | ... | ... |

---

*Đồ án môn Lập Trình Mạng — [Đại học Giao Thông Vận Tải Tp.Hồ Chí Minh]
