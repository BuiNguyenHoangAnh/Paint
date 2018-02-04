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
    class Pencil : HinhVe
    {
        #region Thuộc tính
        private Point[] poin = new Point[100000];
        private Point[] poin1 = new Point[100000];
        private int npoin = 0;
        #endregion

        #region Khởi tạo
        public Pencil()
            : base()
        {
            soDiemDieuKhien = 2;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddCurve(poin);
            khuVuc = new Region(graphicsPath);
            loaiHinh = 15;
        }

        public Pencil(Color mauVe, int doDamNet)
            : base(mauVe, doDamNet)
        {
            soDiemDieuKhien = 2;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddCurve(poin);
            khuVuc = new Region(graphicsPath);
            loaiHinh = 15;

        }

        public Pencil(Color mauve, int dodamnet, Point diembatdau, Point diemketthuc, Point diemmousedown,
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
        public Pencil(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {

        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        #endregion

        #region Phương thức
        // Vẽ
        public override void Ve(Graphics g)
        {
            if (npoin > 4)
            {
                poin1 = new Point[npoin];
                for (int i = 0; i < npoin; i++)
                    poin1[i] = new Point(poin[i].X, poin[i].Y);
                Pen pen = new Pen(mauVe, doDamNet);
                g.DrawCurve(pen, poin1);
                pen.Dispose();
            }
        }

        //Tạo điểm điều khiển
        protected override Point DiemDieuKhien(int viTriDiemDieuKhien)
        {
            if (viTriDiemDieuKhien == 1)
                return diemBatDau;
            return diemKetThuc;
        }

        // Thay đổi điểm Start, End khi Click vào 1 điểm điều khiển
        protected override void ThayDoiDiem(int viTriDiemDieuKhien)
        {
            if (viTriDiemDieuKhien == 1)
            {
                Point point = diemBatDau;
                diemBatDau = diemKetThuc;
                diemKetThuc = point;
            }
        }

        // Di chuyển đối tượng khi biết 1 điểm điều khiển và điểm đến
        protected override void ThayDoiKichThuoc(int viTriDiemDieuKhien, Point point)
        {
            diemKetThuc = point;
        }

        //
        public override void DiChuyen(int deltaX, int deltaY)
        {
            {
                for(int i=0; i<npoin;i++)
                {
                    poin[i].X += deltaX;
                    poin[i].Y += deltaY;
                }
                diemBatDau.X += deltaX;
                diemBatDau.Y += deltaY;
                diemKetThuc.X += deltaX;
                diemKetThuc.Y += deltaY;
            }
        }

        //Vẽ khung
        public override void VeKhung(Graphics g)
        {
            Pen pen = new Pen(Color.Chartreuse, 1);
            for (int i = 1; i <= soDiemDieuKhien; i++)
            {
                g.DrawRectangle(pen, VeChamVuong(i, 3));
                g.FillRectangle(new SolidBrush(Color.Blue), VeChamVuong(i, 6));
            }

            pen.DashStyle = DashStyle.DashDotDot;
            pen.DashOffset = 20;
            if (diemBatDau.Y < diemKetThuc.Y && diemBatDau.X < diemKetThuc.X)
                g.DrawRectangle(pen, new Rectangle(diemBatDau.X, diemBatDau.Y, Math.Abs(diemBatDau.X - diemKetThuc.X), Math.Abs(diemBatDau.Y - diemKetThuc.Y)));
            else if (diemBatDau.Y > diemKetThuc.Y && diemBatDau.X < diemKetThuc.X)
                g.DrawRectangle(pen, new Rectangle(diemBatDau.X, diemKetThuc.Y, Math.Abs(diemBatDau.X - diemKetThuc.X), Math.Abs(diemBatDau.Y - diemKetThuc.Y)));
            else if(diemBatDau.Y<diemKetThuc.Y&&diemBatDau.X>diemKetThuc.X)
                g.DrawRectangle(pen, new Rectangle(diemKetThuc.X, diemBatDau.Y, Math.Abs(diemBatDau.X - diemKetThuc.X), Math.Abs(diemBatDau.Y - diemKetThuc.Y)));
            else
                g.DrawRectangle(pen, new Rectangle(diemKetThuc.X, diemKetThuc.Y, Math.Abs(diemBatDau.X - diemKetThuc.X), Math.Abs(diemBatDau.Y - diemKetThuc.Y)));
            pen.Dispose();
        }

        // Sự kiện chuột
        public override void Mouse_Down(MouseEventArgs e)
        {
            viTriChuotSoVoiHinhVe = KiemTraViTri(e.Location);
            if (viTriChuotSoVoiHinhVe > 0)
            {
                thayDoiKichThuoc = true;
                ThayDoiDiem(viTriChuotSoVoiHinhVe);
            }
            else if (viTriChuotSoVoiHinhVe == 0)
            {
                diChuyen = true;
                diemMouseDown = e.Location;
            }
            else
            {
                diemBatDau = e.Location;
                diemKetThuc.X = e.X;
                diemKetThuc.Y = e.Y - 1;
            }
        }

        public override void Mouse_Move(MouseEventArgs e)
        {
            if (thayDoiKichThuoc == true)
            {
                ThayDoiKichThuoc(viTriChuotSoVoiHinhVe, e.Location);
            }
            else if (diChuyen == true)
            {
                int deltaX = e.X - diemMouseDown.X;
                int deltaY = e.Y - diemMouseDown.Y;
                diemMouseDown = e.Location;
                DiChuyen(deltaX, deltaY);
            }
            else
            {
                poin[npoin] = new Point(e.X, e.Y);
                npoin++;
                if (npoin >= 4)
                {
                    poin1 = new Point[4];
                    for (int i = 0; i < 4; i++)
                        poin1[i] = new Point(poin[npoin - 1 - i].X, poin[npoin - 1 - i].Y);
                }
                diemKetThuc = e.Location;
            }
        }

        public override void Mouse_Up(Object sender)
        {
            graphicsPath = new GraphicsPath();
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath.AddCurve(poin1);

            GraphicsPath gp = new GraphicsPath();
            if (diemBatDau.Y < diemKetThuc.Y && diemBatDau.X < diemKetThuc.X)
                gp.AddRectangle(new Rectangle(diemBatDau.X, diemBatDau.Y, Math.Abs(diemBatDau.X - diemKetThuc.X), Math.Abs(diemBatDau.Y - diemKetThuc.Y)));
            else if (diemBatDau.Y > diemKetThuc.Y && diemBatDau.X < diemKetThuc.X)
                gp.AddRectangle(new Rectangle(diemBatDau.X, diemKetThuc.Y, Math.Abs(diemBatDau.X - diemKetThuc.X), Math.Abs(diemBatDau.Y - diemKetThuc.Y)));
            else if (diemBatDau.Y < diemKetThuc.Y && diemBatDau.X > diemKetThuc.X)
                gp.AddRectangle(new Rectangle(diemKetThuc.X, diemBatDau.Y, Math.Abs(diemBatDau.X - diemKetThuc.X), Math.Abs(diemBatDau.Y - diemKetThuc.Y)));
            else
                gp.AddRectangle(new Rectangle(diemKetThuc.X, diemKetThuc.Y, Math.Abs(diemBatDau.X - diemKetThuc.X), Math.Abs(diemBatDau.Y - diemKetThuc.Y)));
            khuVuc = new Region(gp);

            diChuyen = false;
            thayDoiKichThuoc = false;
            viTriChuotSoVoiHinhVe = -1;
        }

        #endregion

    }
}
