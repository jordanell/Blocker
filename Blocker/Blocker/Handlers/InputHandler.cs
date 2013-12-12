using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Blocker
{
    /// <summary>
    /// InputHandler is a custom wrapper around the input functions of XNA. InputHandler
    /// has custom data structures for storing and manipulating input.
    /// </summary>
    class InputHandler
    {
        // InputHandler is a singleton class
        private static InputHandler instance;
        public static InputHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputHandler();
                return instance;
            }
        }

        // List of gestures for a current frame
        private List<GestureSample> currentGestures;

        /// <summary>
        /// Constructor for InputHandler. Since InputHandler is a singleton, this
        /// constructor is private.
        /// </summary>
        private InputHandler()
        {
            currentGestures = new List<GestureSample>();
        }

        /// <summary>
        /// Disables the gestures that are being sensed.
        /// </summary>
        public void DisableGestures()
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }

        /// <summary>
        /// Enables all gestures needed to be identified for Block3r.
        /// </summary>
        public void EnableGameGestures()
        {
            TouchPanel.EnabledGestures =
                GestureType.FreeDrag |GestureType.DoubleTap | GestureType.Tap | GestureType.DragComplete | GestureType.Hold;
        }

        /// <summary>
        /// Clears the list of currently captured gestures.
        /// </summary>
        public void Clear()
        {
            currentGestures.Clear();

            // Need to clear the XNA queue of gestures as well
            while (TouchPanel.IsGestureAvailable)
                TouchPanel.ReadGesture();
        }

        /// <summary>
        /// Returns a list of tap gestures that are currently stored by InputHandler.
        /// </summary>
        /// <returns>The list of tap gestures to be returned.</returns>
        public List<GestureSample> Taps()
        {
            List<GestureSample> taps = new List<GestureSample>();

            // Go over current gestures and get taps
            foreach (GestureSample gs in currentGestures)
            {
                if (gs.GestureType == GestureType.Tap)
                    taps.Add(gs);
            }
            return taps;
        }

        /// <summary>
        /// Returns the location of the first double tap gestures currently stored.
        /// </summary>
        /// <returns>The location of the double tap.</returns>
        public Vector2 DoubleTap()
        {
            foreach (GestureSample gs in currentGestures)
            {
                // Return the first available location of double tap
                if (gs.GestureType == GestureType.DoubleTap)
                    return gs.Position;
            }

            // Return 0 otherwise
            return Vector2.Zero;
        }

        /// <summary>
        /// Returns the largest grad gesture's direction currently stored.
        /// </summary>
        /// <returns>The direction to be returned</returns>
        public Direction DragDirection()
        {
            Vector2 delta = Vector2.Zero;

            // Look for largest drag gesture
            foreach (GestureSample gs in currentGestures)
            {
                if (gs.GestureType == GestureType.FreeDrag)
                {
                    if (gs.Delta.Length() > delta.Length())
                        delta = gs.Delta;
                }
            }

            // Return none if no gesture found or return direction
            if (delta == Vector2.Zero)
                return Direction.None;
            else
                return DirectionOfVector(delta);
        }

        /// <summary>
        /// Based on the given delta vector, return the Direction of the
        /// of the X and Y. If the abs(y) is larger than abs(x) a up or down
        /// direction will be returned. Oposite is true for X.
        /// </summary>
        /// <param name="delta">The delta vector</param>
        /// <returns>The Direction to be returned</returns>
        private Direction DirectionOfVector(Vector2 delta)
        {
            // If Y is larger than X
            if (Math.Abs(delta.Y) >= Math.Abs(delta.X))
            {
                if (delta.Y < 0)
                    return Direction.Up;
                else
                    return Direction.Down;
            }
            // If X is larger than Y
            else
            {
                if (delta.X < 0)
                    return Direction.Left;
                else
                    return Direction.Right;
            }
        }

        /// <summary>
        /// Updates the current guestures that are stored by InputHandler.
        /// This method first clears the available gestures already stored.
        /// </summary>
        public void Update()
        {
            // Don't use the clear funtion here as it wipes the XNA queue
            // which is needed for next step
            currentGestures.Clear();

            // Get new gestures
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                currentGestures.Add(gs);
            }
        }
    }
}
