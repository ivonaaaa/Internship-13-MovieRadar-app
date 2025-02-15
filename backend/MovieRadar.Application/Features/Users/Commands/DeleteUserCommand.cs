using MediatR;

public record DeleteUserCommand(int id) : IRequest<bool>;
