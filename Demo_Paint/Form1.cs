using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Paint
{
    public partial class Form1 : Form
    {
        #region khai bao bien
        private Property pro = new Property(); //PropertyGrid hiện thông tin của đối tượng hình ảnh

        private listHinhVe lHV;          // List đối tượng
        private HinhVe hinhHienTai;       // đối tượng hình hiện tại sẽ vẽ
        public int IDhinhHienTai;       // ID của đối tượng hình hiện tại

        public Color mauVe; // Màu vẽ
        public int doDamNet; // Độ đậm

        private Bitmap hinhNenChim;     //hình nền
        private Bitmap hinhNenNoi;

        bool isMoving;      //kiểm tra chuột đang di chuyển
        bool savedFlag = true;      //đánh dấu đã save hay chưa

        String lastFileName = "";       //tên file cuối cùng được lưu

        HinhVe hinhCopy;           // sử dụng cho cut, copy, paste

        public bool mousepanel2 = false;    //kiểm tra sự kéo dãn trang vẽ

        private khung kh = new khung();     //trang vẽ

        private bool laymau = false;        //sử dụng để kiểm tra pickcolor

        InstalledFontCollection fontFamilies = new InstalledFontCollection();       //lấy các font có sẵn trong hệ thống
        public Font font, font1;
        public FontStyle fontstyle = FontStyle.Regular;

        public Image img;       //hình ảnh được import vào

        private tool tl = new tool();       //refesh tool
        #endregion

        public Form1()
        {
            InitializeComponent();
            MyInIt();
        }

        public void MyInIt()
        {
            foreach (FontFamily family in fontFamilies.Families)
                fontToolStripComboBox.Items.Add(family.Name);
            font = new Font(fontToolStripComboBox.Text, int.Parse(sizeToolStripComboBox.Text), fontstyle, GraphicsUnit.Point, ((byte)(0)));

            lHV = new listHinhVe();

            hinhNenChim = new Bitmap(panel1.Width, panel1.Height, panel1.CreateGraphics()); //tạo 1 hình bitmap
            Graphics g = Graphics.FromImage(hinhNenChim);   //lấy đối tượng Graphics từ bitmap
            g.Clear(Color.White);                           //xóa trắng bề mặt

            hinhNenNoi = new Bitmap(panel1.Width, panel1.Height, panel1.CreateGraphics());
            g = Graphics.FromImage(hinhNenNoi);
            g.Clear(Color.White);

            mauVe = Color.Black;
            thecurrentcolorToolStripButton.BackColor = mauVe;
            doDamNet = 1;

            showToolStripMenuItem.Checked = true;

            thecurrentcolorToolStripButton.BackColor = mauVe;
            lbSize.Text = doDamNet.ToString() + " pt";

            isMoving = false;

            hinhCopy = null;

            IDhinhHienTai = -1;
            lHV.listHinh.Clear();

            tl.refreshtool(this);

            panel1.Refresh();       //vẽ lại pictureBox-làm mới
        }

        HinhVe LayHinhVeHienTai(int IDHinhAnhHienTai)
        {
            switch (IDHinhAnhHienTai)
            {
                case 0: return new Import(mauVe, doDamNet, img);
                case 1: return new DuongThang(mauVe, doDamNet);
                case 2: return new VanBan(mauVe, doDamNet, font);
                case 3: return new Elip(mauVe, doDamNet);
                case 4: return new HinhChuNhat(mauVe, doDamNet);
                case 5: return new TamGiac(mauVe, doDamNet);
                case 6: return new TamGiacVuong(mauVe, doDamNet);
                case 7: return new HinhThoi(mauVe, doDamNet);
                case 8: return new NguGiac(mauVe, doDamNet);
                case 9: return new Hexagon(mauVe, doDamNet);
                case 10: return new LeftArrow(mauVe, doDamNet);
                case 11: return new uparrow(mauVe, doDamNet);
                case 12: return new FourStar(mauVe, doDamNet);
                case 13: return new Star5(mauVe, doDamNet);
                case 14: return new SixStar(mauVe, doDamNet);
                case 15: return new Pencil(mauVe, doDamNet);
                default: return new ConTro();
            }
        }

        #region su kien voi panel1
        #region mouse down
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if(laymau)
            {
                Point x = new Point();
                x.X = e.X;
                x.Y = e.Y;
                coloerpicker(x);
            }
            savedFlag = false;  //khi mouse_down =>có thay đổi=>đánh dấu là chưa lưu
            if (e.Button == MouseButtons.Left)
            {
                if (isMoving)
                {
                    //đang di chuyển hình
                }
                else
                {
                    if (hinhHienTai == null || hinhHienTai.KiemTraViTri(e.Location) == -1)
                    {
                        hinhHienTai = LayHinhVeHienTai(IDhinhHienTai);
                    }
                    if (hinhHienTai != null)    //&& hinhHienTai.loaiHinh!=0)
                    {
                        hinhHienTai.Mouse_Down(e);          //gọi sự kiện mouse_down của hình
                        panel1.Refresh();               //làm mới
                        hinhHienTai.VeKhung(panel1.CreateGraphics());      //vẽ 8 hình chữ nhật nhỏ (chấm vuông nhỏ) làm khung xung quanh   
                        lHV.listHinh.Insert(lHV.listHinh.Count, hinhHienTai);            //thêm hình mới vào list
                    }
                }
            }
            else
            {
                hinhHienTai = null;
            }
        }

        #endregion

        #region mouse move
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            mouselocationToolStripStatusLabel.Text = e.X.ToString() + "," + e.Y.ToString();
            if (IDhinhHienTai == -1)  //nút "Vị trí và kích cỡ" đã được bấm
            {
                picktoolToolStripButton_Click(sender, e);

                if (isMoving == false)
                {
                    for (int i = lHV.listHinh.ToArray().Length - 1; i >= 0; i--)        //kiểm tra từng hình xem hình nào được chọn
                    {
                        int vt = (lHV.listHinh.ToArray())[i].KiemTraViTri(e.Location);
                        if (vt == 0)        //...chuột di chuyển trên bề mặt    
                        {
                            hinhHienTai = (lHV.listHinh.ToArray())[i];
                            if (e.Button == MouseButtons.Left)      //=> sẽ di chuyển hình này
                            {
                                Cursor = Cursors.Hand;
                                hinhHienTai.diChuyen = true;             //cho phép di chuyển
                                hinhHienTai.thayDoiKichThuoc = false;   //không cho phép thay đổi kích thước
                                isMoving = true;                        //bật cờ đang di chuyển

                                IDhinhHienTai = hinhHienTai.loaiHinh;
                                panel1.Refresh();
                                hinhHienTai.VeKhung(panel1.CreateGraphics());        //vẽ khung
                                lHV.listHinh.RemoveAt(i);                                           //sau khi di chuyển sẽ phát sinh hình mới tại vị trí mới=>xóa hình cũ                          
                            }
                            else // chuột đi qua mà không bấm
                            {
                                isMoving = false;
                                hinhHienTai.diChuyen = false;
                            }
                            Cursor = Cursors.Hand;
                            panel1.Refresh();
                            hinhHienTai.VeKhung(panel1.CreateGraphics());
                            break;
                        }
                        else if (vt > 0) //...chuột chỉ đúng điểm điều khiển (1 trong 8 chấm vuông nhỏ làm khung)   => sẽ thay đổi kích thước hình này
                        {
                            hinhHienTai = (lHV.listHinh.ToArray())[i];
                            if (e.Button == MouseButtons.Left)
                            {
                                hinhHienTai.thayDoiKichThuoc = true;        //cho phép thay đổi kích thước
                                hinhHienTai.diChuyen = false;               //không cho phép di chuyển
                                isMoving = true;

                                IDhinhHienTai = hinhHienTai.loaiHinh;
                                panel1.Refresh();
                                hinhHienTai.VeHCNDiemDieuKhien(panel1.CreateGraphics(), 5);
                                lHV.listHinh.RemoveAt(i);
                            }
                            else
                            {
                                isMoving = false;
                                hinhHienTai.thayDoiKichThuoc = false;
                            }
                            Cursor = Cursors.Cross;
                            panel1.Refresh();
                            hinhHienTai.VeHCNDiemDieuKhien(panel1.CreateGraphics(), 5);
                            break;
                        }
                        else //tìm trong danh sách không có hình nào bị chuột đi qua
                        {
                            Cursor = Cursors.Default;
                        }
                    }
                }
            }
            else  //không phải nút "Vị trí và kích cỡ" => là nút vẽ hình hoặc nút "Chuột"
            {
                if (hinhHienTai != null)
                {

                    if (hinhHienTai.KiemTraViTri(e.Location) > 0)   //nếu chuột chỉ đúng 1 trong 8 chấm vuông nhỏ => đổi chuột thành hình dấu +
                        Cursor = Cursors.Cross;

                    else if (hinhHienTai.KiemTraViTri(e.Location) == 0)     //tương tự với lúc chuột nằm trong hình => chuột hình bàn tay
                        Cursor = Cursors.Hand;
                    else
                        Cursor = Cursors.Default;       //còn lại thì mặc định
                }

                if (e.Button == MouseButtons.Left)
                {

                    if (hinhHienTai != null)
                    {
                        //làm nổi hình mới nhất lên (hiện khung hình đó)
                        hinhHienTai.Mouse_Move(e);
                        panel1.Refresh();
                        hinhHienTai.VeKhung(panel1.CreateGraphics());
                    }
                }
            }
        }
        #endregion

        #region mouse up
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (hinhHienTai != null && hinhHienTai.loaiHinh == -1 && isMoving == false)
            {
                lHV.XoaHinhCuoi();
                panel1.Refresh();

                hinhHienTai = null;
            }
            if (hinhHienTai != null && isMoving == false)
            {
                lHV.listHinh.Insert(lHV.listHinh.Count, hinhHienTai); //thêm hình mới vào list
                hinhHienTai.Mouse_Up(sender);
                hinhHienTai.VeKhung(panel1.CreateGraphics());
            }
            if (isMoving)
            {
                hinhHienTai.Mouse_Up(sender);
                panel1.Refresh();
                isMoving = false;
            }
        }
        #endregion

        #region event panel1 paint
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            hinhNenChim = hinhNenNoi.Clone(new Rectangle(0, 0, hinhNenNoi.Width, hinhNenNoi.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb); //sao chép hình nền

            Graphics g = Graphics.FromImage(hinhNenChim);

            if (lHV.listHinh.Count > 0)
            {
                lHV.Ve(g);                  //vẽ các hình có trong listHinh lên hình nền
                pro.nhap(this, hinhHienTai);            // hiện thông tin của hình trong property
                propertyGrid1.SelectedObject = pro;
            }
            e.Graphics.DrawImageUnscaled(hinhNenChim, 0, 0);
        }
        #endregion
        #endregion

        #region ve hinh
        private void fourpointStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 12;
            tl.refreshtool(this);
            fourpointStarToolStripMenuItem.Checked = true;
        }

        private void picktoolToolStripButton_Click(object sender, EventArgs e)
        {
            IDhinhHienTai = -1;
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 1;
            tl.refreshtool(this);
            lineToolStripMenuItem.Checked = true;
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 4;
            tl.refreshtool(this);
            rectangleToolStripMenuItem.Checked = true;
        }

        private void ovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 3;
            tl.refreshtool(this);
            ovalToolStripMenuItem.Checked = true;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 5;
            tl.refreshtool(this);
            triangleToolStripMenuItem.Checked = true;
        }

        private void rightTriangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 6;
            tl.refreshtool(this);
            rightTriangleToolStripMenuItem.Checked = true;
        }

        private void diamondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 7;
            tl.refreshtool(this);
            diamondToolStripMenuItem.Checked = true;
        }

        private void pentegonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 8;
            tl.refreshtool(this);
            pentegonToolStripMenuItem1.Checked = true;
        }

        private void hexagonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 9;
            tl.refreshtool(this);
            hexagonToolStripMenuItem1.Checked = true;
        }

        private void sixpointStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 14;
            tl.refreshtool(this);
            sixpointStarToolStripMenuItem.Checked = true;
        }

        private void upArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 11;
            tl.refreshtool(this);
            upArrowToolStripMenuItem.Checked = true;
        }

        private void fivepointStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 13;
            tl.refreshtool(this);
            fivepointStarToolStripMenuItem.Checked = true;
        }

        private void rightArrowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 10;
            tl.refreshtool(this);
            rightTriangleToolStripMenuItem.Checked = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = false;
            copyToolStripButton.Enabled = false;
            IDhinhHienTai = 15;
            tl.refreshtool(this);
            toolStripButton1.Checked = true;
        }
        #endregion

        #region mau ve
        private void blackToolStripButton_Click(object sender, EventArgs e)
        {
            mauVe = Color.Black;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }

        private void grayToolStripButton_Click(object sender, EventArgs e)
        {
            mauVe = Color.Gray;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }

        private void darkredToolStripButton_Click(object sender, EventArgs e)
        {
            mauVe = Color.DarkRed;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }

        private void redToolStripButton_Click(object sender, EventArgs e)
        {
            mauVe = Color.Red;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }

        private void yellowToolStripButton_Click(object sender, EventArgs e)
        {
            mauVe = Color.Yellow;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }

        private void greenToolStripButton_Click(object sender, EventArgs e)
        {
            mauVe = Color.Green;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }

        private void purpleToolStripButton_Click(object sender, EventArgs e)
        {
            mauVe = Color.Purple;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }

        private void brownToolStripButton_Click(object sender, EventArgs e)
        {
            mauVe = Color.Brown;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }

        private void morecolorsToolStripButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                mauVe = colorDialog1.Color;
                thecurrentcolorToolStripButton.BackColor = mauVe;
            }
        }
        #endregion

        #region su kien voi panel2
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            mousepanel2 = true;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            kh.keo(sender, e, this);
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            mousepanel2 = false;
        }
        #endregion

        #region su kien voi form chinh
        private void Form1_Shown(object sender, EventArgs e)
        {
            kh.vekhung(this);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (savedFlag == false && lHV.listHinh != null)
            {
                DialogResult dr = MessageBox.Show("Do you want to save your file?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (dr == DialogResult.Yes)
                    {
                        saveToolStripButton_Click(saveToolStripButton, e);
                    }
                    else this.OnClosing(e);
                }

            }
        }
        #endregion

        #region save
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if ((lastFileName.Equals("") == false && savedFlag == false))
            {
                lHV.SaveListHinh(lastFileName);
                savedFlag = true;
            }
            else
            {
                if (sender.Equals(saveAsToolStripMenuItem) || (lastFileName.Equals("") && sender.Equals(saveToolStripButton) || (lastFileName.Equals("") && sender.Equals(saveToolStripMenuItem))))
                {
                    saveFileDialog1.Title = "Save File";
                    saveFileDialog1.Filter = "File DemoPaint (*.ABHK)|*.ABHK|Bitmap file(*.bmp)|*.bmp|All File (*.*)|*.*";
                    saveFileDialog1.CheckPathExists = true;
                    saveFileDialog1.OverwritePrompt = true;

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        if (lHV.SaveListHinh(saveFileDialog1.FileName) == false)
                        {
                            try
                            {
                                hinhNenChim.Save(saveFileDialog1.FileName);
                                lastFileName = saveFileDialog1.FileName;
                                savedFlag = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error");
                            }
                        }
                        else
                        {
                            lastFileName = saveFileDialog1.FileName;
                            savedFlag = true;
                        }
                    }

                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton_Click(sender, e);
        }
        #endregion

        #region open
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if (savedFlag == false)
            {
                DialogResult rep = MessageBox.Show("Do you want to save your file?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (rep == DialogResult.Yes)
                {
                    sender = saveToolStripButton;
                    saveToolStripButton_Click(sender, e);
                    sender = openToolStripButton;
                }
                else if (rep == DialogResult.Cancel)
                    return;
            }
            openFileDialog1.Title = "Open File";
            openFileDialog1.Filter = "File DemoPaint (*.ABHK)|*.ABHK|BMP (*.bmp)|*.bmp|All File (*.*)|*.*";
            openFileDialog1.CheckFileExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                hinhNenChim = new Bitmap(panel1.Width, panel1.Height, panel1.CreateGraphics()); //tạo 1 hình bit map lấp đầy pictureBox
                Graphics g = Graphics.FromImage(hinhNenChim);
                g.Clear(Color.White);
                hinhNenNoi = new Bitmap(panel1.Width, panel1.Height, panel1.CreateGraphics());
                g = Graphics.FromImage(hinhNenNoi);
                g.Clear(Color.White);
                if (lHV.OpenListHinh(openFileDialog1.FileName) == false)
                {
                    try
                    {

                        if (hinhHienTai != null)
                            hinhHienTai = null;
                        lHV.listHinh.Clear();
                        lastFileName = openFileDialog1.FileName;
                        hinhNenNoi = new Bitmap(openFileDialog1.FileName);
                        panel1.Size = hinhNenNoi.Size;
                        panel1.Refresh();
                        savedFlag = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Error");
                    }
                }
                else
                {
                    lastFileName = openFileDialog1.FileName;
                    savedFlag = true;
                    panel1.Refresh();
                }
            }
            savedFlag = false;
            kh.vekhung(this);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openToolStripButton_Click(sender, e);
        }
        #endregion

        #region help and about
        private void aboutPaintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DemoPaint\nVersion 1.0","About");
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("---------------Chương trình DemoPaint--------------\n\tđược thực hiện bởi nhóm sinh viên\ntrường Đại học Công nghệ Thông tin ĐHQG HCM\nGVHD: ThS. Phạm Thi Vương\nNhóm sinh viên thực hiện:\n\tBùi Nguyễn Hoàng Anh - 15520012\n\tDương Chí Bình - 15520050\n\tLê Bá Phúc Hiếu - 15520224\n\tDương Hoàng Khang - 15520337\nTruy cập https://goo.gl/i87kul để biết thêm chi tiết","Help");
        }

        private void productHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helpToolStripButton_Click(sender, e);
        }
        #endregion

        #region share to facebook
        private void shareToolStripButton_Click(object sender, EventArgs e)
        {
            sender = saveToolStripButton;
            saveToolStripButton_Click(sender, e);
            Facebook fb = new Facebook();
            if (savedFlag)
                fb.Show();
        }

        private void shareToFacebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shareToolStripButton_Click(sender, e);
        }
        #endregion

        #region new
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            if (lHV.listHinh.Count > 0)
            {
                DialogResult rep = MessageBox.Show("Do you want to save your file?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (rep != DialogResult.Cancel)
                {
                    if (rep == DialogResult.Yes)
                    {
                        sender = saveToolStripButton;
                        saveToolStripButton_Click(sender, e);
                        sender = newToolStripButton;
                    }
                    MyInIt();
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripButton_Click(sender, e);
        }
        #endregion

        #region save as
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save As";
            saveFileDialog1.Filter = "File DemoPaint (*.ABHK)|*.ABHK|Bitmap file(*.bmp)|*.bmp|All File (*.*)|*.*";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.OverwritePrompt = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (lHV.SaveListHinh(saveFileDialog1.FileName) == false)
                {
                    try
                    {
                        hinhNenChim.Save(saveFileDialog1.FileName);
                        lastFileName = saveFileDialog1.FileName;
                        savedFlag = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error ", ex.Message);
                    }
                }
                else
                {
                    lastFileName = saveFileDialog1.FileName;
                    savedFlag = true;
                }
            }
        }
        #endregion

        #region exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClosingEventArgs ex = new FormClosingEventArgs(CloseReason.UserClosing, false);
            Form1_FormClosing(sender, ex);
            if (ex.Cancel == false)
                this.Close();
        }
        #endregion

        #region print
        private void PrintPage(object o, PrintPageEventArgs e)
        {
            try
            {
                {
                    //Load the image
                    Image img = hinhNenChim;

                    //Adjust the size of the image to the page to print the full image without loosing any part of it
                    Rectangle m = e.MarginBounds;

                    if ((double)img.Width / (double)img.Height > (double)m.Width / (double)m.Height) // image is wider
                    {
                        m.Height = (int)((double)img.Height / (double)img.Width * (double)m.Width);
                    }
                    else
                    {
                        m.Width = (int)((double)img.Width / (double)img.Height * (double)m.Height);
                    }
                    e.Graphics.DrawImage(img, m);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void Print()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            printDialog1.Document = pd;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Print();
        }
        #endregion

        #region cut
        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            if (hinhHienTai != null && hinhHienTai.loaiHinh != -1)
            {
                hinhCopy = hinhHienTai;
                lHV.XoaHinhCuoi();
                lHV.listHinh.Remove(hinhHienTai);
                panel1.Refresh();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cutToolStripButton_Click(sender, e);
        }
        #endregion

        #region copy
        private void copy(HinhVe hinhHienTai, HinhVe hinhCopy)
        {
            hinhCopy.diemBatDau = hinhHienTai.diemBatDau;
            hinhCopy.diemKetThuc = hinhHienTai.diemKetThuc;
            hinhCopy.khuVuc = hinhHienTai.khuVuc;
            hinhCopy.mauVe = hinhHienTai.mauVe;
            hinhCopy.doDamNet = hinhHienTai.doDamNet;
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            if (hinhHienTai != null && hinhHienTai.loaiHinh != 0)
            {
                switch (hinhHienTai.loaiHinh)
                {
                    case 0: hinhCopy = new Import(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 1: hinhCopy = new DuongThang(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 2: hinhCopy = new VanBan(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 3: hinhCopy = new Elip(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 4: hinhCopy = new HinhChuNhat(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 5: hinhCopy = new TamGiac(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 6: hinhCopy = new TamGiacVuong(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 7: hinhCopy = new HinhThoi(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 8: hinhCopy = new NguGiac(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 9: hinhCopy = new Hexagon(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 10: hinhCopy = new LeftArrow(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 11: hinhCopy = new uparrow(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 12: hinhCopy = new FourStar(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 13: hinhCopy = new Star5(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 14: hinhCopy = new SixStar(); copy(hinhHienTai, hinhCopy);
                        break;
                    case 15: hinhCopy = new Pencil(); copy(hinhHienTai, hinhCopy);
                        break;
                    default: break;
                }
                panel1.Refresh();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripButton_Click(sender, e);
        }
        #endregion

        #region paste
        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            if (hinhCopy != null)
            {
                if (sender == cutToolStripButton)
                {
                    hinhCopy.diChuyen = true;
                    hinhCopy.DiChuyen(-hinhCopy.diemBatDau.X, -hinhCopy.diemBatDau.Y);
                    hinhCopy.Mouse_Up(null);
                    lHV.listHinh.Add(hinhCopy);
                    panel1.Refresh();
                    hinhCopy.VeKhung(panel1.CreateGraphics());
                    hinhHienTai = hinhCopy;
                }
                else
                {
                    HinhVe newHinhCopy = new HinhVe();
                    switch (hinhCopy.loaiHinh)
                    {
                        case 0: newHinhCopy = new Import(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 1: newHinhCopy = new DuongThang(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 2: newHinhCopy = new VanBan(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 3: newHinhCopy = new Elip(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 4: newHinhCopy = new HinhChuNhat(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 5: newHinhCopy = new TamGiac(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 6: newHinhCopy = new TamGiacVuong(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 7: newHinhCopy = new HinhThoi(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 8: newHinhCopy = new NguGiac(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 9: newHinhCopy = new Hexagon(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 10: newHinhCopy = new LeftArrow(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 11: newHinhCopy = new uparrow(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 12: newHinhCopy = new FourStar(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 13: newHinhCopy = new Star5(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 14: newHinhCopy = new SixStar(); copy(hinhCopy, newHinhCopy);
                            break;
                        case 15: newHinhCopy = new Pencil(); copy(hinhCopy, newHinhCopy);
                            break;
                        default: break;
                    }
                    newHinhCopy.diChuyen = true;
                    newHinhCopy.DiChuyen(-newHinhCopy.diemBatDau.X, -newHinhCopy.diemBatDau.Y);
                    newHinhCopy.Mouse_Up(null);
                    lHV.listHinh.Add(newHinhCopy);
                    panel1.Refresh();
                    newHinhCopy.VeKhung(panel1.CreateGraphics());
                    hinhHienTai = newHinhCopy;
                }
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteToolStripButton_Click(sender, e);
        }
        #endregion

        #region net ve
        private void sizeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            doDamNet = int.Parse(sizeToolStripComboBox.Text);
            lbSize.Text = doDamNet.ToString() + " pt";
            font = new Font(fontToolStripComboBox.Text, doDamNet, fontstyle, GraphicsUnit.Point, ((byte)(0)));
        }

        private void sizeToolStripComboBox_TextChanged(object sender, EventArgs e)
        {
            string s = sizeToolStripComboBox.Text;
            for (int i = 0; i < sizeToolStripComboBox.Text.Length; i++)
            {
                if (s[i] < '0' || s[i] > '9')
                {
                    return;
                }
                else
                {
                    doDamNet = int.Parse(sizeToolStripComboBox.Text);
                    font = new Font(fontToolStripComboBox.Text, doDamNet, fontstyle, GraphicsUnit.Point, ((byte)(0)));
                    lbSize.Text = doDamNet.ToString() + " pt";
                }
            }
        }
        #endregion

        #region laymau
        private void coloerpicker(Point x)
        {
            Bitmap tam = new Bitmap(hinhNenChim);
            mauVe = tam.GetPixel(x.X, x.Y);
            laymau = false;
            thecurrentcolorToolStripButton.BackColor = mauVe;
        }
        private void colorpickerToolStripButton2_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            tl.refreshtool(this);
            colorpickerToolStripButton2.Checked = true;
            laymau = true;
        }
        #endregion

        #region xuat thanh file pdf
        private void publishtoPDFToolStripButton_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            printDialog1.Document = pd;
            pd.Print();
        }

        private void pushlishtoPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            publishtoPDFToolStripButton_Click(sender, e);
        }
        #endregion

        #region export
        private void exportToolStripButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Export";
            saveFileDialog1.Filter = "PDF (*.pdf)|*.pdf|Bitmap file(*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|All File (*.*)|*.*";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.OverwritePrompt = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    hinhNenChim.Save(saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportToolStripButton_Click(sender, e);
        }
        #endregion

        #region import
        private void inportToolStripButton_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = false;
            copyToolStripButton.Enabled = false;
            if (IDhinhHienTai <= 0)
            {
                openFileDialog1.Title = "Import";
                openFileDialog1.Filter = "Bitmap file(*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|All File (*.*)|*.*";
                openFileDialog1.CheckFileExists = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        img = new Bitmap(openFileDialog1.FileName);
                        IDhinhHienTai = 0;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Error");
                    }
                }
            }
            else
                MessageBox.Show("There are some object is choosen", "Error");
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inportToolStripButton_Click(sender, e);
        }
        #endregion

        #region delete 
        private void deleteToolStripButton_Click_1(object sender, EventArgs e)
        {
            if (hinhHienTai != null && hinhHienTai.loaiHinh != -1)
            {
                //hinhCopy = hinhHienTai;
                lHV.XoaHinhCuoi();
                lHV.listHinh.Remove(hinhHienTai);
                panel1.Refresh();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteToolStripButton_Click_1(sender, e);
        }
        #endregion

        #region property
        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertyGrid1.Visible = false;
            hideToolStripMenuItem.Checked = true;
            showToolStripMenuItem.Checked = false;
            this.Width -= 185;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertyGrid1.Visible = true;
            showToolStripMenuItem.Checked = true;
            hideToolStripMenuItem.Checked = false;
            this.Width += 185;
        }
        #endregion

        #region change color
        private void fillcolorToolStripButton_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = false;
            boldToolStripButton.Enabled = false;
            italicToolStripButton.Enabled = false;
            underlineToolStripButton.Enabled = false;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            tl.refreshtool(this);
            fillcolorToolStripButton.Checked = true;
            if (hinhHienTai != null)
            {
                hinhHienTai.mauVe = mauVe;
            }
            else
                MessageBox.Show("You have to choose an object", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region add text
        private void textToolStripButton_Click(object sender, EventArgs e)
        {
            fontToolStripComboBox.Enabled = true;
            boldToolStripButton.Enabled = true;
            italicToolStripButton.Enabled = true;
            underlineToolStripButton.Enabled = true;
            cutToolStripButton.Enabled = true;
            copyToolStripButton.Enabled = true;
            IDhinhHienTai = 2;
            tl.refreshtool(this);
            textToolStripButton.Checked = true;
        }

        private void boldToolStripButton_Click(object sender, EventArgs e)
        {
            if (boldToolStripButton.Checked == false)
            {
                boldToolStripButton.Checked = true;
                if (underlineToolStripButton.Checked == true)
                    if (italicToolStripButton.Checked)
                        fontstyle = FontStyle.Bold | FontStyle.Italic | FontStyle.Underline;
                    else
                        fontstyle = FontStyle.Bold | FontStyle.Underline;
                else
                {
                    if (italicToolStripButton.Checked)
                        fontstyle = FontStyle.Italic | FontStyle.Bold;
                    else
                        fontstyle = FontStyle.Bold;
                }
            }
            else
            {
                boldToolStripButton.Checked = false;
                if (underlineToolStripButton.Checked == true)
                    if (italicToolStripButton.Checked)
                        fontstyle = FontStyle.Underline | FontStyle.Italic;
                    else
                        fontstyle = FontStyle.Underline;
                else
                    fontstyle = FontStyle.Regular;
            }
            font = new Font(fontToolStripComboBox.Text, int.Parse(sizeToolStripComboBox.Text), fontstyle, GraphicsUnit.Point, ((byte)(0)));
        }

        private void italicToolStripButton_Click(object sender, EventArgs e)
        {
            if (italicToolStripButton.Checked == false)
            {
                italicToolStripButton.Checked = true;
                if (boldToolStripButton.Checked == true)
                    if (underlineToolStripButton.Checked)
                        fontstyle = FontStyle.Bold | FontStyle.Italic | FontStyle.Underline;
                    else
                        fontstyle = FontStyle.Bold | FontStyle.Italic;
                else
                {
                    if (underlineToolStripButton.Checked)
                        fontstyle = FontStyle.Italic | FontStyle.Underline;
                    else
                        fontstyle = FontStyle.Italic;
                }
            }
            else
            {
                italicToolStripButton.Checked = false;
                if (boldToolStripButton.Checked == true)
                    if (italicToolStripButton.Checked)
                        fontstyle = FontStyle.Bold | FontStyle.Underline;
                    else
                        fontstyle = FontStyle.Bold;
                else
                    fontstyle = FontStyle.Regular;
            }
            font = new Font(fontToolStripComboBox.Text, int.Parse(sizeToolStripComboBox.Text), fontstyle, GraphicsUnit.Point, ((byte)(0)));
        }

        private void underlineToolStripButton_Click(object sender, EventArgs e)
        {
            if (underlineToolStripButton.Checked == false)
            {
                underlineToolStripButton.Checked = true;
                if (boldToolStripButton.Checked == true)
                    if (italicToolStripButton.Checked)
                        fontstyle = FontStyle.Bold | FontStyle.Italic | FontStyle.Underline;
                    else
                        fontstyle = FontStyle.Bold | FontStyle.Underline;
                else
                {
                    if (italicToolStripButton.Checked)
                        fontstyle = FontStyle.Italic | FontStyle.Underline;
                    else
                        fontstyle = FontStyle.Underline;
                }
            }
            else
            {
                underlineToolStripButton.Checked = false;
                if (boldToolStripButton.Checked == true)
                    if (italicToolStripButton.Checked)
                        fontstyle = FontStyle.Bold | FontStyle.Italic;
                    else
                        fontstyle = FontStyle.Bold;
                else
                    fontstyle = FontStyle.Regular;
            }
            font = new Font(fontToolStripComboBox.Text, int.Parse(sizeToolStripComboBox.Text), fontstyle, GraphicsUnit.Point, ((byte)(0)));
        }

        private void fontToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            font = new Font(fontToolStripComboBox.Text, int.Parse(sizeToolStripComboBox.Text), fontstyle, GraphicsUnit.Point, ((byte)(0)));
        }
        #endregion
    }
}
