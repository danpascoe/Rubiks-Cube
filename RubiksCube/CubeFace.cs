using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    internal class CubeFace
    {
        public int ID;
        public CubeSegment[,] Segments;

        public int NorthNeighbourID;
        public int EastNeighbourID;
        public int SouthNeighbourID;
        public int WestNeighbourID;

        /// <summary>
        /// CubeFace constructor which generates each of the 9 segments belonging to a face of a Rubik's Cube.
        /// A segment is created and stored in a 2D array. 
        /// </summary>
        /// <param name="inID">The ID is needed to identify the segment when calculating the rotation</param>
        /// <param name="inColour">The colour of the 9 segments</param>
        public CubeFace(int inID, Colours inColour)
        {
            ID = inID;
            Segments = new CubeSegment[3, 3];

            int segmentID = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Segments[i, j] = new CubeSegment(segmentID, inColour);
                    segmentID++;
                }
            }
        }

        /// <summary>
        /// Sets the neighbouring CubeFaces. 
        /// The neighbours are required for the segments on the side of the face that is being rotated.
        /// </summary>
        public void SetNeighbour(int inNorth, int inEast, int inSouth, int inWest)
        {
            NorthNeighbourID = inNorth;
            EastNeighbourID = inEast;
            SouthNeighbourID = inSouth;
            WestNeighbourID = inWest;
        }
    }
}
