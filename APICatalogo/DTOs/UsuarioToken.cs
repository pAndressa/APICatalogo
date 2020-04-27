using System;

namespace APICatalogo.DTOs
{
    public class UsuarioToken
    {
        public bool Authenticated { get; set; }
        public DateTime Experiention { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
