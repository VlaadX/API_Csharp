using ApiTeste.Database;
using ApiTarefas.Servicos;
using ApiTarefas.Servicos.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração da string de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração dos serviços
ConfigureServices(builder.Services, connectionString);

var app = builder.Build();

// Configuração do pipeline de requisições
ConfigurePipeline(app);

app.Run();

void ConfigureServices(IServiceCollection services, string connectionString)
{
    services.AddDbContext<TarefasContext>(options =>
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 37)))
    );

    services.AddScoped<ITarefaServico, TarefaServico>();

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}
