using System;

namespace GuardiaoDeComandos
{
    public class Usuario
    {

        // Usuário simples
        public string NomeUsuario { get; set; }

        // Senha 
        public string Senha { get; set; } 

        public Usuario(string nomeUsuario, string senha)
        {
            NomeUsuario = nomeUsuario;
            Senha = senha;
        }

        public Usuario()
        {
            NomeUsuario = string.Empty;
            Senha = string.Empty;
        }
    }
}