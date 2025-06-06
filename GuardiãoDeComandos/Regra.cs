using System;

namespace GuardiaoDeComandos;

public class Regra
{
    // Identificador único da regra
    public string IdRegra { get; set; }

    // Descrição amigável da regra
    public string Descricao { get; set; }

    // Tipo da regra (ex: "FAIXA_PARAMETRO", "PADRAO_SUSPEITO_REPETICAO", "ORIGEM_INVALIDA")
    public string Tipo { get; set; }

    //Identificador do equipamento ao qual a regra se aplica especificamente
    public string? IdEquipamentoAlvo { get; set; } 

    //Nome do parâmetro do comando a ser verificado (ex: "TENSAO", "CORRENTE")
    public string? Parametro { get; set; }

    //Valor mínimo para regras de faixa de parâmetro
    public double? ValorMin { get; set; } 

    //Valor máximo para regras de faixa de parâmetro
    public double? ValorMax { get; set; }

    //Número de repetições para regras de padrão suspeito
    public int NumRepeticoes { get; set; }

    //Intervalo em segundos para regras de padrão suspeito
    public int IntervaloSegundos { get; set; }

    // Severidade da violação da regra (ex: "ALTA", "MEDIA", "BAIXA")
    public string Severidade { get; set; }

    // Construtor padrão 
    public Regra()
    {
        // Inicializações padrão, se necessário
        IdRegra = string.Empty;
        Descricao = string.Empty;
        Tipo = string.Empty;
        Severidade = string.Empty;
    }
}