using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Paint
{
    [Serializable()]
    class Import:HinhChuNhat
    {
#region Thuộc tính
#endregion

#region Khởi tạo
        public Import()
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
            loaiHinh = 0;
            //hinhNen = null;
        }

        public Import(Color mauVe, int doDamNet, Image hinhnen)
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
            loaiHinh = 0;
            hinhNen = hinhnen;
        }
        public Import(Color mauve , int dodamnet , Point diembatdau , Point diemketthuc , Point diemmousedown ,
     int sodiemdieukhien , GraphicsPath graphicspath , Region khuvuc , int vitrichuotsovoihinhve ,
     bool dichuyen , bool thaydoikichthuoc , int loaihinh, Image hinhnen)
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
            hinhNen = hinhnen;
        }
#endregion

#region Tuần tự hóa và giải tuần tự hóa
        public Import(SerializationInfo info, StreamingContext ctxt)
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
            //diemKetThuc.X = diemBatDau.X + hinhNen.Width;
            //diemKetThuc.Y = diemBatDau.Y + hinhNen.Height;
            if (hinhNen != null)
                g.DrawImage(hinhNen, diemBatDau);
        }
#endregion
    }
}
