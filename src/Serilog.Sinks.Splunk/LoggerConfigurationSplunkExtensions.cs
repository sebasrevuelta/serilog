﻿// Copyright 2014 Serilog Contributors
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
using System.ComponentModel;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Splunk;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.Splunk() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationSplunkExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events as rolling files for consumption in a Splunk instance.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="splunkConnectionInfoInfo"></param>
        /// <param name="batchSizeLimit"></param>
        /// <param name="defaultPeriod"></param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        [Obsolete("Please use the concrete SplunkConnectionInfo class instead of ISplunkConnectionInfo."), EditorBrowsable(EditorBrowsableState.Never)]
        public static LoggerConfiguration Splunk(
            this LoggerSinkConfiguration loggerConfiguration,
#pragma warning disable 618
            ISplunkConnectionInfo splunkConnectionInfoInfo,
#pragma warning restore 618
            int batchSizeLimit,
            TimeSpan? defaultPeriod,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            var defaultedPeriod = defaultPeriod ?? SplunkSink.DefaultPeriod;

            return loggerConfiguration.Sink(new SplunkSink(batchSizeLimit, defaultedPeriod, splunkConnectionInfoInfo), restrictedToMinimumLevel);
        }
        
        /// <summary>
        /// Adds a sink that writes log events as rolling files for consumption in a Splunk instance.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="splunkConnectionInfoInfo"></param>
        /// <param name="batchSizeLimit"></param>
        /// <param name="defaultPeriod"></param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration Splunk(
            this LoggerSinkConfiguration loggerConfiguration,
            SplunkConnectionInfo splunkConnectionInfoInfo,
            int batchSizeLimit,
            TimeSpan? defaultPeriod,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            var defaultedPeriod = defaultPeriod ?? SplunkSink.DefaultPeriod;

            return loggerConfiguration.Sink(new SplunkSink(batchSizeLimit, defaultedPeriod, splunkConnectionInfoInfo), restrictedToMinimumLevel);
        }
    }
}