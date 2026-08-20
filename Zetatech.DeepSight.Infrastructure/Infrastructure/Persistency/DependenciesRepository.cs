using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Data;
using Zetatech.Accelerate.Data.Abstractions;
using Zetatech.DeepSight.Domain.Entities;
using Zetatech.DeepSight.Domain.Repositories;

namespace Zetatech.DeepSight.Infrastructure.Persistency;

public sealed class DependenciesRepository : BaseEntityFrameworkRepository<DependencyEntity>, IDependenciesRepository
{
    public DependenciesRepository(IOptions<EntityFrameworkRepositoryOptions> options) : base(options)
    {
    }
}
