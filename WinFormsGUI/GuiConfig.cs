using System.Drawing;
using System.Globalization;

namespace ZelluSimFormz.WinFormsGUI
{
    //TODO: Make class Serializable and do test-save and test-load into a .zsimf file.

    /// <summary>
    /// The configuration of the GUI is defined in this class. 
    /// Any changes to this configuration will either result in a 'RebuildGui' or 'ReformatGui' call on the Form1 form. 
    /// 'RebuildGui' will be necessary when the fundamental GUI elements have changed in size, position or quantity. 
    /// 'ReformatGui' will be enough in situations where only colors or texts have changed.
    /// </summary>
    public class GuiConfig
    {
        //state:

        protected bool reformatNeeded = false;
        protected bool rebuildNeeded = false;
        protected bool suppress = false;

        protected Form1 form = null;

        protected int cellSize = 23;
        protected int topLeftX = 3;
        protected int topLeftY = 25;
        
        protected string alifeText = "X";
        protected string deadText = "";
        protected string halfAlifeText = "+";
        protected Color alifeColor = Color.Green;
        //protected Color deadColor = Color.Transparent;
        protected Color deadColor = Color.Red;
        protected Color halfAlifeColor = Color.Yellow;
        protected bool interpolateColors = true;

        protected string runningText = "RUNNING";
        protected string stoppedText = "STOPPED";
        protected Color runningColor = Color.DarkGreen;
        protected Color stoppedColor = Color.DarkRed;

        protected string generationText = "generation: {0,0.0}";
        protected CultureInfo generationTextCulture = CultureInfo.InvariantCulture;
        protected Color generationTextColor = Color.DarkBlue;


        //c'tors:

        /// <summary>
        /// The c'tor variant 1: The GuiConfig is not connected (the Form is null). 
        /// As soon as you set the Form (property 'Form') the GuiConfig will try to 
        /// connect with the Form.
        /// </summary>
        public GuiConfig()
        {

        }

        /// <summary>
        /// The c'tor variant 2: Connects the form right now and tries to rebuild the UI.
        /// </summary>
        /// <param name="form">The Form that you want to connect to.</param>
        public GuiConfig(Form1 form)
        {
            Ctor1Action(form);
        }
        protected void Ctor1Action(Form1 form) { Form = form; }


        //helper methods:

        protected void TryRebuild()
        {
            rebuildNeeded = true;
            if (form == null)
                return;
            if (suppress)
                return;
            form.RebuildGui(); //TODO: use proper C# event 'RequestRebuild'
            rebuildNeeded = false;
            reformatNeeded = false;
        }

        protected void TryReformat()
        {
            reformatNeeded = true;
            if (form == null)
                return;
            if (suppress)
                return;
            form.ReformatGui(); //TODO: use proper C# event 'RequestReformat'
            reformatNeeded = false;
        }


        //public methods:

        /// <summary>
        /// Allows to temporarily suppress any GUI rebuilds and GUI reformats.<br></br>
        /// If you want to mass-change properties of the GuiConfig do as follows:<br></br>
        /// <br></br>
        /// <list type="number">
        /// <item>call 'SuppressRebuildReformat=true'</item>
        /// <item>change as many properties as you like</item>
        /// <item>call 'SuppressRebuildReformat=false'</item>
        /// </list>
        /// <br></br>
        /// Alternatively you can follow this procedure:
        /// <list type="number">
        /// <item>call 'Form=null'</item>
        /// <item>change as many properties as you like</item>
        /// <item>call 'Form=yourForm'</item>
        /// </list>
        /// </summary>
        public bool SuppressRebuildReformat
        {
            get => suppress;
            set
            {
                suppress = value;
                if (suppress == false)
                {
                    if (rebuildNeeded)
                        TryRebuild();
                    else
                    if (reformatNeeded)
                        TryReformat();
                }
            }
        }

        /// <summary>
        /// The form on which we will call 'RebuildGui' or 'ReformatGui'.
        /// </summary>
        public Form1 Form
        {
            get => form;
            set { form = value; TryRebuild(); }
        }

        /// <summary>
        /// The size of the cells (width and height) in pixels.
        /// </summary>
        public int CellSize 
        { 
            get => cellSize;
            set { cellSize = value; TryRebuild(); }
        }

        /// <summary>
        /// The x coordinate of the top-left position of our cells field.
        /// </summary>
        public int TopLeftX
        { 
            get => topLeftX;
            set { topLeftX = value; TryRebuild(); }
        }

        /// <summary>
        /// The y coordinate of the top-left position of our cells field.
        /// </summary>
        public int TopLeftY
        {
            get => topLeftY;
            set { topLeftY = value; TryRebuild(); }
        }

        /// <summary>
        /// We display this text, when cell is alife (more than 0% life).
        /// </summary>
        public string AlifeText 
        { 
            get => alifeText;
            set { alifeText = value; TryReformat(); }
        }

        /// <summary>
        /// We display this text, when cell is dead (at 0% life).
        /// </summary>
        public string DeadText
        {
            get => deadText;
            set { deadText = value; TryReformat(); }
        }

        /// <summary>
        /// We display this text, when cell is 50% alife / 50% dead.
        /// </summary>
        public string HalfAlifeText
        {
            get => halfAlifeText;
            set { halfAlifeText = value; TryReformat(); }
        }

        /// <summary>
        /// We display this color, when cell is (100%) alife.
        /// </summary>
        public Color AlifeColor
        {
            get => alifeColor;
            set { alifeColor = value; TryReformat(); }
        }

        /// <summary>
        /// We display this color, when cell is (100%) dead.
        /// </summary>
        public Color DeadColor
        {
            get => deadColor;
            set { deadColor = value; TryReformat(); }
        }

        /// <summary>
        /// We display this color, when cell is 50% alife / 50% dead.
        /// </summary>
        public Color HalfAlifeColor
        {
            get => halfAlifeColor;
            set { halfAlifeColor = value; TryReformat(); }
        }

        /// <summary>
        /// We may either interpolate the colors or not interpolate the colors.
        /// </summary>
        public bool InterpolateColors
        {
            get => interpolateColors;
            set { interpolateColors = value; TryReformat(); }
        }

        /// <summary>
        /// We display this text, when the simulation is running.
        /// </summary>
        public string RunningText
        {
            get => runningText;
            set { runningText = value; TryReformat(); }
        }

        /// <summary>
        /// We display this text, when the simulation is stopped.
        /// </summary>
        public string StoppedText
        {
            get => stoppedText;
            set { stoppedText = value; TryReformat(); }
        }

        /// <summary>
        /// We display this color, when the simulation is running.
        /// </summary>
        public Color RunningColor
        {
            get => runningColor;
            set { runningColor = value; TryReformat(); }
        }

        /// <summary>
        /// We display this color, when the simulation is stopped.
        /// </summary>
        public Color StoppedColor
        {
            get => stoppedColor;
            set { stoppedColor = value; TryReformat(); }
        }

        /// <summary>
        /// This formatting string will be used to display the information "which generation do we currently have?". 
        /// The string will be used with 'String.Format()'. It is expected to receive one value of type integer.
        /// </summary>
        public string GenerationText
        {
            get => generationText;
            set { generationText = value; TryReformat(); }
        }

        /// <summary>
        /// More information about the formatting string. In some cultures the decimal separator is a point, not a 
        /// comma (as in US English). So, in an English UI the number 12334521.7 will be shown as "12,334,521.7", but 
        /// in a German UI the number will be shown as "12.334.521,7".
        /// </summary>
        public CultureInfo GenerationTextCulture
        {
            get => generationTextCulture;
            set { generationTextCulture = value; TryReformat(); }
        }

        /// <summary>
        /// We display this color, when showing the current generation number.
        /// </summary>
        public Color GenerationTextColor
        {
            get => generationTextColor;
            set { generationTextColor = value; TryReformat(); }
        }
    }
}
