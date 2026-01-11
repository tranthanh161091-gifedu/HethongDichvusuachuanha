# Hướng Dẫn Sửa Lỗi Database

## Lỗi: "no such table: Users"

Lỗi này xảy ra khi database cũ không có bảng `Users` (do bạn đã thêm model `AppUser` sau khi tạo database).

### Cách sửa nhanh nhất:

1. **Dừng ứng dụng** (nếu đang chạy):
   - Nhấn `Ctrl + C` trong terminal/PowerShell

2. **Xóa file database cũ**:
   ```powershell
   cd E:\hethongnha\DichVuSuaChuaNha
   Remove-Item hethongnha.db -ErrorAction SilentlyContinue
   ```

   Hoặc xóa thủ công:
   - Mở File Explorer
   - Vào thư mục `E:\hethongnha\DichVuSuaChuaNha`
   - Xóa file `hethongnha.db`

3. **Chạy lại ứng dụng**:
   ```powershell
   dotnet run
   ```

4. **Database sẽ được tạo tự động** với đầy đủ các bảng:
   - `Services` (10 dịch vụ mặc định)
   - `Bookings` 
   - `Users` (admin/admin123 và user/user123)

### Lưu ý:

- **Dữ liệu cũ sẽ bị mất** khi xóa database
- Nếu bạn có dữ liệu quan trọng, hãy backup trước
- Sau khi xóa và chạy lại, tất cả dữ liệu sẽ được seed lại tự động

### Kiểm tra:

Sau khi chạy lại, thử đăng nhập:
- Admin: `admin / admin123`
- User: `user / user123`

Nếu vẫn còn lỗi, kiểm tra:
- File `hethongnha.db` đã được xóa chưa?
- Ứng dụng có đang chạy ở đâu khác không? (dừng tất cả)




