using System;
using System.IO;
using System.Collections.Generic;
using BL_BedeLottery;
using BL_BedeLottery.Classes;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Console_BedeLottery.Tests
{
    /// <summary>
    /// Unit tests for the Program class in the Console_BedeLottery namespace.
    /// Ensures proper behavior of key game methods and handles edge cases.
    /// </summary>
    public class ProgramTests
    {
        // Tests whether InitializeGame correctly returns a new instance of LotteryGame.
        [Fact]
        public void InitializeGame_ShouldReturnNewLotteryGameInstance()
        {
            var game = Program.InitializeGame();

            Assert.NotNull(game);
            Assert.IsType<LotteryGame>(game);
        }

        // Tests if GetValidIntegerInput correctly returns an integer when given valid input.
        [Fact]
        public void GetValidIntegerInput_ValidInput_ReturnsInteger()
        {
            using var input = new StringReader("5\n");
            Console.SetIn(input);

            int result = Program.GetValidIntegerInput("Invalid input", 3);

            Assert.Equal(5, result);
        }

        // Tests if GetValidIntegerInput exits gracefully after exceeding maximum retries for invalid input.
        [Fact]
        public void GetValidIntegerInput_InvalidInput_ExitsAfterMaxRetries()
        {
            using var input = new StringReader("a\nb\nc\n");
            Console.SetIn(input);
            using var output = new StringWriter();
            Console.SetOut(output);

            var exception = Record.Exception(() => Program.GetValidIntegerInput("Invalid input", 3));

            Assert.Null(exception); // The test should exit without throwing an exception
            Assert.Contains("Maximum retries (3) exceeded", output.ToString());
        }

        // Tests if AskForAnotherGame correctly interprets valid inputs and returns the expected boolean values.
        [Theory]
        [InlineData("Y", true)]
        [InlineData("y", true)]
        [InlineData("N", false)]
        [InlineData("n", false)]
        [InlineData("  Y  ", true)]
        [InlineData("  N  ", false)]
        public void AskForAnotherGame_ValidResponses_ReturnsExpectedBoolean(string input, bool expected)
        {
            using var inputReader = new StringReader(input + "\n");
            Console.SetIn(inputReader);

            bool result = Program.AskForAnotherGame();

            Assert.Equal(expected, result);
        }

        // Tests if AskForAnotherGame prompts the user again when invalid input is provided.
        [Fact]
        public void AskForAnotherGame_InvalidInput_PromptsAgain()
        {
            using var inputReader = new StringReader("invalid\nN\n");
            Console.SetIn(inputReader);
            using var outputWriter = new StringWriter();
            Console.SetOut(outputWriter);

            bool result = Program.AskForAnotherGame();

            Assert.False(result);
            Assert.Contains("Please respond with Y for Yes or N for No.", outputWriter.ToString());
        }
    }
}
