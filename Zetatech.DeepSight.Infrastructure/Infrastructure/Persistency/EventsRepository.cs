using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Data;
using Zetatech.Accelerate.Data.Abstractions;
using Zetatech.DeepSight.Domain.Entities;
using Zetatech.DeepSight.Domain.Repositories;

namespace Zetatech.DeepSight.Infrastructure.Persistency;

public sealed class EventsRepository : BaseEntityFrameworkRepository<EventEntity>, IEventsRepository
{
    public EventsRepository(IOptions<EntityFrameworkRepositoryOptions> options) : base(options)
    {
    }
}
