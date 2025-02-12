using MediatR;
using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Queries
{
    public class GetAll : IRequest<IEnumerable<User>>
    {
    }
}
