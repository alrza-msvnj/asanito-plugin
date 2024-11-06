using Microsoft.EntityFrameworkCore;
using Nop.Core;
using Nop.Data;
using Nop.Data.Extensions;
using System;
using System.Linq;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Data
{
    public class AsanitoContext : DbContext, IDbContext
    {
        #region Ctor

        public AsanitoContext(DbContextOptions<AsanitoContext> options) : base(options)
        {

        }

        #endregion

        #region Utilities

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AsanitoCustomerMappingMap());
            modelBuilder.ApplyConfiguration(new AsanitoCategoryMappingMap());
            modelBuilder.ApplyConfiguration(new AsanitoProductMappingMap());
            modelBuilder.ApplyConfiguration(new AsanitoInvoiceMappingMap());

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        public void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            using (var transaction = Database.BeginTransaction())
            {
                var result = Database.ExecuteSqlCommand(sql, parameters);
                transaction.Commit();
                return result;
            }
        }

        public string GenerateCreateScript()
        {
            return Database.GenerateCreateScript();
        }

        public IQueryable<TQuery> QueryFromSql<TQuery>(string sql, params object[] parameters) where TQuery : class
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Install object context
        /// </summary>
        public void Install()
        {
            this.ExecuteSqlScript(GenerateCreateScript());
        }

        /// <summary>
        /// Uninstall object context
        /// </summary>
        public void Uninstall()
        {
            this.DropPluginTable(nameof(AsanitoCustomerMapping));
            this.DropPluginTable(nameof(AsanitoCategoryMapping));
            this.DropPluginTable(nameof(AsanitoProductMapping));
            this.DropPluginTable(nameof(AsanitoInvoiceMapping));
        }
    }
}