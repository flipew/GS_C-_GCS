using System;
using System.Collections.Generic;
using System.Linq; 

namespace GuardiaoDeComandos
{
    class Program
    {
        // Variáveis para manter o estado da aplicação
        private static List<Regra> _regrasDoSistema = new List<Regra>();
        private static Usuario? _usuarioLogado = null;
        private static GerenciadorDeRegras _gerenciadorDeRegras = new GerenciadorDeRegras(); 
        private static Autenticador _autenticador = new Autenticador();
        private static MotorDeValidacao _motorDeValidacao = new MotorDeValidacao();

        // Contadores para o painel de status
        private static long _totalComandosProcessados = 0;
        private static long _comandosValidos = 0;
        private static long _comandosInvalidos = 0;
        private static long _comandosSuspeitos = 0;
        private static string _ultimoAlertaTexto = "Nenhum alerta recente.";

        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao Guardião de Comandos de Subestação (GCS)!");
            Logger.RegistrarInfo("Aplicação GCS iniciada.");

            // Carregar regras na inicialização
            _regrasDoSistema = _gerenciadorDeRegras.CarregarRegras();
            if (_regrasDoSistema.Count == 0)
            {
                Console.WriteLine("AVISO: Nenhuma regra de validação foi carregada. O sistema pode não funcionar como esperado.");
                // Logger já registrou o erro/aviso no método CarregarRegras
            }
            else
            {
                Console.WriteLine($"{_regrasDoSistema.Count} regra(s) carregada(s).");
            }

            //  Loop de Autenticação
            while (_usuarioLogado == null)
            {
                Console.Write("Usuário: ");
                string? nomeUsuario = Console.ReadLine();
                Console.Write("Senha: ");
                string? senha = Console.ReadLine(); 

                if (string.IsNullOrWhiteSpace(nomeUsuario) || string.IsNullOrWhiteSpace(senha))
                {
                    Console.WriteLine("Nome de usuário e senha não podem ser vazios.");
                    continue;
                }
                _usuarioLogado = _autenticador.Autenticar(nomeUsuario, senha);

                if (_usuarioLogado == null)
                {
                    Console.WriteLine("Login falhou. Tente novamente ou digite 'sair' no usuário para fechar.");
                    if (nomeUsuario.Equals("sair", StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.RegistrarInfo("Usuário optou por sair na tela de login.");
                        Console.WriteLine("Saindo do GCS...");
                        return; 
                    }
                }
            }

            Console.WriteLine($"\nLogin bem-sucedido! Bem-vindo, {_usuarioLogado.NomeUsuario}!");
            Logger.RegistrarInfo($"Usuário '{_usuarioLogado.NomeUsuario}' logado com sucesso.");

            // Loop Principal da Aplicação
            bool continuarExecutando = true;
            while (continuarExecutando)
            {
                Console.WriteLine("\n-------------------------------------------");
                Console.WriteLine("Opções disponíveis:");
                Console.WriteLine("1. Validar Comando");
                Console.WriteLine("2. Ver Estatísticas (Dashboard)");
                Console.WriteLine("3. Sair");
                Console.Write("Escolha uma opção: ");
                string? escolha = Console.ReadLine();

                switch (escolha)
                {
                    case "1":
                        ProcessarEntradaDeComando();
                        break;
                    case "2":
                        ExibirEstatisticas();
                        break;
                    case "3":
                        continuarExecutando = false;
                        Logger.RegistrarInfo($"Usuário '{_usuarioLogado.NomeUsuario}' deslogou.");
                        Console.WriteLine("Saindo do GCS...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        Logger.RegistrarAlerta($"Usuário '{_usuarioLogado.NomeUsuario}' inseriu opção inválida no menu: {escolha}");
                        break;
                }
            }
            Logger.RegistrarInfo("Aplicação GCS finalizada.");
        }

        static void ProcessarEntradaDeComando()
        {
            Console.WriteLine("\n--- Validação de Comando ---");
            Console.WriteLine("Digite o comando no formato: ORIGEM EQUIPAMENTO ACAO [PARAM1=VALOR1 PARAM2=VALOR2 ...]");
            Console.Write("Comando: ");
            string? inputComando = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputComando))
            {
                Console.WriteLine("Comando vazio. Nenhuma ação tomada.");
                Logger.RegistrarAlerta("Tentativa de validar comando vazio.");
                return;
            }

            // Lógica de Parsing do Comando 
            Comando? comandoParaValidar = ParseComando(inputComando);

            if (comandoParaValidar == null)
            {
                Console.WriteLine("Formato de comando inválido. Não foi possível analisar.");
                Logger.RegistrarErro($"Não foi possível parsear o comando: {inputComando}");
                return;
            }

            // Validar o comando
            _totalComandosProcessados++;
            ResultadoValidacao resultado = _motorDeValidacao.ValidarComando(comandoParaValidar, _regrasDoSistema);

            // Exibir resultado
            Console.WriteLine($"\n--- Resultado da Validação ---");
            Console.WriteLine($"Comando Original: {resultado.ComandoAnalisado.TextoOriginalComando}");
            Console.WriteLine($"Status: {resultado.Status}");
            Console.WriteLine($"Mensagem: {resultado.MensagemAlerta}");

            if (resultado.RegrasVioladas.Any()) 
            {
                Console.WriteLine("Regras Violadas:");
                foreach (var regra in resultado.RegrasVioladas)
                {
                    Console.WriteLine($"  - ID: {regra.IdRegra}, Descrição: {regra.Descricao}, Severidade: {regra.Severidade}");
                }
            }

            // Atualizar estatísticas
            if (resultado.Status == "VALIDO") _comandosValidos++;
            else if (resultado.Status == "INVALIDO") _comandosInvalidos++;
            else if (resultado.Status == "SUSPEITO") _comandosSuspeitos++;

            if (resultado.Status != "VALIDO")
            {
                _ultimoAlertaTexto = $"[{resultado.Timestamp:HH:mm:ss}] {resultado.Status}: {resultado.MensagemAlerta}";
            }
        }

        static Comando? ParseComando(string input)
        {

            string[] partes = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length < 3) 
            {
                return null; 
            }

            string origem = partes[0];
            string equipamento = partes[1];
            string acao = partes[2];
            var parametros = new Dictionary<string, string>();

            for (int i = 3; i < partes.Length; i++)
            {
                string[] parParametro = partes[i].Split('=', 2); 
                if (parParametro.Length == 2)
                {
                    parametros[parParametro[0].ToUpperInvariant()] = parParametro[1];
                }
                else
                {
                    Logger.RegistrarAlerta($"Ignorando parte malformada do comando durante o parse: '{partes[i]}'");
                }
            }
            return new Comando(origem, equipamento, acao, input, parametros);
        }

        static void ExibirEstatisticas()
        {
            Console.WriteLine("\n--- Estatísticas do Sistema (Dashboard) ---");
            Console.WriteLine($"Total de Comandos Processados: {_totalComandosProcessados}");
            Console.WriteLine($"Comandos Válidos: {_comandosValidos}");
            Console.WriteLine($"Comandos Inválidos: {_comandosInvalidos}");
            Console.WriteLine($"Comandos Suspeitos: {_comandosSuspeitos}");
            Console.WriteLine($"Último Alerta Significativo: {_ultimoAlertaTexto}");
            Logger.RegistrarInfo($"Usuário '{_usuarioLogado?.NomeUsuario ?? "N/A"}' visualizou estatísticas.");
        }
    }
}