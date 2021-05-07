﻿using Serilog.Core.Pipeline;
using Serilog.Tests.Support;
using Xunit;

namespace Serilog.Tests
{
    [Collection("Log.Logger")]
    public class LogTests
    {
        [Fact]
        public void TheUninitializedLoggerIsSilent()
        {
            // This test depends on being there first executed from
            // the collection.
            Assert.IsType<SilentLogger>(Log.Logger);
        }

        [Fact]
        public void DisposesTheLogger()
        {
            DisposableLogger disposableLogger = new();
            Log.Logger = disposableLogger;
            Log.CloseAndFlush();
            Assert.True(disposableLogger.Disposed);
        }

        [Fact]
        public void ResetsLoggerToSilentLogger()
        {
            Log.Logger = new DisposableLogger();
            Log.CloseAndFlush();
            Assert.IsType<SilentLogger>(Log.Logger);
        }
    }
}
