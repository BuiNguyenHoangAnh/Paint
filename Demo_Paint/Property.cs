using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Demo_Paint
{
    class Property
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        private Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        private Point Location;

        public Point Location1
        {
            get { return Location; }
            set { Location = value; }
        }
        private int with;

        public int With
        {
            get { return with; }
            set { with = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        private int netve;

        public int Netve
        {
            get { return netve; }
            set { netve = value; }
        }

        public void nhap(Form1 form, HinhVe hinhve)
        {
            if (form.IDhinhHienTai != -1)
            {
                color = form.mauVe;
                Location = new Point(hinhve.diemBatDau.X, hinhve.diemBatDau.Y);
                with = Math.Abs(hinhve.diemBatDau.X - hinhve.diemKetThuc.X);
                height = Math.Abs(hinhve.diemBatDau.Y - hinhve.diemKetThuc.Y);
                netve = hinhve.doDamNet;
                if (hinhve.loaiHinh == 0)
                    Name = "Image";
                if (hinhve.loaiHinh == 1)
                    Name = "Line";
                if (hinhve.loaiHinh == 2)
                    Name = "Text";
                if (hinhve.loaiHinh == 4)
                    Name = "Rectangle";
                if (hinhve.loaiHinh == 3)
                    Name = "Oval";
                if (hinhve.loaiHinh == 5)
                    Name = "Triangle";
                if (hinhve.loaiHinh == 6)
                    Name = "Right Triangle";
                if (hinhve.loaiHinh == 7)
                    Name = "Diamond";
                if (hinhve.loaiHinh == 8)
                    Name = "Pentegon";
                if (hinhve.loaiHinh == 9)
                    Name = "Hexagon";
                if (hinhve.loaiHinh == 10)
                    Name = "up Arrow";
                if (hinhve.loaiHinh == 11)
                    Name = "Right Arrow";
                if (hinhve.loaiHinh == 12)
                    Name = "Four Point Star";
                if (hinhve.loaiHinh == 13)
                    Name = "Five Point Star";
                if (hinhve.loaiHinh == 14)
                    Name = "Six Point Star";
                if (hinhve.loaiHinh == 15)
                    Name = "Pencil";
            }
        }
    }
}
