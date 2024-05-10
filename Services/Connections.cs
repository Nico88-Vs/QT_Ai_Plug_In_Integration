using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace QuantowerPlugin_Decomplied
{
    public class Connections
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string postwithbody = "http://localhost:5000/list_models_path";
        private readonly string postwithbody_test = "https://jsonplaceholder.typicode.com/todos/1";

        //TODO: impl gli args di questi delegate
        public event EventHandler<string> message_recived;
        public event EventHandler<string> server_running;
        private readonly string scriptPath = @"C:\Users\user\source\repos\qt_con_test\qt_con_test.py";

        public virtual void oN_Server_Status_Update(string message)
        {
            server_running?.Invoke(this, message);
        }


        public virtual void oN_Message_Recived(string message)
        {
            message_recived?.Invoke(this, message);
        }
        public void RunPythonScript()
        {
            //TODO: implementare un evento per verificare l esecuzione del server

            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "C:\\Users\\user\\anaconda3\\envs\\enn_tre\\python.exe"; // Assicurati che python sia nel PATH o specifica il percorso completo.
                start.Arguments = this.scriptPath; // Percorso dello script Python.
                start.UseShellExecute = false;
                Process process = Process.Start(start);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore nell'esecuzione dello script Python: " + ex.Message);
            }
        }

        public void StartGetPats(string endpoint)
        {
            Task.Run(async () => await Get_Pats(endpoint));
        }

        public async Task Get_Pats(string endpoint)
        {
            //TODO: implementare un enum per gli endpoint
            //TODO: aggiungere la gestione di post and gest and so
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                string responseBody = await response.Content.ReadAsStringAsync();
                oN_Message_Recived(responseBody);
            }
            catch (HttpRequestException e)
            {
                oN_Message_Recived($"Errore di  {e}");
            }
            catch (Exception e)
            {
                oN_Message_Recived($"Errore sconosciuto {e}");
            }

        }
    }

}