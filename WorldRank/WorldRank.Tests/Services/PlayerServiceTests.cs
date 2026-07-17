using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Services;
using WorldRank;
using WorldRank.Domain.Entities;

namespace WorldRank.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new();
        private readonly Mock<ICache> _cacheMock = new();
        private readonly PlayerService _sut;

        public PlayerServiceTests()
        {
            _sut = new PlayerService(_playerRepositoryMock.Object, _cacheMock.Object);
        }

        /*[Fact]
         public Task GetPlayerById_IdExists_ReturnsPlayer()
         {
             Arrange
             _cacheMock.Setup(mock=>mock.TryGet(It.IsAny<string>(),out ))
             int id = 1;
             string name = 'el arabi';
             var expectedPlayer = new Player(1, name);
             _playerRepositoryMock.Setup(mock => mock.GetPlayerById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAny();
              Act
             var player = await _sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);
             Assert

             Assert.NotNull(player);

             Assert.Equal(expectedPlayer.Name, player?.Name);
         }
     */
    }
}
