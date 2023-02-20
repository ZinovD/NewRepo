using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FurnitureStore.Data.Models;
using FurnitureStore.Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using FurnitureStore.Data;
using FurnitureStore.Data.Repository;
using Microsoft.AspNetCore.Http;

namespace FurnitureStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRepository<Furniture>, FurnitureRepository>();
            services.AddScoped<IRepository<FurnitureKind>, FurnitureKindRepository>();
            services.AddScoped<IRepository<FurnitureType>, FurnitureTypeRepository>();
            services.AddScoped<IRepository<FurnitureColor>, FurnitureColorRepository>();
            services.AddScoped<IRepository<Order>, OrderRepository>();
            services.AddScoped<IRepository<Comments>, CommentsRepository>();
            services.AddScoped<IRepository<Stock>, StockRepository>();
            services.AddScoped<IRepository<HomePage>, HomePageRepository>();
            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 6;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуются ли цифры
                opts.User.RequireUniqueEmail = true;    // уникальный email
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ "; // допустимые символы
            })
                .AddEntityFrameworkStores<ApplicationContext>();

            services.AddControllersWithViews();
            services.AddTransient<IPasswordValidator<User>,
            CustomPasswordValidator>(serv => new CustomPasswordValidator(6));
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => ShopCart.GetCart(sp));

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();    // подключение аутентификации
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=HomePage}/{action=Index}/{id?}");
            });
        }
    }
}
