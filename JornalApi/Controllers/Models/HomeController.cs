using Microsoft.AspNetCore.Mvc;
using JornalApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JornalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiasController : ControllerBase
    {
        // Lista de notícias como armazenamento em memória (substituir por banco de dados em produção)
        private static List<Noticia> _noticias = new List<Noticia>
        {
            new Noticia { Id = 1, Titulo = "Primeira Notícia", Conteudo = "Conteúdo da primeira notícia.", DataPublicacao = DateTime.Now.AddDays(-2) },
            new Noticia { Id = 2, Titulo = "Segunda Notícia", Conteudo = "Conteúdo da segunda notícia.", DataPublicacao = DateTime.Now.AddDays(-1) }
        };

        private static int _nextId = _noticias.Count + 1;

        // Retorna todas as notícias
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_noticias);
        }

        // Retorna uma notícia por ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var noticia = _noticias.FirstOrDefault(n => n.Id == id);
            if (noticia == null)
            {
                return NotFound();
            }
            return Ok(noticia);
        }

        // Cria uma nova notícia
        [HttpPost]
        public IActionResult Create(Noticia noticia)
        {
            noticia.Id = _nextId++;
            noticia.DataPublicacao = DateTime.Now;
            _noticias.Add(noticia);
            return CreatedAtAction(nameof(GetById), new { id = noticia.Id }, noticia);
        }

        // Atualiza uma notícia por ID
        [HttpPut("{id}")]
        public IActionResult Update(int id, Noticia noticiaAtualizada)
        {
            var index = _noticias.FindIndex(n => n.Id == id);
            if (index == -1)
            {
                return NotFound();
            }
            noticiaAtualizada.Id = id;
            noticiaAtualizada.DataPublicacao = _noticias[index].DataPublicacao;
            _noticias[index] = noticiaAtualizada;
            return NoContent();
        }

        // Exclui uma notícia por ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var noticia = _noticias.FirstOrDefault(n => n.Id == id);
            if (noticia == null)
            {
                return NotFound();
            }
            _noticias.Remove(noticia);
            return NoContent();
        }

        // Autocompletar títulos de notícias
        [HttpGet("autocomplete")]
        public IActionResult Autocomplete([FromQuery] string termo)
        {
            var titulosAutocompletar = _noticias
                .Where(n => n.Titulo.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .Select(n => n.Titulo)
                .ToList();

            return Ok(titulosAutocompletar);
        }

        // Retorna as notícias mais recentes
        [HttpGet("recentes")]
        public IActionResult GetRecentes()
        {
            var noticiasRecentes = _noticias
                .OrderByDescending(n => n.DataPublicacao)
                .Take(5)
                .ToList();

            return Ok(noticiasRecentes);
        }

        // Marca uma notícia como favorita
        [HttpPost("{id}/favoritar")]
        public IActionResult Favoritar(int id)
        {
            var noticia = _noticias.FirstOrDefault(n => n.Id == id);
            if (noticia == null)
            {
                return NotFound();
            }
            noticia.Favorita = true;
            return Ok();
        }

        // Busca notícias por faixa de datas
        [HttpGet("buscarPorData")]
        public IActionResult BuscarPorData([FromQuery] DateTime dataInicial, [FromQuery] DateTime dataFinal)
        {
            var noticiasPorData = _noticias
                .Where(n => n.DataPublicacao >= dataInicial && n.DataPublicacao <= dataFinal)
                .ToList();

            return Ok(noticiasPorData);
        }

        // Pesquisa notícia por ID
        [HttpGet("pesquisarPorId")]
        public IActionResult PesquisarPorId([FromQuery] int id)
        {
            var noticia = _noticias.FirstOrDefault(n => n.Id == id);
            if (noticia == null)
            {
                return NotFound();
            }
            return Ok(noticia);
        }
    }
}
