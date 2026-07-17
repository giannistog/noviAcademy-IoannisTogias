using NoviCode.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Test.Entities
{
    public class WalletTests
    {
        [Fact]
        public void Constructor_ProperInput_InitializedCorrectly()
        {
            //Arrange
            int id = 0;
            int playerId = 123;
            Currency currency=Currency.EUR;
            decimal balance = 123.1m;
            bool isBlocked = false;
            //Act
            var wallet = new Wallet(id,playerId,currency,balance,isBlocked);
            //Assert
            Assert.Equal(id, wallet.Id);
            Assert.Equal(playerId, wallet.PlayerId);
            Assert.Equal(currency, wallet.Currency);
            Assert.Equal(balance, wallet.Balance);
            Assert.Equal(isBlocked.wallet.IsBlocked);
        }
    }
}
