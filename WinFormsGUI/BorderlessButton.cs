using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZelluSimFormz.WinFormsGUI
{
    public class BorderlessButton : Button
    {
        public int buttomnumber;
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent); //TODO: is this needed?
            pevent.Graphics.DrawRectangle(new Pen(BackColor, 8), ClientRectangle);
        }
    }
}
