using System;
using System.Collections.Generic;
using System.Linq; // Pode ser útil para consultas mais complexas em regras ou parâmetros

namespace GuardiaoDeComandos
{
    public class MotorDeValidacao
    {
        public MotorDeValidacao()
        {
        }

        public ResultadoValidacao ValidarComando(Comando comando, List<Regra> todasAsRegras)
        {
            ResultadoValidacao resultado = new ResultadoValidacao(comando);
            resultado.Status = "VALIDO"; // Assume que o comando é válido inicialmente

            Logger.RegistrarInfo($"Iniciando validação para o comando: {comando.TextoOriginalComando}");


            foreach (var regra in todasAsRegras)
            {
                bool regraViolada = false;

                switch (regra.Tipo?.ToUpperInvariant()) 
                {
                    case "FAIXA_PARAMETRO":
                        regraViolada = ValidarRegraDeFaixaDeParametro(comando, regra);
                        break;

                    case "ORIGEM_INVALIDA":

                        if (comando.Origem.Equals("DESCONHECIDO", StringComparison.OrdinalIgnoreCase))
                        {

                            if (regra.Parametro != null && comando.Origem.Equals(regra.Parametro, StringComparison.OrdinalIgnoreCase))
                            {
                                regraViolada = true;
                            }
                            else if (regra.Parametro == null && comando.Origem.Equals("DESCONHECIDO", StringComparison.OrdinalIgnoreCase))
                            {

                                regraViolada = true;
                            }
                        }
                        break;

                    default:
                        Logger.RegistrarAlerta($"Tipo de regra desconhecido ou não implementado: '{regra.Tipo}' para a regra ID: {regra.IdRegra}");
                        break;
                }

                if (regraViolada)
                {
                    resultado.RegrasVioladas.Add(regra);

                    if (regra.Severidade?.ToUpperInvariant() == "ALTA")
                    {
                        resultado.Status = "INVALIDO";
                    }
                    else if (regra.Severidade?.ToUpperInvariant() == "MEDIA" && resultado.Status != "INVALIDO")
                    {
                        resultado.Status = "SUSPEITO";
                    }

                    resultado.MensagemAlerta += $"Violação da Regra ID {regra.IdRegra} ({regra.Descricao}). Severidade: {regra.Severidade}. ";
                    Logger.RegistrarAlerta($"Comando '{comando.TextoOriginalComando}' VIOLOU a regra ID {regra.IdRegra} ({regra.Descricao}).");
                }
            } 

            if (resultado.RegrasVioladas.Count == 0)
            {
                resultado.MensagemAlerta = "Comando validado com sucesso. Nenhuma regra violada.";
                Logger.RegistrarInfo($"Comando '{comando.TextoOriginalComando}' é considerado {resultado.Status}.");
            }
            else
            {
                Logger.RegistrarAlerta($"Comando '{comando.TextoOriginalComando}' finalizou com status {resultado.Status} devido a {resultado.RegrasVioladas.Count} violação(ões).");
            }

            return resultado;
        }


        private bool ValidarRegraDeFaixaDeParametro(Comando comando, Regra regra)
        {
            if (string.IsNullOrEmpty(regra.Parametro))
            {
                Logger.RegistrarErro($"Regra de Faixa de Parâmetro (ID: {regra.IdRegra}) não tem 'Parametro' definido.");
                return false; 
            }

            double? valorParametroComando = comando.ObterParametroComoDouble(regra.Parametro);

            if (!valorParametroComando.HasValue)
            {

                if (regra.ValorMin.HasValue || regra.ValorMax.HasValue)
                {
                    Logger.RegistrarAlerta($"Regra ID {regra.IdRegra}: Parâmetro '{regra.Parametro}' esperado pela regra não encontrado ou inválido no comando '{comando.TextoOriginalComando}'.");
                    return true; 
                }
                return false; 
            }

            // Verifica o limite mínimo, se definido na regra
            if (regra.ValorMin.HasValue && valorParametroComando.Value < regra.ValorMin.Value)
            {
                return true; 
            }

            // Verifica o limite máximo, se definido na regra
            if (regra.ValorMax.HasValue && valorParametroComando.Value > regra.ValorMax.Value)
            {
                return true;
            }

            return false; // Nenhuma violação de faixa de parâmetro encontrada
        }

    }
}