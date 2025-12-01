CREATE DATABASE QuanLyDiemSinhVien;
GO
USE QuanLyDiemSinhVien;
GO


CREATE TABLE Lop (
    MaLop VARCHAR(20) PRIMARY KEY, 
    TenLop NVARCHAR(100) NOT NULL,
    Khoa NVARCHAR(100)             
);


CREATE TABLE MonHoc (
    MaMH VARCHAR(20) PRIMARY KEY, 
    TenMH NVARCHAR(100) NOT NULL,
    SoTinChi INT CHECK (SoTinChi > 0) 
);


CREATE TABLE SinhVien (
    MaSV VARCHAR(50) PRIMARY KEY,  
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),         
    DiaChi NVARCHAR(200),
    Email VARCHAR(100),
    MaLop VARCHAR(20),             
    
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);


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


CREATE TABLE DangNhap (
    TaiKhoan VARCHAR(50) PRIMARY KEY, 
    MatKhau VARCHAR(50) NOT NULL,
    HoTen NVARCHAR(100),              
    Quyen VARCHAR(20) DEFAULT 'SinhVien' 
);
GO
CREATE TRIGGER TG_TuDongTaoTaiKhoan
ON SinhVien
AFTER INSERT
AS
BEGIN
    
    INSERT INTO DangNhap (TaiKhoan, MatKhau, HoTen, Quyen)
    SELECT 
        i.MaSV,             
        '123',              
        i.HoTen,            
        'SinhVien'          
    FROM inserted i;
END;
GO


INSERT INTO Lop VALUES ('DH22TH1', N'Đại học Tin Học K22 - Lớp 1', N'Công nghệ thông tin');
INSERT INTO Lop VALUES ('DH23AV', N'Đại học Ngôn Ngữ Anh K23', N'Ngoại Ngữ');
INSERT INTO Lop VALUES ('DH21SP', N'Đại học Sư Phạm Toán K21', N'Sư Phạm');


INSERT INTO MonHoc VALUES ('TIN01', N'Nhập môn Lập trình', 3);
INSERT INTO MonHoc VALUES ('ML01', N'Triết học Mác - Lênin', 3);
INSERT INTO MonHoc VALUES ('ANH01', N'Tiếng Anh căn bản 1', 2);
INSERT INTO MonHoc VALUES ('CSDL', N'Cơ sở dữ liệu', 4);


INSERT INTO DangNhap (TaiKhoan, MatKhau, HoTen, Quyen) 
VALUES ('admin', '123', N'Giáo Vụ Khoa CNTT', 'GiaoVien');


INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop) 
VALUES ('DQU001', N'Lê Minh Trí', '2004-05-10', N'Nam', N'Long Xuyên, An Giang', 'DH22TH1');

INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop) 
VALUES ('DQU002', N'Nguyễn Ngọc Hân', '2005-08-20', N'Nữ', N'Châu Đốc, An Giang', 'DH23AV');
GO
ALTER TABLE SinhVien
ADD SoDienThoai VARCHAR(15);
ALTER TABLE SinhVien ADD AnhThe VARBINARY(MAX);
INSERT INTO Diem (MaSV, MaMH, DiemQuaTrinh, DiemThi, HocKy, NamHoc)
VALUES 
  (N'DQU001', N'CSDL', 8.5, 7.0, 1, N'2024-2025'),
  (N'DQU001', N'TIN01', 9.0, 8.0, 1, N'2024-2025'),
  (N'DQU002', N'ANH01', 7.0, 6.5, 1, N'2024-2025');
