/*
 * NVX_VideoWallConfigurator_Rev1.0
 *Created by Sean A Spiggle on 09/27/2023. 
 
 Program Summary: This program takes in panel button presses and will set panels high or low accordingly.
                  When a Video Input button is pressed, all high panels will get sent the video source and layout
                  and position.
 
 This program operates with the following: 
 1) Crestron SIMPL+ Module "NVX_VideoWallConfigurator_SIMPLplus.usp
 2) 3-Series and above Crestron Control Processor
 3) Crestron DM-NVX Receivers that support Video Wall capabilities. 
  
Input from SIMPL PLUS: Panel Button Presses, Video Input Button Presses
 Output to SIMPL PLUS: Panel Button Feedback, Video Input Button Feedback, 
                      Panel Video Source, Panel Layout and Position
*/

using System;
using System.Linq;
using System.Collections.Generic;
using Crestron.SimplSharp;

namespace NVX_VideoWallConfigurator
{
    // Represents an individual video wall panel.
    public class Panel
    {
        // Source represents the input source currently routed to the panel.
        public ushort Source { get; protected set; }

        public void SetSource(ushort source)
        {
            Source = source;
        }

        public ushort IsHigh { get; private set; }

        // Default constructor for the Panel. Initializes the state to 'Low'.
        public Panel()
        {
            IsHigh = 0;
        }

        public void SetHigh()
        {
            IsHigh = 1; // Set panel to 'High' state.
        }

        public void SetLow()
        {
            IsHigh = 0; // Set panel to 'Low' state.
        }
    }


    // Represents the entire video wall configuration.
    public class VideoWall
    {
        // Dimensions of the video wall
        private const int WallWidth = 4;
        private const int WallHeight = 2;

        private List<HighPanelInfo> highPanelInfos = new List<HighPanelInfo>();

        // Delegate and event for indicating when a video input button is pressed.
        public delegate void VideoInputPressedHandler(int videoInput);
        public event VideoInputPressedHandler OnVideoInputPressed;

        // Method to get the height and width of the current high panel layout.
        public void GetCurrentHighPanelLayout(out int currentHeight, out int currentWidth)
        {
            List<int> highPanels = GetHighPanels();

            if (highPanels.Count == 0)
            {
                // If there are no high panels, set both height and width to 0.
                currentHeight = 0;
                currentWidth = 0;
            }
            else
            {
                // Initialize height and width to minimum values, then find the maximum row and column.
                int minHeight = WallHeight;
                int minWidth = WallWidth;

                foreach (int panelNumber in highPanels)
                {
                    int x = (panelNumber - 1) % WallWidth;
                    int y = (panelNumber - 1) / WallWidth;

                    if (y < minHeight)
                        minHeight = y;
                    if (x < minWidth)
                        minWidth = x;
                }

                currentHeight = highPanels.Max() / WallWidth - minHeight + 1;
                currentWidth = highPanels.Max() % WallWidth - minWidth + 1;
            }
        }

        public class HighPanelPosition
        {
            public int X { get; set; }
            public int Y { get; set; }

            public HighPanelPosition(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        // Represents the position of a high panel relative to the current layout.
        public void GetHighPanelPositions(ref object highPanelPositions)
        {
            // Get the current high panel layout dimensions
            int currentHeight, currentWidth;
            GetCurrentHighPanelLayout(out currentHeight, out currentWidth);

            // Get the list of high panels
            List<int> highPanels = GetHighPanels();

            // Create a list to store the positions of high panels
            List<HighPanelInfo> highPanelInfos = new List<HighPanelInfo>();

            // Iterate over the high panels and calculate their positions
            foreach (int panelNumber in highPanels)
            {
                int x = (panelNumber - 1) % WallWidth;
                int y = (panelNumber - 1) / WallWidth;

                // Calculate the position relative to the current layout
                int relativeX = x % currentWidth;
                int relativeY = y % currentHeight;

                // Create an instance of HighPanelInfo and populate it
                HighPanelInfo panelInfo = new HighPanelInfo
                {
                    W = currentWidth,
                    H = currentHeight,
                    X = relativeX,
                    Y = relativeY
                };

                highPanelInfos.Add(panelInfo);
            }

            // Assign the list of positions to the out parameter
            highPanelPositions = highPanelInfos.ToArray();
        }


        //Method that handles the logic when a video input button is pressed.
        //Routes input source and video wall layout to panel. 
        public void VideoInputButtonPressed(int videoInput)
        {
            // Trigger the event to indicate that a video input button has been pressed.
            VideoInputPressedHandler handler = OnVideoInputPressed;
            if (handler != null)
            {
                handler(videoInput);
            }

            RouteSourceToHighPanels((ushort)videoInput);

            // Get the current high panel layout dimensions
            int currentHeight, currentWidth;
            GetCurrentHighPanelLayout(out currentHeight, out currentWidth);

            // Get the positions of high panels
            object highPanelPositions = null;
            GetHighPanelPositions(ref highPanelPositions);
        }

        // 2D array representing all the panels.
        public Panel[,] Panels { get; private set; }

        // Constructor initializes a video wall with default 'low' state panels.
        public VideoWall()
        {
            Panels = new Panel[WallWidth, WallHeight];
            for (int x = 0; x < WallWidth; x++)
            {
                for (int y = 0; y < WallHeight; y++)
                {
                    Panels[x, y] = new Panel();
                }
            }
        }

        // Method to check if a specific panel is in 'high' state.
        public ushort GetIsHigh(int x, int y)
        {
            return Panels[x, y].IsHigh;
        }

        // This method checks if any neighboring panels (top, bottom, left, or right) to a given panel (x, y)
        // are currently in the 'high' state.
        private bool IsNeighborOfAnyHighPanel(int x, int y)
        {
            // Create a list of potential neighboring coordinates to the provided panel.
            List<PanelCoordinate> neighbors = new List<PanelCoordinate>
            {
                new PanelCoordinate(x - 1, y),
                new PanelCoordinate(x + 1, y),
                new PanelCoordinate(x, y - 1),
                new PanelCoordinate(x, y + 1)
            };

            // Iterate over the list of potential neighboring coordinates.
            foreach (var coord in neighbors)
            {
                // Ensure that the neighboring coordinates are within the bounds of the video wall.
                if (coord.X >= 0 && coord.X < WallWidth && coord.Y >= 0 && coord.Y < WallHeight)
                {
                    if (Panels[coord.X, coord.Y].IsHigh == 1) return true; // Use highState here
                }
            }
            return false;
        }


        // This method retrieves a list of panel numbers that are currently in the 'high' state.
        public List<int> GetHighPanels()
        {
            // Initialize a new list to store the panel numbers in the 'high' state.
            List<int> highPanels = new List<int>();

            // Iterate over the entire video wall.
            for (int x = 0; x < WallWidth; x++)
            {
                for (int y = 0; y < WallHeight; y++)
                {
                    // Check if the current panel is in the 'high' state.
                    if (Panels[x, y].IsHigh == 1)
                    {
                        // Convert the (x, y) coordinates of the panel to its corresponding panel number.
                        int panelNumber = y * WallWidth + x + 1;  // Convert (x, y) to panel number
                        highPanels.Add(panelNumber);
                    }
                }
            }
            return highPanels;
        }

        // Routes a given source number to all panels currently set to 'High'.
        public void RouteSourceToHighPanels(ushort sourceNumber)
        {
            List<int> highPanels = GetHighPanels();
            foreach (int panelNumber in highPanels)
            {
                int x = (panelNumber - 1) % WallWidth;
                int y = (panelNumber - 1) / WallWidth;

                Panels[x, y].SetSource(sourceNumber);
            }
        }

        // Retrieves the source number currently routed to a specified panel.
        public ushort GetPanelSource(int x, int y)
        {
            ValidateCoordinates(x, y);
            return Panels[x, y].Source;
        }

        // Method to set a panel and its neighbors to 'high' state based on a number.
        public void SetPanel(int number)
        {
            ValidatePanelNumber(number);

            int x = (number - 1) % WallWidth;
            int y = (number - 1) / WallWidth;

            // If the pressed panel is already high, set it to low and return.
            if (Panels[x, y].IsHigh == 1)
            {
                Panels[x, y].SetLow();
                return;
            }

            // Check if the newly pressed panel is a neighbor of any high panel.
            bool isNeighborOfHighPanel = IsNeighborOfAnyHighPanel(x, y);

            // If the pressed panel isn't neighboring any high panel, reset all to low.
            if (!isNeighborOfHighPanel)
                ResetAllPanels();

            // Set the pressed panel to high.
            Panels[x, y].SetHigh();
        }

    // Ensures that provided panel coordinates are valid and within bounds.
    private void ValidateCoordinates(int x, int y)
    {
        if (x < 0 || x >= WallWidth || y < 0 || y >= WallHeight)
        {
            throw new ArgumentOutOfRangeException(
                "x", 
                "Invalid panel coordinates. X and Y must be within the range [0, {WallWidth - 1}] and [0, {WallHeight - 1}] respectively.");
        }
    }

    // Ensures that the provided panel number is valid.
    private void ValidatePanelNumber(int number)
    {
        if (number < 1 || number > (WallWidth * WallHeight))
        {
            throw new ArgumentOutOfRangeException(
                "number", 
                "Invalid panel number. Panel number must be in the range [1, {WallWidth * WallHeight}].");
        }
    }

        // Reset the state of all panels to 'Low'.
        private void ResetAllPanels()
        {
            for (int i = 0; i < WallWidth; i++)
            {
                for (int j = 0; j < WallHeight; j++)
                {
                    Panels[i, j].SetLow();
                }
            }
        }
    }

    // Represents a coordinate for a panel's position.
    public class PanelCoordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        // Constructor to set the X and Y coordinates.
        public PanelCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    // Represents the dimensions and position of a high panel in the video wall.
    public class HighPanelInfo
    {
        public int W { get; set; } // Width of the high panel layout.
        public int H { get; set; } // Height of the high panel layout.
        public int X { get; set; } // X position of the high panel relative to the layout.
        public int Y { get; set; } // Y position of the high panel relative to the layout.
    }
}
