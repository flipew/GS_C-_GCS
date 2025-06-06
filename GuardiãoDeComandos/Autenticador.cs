using System;
using System.Collections.Generic;
using System.Linq; 

namespace GuardiaoDeComandos
{
    public class Autenticador
    {
        private readonly List<Usuario> _usuariosValidos;

        public Autenticador()
        {
            _usuariosValidos = new List<Usuario>
            {
                new Usuario("admin", "admin123"), // Usuário adm
                new Usuario("operador", "op456")   // Usuário op
            };
        }

        public Usuario? Autenticar(string nomeUsuario, string senha)
        {
            Logger.RegistrarInfo($"Tentativa de login para o usuário: {nomeUsuario}");


            Usuario? usuarioEncontrado = _usuariosValidos.FirstOrDefault(u =>
                u.NomeUsuario.Equals(nomeUsuario, StringComparison.OrdinalIgnoreCase));

            if (usuarioEncontrado != null)
            {  
                if (usuarioEncontrado.Senha == senha)
                {
                    Logger.RegistrarInfo($"Usuário '{nomeUsuario}' autenticado com sucesso.");
                    return usuarioEncontrado;
                }
                else
                {
                    Logger.RegistrarAlerta($"Tentativa de login falhou para o usuário '{nomeUsuario}': Senha incorreta.");
                    return null;
                }
            }
            else
            {
                Logger.RegistrarAlerta($"Tentativa de login falhou: Usuário '{nomeUsuario}' não encontrado.");
                return null;
            }
        }
    }
}