using BL_BedeLottery.Classes;
using BL_BedeLottery.Enums;

namespace Test_BL_BedeLottery
{
    /// <summary>
    /// Test suite for the <see cref="Ticket"/> class.
    /// This suite includes tests to validate the behavior of the Ticket class. The tests cover a variety of scenarios including:
    /// - Correct ticket initialization.
    /// - Proper handling of winnings and player balance updates.
    /// - Multiple winner types (Grand, Second, Third).
    /// - Ensuring that a ticket cannot be marked as a winner more than once.
    /// - Edge cases such as zero or negative winnings, large winnings, and multiple tickets for the same player.
    /// The goal is to ensure that the Ticket class behaves as expected under normal and extreme conditions.
    /// </summary>
    public class TicketTests
    {
        // Verifies that the Ticket constructor correctly initializes the ticket with the provided player.
        [Fact]
        public void Constructor_ShouldInitializeWithCorrectPlayer()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            Assert.Equal(player, ticket.Player);
        }

        // Verifies that the ticket is correctly marked as a winner and that the winnings are properly assigned
        // to the ticket and the player's balance.
        [Fact]
        public void MarkWinner_ShouldMarkTicketAsWinner()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = 50.00m;
            ticket.SetWinner(WinnerType.Grand, winnings);
            Assert.True(ticket.IsWinner);
            Assert.Equal(winnings, ticket.Winnings);
            Assert.Equal(150.00m, player.Balance);
        }

        // Verifies that once a ticket has been marked as a winner, it cannot be marked again with a new winner type.
        [Fact]
        public void MarkWinner_ShouldNotAllowMultipleMarking()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings1 = 50.00m;
            decimal winnings2 = 100.00m;
            ticket.SetWinner(WinnerType.Grand, winnings1);
            ticket.SetWinner(WinnerType.Second, winnings2);
            Assert.True(ticket.IsWinner);
            Assert.Equal(winnings1, ticket.Winnings);
            Assert.Equal(150.00m, player.Balance);
        }

        // Verifies that when a ticket is marked as a "Grand" winner, the corresponding property is set to true
        // and the winnings are properly assigned.
        [Fact]
        public void SetWinner_ShouldMarkAsGrandWinner()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = 1000.00m;
            ticket.SetWinner(WinnerType.Grand, winnings);
            Assert.True(ticket.IsGrandWinner);
            Assert.Equal(winnings, ticket.Winnings);
        }

        // Verifies that when a ticket is marked as a "Second" winner, the corresponding property is set to true
        // and the winnings are properly assigned.
        [Fact]
        public void SetWinner_ShouldMarkAsSecondWinner()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = 500.00m;
            ticket.SetWinner(WinnerType.Second, winnings);
            Assert.True(ticket.IsSecondWinner);
            Assert.Equal(winnings, ticket.Winnings);
        }

        // Verifies that when a ticket is marked as a "Third" winner, the corresponding property is set to true
        // and the winnings are properly assigned.
        [Fact]
        public void SetWinner_ShouldMarkAsThirdWinner()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = 200.00m;
            ticket.SetWinner(WinnerType.Third, winnings);
            Assert.True(ticket.IsThirdWinner);
            Assert.Equal(winnings, ticket.Winnings);
        }

        // Verifies that each ticket gets a unique GUID as its ID, ensuring that two different tickets do not share the same ID.
        [Fact]
        public void Ticket_ShouldHaveUniqueGuidId()
        {
            Player player1 = new Player(1, "Player One", 100.00m);
            Player player2 = new Player(2, "Player Two", 100.00m);
            Ticket ticket1 = new Ticket(player1);
            Ticket ticket2 = new Ticket(player2);
            Assert.NotEqual(ticket1.Id, ticket2.Id);
        }

        // Verifies that the player's balance is updated correctly when a winner is set, and ensures that
        // the winnings are reflected in the player's balance.
        [Fact]
        public void SetWinner_ShouldUpdatePlayerBalance()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = 250.00m;
            ticket.SetWinner(WinnerType.Grand, winnings);
            Assert.Equal(350.00m, player.Balance);
        }

        // Verifies that the player's balance only reflects the winnings of the first "SetWinner" call,
        // and subsequent calls for different winner types are ignored for winnings after the first.
        [Fact]
        public void SetWinner_ShouldAccumulateBalanceAfterMultipleWins()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal grandPrize = 1000.00m;
            decimal secondPrize = 500.00m;
            decimal thirdPrize = 200.00m;
            ticket.SetWinner(WinnerType.Grand, grandPrize);
            ticket.SetWinner(WinnerType.Second, secondPrize); // Ignored
            ticket.SetWinner(WinnerType.Third, thirdPrize); // Ignored
            Assert.Equal(grandPrize, ticket.Winnings);
            Assert.Equal(1100.00m, player.Balance);
        }

        // Verifies the behavior when the winnings are set to zero, ensuring the player's balance does not change.
        [Fact]
        public void SetWinner_ShouldHandleZeroWinnings()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = 0.00m;
            ticket.SetWinner(WinnerType.Grand, winnings);
            Assert.True(ticket.IsWinner);
            Assert.Equal(winnings, ticket.Winnings);
            Assert.Equal(100.00m, player.Balance); // Balance should not change
        }

        // Verifies the behavior when negative winnings are set, ensuring the player's balance is properly decreased.
        [Fact]
        public void SetWinner_ShouldHandleNegativeWinnings()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = -50.00m;
            ticket.SetWinner(WinnerType.Grand, winnings);
            Assert.True(ticket.IsWinner);
            Assert.Equal(winnings, ticket.Winnings); // Negative winnings
            Assert.Equal(50.00m, player.Balance); // Player's balance should be decreased
        }

        // Verifies the behavior when large winnings are set, ensuring that large numbers are handled correctly without overflow.
        [Fact]
        public void SetWinner_ShouldHandleLargeWinnings()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = 1000000.00m; // Large winnings value
            ticket.SetWinner(WinnerType.Grand, winnings);
            Assert.Equal(1000100.00m, player.Balance); // Player's balance should be updated with large amount
        }

        // Verifies the behavior when a player with zero balance wins, ensuring that their balance is correctly updated.
        [Fact]
        public void SetWinner_ShouldHandleZeroBalancePlayer()
        {
            Player player = new Player(1, "Test Player", 0.00m);
            Ticket ticket = new Ticket(player);
            decimal winnings = 100.00m;
            ticket.SetWinner(WinnerType.Grand, winnings);
            Assert.Equal(100.00m, player.Balance); // Player's balance should be updated correctly
        }

        // Verifies that multiple tickets for the same player are handled correctly, with the player’s balance accumulating the winnings.
        [Fact]
        public void SetWinner_ShouldHandleMultipleTicketsForSamePlayer()
        {
            Player player = new Player(1, "Test Player", 100.00m);
            Ticket ticket1 = new Ticket(player);
            Ticket ticket2 = new Ticket(player);
            decimal winnings1 = 500.00m;
            decimal winnings2 = 300.00m;

            ticket1.SetWinner(WinnerType.Grand, winnings1);
            ticket2.SetWinner(WinnerType.Second, winnings2);

            Assert.Equal(900.00m, player.Balance); // Both tickets should accumulate balance
        }
    }
}
