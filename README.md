# ♟️ Trò Chơi Cờ Tướng 

> **Đồ Án Lập Trình Mạng** — Lớp [012012301305]

---

## 👥 Thông Tin Nhóm

| STT | Họ và Tên | MSSV | Vai trò |
|-----|-----------|------|---------|
| 1 | Nguyễn Văn A | 12345678 | Nhóm trưởng, Backend |
| 2 | Trần Thị B | 87654321 | Frontend, Báo cáo |
| 2 | Trần Thị B | 87654321 | Frontend, Báo cáo |
| 2 | Trần Thị B | 87654321 | Frontend, Báo cáo |
| 2 | Trần Thị B | 87654321 | Frontend, Báo cáo |

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
│   ├── BaoCao.docx             # Báo cáo Word
│   └── Slide_ThuyetTrinh.pptx  # PowerPoint thuyết trình
│
├── reports/                    # Bảng phân công nhiệm vụ
│   └── PhanCongNhiemVu.xlsx
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
| Ngôn ngữ | Java / Python / C++ |
| Giao thức mạng | TCP Socket |
| Giao diện | JavaFX / Pygame / Qt |
| Giao tiếp | Client-Server hoặc P2P |

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

## 🎥 Video Báo Cáo

| Nội dung | Link |
|----------|------|
| 📹 Video thuyết trình | [Dán link Drive/YouTube tại đây] |

> Video bao gồm phần trình bày của từng thành viên, hiển thị mặt để đối chiếu, và demo chương trình chạy thực tế.

---

## 📋 Phân Công Nhiệm Vụ

| Thành viên | Nhiệm vụ | Tiến độ |
|------------|----------|---------|
| Nguyễn Văn A | Thiết kế kiến trúc mạng, lập trình Server | 🔲 Chưa bắt đầu |
| Trần Thị B | Giao diện người dùng, báo cáo | 🔲 Chưa bắt đầu |
| ... | ... | ... |

---

*Đồ án môn Lập Trình Mạng — [Đại học Giao Thông Vận Tải Tp.Hồ Chí Minh]
