using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace Demo_Paint
{
    [Serializable()]
    class SixStar : Elip
    {
        #region Thuộc tính
        #endregion

        #region Khởi tạo
        public SixStar()
            : base()
        {
            soDiemDieuKhien = 8;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = 14;
        }

        public SixStar(Color mauVe, int doDamNet)
            : base(mauVe, doDamNet)
        {
            soDiemDieuKhien = 8;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = 14;
        }
        public SixStar(Color mauve, int dodamnet, Point diembatdau, Point diemketthuc, Point diemmousedown,
     int sodiemdieukhien, GraphicsPath graphicspath, Region khuvuc, int vitrichuotsovoihinhve,
     bool dichuyen, bool thaydoikichthuoc, int loaihinh)
            : base(mauve, dodamnet, diembatdau, diemketthuc, diemmousedown, sodiemdieukhien, graphicspath, khuvuc, vitrichuotsovoihinhve, dichuyen, thaydoikichthuoc, loaihinh)
        {
            mauVe = mauve;
            doDamNet = dodamnet;
            diemBatDau = diembatdau;
            diemKetThuc = diemketthuc;
            diemMouseDown = diemmousedown;
            soDiemDieuKhien = sodiemdieukhien;
            graphicsPath = graphicspath;
            khuVuc = khuvuc;
            viTriChuotSoVoiHinhVe = vitrichuotsovoihinhve;
            diChuyen = dichuyen;
            thayDoiKichThuoc = thaydoikichthuoc;
            loaihinh = loaiHinh;
        }
        #endregion

        #region Tuần tự hóa và giải tuần tự hóa
        public SixStar(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            khuVuc = new Region(VeHCN(diemBatDau, diemKetThuc));
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        #endregion

        #region Phương thức
        // Vẽ six star cho các trường hợp di chuyển khác nhau,
        public override void Ve(Graphics g)
        {
            Point diem1, diem2, diem3, diem4;
            diem1 = new Point(DiemDieuKhien(1).X, DiemDieuKhien(1).Y+((DiemDieuKhien(4).Y - DiemDieuKhien(1).Y) / 2));
            diem2 = new Point(DiemDieuKhien(1).X, DiemDieuKhien(4).Y+((DiemDieuKhien(6).Y - DiemDieuKhien(4).Y) / 2));
            diem3 = new Point(DiemDieuKhien(3).X, DiemDieuKhien(3).Y+((DiemDieuKhien(5).Y - DiemDieuKhien(3).Y) / 2));
            diem4 = new Point(DiemDieuKhien(3).X, DiemDieuKhien(5).Y+((DiemDieuKhien(8).Y - DiemDieuKhien(5).Y) / 2));

            Pen pen = new Pen(mauVe, doDamNet);
            g.DrawLine(pen, DiemDieuKhien(2), diem2);
            g.DrawLine(pen, diem2, diem4);
            g.DrawLine(pen, diem4, DiemDieuKhien(2));
            g.DrawLine(pen, diem1, DiemDieuKhien(7));
            g.DrawLine(pen, DiemDieuKhien(7), diem3);
            g.DrawLine(pen, diem3, diem1);

            pen.Dispose();
        }
        #endregion
    }
}
