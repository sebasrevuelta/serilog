﻿using System;
using Microsoft.WindowsAzure.Storage;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.AzureTableStorage;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.AzureTableStorage() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationAzureTableStorageExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events as records in the 'LogEventEntity' Azure Table Storage table in the given storage account.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="storageAccount">The Cloud Storage Account to use to insert the log entries to</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration AzureTableStorage(this LoggerSinkConfiguration loggerConfiguration,
            CloudStorageAccount storageAccount, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (storageAccount == null) throw new ArgumentNullException("storageAccount");
            return loggerConfiguration.Sink(new AzureTableStorageSink(storageAccount), restrictedToMinimumLevel);
        }
    }
}
