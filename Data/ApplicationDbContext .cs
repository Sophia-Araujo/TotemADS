using Microsoft.EntityFrameworkCore;
using TotemPWA.Models;

namespace TotemPWA.Data
{
    public class ApplicationDbContext  : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext > options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<Cupom> Cupons { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Adicional> Adicionais { get; set; }
        public DbSet<IngredienteProduto> IngredientesProduto { get; set; }
        public DbSet<ItemCombo> ItensCombo { get; set; }
        public DbSet<AdministradorProduto> AdministradorProdutos { get; set; }
        public DbSet<AdministradorCombo> AdministradorCombos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Chaves compostas para tabelas que aparentam ser de relacionamento

            modelBuilder.Entity<ItemPedido>()
                .HasKey(ip => new { ip.PedidoId, ip.ProdutoId, ip.ComboId });

            modelBuilder.Entity<Adicional>()
                .HasKey(a => new { a.ProdutoId, a.IngredienteId, a.PedidoId });

            modelBuilder.Entity<IngredienteProduto>()
                .HasKey(ip => new { ip.ProdutoId, ip.IngredienteId });

            modelBuilder.Entity<ItemCombo>()
                .HasKey(ic => new { ic.ComboId, ic.ProdutoId });

            modelBuilder.Entity<AdministradorProduto>()
                .HasKey(ap => new { ap.AdministradorId, ap.ProdutoId });

            modelBuilder.Entity<AdministradorCombo>()
                .HasKey(ac => new { ac.AdministradorId, ac.ComboId });

            // Relações (exemplos básicos):

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Pedidos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.CPF);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Pagamentos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.CPF);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<Cupom>()
                .HasOne(c => c.Combo)
                .WithMany(co => co.Cupons)
                .HasForeignKey(c => c.ComboId);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(ip => ip.PedidoId);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Produto)
                .HasOne(p => p.Pedido)
                .HasForeignKey(ip => ip.ProdutoId)
                .HasForeignKey(ip => ip.PedidoId);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Combo)
                .WithMany(c => c.ItensCombo)
                .HasForeignKey(ip => ip.ComboId)
                .IsRequired(false);

            // Continue adicionando relacionamentos conforme sua necessidade...
        }
    }
}
