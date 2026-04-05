using System;
using System.Collections.Generic;
using System.Timers;

namespace CoTuongGame
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

    public class GameTimer
    {
        public int PlayerId { get; set; } = 1;

        private readonly Timer gameTimer = new(100);
        private readonly Timer turnTimer = new(1000);

        private long gameTime;
        private int turnTime = 30;

        private readonly List<NuocDi> history = new();

        public event Action<long> GameTimeChanged;
        public event Action<int> CountdownChanged;
        public event Action CountdownEnd;
        public event Action<NuocDi> MoveAdded;
        public event Action<int> TurnChanged;

        public long GameTime => gameTime;
        public int TurnTime => turnTime;
        public TimerState State { get; private set; }

        public GameTimer()
        {
            gameTimer.Elapsed += (_, _) =>
            {
                gameTime += 100;
                GameTimeChanged?.Invoke(gameTime);
            };

            turnTimer.Elapsed += (_, _) => Tick();
        }

        //GAME
        public void Start()
        {
            State = TimerState.Running;
            gameTimer.Start();
            StartTurn();
        }

        public void Pause()
        {
            if (State != TimerState.Running) return;
            State = TimerState.Paused;
            gameTimer.Stop();
            turnTimer.Stop();
        }

        public void Resume()
        {
            if (State != TimerState.Paused) return;
            State = TimerState.Running;
            gameTimer.Start();
            turnTimer.Start();
        }

        public void Stop()
        {
            State = TimerState.Stopped;
            gameTimer.Stop();
            turnTimer.Stop();
        }

        public void Reset()
        {
            Stop();
            gameTime = 0;
            turnTime = 30;
            history.Clear();

            GameTimeChanged?.Invoke(gameTime);
            CountdownChanged?.Invoke(turnTime);
        }

        // luot di
        public void StartTurn()
        {
            turnTimer.Stop();
            turnTime = 30;
            turnTimer.Start();
            CountdownChanged?.Invoke(turnTime);
        }

        public void StopTurn() => turnTimer.Stop();

        public bool MyTurn(int id)
        {
            bool mine = PlayerId == id;
            if (mine) StartTurn();
            else StopTurn();

            TurnChanged?.Invoke(id);
            return mine;
        }

        private void Tick()
        {
            if (--turnTime > 0)
                CountdownChanged?.Invoke(turnTime);
            else
            {
                turnTimer.Stop();
                CountdownEnd?.Invoke();
            }
        }

        //SYNC
        public void SyncTime(long ms)
        {
            gameTime = ms;
            GameTimeChanged?.Invoke(ms);
        }

        public void SyncCountdown(int sec)
        {
            turnTime = sec;
            CountdownChanged?.Invoke(sec);
            if (sec > 0) turnTimer.Start();
        }

        // lichsu
        public void AddMove(int x1, int y1, int x2, int y2, string eat = "")
        {
            string text = $"({x1},{y1})->({x2},{y2})" + (eat != "" ? $" [Ăn {eat}]" : "");

            var move = new NuocDi(DateTime.Now, text, new[] { x1, y1 }, new[] { x2, y2 }, eat);
            history.Add(move);

            MoveAdded?.Invoke(move);
        }

        public void Undo()
        {
            if (history.Count > 0)
                history.RemoveAt(history.Count - 1);
        }

        public List<NuocDi> GetHistory() => history;
    }
}