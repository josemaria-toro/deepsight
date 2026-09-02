using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Zetatech.DeepSight.Logging.Providers;

public sealed class DeepSightLoggerProvider : ILoggerProvider
{
    private Boolean _disposed;
    private readonly HttpClient _httpClient;
    private ConcurrentDictionary<String, DeepSightLogger> _loggers;
    private readonly IOptions<DeepSightLoggerOptions> _options;

    public DeepSightLoggerProvider(IOptions<DeepSightLoggerOptions> options)
    {
        _loggers = new ConcurrentDictionary<String, DeepSightLogger>();
        _options = options ?? throw new ArgumentException("The provided configuration options must be a valid instance", nameof(options));
        _httpClient = new HttpClient
        {
            BaseAddress = _options.Value.Uri,
            Timeout = TimeSpan.FromSeconds(1)
        };

        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!$"{_httpClient.BaseAddress}".EndsWith("/"))
        {
            _httpClient.BaseAddress = new Uri($"{_httpClient.BaseAddress}/");
        }
    }

    public ILogger CreateLogger(String category)
    {
        if (String.IsNullOrEmpty(category))
        {
            throw new ArgumentException("The provided category is invalid", nameof(category));
        }

        return _loggers.GetOrAdd(category, x => new DeepSightLogger(_options, x, _httpClient));
    }
    public void Dispose()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }

        _disposed = true;
        _loggers = null;

        GC.SuppressFinalize(this);
    }
}
