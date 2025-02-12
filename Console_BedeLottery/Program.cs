using BL_BedeLottery;
using BL_BedeLottery.Classes;

namespace Console_BedeLottery
{
    /// <summary>
    /// Entry point of the Lottery Game application. Manages game initialization, player interaction, 
    /// and game flow for each lottery round.
    /// </summary>
    public class Program
    {
        private static LotteryGame _game; // Chosen to initiate the game with default values

        /// <summary>
        /// Main entry point for the Lottery Game.
        /// </summary>
        static void Main()
        {
            _game = InitializeGame(); // Decoupled instantiation
            DisplayWelcome();
            PlayGame();
        }

        public static LotteryGame InitializeGame()
        {
            return new LotteryGame(); // Dependency Injection ready
        }




        // ------------ Game Initialization and Flow ------------

        /// <summary>
        /// Displays a welcome message to the active player and their current balance.
        /// Exits the game if no active player is found.
        /// </summary>
        public static void DisplayWelcome()
        {
            Player? activePlayer = _game.GetActivePlayer();
            if (activePlayer == null)
            {
                Console.WriteLine("No active player found. Exiting the game.");
                Environment.Exit(0); // Exit the game if no active player is found
            }

            Console.WriteLine($"Welcome to the Bede Lottery, {activePlayer.Name}!\n");
            DisplayBalance(activePlayer);
        }

        /// <summary>
        /// Starts the lottery game, processes player ticket requests, and displays the results 
        /// (players, winners, and house profit).
        /// </summary>
        public static void PlayGame()
        {
            int requestedPlayerTickets = GetPlayerTicketRequest();
            if (requestedPlayerTickets > 0)
            {
                _game.PlayLottery(requestedPlayerTickets);

                DisplayPlayers();
                DisplayWinners();
                DisplayHouseProfit();

                RequestSubsequentGame();
            }
            else
            {
                Console.WriteLine("Thank you for playing!");
            }
        }

        /// <summary>
        /// Prompts the player if they wish to play another game. 
        /// Ends the game if the player's balance is insufficient or they decline to play again.
        /// </summary>
        public static void RequestSubsequentGame()
        {
            Player? activePlayer = _game.GetActivePlayer();

            if (activePlayer == null)
            {
                Console.WriteLine("No active player found. Exiting the game.");
                return; // Exit the method if no active player is found
            }

            if (activePlayer.Balance >= _game.TicketCost)
            {
                DisplayBalance(activePlayer);
                if (AskForAnotherGame())
                {
                    PlayGame();
                }
                else
                {
                    Console.WriteLine("Thank you for playing!");
                }
            }
            else
            {
                Console.WriteLine("Sorry! You do not have enough balance to play again.");
            }
        }

        // ------------ User Input and Validation ------------

        /// <summary>
        /// Asks the player how many tickets they want to buy and validates the input.
        /// </summary>
        /// <returns>The number of tickets the player wishes to purchase.</returns>
        public static int GetPlayerTicketRequest()
        {
            Console.Write("How many tickets do you want to buy, Player 1? ");
            return GetValidIntegerInput("Please ensure the input is numeric.", 3);
        }

        /// <summary>
        /// Ensures that the input is a valid integer. Keeps prompting the user until valid input is entered.
        /// </summary>
        /// <param name="errorMessage">The error message to display if the input is invalid.</param>
        /// <returns>A valid integer input.</returns>
        public static int GetValidIntegerInput(string errorMessage, int maxRetries)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;

                Console.WriteLine(errorMessage);
                retries++;
            }

            Console.WriteLine($"Maximum retries ({maxRetries}) exceeded. Exiting game.");
            return 0;
        }

        /// <summary>
        /// Asks the player if they would like to play another game.
        /// </summary>
        /// <returns>True if the player chooses to play another game, otherwise false.</returns>
        public static bool AskForAnotherGame()
        {
            Console.WriteLine("Do you want to play another game? (Y/N)");
            string response = Console.ReadLine()?.Trim() ?? string.Empty;

            if (response.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                return true;
            else if (response.Equals("N", StringComparison.CurrentCultureIgnoreCase))
                return false;
            else
            {
                Console.WriteLine("Please respond with Y for Yes or N for No.");
                return AskForAnotherGame(); // Retry the input
            }
        }

        // ------------ Game Display Methods ------------

        /// <summary>
        /// Displays the player's current balance and ticket cost.
        /// </summary>
        /// <param name="activePlayer">The active player whose balance is being displayed.</param>
        public static void DisplayBalance(Player activePlayer)
        {
            Console.WriteLine($"* Your digital balance: {activePlayer.Balance:C}");
            Console.WriteLine($"* Ticket Price: {_game.TicketCost:C} each\n");
        }

        /// <summary>
        /// Displays the number of CPU players and the total number of tickets purchased.
        /// </summary>
        public static void DisplayPlayers()
        {
            Console.WriteLine($"\nThere are {_game.GetNumberOfPlayers() - 1} CPU Players");
            Console.WriteLine($"A total of {_game.GetNumberOfTicketsPurchased()} tickets have been purchased\n");
        }

        /// <summary>
        /// Displays the winners of each tier (Grand, Second, Third).
        /// </summary>
        public static void DisplayWinners()
        {
            DisplayWinnerTier("Grand", _game.GetGrandWinner());
            DisplayWinnerTier("Second", _game.GetSecondWinners());
            DisplayWinnerTier("Third", _game.GetThirdWinners());

            Console.WriteLine("Congratulations to the winners!\n");
        }

        /// <summary>
        /// Displays the winners for a specific tier.
        /// </summary>
        /// <param name="tierName">The name of the tier (e.g., "Grand", "Second", "Third").</param>
        /// <param name="winners">The list of winners for the tier.</param>
        public static void DisplayWinnerTier(string tierName, object? winners)
        {
            if (winners is IReadOnlyList<Ticket> ticketList && ticketList.Count > 0)
            {
                // Handle list of winners (Second and Third Tier)
                string winnersString = string.Join(", ", ticketList.Select(ticket => ticket.Player.Id.ToString()));
                decimal winnings = ticketList.FirstOrDefault()?.Winnings ?? 0m;
                Console.WriteLine($"* {tierName} Tier: Players {winnersString} win {winnings:C} each!");
            }
            else
            {
                // Handle no winners
                Console.WriteLine($"There were no {tierName.ToLower()} tier winners.");
            }
        }

        /// <summary>
        /// Displays the house revenue (profit) after a round of lottery.
        /// </summary>
        public static void DisplayHouseProfit()
        {
            Console.WriteLine($"House Revenue: {_game.GetHouseProfit():C}\n");
        }
    }
}
