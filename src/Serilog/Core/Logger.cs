﻿// Copyright 2013 Nicholas Blumhardt
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Parameters;

namespace Serilog.Core
{
    sealed class Logger : ILogger, ILogEventSink, IDisposable
    {
        public const string SourceContextPropertyName = "SourceContext";

        readonly MessageTemplateProcessor _messageTemplateProcessor;
        readonly LogEventLevel _minimumLevel;
        readonly ILogEventSink _sink;
        readonly Action _dispose;
        readonly ILogEventEnricher[] _enrichers;
        readonly IFormatProvider _formatProvider;

        public Logger(
            MessageTemplateProcessor messageTemplateProcessor,
            LogEventLevel minimumLevel,
            ILogEventSink sink,
            IEnumerable<ILogEventEnricher> enrichers,
            Action dispose = null,
            IFormatProvider formatProvider = null)
        {
            if (sink == null) throw new ArgumentNullException("sink");
            if (enrichers == null) throw new ArgumentNullException("enrichers");
            _messageTemplateProcessor = messageTemplateProcessor;
            _minimumLevel = minimumLevel;
            _sink = sink;
            _dispose = dispose;
            _formatProvider = formatProvider;
            _enrichers = enrichers.ToArray();
        }

        public IFormatProvider FormatProvider
        {
            get { return _formatProvider; }
        }

        public ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
        {
            return new Logger(
                _messageTemplateProcessor,
                _minimumLevel, 
                this,
                (enrichers ?? new ILogEventEnricher[0]).ToArray());
        }

        public ILogger ForContext(string propertyName, object value, bool destructureObjects = false)
        {
            return ForContext(new[] {
                new FixedPropertyEnricher(
                    _messageTemplateProcessor.CreateProperty(propertyName, value, destructureObjects)) });
        }

        public ILogger ForContext(Type source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ForContext(SourceContextPropertyName, source.FullName);
        }

        public ILogger ForContext<TSource>()
        {
            return ForContext(typeof(TSource));
        }

        public void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        {
            Write(level, null, null, messageTemplate, propertyValues);
        }

        public void Write(LogEventLevel level, IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Write(level, null, formatProvider, messageTemplate, propertyValues);
        }

        public void Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Write(level, exception, null, messageTemplate, propertyValues);
        }

        public void Write(LogEventLevel level, Exception exception, IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            if (messageTemplate == null)
                return;

            if ((int)level < (int)_minimumLevel)
                return;

            // Catch a common pitfall when a single non-object array is cast to object[]
            // Needs some more thought
            if (propertyValues != null &&
                propertyValues.GetType() != typeof(object[]))
                propertyValues = new object[] { propertyValues };

            var now = DateTimeOffset.Now;

            MessageTemplate parsedTemplate;
            IEnumerable<LogEventProperty> properties;
            _messageTemplateProcessor.Process(messageTemplate, propertyValues, out parsedTemplate, out properties);

            var actualFormatProvider = formatProvider ?? _formatProvider;
            var logEvent = new LogEvent(now, level, exception, parsedTemplate, properties, actualFormatProvider);
            Write(logEvent);
        }

        public bool IsEnabled(LogEventLevel level)
        {
            return (int)level >= (int)_minimumLevel;
        }

        public void Write(LogEvent logEvent)
        {
            if (logEvent == null) return;

            foreach (var enricher in _enrichers)
            {
                try
                {
                    enricher.Enrich(logEvent, _messageTemplateProcessor);
                }
                catch (Exception ex)
                {
                    SelfLog.WriteLine("Exception {0} caught while enriching {1} with {2}.", ex, logEvent, enricher);
                }
            }

            _sink.Emit(logEvent);
        }


        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            Verbose(null, null, messageTemplate, propertyValues);
        }

        public void Verbose(IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Verbose(null, formatProvider, messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Verbose(exception, null, messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Write(LogEventLevel.Verbose, exception, formatProvider, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            Debug(null, null, messageTemplate, propertyValues);
        }

        public void Debug(IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Debug(null, formatProvider, messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Debug(exception, null, messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Write(LogEventLevel.Debug, exception, formatProvider, messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            Information(null, null, messageTemplate, propertyValues);
        }

        public void Information(IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Information(null, formatProvider, messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Information(exception, null, messageTemplate, propertyValues);
        }

        public void Information(Exception exception, IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Write(LogEventLevel.Information, exception, formatProvider, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            Warning(null, null, messageTemplate, propertyValues);
        }

        public void Warning(IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Warning(null, formatProvider, messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Warning(exception, null, messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Write(LogEventLevel.Warning, exception, formatProvider, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            Error(null, null, messageTemplate, propertyValues);
        }

        public void Error(IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Error(null, formatProvider, messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Error(exception, null, messageTemplate, propertyValues);
        }

        public void Error(Exception exception, IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Write(LogEventLevel.Error, exception, formatProvider, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            Fatal(null, null, messageTemplate, propertyValues);
        }

        public void Fatal(IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Fatal(null, formatProvider, messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Fatal(exception, null, messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, IFormatProvider formatProvider, string messageTemplate, params object[] propertyValues)
        {
            Write(LogEventLevel.Fatal, exception, formatProvider, messageTemplate, propertyValues);
        }

        public void Dispose()
        {
            if (_dispose != null)
                _dispose();
        }

        public void Emit(LogEvent logEvent)
        {
            Write(logEvent);
        }
    }
}
