namespace BL_BedeLottery.Classes
{
    /// <summary>
    /// Represents a player in the lottery game, including their unique ID, name, balance, and active status.
    /// </summary>
    public class Player
    {
        public int Id { get; }

        public string Name { get; }

        public decimal Balance { get; private set; }

        public bool IsActivePlayer { get; }

        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        /// <param name="id">The unique identifier for this player.</param>
        /// <param name="name">The name of the player.</param>
        /// <param name="balance">The starting balance for this player.</param>
        /// <param name="isActivePlayer">Indicates if the player is the active player. Defaults to false.</param>
        public Player(int id, string name, decimal balance, bool isActivePlayer = false)
        {
            Id = id;
            Name = name;
            Balance = balance;
            IsActivePlayer = isActivePlayer;
        }

        /// <summary>
        /// Increases the player's balance by the specified amount.
        /// </summary>
        /// <param name="amount">The amount to add to the player's balance.</param>
        public void IncreaseBalance(decimal amount)
        {
            Balance += amount;
        }

        /// <summary>
        /// Decreases the player's balance by the specified amount.
        /// </summary>
        /// <param name="amount">The amount to subtract from the player's balance.</param>
        public void DecreaseBalance(decimal amount)
        {
            Balance -= amount;
        }
    }
}
