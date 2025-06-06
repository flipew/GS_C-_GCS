using System;
using System.Collections.Generic;
using System.IO; 
using System.Text.Json; 

namespace GuardiaoDeComandos
{
    public class GerenciadorDeRegras
    {
        private readonly string _caminhoArquivoRegras;

        public GerenciadorDeRegras(string caminhoArquivoRegras = "regras.json")
        {
            _caminhoArquivoRegras = caminhoArquivoRegras;
        }

        public List<Regra> CarregarRegras()
        {
            Logger.RegistrarInfo($"Tentando carregar regras de: {_caminhoArquivoRegras}");

            if (!File.Exists(_caminhoArquivoRegras))
            {
                Logger.RegistrarErro($"Arquivo de regras não encontrado: {_caminhoArquivoRegras}");
                return new List<Regra>();
            }

            try
            {
                string conteudoJson = File.ReadAllText(_caminhoArquivoRegras);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<Regra>? regras = JsonSerializer.Deserialize<List<Regra>>(conteudoJson, options);

                if (regras != null)
                {
                    Logger.RegistrarInfo($"{regras.Count} regra(s) carregada(s) com sucesso de {_caminhoArquivoRegras}.");
                    return regras;
                }
                else
                {
                    Logger.RegistrarErro($"Desserialização do arquivo '{_caminhoArquivoRegras}' resultou em lista nula.");
                    return new List<Regra>();
                }
            }
            catch (JsonException jsonEx)
            {
                Logger.RegistrarErro($"Erro de formato JSON ao ler '{_caminhoArquivoRegras}'. Detalhes: {jsonEx.Message}", jsonEx);
                return new List<Regra>();
            }
            catch (Exception ex)
            {
                Logger.RegistrarErro($"Erro inesperado ao carregar regras de '{_caminhoArquivoRegras}'. Detalhes: {ex.Message}", ex);
                return new List<Regra>();
            }
        }
    }
}