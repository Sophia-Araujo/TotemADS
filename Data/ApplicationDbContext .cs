using Microsoft.EntityFrameworkCore;
using TotemPWA.Models;

namespace TotemPWA.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Cupom> Cupons { get; set; }
        public DbSet<ItensPedido> ItensPedidos { get; set; }
        public DbSet<Adicional> Adicionais { get; set; }
        public DbSet<Igrediente> Igredientes { get; set; }
        public DbSet<IgredienteProduto> IgredienteProdutos { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ItensCombo> ItensCombos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da tabela Cupom
            modelBuilder.Entity<Cupom>(entity =>
            {
                entity.HasKey(e => e.CupomId);
                
                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(e => e.Desconto)
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();
                
                entity.Property(e => e.Validade)
                    .IsRequired();
                
                entity.Property(e => e.ProdutoId)
                    .IsRequired(false);
                
                entity.Property(e => e.AdministradorId)
                    .IsRequired();

                // Índice único para código do cupom
                entity.HasIndex(e => e.Codigo).IsUnique();
            });

            // Relacionamento Cupom -> Administrador
            modelBuilder.Entity<Cupom>()
                .HasOne(c => c.Administrador)
                .WithMany()
                .HasForeignKey(c => c.AdministradorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Cupom -> Produto (opcional)
            modelBuilder.Entity<Cupom>()
                .HasOne(c => c.Produto)
                .WithMany()
                .HasForeignKey(c => c.ProdutoId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Relacionamento 1:1 entre Pedido e Pagamento
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Pagamento)
                .WithOne(pag => pag.Pedido)
                .HasForeignKey<Pagamento>(pag => pag.PedidoId);

            // Relacionamento 1:N entre Cliente e Pedido
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId);

            // Chave simples em ItensPedido
            modelBuilder.Entity<ItensPedido>()
                .HasKey(ip => ip.ItensPedidoId);

            // Relacionamento 1:N entre Pedido e ItensPedido
            modelBuilder.Entity<ItensPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(ip => ip.PedidoId);

            // Relacionamento 1:N entre Produto e ItensPedido
            modelBuilder.Entity<ItensPedido>()
                .HasOne(ip => ip.Produto)
                .WithMany(p => p.ItensPedidos)
                .HasForeignKey(ip => ip.ProdutoId);

            // Configuração da tabela Adicional
            modelBuilder.Entity<Adicional>()
                .HasOne(a => a.ItensPedido)
                .WithMany(i => i.Adicionais)
                .HasForeignKey(a => a.ItensPedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Autorelacionamento de Categoria
            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.CategoriaPai)
                .WithMany(c => c.Subcategorias)
                .HasForeignKey(c => c.CategoriaPaiId);

            // Configuração para ItensCombo
            modelBuilder.Entity<ItensCombo>()
                .HasOne(ic => ic.Produto)
                .WithMany()
                .HasForeignKey(ic => ic.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da entidade Cupom
            modelBuilder.Entity<Cupom>(entity =>
            {
                entity.HasKey(e => e.CupomId);

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.Codigo)
                    .IsUnique(); // Código único

                entity.Property(e => e.Desconto)
                    .IsRequired()
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Validade)
                    .IsRequired();

                entity.Property(e => e.ProdutoId)
                    .IsRequired(false);

                entity.Property(e => e.AdministradorId)
                    .IsRequired();

                // Relacionamento: Cupom -> Administrador (Obrigatório)
                entity.HasOne(c => c.Administrador)
                    .WithMany(a => a.Cupons) // Adicione isso no modelo se ainda não tiver
                    .HasForeignKey(c => c.AdministradorId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relacionamento: Cupom -> Produto (Opcional)
                entity.HasOne(c => c.Produto)
                    .WithMany(p => p.Cupons) // Adicione no modelo se necessário
                    .HasForeignKey(c => c.ProdutoId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            });
        }
    }
}