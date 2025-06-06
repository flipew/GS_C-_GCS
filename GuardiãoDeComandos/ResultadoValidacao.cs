using System;
using System.Collections.Generic;

namespace GuardiaoDeComandos
{
    public class ResultadoValidacao
    {
        public Comando ComandoAnalisado { get; private set; }

        // Status da validação: "VALIDO", "INVALIDO", "SUSPEITO"
        public string Status { get; set; }

        // Lista de regras que foram violadas por este comando 
        public List<Regra> RegrasVioladas { get; set; }

        // Mensagem de alerta consolidada, se houver violações
        public string MensagemAlerta { get; set; }

        // Data e hora em que a validação ocorreu
        public DateTime Timestamp { get; private set; }

        // Construtor
        public ResultadoValidacao(Comando comandoAnalisado)
        {
            ComandoAnalisado = comandoAnalisado;
            Timestamp = DateTime.Now; 
            Status = "PENDENTE"; 
            RegrasVioladas = new List<Regra>();
            MensagemAlerta = string.Empty;
        }
    }
}