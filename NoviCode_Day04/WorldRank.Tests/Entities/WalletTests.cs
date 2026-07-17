
using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Test.Entities
{
    public class WalletTests
    {
        [Fact(DisplayName ="Verify that Constructor initialized correctly")]
        public void Constructor_ProperInput_InitializedCorrectly()
        {
            //Arrange
            int id = 0;
            int playerId = 123;
            Currency currency = Currency.EUR;
            decimal balance = 123.1m;
            bool isBlocked = false;
            //Act
            var wallet = new Wallet(id, playerId, currency, balance, isBlocked);
            //Assert
            Assert.Equal(id, wallet.Id);
            Assert.Equal(playerId, wallet.PlayerId);
            Assert.Equal(currency, wallet.Currency);
            Assert.Equal(balance, wallet.Balance);
            Assert.Equal(isBlocked,wallet.IsBlocked);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(50)]
        
        public void Deposit_NotPositiveAmount_ThrowInvalidAmountException(int amount)
        {
            //Arrange
            int id = 0;
            int playerId = 123;
            Currency currency = Currency.EUR;
            decimal balance = 123.1m;
            bool isBlocked = false;
            var wallet=new Wallet(id,playerId,currency,balance,isBlocked);

            //Act
            var action = () => wallet.Deposit(amount);

            //Assert
            Assert.Throws<InvalidAmountException>(action);
        }


    }
}
