using System;
using QuantowerPlugin_Decomplied;
using Services;

namespace Services.Command
{
    public class RunPythonCommand : ICommand_plug
    {
        private readonly Connections Connection = new Connections();
        public EventArgsCommand argsCommand { get; private set; }
        public event EventHandler<EventArgsCommand> Handler;

        public void Execute(EventArgsCommand e)
        {
            // TODO: valutare la rimozione del evento
            // TODO: aggiungerer evento per valutare lo stato di .py
            this.argsCommand = e;
            Connection.RunPythonScript();
        }

        public void On_Execute(EventArgsCommand e)
        {
            Handler?.Invoke(this, e);
        }

    }
}
