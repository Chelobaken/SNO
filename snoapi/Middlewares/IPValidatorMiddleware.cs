using System.Net;


namespace SNO.API;

public class IPValidatorMiddleware
{
    private IPAddress[] allowedClients;
    private RequestDelegate next;
    private ILogger<IPValidatorMiddleware> logger;
    private IConfiguration appConfig;

    public IPValidatorMiddleware(RequestDelegate next, ILogger<IPValidatorMiddleware> logger, IConfiguration config)
    {
        this.appConfig = config;
        this.logger = logger;
        this.next = next;

        string[]? addresses = null;

        if (allowedClients == null)
        {
            addresses = appConfig["AllowedHosts"]?.Split(";");
            allowedClients = new IPAddress[addresses.Length];

            for (int i = 0; i < addresses.Length; i++)
                allowedClients[i] = IPAddress.Parse(addresses[i]);
        }
    }

    public async Task Invoke(HttpContext context)
    {
        IPAddress? clientAddress = context.Connection.RemoteIpAddress;

        if (!allowedClients.Contains(clientAddress))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
        else
        {
            await next.Invoke(context);
        }
    }
}