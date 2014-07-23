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
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.ApplicationInsights() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationApplicationInsightsExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events against Microsoft Application Insights for the provided component id.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="applicationInsightsComponentId">The ID that determines the application component under which your data appears in Application Insights.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">loggerConfiguration</exception>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration ApplicationInsights(
            this LoggerSinkConfiguration loggerConfiguration,
            string applicationInsightsComponentId,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            if (applicationInsightsComponentId == null) throw new ArgumentNullException("applicationInsightsComponentId");
            if (string.IsNullOrWhiteSpace(applicationInsightsComponentId)) throw new ArgumentOutOfRangeException("applicationInsightsComponentId", "Cannot be empty.");

            return loggerConfiguration.Sink(
                new ApplicationInsightsSink(applicationInsightsComponentId, formatProvider),
                restrictedToMinimumLevel);
        }
    }
}
