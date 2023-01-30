using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    internal class RubiksCube
    {
        List<CubeFace> Faces;

        /// <summary>
        /// Creates the Cube object
        /// Each face is created and assigned neighbours (north, east, south, west) then added to the Faces array
        /// </summary>
        public RubiksCube()
        {
            var cf_0 = new CubeFace(0, Colours.GREEN);
            var cf_1 = new CubeFace(1, Colours.ORANGE);
            var cf_2 = new CubeFace(2, Colours.YELLOW);
            var cf_3 = new CubeFace(3, Colours.RED);
            var cf_4 = new CubeFace(4, Colours.BLUE);
            var cf_5 = new CubeFace(5, Colours.WHITE);

            cf_0.SetNeighbour(5, 3, 2, 1); // WHITE, RED, YELLOW, ORANGE
            cf_1.SetNeighbour(5, 0, 2, 4); // WHITE, GREEN, YELLOW, BLUE
            cf_2.SetNeighbour(0, 3, 4, 1); // GREEN, RED, BLUE, ORANGE
            cf_3.SetNeighbour(5, 4, 2, 0); // WHITE, BLUE, YELLOW, GREEN
            cf_4.SetNeighbour(5, 1, 2, 3); // YELLOW, RED, WHITE, ORANGE            
            cf_5.SetNeighbour(4, 3, 0, 1); // BLUE, RED, GREEN, ORANGE

            Faces = new List<CubeFace> { cf_0, cf_1, cf_2, cf_3, cf_4, cf_5 };

            PrintCube();
        }

        /// <summary>
        /// Prints each face of the cube to the console, including the blank spaces
        /// </summary>
        public void PrintCube()
        {
            Helpers.PrintFaces(new List<CubeFace?>() { null, Faces[5], null, null });
            Helpers.PrintFaces(new List<CubeFace?>() { Faces[1], Faces[0], Faces[3], Faces[4] });
            Helpers.PrintFaces(new List<CubeFace?>() { null, Faces[2], null, null });

            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Handles the cubes movement
        /// Takes the input from the console, gets the relevent face and the calls the rotate method
        /// </summary>
        /// <param name="inInput">Input from the console</param>
        public void Move(string inInput)
        {
            int faceIndex = Helpers.GetMoveFaceIndex(inInput);
            if (faceIndex == -1) return;

            RotateFace(Faces[faceIndex], inInput.Contains("'"));
        }

        /// <summary>
        /// Rotate the face and reasign the segments to the new position in the face arrays
        /// </summary>
        /// <param name="inFace">Which face is to be rotated</param>
        /// <param name="inIsAntiClockwise">Is the face being rotated clockwise or anti-clockwise</param>
        private void RotateFace(CubeFace inFace, bool inIsAntiClockwise)
        {
            // Create a dictionary containing each move of the face
            // Each segment has a start position and an end position when rotated            
            var positions = new Dictionary<KeyValuePair<int, int>, KeyValuePair<int, int>>
            {
                { new KeyValuePair<int, int>(0, 0), new KeyValuePair<int, int>(0, 2) },
                { new KeyValuePair<int, int>(0, 1), new KeyValuePair<int, int>(1, 2) },
                { new KeyValuePair<int, int>(0, 2), new KeyValuePair<int, int>(2, 2) },
                { new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(2, 1) },
                { new KeyValuePair<int, int>(2, 2), new KeyValuePair<int, int>(2, 0) },
                { new KeyValuePair<int, int>(2, 1), new KeyValuePair<int, int>(1, 0) },
                { new KeyValuePair<int, int>(2, 0), new KeyValuePair<int, int>(0, 0) },
                { new KeyValuePair<int, int>(1, 0), new KeyValuePair<int, int>(0, 1) }
            };

            // Creates a clone of the segments, in order to move them to the correct position in the array
            var _segments = inFace.Segments.Clone() as CubeSegment[,];
            if (_segments == null) return;

            // Foreach of the positions in the dictionary, move the segment from the original position to the new position
            // If the face has been rotated anti-clockwise, the start poistion is taken from the value of the dictionary, rather than the key
            foreach (var position in positions)
            {
                var originalPosition = position.Key;
                var newPostion = position.Value;

                if (inIsAntiClockwise)
                {
                    originalPosition = position.Value;
                    newPostion = position.Key;
                }

                inFace.Segments[newPostion.Key, newPostion.Value] = _segments[originalPosition.Key, originalPosition.Value];
            }

            // Get each of the faces that are connected to the face that is being rotated
            var northFace = GetFace(inFace.NorthNeighbourID);
            var eastFace = GetFace(inFace.EastNeighbourID);
            var southFace = GetFace(inFace.SouthNeighbourID);
            var westFace = GetFace(inFace.WestNeighbourID);

            // Get the segment array addresses for the neighbouring faces            
            var northSegment = GetRelativeNeighbourSegmentAddress(inFace, northFace);
            var eastSegment = GetRelativeNeighbourSegmentAddress(inFace, eastFace);
            var southSegment = GetRelativeNeighbourSegmentAddress(inFace, southFace);
            var westSegment = GetRelativeNeighbourSegmentAddress(inFace, westFace);

            // Create a temporary list of segements at one of the edges
            var _tmp = new List<CubeSegment>();
            foreach (var segment in northSegment)
            {
                _tmp.Add(northFace.Segments[segment.Key, segment.Value]);
            }

            // Move the outside edge of the face to the new position
            if (!inIsAntiClockwise)
            {
                MoveOutsideSegments(westFace, westSegment, northFace, northSegment);
                MoveOutsideSegments(southFace, southSegment, westFace, westSegment);
                MoveOutsideSegments(eastFace, eastSegment, southFace, southSegment);
                MoveOutsideSegments(northFace, northSegment, eastFace, eastSegment, _tmp);
            }
            else
            {
                MoveOutsideSegments(eastFace, eastSegment, northFace, northSegment);
                MoveOutsideSegments(southFace, southSegment, eastFace, eastSegment);
                MoveOutsideSegments(westFace, westSegment, southFace, southSegment);
                MoveOutsideSegments(northFace, northSegment, westFace, westSegment, _tmp);
            }
        }

        /// <summary>
        /// Move the segments from one face to another
        /// </summary>
        /// <param name="inOriginalFace">The face the segments start on</param>
        /// <param name="inOriginalSegments">A list of addresses the segments connecting the rotating face to the outside face</param>
        /// <param name="inNewFace">The face the segments will be rotated onto</param>
        /// <param name="inNewSegments">A list of addresses the original segments will be moved into</param>
        /// <param name="inTempSegments">A list of segments that were stored in order to complete the rotation without loss of information</param>
        private void MoveOutsideSegments(CubeFace inOriginalFace, List<KeyValuePair<int, int>> inOriginalSegments, CubeFace inNewFace, List<KeyValuePair<int, int>> inNewSegments, List<CubeSegment>? inTempSegments = null)
        {
            // For each the segment address, get the segment and move it to the new face.
            // If temporary segments have been passed into the method, use them instead.
            for (int i = 0; i < 3; i++)
            {
                var originalSegmentAddress = inOriginalSegments[i];
                var originalSegment = inOriginalFace.Segments[originalSegmentAddress.Key, originalSegmentAddress.Value];

                if (inTempSegments != null)
                {
                    originalSegment = inTempSegments[i];
                }

                var newSegmentAddress = inNewSegments[i];
                inNewFace.Segments[newSegmentAddress.Key, newSegmentAddress.Value] = originalSegment;
            }
        }

        /// <summary>
        /// Gets the relevent face from the array of faces
        /// </summary>
        /// <param name="inID">The ID of the face</param>        
        private CubeFace GetFace(int inID)
        {
            return Faces.Where(f => f.ID == inID).First();
        }

        /// <summary>
        /// Gets the addresses of the segments that are connected to the rotating face        
        /// </summary>
        /// <param name="inRotatingFace">The face that is being rotated</param>
        /// <param name="inNeighbourFace">The neighbouring that is set during initialisation</param>
        /// <returns>A list of addresses for the segments in the neighbouring face</returns>
        private List<KeyValuePair<int, int>> GetRelativeNeighbourSegmentAddress(CubeFace inRotatingFace, CubeFace inNeighbourFace)
        {
            var positions = new List<KeyValuePair<int, int>>();

            if (inNeighbourFace.NorthNeighbourID == inRotatingFace.ID)
            {
                positions.Add(new KeyValuePair<int, int>(0, 2));
                positions.Add(new KeyValuePair<int, int>(0, 1));
                positions.Add(new KeyValuePair<int, int>(0, 0));
            }
            else if (inNeighbourFace.EastNeighbourID == inRotatingFace.ID)
            {
                positions.Add(new KeyValuePair<int, int>(2, 2));
                positions.Add(new KeyValuePair<int, int>(1, 2));
                positions.Add(new KeyValuePair<int, int>(0, 2));
            }
            else if (inNeighbourFace.SouthNeighbourID == inRotatingFace.ID)
            {
                positions.Add(new KeyValuePair<int, int>(2, 0));
                positions.Add(new KeyValuePair<int, int>(2, 1));
                positions.Add(new KeyValuePair<int, int>(2, 2));
            }
            else if (inNeighbourFace.WestNeighbourID == inRotatingFace.ID)
            {
                positions.Add(new KeyValuePair<int, int>(0, 0));
                positions.Add(new KeyValuePair<int, int>(1, 0));
                positions.Add(new KeyValuePair<int, int>(2, 0));
            }

            return positions;
        }
    }
}
