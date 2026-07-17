using MediatR;
using NoviCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Commands.Players
{
    public class CreatePlayerCommandHandler:IStreamRequestHandler<CreatePlayerCommand>
    {
        private readonly ICreatePlayerPersistence _Persistence;
        public Task Handle(CreatePlayerCommand request, CancellationToken cancellationToken) {
            Persistence = _Persistence;
        }
    }
}
