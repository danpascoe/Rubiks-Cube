using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    internal class GameManager
    {
        public RubiksCube Cube;
        public List<string> History;

        public GameManager()
        {
            Cube = new RubiksCube();
            History = new List<string>();
            Run();
        }

        /// <summary>
        /// Loops the ParseInput method. 
        /// Once the END condition has been met, the loop is broken.
        /// </summary>
        private void Run()
        {
            bool isRunning = true;

            Console.WriteLine("");

            while (isRunning)
            {
                Console.Write("Input: ");

                var input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    isRunning = ParseInput(input.ToUpper().Trim());
                }
            }

            Console.WriteLine("Thanks for playing!");
            Console.WriteLine("");

            return;
        }

        /// <summary>
        /// Takes the user input from the console and runs code dependant on the input.       
        /// </summary>
        /// <param name="inInput"></param>
        /// <returns>Returns false if the user inputs END.</returns>
        private bool ParseInput(string inInput)
        {
            List<string> output = new List<string>();
            List<string> cubeMoves = new List<string> { "F", "F'", "R", "R'", "U", "U'", "B", "B'", "L", "L'", "D", "D'" };

            bool isValidInput = true;

            switch (inInput.ToUpper())
            {
                case "END":
                    return false;

                case "RESET":
                    Cube = new RubiksCube();
                    History = new List<string>();
                    break;

                case "HISTORY":

                    string history = "";

                    if (History.Count == 0)
                    {
                        output.Add("You haven't entered any commands.");
                        break;
                    }

                    foreach (var item in History)
                    {
                        history += item + ", ";
                    }

                    output.Add("");
                    output.Add("Previous commands:");
                    output.Add(history.Remove(history.Length - 2, 2));
                    break;

                case "MULTI":

                    output.Add("");
                    output.Add("Input is now in multi-mode. Enter a string of commands seperated by a space");

                    PrintScreen(output);

                    Console.Write("Input: ");
                    var multiMove = Console.ReadLine();
                    if (string.IsNullOrEmpty(multiMove)) return true;

                    foreach (var move in multiMove.Split(' '))
                    {
                        ParseInput(move);
                    }

                    return true;

                case "HELP":

                    output.Add("");
                    output.Add("Valid commands:");
                    output.Add("-----------------------------------------------");
                    output.Add("F               Front face clockwise 90deg");
                    output.Add("R               Right face clockwise 90deg");
                    output.Add("U               Up face clockwise 90deg");
                    output.Add("B               Back face clockwise 90deg");
                    output.Add("L               Left face clockwise 90deg");
                    output.Add("D               Down face clockwise 90deg");
                    output.Add("");
                    output.Add("Add an apostrophe (') after any of the above moves in order to make the rotation anti-clockwise");
                    output.Add("");
                    output.Add("END             Stops the cube");
                    output.Add("HELP            You're already here!");
                    output.Add("RESET           Resets the cube to it's original state");
                    output.Add("HISTORY         Prints your previous commands");
                    output.Add("MULTI           Puts the input into \"multi-mode\".");
                    output.Add("                This allows you to enter multiple commands, seperated by a space");

                    break;

                default:

                    if (cubeMoves.Contains(inInput.ToUpper()))
                    {
                        Cube.Move(inInput);
                    }
                    else
                    {
                        isValidInput = false;
                        output.Add("");
                        output.Add("That input is unknown. Please enter a valid command, or type help for a list of commands");
                    }

                    break;
            }

            if (isValidInput) History.Add(inInput.ToUpper());

            PrintScreen(output);
            return true;
        }

        /// <summary>
        /// Prints the relevent information to the console.
        /// </summary>
        /// <param name="inOutput"></param>
        private void PrintScreen(List<string> inOutput)
        {
            Console.Clear();
            Cube.PrintCube();
            Console.WriteLine();

            if (inOutput.Any())
            {
                foreach (var str in inOutput) Console.WriteLine(str);
                Console.WriteLine();
            }
        }
    }
}
