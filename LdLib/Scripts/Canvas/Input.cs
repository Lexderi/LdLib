using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Input;

namespace LdLib
{
    public static class Input
    {
        private static IInputContext? input;
        
        public static bool MouseDown { get; private set; }

        internal static void Initialize(IInputContext inputContext)
        {
            input = inputContext;

            // assign mouse events
            if(input.Mice.Count == 0) Debug.WriteLine("WARNING: No connected mice found");
            foreach (IMouse mouse in input.Mice)
            {
                mouse.MouseDown += OnMouseDown;
                mouse.MouseUp += OnMouseUp;
            }

        }

        private static void OnMouseDown(IMouse mouse, MouseButton mouseButton)
        {
            MouseDown = true;
        }

        private static void OnMouseUp(IMouse mouse, MouseButton mouseButton)
        {
            MouseDown = false;
        }
    }
}
