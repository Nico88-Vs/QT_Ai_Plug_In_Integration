using QuantowerPlugin_Decomplied;
using System;
using System.Collections.Generic;
using Front_End;

namespace Services.Command
{
    public class EventArgsCommand : EventArgs
    {
        public IRenderable Renderable_Obj { get; set; }
        public ICommand_plug Command { get; private set; }

        public EventArgsCommand(IRenderable renderable)
        {
            this.Renderable_Obj = renderable;

            switch (renderable.B_Usage)
            {
                case UI_usage.Lunch_Python:
                    this.Command = new RunPythonCommand();
                    break;
                case UI_usage.Run:
                    this.Command = new RunPythonCommand();
                    break;
                default:
                    this.Command = new RunPythonCommand();
                    break;
            }
        }
    }
}
