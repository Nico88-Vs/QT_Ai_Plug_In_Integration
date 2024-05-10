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
        private static Connections _connection;

        public Run_Python_Command()
        {
            _connection = new Connections();
        }

        public void Execute(object e, Delegate delegato = null) => throw new NotImplementedException();
    }
}
