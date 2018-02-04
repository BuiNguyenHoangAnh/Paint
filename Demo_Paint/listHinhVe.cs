using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Demo_Paint
{

    class listHinhVe
    {
#region Thuộc tính
        public List<HinhVe> listHinh; // List các đối tượng        
#endregion

#region Khởi tạo
        public listHinhVe()
        {
            listHinh = new List<HinhVe>();
        }
#endregion

#region Phương thức
        // Vẽ
        public void Ve(Graphics g)
        {
            foreach (HinhVe hinhVe in listHinh)
            {
                hinhVe.Ve(g);
            }
        }

        // Xóa phần tử cuối
        public void XoaHinhCuoi()
        {
            try
            {
                for(int i=0;i<listHinh.Count;i++)
                    for(int j=i+1;j<listHinh.Count;j++)
                        if(listHinh[i].Equals(listHinh[j]))
                            listHinh.RemoveAt(j);


                listHinh.RemoveAt(listHinh.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning");
            }
        }

        public bool SaveListHinh(string fileName)
        {
            string[] s;
            s=fileName.Split('.');
            
            if (s[s.Length - 1] == "ABHK")  //xác định đuôi mở rộng
            {   
                try
                {
                    Stream stream = File.Open(fileName, FileMode.Create);
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    binFormatter.Serialize(stream, listHinh);
                    stream.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool OpenListHinh(string fileName)
        {
            string[] s;
            s=fileName.Split('.');
            
            if (s[s.Length - 1] == "ABHK")
            {
                try
                {
                    listHinh = null;
                    Stream stream = File.Open(fileName, FileMode.Open);
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    listHinh = (List<HinhVe>)binFormatter.Deserialize(stream);
                    stream.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show( ex.Message, "Error");
                    return false;
                }
            }
            return false;
        }
#endregion
    }
}
