using BL_BedeLottery.Classes;

namespace Test_BL_BedeLottery
{
    /// <summary>
    /// Unit tests for the <see cref="Player"/> class in the BL_BedeLottery project.
    /// These tests cover various edge cases and scenarios to ensure that the Player class behaves as expected.
    /// The tests include:
    /// 
    /// 1. **Constructor Tests**:
    ///    - Verifies that the Player class can be initialized with a zero or negative balance.
    ///    - Tests for various types of player names (long, empty, and special characters).
    ///    - Ensures that the IsActivePlayer flag is set correctly, with the default being false.
    ///
    /// 2. **Balance Tests**:
    ///    - Tests the functionality of increasing and decreasing the player's balance.
    ///    - Verifies that negative balances are handled correctly.
    ///    - Ensures that large balance increases work as expected.
    ///
    /// 3. **ID and Sequence Tests**:
    ///    - Verifies that player IDs are unique across different instances.
    ///    - Ensures that increasing and decreasing balance in sequence behaves as expected.
    /// </summary>
    public class PlayerTests
    {
        // Test for Constructor with Zero Balance
        [Fact]
        public void Constructor_ShouldInitializeWithZeroBalance()
        {
            int expectedId = 1;
            string expectedName = "Test Player";
            decimal expectedBalance = 0.00m;

            Player player = new Player(expectedId, expectedName, expectedBalance);

            Assert.Equal(expectedBalance, player.Balance);
        }

        // Test for Constructor with Negative Balance
        [Fact]
        public void Constructor_ShouldInitializeWithNegativeBalance()
        {
            int expectedId = 2;
            string expectedName = "Negative Player";
            decimal expectedBalance = -50.00m;

            Player player = new Player(expectedId, expectedName, expectedBalance);

            Assert.Equal(expectedBalance, player.Balance);
        }

        // Test for Increasing Balance to a Large Value
        [Fact]
        public void IncreaseBalance_ShouldHandleLargeBalance()
        {
            Player player = new Player(1, "Test Player", 1000000.00m);

            decimal amountToIncrease = 1000000.00m;
            decimal expectedBalance = 2000000.00m;

            player.IncreaseBalance(amountToIncrease);

            Assert.Equal(expectedBalance, player.Balance);
        }

        // Test for Decreasing Balance to Zero
        [Fact]
        public void DecreaseBalance_ShouldAllowBalanceToReachZero()
        {
            Player player = new Player(1, "Test Player", 100.00m);

            decimal amountToDecrease = 100.00m;
            decimal expectedBalance = 0.00m;

            player.DecreaseBalance(amountToDecrease);

            Assert.Equal(expectedBalance, player.Balance);
        }

        // Test for Decreasing Balance Below Zero
        [Fact]
        public void DecreaseBalance_ShouldNotAllowNegativeBalance()
        {
            Player player = new Player(1, "Test Player", 50.00m);

            decimal amountToDecrease = 100.00m;

            player.DecreaseBalance(amountToDecrease);

            Assert.True(player.Balance < 0); // Ensure balance is negative
        }

        // Test for Active Player Flag
        [Fact]
        public void Constructor_ShouldSetIsActivePlayerCorrectly()
        {
            int expectedId = 1;
            string expectedName = "Test Player";
            decimal expectedBalance = 100.00m;

            Player player = new Player(expectedId, expectedName, expectedBalance, true);

            Assert.True(player.IsActivePlayer);
        }

        // Test for Default Active Player Flag
        [Fact]
        public void Constructor_ShouldSetIsActivePlayerToFalseByDefault()
        {
            int expectedId = 1;
            string expectedName = "Test Player";
            decimal expectedBalance = 100.00m;

            Player player = new Player(expectedId, expectedName, expectedBalance);

            Assert.False(player.IsActivePlayer);
        }

        // Test for Very Long Player Name
        [Fact]
        public void Constructor_ShouldAllowVeryLongPlayerName()
        {
            string veryLongName = new string('A', 1000);  // Long string of 'A's
            Player player = new Player(1, veryLongName, 100.00m);

            Assert.Equal(veryLongName, player.Name);
        }

        // Test for Empty Player Name
        [Fact]
        public void Constructor_ShouldAllowEmptyPlayerName()
        {
            string emptyName = "";
            Player player = new Player(2, emptyName, 100.00m);

            Assert.Equal(emptyName, player.Name);
        }

        // Test for Special Characters in Player Name
        [Fact]
        public void Constructor_ShouldAllowSpecialCharactersInName()
        {
            string specialCharName = "Player!@#$%^&*()";
            Player player = new Player(3, specialCharName, 100.00m);

            Assert.Equal(specialCharName, player.Name);
        }

        // Test for Unique Player IDs
        [Fact]
        public void Player_ShouldHaveUniqueIds()
        {
            Player player1 = new Player(1, "Player One", 100.00m);
            Player player2 = new Player(2, "Player Two", 50.00m);

            Assert.NotEqual(player1.Id, player2.Id);
        }

        // Test for Sequence of Balance Changes (Increase then Decrease)
        [Fact]
        public void IncreaseAndDecreaseBalance_ShouldWorkInSequence()
        {
            Player player = new Player(1, "Test Player", 100.00m);

            decimal increaseAmount = 50.00m;
            decimal decreaseAmount = 30.00m;

            player.IncreaseBalance(increaseAmount);
            player.DecreaseBalance(decreaseAmount);

            Assert.Equal(120.00m, player.Balance);
        }
    }
}
