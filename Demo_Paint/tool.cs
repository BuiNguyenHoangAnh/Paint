using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
namespace Demo_Paint
{
    class tool
    {
        #region biến toàn cục
        #endregion
        public tool() { }

        #region cac tool vẽ
        public void refreshtool(Form1 form)
        {
            form.picktoolToolStripButton.Checked = false;
            form.lineToolStripMenuItem.Checked = false;
            form.ovalToolStripMenuItem.Checked = false;
            form.rectangleToolStripMenuItem.Checked = false;
            form.triangleToolStripMenuItem.Checked = false;
            form.rightTriangleToolStripMenuItem.Checked = false;
            form.diamondToolStripMenuItem.Checked = false;
            form.pentegonToolStripMenuItem1.Checked = false;
            form.hexagonToolStripMenuItem1.Checked = false;
            form.upArrowToolStripMenuItem.Checked = false;
            form.rightArrowToolStripMenuItem1.Checked = false;
            form.fourpointStarToolStripMenuItem.Checked = false;
            form.fivepointStarToolStripMenuItem.Checked = false;
            form.sixpointStarToolStripMenuItem.Checked = false;
            form.inportToolStripButton.Checked = false;
            form.colorpickerToolStripButton2.Checked = false;
            form.textToolStripButton.Checked = false;
            form.toolStripButton1.Checked = false;
            form.fillcolorToolStripButton.Checked = false;
        }
        #endregion
    }
}
