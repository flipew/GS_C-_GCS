# Guardião de Comandos de Subestação (GCS)

Projeto desenvolvido para a disciplina de **C# Software Development** do curso de Engenharia de Software.

## 🎯 Finalidade do Sistema

O **Guardião de Comandos de Subestação (GCS)** é uma aplicação de console desenvolvida em C# que simula um sistema de validação e monitoramento de segurança para comandos enviados a sistemas de controle de subestações elétricas.

O principal objetivo é demonstrar como uma camada de software pode atuar de forma preventiva contra operações indevidas ou ataques cibernéticos durante momentos críticos, como o de restabelecimento de energia. O sistema analisa cada comando simulado contra um conjunto de regras de segurança flexíveis e configuráveis, classificando-os como válidos, suspeitos ou inválidos, e registrando todas as atividades para fins de auditoria.

## 🚀 Instruções de Execução

Siga os passos abaixo para executar o projeto em sua máquina local.

### Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) (versão 8.0 ou compatível).
- [Git](https://git-scm.com/downloads/).


### Configuração

Antes de executar, é crucial garantir que o arquivo de configuração de regras (`regras.json`) esteja presente e configurado.

- Na raiz do projeto, crie ou verifique o arquivo `regras.json`. Exemplo de conteúdo:
  ```json
  [
    {
      "IdRegra": "REG_TENSAO_ALTA",
      "Descricao": "Tensão acima do limite para DISJUNTOR_01",
      "Tipo": "FAIXA_PARAMETRO",
      "IdEquipamentoAlvo": "DISJUNTOR_01",
      "Parametro": "TENSAO",
      "ValorMax": 230.0,
      "Severidade": "ALTA"
    },
    {
      "IdRegra": "REG_ORIGEM_BLOQUEADA",
      "Descricao": "Comandos da origem SCRIPT_MALICIOSO são bloqueados",
      "Tipo": "ORIGEM_INVALIDA",
      "Parametro": "SCRIPT_MALICIOSO",
      "Severidade": "ALTA"
    }
  ]

## 📁 Estrutura de Pastas e Arquivos

A estrutura do projeto é organizada da seguinte forma, seguindo o princípio da responsabilidade única para cada classe:

- **`GuardiãoDeComandos/`** (Pasta da Solução)
  - `GuardiãoDeComandos.sln` - Arquivo da Solução Visual Studio.
  - **`GuardiãoDeComandos/`** (Pasta do Projeto)
    - `GuardiãoDeComandos.csproj` - Arquivo de definição do projeto C#.
    - `Program.cs` - Ponto de entrada da aplicação, controla o menu e o fluxo principal.
    - `Autenticador.cs` - Contém a lógica de autenticação de usuários.
    - `Comando.cs` - Modelo de dados que representa um comando a ser validado.
    - `GerenciadorDeRegras.cs` - Responsável por carregar as regras do arquivo `regras.json`.
    - `Logger.cs` - Classe utilitária para registrar eventos em um arquivo de log.
    - `MotorDeValidacao.cs` - Núcleo do sistema, contém a lógica para validar comandos contra as regras.
    - `Regra.cs` - Modelo de dados que representa uma regra de segurança.
    - `ResultadoValidacao.cs` - Modelo de dados que encapsula o resultado de uma validação.
    - `Usuario.cs` - Modelo de dados que representa um usuário do sistema.
    - `regras.json` - Arquivo de configuração das regras de segurança.
    - `gcs_audit_log.txt` - (Gerado na execução) Arquivo de log com o registro de todas as atividades.
