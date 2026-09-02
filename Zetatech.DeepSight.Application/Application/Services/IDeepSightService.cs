using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application;

namespace Zetatech.DeepSight.Application.Services;

public interface IDeepSightService : IService
{
    Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default);
}