using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZelluSimFormz.WinFormsGUI
{
    class BorderlessButton2 : Button
    {
        public int buttomnumber; //TODO: what's this for?
        protected override void OnPaint(PaintEventArgs pevent)
        {
            //base.OnPaint(pevent); //TODO: is this needed?
            pevent.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
        }
    }
}
