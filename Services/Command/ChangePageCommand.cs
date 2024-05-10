using System;
using QuantowerPlugin_Decomplied;

namespace Services.Command
{
    public class ChangePageCommand : ICommand_plug
    {
        public EventArgsCommand argsCommand { get; private set; }
        public Pages DestinatioPage { get; set; }


        public void Execute(object e, Delegate delegato = null)
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
