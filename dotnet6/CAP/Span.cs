﻿using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Cap.OpenTelemetry
{
    //TODO: add proper logging in file

    public class Span
    {
        public string libName { get; set; }
        public string libVersion { get; set; }

        private Activity _activity { get; set; }

        public ILogger _logger;

        public Span(ILogger<Span> logger)
        {
            libName = "OpenTelemetry.Instrumentation.AspNetCore";
            libVersion = "1.0.0.0";
            _logger =  logger;
        }

        public Span(string lib, string version, ILogger<Span> logger)
        {
            libName = lib;
            libVersion = version;
            _logger = logger;

        }

        public TelemetrySpan getCurrentSpan()
        {
            return Tracer.CurrentSpan;
        }

        public SpanContext GetSpanContext()
        {
            return Tracer.CurrentSpan.Context;
        }

        public void start(string operationName)
        {
            try
            {
                _logger.LogInformation("Starting span in span lib");
                ActivitySource activitySource = new ActivitySource(
                libName,
                libVersion);

                _activity = activitySource.StartActivity(operationName);

                if (_activity != null)
                {
                    _activity?.Start();
                }
                else
                {
                     _logger.LogError("Unable to start span");
                }
            }
            catch (Exception ex) {
                _logger.LogError($"Unable to start span. Error: {ex.ToString()}");
            }
        }

        public void startWithContext(string operationName)
        {
            try
            {
                _logger.LogInformation("Starting span WITH context in span lib");
                ActivitySource activitySource = new ActivitySource(
                libName,
                libVersion);

                var currentSpan = getCurrentSpan();

                if (currentSpan != null)
                {
                    string traceID = currentSpan?.Context.TraceId.ToString();
                    string spanID = currentSpan?.Context.SpanId.ToString();

                    var parentContext = new ActivityContext(
                                ActivityTraceId.CreateFromString(traceID.AsSpan()),
                                ActivitySpanId.CreateFromString(spanID.AsSpan()),
                                ActivityTraceFlags.Recorded);
                    _activity = activitySource?.StartActivity(operationName, ActivityKind.Server, parentContext);

                    if (_activity != null)
                    {
                        _activity.Start();
                    }
                    else
                    {
                          _logger.LogError("Unable to start span with current trace context");
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError($"Unable to start span with current trace context {ex.ToString()}");

            }




        }

        public void setTag(string key, string value)
        {
            if (_activity != null)
            {
                _activity?.SetTag(key, value);
            }
            else
            {
                //   _logger.LogError("Unable to set tag to activity, [_activity] is null");
            }
        }

        public void addEventLog(string evenLogMessage)
        {
            if (_activity != null)
            {
                _activity?.AddEvent(new ActivityEvent(evenLogMessage));
            }
        }

        public void Stop(SpanStatus status, string message = null)
        {
            if (_activity != null)
            {
                _activity.SetStatus(status == SpanStatus.Ok ? Status.Ok : Status.Error);
                if (status.Equals(SpanStatus.Error))
                {
                    _activity?.SetTag("otel.status_code", "ERROR");
                    _activity?.SetTag("otel.status_description", message == null ? "Failed to perform operation" : message);
                }

                _activity?.Stop();
            }
            else
            {
                //       _logger.LogError("Unable to stop span.Activity [_activity] is null");
            }
        }



    }


    public enum SpanStatus
    {
        Ok,
        Error
    }


}