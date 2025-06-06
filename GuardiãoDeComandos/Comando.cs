using System;
using System.Collections.Generic; 

namespace GuardiaoDeComandos
{
    public class Comando
    {
        // De onde o comando se origina (ex: "OPERADOR_CONSOLE", "ARQUIVO_SCRIPT")
        public string Origem { get; set; }

        // O equipamento alvo do comando (ex: "DISJUNTOR_01", "TRANSFORMADOR_A")
        public string EquipamentoAlvo { get; set; }

        // A ação a ser executada (ex: "LIGAR", "DESLIGAR", "AJUSTAR_TENSAO")
        public string Acao { get; set; }

        // Parâmetros adicionais do comando, como chave-valor
        // Ex: {"TENSAO", "225.5"}, {"TEMPO_RELIGAMENTO", "30"}
        public Dictionary<string, string> Parametros { get; set; }

        public string TextoOriginalComando { get; set; }

        public Comando(string origem, string equipamentoAlvo, string acao, string textoOriginalComando, Dictionary<string, string>? parametros = null)
        {
            Origem = origem;
            EquipamentoAlvo = equipamentoAlvo;
            Acao = acao;
            TextoOriginalComando = textoOriginalComando;
            Parametros = parametros ?? new Dictionary<string, string>(); 
        }

        public Comando()
        {
            Origem = string.Empty;
            EquipamentoAlvo = string.Empty;
            Acao = string.Empty;
            TextoOriginalComando = string.Empty;
            Parametros = new Dictionary<string, string>();
        }

        // Método auxiliar para facilitar a obtenção de um parâmetro como double
        public double? ObterParametroComoDouble(string nomeParametro)
        {
            if (Parametros.TryGetValue(nomeParametro, out string? valorString))
            {
                if (double.TryParse(valorString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double valorDouble))
                {
                    return valorDouble;
                }
                else
                {
                    Logger.RegistrarAlerta($"Comando '{TextoOriginalComando}': Parâmetro '{nomeParametro}' ('{valorString}') não é um número double válido.");
                }
            }
            return null; 
        }

        // Método auxiliar para facilitar a obtenção de um parâmetro como int
        public int? ObterParametroComoInt(string nomeParametro)
        {
            if (Parametros.TryGetValue(nomeParametro, out string? valorString))
            {
                if (int.TryParse(valorString, out int valorInt))
                {
                    return valorInt;
                }
                else
                {
                    Logger.RegistrarAlerta($"Comando '{TextoOriginalComando}': Parâmetro '{nomeParametro}' ('{valorString}') não é um número inteiro válido.");
                }
            }
            return null; 
        }
    }
}