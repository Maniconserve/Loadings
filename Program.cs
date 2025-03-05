using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
namespace Loadings
{
	class Program
	{
		static void Main()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory()) 
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var host = Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
				   .UseLazyLoadingProxies());
				})
				.Build();

			using var scope = host.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			dbContext.Database.EnsureCreated();

			Console.WriteLine("=== Eager Loading ===");
			EagerLoading(dbContext);

			Console.WriteLine("\n=== Lazy Loading ===");
			LazyLoading(dbContext);

			Console.WriteLine("\n=== Explicit Loading ===");
			ExplicitLoading(dbContext);


		}
		// 1. EAGER LOADING
		static void EagerLoading(ApplicationDbContext context)
		{
			var departments = context.Departments.Include(d => d.Employees).ToList();
			foreach (var department in departments)
			{
				Console.WriteLine($"{department.DepartmentName} has employees: {string.Join(", ", department.Employees.Select(e => e.Name))}");
			}
		}

		// 2. LAZY LOADING
		static void LazyLoading(ApplicationDbContext context)
		{
			var departments = context.Departments.ToList();
			foreach (var department in departments)
			{
				Console.WriteLine($"{department.DepartmentName} has employees: {string.Join(", ", department.Employees.Select(e => e.Name))}");
			}
		}

		// 3. EXPLICIT LOADING
		static void ExplicitLoading(ApplicationDbContext context)
		{
			var department = context.Departments.FirstOrDefault();
			if (department != null)
			{
				context.Entry(department).Collection(d => d.Employees).Load();
				Console.WriteLine($"{department.DepartmentName} has employees: {string.Join(", ", department.Employees.Select(e => e.Name))}");
			}
		}
	}
}
