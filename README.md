Guardião de Comandos de Subestação (GCS)
Projeto desenvolvido para a disciplina de C# Software Development.

🎯 Finalidade do Sistema
O Guardião de Comandos de Subestação (GCS) é uma aplicação de console desenvolvida em C# que simula um sistema de validação e monitoramento de segurança para comandos enviados a sistemas de controle de subestações elétricas.

O principal objetivo é demonstrar como uma camada de software pode atuar de forma preventiva contra operações indevidas ou ataques cibernéticos durante momentos críticos, como o de restabelecimento de energia. O sistema analisa cada comando simulado contra um conjunto de regras de segurança flexíveis e configuráveis, classificando-os como válidos, suspeitos ou inválidos, e registrando todas as atividades para fins de auditoria.

🚀 Instruções de Execução
Siga os passos abaixo para executar o projeto em sua máquina local.

Pré-requisitos
.NET SDK (versão 8.0 ou compatível).
Git.
As credenciais de usuário para login estão definidas no código (hardcoded) na classe Autenticador.cs. As credenciais padrão são:

Usuário: admin | Senha: admin123
Usuário: operador | Senha: op456

📦 Dependências
O projeto foi construído utilizando as bibliotecas padrão do .NET SDK e não possui dependências externas (pacotes NuGet de terceiros). As principais tecnologias do framework utilizadas são:

.NET 8.0 (ou a versão que você usou)
System.Text.Json: Para manipulação e desserialização do arquivo de regras.
System.IO: Para leitura e escrita de arquivos (configuração de regras e log de auditoria).

📁 Estrutura de Pastas e Arquivos
A estrutura do projeto é organizada da seguinte forma, seguindo o princípio da responsabilidade única para cada classe:

GuardiãoDeComandos/
│
├── GuardiãoDeComandos.sln        # Arquivo da Solução Visual Studio
│
└── GuardiãoDeComandos/
    ├── GuardiãoDeComandos.csproj   # Arquivo de definição do projeto C#
    ├── Program.cs                  # Ponto de entrada da aplicação, controla o menu e o fluxo principal.
    ├── Autenticador.cs             # Contém a lógica de autenticação de usuários.
    ├── Comando.cs                  # Modelo de dados que representa um comando a ser validado.
    ├── GerenciadorDeRegras.cs      # Responsável por carregar as regras do arquivo `regras.json`.
    ├── Logger.cs                   # Classe utilitária para registrar eventos em um arquivo de log.
    ├── MotorDeValidacao.cs         # Núcleo do sistema, contém a lógica para validar comandos contra as regras.
    ├── Regra.cs                    # Modelo de dados que representa uma regra de segurança.
    ├── ResultadoValidacao.cs       # Modelo de dados que encapsula o resultado de uma validação.
    ├── Usuario.cs                  # Modelo de dados que representa um usuário do sistema.
    ├── regras.json                 # Arquivo de configuração das regras de segurança.
    └── gcs_audit_log.txt           # (Gerado na execução) Arquivo de log com o registro de todas as atividades.
