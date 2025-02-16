using MediatR;

public record DeleteRatingCommentCommand(int id) : IRequest<bool>;
