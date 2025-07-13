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

            // Configuração para ItensCombo - ATUALIZADA
            modelBuilder.Entity<ItensCombo>()
                .HasOne(ic => ic.Produto)
                .WithMany()
                .HasForeignKey(ic => ic.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // CupomId agora é nullable e não obrigatório
            modelBuilder.Entity<ItensCombo>()
                .HasOne(ic => ic.Cupom)
                .WithMany()
                .HasForeignKey(ic => ic.CupomId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false); // Permite null
        }
    }
}