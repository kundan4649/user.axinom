using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using z5.ms.common.extensions;
using z5.ms.common.helpers;
using Timer = System.Timers.Timer;

namespace z5.ms.common.infrastructure.logging
{
    /// <summary>A client for sending log entries to an elastic stack instance</summary>
    public interface IElasticStackLogClient : IDisposable
    {
        /// <summary>Enqueue a log entry for sending</summary>
        void Send(ElasticStackLogEntry logEntry, bool noFlush = false);

        /// <summary>Send all enqueue log entries</summary>
        Task Flush();
    }

    /// <summary>Configuration details required to create an elastic stack log client</summary>
    public class ElasticStackLogClientConfiguration
    {
        /// <summary>Url of the elastic stack instance</summary>
        public string ElasticStackUrl { get; set; }

        /// <summary>Username for user account write access</summary>
        public string ElasticStackUsername { get; set; }

        /// <summary>Password for user account with write access</summary>
        public string ElasticStackPassword { get; set; }

        /// <summary>Project name for log entries (customer name)</summary>
        public string LogProjectName { get; set; }

        /// <summary>Producer name for log entries (e.g. MS)</summary>
        public string LogProducerName { get; set; }

        /// <summary>Component name for log entries (e.g. serverless-azure)</summary>
        public string LogComponentName { get; set; }

        /// <summary>Environment name for log entries (e.g. CB)</summary>
        public string LogEnvironmentName { get; set; }

        /// <summary>Minimum log level to post to elastic stack</summary>
        public LogLevel ElasticStackMinLogLevel { get; set; } = LogLevel.Information;
    }

    /// <summary>A log entry to be sent to elastic stack</summary>
    public class ElasticStackLogEntry
    {
        /// <summary>Timestamp for the original log entry</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>Log level, error, info etc.</summary>
        public LogLevel Level { get; set; }

        /// <summary>The log message</summary>
        public string Message { get; set; }

        /// <summary>Name of the originating logger</summary>
        public string Logger { get; set; }

        /// <summary>Exception information associated with the log entry</summary>
        public Exception Exception { get; set; }
    }

    /// <inheritdoc />
    public class ElasticStackLogClient : IElasticStackLogClient
    {
        private const int MaxQueueLength = 100;
        private const int MaxQueuePeriod = 5000;
        private readonly ElasticStackLogClientConfiguration _config;
        private readonly ILogger _errorLog;
        private readonly AuthenticationHeaderValue _authHeader;
        private static ElasticStackLogClient _instance;
        private readonly string _version;
        private readonly ConcurrentQueue<ElasticStackLogEntry> _logEntries = new ConcurrentQueue<ElasticStackLogEntry>();
        private readonly object _queueLock = new object();
        private readonly Timer _flushScheduler;
        private readonly object _schedulerLock = new object();
        private int _scheduleQueueCount;
        private DateTime? _scheduleQueueExpiry;
        private static readonly object InstanceLock = new object();

        /// <summary>Get or create a static instance</summary>
        public static IElasticStackLogClient GetInstance(ElasticStackLogClientConfiguration config, ILogger errorLog = null, CancellationToken? token = null)
        {
            lock (InstanceLock)
            {
                _instance = _instance ?? (_instance = new ElasticStackLogClient(config, errorLog));
            }

            token?.Register(() =>
            {
                _instance._errorLog.LogInformation("Cancellation token received! Will try to flush logs before shutdown");
                _instance.Flush().Wait();
                _instance._errorLog.LogInformation("Flushed shutdown logs successfully");
            });
            
            return _instance;
        }
        
        /// <inheritdoc />
        public ElasticStackLogClient(ElasticStackLogClientConfiguration config, ILogger errorLog = null)
        {
            _config = config;
            _errorLog = errorLog;
            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            var authString = $"{_config.ElasticStackUsername}:{_config.ElasticStackPassword}";
            var bytes = Encoding.UTF8.GetBytes(authString);
            _authHeader = AuthenticationHeaderValue.Parse($"Basic {Convert.ToBase64String(bytes)}");

            _flushScheduler = new Timer();
            _flushScheduler.Elapsed += (o,e) => RunFlushScheduler(false);
            _flushScheduler.Interval = MaxQueuePeriod;
            _flushScheduler.Enabled = true;
        }
        
        /// <inheritdoc />
        public void Send(ElasticStackLogEntry logEntry, bool noFlush = false)
        {
            if (logEntry.Level < _config.ElasticStackMinLogLevel)
                return;

            _logEntries.Enqueue(logEntry);

            if (noFlush)
                return;

            RunFlushScheduler(true);
        }

        /// <inheritdoc />
        public async Task Flush()
        {
            var body = "";

            lock (_queueLock)
            {
                while (_logEntries.TryDequeue(out var logEntry))
                {
                    body += JsonConvert.SerializeObject(ToIndexCommand(_config, logEntry), Formatting.None) + "\n"
                         + JsonConvert.SerializeObject(ToDocument(_config, _version, logEntry), Formatting.None) + "\n";
                }
            }

            if (body == "")
                return;

            var uri = $"{_config.ElasticStackUrl?.TrimEnd('/')}/_bulk";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
            };
            request.Headers.Authorization = _authHeader;

            try
            {
                var result = await HttpHelpers.HttpClient.SendAsync(request).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                    _errorLog.LogError($"Error posting bulk logs to ElasticStack: {result.StatusCode}, {await result.Content.ReadAsStringAsync().ConfigureAwait(false)}");
            }
            catch (Exception e)
            {
                _errorLog.LogError($"Error posting bulk logs to ElasticStack: {e.Message}");
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _flushScheduler.Enabled = false;
            Flush().Wait();
        }

        /// <inheritdoc />
        ~ElasticStackLogClient()
        {
            _flushScheduler.Enabled = false;
            Flush().Wait();
        }

        private void RunFlushScheduler(bool increment)
        {
            lock (_schedulerLock)
            {
                if (increment)
                {
                    _scheduleQueueCount ++;
                    _scheduleQueueExpiry = _scheduleQueueExpiry ?? DateTime.UtcNow.AddMilliseconds(MaxQueuePeriod);
                }

                if (_scheduleQueueCount < MaxQueueLength && (_scheduleQueueExpiry == null || DateTime.UtcNow < _scheduleQueueExpiry))
                    return;

                _scheduleQueueCount = 0;
                _scheduleQueueExpiry = null;
            }

            Task.Run(async () => { await Flush().ConfigureAwait(false); });
        }
        
        private ElasticStackIndexCommand ToIndexCommand(ElasticStackLogClientConfiguration config, ElasticStackLogEntry le)
            => new ElasticStackIndexCommand($"{config.LogComponentName}-{le.Timestamp:yyyy-MM-dd}", "log");

        private ElasticStackLogEntryDocument ToDocument(ElasticStackLogClientConfiguration config, string version, ElasticStackLogEntry le)
            => new ElasticStackLogEntryDocument
            {
                Timestamp = le.Timestamp.ToZuluDateTimeString(),
                Level = le.Level.ToString().ToUpperInvariant(),
                Message = le.Message,
                Logger = le.Logger,
                Project = config.LogProjectName,
                Producer = config.LogProducerName,
                Component = config.LogComponentName,
                Environment = config.LogEnvironmentName,
                Version = version,
                Exception = le.Exception == null
                    ? null
                    : new ElasticStackLogEntryDocument.ExceptionLog
                    {
                        StackTrace = le.Exception.StackTrace,
                        Message = le.Exception.Message
                    },
            };
    }

    internal class ElasticStackIndexCommand
    {
        public ElasticStackIndexCommand(string index, string type)
        {
            Parameters = new BulkCommandParameters
            {
                Index = index,
                Type = type
            };
        }

        [JsonProperty("index")]
        public BulkCommandParameters Parameters { get; set; }

        public class BulkCommandParameters
        {
            [JsonProperty("_index")]
            public string Index { get; set; }

            [JsonProperty("_type")]
            public string Type { get; set; }
        }
    }

    internal class ElasticStackLogEntryDocument
    {
        [JsonProperty("@timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("logger")]
        public string Logger { get; set; }

        [JsonProperty("project")]
        public string Project { get; set; }

        [JsonProperty("producer")]
        public string Producer { get; set; }

        [JsonProperty("component")]
        public string Component { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("exception")]
        public ExceptionLog Exception { get; set; }

        public class ExceptionLog
        {
            public string StackTrace { get; set; }
            public string Message { get; set; }
        }
    }
}