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
    class HinhThoi:Elip
    {
#region Thuộc tính
#endregion

#region Khởi tạo
        public HinhThoi()
            :base()
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
            loaiHinh = 7;
        }

        public HinhThoi(Color mauVe, int doDamNet)
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
            loaiHinh = 7;
        }
        public HinhThoi(Color mauve , int dodamnet , Point diembatdau , Point diemketthuc , Point diemmousedown ,
     int sodiemdieukhien , GraphicsPath graphicspath , Region khuvuc , int vitrichuotsovoihinhve ,
     bool dichuyen , bool thaydoikichthuoc , int loaihinh)
            : base(mauve , dodamnet , diembatdau , diemketthuc , diemmousedown , sodiemdieukhien , graphicspath , khuvuc , vitrichuotsovoihinhve , dichuyen , thaydoikichthuoc , loaihinh)
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
        public HinhThoi(SerializationInfo info, StreamingContext ctxt)
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
        // Vẽ thoi cho các trường hợp di chuyển khác nhau,
        public override void Ve(Graphics g)
        {
                Pen pen = new Pen(mauVe, doDamNet);
                g.DrawLine(pen, DiemDieuKhien(2), DiemDieuKhien(4));
                g.DrawLine(pen, DiemDieuKhien(4), DiemDieuKhien(7));
                g.DrawLine(pen, DiemDieuKhien(7), DiemDieuKhien(5));
                g.DrawLine(pen, DiemDieuKhien(5), DiemDieuKhien(2));
                pen.Dispose();
        }
#endregion
    }
}
