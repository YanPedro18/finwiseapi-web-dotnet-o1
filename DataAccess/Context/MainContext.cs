using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Context
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }

        #region FinWise
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<User> User { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configurações específicas para o banco de dados PostgreSQL
            if (Database.IsNpgsql())
            {
                // Percorre todas as entidades (classes) mapeadas no DbContext
                foreach (var entity in builder.Model.GetEntityTypes())
                {
                    // Define o nome da tabela como minúsculo (PostgreSQL é case-sensitive)
                    entity.SetTableName(entity.GetTableName().ToLower());

                    // Percorre todas as propriedades (colunas) da entidade
                    foreach (var property in entity.GetProperties())
                    {
                        // Define o nome da coluna como minúsculo
                        property.SetColumnName(property.GetColumnName().ToLower());

                        // Se a propriedade for do tipo DateTime
                        if (property.ClrType == typeof(DateTime))
                        {
                            // Define um conversor de valor que:
                            // - Salva a data como está
                            // - Ao ler do banco, marca o DateTime como UTC (evita problemas com fuso horário)
                            property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                                v => v,
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                            );
                        }
                        // Se a propriedade for do tipo DateTime? (nullable)
                        else if (property.ClrType == typeof(DateTime?))
                        {
                            // Define um conversor para DateTime? que:
                            // - Salva a data como está
                            // - Ao ler do banco, marca como UTC se houver valor
                            property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime?, DateTime?>(
                                v => v,
                                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null)
                            );
                        }
                    }
                }
            }

            #region Configurações do FinWise

            //Transaction
            builder.Entity<Transaction>()
                    .HasKey(s => s.Id);


            #endregion

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false); // Certifique-se de que isso está ativado
            base.OnConfiguring(optionsBuilder);
        }

    }


}

