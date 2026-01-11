# Website Đặt Dịch Vụ Sửa Chữa Nhà Cửa

Website đặt dịch vụ sửa chữa nhà cửa được xây dựng bằng ASP.NET Core MVC với Entity Framework Core và SQLite.

## Tính năng

### Cho khách hàng:
- Xem danh sách các dịch vụ sửa chữa nhà cửa
- Đặt dịch vụ với form đầy đủ thông tin
- Xem thông tin xác nhận sau khi đặt dịch vụ

### Cho Admin:
- Dashboard tổng quan với thống kê đơn hàng
- Quản lý đơn đặt dịch vụ (xem, tìm kiếm, lọc theo trạng thái)
- Cập nhật trạng thái đơn hàng (Đang chờ, Đã xác nhận, Đang thực hiện, Hoàn thành, Đã hủy)
- Xem danh sách dịch vụ

## Công nghệ sử dụng

- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core 8.0** - ORM
- **SQLite** - Database
- **Bootstrap 5** - UI Framework
- **Bootstrap Icons** - Icon library

## Cài đặt và chạy

1. **Yêu cầu hệ thống:**
   - .NET 8.0 SDK hoặc cao hơn

2. **Cài đặt:**
   ```bash
   cd DichVuSuaChuaNha
   dotnet restore
   ```

3. **Chạy ứng dụng:**
   ```bash
   dotnet run
   ```

4. **Truy cập:**
   - Website: https://localhost:5001 hoặc http://localhost:5000
   - Admin Dashboard: https://localhost:5001/Admin/Dashboard

## Cấu trúc dự án

```
DichVuSuaChuaNha/
├── Controllers/
│   ├── HomeController.cs      # Trang chủ
│   ├── BookingController.cs   # Đặt dịch vụ
│   └── AdminController.cs     # Quản trị
├── Models/
│   ├── Service.cs             # Model dịch vụ
│   └── Booking.cs             # Model đơn đặt dịch vụ
├── Data/
│   └── ApplicationDbContext.cs # DbContext với seed data
├── Views/
│   ├── Home/                  # Views trang chủ
│   ├── Booking/               # Views đặt dịch vụ
│   └── Admin/                 # Views quản trị
└── wwwroot/                   # Static files

```

## Database

Database SQLite sẽ được tự động tạo khi chạy ứng dụng lần đầu với dữ liệu mẫu:
- 8 dịch vụ mặc định (Sơn nhà, Sửa điện, Sửa nước, Chống thấm, Cải tạo nhà, Lắp đặt cửa, Lát gạch, Sửa máy lạnh)

## Các trang chính

1. **Trang chủ** (`/`) - Hiển thị danh sách dịch vụ
2. **Đặt dịch vụ** (`/Booking/Create`) - Form đặt dịch vụ
3. **Cảm ơn** (`/Booking/ThankYou`) - Xác nhận đặt dịch vụ thành công
4. **Admin Dashboard** (`/Admin/Dashboard`) - Tổng quan hệ thống
5. **Quản lý đơn** (`/Admin/Bookings`) - Danh sách và quản lý đơn hàng
6. **Chi tiết đơn** (`/Admin/BookingDetails/{id}`) - Chi tiết và cập nhật trạng thái đơn
7. **Quản lý dịch vụ** (`/Admin/Services`) - Danh sách dịch vụ

## Trạng thái đơn hàng

- **Pending** (Đang chờ) - Đơn hàng mới được tạo
- **Confirmed** (Đã xác nhận) - Đã xác nhận với khách hàng
- **InProgress** (Đang thực hiện) - Đang thực hiện dịch vụ
- **Completed** (Hoàn thành) - Đã hoàn thành dịch vụ
- **Cancelled** (Đã hủy) - Đơn hàng đã bị hủy

## Ghi chú

- Database SQLite sẽ được tạo tự động trong thư mục gốc của project với tên `hethongnha.db`
- Dữ liệu seed sẽ được thêm tự động khi database được tạo
- Ứng dụng sử dụng Bootstrap 5 và Bootstrap Icons cho giao diện

## Phát triển tiếp

Các tính năng có thể thêm:
- Xác thực người dùng (Login/Register)
- Phân quyền Admin/Khách hàng
- Gửi email thông báo
- Upload hình ảnh
- Đánh giá và nhận xét dịch vụ
- Thanh toán online
- Quản lý thợ và phân công công việc

