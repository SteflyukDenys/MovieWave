using System.Net;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Result;
using ILogger = Serilog.ILogger;
namespace MovieWave.API.Middlewares;

public class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger _logger;

	public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext httpContext)
	{
		try
		{
			await _next(httpContext);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(httpContext, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
	{
		_logger.Error(ex, ex.Message);

		var errorMessage = ex.Message;
		var response = ex switch
		{
			UnauthorizedAccessException _ => new BaseResult()
			{ ErrorMessage = errorMessage, ErrorCode = (int)HttpStatusCode.Unauthorized },
			_ => new BaseResult()
			{ ErrorMessage = "Internal server error. Please retry later", ErrorCode = (int)HttpStatusCode.InternalServerError }
		};

		httpContext.Response.ContentType = "application/json";
		httpContext.Response.StatusCode = (int)response.ErrorCode;
		await httpContext.Response.WriteAsJsonAsync(response);
	}
}