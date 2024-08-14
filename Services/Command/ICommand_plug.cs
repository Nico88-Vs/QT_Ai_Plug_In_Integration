using System;

namespace Services.Command
{
    public interface ICommand_plug
    {
        void Execute(Object e = null, Delegate delegato = null);
        public EventArgsCommand argsCommand { get; }
    }
}
