using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Services.Tasks;
using Nop.Web.Framework.Infrastructure.Extensions;
using Tesla.Plugin.Widgets.CRM.Asanito.Areas.Admin.Factories;
using Tesla.Plugin.Widgets.CRM.Asanito.Client;
using Tesla.Plugin.Widgets.CRM.Asanito.Data;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;
using Tesla.Plugin.Widgets.CRM.Asanito.Services;
using Tesla.Plugin.Widgets.CRM.Asanito.Services.Tasks;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        #region Constant

        private const string ContextName = nameof(AsanitoContext);

        #endregion

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterPluginDataContext<AsanitoContext>(ContextName);

            builder.RegisterType<EfRepository<AsanitoCustomerMapping>>().As<IRepository<AsanitoCustomerMapping>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<AsanitoCategoryMapping>>().As<IRepository<AsanitoCategoryMapping>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<AsanitoProductMapping>>().As<IRepository<AsanitoProductMapping>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<AsanitoInvoiceMapping>>().As<IRepository<AsanitoInvoiceMapping>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();

            builder.RegisterType<AsanitoService>().As<IAsanitoService>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceStatusTask>().As<IScheduleTask>().InstancePerLifetimeScope();

            builder.RegisterType<AsanitoModelFactory>().As<IAsanitoModelFactory>().InstancePerLifetimeScope();

            builder.RegisterType<AsanitoClient>().As<IAsanitoClient>().InstancePerLifetimeScope();
        }

        public int Order => 0;
    }
}