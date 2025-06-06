using System;
using System.IO; 

namespace GuardiaoDeComandos
{
    public static class Logger 
    {
        private static readonly string _logFileName = "gcs_audit_log.txt"; 
        private static readonly object _lock = new object(); 

        // Método para registrar uma mensagem informativa
        public static void RegistrarInfo(string mensagem)
        {
            EscreverNoLog("INFO", mensagem);
        }

        public static void RegistrarAlerta(string mensagem)
        {
            EscreverNoLog("ALERTA", mensagem);
        }

        // Método para registrar uma mensagem de erro, opcionalmente com uma exceção
        public static void RegistrarErro(string mensagem, Exception? ex = null) 
        {
            string mensagemCompleta = mensagem;
            if (ex != null)
            {
                mensagemCompleta += $"\n\tDetalhes da Exceção: {ex.Message}";
                if (ex.InnerException != null)
                {
                    mensagemCompleta += $"\n\tExceção Interna: {ex.InnerException.Message}";
                }
            }
            EscreverNoLog("ERRO", mensagemCompleta);
        }

        private static void EscreverNoLog(string nivel, string mensagem)
        {
            try
            {
                // prevenindo corrupção do arquivo se sua aplicação ficasse mais complexa (multi-thread)
                lock (_lock)
                {
                    string entradaLog = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{nivel}] - {mensagem}{Environment.NewLine}";

                    // Adiciona a entrada ao final do arquivo de log.
                    // File.AppendAllText cria o arquivo se ele não existir.
                    File.AppendAllText(_logFileName, entradaLog);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO CRÍTICO AO ESCREVER NO LOG: {ex.Message}");
                Console.WriteLine($"Mensagem original do log ({nivel}): {mensagem}");
            }
        }
    }
}