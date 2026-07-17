using NoviCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Persistence.Commands.Players
{
    public class CreatePlayerPersistence : ICreatePlayerPersistence
    {
        private readonly AppDbContext _appDbContext;

        public CreatePlayerPersistence(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Persist(Player player)
        {
            _appDbContext.Add(player);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
