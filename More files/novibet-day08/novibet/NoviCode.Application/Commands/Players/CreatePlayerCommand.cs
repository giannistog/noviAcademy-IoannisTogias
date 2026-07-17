using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Commands.Players
{
    public record CreatePlayerCommand(string name, int score) : IRequest<Guid>;
}
