using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Padaria.Domain.Entities;
using Padaria.Domain.Entities.Identity;
using System;

namespace Padaria.Repository
{
    public class DataContext : IdentityDbContext<User, Role, Guid,
                                                    IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
                                                    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //Esse comando cria a tabela e deve estar no plural
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        //Referenciamos as tabelas M:N
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            }
            );

            modelBuilder.Entity<Venda>()
                .HasKey(v => new { v.ProdutoId, v.PedidoId });
        }

    }
}