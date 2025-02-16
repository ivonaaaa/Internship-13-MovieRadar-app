using MediatR;

public record DeleteRatingReactionCommand(int id) : IRequest<bool>;
