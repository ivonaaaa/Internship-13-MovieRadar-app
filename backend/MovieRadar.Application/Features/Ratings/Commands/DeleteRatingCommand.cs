using MediatR;

public record DeleteRatingCommand(int id) : IRequest<bool>;
