using Autofac;
using Messenger.Authorization;
using Messenger.Controllers;
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
        builder.RegisterType<AuthSidProvider>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<SessionStateProvider>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<ChattingService>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.Register<SessionState>(ctx => ctx.Resolve<SessionStateProvider>().GetAsync().GetAwaiter().GetResult()).InstancePerLifetimeScope();
        builder.RegisterType<AuthorizationService>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        base.Load(builder);
    }
}