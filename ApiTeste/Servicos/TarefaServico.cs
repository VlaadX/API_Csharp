using ApiTeste.Database;
using ApiTarefas.Dto;
using ApiTarefas.Model.Erros;
using ApiTarefas.Models;
using ApiTarefas.Servicos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiTarefas.Servicos;

public class TarefaServico : ITarefaServico
{
    private readonly TarefasContext _db;

    public TarefaServico(TarefasContext db)
    {
        _db = db;
    }

    public List<Tarefa> Lista(int page = 1)
    {
        page = page < 1 ? 1 : page;
        const int limit = 10;
        int offset = (page - 1) * limit;

        return _db.Tarefas.Skip(offset).Take(limit).ToList();
    }

    public Tarefa Incluir(TarefaDto tarefaDto)
    {
        ValidateTarefaDto(tarefaDto);

        var tarefa = new Tarefa
        {
            Titulo = tarefaDto.Titulo,
            Descricao = tarefaDto.Descricao,
            Concluida = tarefaDto.Concluida,
        };

        _db.Tarefas.Add(tarefa);
        _db.SaveChanges();

        return tarefa;
    }

    public Tarefa Update(int id, TarefaDto tarefaDto)
    {
        ValidateTarefaDto(tarefaDto);

        var tarefaDb = _db.Tarefas.Find(id);
        if (tarefaDb == null)
        {
            throw new TarefaErro("Tarefa não encontrada");
        }

        tarefaDb.Titulo = tarefaDto.Titulo;
        tarefaDb.Descricao = tarefaDto.Descricao;
        tarefaDb.Concluida = tarefaDto.Concluida;

        _db.Tarefas.Update(tarefaDb);
        _db.SaveChanges();

        return tarefaDb;
    }

    public Tarefa BuscaPorId(int id)
    {
        var tarefaDb = _db.Tarefas.Find(id);
        if (tarefaDb == null)
        {
            throw new TarefaErro("Tarefa não encontrada");
        }

        return tarefaDb;
    }

    public void Delete(int id)
    {
        var tarefaDb = _db.Tarefas.Find(id);
        if (tarefaDb == null)
        {
            throw new TarefaErro("Tarefa não encontrada");
        }

        _db.Tarefas.Remove(tarefaDb);
        _db.SaveChanges();
    }

    private void ValidateTarefaDto(TarefaDto tarefaDto)
    {
        if (string.IsNullOrEmpty(tarefaDto.Titulo))
        {
            throw new TarefaErro("O título da tarefa não pode ser vazio");
        }
    }
}
