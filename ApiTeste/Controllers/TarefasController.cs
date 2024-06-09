using ApiTeste.Database;
using ApiTarefas.Dto;
using ApiTarefas.Model.Erros;
using ApiTarefas.Models;
using ApiTarefas.ModelViews;
using ApiTarefas.Servicos;
using ApiTarefas.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiTarefas.Controllers;

[ApiController]
[Route("tarefas")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaServico _servico;

    public TarefasController(ITarefaServico servico)
    {
        _servico = servico;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] int page = 1)
    {
        var tarefas = _servico.Lista(page);
        return Ok(tarefas);
    }

    [HttpPost]
    public IActionResult Create([FromBody] TarefaDto tarefaDto)
    {
        try
        {
            var tarefa = _servico.Incluir(tarefaDto);
            return CreatedAtAction(nameof(Show), new { id = tarefa.Id }, tarefa);
        }
        catch (TarefaErro erro)
        {
            return BadRequest(new ErroView { Mensagem = erro.Message });
        }
    }

    [HttpGet("{id}")]
    public IActionResult Show([FromRoute] int id)
    {
        try
        {
            var tarefaDb = _servico.BuscaPorId(id);
            return Ok(tarefaDb);
        }
        catch (TarefaErro erro)
        {
            return NotFound(new ErroView { Mensagem = erro.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] TarefaDto tarefaDto)
    {
        try
        {
            var tarefaDb = _servico.Update(id, tarefaDto);
            return Ok(tarefaDb);
        }
        catch (TarefaErro erro)
        {
            return BadRequest(new ErroView { Mensagem = erro.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        try
        {
            _servico.Delete(id);
            return NoContent();
        }
        catch (TarefaErro erro)
        {
            return BadRequest(new ErroView { Mensagem = erro.Message });
        }
    }
}
