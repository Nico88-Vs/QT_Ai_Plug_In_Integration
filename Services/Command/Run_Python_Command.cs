using QuantowerPlugin_Decomplied;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Command
{
    public class Run_Python_Command : ICommand_plug
    {
        public EventArgsCommand argsCommand {  get; private set; }
        private  Connections _connection = new Connections();

        public Run_Python_Command()
        {
            //TODO: SOSPESO
            //this.Execute();
            int x = 2;
        }

        private void _connection_message_recived(object sender, string e) => throw new NotImplementedException();
        private void _connection_server_running(object sender, string e) => throw new NotImplementedException();
        public void Execute(object e = null, Delegate delegato = null)
        {
            _connection.RunPythonScript();
        }
    }
}
