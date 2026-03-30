using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;


namespace CoTuongOnline.Common
{
    public enum TimerState { Stopped, Running, Paused }

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

    public class GameTimer : IDisposable
    {
        private System.Timers.Timer _gameTimer = new(100), _countdownTimer = new(1000);
        private long _gameTimeMs;
        private int _countdownSeconds = 30;
        private bool _countdownRunning;
        private List<NuocDi> _lichSu = new();
        private TimerState _state = TimerState.Stopped;

        public event Action<long> GameTimeChanged;
        public event Action<int> CountdownChanged;
        public event Action CountdownEnd;
        public event Action<NuocDi> NuocDiAdded;

        public long GameTimeMs => _gameTimeMs;
        public int CountdownSeconds => _countdownSeconds;
        public bool CountdownRunning => _countdownRunning;
        public TimerState State => _state;

        public GameTimer()
        {
            _gameTimer.Elapsed += (s, e) => { 
    Interlocked.Add(ref _gameTimeMs, 100); 
    GameTimeChanged?.Invoke(_gameTimeMs); 
};
            _countdownTimer.Elapsed += OnCountdownTick;
        }

        public void StartGame() { _state = TimerState.Running; _gameTimer.Start(); StartCountdown(); }
        public void StartTurn() { _countdownSeconds = 30; StartCountdown(); CountdownChanged?.Invoke(30); }
        public void StopCountdown() { _countdownRunning = false; _countdownTimer.Stop(); }

        public void ThemNuocDi(int x1, int y1, int x2, int y2, string quanAn = "")
        {
            string moTa = $"({x1},{y1})->({x2},{y2})" + (quanAn != "" ? $" [Ăn {quanAn}]" : "");
            var nuocDi = new(DateTime.Now, moTa, new[] { x1, y1 }, new[] { x2, y2 }, quanAn);
            _lichSu.Add(nuocDi); NuocDiAdded?.Invoke(nuocDi);
        }

        public void Undo() => _lichSu.RemoveAt(_lichSu.Count - 1);

        private void StartCountdown() { _countdownRunning = true; _countdownTimer.Start(); }
        private void OnCountdownTick(object s, ElapsedEventArgs e)
        {
            if (_countdownSeconds > 0) { _countdownSeconds--; CountdownChanged?.Invoke(_countdownSeconds); }
            else { StopCountdown(); CountdownEnd?.Invoke(); }
        }

        public void StopAll()
        {
            _state = TimerState.Stopped; _gameTimer.Stop(); _countdownTimer.Stop();
            _gameTimeMs = 0; _countdownSeconds = 30;
        }

        public void Pause()
        {
            if (_state == TimerState.Running)
            {
                _state = TimerState.Paused;
                _gameTimer.Stop();
                StopCountdown();
            }
        }

        public List<NuocDi> GetLichSu() => new List<NuocDi>(_lichSu);

        public void Dispose()
{
    StopAll();
    _gameTimer.Dispose();
    _countdownTimer.Dispose();
}
    }
}
