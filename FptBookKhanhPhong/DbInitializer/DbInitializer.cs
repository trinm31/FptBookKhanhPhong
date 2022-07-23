using System;
using System.Linq;
using FptBookKhanhPhong.Data;
using FptBookKhanhPhong.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FptBookKhanhPhong.DbInitializer
{
    public class DbInitializer: IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public void Initializer()
        {
            // kiểm tra xem có migration nào chưa dc migration thì nó sẽ tự động migration
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            
            // kiểm tra xem dưới bảng role có những role này chưa có gòi thì return ko có thif tạo bên dưới 
            if (_db.Roles.Any(r => r.Name == "StoreOwner")) return;
            if (_db.Roles.Any(r => r.Name == "Customer")) return;
            if (_db.Roles.Any(r => r.Name == "Admin")) return;
            
            // tạo role cần thiêt cho hệ thống
            _roleManager.CreateAsync(new IdentityRole("StoreOwner")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();

            // tạo user admin
            _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true, 
                FullName = "Admin",
                Address = "Đà Nẵng"
            }, "Admin123@").GetAwaiter().GetResult();
            
            // xuống db tìm thằng user vừa tạo ra
            ApplicationUser admin = _db.ApplicationUsers.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();
            
            // add user vừa tạo vào role admin
            _userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
        }
    }
}