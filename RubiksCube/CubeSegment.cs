using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    internal class CubeSegment
    {
        public int ID;
        public Colours Colour;

        public CubeSegment(int inID, Colours inColour)
        {
            ID = inID;
            Colour = inColour;
        }

        /// <summary>
        /// Prints the segment to the console in the relevent colour
        /// </summary>
        public void GetSegment()
        {
            Helpers.SetConsoleColour(Colour);
            Console.Write("[]");
        }
    }
}
