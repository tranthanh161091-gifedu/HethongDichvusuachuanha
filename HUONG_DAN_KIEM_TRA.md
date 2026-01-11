# Hướng Dẫn Kiểm Tra Website Đặt Dịch Vụ Sửa Chữa Nhà Cửa

## Cách Chạy Ứng Dụng

1. **Mở terminal/PowerShell tại thư mục project:**
   ```powershell
   cd DichVuSuaChuaNha
   ```

2. **Chạy ứng dụng:**
   ```powershell
   dotnet run
   ```

3. **Truy cập website:**
   - Mở trình duyệt và vào: `https://localhost:5001` hoặc `http://localhost:5000`
   - Nếu có cảnh báo SSL, chọn "Advanced" và "Continue to localhost"

## Kiểm Tra Các Chức Năng

### 1. Trang Chủ (Home)
- **URL:** `/` hoặc `/Home/Index`
- **Kiểm tra:**
  - ✅ Hiển thị banner hero với tiêu đề và nút "Đặt dịch vụ ngay"
  - ✅ Hiển thị danh sách 8 dịch vụ với icon, tên, mô tả, giá
  - ✅ Mỗi dịch vụ có nút "Đặt ngay" để chuyển đến form đặt dịch vụ
  - ✅ Hiển thị phần Features (Uy tín, Nhanh chóng, Giá cả hợp lý)

### 2. Đặt Dịch Vụ (Booking)
- **URL:** `/Booking/Create`
- **Kiểm tra:**
  - ✅ Form hiển thị dropdown chọn dịch vụ
  - ✅ Các trường bắt buộc: Họ tên, Số điện thoại, Địa chỉ, Thời gian
  - ✅ Validation hoạt động (thử submit form trống)
  - ✅ Sau khi submit thành công, chuyển đến trang ThankYou

### 3. Trang Cảm Ơn (Thank You)
- **URL:** `/Booking/ThankYou/{id}`
- **Kiểm tra:**
  - ✅ Hiển thị thông báo thành công
  - ✅ Hiển thị đầy đủ thông tin đơn đặt dịch vụ
  - ✅ Có nút "Về trang chủ" và "Đặt thêm dịch vụ"

### 4. Admin Dashboard
- **URL:** `/Admin/Dashboard`
- **Kiểm tra:**
  - ✅ Hiển thị 4 thẻ thống kê: Tổng đơn hàng, Đang chờ, Đã hoàn thành, Dịch vụ
  - ✅ Hiển thị bảng đơn hàng mới nhất (10 đơn)
  - ✅ Có nút "Xem tất cả đơn hàng"

### 5. Quản Lý Đơn Hàng (Admin)
- **URL:** `/Admin/Bookings`
- **Kiểm tra:**
  - ✅ Hiển thị form lọc theo trạng thái
  - ✅ Hiển thị form tìm kiếm (theo tên, số điện thoại, địa chỉ)
  - ✅ Hiển thị bảng danh sách đơn hàng với đầy đủ thông tin
  - ✅ Mỗi đơn có nút "Chi tiết" để xem thông tin chi tiết

### 6. Chi Tiết Đơn Hàng (Admin)
- **URL:** `/Admin/BookingDetails/{id}`
- **Kiểm tra:**
  - ✅ Hiển thị thông tin khách hàng đầy đủ
  - ✅ Hiển thị thông tin dịch vụ
  - ✅ Hiển thị mô tả vấn đề (nếu có)
  - ✅ Hiển thị trạng thái hiện tại với badge màu phù hợp
  - ✅ Form cập nhật trạng thái hoạt động
  - ✅ Có thể thêm ghi chú khi cập nhật trạng thái

### 7. Quản Lý Dịch Vụ (Admin)
- **URL:** `/Admin/Services`
- **Kiểm tra:**
  - ✅ Hiển thị bảng danh sách 8 dịch vụ
  - ✅ Hiển thị đầy đủ thông tin: Icon, Tên, Mô tả, Giá, Đơn vị, Thứ tự, Trạng thái

## Kiểm Tra Database

Database SQLite được tạo tự động tại: `hethongnha.db`

Có thể mở bằng SQLite Browser hoặc công cụ tương tự để xem dữ liệu:
- Bảng `Services`: 8 dịch vụ mặc định
- Bảng `Bookings`: Các đơn đặt dịch vụ đã tạo

## Các Trường Hợp Test

### Test Case 1: Đặt dịch vụ thành công
1. Vào trang chủ
2. Click "Đặt dịch vụ ngay" hoặc chọn một dịch vụ và click "Đặt ngay"
3. Điền đầy đủ form:
   - Chọn dịch vụ: Sơn nhà
   - Họ tên: Nguyễn Văn A
   - Số điện thoại: 0901234567
   - Email: test@example.com
   - Địa chỉ: 123 Đường ABC, Quận 1, TP.HCM
   - Mô tả: Cần sơn lại phòng khách
   - Thời gian: Chọn ngày mai, 9h sáng
4. Click "Xác nhận đặt dịch vụ"
5. ✅ Kiểm tra: Chuyển đến trang ThankYou với thông tin đầy đủ

### Test Case 2: Validation form
1. Vào `/Booking/Create`
2. Không điền gì, click "Xác nhận đặt dịch vụ"
3. ✅ Kiểm tra: Hiển thị lỗi validation cho các trường bắt buộc

### Test Case 3: Admin cập nhật trạng thái
1. Vào `/Admin/Bookings`
2. Click "Chi tiết" của một đơn hàng
3. Chọn trạng thái mới: "Đã xác nhận"
4. Thêm ghi chú: "Đã liên hệ khách hàng"
5. Click "Cập nhật"
6. ✅ Kiểm tra: Trạng thái được cập nhật, hiển thị thông báo thành công

### Test Case 4: Tìm kiếm và lọc
1. Vào `/Admin/Bookings`
2. Nhập từ khóa tìm kiếm: "Nguyễn"
3. Chọn lọc trạng thái: "Đang chờ"
4. Click "Tìm"
5. ✅ Kiểm tra: Chỉ hiển thị các đơn hàng phù hợp

## Lưu Ý

- Nếu ứng dụng đang chạy, cần dừng (Ctrl+C) trước khi build lại
- Database sẽ được tạo tự động lần đầu chạy
- Seed data (8 dịch vụ) sẽ được thêm tự động nếu database trống
- Tất cả các trang đều responsive, có thể test trên mobile

## Troubleshooting

**Lỗi: "The process cannot access the file... because it is being used by another process"**
- Giải pháp: Dừng ứng dụng đang chạy (Ctrl+C trong terminal)

**Lỗi: "Database not found"**
- Giải pháp: Chạy lại `dotnet run`, database sẽ được tạo tự động

**Lỗi: "No services found"**
- Giải pháp: Xóa file `hethongnha.db` và chạy lại ứng dụng để seed data







