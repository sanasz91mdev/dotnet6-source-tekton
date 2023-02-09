﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DigitalBanking.ServiceExtensions
{
    public class SampleHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                                   HealthCheckResult.Healthy("Service is healthy."));
        }
    }
}
