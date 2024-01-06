using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    /*DbContext, Entity Framework tarafından sağlanan temel sınıflardan biridir ve bir veritabanı bağlantısını temsil eder.
     * Bu sınıf, veritabanındaki tabloların ve nesnelerin eşleştirilmesini, veritabanı işlemlerinin yönetilmesini ve nesne modelinin 
     * veritabanına aktarılmasını kolaylaştırır. DbContext sınıfı ayrıca LINQ (Language Integrated Query) sorgularını kullanarak 
     * veritabanından veri çekmeyi ve güncellemeyi sağlar.*/
    public class BaseDbContext : DbContext
    {
        /*Bu satır, bir sınıfta bulunan bir özelliği temsil eder. Özelliğin adı Configuration ve tipi IConfigurationdır. 
         * Bu özellik, genellikle .NET Core veya ASP.NET Core gibi projelerde yapılandırma bilgilerine erişim sağlamak amacıyla kullanılır.
         * Açıklamalara ayrıntılı olarak bakalım:
         * protected: Bu erişim belirleyicisi, özelliğin sadece kendi sınıfı ve türetilmiş sınıfları tarafından erişilebilir olduğunu belirtir. Diğer sınıflar bu özelliği doğrudan erişemez.
         * IConfiguration: Bu, bir arabirim (interface) tipidir. IConfiguration, .NET Core uygulamalarında yapılandırma bilgilerine erişim sağlamak için kullanılan bir arabirimdir. 
         * Bu, uygulamanın çeşitli ayarları, veritabanı bağlantı bilgileri, API anahtarları gibi bilgileri içerebilir.
         * Configuration: Bu, özelliğin adıdır. Bu özellik, genellikle sınıf içinde yapılandırma bilgilerine erişmek için kullanılır.
     
         * Bu tür bir özellik, uygulama yapılandırma bilgilerini kod içinde kullanılabilir hale getirmek amacıyla kullanılır. 
         * Örneğin, bir .NET Core uygulamasında appsettings.json dosyasındaki ayarlara erişim sağlamak için IConfiguration kullanılabilir. 
         * Ayrıca, DI (Dependency Injection) ile bu yapılandırma nesnesi sınıfa enjekte edilebilir, böylece bağımlılık tersine çevirme prensibi 
         * (Dependency Inversion Principle) uygulanabilir.*/
        protected IConfiguration Configuration { get; set; }
        /*Bu satır, Entity Framework ile ilişkilendirilmiş bir .NET Core veya .NET Framework projesinde kullanılan bir DbContext sınıfının bir özelliğini temsil eder.
         * DbSet<Brand> Brands { get; set;} ifadesinin ayrıntıları şu şekildedir:
         * DbSet<Brand>: Bu, Entity Framework tarafından sağlanan bir generic sınıftır. DbSet, bir veritabanı tablosunu temsil eden nesnelerin bir kümesini içerir.
         * Brand, bu durumda, Brands özelliği tarafından temsil edilen tablonun veri tipini belirtir. Brand nesnesi, genellikle bir veritabanı tablosunu temsil eden bir sınıftır.
         * Brands: Bu, DbContext sınıfında bulunan bir özelliktir. Bu özellik, genellikle bir veritabanındaki "brands" adlı tabloyu temsil eder. DbSet<Brand> türündeki bu özellik, 
         * bu tablo ile etkileşimde bulunmak için kullanılır. Örneğin, LINQ sorguları kullanarak veri çekme, ekleme, güncelleme ve silme işlemleri bu özellik üzerinden gerçekleştirilebilir.
         * get; set;: Bu, özelliğin hem okunabilir (get) hem de yazılabilir (set) olduğunu belirtir. get ile özelliğin değeri alınabilir, set ile değeri değiştirilebilir. Entity Framework 
         * tarafından kullanılan DbContext sınıflarında, genellikle veritabanı işlemlerini gerçekleştirmek için bu özelliklere erişim sağlamak amacıyla DbSet özellikleri kullanılır.*/
        public DbSet<Brand> Brands { get; set; }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                base.OnConfiguring(
                    optionsBuilder.UseSqlServer(Configuration.GetConnectionString("RentACarDb")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(a =>
            {
                a.ToTable("Brands").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.Name).HasColumnName("Name");
            });

            Brand[] brandEntitySeeds = { new(1, "BMW"), new(2, "Mercedes") };
            modelBuilder.Entity<Brand>().HasData(brandEntitySeeds);
        }
    }
}
