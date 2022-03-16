using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;

namespace Serilog.Tests.Support
{
    static class Some
    {
        static int Counter;

        public static int Int() => Interlocked.Increment(ref Counter);

        public static decimal Decimal() => Int() + 0.123m;

        public static string String(string? tag = null) => (tag ?? "") + "__" + Int();

        public static TimeSpan TimeSpan() => System.TimeSpan.FromMinutes(Int());

        public static DateTime Instant() => new DateTime(2012, 10, 28) + TimeSpan();

        public static DateTimeOffset OffsetInstant() => new(Instant());

        public static LogEvent LogEvent(string sourceContext, DateTimeOffset? timestamp = null, LogEventLevel level = LogEventLevel.Information)
        {
            return new(timestamp ?? OffsetInstant(), level,
                null, MessageTemplate(),
                new List<LogEventProperty> { new(Constants.SourceContextPropertyName, new ScalarValue(sourceContext)) });
        }

        public static LogEvent LogEvent(DateTimeOffset? timestamp = null, LogEventLevel level = LogEventLevel.Information)
        {
            return new(timestamp ?? OffsetInstant(), level,
                null, MessageTemplate(), Enumerable.Empty<LogEventProperty>());
        }

        public static LogEvent InformationEvent(DateTimeOffset? timestamp = null)
        {
            return LogEvent(timestamp, LogEventLevel.Information);
        }

        public static LogEvent DebugEvent(DateTimeOffset? timestamp = null)
        {
            return LogEvent(timestamp, LogEventLevel.Debug);
        }

        public static LogEvent WarningEvent(DateTimeOffset? timestamp = null)
        {
            return LogEvent(timestamp, LogEventLevel.Warning);
        }

        public static LogEventProperty LogEventProperty()
        {
            return new(String(), new ScalarValue(Int()));
        }

        public static string NonexistentTempFilePath()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        }

        public static string TempFilePath() => Path.GetTempFileName();

        public static string TempFolderPath()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            return dir;
        }

        public static MessageTemplate MessageTemplate()
        {
            return new MessageTemplateParser().Parse(String());
        }
    }
}
