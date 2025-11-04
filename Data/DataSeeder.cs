using ContratistasMM.Models;
using Microsoft.AspNetCore.Identity;

namespace ContratistasMM.Data
{
    public static class DataSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // --- 1. CREAR ROLES ---
            string[] roleNames = { "Admin", "Cliente" };
            foreach (var roleName in roleNames)
            {
                // Verifica si el rol ya existe
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Si no existe, lo crea
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // --- 2. CREAR USUARIO ADMINISTRADOR ---
            var adminEmail = "admin@constratistas.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Si el admin no existe, lo crea
                var newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true // Lo confirmamos directamente
                };

                // ¡IMPORTANTE! Usa una contraseña segura en un proyecto real.
                var result = await userManager.CreateAsync(newAdminUser, "Admin123!");

                if (result.Succeeded)
                {
                    // Asigna el rol "Admin" al nuevo usuario
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
            }

            // --- 3. CREAR USUARIOS CLIENTE DE EJEMPLO ---

            var cliente1Email = "juan.perez@contratistas.com";
            var cliente1User = await userManager.FindByEmailAsync(cliente1Email);
                if (cliente1User == null)
                {
                    var newCliente1 = new ApplicationUser
                    {
                        UserName = cliente1Email,
                        Email = cliente1Email,
                        EmailConfirmed = true,
                        // Aquí puedes añadir más datos si los agregas a ApplicationUser.cs
                        // Por ejemplo: Nombre = "Juan", Apellido = "Pérez"
                    };
                    var result = await userManager.CreateAsync(newCliente1, "Cliente123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newCliente1, "Cliente");
                    }
                }

            // Cliente 2: Ana Gómez
            var cliente2Email = "ana.gomez@contratistas.com";
            var cliente2User = await userManager.FindByEmailAsync(cliente2Email);
                        if (cliente2User == null)
            {
                var newCliente2 = new ApplicationUser
                {
                    UserName = cliente2Email,
                    Email = cliente2Email,
                    EmailConfirmed = true,
                    // Por ejemplo: Nombre = "Ana", Apellido = "Gómez"
                };
                var result = await userManager.CreateAsync(newCliente2, "Cliente123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newCliente2, "Cliente");
                }
            }

        }
    }
}