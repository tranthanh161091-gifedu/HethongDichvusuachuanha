using System.Collections.Generic;
using System.Linq;
using DichVuSuaChuaNha.Data;
using DichVuSuaChuaNha.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình Entity Framework Core với SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình xác thực bằng Cookie
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Tự động tạo database và seed data (dịch vụ + user mặc định)
InitializeDatabase(app.Services);

static void InitializeDatabase(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        
        ApplicationDbContext? context = null;
        try
        {
            context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            logger.LogInformation("Bắt đầu khởi tạo database...");
            
            // Kiểm tra xem database có tồn tại và có đúng schema không
            bool dbExists = false;
            bool needsRecreate = false;
            
            try
            {
                dbExists = context.Database.CanConnect();
                logger.LogInformation("Kết nối database: {Status}", dbExists ? "Thành công" : "Chưa tồn tại");
            }
            catch (Exception ex)
            {
                logger.LogWarning("Không thể kết nối database: {Message}", ex.Message);
                dbExists = false;
            }
            
            if (dbExists)
            {
                try
                {
                    // Thử kiểm tra tất cả các bảng cần thiết
                    _ = context.Users.Count();
                    _ = context.Services.Count();
                    _ = context.Bookings.Count();
                    logger.LogInformation("Tất cả các bảng đã tồn tại.");
                }
                catch (Exception ex)
                {
                    // Nếu thiếu bất kỳ bảng nào, cần tạo lại database
                    if (ex.Message.Contains("no such table"))
                    {
                        needsRecreate = true;
                        logger.LogWarning("Database tồn tại nhưng thiếu bảng. Sẽ tạo lại database... Lỗi: {Message}", ex.Message);
                    }
                    else
                    {
                        logger.LogError(ex, "Lỗi không mong đợi khi kiểm tra database");
                        throw;
                    }
                }
            }
            
            if (needsRecreate || !dbExists)
            {
                if (dbExists && needsRecreate)
                {
                    // Đóng connection và dispose context trước khi xóa database
                    try
                    {
                        context.Database.CloseConnection();
                    }
                    catch { }
                    
                    context.Dispose();
                    context = null;
                    
                    // Xóa database cũ
                    var dbPath = "hethongnha.db";
                    if (System.IO.File.Exists(dbPath))
                    {
                        try
                        {
                            // Đợi để đảm bảo connection đã đóng hoàn toàn
                            System.Threading.Thread.Sleep(2000);
                            
                            // Thử xóa nhiều lần nếu cần
                            int retryCount = 0;
                            while (retryCount < 5 && System.IO.File.Exists(dbPath))
                            {
                                try
                                {
                                    System.IO.File.Delete(dbPath);
                                    logger.LogInformation("Đã xóa database cũ với schema không đúng.");
                                    break;
                                }
                                catch (Exception)
                                {
                                    retryCount++;
                                    if (retryCount < 5)
                                    {
                                        System.Threading.Thread.Sleep(500);
                                    }
                                    else
                                    {
                                        throw;
                                    }
                                }
                            }
                        }
                        catch (Exception deleteEx)
                        {
                            logger.LogError(deleteEx, "Không thể xóa database cũ. Vui lòng dừng ứng dụng và xóa thủ công file hethongnha.db");
                            throw;
                        }
                    }
                    
                    // Tạo lại context sau khi xóa database
                    context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                }
            }
            
            // Tạo database nếu chưa tồn tại
            context.Database.EnsureCreated();
            logger.LogInformation("Database đã được đảm bảo tồn tại.");

            // Seed dịch vụ (chỉ thêm nếu chưa tồn tại theo tên)
            var defaultServices = new List<Service>
            {
                new Service { Name = "Sơn nhà", Description = "Dịch vụ sơn tường, trần nhà chuyên nghiệp với các loại sơn cao cấp", BasePrice = 50000, Unit = "m²", Icon = "bi bi-brush", IsActive = true, DisplayOrder = 1 },
                new Service { Name = "Sửa chữa điện", Description = "Sửa chữa, lắp đặt hệ thống điện dân dụng, công nghiệp", BasePrice = 200000, Unit = "công việc", Icon = "bi bi-lightning-charge", IsActive = true, DisplayOrder = 2 },
                new Service { Name = "Sửa chữa nước", Description = "Sửa chữa đường ống nước, thay thế thiết bị vệ sinh", BasePrice = 150000, Unit = "công việc", Icon = "bi bi-droplet", IsActive = true, DisplayOrder = 3 },
                new Service { Name = "Chống thấm", Description = "Chống thấm tường, sân thượng, nhà vệ sinh", BasePrice = 80000, Unit = "m²", Icon = "bi bi-shield-check", IsActive = true, DisplayOrder = 4 },
                new Service { Name = "Cải tạo nhà", Description = "Cải tạo, nâng cấp nhà cửa theo yêu cầu", BasePrice = 1000000, Unit = "dự án", Icon = "bi bi-house-gear", IsActive = true, DisplayOrder = 5 },
                new Service { Name = "Lắp đặt cửa", Description = "Lắp đặt cửa gỗ, cửa nhôm kính, cửa sắt", BasePrice = 500000, Unit = "bộ", Icon = "bi bi-door-open", IsActive = true, DisplayOrder = 6 },
                new Service { Name = "Lát gạch", Description = "Lát gạch nền, tường, ốp lát chuyên nghiệp", BasePrice = 120000, Unit = "m²", Icon = "bi bi-grid-3x3", IsActive = true, DisplayOrder = 7 },
                new Service { Name = "Sửa máy lạnh", Description = "Bảo dưỡng, sửa chữa máy lạnh, điều hòa", BasePrice = 300000, Unit = "máy", Icon = "bi bi-snow", IsActive = true, DisplayOrder = 8 },
                // Bổ sung thêm một số dịch vụ
                new Service { Name = "Vệ sinh nhà cửa", Description = "Dọn dẹp, vệ sinh nhà cửa định kỳ hoặc theo yêu cầu", BasePrice = 200000, Unit = "buổi", Icon = "bi bi-bucket", IsActive = true, DisplayOrder = 9 },
                new Service { Name = "Lắp đặt thiết bị nhà bếp", Description = "Lắp đặt bếp ga, máy hút mùi, lò vi sóng, tủ bếp", BasePrice = 400000, Unit = "công việc", Icon = "bi bi-egg-fried", IsActive = true, DisplayOrder = 10 }
            };

            int serviceCount = 0;
            foreach (var svc in defaultServices)
            {
                if (!context.Services.Any(s => s.Name == svc.Name))
                {
                    context.Services.Add(svc);
                    serviceCount++;
                }
            }
            if (serviceCount > 0)
            {
                logger.LogInformation("Đã thêm {Count} dịch vụ mới.", serviceCount);
            }

            // Seed user mặc định (admin / user) nếu chưa có
            if (!context.Users.Any())
            {
                context.Users.Add(new AppUser
                {
                    UserName = "admin",
                    Password = "admin123", // Demo ONLY
                    Role = "Admin"
                });

                context.Users.Add(new AppUser
                {
                    UserName = "user",
                    Password = "user123", // Demo ONLY
                    Role = "User"
                });
                logger.LogInformation("Đã thêm 2 tài khoản mặc định: admin và user.");
            }

            if (context.ChangeTracker.HasChanges())
            {
                context.SaveChanges();
                logger.LogInformation("✅ Database đã được khởi tạo và seed data thành công.");
            }
            else
            {
                logger.LogInformation("✅ Database đã sẵn sàng, không cần seed thêm.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Lỗi khi tạo database: {Message}", ex.Message);
            logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
            
            // Không throw để app vẫn có thể chạy, nhưng sẽ có lỗi khi truy cập database
        }
        finally
        {
            context?.Dispose();
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
