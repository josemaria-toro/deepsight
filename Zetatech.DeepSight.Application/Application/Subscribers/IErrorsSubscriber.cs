using Zetatech.Accelerate.Messaging;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Subscribers;

public interface IErrorsSubscriber : IMessageSubscriber<ErrorDto>
{
}