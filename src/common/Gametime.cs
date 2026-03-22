using System;
using System.Collections.Generic;
using System.Timers;

namespace CoTuongGame
{
    public enum TimerState { Stopped, Running, Paused }

    // Lưu 1 nước đi
    public class NuocDi
    {
        public DateTime Time;
        public string MoTa;
        public int[] From, To;
        public string QuanAn;

        public NuocDi(DateTime time, string moTa, int[] from, int[] to, string quanAn = "")
        {
            Time = time; MoTa = moTa; From = from; To = to; QuanAn = quanAn;
        }
    }

    public class GameTimer
    {
        private System.Timers.Timer _timer;
        private long _timeMs;
        private TimerState _state;
        private List<NuocDi> _lichSu;

        public event Action<long> TimeChanged;
        public event Action<NuocDi> NuocDiAdded;

        public long TimeMs => _timeMs;
        public TimerState State => _state;
        public int SoNuocDi => _lichSu.Count;

        public GameTimer()
        {
            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += (s, e) => { _timeMs += 100; TimeChanged?.Invoke(_timeMs); };
            _lichSu = new List<NuocDi>();
            _state = TimerState.Stopped;
        }

        // Điều khiển timer
        public void Start() { if (_state != TimerState.Running) { _state = TimerState.Running; _timer.Start(); } }
        public void Pause() { if (_state == TimerState.Running) { _state = TimerState.Paused; _timer.Stop(); } }
        public void Stop() { _state = TimerState.Stopped; _timer.Stop(); _timeMs = 0; TimeChanged?.Invoke(0); }

        // Quản lý nước đi
        public void ThemNuocDi(int x1, int y1, int x2, int y2, string quanAn = "")
        {
            string moTa = $"({x1},{y1})->({x2},{y2})";
            if (!string.IsNullOrEmpty(quanAn)) moTa += $" [Ăn {quanAn}]";
            
            var nuocDi = new NuocDi(DateTime.Now, moTa, new[] { x1, y1 }, new[] { x2, y2 }, quanAn);
            _lichSu.Add(nuocDi);
            NuocDiAdded?.Invoke(nuocDi);
        }

        public void Undo() 
        {
            if (_lichSu.Count > 0) _lichSu.RemoveAt(_lichSu.Count - 1);
        }

        public List<NuocDi> GetLichSu() => new List<NuocDi>(_lichSu);
    }
}
