using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Command
{
    public class Run_Command : ICommand_plug
    {
        public EventArgsCommand argsCommand { get; private set; }

        public event EventHandler<EventArgsCommand> Handler;

        public void Execute(EventArgsCommand e)
        {
            this.argsCommand = e;
        }

        public void Execute(object e, Delegate delegato) => throw new NotImplementedException();
        public void On_Execute(EventArgsCommand e) => throw new NotImplementedException();
    }
}
