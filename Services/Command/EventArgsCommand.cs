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

            this.Initialize();
        }

        private void Initialize()
        {
            switch (this.Renderable_Obj.B_Usage)
            {
                case UI_usage.Lunch_Python:
                    var run_p = new Run_Python_Command();
                    this.Command = new ChangePageCommand();
                    this.Command.Execute(Pages.Started);
                    break;
                case UI_usage.Back_ToStart:
                    this.Command = new ChangePageCommand();
                    this.Command.Execute(Pages.Run);
                    break;
                default:
                    this.Command = new VoidCommand();
                    break;
            }
        }
    }
}
