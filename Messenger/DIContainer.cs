using Autofac;
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
                .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Hour)
                .CreateLogger())
            .AsSelf().AsImplementedInterfaces().SingleInstance();

        builder.RegisterType<ExceptionMiddleware>().SingleInstance();
        base.Load(builder);
    }
}