using NoviCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Persistence.Commands.Players
{
    public class CreatePlayersPersistenceCachingDecorator : ICreatePlayerPersistence
    {
        private readonly ICreatePlayerPersistence _inner;
        private readonly ICache _cache;

        private static string PlayerKey(Guid id) => $"player:{id}";
        private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);
        private const string AllPlayersKey = "players:all";


        public CreatePlayersPersistenceCachingDecorator(ICreatePlayerPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task Persist(Player player)
        {
            await _inner.Persist(player);

            _cache.Set(PlayerKey(player.Id), player, Ttl);
            _cache.Remove(AllPlayersKey);
        }
    }
}
