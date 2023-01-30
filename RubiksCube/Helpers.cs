using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    internal static class Helpers
    {
        /// <summary>
        /// Returns the ID of the face that should be rotated.        
        /// </summary>
        /// <param name="inMove">The input from Console.ReadLine</param>
        /// <returns></returns>
        public static int GetMoveFaceIndex(string inMove)
        {
            switch (inMove)
            {
                case "F":
                case "F'":
                    return 0;

                case "R":
                case "R'":
                    return 3;

                case "U":
                case "U'":
                    return 5;

                case "B":
                case "B'":
                    return 4;

                case "L":
                case "L'":
                    return 1;

                case "D":
                case "D'":
                    return 2;
            }

            return -1;
        }

        /// <summary>
        /// Sets the console colour based on the segement colour
        /// </summary>        
        public static void SetConsoleColour(Colours inColour)
        {
            switch (inColour)
            {
                case Colours.WHITE:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Colours.BLUE:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case Colours.RED:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Colours.GREEN:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Colours.ORANGE:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case Colours.YELLOW:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        /// <summary>
        /// Prints the faces to the console 
        /// </summary>
        /// <param name="inFaces">A list of faces that appear in the row  </param>
        public static void PrintFaces(List<CubeFace?> inFaces)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    PrintFaceRow(inFaces[j], i);
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints each row of the cube to the console        
        /// </summary>
        /// <param name="inFace">Which face is currently being printed. A null face will return empty space</param>
        /// <param name="inRow">Which row of the face</param>
        private static void PrintFaceRow(CubeFace? inFace, int inRow)
        {
            for (int i = 0; i < 3; i++)
            {
                if (inFace != null) inFace.Segments[inRow, i].GetSegment();
                else Console.Write("  ");
            }
        }
    }
}
