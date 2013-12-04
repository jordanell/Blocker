using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Blocker
{
    class InputHandler
    {
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

        private List<GestureSample> currentGestures;

        private InputHandler()
        {
            currentGestures = new List<GestureSample>();
        }

        public void DisableGestures()
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }

        public void EnableGameGestures()
        {
            TouchPanel.EnabledGestures =
                GestureType.FreeDrag |GestureType.DoubleTap | GestureType.Tap | GestureType.DragComplete | GestureType.Hold;
        }

        public void Clear()
        {
            currentGestures.Clear();
            while (TouchPanel.IsGestureAvailable)
                TouchPanel.ReadGesture();
        }

        public List<GestureSample> Taps()
        {
            List<GestureSample> taps = new List<GestureSample>();
            foreach (GestureSample gs in currentGestures)
            {
                if (gs.GestureType == GestureType.Tap)
                    taps.Add(gs);
            }
            return taps;
        }

        public Vector2 DoubleTap()
        {
            foreach (GestureSample gs in currentGestures)
            {
                if (gs.GestureType == GestureType.DoubleTap)
                    return gs.Position;
            }
            return Vector2.Zero;
        }

        public Direction PlayerDirection()
        {
            Vector2 delta = Vector2.Zero;
            foreach (GestureSample gs in currentGestures)
            {
                if (gs.GestureType == GestureType.FreeDrag)
                {
                    if (gs.Delta.Length() > delta.Length())
                        delta = gs.Delta;
                }
            }

            if (delta == Vector2.Zero)
                return Direction.None;
            else
                return DirectionOfVector(delta);
        }

        private Direction DirectionOfVector(Vector2 delta)
        {
            if (Math.Abs(delta.Y) >= Math.Abs(delta.X))
            {
                if (delta.Y < 0)
                    return Direction.Up;
                else
                    return Direction.Down;
            }
            else
            {
                if (delta.X < 0)
                    return Direction.Left;
                else
                    return Direction.Right;
            }
        }

        public void Update()
        {
            currentGestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                currentGestures.Add(gs);
                System.Diagnostics.Debug.WriteLine(gs.GestureType.ToString());
            }
        }
    }
}
