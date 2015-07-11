﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Tests.Support;

namespace Serilog.Tests.Sinks.PeriodicBatching
{
    class InMemoryPeriodicBatchingSink : PeriodicBatchingSink
    {
        readonly TimeSpan _batchEmitDelay;
        readonly object _stateLock = new object();
        bool _isDisposed;
        bool _stopped;

        // Post-mortem only
        public bool WasCalledAfterDisposal { get; private set; }
        public IList<IList<LogEvent>> Batches { get; private set; }
 
        public InMemoryPeriodicBatchingSink(int batchSizeLimit, TimeSpan period, TimeSpan batchEmitDelay) : base(batchSizeLimit, period)
        {
            _batchEmitDelay = batchEmitDelay;
            Batches = new List<IList<LogEvent>>();
        }

        public void Stop()
        {
            lock (_stateLock)
            {
                _stopped = true;

                base.Dispose(true);
            }
        }

        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            lock (_stateLock)
            {
                if (_stopped)
                    return;

                if (_isDisposed)
                    WasCalledAfterDisposal = true;

                Thread.Sleep(_batchEmitDelay);
                Batches.Add(events.ToList());
            }
        }

        protected override void Dispose(bool disposing)
        {
 	         base.Dispose(disposing);
            _isDisposed = true;
        }
    }

    [TestFixture]
    public class PeriodicBatchingSinkTests
    {
        static readonly TimeSpan TinyWait = TimeSpan.FromMilliseconds(50);
        static readonly TimeSpan MicroWait = TimeSpan.FromMilliseconds(1);

        // Some very, very approximate tests here :)

        [Test]
        public void WhenAnEventIsEnqueuedItIsWrittenToABatch_OnFlush()
        {
            var pbs = new InMemoryPeriodicBatchingSink(2, TinyWait, TimeSpan.Zero);
            var evt = Some.InformationEvent();
            pbs.Emit(evt);
            pbs.Dispose();
            Assert.AreEqual(1, pbs.Batches.Count);
            Assert.AreEqual(1, pbs.Batches[0].Count);
            Assert.AreSame(evt, pbs.Batches[0][0]);
            Assert.IsFalse(pbs.WasCalledAfterDisposal);
        }

        [Test]
        public void WhenAnEventIsEnqueuedItIsWrittenToABatch_OnTimer()
        {
            var pbs = new InMemoryPeriodicBatchingSink(2, TinyWait, TimeSpan.Zero);
            var evt = Some.InformationEvent();
            pbs.Emit(evt);
            Thread.Sleep(TinyWait + TinyWait);
            pbs.Stop();
            Assert.AreEqual(1, pbs.Batches.Count);
            Assert.IsFalse(pbs.WasCalledAfterDisposal);
        }

        [Test]
        public void WhenAnEventIsEnqueuedItIsWrittenToABatch_FlushWhileRunning()
        {
            var pbs = new InMemoryPeriodicBatchingSink(2, MicroWait, TinyWait + TinyWait);
            var evt = Some.InformationEvent();
            pbs.Emit(evt);
            Thread.Sleep(TinyWait);
            pbs.Dispose();
            Assert.AreEqual(1, pbs.Batches.Count);
            Assert.IsFalse(pbs.WasCalledAfterDisposal);
        }
    }
}
