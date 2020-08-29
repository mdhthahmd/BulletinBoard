## Add .gitignore
`dotnet new gitignore`

## Add global.json
`dotnet new globaljson`
specifies which version to use

## Add sln
`dotnet new sln -n BulletinBoard`

## Add Models Library
-  `dotnet new classlib -o Models`

## Add Api
- `dotnet new webapi -o Api`

## Add Web
- `dotnet new blazorserver -o Web`

## Add project to sln
- `dotnet sln add <todo-app/todo-app.csproj>` to add projects
- `dotnet sln <todo.sln> list` to see exisiting projects
- `dotnet sln remove <todo-app/todo-app.csproj>` to remove projects

## Add reference
`dotnet add reference ..\BulletinBoard.Models\BulletinBoard.Models.csproj` to both Api and Web


## CRUD functions for Api

### Add db connection string
```json
"ConnectionStrings": {
    "DBConnection": "server=localhost;database=BulletinBoardDB;Trusted_Connection=true;User=staff;Password=password"
  }
```
to the `appsettings.json` file

### Add nuget packages

Add the packages
- `dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.1.7`     
- `dotnet add package Microsoft.EntityFrameworkCore.Tools --version 3.1.7`

### Add DbContext Class
-  `mkdir` Data\AddDbContext.cs

```cs
using Microsoft.EntityFrameworkCore;
using Models;
using System;

namespace Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base (options)
        {
        }

        public DbSet<Bulletin> Bulletins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<Bulletin>().HasData( new Bulletin {  
                Id = 1,
                CreatedBy = 1,
                CreatedAt = DateTime.Now,
                HeadingText = "Bulletin Header",
                Content = "This is the Content of the Bulletin",
                Status = Status.Active
             });
        }
    }
}
```

### Update Startup.cs
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api
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
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

```
### Make Migration
`dotnet ef migrations add InitialCreate`

### Update Database
`dotnet ef database update`

### Interface and Repository
 - In Data Folder, Make:
  - `IBulletinRepository.cs`
  - `BullentinRepository.cs`

### Add Links to Startup.cs
`services.AddScoped<IBulletinRepository, BulletinRepository>();` 

### Add Controllers


### Add Validation
- Install
    - `dotnet add package System.ComponentModel.Annotations --version 4.7.0` in Models Library

###

