using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;


namespace Metro
{
    public partial class Form1 : Form
    {
        public const int sizeTable = 14;
        public const int STATE_RIGHT = 1, STATE_LEFT = -1, STATE_TOP = -14, STATE_BOTTOM = 14;
        private const int labelSize = 196;
        private const int lettersSize = 32;
        private const int sizeListPerson = 99;

        private string[] letters= {"А","Б","В","Г","Д","Е","Є","Ж","З","И","І","Ї","Й","К","Л","М","Н","О","П","Р","С","Т","У","Ф","Х","Ц","Ч","Ш","Щ","Ь","Я","Ю"};
        private String[] name;

        private Dictionary<char, Bitmap> mapImage;
        private Dictionary<int, string> mapName;
        static public List<Label> listLabel { get;set;}
        private Random random;

        private GlobalMouseHandler sr;
        private Dictionary<int, string> nameImageToLabel;

        private int prevInd = -1;
        private int current;
        private ResourceManager rm;

        private Search search;
        private Stack<char> stForLetter;
        private Stack<int> stForInd;
        private int prevState = 0;
        private bool first = true;
        private string textLabel = "";
        private bool[] win;

        private for_help fNameH, lNameH;

        private bool block;

        public Form1()
        {
            InitializeComponent();
            random = new Random();

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, pictureBox.Width - 3, pictureBox.Height - 3);
            Region rg = new Region(gp);
            pictureBox.Region = rg;


            listLabel = new List<Label>(labelSize);
            mapImage = new Dictionary<char, Bitmap>(lettersSize);
            nameImageToLabel = new Dictionary<int, string>();


            sr = new GlobalMouseHandler();
            sr.TheMouseMoved += new MouseMovedEvent(mouseMove);

            search = new Search();
            stForLetter = new Stack<char>();
            stForInd = new Stack<int>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < labelSize; i++)
            {
                 String name = "label" + i;
                 Label label = this.Controls.Find(name, true).FirstOrDefault() as Label;

                 if (label != null)
                 {
                     listLabel.Add(label);
                     label.MouseDown += new MouseEventHandler(mouseDown);
                     label.MouseUp += new MouseEventHandler(mouseUp);
                 }
                 else
                 {
                     Console.WriteLine("Not load all label  " + name);
                 }
            }

            rm = Metro.Properties.Resources.ResourceManager;


            for (int i = 0; i < lettersSize; i++)
            {
                string name = letters[i];

                Bitmap bmap = (Bitmap)rm.GetObject(letters[i]);

                if (bmap != null)
                {
                    char key = letters[i].ToCharArray()[0];

                    mapImage.Add(key, bmap);
                }
            }

            int lbSize = listLabel.Count;
            int imSize = mapImage.Count;

            if (lbSize != labelSize)
            {
                Console.WriteLine("Not load all label");
                return;
            }

            if (imSize != lettersSize)
            {
                Console.WriteLine("Not load all image");
                return;
            }

            ReadFile rf = new ReadFile();
            mapName = rf.getData(Metro.Properties.Resources.list_name);
        }

        private void button1_Click1(object sender, EventArgs e)
        {
            block = false;
            win = new bool[2];

            nameImageToLabel.Clear();
            labelResult.Text = "";
            textLabel = "";

            int numbName = random.Next(sizeListPerson);

            for (int i = 0; i < labelSize; i++)
            {
                string letter = letters[random.Next(lettersSize)];
                listLabel[i].Image = (Image) rm.GetObject(letter);

                nameImageToLabel.Add(i, letter);
            }

            string nam = "_" + numbName;
            pictureBox.Image = (Image)rm.GetObject(nam);
         
            name = mapName[numbName].Split(' ');

            Logic logic = new Logic(name, mapImage, nameImageToLabel);
            logic.rearrangeLName();

            fNameH = logic.getFName();
            lNameH = logic.getLName();

        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (block)
                return;

            current = getIndex(((Label)sender).Name);

            Application.AddMessageFilter(sr);
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            Application.RemoveMessageFilter(sr);

            repaint();
        }

        private void mouseMove()
        {
            int ind = search.calculate(tableLayoutPanel, label0, Cursor.Position, this.Location);

            if(ind >= labelSize || ind < 0)
            {
                repaint();
                return;
            }

            if (prevInd == -1)
                prevInd = ind;

            int state = ind - prevInd;

            Bitmap btm;
            char key;
            string nameLabel;
            bool add = true;

            if (stForLetter.Count == 1)
            {
                prevState = state;
            }

            switch (state)
            {
                case STATE_RIGHT:
                    if (prevState == STATE_BOTTOM || prevState == STATE_TOP)
                        repaint();

                    if (prevState == STATE_LEFT && stForLetter.Count > 0 )
                    {
                        key = stForLetter.Pop();

                        int tmp_ind = stForInd.Pop();

                        listLabel[tmp_ind].Image = null;
                        listLabel[tmp_ind].Image = mapImage[key];
                        add = false;
                    }
                    break;
                case STATE_LEFT:
                    if (prevState == STATE_BOTTOM || prevState == STATE_TOP)
                        repaint();

                    if (prevState == STATE_RIGHT && stForLetter.Count > 0)
                    {

                        key = stForLetter.Pop();

                        int tmp_ind = stForInd.Pop();

                        listLabel[tmp_ind].Image = null;

                        listLabel[tmp_ind].Image = mapImage[key];
                        add = false;
                    }
                    break;
                case STATE_BOTTOM:
                    if (prevState == STATE_LEFT || prevState == STATE_RIGHT)
                        repaint();

                    if (prevState == STATE_TOP && stForLetter.Count > 0)
                    {
                        key = stForLetter.Pop();

                        int tmp_ind = stForInd.Pop();

                        listLabel[tmp_ind].Image = null;
                        listLabel[tmp_ind].Image = mapImage[key];
                        add = false;
                    }
                    break;
                case STATE_TOP:
                    if (prevState == STATE_LEFT || prevState == STATE_RIGHT)
                        repaint();

                    if (prevState == STATE_BOTTOM && stForLetter.Count > 0)
                    {
                        key = stForLetter.Pop();

                        int tmp_ind = stForInd.Pop();

                        listLabel[tmp_ind].Image = null;
                        listLabel[tmp_ind].Image = mapImage[key];
                        add = false;
                    }
                    break;
                case 0:
                    add = false;
                    break;
                default:
                    repaint();
                    break;
            }

            key = nameImageToLabel[ind].ToArray()[0];

            if (add || first)
            {
                stForInd.Push(ind);
                stForLetter.Push(key);

                first = false;
            }

            btm = mapImage[key];
            btm = grayScale(btm);

            nameLabel = "label" + ind;

            listLabel[ind].Image = null;
            listLabel[ind].Image = btm;

            char[] str_letter = stForLetter.ToArray();
            Array.Reverse(str_letter);

            String str = new String(str_letter);
            labelResult.Text = textLabel + str;

            prevInd = ind;
        }

        public int getIndex(string name)
        {
            return Int32.Parse(new String(name.Where(Char.IsDigit).ToArray()));
        }

        public Bitmap grayScale(Bitmap bitmap){
            Bitmap tmp_btm = (Bitmap)bitmap.Clone();
            int x, y;

            for (x = 0; x < bitmap.Width; x++)
            {
                for (y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.R, 50, 50);
                    tmp_btm.SetPixel(x, y, newColor); 
                }
            }

            return tmp_btm;
        }

        public void repaint()
        {
            int size = stForInd.Count;

            char[] c = stForLetter.ToArray();
            Array.Reverse(c);
            string str = new string(c);
            bool[] b = new bool[2];

            if ((b[0] = str.Equals(name[0].ToUpper())) || (b[1] = str.Equals(name[1].ToUpper())))
            {
                if (b[0] && ! win[0])
                {
                    win[0] = b[0];
                    if (textLabel.Equals(""))
                        textLabel += (str + " ");
                    else
                        textLabel = str + " " + textLabel;
                }

                if (b[1] && !win[1])
                {
                    textLabel += str;
                    win[1] = b[1];
                }

            }

           
            if (stForInd.Count > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    int ind = stForInd.Pop();
                    string key = nameImageToLabel[ind];

                    listLabel[ind].Image = mapImage[key.ToCharArray()[0]];
                }
            }
            

            labelResult.Text = textLabel;
            stForLetter.Clear();

            if (win[0] && win[1])
                block = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelStart.Visible = false;
            button1_Click1(null, null);
        }

        private void button_help(object sender, EventArgs e)
        {
            if (win[0] == false)
            {
                int length = name[0].Length;

                int pos_letter = random.Next(length);
                if (fNameH.mode == Logic.ROW_MODE)
                    pos_letter += fNameH.position;
                else
                    pos_letter = fNameH.position + pos_letter * sizeTable;

                string key = nameImageToLabel[pos_letter];

                Bitmap btm = mapImage[key.ToCharArray()[0]];
                listLabel[pos_letter].Image = grayScale(btm);
            }

            if (win[1] == false)
            {
                int length = name[1].Length;

                int pos_letter = random.Next(length);

                if (lNameH.mode == Logic.ROW_MODE)
                    pos_letter += lNameH.position;
                else
                    pos_letter = lNameH.position + pos_letter * sizeTable;

                string key = nameImageToLabel[pos_letter];

                Bitmap btm = mapImage[key.ToCharArray()[0]];
                listLabel[pos_letter].Image = grayScale(btm);
            }
        }
    }
}
