
using System.Net;
using Newtonsoft.Json;

namespace Messenger;

internal class ExceptionMiddleware
{
    private readonly Serilog.ILogger logger;

    public ExceptionMiddleware(Serilog.ILogger log)
    {
        logger = log;
        log.Debug("I am here");
    }
    
    public async Task Handle(HttpContext context, Func<Task> action)
    {
        try
        {
            await action().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            var response = context.Response;
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Unexpected error";
            if (e is ApiException apiException)
            {
                statusCode = apiException.StatusCode;
                message = apiException.Message;
            }

            LogException(statusCode, e);

            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";
            await response.WriteAsync(
                    JsonConvert.SerializeObject(
                        new Error
                        {
                            StatusCode = response.StatusCode,
                            Message = message
                        }))
                .ConfigureAwait(false);
        }
    }

    private void LogException(HttpStatusCode statusCode, Exception message)
    {
        //TODO eventId добавить надо
        switch (statusCode) 
        {
            case HttpStatusCode.NotFound:
            case HttpStatusCode.Forbidden:
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.BadRequest:
                logger.Warning(message, "some template");
                break;
            default:
                logger.Error(message, "some template");
                break;
        }
    }
}

public class Error
{
    public int StatusCode;
    public string Message;
}

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
    {
        this.StatusCode = statusCode;
    }
}