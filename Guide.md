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
```cs
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Data
{
    public interface IBulletinRepository 
    {
        Task<IEnumerable<Bulletin>> GetBulletins();
        Task<Bulletin> GetBulletin(int id);
        Task<Bulletin> AddBulletin(Bulletin bulletin);
        Task<Bulletin> UpdateBulletin(Bulletin bulletin);
        Task<Bulletin> DeleteBulletin(int id);
    }
}
```
  - `BullentinRepository.cs`
```cs
using Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Data
{
    public class BulletinRepository : IBulletinRepository
    {
        private readonly AppDbContext appDbContext;

        public BulletinRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Bulletin>> GetBulletins()
        {
            return await appDbContext.Bulletins.ToListAsync();
        }

        public async Task<Bulletin> GetBulletin(int bulletinId)
        {
            return await appDbContext.Bulletins
                .FirstOrDefaultAsync(e => e.Id == bulletinId);
        }

        public async Task<Bulletin> AddBulletin(Bulletin Bulletin)
        {
            var result = await appDbContext.Bulletins.AddAsync(Bulletin);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Bulletin> UpdateBulletin(Bulletin Bulletin)
        {
            var result = await appDbContext.Bulletins
                .FirstOrDefaultAsync(e => e.Id == Bulletin.Id);

            if (result != null)
            {
                result.Id = Bulletin.Id;
                result.HeadingText = Bulletin.HeadingText;
                result.Status = Bulletin.Status;
                await appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        
        public async Task<Bulletin> DeleteBulletin(int bulletinId)
        {
            var result = await appDbContext.Bulletins
                .FirstOrDefaultAsync(e => e.Id == bulletinId);
            if (result != null)
            {
                appDbContext.Bulletins.Remove(result);
                await appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
```

### Add Links to Startup.cs
`services.AddScoped<IBulletinRepository, BulletinRepository>();` 

### Add Controllers
- `BulletinController`
```cs
using System;
using Models;
using Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BulletinsController : ControllerBase
    {
        private readonly IBulletinRepository bulletinRepository;

        public BulletinsController(IBulletinRepository bulletinRepository)
        {
            this.bulletinRepository = bulletinRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult> GetBulletins()
        {
            try
            {
                return Ok(await bulletinRepository.GetBulletins());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
					"Error retrieving data from the database");
            }
        }
    
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Bulletin>> GetBulletin(int id)
        {
            try
            {
                var result = await bulletinRepository.GetBulletin(id);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    
        [HttpPost]
        public async Task<ActionResult<Bulletin>> CreateBulletin(Bulletin bulletin)
        {
            try
            {
                if (bulletin == null)
                    return BadRequest();

                // add user data
                // bulletin.CreatedBy = Cookie;
                // add datetime
                bulletin.CreatedAt = DateTime.Now;

                var createdBulletin = await bulletinRepository.AddBulletin(bulletin);

                return CreatedAtAction(nameof(GetBulletin),
                    new { id = createdBulletin.Id }, createdBulletin);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new bulletin record");
            }
        }
    
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Bulletin>> UpdateBulletin(int id, Bulletin bulletin)
        {
            try
            {
                if (id != bulletin.Id)
                    return BadRequest("Bulletin ID mismatch");

                var bulletinToUpdate = await bulletinRepository.GetBulletin(id);

                if (bulletinToUpdate == null)
                    return NotFound($"Bulletin with Id = {id} not found");

                return await bulletinRepository.UpdateBulletin(bulletin);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }
    
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Bulletin>> DeleteBulletin(int id)
        {
            try
            {
                var bulletinToDelete = await bulletinRepository.GetBulletin(id);

                if (bulletinToDelete == null)
                {
                    return NotFound($"Bulletin with Id = {id} not found");
                }

                return await bulletinRepository.DeleteBulletin(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}

```


### Add Validation
- Install
    - `dotnet add package System.ComponentModel.Annotations --version 4.7.0` in Models Library

###

