# Khắc phục lỗi “One or more projects in the solution were not loaded correctly”

## 1. Mở đúng file solution
Giải nén toàn bộ ZIP. File `Quan_Li_Nha_Hang_Suon_Muoi.slnx` nằm ở thư mục ngoài cùng của gói ZIP, cùng cấp với thư mục `Quan_Li_Nha_Hang_Suon_Muoi`; hãy mở file này và không chạy project trực tiếp trong ZIP.

## 2. Kiểm tra phiên bản Visual Studio
Project này dùng `net9.0-windows`. Cần Visual Studio 2022 phiên bản 17.12 trở lên và workload **.NET desktop development**. Mở Visual Studio Installer → Modify → chọn workload này → Install/Modify. Cập nhật Visual Studio nếu phiên bản cũ.

## 3. Nếu project vẫn bị unload
Trong Visual Studio, mở View → Output (hoặc nhấn `Ctrl+Alt+O`). Ở danh sách “Show output from”, chọn “Solution” hoặc “Project System” nếu có. Đọc dòng lỗi đầu tiên có tên `Quan_Li_Nha_Hang_Suon_Muoi.csproj`.

## 4. Tải lại project
Trong Solution Explorer, nhấp phải vào project bị mờ hoặc ghi `(unloaded)` → **Reload Project**. Nếu được yêu cầu chọn SDK hoặc cài workload, làm theo thông báo rồi reload lại.

## 5. Nếu cần gửi lỗi để chẩn đoán
Sao chép khoảng 10–20 dòng đầu tiên trong Output Window có nhắc đến `Quan_Li_Nha_Hang_Suon_Muoi.csproj` và gửi cùng ảnh Solution Explorer. Hộp thoại cảnh báo hiện tại chỉ báo project không tải được, chưa cho biết nguyên nhân cụ thể.

Lưu ý: tài khoản `admin/admin123` và `staff/staff123` chỉ dùng cho demo. Bản khung này chưa kết nối SQL Server.
