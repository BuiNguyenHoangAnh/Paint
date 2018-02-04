
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Demo_Paint
{
    [Serializable()]
    class Star5 : Elip
    {
        #region Thuộc tính
        #endregion

        #region Khởi tạo
        public Star5()
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
            loaiHinh = 13;
        }

        public Star5(Color mauVe, int doDamNet)
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
            loaiHinh = 8;
        }
        public Star5(Color mauve, int dodamnet, Point diembatdau, Point diemketthuc, Point diemmousedown,
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
        public Star5(SerializationInfo info, StreamingContext ctxt)
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
        public override void Ve(Graphics g)
        {
            Point diem1, diem2, diem3, diem4, diem5;
            diem1 = DiemDieuKhien(2);
            diem2 = new Point(DiemDieuKhien(3).X, DiemDieuKhien(3).Y + (Math.Abs(DiemDieuKhien(5).Y - DiemDieuKhien(3).Y) * 2 / 3));
            diem3 = new Point(DiemDieuKhien(7).X + (Math.Abs(DiemDieuKhien(7).X - DiemDieuKhien(8).X) * 1 / 2), DiemDieuKhien(7).Y);
            diem4 = new Point(DiemDieuKhien(7).X - (Math.Abs(DiemDieuKhien(7).X - DiemDieuKhien(8).X) * 1 / 2), DiemDieuKhien(7).Y);
            diem5 = new Point(DiemDieuKhien(1).X, DiemDieuKhien(1).Y + (Math.Abs(DiemDieuKhien(4).Y - DiemDieuKhien(1).Y) * 2 / 3));
            if (diemBatDau.X < diemKetThuc.X && diemBatDau.Y < diemKetThuc.Y)
            {
                Pen pen = new Pen(mauVe, doDamNet);

                g.DrawLine(pen, diem1, diem3);
                g.DrawLine(pen, diem3, diem5);
                g.DrawLine(pen, diem4, diem2);
                g.DrawLine(pen, diem4, diem1);
                g.DrawLine(pen, diem1, diem4);
                g.DrawLine(pen, diem2, diem5);

                pen.Dispose();
            }
            else
            {
                if (diemBatDau.X > diemKetThuc.X && diemBatDau.Y < diemKetThuc.Y)
                {
                    // doi diem

                    Pen pen = new Pen(mauVe, doDamNet);

                    g.DrawLine(pen, diem1, diem3);
                    g.DrawLine(pen, diem4, diem5);
                    g.DrawLine(pen, diem3, diem2);
                    g.DrawLine(pen, diem4, diem1);
                    g.DrawLine(pen, diem1, diem4);
                    g.DrawLine(pen, diem2, diem5);

                    pen.Dispose();
                }
                else
                {
                    if (diemBatDau.X > diemKetThuc.X && diemBatDau.Y > diemKetThuc.Y)
                    {
                        diem2 = new Point(DiemDieuKhien(3).X, DiemDieuKhien(3).Y - (Math.Abs(DiemDieuKhien(5).Y - DiemDieuKhien(3).Y) * 2 / 3));
                        diem5 = new Point(DiemDieuKhien(1).X, DiemDieuKhien(1).Y - (Math.Abs(DiemDieuKhien(4).Y - DiemDieuKhien(1).Y) * 2 / 3));

                        //doi diem

                        Pen pen = new Pen(mauVe, doDamNet);

                        g.DrawLine(pen, diem1, diem3);
                        g.DrawLine(pen, diem4, diem5);
                        g.DrawLine(pen, diem3, diem2);
                        g.DrawLine(pen, diem4, diem1);
                        g.DrawLine(pen, diem1, diem4);
                        g.DrawLine(pen, diem2, diem5);
                        pen.Dispose();
                    }
                    else
                    {
                        diem2 = new Point(DiemDieuKhien(3).X, DiemDieuKhien(3).Y - (Math.Abs(DiemDieuKhien(5).Y - DiemDieuKhien(3).Y) * 2 / 3));
                        diem5 = new Point(DiemDieuKhien(1).X, DiemDieuKhien(1).Y - (Math.Abs(DiemDieuKhien(4).Y - DiemDieuKhien(1).Y) * 2 / 3));

                        Pen pen = new Pen(mauVe, doDamNet);

                        g.DrawLine(pen, diem1, diem3);
                        g.DrawLine(pen, diem3, diem5);
                        g.DrawLine(pen, diem4, diem2);
                        g.DrawLine(pen, diem4, diem1);
                        g.DrawLine(pen, diem1, diem4);
                        g.DrawLine(pen, diem2, diem5);
                        pen.Dispose();
                    }
                }
            }

        }
        #endregion
    }
}
