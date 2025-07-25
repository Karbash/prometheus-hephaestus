using Hephaestus.Domain.DTOs.Request;
using Hephaestus.Domain.DTOs.Response;
using Hephaestus.Application.Interfaces.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hephaestus.Controllers;

/// <summary>
/// Controller para opera��es seguras de banco de dados, permitindo a execu��o de queries SQL de consulta.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", Policy = "RequireMfa")]
public class DatabaseController : ControllerBase
{
    private readonly IExecuteQueryUseCase _executeQueryUseCase;
    private readonly ILogger<DatabaseController> _logger;

    /// <summary>
    /// Inicializa uma nova inst�ncia do <see cref="DatabaseController"/>.
    /// </summary>
    /// <param name="executeQueryUseCase">Caso de uso para executar queries no banco de dados.</param>
    /// <param name="logger">Logger para registro de eventos e erros.</param>
    public DatabaseController(IExecuteQueryUseCase executeQueryUseCase, ILogger<DatabaseController> logger)
    {
        _executeQueryUseCase = executeQueryUseCase;
        _logger = logger;
    }

    /// <summary>
    /// Executa uma query SQL de consulta (SELECT) no banco de dados.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite a execu��o de queries SQL do tipo **SELECT** para consulta de dados.
    /// � uma opera��o altamente privilegiada e requer que o usu�rio autenticado possua a role **Admin** e tenha passado pela valida��o de **MFA (Autentica��o Multifator)**.
    /// 
    /// Exemplo de requisi��o:
    /// ```json
    /// {
    ///   "query": "SELECT Id, Name, Email FROM Companies WHERE IsEnabled = TRUE LIMIT 10"
    /// }
    /// ```
    /// 
    /// Exemplo de resposta de sucesso (Status 200 OK):
    /// ```json
    /// {
    ///   "columns": ["Id", "Name", "Email"],
    ///   "rows": [
    ///     ["123e4567-e89b-12d3-a456-426614174001", "Empresa A", "contato@empresaA.com"],
    ///     ["987f6543-d2c1-b0a9-8765-43210fedcba9", "Empresa B", "info@empresaB.com"]
    ///   ],
    ///   "rowCount": 2
    /// }
    /// ```
    /// 
    /// Exemplo de erro de requisi��o inv�lida (Status 400 Bad Request):
    /// ```json
    /// {
    ///   "type": "[https://tools.ietf.org/html/rfc7231#section-6.5.1](https://tools.ietf.org/html/rfc7231#section-6.5.1)",
    ///   "title": "Bad Request",
    ///   "status": 400,
    ///   "detail": "Apenas comandos SELECT s�o permitidos. Query recebida: 'DELETE FROM Users'."
    /// }
    /// ```
    /// Exemplo de erro de autentica��o (Status 401 Unauthorized):
    /// ```
    /// (Nenhum corpo de resposta, apenas status 401)
    /// ```
    /// Exemplo de erro interno do servidor (Status 500 Internal Server Error):
    /// ```json
    /// {
    ///   "type": "[https://tools.ietf.org/html/rfc7231#section-6.6.1](https://tools.ietf.org/html/rfc7231#section-6.6.1)",
    ///   "title": "Internal Server Error",
    ///   "status": 500,
    ///   "detail": "Ocorreu um erro inesperado ao executar a query: Timeout de banco de dados."
    /// }
    /// ```
    /// </remarks>
    /// <param name="request">Um objeto <see cref="ExecuteQueryRequest"/> contendo a query SQL a ser executada.</param>
    /// <returns>Um <see cref="OkObjectResult"/> contendo os resultados da query em um <see cref="ExecuteQueryResponse"/>.</returns>
    [HttpPost("execute-query")]
    [SwaggerOperation(Summary = "Executa uma query SQL de consulta", Description = "Executa uma query SQL de consulta (SELECT) e retorna os resultados. **Requer autentica��o com Role='Admin' e valida��o MFA.**")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExecuteQueryResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))] // Para erros de query inv�lida (n�o SELECT)
    [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Removido Type=typeof(object)
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Removido Type=typeof(object)
    public async Task<IActionResult> ExecuteQuery([FromBody] ExecuteQueryRequest request)
    {
        var response = await _executeQueryUseCase.ExecuteAsync(request);
        return Ok(response);
    }
}
