using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CoTuongGame.Network
{
    // Enum mã lệnh
    public enum PacketType : byte
    {
        Login = 10, LoginOK = 11, LoginFail = 12,
        Logout = 20,
        Move = 30, MoveOK = 31, MoveFail = 32,
        Chat = 40,
        GameStart = 50, GameEnd = 60,
        Heartbeat = 99
    }

    // Header gói tin (5 bytes)
    public struct PacketHeader
    {
        public PacketType Type;
        public int Length;
    }

    public class Protocol
    {
        private const int HEADER_SIZE = 5;

        // Tạo gói tin Move: "x1,y1,x2,y2"
        public static byte[] CreateMove(int x1, int y1, int x2, int y2)
        {
            string data = $"{x1},{y1},{x2},{y2}";
            return CreatePacket(PacketType.Move, data);
        }

        // Tạo gói tin Chat
        public static byte[] CreateChat(string message)
        {
            return CreatePacket(PacketType.Chat, message);
        }

        // Tạo gói tin chung
        private static byte[] CreatePacket(PacketType type, string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            PacketHeader header = new PacketHeader { Type = type, Length = dataBytes.Length };
            
            byte[] headerBytes = StructToBytes(header);
            byte[] packet = new byte[HEADER_SIZE + dataBytes.Length];
            
            Array.Copy(headerBytes, 0, packet, 0, HEADER_SIZE);
            Array.Copy(dataBytes, 0, packet, HEADER_SIZE, dataBytes.Length);
            
            return packet;
        }

        // Đọc loại gói tin
        public static PacketType ReadType(byte[] buffer)
        {
            if (buffer.Length < HEADER_SIZE) return PacketType.Heartbeat;
            return BytesToStruct<PacketHeader>(buffer).Type;
        }

        // Đọc dữ liệu gói tin
        public static string ReadData(byte[] buffer)
        {
            PacketHeader header = BytesToStruct<PacketHeader>(buffer);
            return Encoding.UTF8.GetString(buffer, HEADER_SIZE, header.Length);
        }

        // Struct <-> Bytes
        private static byte[] StructToBytes<T>(T data) where T : struct
        {
            int size = Marshal.SizeOf(data);
            byte[] bytes = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, true);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);
            return bytes;
        }

        private static T BytesToStruct<T>(byte[] bytes) where T : struct
        {
            int size = Marshal.SizeOf<T>();
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size);
            T result = Marshal.PtrToStructure<T>(ptr);
            Marshal.FreeHGlobal(ptr);
            return result;
        }
    }
}