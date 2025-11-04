using ContratistasMM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContratistasMM.Data
{
    public static class DataSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            await SeedUsersAndRoles(serviceProvider);
            // El seeder de proyectos ya no es necesario aquí si los creas desde el panel.
        }

        private static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Cliente" };
            foreach (var roleName in roleNames) { if (!await roleManager.RoleExistsAsync(roleName)) { await roleManager.CreateAsync(new IdentityRole(roleName)); } }

            // Admin
            if (await userManager.FindByEmailAsync("admin@contratistas.com") == null)
            {
                var adminUser = new ApplicationUser { UserName = "admin@contratistas.com", Email = "admin@contratistas.com", Nombre = "Admin", Apellido = "M&M", EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded) { await userManager.AddToRoleAsync(adminUser, "Admin"); }
            }

            // Cliente 1
            if (await userManager.FindByEmailAsync("juan.perez@cliente.com") == null)
            {
                var cliente1User = new ApplicationUser { UserName = "juan.perez@cliente.com", Email = "juan.perez@cliente.com", Nombre = "Juan", Apellido = "Pérez", EmailConfirmed = true };
                var result = await userManager.CreateAsync(cliente1User, "Cliente123!");
                if (result.Succeeded) { await userManager.AddToRoleAsync(cliente1User, "Cliente"); }
            }

            // Cliente 2
            if (await userManager.FindByEmailAsync("ana.garcia@cliente.com") == null)
            {
                var cliente2User = new ApplicationUser { UserName = "ana.garcia@cliente.com", Email = "ana.garcia@cliente.com", Nombre = "Ana", Apellido = "García", EmailConfirmed = true };
                var result = await userManager.CreateAsync(cliente2User, "Cliente123!");
                if (result.Succeeded) { await userManager.AddToRoleAsync(cliente2User, "Cliente"); }
            }
            
            // --- NUEVO CLIENTE PARA PRUEBAS ---
            if (await userManager.FindByEmailAsync("carlos.ruiz@cliente.com") == null)
            {
                var cliente3User = new ApplicationUser { UserName = "carlos.ruiz@cliente.com", Email = "carlos.ruiz@cliente.com", Nombre = "Carlos", Apellido = "Ruiz", EmailConfirmed = true };
                var result = await userManager.CreateAsync(cliente3User, "Cliente123!");
                if (result.Succeeded) { await userManager.AddToRoleAsync(cliente3User, "Cliente"); }
            }
        }
    }
}