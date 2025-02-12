using BL_BedeLottery.Classes;
using BL_BedeLottery.Enums;

namespace BL_BedeLottery
{
    /// <summary>
    /// Represents the lottery game, managing the players, ticket purchases, and winner selection.
    /// </summary>
    public partial class LotteryGame
    {
        private List<Player> _players = new();
        internal List<Ticket> _tickets = new();
        private decimal _houseProfit = 0;
        private readonly Random _random = new();

        // Configurable Properties
        private int MinPlayers { get; }
        private int MaxPlayers { get; }
        private int MinTickets { get; }
        private int MaxTickets { get; }
        public decimal TicketCost { get; }
        private decimal InitialBalance { get; }
        private int NumGrandPrizeWinners { get; }
        private decimal GrandPrizeRevenuePercentage { get; }
        private decimal SecondTierTicketPercentage { get; }
        private decimal SecondTierRevenuePercentage { get; }
        private decimal ThirdTierTicketPercentage { get; }
        private decimal ThirdTierRevenuePercentage { get; }

        // ------------ Constructor ------------

        /// <summary>
        /// Initializes the lottery game with configurable properties for player numbers, ticket counts, 
        /// and prize distribution.
        /// </summary>
        /// <param name="minPlayers">Minimum number of players in the game.</param>
        /// <param name="maxPlayers">Maximum number of players in the game.</param>
        /// <param name="minTickets">Minimum number of tickets a player can purchase.</param>
        /// <param name="maxTickets">Maximum number of tickets a player can purchase.</param>
        /// <param name="ticketCost">Cost of a single lottery ticket.</param>
        /// <param name="initialBalance">Initial balance for each player.</param>
        /// <param name="numGrandPrizeWinners">Number of grand prize winners.</param>
        /// <param name="grandPrizeRevenuePercentage">Revenue percentage for grand prize winners.</param>
        /// <param name="secondTierTicketPercentage">Percentage of total tickets for second-tier winners.</param>
        /// <param name="secondTierRevenuePercentage">Revenue percentage for second-tier winners.</param>
        /// <param name="thirdTierTicketPercentage">Percentage of total tickets for third-tier winners.</param>
        /// <param name="thirdTierRevenuePercentage">Revenue percentage for third-tier winners.</param>
        public LotteryGame(
            int minPlayers = 10,
            int maxPlayers = 15,
            int minTickets = 1,
            int maxTickets = 10,
            decimal ticketCost = 1m,
            decimal initialBalance = 10m,
            int numGrandPrizeWinners = 1,
            decimal grandPrizeRevenuePercentage = 0.5m, // Default is 50%
            decimal secondTierTicketPercentage = 0.10m, // Default is 10% of total tickets
            decimal secondTierRevenuePercentage = 0.3m, // Default is 30% of total revenue
            decimal thirdTierTicketPercentage = 0.20m, // Default is 20% of total tickets
            decimal thirdTierRevenuePercentage = 0.1m) // Default is 10% of total revenue
        {
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            MinTickets = minTickets;
            MaxTickets = maxTickets;
            TicketCost = ticketCost;
            InitialBalance = initialBalance;
            NumGrandPrizeWinners = numGrandPrizeWinners;
            GrandPrizeRevenuePercentage = grandPrizeRevenuePercentage;
            SecondTierTicketPercentage = secondTierTicketPercentage;
            SecondTierRevenuePercentage = secondTierRevenuePercentage;
            ThirdTierTicketPercentage = thirdTierTicketPercentage;
            ThirdTierRevenuePercentage = thirdTierRevenuePercentage;

            InitialisePlayers();
        }

        // ------------ Game Setup and Initialization ------------

        /// <summary>
        /// Initializes the players by adding one active player and a random number of additional players.
        /// </summary>
        private void InitialisePlayers()
        {
            // Add active player
            _players.Add(new Player(1, "Player 1", InitialBalance, true));

            // Add random extra players
            int totalPlayers = _random.Next(MinPlayers - 1, MaxPlayers - 1);
            for (int i = 2; i <= totalPlayers + 1; i++)
            {
                _players.Add(new Player(i, $"Player {i}", InitialBalance));
            }
        }

        /// <summary>
        /// Purchases tickets for all players. The active player purchases the requested number of tickets,
        /// and other players purchase a random number of tickets within the defined limits.
        /// </summary>
        /// <param name="requestedTickets">The number of tickets requested by the active player.</param>
        private void PurchaseTickets(int requestedTickets)
        {
            foreach (var player in _players)
            {
                int ticketCount = player.Id == 1 ? requestedTickets : _random.Next(MinTickets, MaxTickets);
                for (int i = 0; i < ticketCount; i++)
                {
                    TryPurchaseTicket(player);
                }
            }
        }

        /// <summary>
        /// Attempts to purchase a ticket for a player if they have sufficient balance.
        /// </summary>
        /// <param name="player">The player attempting to purchase a ticket.</param>
        private void TryPurchaseTicket(Player player)
        {
            if (player.Balance >= TicketCost)
            {
                var newTicket = new Ticket(player);
                _tickets.Add(newTicket);
                player.DecreaseBalance(TicketCost);
            }
        }

        // ------------ Main Game Functions ------------

        /// <summary>
        /// Starts the lottery game, purchases tickets, and draws the winners.
        /// </summary>
        /// <param name="requestTicketQuantity">The number of tickets the active player wants to purchase.</param>
        public void PlayLottery(int requestTicketQuantity)
        {
            _tickets.Clear();  // Clear previous tickets before a new game
            PurchaseTickets(requestTicketQuantity);
            DrawWinners();
            CalculateHouseProfit();
        }

        /// <summary>
        /// Draws winners for the grand prize, second tier, and third tier based on predefined revenue percentages.
        /// </summary>
        public void DrawWinners()
        {
            decimal totalRevenue = _tickets.Count * TicketCost;

            // Grand Prize
            DrawGrandPrize(totalRevenue);

            // Second Tier
            DrawSecondTier(totalRevenue);

            // Third Tier
            DrawThirdTier(totalRevenue);
        }

        // ------------ Drawing Winners ------------

        /// <summary>
        /// Draws the grand prize winners and assigns their prize.
        /// </summary>
        /// <param name="totalRevenue">The total revenue from ticket sales.</param>
        public void DrawGrandPrize(decimal totalRevenue)
        {
            decimal prizePerWinner = totalRevenue * GrandPrizeRevenuePercentage;
            DrawTierWinners(NumGrandPrizeWinners, prizePerWinner, WinnerType.Grand);
        }

        /// <summary>
        /// Draws the second tier winners and assigns their prize.
        /// </summary>
        /// <param name="totalRevenue">The total revenue from ticket sales.</param>
        private void DrawSecondTier(decimal totalRevenue)
        {
            decimal numSecondTierWinners = Math.Round(_tickets.Count * SecondTierTicketPercentage, 0); // Percentage of total tickets
            if (numSecondTierWinners > 0)
            {
                decimal prizePerWinner = (totalRevenue * SecondTierRevenuePercentage) / numSecondTierWinners;
                DrawTierWinners((int)numSecondTierWinners, prizePerWinner, WinnerType.Second);
            }
        }

        /// <summary>
        /// Draws the third tier winners and assigns their prize.
        /// </summary>
        /// <param name="totalRevenue">The total revenue from ticket sales.</param>
        private void DrawThirdTier(decimal totalRevenue)
        {
            decimal numThirdTierWinners = Math.Round(_tickets.Count * ThirdTierTicketPercentage, 0); // Percentage of total tickets
            if (numThirdTierWinners > 0)
            {
                decimal prizePerWinner = (totalRevenue * ThirdTierRevenuePercentage) / numThirdTierWinners;
                DrawTierWinners((int)numThirdTierWinners, prizePerWinner, WinnerType.Third);
            }
        }

        /// <summary>
        /// Selects winners for a specific tier and assigns their prize.
        /// </summary>
        /// <param name="numWinners">The number of winners to select.</param>
        /// <param name="prizePerWinner">The prize awarded to each winner.</param>
        /// <param name="winnerType">The type of winner (e.g., Grand, Second, Third).</param>
        private void DrawTierWinners(int numWinners, decimal prizePerWinner, WinnerType winnerType)
        {
            if (_tickets.Count == 0 || numWinners <= 0) return;

            for (int i = 0; i < numWinners; i++)
            {
                var nonWinningTickets = _tickets.Where(x => !x.IsWinner).ToList();
                if (nonWinningTickets.Count == 0) break;

                var winnerTicket = nonWinningTickets[_random.Next(nonWinningTickets.Count)];
                winnerTicket.SetWinner(winnerType, prizePerWinner);
            }
        }

        // ------------ Information Accessors ------------

        /// <summary>
        /// Retrieves the active player in the game.
        /// </summary>
        /// <returns>The active player, or null if no active player exists.</returns>
        public Player? GetActivePlayer() => _players.FirstOrDefault(x => x.IsActivePlayer);

        /// <summary>
        /// Retrieves the grand prize winner.
        /// </summary>
        /// <returns>The grand prize winning ticket, or null if there is no winner.</returns>
        public IReadOnlyList<Ticket> GetGrandWinner() => _tickets.Where(x => x.IsGrandWinner).ToList().AsReadOnly();

        /// <summary>
        /// Retrieves the second tier winners.
        /// </summary>
        /// <returns>A read-only list of second tier winning tickets.</returns>
        public IReadOnlyList<Ticket> GetSecondWinners() => _tickets.Where(x => x.IsSecondWinner).ToList().AsReadOnly();

        /// <summary>
        /// Retrieves the third tier winners.
        /// </summary>
        /// <returns>A read-only list of third tier winning tickets.</returns>
        public IReadOnlyList<Ticket> GetThirdWinners() => _tickets.Where(x => x.IsThirdWinner).ToList().AsReadOnly();

        /// <summary>
        /// Retrieves all purchased tickets.
        /// </summary>
        /// <returns>A read-only list of all tickets.</returns>
        public IReadOnlyList<Ticket> GetTickets() => _tickets.AsReadOnly();

        /// <summary>
        /// Retrieves the total number of tickets purchased.
        /// </summary>
        /// <returns>The total number of tickets purchased.</returns>
        public int GetNumberOfTicketsPurchased() => _tickets.Count;

        /// <summary>
        /// Retrieves the total number of players in the game.
        /// </summary>
        /// <returns>The total number of players.</returns>
        public int GetNumberOfPlayers() => _players.Count;

        /// <summary>
        /// Retrieves all players in the game.
        /// </summary>
        /// <returns>All players.</returns>
        public List<Player> GetPlayers() => _players;

        /// <summary>
        /// Retrieves the house profit, calculated by the total revenue minus the total winnings.
        /// </summary>
        /// <returns>The house profit.</returns>
        public decimal GetHouseProfit()
        {
            return _houseProfit;
        }

        // ------------ Helper Methods ------------

        /// <summary>
        /// Calculates the house profit by subtracting the winnings of all winners from the total revenue.
        /// </summary>
        private void CalculateHouseProfit()
        {
            _houseProfit += _tickets.Count * TicketCost - _tickets.Where(x => x.IsWinner).Sum(x => x.Winnings);
        }
    }
}