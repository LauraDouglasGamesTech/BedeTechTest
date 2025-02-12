using BL_BedeLottery.Enums;

namespace BL_BedeLottery.Classes
{
    /// <summary>
    /// Represents a lottery ticket, which is associated with a player and can have a winning status.
    /// </summary>
    public class Ticket
    {
        public Guid Id { get; } = Guid.NewGuid();

        public Player Player { get; internal set; }

        public bool IsWinner { get; private set; }

        public bool IsGrandWinner { get; private set; }

        public bool IsSecondWinner { get; private set; }

        public bool IsThirdWinner { get; private set; }

        public decimal Winnings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Ticket class for a specific player.
        /// </summary>
        /// <param name="player">The player associated with this ticket.</param>
        public Ticket(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Marks the ticket as a winner and assigns the winnings to the associated player.
        /// This method ensures that a ticket cannot be marked as a winner more than once.
        /// </summary>
        /// <param name="winnings">The amount of money this ticket wins.</param>
        public void MarkWinner(decimal winnings)
        {
            if (IsWinner) return;  // Avoid marking a ticket more than once
            IsWinner = true;
            Winnings = winnings;
            Player.IncreaseBalance(Winnings);
        }

        /// <summary>
        /// Sets the winner status for this ticket based on the winner type (grand, second, or third) 
        /// and assigns the corresponding winnings.
        /// </summary>
        /// <param name="winnerType">The type of winner (grand, second, or third).</param>
        /// <param name="winnings">The amount of money this ticket wins.</param>
        public void SetWinner(WinnerType winnerType, decimal winnings)
        {
            switch (winnerType)
            {
                case WinnerType.Grand:
                    IsGrandWinner = true;
                    break;
                case WinnerType.Second:
                    IsSecondWinner = true;
                    break;
                case WinnerType.Third:
                    IsThirdWinner = true;
                    break;
            }

            MarkWinner(winnings);
        }
    }
}
