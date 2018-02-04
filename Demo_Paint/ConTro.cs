using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Demo_Paint
{
    [Serializable()]
    class ConTro:HinhChuNhat
    {
#region Thuộc tính
#endregion

#region Khởi tạo
        public ConTro()
            : base()
        {
            soDiemDieuKhien = 0;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = -1;
        }
        public ConTro(Color mauVe, int doDamNet)
            : base(mauVe, doDamNet)
        {
            soDiemDieuKhien = 0;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = -1;
        }
#endregion

#region Phương thức
        // Vẽ
        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(Color.Chartreuse, 1);
            pen.DashStyle = DashStyle.DashDotDot;
            pen.DashOffset = 10;
            g.DrawRectangle(pen, VeHCN(diemBatDau, diemKetThuc));
            pen.Dispose();
        }
        public override void Mouse_Up(object sender)
        {
            
        }
#endregion
    }
}
