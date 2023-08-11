using System;

namespace JornalApi.Models
{
    public class Noticia
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataPublicacao { get; set; }
        public bool Favorita { get; internal set; }
    }
}
