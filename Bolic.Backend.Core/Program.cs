using Bolic.Shared.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => { services.AddSingleton<IRuntime, Runtime>(); })
    .Build();

host.Run();