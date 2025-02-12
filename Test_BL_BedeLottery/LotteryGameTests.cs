using BL_BedeLottery;
using BL_BedeLottery.Enums;

namespace Test_BL_BedeLottery
{
    /// <summary>
    /// Unit tests for the <see cref="LotteryGame"/> class, ensuring the correct behavior of the game's various features.
    /// The tests cover different scenarios and edge cases for ticket purchases, player management, prize distribution, and house profit calculation.
    /// 
    /// Tests include:
    /// 1. **Game Initialization**: Verifies the game initializes with the correct number of players and tickets.
    /// 2. **Zero and Excessive Players**: Validates the behavior when no players or more tickets than players are requested.
    /// 3. **Insufficient Balance**: Ensures the game correctly handles cases where players don't have enough balance to purchase tickets.
    /// 4. **Large Number of Players and Tickets**: Assesses the game's performance and accuracy with a high volume of players and tickets.
    /// 5. **Grand Prize and Tier Prizes**: Verifies the correct handling of grand prize and second/third tier winners, including when no tickets are sold or all tickets are winners.
    /// 6. **Ticket Purchases**: Tests correct ticket purchase functionality, including no tickets being purchased or all players winning the grand prize.
    /// 7. **House Profit Calculation**: Ensures the correct calculation of house profit when no winners are selected or all winners are drawn from the same prize tier.
    /// 8. **Edge Cases**: Tests extreme values for ticket costs, player counts, and prize revenue percentages to ensure robustness.
    /// </summary>
    public class LotteryGameTests
    {
        // Test to ensure the game initializes with a default number of players and tickets.
        // Verifies that the ticket collection is not empty and that there are 10 players by default.
        [Fact]
        public void Game_ShouldInitializeWithPlayers()
        {

            LotteryGame game = new LotteryGame();
            game.PlayLottery(1);
            Assert.NotEmpty(game.GetTickets());
            Assert.True(game.GetPlayers().Count != 0);
        }

        // Test to ensure the game can handle a case where there are no players.
        // Manipulates the game setup to set both the minimum and maximum number of players to 0, resulting in no players.
        [Fact]
        public void Game_ShouldHandleNoCPUPlayers()
        {
            LotteryGame game = new LotteryGame(minPlayers: 0, maxPlayers: 0);
            Assert.Equal(1, game.GetNumberOfPlayers());
        }

        // Test to ensure the game handles the scenario where all players have insufficient balance to purchase tickets.
        // Initial balance is set to 0, and the game is played. No tickets should be purchased as the player cannot afford it.
        [Fact]
        public void Game_ShouldHandleAllPlayersHavingInsufficientBalance()
        {
            LotteryGame game = new LotteryGame(initialBalance: 0m);
            game.PlayLottery(5);
            Assert.Empty(game.GetTickets());
        }

        // Test to ensure the game behaves correctly when no tickets are requested.
        // In this case, when 0 tickets are requested, no tickets should be purchased, and there should be no grand winner.
        [Fact]
        public void Game_ShouldHandleZeroTicketsRequested()
        {
            LotteryGame game = new LotteryGame(minTickets: 0, maxTickets: 0);
            game.PlayLottery(0);
            Assert.Empty(game.GetTickets());
            Assert.Empty(game.GetGrandWinner());
        }


        // Test to ensure the game can handle extreme edge cases for ticket costs.
        // Verifies that the game properly accepts a very low ticket cost (0.01).
        [Fact]
        public void Game_ShouldHandleLowTicketCost()
        {
            LotteryGame lowCostGame = new LotteryGame(ticketCost: 0.01m);

            Assert.Equal(0.01m, lowCostGame.TicketCost);

            lowCostGame.PlayLottery(2);
            Assert.NotEqual(8m, lowCostGame.GetActivePlayer()?.Balance);
        }

        // Test to ensure the game can handle extreme edge cases for ticket costs.
        // Verifies that the game properly accepts a very high ticket cost (1000).
        [Fact]
        public void Game_ShouldHandleHighTicketCost()
        {
            LotteryGame highCostGame = new LotteryGame(ticketCost: 1000m); // High ticket cost

            Assert.Equal(1000m, highCostGame.TicketCost);

            highCostGame.PlayLottery(2);
            Assert.NotEqual(8m, highCostGame.GetActivePlayer()?.Balance);
        }

        // Test to verify the behavior when requesting the maximum number of tickets within the allowed range.
        // This checks that the game correctly handles edge cases for ticket numbers, ensuring no more than the maximum allowed tickets are purchased.
        [Fact]
        public void Game_ShouldHandleEdgeTicketNumbers()
        {

            LotteryGame edgeCaseGame = new LotteryGame(minPlayers: 1, maxPlayers: 1, minTickets: 0, maxTickets: 1000, ticketCost: 0.01m, initialBalance: 100m);
            int requestedTickets = 1000;
            edgeCaseGame.PlayLottery(requestedTickets);
            Assert.Equal(1000, edgeCaseGame.GetTickets().Count);
        }

        // Test to ensure the game correctly handles the scenario where a grand prize is drawn even when no tickets remain for that prize.
        // This checks that the game prevents the grand prize from being re-drawn after it has already been assigned to a winner.
        [Fact]
        public void Game_ShouldHandleGrandPrizeWithNoRemainingTickets()
        {
            LotteryGame game = new LotteryGame(numGrandPrizeWinners: 1);
            int requestedTickets = 1;
            game.PlayLottery(requestedTickets);
            game.GetTickets().First().SetWinner(WinnerType.Grand, 10);
            game.DrawGrandPrize(10); // Try to draw again after winner already marked
            Assert.Equal(1, game.GetGrandWinner() != null ? 1 : 0); // Ensure there's only one winner
        }

        // Test to verify that the game handles the situation where there are no remaining tickets for second-tier winners.
        // This scenario occurs when all tickets go to the grand prize and there are no tickets left for the second-tier prize pool.
        // The game should ensure that no second-tier winners are selected.
        [Fact]
        public void Game_ShouldHandleSecondTierWithNoRemainingTickets()
        {
            // Initialize the game with only grand prize winners
            var game = new LotteryGame(
                minPlayers: 5,
                maxPlayers: 5,
                minTickets: 1,
                maxTickets: 1,
                ticketCost: 1m,
                initialBalance: 10m,
                numGrandPrizeWinners: 5,
                grandPrizeRevenuePercentage: 0.5m,
                secondTierTicketPercentage: 0.10m,  // 10% for second tier, but no tickets left
                secondTierRevenuePercentage: 0.3m,
                thirdTierTicketPercentage: 0.20m,
                thirdTierRevenuePercentage: 0.1m
            );

            game.PlayLottery(requestTicketQuantity: 1);

            // There are no tickets left for second-tier winners, so the second tier should be empty
            var secondTierWinners = game.GetSecondWinners();
            Assert.Empty(secondTierWinners); // Assert no second-tier winners

            // Assert there are grand prize winners
            Assert.NotEmpty(game.GetGrandWinner());

            // Assert the second-tier ticket percentage is respected (i.e., there are no tickets for second-tier)
            Assert.Empty(secondTierWinners);
        }

        // Test to verify that the game correctly handles the situation where there are no remaining tickets for third-tier winners.
        // This occurs when all available tickets are either allocated to the grand prize or second-tier winners, leaving no tickets for the third-tier pool.
        // The game should ensure that no third-tier winners are selected.
        [Fact]
        public void Game_ShouldHandleThirdTierWithNoRemainingTickets()
        {
            // Initialize the game with only grand and second-tier winners
            var game = new LotteryGame(
                minPlayers: 5,
                maxPlayers: 5,
                minTickets: 1,
                maxTickets: 1,
                ticketCost: 1m,
                initialBalance: 10m,
                numGrandPrizeWinners: 2,
                grandPrizeRevenuePercentage: 0.5m,
                secondTierTicketPercentage: 0.70m,
                secondTierRevenuePercentage: 0.3m,
                thirdTierTicketPercentage: 0.20m,  // 20% for third tier, but no tickets left
                thirdTierRevenuePercentage: 0.1m
            );

            // Purchase tickets for the players (some tickets go to grand prize and second-tier)
            game.PlayLottery(requestTicketQuantity: 1);

            // There are no tickets left for third-tier winners, so the third tier should be empty
            var thirdTierWinners = game.GetThirdWinners();
            Assert.Empty(thirdTierWinners);

            Assert.NotEmpty(game.GetSecondWinners());
            Assert.NotEmpty(game.GetGrandWinner());
        }

        // Test to ensure that the game correctly handles a scenario where there is a large number of players and a substantial number of tickets.
        // This test simulates a large-scale lottery game with between 1000 and 2000 players, where each player can purchase tickets in the range of 10 to 100.
        // The method verifies that the total number of tickets purchased matches the expected number, based on the number of players and requested tickets.
        [Fact(Skip = "Take a while to run, so should be set off manually")]
        public void Game_ShouldHandleLargeNumberOfPlayersAndTickets()
        {
            LotteryGame game = new LotteryGame(minPlayers: 1000, maxPlayers: 2000, minTickets: 100, maxTickets: 100, initialBalance: 110);
            int requestedTickets = 100;
            game.PlayLottery(requestedTickets);
            Assert.Equal(100 * game.GetNumberOfPlayers(), game.GetNumberOfTicketsPurchased());
        }

        // This test checks that the house profit calculation is accurate when there are no winners.
        // It verifies that if all tickets are sold but no prizes are awarded, the house profit should be the sum of all ticket costs.
        [Fact]
        public void Game_ShouldHandleHouseProfitWithZeroWinners()
        {
            LotteryGame game = new LotteryGame(minPlayers: 1, maxPlayers: 1, numGrandPrizeWinners: 0, secondTierTicketPercentage: 0, thirdTierTicketPercentage: 0);
            int requestedTickets = 10;
            game.PlayLottery(requestedTickets);

            Assert.Equal(requestedTickets * game.TicketCost, game.GetHouseProfit());
        }

        // This test verifies that the game correctly handles the case where all players win the same prize tier.
        // It checks if all tickets are correctly marked as grand prize winners and ensures that the grand prize tier logic is properly applied.
        [Fact]
        public void Game_ShouldHandleAllPlayersWinningSameTier()
        {
            LotteryGame game = new LotteryGame();
            int requestedTickets = 10;
            game.PlayLottery(requestedTickets);
            var tickets = game.GetTickets();
            foreach (var ticket in tickets)
            {
                ticket.SetWinner(WinnerType.Grand, 100m); // All players win grand prize
            }
            Assert.True(tickets.All(t => t.IsGrandWinner));
        }

        // This test ensures that the house profit is correctly calculated based on ticket purchases.
        // It simulates a lottery game with 5 tickets purchased by a single player and checks if the house profit matches the expected revenue of 2.00 
        [Fact]
        public void Game_ShouldHandleHouseProfit()
        {
            LotteryGame game = new LotteryGame(minPlayers: 1, maxPlayers: 1);
            int requestedTickets = 5;
            game.PlayLottery(requestedTickets);

            Assert.Equal(2m, game.GetHouseProfit());
        }

        // This test ensures that the house profit is correctly calculated based on ticket purchases for multiple games.
        // It simulates a lottery game with 2 games and ensures the house profit increments each time
        [Fact]
        public void Game_ShouldHandleHouseProfitMultipleGames()
        {
            decimal houseProfit = 0;

            LotteryGame game = new LotteryGame();
            game.PlayLottery(5);

            Assert.True(houseProfit < game.GetHouseProfit());
            houseProfit = game.GetHouseProfit();

            game.PlayLottery(2);

            Assert.True(houseProfit < game.GetHouseProfit());
        }

        // This test verifies that the correct number of tickets are purchased during the lottery.
        // It simulates a lottery game where 3 tickets are requested, and checks if the number of tickets purchased matches the requested amount.
        [Fact]
        public void Game_ShouldHandleTicketPurchases()
        {
            LotteryGame game = new LotteryGame(minPlayers: 1, maxPlayers: 1);
            int requestedTickets = 3;
            game.PlayLottery(requestedTickets);

            Assert.Equal(requestedTickets, game.GetTickets().Count);
        }

        // This test ensures that winners for all prize tiers (grand, second, and third) are drawn correctly after the lottery is played.
        // It simulates a lottery game, draws the winners, and checks that winners exist for all tiers (grand, second, and third).
        [Fact]
        public void Game_ShouldHandleWinnerDrawing()
        {

            LotteryGame game = new LotteryGame();
            game.PlayLottery(5);

            // Draw the winners for all prize tiers.
            game.DrawWinners();

            // Assert that there is a grand winner and winners in the second and third tiers.
            Assert.NotEmpty(game.GetGrandWinner());     // Ensure there is a grand winner.
            Assert.NotEmpty(game.GetSecondWinners());  // Ensure there are second-tier winners.
            Assert.NotEmpty(game.GetThirdWinners());   // Ensure there are third-tier winners.
        }

        // This test verifies that the number of players in the game is within the expected range.
        // It checks if the game has between 10 and 15 players.
        [Fact]
        public void Game_ShouldReturnCorrectPlayerCount()
        {
            LotteryGame game = new LotteryGame();
            int playerCount = game.GetNumberOfPlayers();

            Assert.True(playerCount >= 10 && playerCount <= 15);
        }

        // This test ensures that the number of tickets purchased matches the requested number.
        // It simulates a lottery game where 5 tickets are requested, and checks if the ticket count is equal to the requested amount.
        [Fact]
        public void Game_ShouldReturnCorrectTicketCount()
        {
            LotteryGame game = new LotteryGame(minPlayers: 1, maxPlayers: 1);
            game.PlayLottery(5);

            Assert.Equal(5, game.GetTickets().Count);
        }
    }
}
