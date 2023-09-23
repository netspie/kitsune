using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Authors;
using Mediator;

namespace Manabu.UseCases.Content.Authors;

public class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand, Result>
{
    private readonly IRepository<Author, AuthorId> _authorRepository;

    public CreateAuthorCommandHandler(IRepository<Author, AuthorId> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async ValueTask<Result> Handle(CreateAuthorCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var author = new Author(command.Name);
        await _authorRepository.Save(author, result);

        return result;
    }
}

public record CreateAuthorCommand(
    string Name) : ICommand<Result>;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand> { }
