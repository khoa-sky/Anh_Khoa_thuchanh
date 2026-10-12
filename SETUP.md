# Cấu hình chạy QL_CAFE

## Database

Ứng dụng vẫn dùng chuỗi kết nối trong `App.config` nếu không có cấu hình ngoài. Khi chạy trên máy khác, nên đặt biến môi trường `CAFE_DB_CONNECTION` bằng connection string SQL Server, ví dụ:

`Server=.\SQLEXPRESS;Database=QL_CA_FE;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True`

Database phải có schema tương ứng với `Model1.edmx`.

## Gửi OTP

Không lưu mật khẩu Gmail trong mã nguồn. Trước khi chạy chức năng quên mật khẩu, đặt:

- `CAFE_SMTP_USER`: địa chỉ Gmail gửi OTP.
- `CAFE_SMTP_PASSWORD`: App Password của Gmail.

Sau khi thay đổi biến môi trường, khởi động lại ứng dụng.

## Build

Dự án là WinForms .NET Framework 4.7.2 và dùng Excel COM Interop. Máy build cần Visual Studio/MSBuild .NET Framework, .NET Framework 4.7.2 Developer Pack, Windows SDK và Microsoft Excel/Office interop phù hợp.