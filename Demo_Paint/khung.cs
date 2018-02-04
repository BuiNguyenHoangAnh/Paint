using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Demo_Paint
{
    class khung
    {
        #region khai báo biến và hàm khởi tạo
        public Point x, y, z = new Point(0, 0);
        public bool a = false, b = false, c = false;
        public khung()
        {
        }
        #endregion

        #region vẽ các điểm co dãn của khung ảnh
        public void vekhung(Form1 frm)
        {
            Graphics g = frm.panel2.CreateGraphics();
            Pen p = new Pen(Color.Black, 4);
            // điểm co dãn dưới
            x.X = frm.panel1.Location.X + frm.panel1.Size.Width / 2;
            x.Y = frm.panel1.Location.Y + frm.panel1.Size.Height;

            //điểm co dãn phải
            y.X = frm.panel1.Location.X + frm.panel1.Size.Width;
            y.Y = frm.panel1.Location.Y + frm.panel1.Size.Height / 2;

            //điểm co dãn góc dưới trái
            z.X = frm.panel1.Location.X + frm.panel1.Size.Width;
            z.Y = frm.panel1.Location.Y + frm.panel1.Size.Height;

            frm.panel2.Refresh();
            g.DrawLine(p, x.X + 2, x.Y + 2, x.X - 2, x.Y + 2);
            g.DrawLine(p, y.X + 2, y.Y + 2, y.X + 2, y.Y - 2);
            g.DrawLine(p, z.X + 2, z.Y + 2, z.X + 2, z.Y - 2);
            //thanh cuon xuat hien khi kich thuoc trang ve vuot qua kich thuoc của form
            //thanh cuon ngang
            if (frm.panel1.Width > (frm.Width - frm.panel2.Location.X - 10))
            {
                frm.hScrollBar1.Visible = true;
                frm.hScrollBar1.Maximum = frm.panel1.Width - frm.panel2.Width + 115;
                frm.hScrollBar1.LargeChange = 110;
            }
            //thanh cuon doc
            if (frm.panel1.Height > (frm.Height - frm.panel2.Location.Y - 10))
            {
                frm.vScrollBar1.Visible = true;
                frm.vScrollBar1.Maximum = frm.panel1.Height - frm.panel2.Height + 115;
                frm.vScrollBar1.LargeChange = 110;
            }
        }
        #endregion

        #region kiểm tra chuột có Move vào điểm co dãn nếu có thì đổi cursors
        public void kiemtra(object sender, MouseEventArgs e, Form1 form1)
        {
            //khi chuột move vào điểm co dãn dưới
            if (e.X > (x.X - 20) && e.X < (x.X + 20) && e.Y < (x.Y + 10) && e.Y > (x.Y - 10))
            {
                if (a == false && b == false && c == false)
                {
                    a = true;
                    form1.panel2.Cursor = Cursors.SizeNS;
                }
            }
            else
                //khi chuột move vào điểm co dãn phải
                if (e.Y > (y.Y - 20) && e.Y < (y.Y + 20) && e.X < (y.X + 10) && e.X > (y.X - 10))
                {
                    if (a == false && b == false && c == false)
                    {
                        b = true;
                        form1.panel2.Cursor = Cursors.SizeWE;
                    }
                }
                else
                    //khi chuột move vào điểm co dãn goc dưới
                    if (e.X > z.X - 20 && e.X < z.X + 20 && e.Y < z.Y + 10 && e.Y > (z.Y - 10))
                    {
                        if (a == false && b == false && c == false)
                        {
                            c = true;
                            form1.panel2.Cursor = Cursors.SizeNWSE;
                        }
                    }
                    else
                    {
                        form1.panel2.Cursor = Cursors.Default;
                        if (form1.mousepanel2 == false)
                        {
                            a = false;
                            b = false;
                            c = false;
                        }
                    }
        }
        #endregion

        #region khi chuột Move vào điểm co dãn và mousedows thì kéo dãn
        public void keo(object sender, MouseEventArgs e, Form1 form1)
        {
            kiemtra(sender, e, form1);

            if (form1.mousepanel2 == true)
            {
                //điểm co dãn dưới
                if (a == true && b == false && c == false)
                {
                    form1.panel2.Cursor = Cursors.SizeNS;
                    form1.panel1.Height += e.Y - form1.panel1.Size.Height - form1.panel1.Location.Y;
                    form1.Invalidate();
                    vekhung(form1);
                }
                //điểm co dãn phải
                if (a == false && b == true && c == false)
                {
                    form1.panel2.Cursor = Cursors.SizeWE;
                    form1.panel1.Width += e.X - form1.panel1.Size.Width - form1.panel1.Location.X;
                    form1.Invalidate();
                    vekhung(form1);
                }
                //điểm co dãn góc dưới
                if (a == false && b == false && c == true)
                {
                    form1.panel2.Cursor = Cursors.SizeNWSE;
                    form1.panel1.Width += e.X - form1.panel1.Size.Width - form1.panel1.Location.X;
                    form1.panel1.Height += e.Y - form1.panel1.Size.Height - form1.panel1.Location.Y;
                    form1.Invalidate();
                    vekhung(form1);
                }
            }
        }
        #endregion
    }
}
