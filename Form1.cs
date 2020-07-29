using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZelluSimFormz.WinFormsGUI;
using ZelluSim;
using ZelluSim.Misc;
using ZelluSim.SimulationTypes;

namespace ZelluSimFormz
{

    /// <summary>
    /// Our form for showing the cellular automaton simulations (e.g. "Conway's Game of Life").
    /// </summary>
    public partial class Form1 : Form
    {
        //state:

        //the configuration of WindowsForms UI elements
        protected GuiConfig conf;

        //1st dim = "X"
        //2nd dim = "Y"
        //(0,0) = "upper left corner"
        Button[,] buttons;

        //is simulation RUNNING or STOPPED?
        bool running = false;

        //this is the simulation we currently work with (also: sim.Settings)
        ICellSimulation sim;


        //c'tors:

        public Form1()
        {
            InitializeComponent();
        }



        //helper methods:

        protected GuiConfig Conf
        {
            get => conf;
            set 
            {
                conf = value;
                RebuildGui();
            }
        }

        protected void Form1_Load(object sender, EventArgs e)
        {
            //data and simulation delegate
            sim = new ClassicSimulation(new SimulationSettings());

            //establish a gui config
            conf = new GuiConfig(this);

            //create cell grafix
            Button[,] newButtons = CreateButtons(sim.Settings.SizeX, sim.Settings.SizeY);
            ConnectButtons(newButtons);
            ResizeButtons();//just make sure, font looks always the same

            //pull values into the UI elements (from 'conf' and 'sim')
            PullValuesFromModelToView();
        }

        public void RebuildGui()
        {
            //TODO
            ReformatGui();
        }

        public void ReformatGui()
        {
            //TODO
        }

        protected void PullValuesFromModelToView()
        {
            //TODO

            Num_BoardXY_Ignore = true;
            Num_BoardX.Value = cnumX;
            Num_BoardY.Value = cnumY;
            Num_BoardXY_Ignore = false;

            Num_SizeXY_Ignore = true;
            Num_SizeXY.Value = csize;
            Num_SizeXY_Ignore = false;

            Chk_Wrap_Ignore = true;
            Chk_Wrap.Checked = wrap;
            Chk_Wrap_Ignore = false;

            UpdateStatus();
        }

        protected void PullValuesFromConf()
        {
            if (running)
            {
                Lbl_Status.ForeColor = conf.RunningColor;
                Lbl_Status.Text = conf.RunningText;
            }
            else
            {
                Lbl_Status.ForeColor = conf.StoppedColor;
                Lbl_Status.Text = conf.StoppedText;
            }
        }

        protected void UpdateStatus()
        {

        }

        protected bool IsAlife(Button button)
        {
            for (int i = 0; i < cnumX; i++)
                for (int j = 0; j < cnumY; j++)
                    if (buttons[i, j] == button)
                        return IsAlife(i, j);
            throw new ArgumentException($"button with Text \"{button.Text}\" is not a cell!");
        }

        protected void SetAlife(Button button, bool alife = true)
        {
            button.Text = alife ? alifeText : "";
        }

        protected bool IsAlife(int x, int y)
        {
            while (x < 0)
                x += cnumX;
            while (y < 0)
                y += cnumY;
            if (x >= cnumX)
                x %= cnumX;
            if (y >= cnumY)
                y %= cnumY;
            return IsAlife(buttons[x, y]);
        }

        protected bool IsAlife(int x, int y, Direction dir)
        {
            int x2 = x, y2 = y;
            switch (dir)
            {
                case Direction.N: y2--; break;
                case Direction.NE: x2++; y2--; break;
                case Direction.E: x2++; break;
                case Direction.SE: x2++; y2++; break;
                case Direction.S: y2++; break;
                case Direction.SW: x2--; y2++; break;
                case Direction.W: x2--; break;
                case Direction.NW: x2--; y2--; break;
            }
            return IsAlife(x2, y2);
        }

        protected int GetNumAlife(int x, int y)
        {
            int num = 0;
            if (IsAlife(x, y, Direction.N)) num++;
            if (IsAlife(x, y, Direction.NE)) num++;
            if (IsAlife(x, y, Direction.E)) num++;
            if (IsAlife(x, y, Direction.SE)) num++;
            if (IsAlife(x, y, Direction.S)) num++;
            if (IsAlife(x, y, Direction.SW)) num++;
            if (IsAlife(x, y, Direction.W)) num++;
            if (IsAlife(x, y, Direction.NW)) num++;
            return num;
        }

        protected void EnsureOutputBuffer()
        {
            if (outputBuffer == null)
                outputBuffer = new bool[cnumX, cnumY];
        }

        protected void RunSimulation()
        {
            EnsureOutputBuffer();

            int neighbors;
            for(int x = 0; x < cnumX; x++)
                for(int y = 0; y < cnumY; y++)
                {
                    neighbors = GetNumAlife(x, y);
                    if (IsAlife(x, y))
                    {//cell is alive
                        if (neighbors == 2 || neighbors == 3)
                            outputBuffer[x, y] = true;
                        else
                            outputBuffer[x, y] = false;
                    }
                    else
                    {//cell is dead
                        if (neighbors == 3)
                            outputBuffer[x, y] = true;
                        else
                            outputBuffer[x, y] = false;
                    }
                }

            for(int x = 0; x < cnumX; x++)
                for(int y = 0; y < cnumY; y++)
                {
                    SetAlife(buttons[x, y], outputBuffer[x, y]);
                }
        }

        protected Button[,] CreateButtons(int numX, int numY)
        {
            Button[,] buttons = new Button[numX, numY];
            Button button;
            for (int x = 0; x < numX; x++)
                for (int y = 0; y < numY; y++)
                {
                    button = new Button();
                    buttons[x, y] = button;
                    button.Name = "button(" + x + "," + y + ")";

                    button.Size = new Size(csize, csize);
                    button.Location = new Point(
                        topLeftX + x * csize,
                        topLeftY + y * csize);
                    button.Margin = new Padding(0);
                    button.Padding = new Padding(0);

                    button.UseVisualStyleBackColor = true;
                }
            return buttons;
        }

        protected void ConnectButtons(Button[,] buttons)
        {
            this.buttons = buttons;
            foreach(Button button in buttons)
            {
                button.Click += new System.EventHandler(this.button1_Click);
                this.Controls.Add(button);
            }
        }

        protected void RemoveButtons()
        {
            foreach(Button button in buttons)
            {
                button.Click -= new System.EventHandler(this.button1_Click);
                this.Controls.Remove(button);
            }
        }

        protected void ResizeField(int newX, int newY)
        {
            if (buttons == null)
                return;

            //- create new button-Array
            Button[,] newButtons = CreateButtons(newX, newY);

            //- copy values from old button-Array
            int maxX = cnumX > newX ? newX : cnumX;
            int maxY = cnumY > newY ? newY : cnumY;
            for(int x = 0; x < maxX; x++)
                for(int y = 0; y < maxY; y++)
                    newButtons[x, y].Text = buttons[x, y].Text;

            //- remove old buttons
            RemoveButtons();

            //- update the size information HERE
            cnumX = newX;
            cnumY = newY;

            //- connect new buttons
            ConnectButtons(newButtons);

            //- replace the 'outputBuffer' by apropriate array
            outputBuffer = null;
            EnsureOutputBuffer();
        }

        protected void ResizeButtons()
        {
            int csize = conf.CellSize;

            Font oldFont = buttons[0, 0].Font;
            //float oldSize = oldFont.Size;
            float newSize = csize / 3f;
            Font newFont = new Font(
                    oldFont.FontFamily.Name,
                    newSize,
                    oldFont.Style
                );

            Button button;
            int cnumX = sim.Settings.SizeX, cnumY = sim.Settings.SizeY;
            int topLeftX = conf.TopLeftX, topLeftY = conf.TopLeftY;
            for (int x = 0; x < cnumX; x++)
                for (int y = 0; y < cnumY; y++)
                {
                    button = buttons[x, y];

                    button.Size = new Size(csize, csize);
                    button.Location = new Point(
                        topLeftX + x * csize,
                        topLeftY + y * csize);
                    button.Margin = new Padding(0);
                    button.Padding = new Padding(0);

                    button.Font = newFont;
                }
        }

        protected void button1_Click(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button button = (sender as Button);
                if (IsAlife(button))
                    SetAlife(button, false);
                else
                    SetAlife(button, true);
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            if(running)
                RunSimulation();
        }

        protected void Cmd_StartStop_Click(object sender, EventArgs e)
        {
            running = !running;
            UpdateStatus();
        }

        protected bool Num_SizeXY_Ignore = false;
        protected void Num_SizeXY_ValueChanged(object sender, EventArgs e)
        {
            if (Num_SizeXY_Ignore)
                return;
            ResizeButtons();
        }

        protected bool Num_BoardXY_Ignore = false;
        protected void Num_BoardXY_ValueChanged(object sender, EventArgs e)
        {
            if (Num_BoardXY_Ignore)
                return;
            int newX = (int)Num_BoardX.Value;
            int newY = (int)Num_BoardY.Value;
            ResizeField(newX, newY);
        }

        protected bool Chk_Wrap_Ignore = false;
        protected void Chk_Wrap_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_Wrap_Ignore)
                return;
            wrap = wrap ? false : true;
        }

        protected bool Txt_TextAlife_Ignore = false;
        protected void Txt_TextAlife_TextChanged(object sender, EventArgs e)
        {
            if (Txt_TextAlife_Ignore)
                return;
            ReformatField();
        }


        //UI elements (controls) without state

        protected void Cmd_Random_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int cnumX = sim.Settings.SizeX, cnumY = sim.Settings.SizeY;
            for (int x = 0; x < cnumX; x++)
                for (int y = 0; y < cnumY; y++)
                    SetAlife(buttons[x,y], rand.Next(0, 2) == 1 ? true : false);
        }

        protected void Cmd_Clear_Click(object sender, EventArgs e)
        {
            foreach (Button button in buttons)
                SetAlife(button, false);
        }

        protected void Cmd_Fill_Click(object sender, EventArgs e)
        {
            foreach (Button button in buttons)
                SetAlife(button, true);
        }

        protected void Cmd_Load_Click(object sender, EventArgs e)
        {

        }

        protected void Cmd_Save_Click(object sender, EventArgs e)
        {

        }

        protected void setAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        protected void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
