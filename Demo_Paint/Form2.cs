using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Paint
{
    public partial class Facebook : Form
    {
        public Facebook()
        {
            InitializeComponent();
            webBrowser1.Navigate("https://www.facebook.com/me?");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
        }
    }
}
