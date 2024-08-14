using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Command
{
    public class VoidCommand : ICommand_plug
    {
        public EventArgsCommand argsCommand { get; private set; }
        public Pages DestinatioPage { get; set; }

        public void Execute(object e = null, Delegate delegato = null)
        {
            try
            {
                this.DestinatioPage = (Pages)e;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
