using Autofac;
using Messenger.Authorization;
using Serilog;

namespace Messenger;

public class DIContainer : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx =>
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger())
            .AsSelf().AsImplementedInterfaces().SingleInstance();

        builder.RegisterType<ExceptionMiddleware>().SingleInstance();
        builder.RegisterType<AuthSidProvider>().SingleInstance();
        builder.RegisterType<SessionStateProvider>().SingleInstance();
        builder.RegisterType<AuthorizationService>().AsSelf().AsImplementedInterfaces();
        base.Load(builder);
    }
}