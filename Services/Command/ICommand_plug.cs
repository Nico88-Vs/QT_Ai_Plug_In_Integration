using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Command
{
    public interface ICommand_plug
    {
        void Execute(EventArgsCommand e);
        public EventArgsCommand argsCommand { get; }
        public event EventHandler<EventArgsCommand> Handler;
        public abstract void On_Execute(EventArgsCommand e);
    }
}
