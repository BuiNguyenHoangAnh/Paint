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
    class Hexagon : Elip
    {
        #region Thuộc tính
        #endregion

        #region Khởi tạo
        public Hexagon()
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
            loaiHinh = 9;
        }

        public Hexagon(Color mauVe, int doDamNet)
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
            loaiHinh = 9;
        }
        public Hexagon(Color mauve, int dodamnet, Point diembatdau, Point diemketthuc, Point diemmousedown,
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
        public Hexagon(SerializationInfo info, StreamingContext ctxt)
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
        // Vẽ hexaron cho các trường hợp di chuyển khác nhau,
        public override void Ve(Graphics g)
        {
            Point diem1, diem2, diem3, diem4;
            diem1 = new Point(DiemDieuKhien(1).X + ((DiemDieuKhien(2).X - DiemDieuKhien(1).X)/2),DiemDieuKhien(1).Y);
            diem2 = new Point(DiemDieuKhien(2).X + ((DiemDieuKhien(3).X - DiemDieuKhien(2).X)/2),DiemDieuKhien(1).Y);
            diem3 = new Point(DiemDieuKhien(6).X + ((DiemDieuKhien(7).X - DiemDieuKhien(6).X)/2),DiemDieuKhien(6).Y);
            diem4 = new Point(DiemDieuKhien(7).X + ((DiemDieuKhien(8).X - DiemDieuKhien(7).X)/2),DiemDieuKhien(6).Y);

            Pen pen = new Pen(mauVe, doDamNet);
            g.DrawLine(pen, DiemDieuKhien(4), diem1);
            g.DrawLine(pen, diem1, diem2);
            g.DrawLine(pen, diem2, DiemDieuKhien(5));
            g.DrawLine(pen, DiemDieuKhien(5), diem4);
            g.DrawLine(pen, diem4, diem3);
            g.DrawLine(pen, diem3, DiemDieuKhien(4));

            pen.Dispose();
        }
        #endregion
    }
}
