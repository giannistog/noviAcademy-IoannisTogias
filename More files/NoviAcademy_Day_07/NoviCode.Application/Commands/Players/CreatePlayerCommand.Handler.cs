using MediatR;
using NoviCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Commands.Players
{
    public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Guid>
    {
        private readonly ICreatePlayerPersistence _createPlayerPersistence;

        public CreatePlayerCommandHandler(ICreatePlayerPersistence persistence)
        {
            _createPlayerPersistence = persistence;
        }

        public async Task<Guid> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        {
            var player = Player.CreateNew(request.name);
            
            await _createPlayerPersistence.Persist(player);
            return player.Id;
        }
    }
}
