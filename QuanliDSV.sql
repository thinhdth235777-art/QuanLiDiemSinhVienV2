USE master;
GO

-- 1. NGẮT KẾT NỐI VÀ XÓA DATABASE CŨ (Làm sạch)
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyDiemSinhVien')
BEGIN
    ALTER DATABASE QuanLyDiemSinhVien SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyDiemSinhVien;
END
GO

-- 2. TẠO DATABASE MỚI
CREATE DATABASE QuanLyDiemSinhVien;
GO
USE QuanLyDiemSinhVien;
GO

-- 3. TẠO BẢNG KHOA (Bảng này cần có trước để Môn học tham chiếu tới)
CREATE TABLE Khoa (
    MaKhoa VARCHAR(10) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL
);

-- 4. TẠO BẢNG LỚP
CREATE TABLE Lop (
    MaLop VARCHAR(20) PRIMARY KEY, 
    TenLop NVARCHAR(100) NOT NULL,
    Khoa NVARCHAR(100)             
);

-- 5. TẠO BẢNG MÔN HỌC (Đã thêm HocKy và MaKhoa)
CREATE TABLE MonHoc (
    MaMH VARCHAR(20) PRIMARY KEY, 
    TenMH NVARCHAR(100) NOT NULL,
    SoTinChi INT CHECK (SoTinChi > 0),
    HocKy INT DEFAULT 1,                -- Thêm cột Học kỳ
    MaKhoa VARCHAR(10),                 -- Thêm cột Mã Khoa
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

-- 6. TẠO BẢNG SINH VIÊN (Đã thêm SoDienThoai và AnhThe ngay từ đầu)
CREATE TABLE SinhVien (
    MaSV VARCHAR(50) PRIMARY KEY,  
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),         
    DiaChi NVARCHAR(200),
    Email VARCHAR(100),
    SoDienThoai VARCHAR(15),       -- Thêm cột SĐT
    AnhThe VARBINARY(MAX),         -- Thêm cột Ảnh
    MaLop VARCHAR(20),             
    
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);

-- 7. TẠO BẢNG ĐIỂM
CREATE TABLE Diem (
    MaSV VARCHAR(50),
    MaMH VARCHAR(20),
    
    DiemQuaTrinh FLOAT CHECK (DiemQuaTrinh BETWEEN 0 AND 10),
    DiemThi FLOAT CHECK (DiemThi BETWEEN 0 AND 10),
    DiemTongKet FLOAT, 
    
    HocKy INT,         
    NamHoc VARCHAR(20),
    GhiChu NVARCHAR(100),

    PRIMARY KEY (MaSV, MaMH), 
    
    FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV),
    FOREIGN KEY (MaMH) REFERENCES MonHoc(MaMH)
);

-- 8. TẠO BẢNG ĐĂNG NHẬP
CREATE TABLE DangNhap (
    TaiKhoan VARCHAR(50) PRIMARY KEY, 
    MatKhau VARCHAR(50) NOT NULL,
    HoTen NVARCHAR(100),              
    Quyen VARCHAR(20) DEFAULT 'SinhVien' 
);
GO

-- =======================================================
-- PHẦN THÊM DỮ LIỆU MẪU (DATA SEEDING)
-- =======================================================

-- A. Thêm Khoa
INSERT INTO Khoa VALUES ('CNTT', N'Công nghệ thông tin');
INSERT INTO Khoa VALUES ('NNA', N'Ngôn ngữ Anh');
INSERT INTO Khoa VALUES ('SP', N'Sư phạm');

-- B. Thêm Lớp
INSERT INTO Lop VALUES ('DH22TH1', N'Đại học Tin Học K22 - Lớp 1', N'Công nghệ thông tin');
INSERT INTO Lop VALUES ('DH23AV', N'Đại học Ngôn Ngữ Anh K23', N'Ngoại Ngữ');
INSERT INTO Lop VALUES ('DH21SP', N'Đại học Sư Phạm Toán K21', N'Sư Phạm');

-- C. Thêm Môn Học (Có phân khoa và học kỳ)
INSERT INTO MonHoc (MaMH, TenMH, SoTinChi, HocKy, MaKhoa) VALUES ('TIN01', N'Nhập môn Lập trình', 3, 1, 'CNTT');
INSERT INTO MonHoc (MaMH, TenMH, SoTinChi, HocKy, MaKhoa) VALUES ('CSDL', N'Cơ sở dữ liệu', 4, 2, 'CNTT');
INSERT INTO MonHoc (MaMH, TenMH, SoTinChi, HocKy, MaKhoa) VALUES ('ML01', N'Triết học Mác - Lênin', 3, 1, 'SP'); -- Môn chung
INSERT INTO MonHoc (MaMH, TenMH, SoTinChi, HocKy, MaKhoa) VALUES ('ANH01', N'Tiếng Anh căn bản 1', 2, 1, 'NNA');

-- D. Thêm Tài khoản Admin
INSERT INTO DangNhap (TaiKhoan, MatKhau, HoTen, Quyen) 
VALUES ('admin', '123', N'Giáo Vụ Khoa CNTT', 'GiaoVien');

-- E. Thêm Sinh Viên (Có SĐT)
INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop, SoDienThoai) 
VALUES ('DQU001', N'Lê Minh Trí', '2004-05-10', N'Nam', N'Long Xuyên, An Giang', 'DH22TH1', '0987654321');

INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop, SoDienThoai) 
VALUES ('DQU002', N'Nguyễn Ngọc Hân', '2005-08-20', N'Nữ', N'Châu Đốc, An Giang', 'DH23AV', '0123456789');

-- F. Tạo tài khoản cho Sinh viên luôn (để test đăng nhập)
INSERT INTO DangNhap (TaiKhoan, MatKhau, HoTen, Quyen) VALUES ('DQU001', '123', N'Lê Minh Trí', 'SinhVien');
INSERT INTO DangNhap (TaiKhoan, MatKhau, HoTen, Quyen) VALUES ('DQU002', '123', N'Nguyễn Ngọc Hân', 'SinhVien');

-- G. Thêm Điểm mẫu
INSERT INTO Diem (MaSV, MaMH, DiemQuaTrinh, DiemThi, HocKy, NamHoc)
VALUES 
  (N'DQU001', N'CSDL', 8.5, 7.0, 2, N'2024-2025'),
  (N'DQU001', N'TIN01', 9.0, 8.0, 1, N'2024-2025'),
  (N'DQU002', N'ANH01', 7.0, 6.5, 1, N'2024-2025');
GO