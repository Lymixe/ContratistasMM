using ContratistasMM.Models;
using Microsoft.AspNetCore.Identity;


namespace ContratistasMM.Data
{
    public static class DataSeeder
    {
        /// <summary>
        /// Inicializa la base de datos con roles y usuarios por defecto si no existen.
        /// </summary>
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // --- 1. CREAR ROLES ---
            string[] roleNames = { "Admin", "Cliente" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // --- 2. CREAR USUARIO ADMINISTRADOR ---
            var adminEmail = "admin@contratistas.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nombre = "Usuario",
                    Apellido = "Administrador",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // --- 3. CREAR USUARIOS CLIENTE DE EJEMPLO ---

            // Cliente 1: Juan Pérez
            var cliente1Email = "juan.perez@contratistas.com";
            if (await userManager.FindByEmailAsync(cliente1Email) == null)
            {
                var cliente1User = new ApplicationUser
                {
                    UserName = cliente1Email,
                    Email = cliente1Email,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(cliente1User, "Cliente123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(cliente1User, "Cliente");
                }
            }

            // Cliente 2: Ana Gómez
            var cliente2Email = "ana.gomez@contratistas.com";
            if (await userManager.FindByEmailAsync(cliente2Email) == null)
            {
                var cliente2User = new ApplicationUser
                {
                    UserName = cliente2Email,
                    Email = cliente2Email,
                    Nombre = "Ana",
                    Apellido = "Gómez",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(cliente2User, "Cliente123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(cliente2User, "Cliente");
                }
            }
        }
    }

}