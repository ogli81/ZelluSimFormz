using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ZelluSimFormz.WinFormsGUI
{
    public class GridButton : Button
    {
        //state:

        public int X { get; }
        public int Y { get; }


        //c'tors:

        //first c'tor: no checks
        public GridButton(int x, int y)
        {
            X = x;
            Y = y;
        }

        //second c'tor: some checks
        public GridButton(int x, int y, int buttonsX, int buttonsY)
        {
#if DEBUG
            if (x < 0) throw new Exception("x of button can't be less than zero!");
            if (y < 0) throw new Exception("y of button can't be less than zero!");
            if (x > buttonsX - 1) throw new Exception("x of button too high!");
            if (y > buttonsY - 1) throw new Exception("y of button too high!");
#endif
            X = x;
            Y = y;
        }

        //third c'tor: some more checks
        public GridButton(int x, int y, int buttonsX, int buttonsY, Dictionary<(int,int),GridButton> buttons)
        {
#if DEBUG
            if (x < 0) throw new Exception("x of button can't be less than zero!");
            if (y < 0) throw new Exception("y of button can't be less than zero!");
            if (x > buttonsX - 1) throw new Exception("x of button too high!");
            if (y > buttonsY - 1) throw new Exception("y of button too high!");
            GridButton val;
            if (buttons.TryGetValue((x, y), out val))
                throw new Exception("already contains a button with that (x,y)!");
#endif
            X = x;
            Y = y;
            buttons.Add((x, y), this);
        }
    }
}
