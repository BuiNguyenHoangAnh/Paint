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
    class VanBan : HinhChuNhat
    {
        #region Thuộc tính
        private Font phongChu;
        private TextBox hopChu;
        #endregion

        #region Khởi tạo
        public VanBan()
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
            loaiHinh = 2;
            VanBan = "";
        }

        public VanBan(Color cl, int pw, Font phongchu)
            : base(cl, pw)
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
            loaiHinh = 2;
            phongChu = phongchu;
            hopChu = new TextBox();
            hopChu.Validated += new EventHandler(tbValidate);
            hopChu.Multiline = true;
            hopChu.ForeColor = mauVe;
            hopChu.BackColor = Color.White;
            hopChu.Font = phongchu;
        }

        public VanBan(Color mauve, int dodamnet, Point diembatdau, Point diemketthuc, Point diemmousedown,
            int sodiemdieukhien, GraphicsPath graphicspath, Region khuvuc, int vitrichuotsovoihinhve,
            bool dichuyen, bool thaydoikichthuoc, int loaihinh, Font phongchu)
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
            phongChu = phongchu;
            hopChu = new TextBox();
        }
        #endregion

        #region Tuần tự hóa và giải tuần tự hóa
        public VanBan(SerializationInfo info, StreamingContext ctxt)
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
        protected void tbValidate(Object sender, EventArgs e)
        {
            VanBan = hopChu.Text;
            if (VanBan != null)
                hopChu.Visible = false;
        }

        //Vẽ
        public override void Ve(Graphics g)
        {
            SolidBrush co = new SolidBrush(mauVe);
            g.DrawString(VanBan, phongChu, co, diemBatDau.X, diemBatDau.Y);
            co.Dispose();
        }
        #endregion

        #region Chiến thuật thi hành lệnh bắt đối tượng
        public override void Mouse_Up(Object sender)
        {
            graphicsPath = new GraphicsPath();
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath.AddRectangle(VeHCN(diemBatDau, diemKetThuc));
            graphicsPath.Widen(pen);
            khuVuc = new Region(VeHCN(diemBatDau, diemKetThuc));
            khuVuc.Union(graphicsPath);
            diChuyen = false;
            thayDoiKichThuoc = false;
            PictureBox hinhAnh = ((PictureBox)sender);
            hinhAnh.Parent.Controls.Add(hopChu);
            hopChu.Size = new Size(Math.Abs(diemKetThuc.X - diemBatDau.X), Math.Abs(diemKetThuc.Y - diemBatDau.Y));
            hopChu.Location = new Point(diemBatDau.X, diemBatDau.Y);
            hopChu.BringToFront();
            hopChu.Show();
            hopChu.Focus();
        }
        #endregion
    }
}
