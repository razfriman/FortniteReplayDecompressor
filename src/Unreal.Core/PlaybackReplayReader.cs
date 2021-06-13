using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;
using Unreal.Core.Models.Playback;

namespace Unreal.Core
{
    public abstract class PlaybackReplayReader<T> : ReplayReader<T> where T : Replay, new()
    {
        public double AverageFPS { get; private set; } = 0;
        public int BufferCount => _buffer.Count;
        public int FPSLimit { get; private set; } = 60;
        public double BufferLengthMs { get; private set; } = 20000;
        public double PlaybackSpeed { get; private set; } = 100;
        public double CurrentTime { get; private set; }

        /// <summary>
        /// Current time of latest event update
        /// </summary>
        public double LastUpdateTime { get; private set; }

        //Used to pause/unpause reader
        private ManualResetEvent _pauseWaiter = new ManualResetEvent(false);

        //Used to limit buffer size
        private AutoResetEvent _bufferWaiter = new AutoResetEvent(false);

        //Whether or not the replay has been fully read
        private bool _finished = false;

        //Current time of latest read packet
        private float _currentReadTimeSeconds = 0;

        //Buffer used to hold playback events
        private ConcurrentQueue<ReplayPlaybackEvent> _buffer = new ConcurrentQueue<ReplayPlaybackEvent>();
        private System.Timers.Timer _bufferTimer = new System.Timers.Timer();

        private bool _firstPacket = true;
        private Stopwatch _timePassed = new Stopwatch();

        public event EventHandler<T> OnRender;

        public PlaybackReplayReader(ILogger logger) : base(logger)
        {
            _bufferTimer.Elapsed += _bufferTimer_Elapsed;
            _bufferTimer.Interval = 100;
            _bufferTimer.Start();
        }

        private void _bufferTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if(CurrentTime + (BufferLengthMs / 2) >= _currentReadTimeSeconds * 1000)
            {
                _bufferWaiter.Set();
            }
        }

        public void ReadReplay(string fileName, ParseType parseType = ParseType.Full)
        {
            using var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            
            ReadReplay(stream, parseType);
        }

        public void ReadReplay(Stream stream, ParseType parseType = ParseType.Full)
        {
            _finished = false;
            _firstPacket = true;
            _pauseWaiter.Reset();
            _timePassed.Restart();
            _currentReadTimeSeconds = 0;
            ClearBuffer();

            using var archive = new BinaryReader(stream);

            Thread thread = new Thread(UpdateThread);
            thread.IsBackground = true;
            thread.Start();

            ReadReplay(archive, parseType);

            _finished = true;

            //Wait for thread to finish
            thread.Join();
        }

        /// <summary>
        /// Sets playback speed of replay file.
        /// </summary>
        /// <param name="speed"></param>
        public void SetPlaybackSpeed(double speed = 1)
        {
            PlaybackSpeed = speed;
        }

        /// <summary>
        /// Pauses playback of replay file
        /// </summary>
        public void Pause()
        {
            _pauseWaiter.Set();
            _timePassed.Stop();
        }

        /// <summary>
        /// Unpauses playback of replay file
        /// </summary>
        public void UnPause()
        {
            _pauseWaiter.Set();
            _timePassed.Start();
        }

        /// <summary>
        /// Stops replay completely
        /// </summary>
        public void Stop()
        {
            _finished = true;

            _pauseWaiter.Set();
            _bufferWaiter.Set();
            ClearBuffer();
        }

        /// <summary>
        /// Goes to specific time in replay file
        /// </summary>
        /// <param name="time"></param>
        public void GoToTime(TimeSpan time)
        {
            throw new NotImplementedException();

            //TODO
            //Write code in ReplayReader to go to start from a checkpoint
        }

        /// <summary>
        /// Sets maximum times Render can be called within a second
        /// </summary>
        /// <param name="fps"></param>
        public void SetFPSLimit(int fps = 30)
        {
            if(fps <= 0)
            {
                throw new ArgumentException($"{nameof(fps)} needs to be > 0");
            }

            FPSLimit = fps;
        }

        public void SetBufferLength(double milliseconds)
        {
            if (milliseconds <= 0)
            {
                throw new ArgumentException($"{nameof(milliseconds)} needs to be > 0");
            }

            BufferLengthMs = milliseconds;
        }

        private void UpdateThread()
        {
            //Wait until first packet is read
            _pauseWaiter.WaitOne();

            double lastLoopMs = 0;
            int _frames = 0;
            double _startTime = 0;

            while (!_finished || _buffer.Count > 0)
            {
                double deltaTime = (_timePassed.Elapsed.TotalMilliseconds - lastLoopMs) * PlaybackSpeed;
                CurrentTime += deltaTime;
                lastLoopMs = _timePassed.Elapsed.TotalMilliseconds;

                double loopStartMs = CurrentTime;
                double msPerTick = 1000d / FPSLimit;

                ReadQueue(loopStartMs);

                //Catch up on updates
                while (loopStartMs + msPerTick < _timePassed.Elapsed.TotalMilliseconds)
                {
                    loopStartMs += msPerTick;

                    ReadQueue(loopStartMs);
                }

                OnRender?.Invoke(this, Replay);

                ++_frames;

                //Updates average FPS
                if (_timePassed.ElapsedMilliseconds - _startTime > 500)
                {
                    AverageFPS = (float)_frames / (_timePassed.Elapsed.TotalMilliseconds - _startTime) * 1000;
                    _startTime = _timePassed.Elapsed.TotalMilliseconds;
                    _frames = 0;
                }

                //Wait, if needed. Accurate enough
                TimeSpan waitTime = TimeSpan.FromMilliseconds((lastLoopMs + msPerTick) - _timePassed.Elapsed.TotalMilliseconds);

                if (waitTime.TotalMilliseconds > 0)
                {
                    Thread.Sleep(waitTime);
                }

                //Wait when paused
                _pauseWaiter.WaitOne();
            }
        }

        private void ReadQueue(double untilTime)
        {
            while(_buffer.TryPeek(out ReplayPlaybackEvent playbackEvent) && playbackEvent.Time < untilTime / 1000)
            {
                if (_buffer.TryDequeue(out playbackEvent))
                {
                    Update(playbackEvent);

                    LastUpdateTime = playbackEvent.Time;
                }
            }
        }

        private void ClearBuffer()
        {
#if NET5_0_OR_GREATER
            _buffer.Clear();
#else
            while(_buffer.TryDequeue(out var _))
            {

            }
#endif
        }
        protected override void ReceivedRawPacket(PlaybackPacket packet)
        {
            _currentReadTimeSeconds = packet.TimeSeconds;

            base.ReceivedRawPacket(packet);

            if (_firstPacket)
            {
                _firstPacket = false;
                _timePassed.Start();
                _pauseWaiter.Set();
            }
        }

        protected override IEnumerable<PlaybackPacket> ReadDemoFrameIntoPlaybackPackets(BinaryReader archive)
        {
            //Limits the buffer size by time
            IEnumerable<PlaybackPacket> packets = base.ReadDemoFrameIntoPlaybackPackets(archive);

            //Used to stop during playback
            if (!_finished)
            {
                PlaybackPacket packet = packets.FirstOrDefault();

                if (packet != null && packet.TimeSeconds * 1000 > LastUpdateTime * 1000 + BufferLengthMs)
                {
                    _bufferWaiter.WaitOne();
                }
            }

            return packets;
        }

        protected override bool ContinueParsingChannel(INetFieldExportGroup exportGroup)
        {
            //Parse everything
            return true;
        }

        protected override void OnChannelActorRead(uint channel, Actor actor)
        {
            _buffer.Enqueue(new ActorReadPlaybackEvent
            {
                Time = _currentReadTimeSeconds,
                Channel = channel,
                Actor = actor
            });
        }

        protected override void OnChannelClosed(uint channel)
        {
            _buffer.Enqueue(new ChannelClosedPlaybackEvent
            {
                Time = _currentReadTimeSeconds,
                Channel = channel
            });
        }

        protected override void OnExportRead(uint channel, INetFieldExportGroup exportGroup)
        {
            _buffer.Enqueue(new ExportGroupPlaybackEvent
            {
                Time = _currentReadTimeSeconds,
                Channel = channel,
                ExportGroup = exportGroup
            });
        }

        protected override void OnNetDeltaRead(NetDeltaUpdate deltaUpdate)
        {
            _buffer.Enqueue(new NetDeltaPlaybackEvent
            {
                Time = _currentReadTimeSeconds,
                DeltaUpdate = deltaUpdate
            });
        }

        /// <summary>
        /// Used to update current game state
        /// </summary>
        /// <param name="playbackEvent">Event causing this update</param>
        protected abstract void Update(ReplayPlaybackEvent playbackEvent);
    }
}
