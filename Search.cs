using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metro
{
    class Search
    {
        public int calculate(TableLayoutPanel panel, Label label, Point point, Point loc)
        {
            int number = -1;

            int panelWidth = panel.Width;
            int panelHeight = panel.Height;

            int posPanelx1 = panel.Location.X;
            int posPanely1 = panel.Location.Y;

            int colWidth = panel.GetColumnWidths()[0];
            int rowWidth = panel.GetRowHeights()[0];

            int numb = panel.GetColumnWidths().Length;

            int x = point.X - loc.X - posPanelx1, y = point.Y - loc.Y - posPanely1;

            if (x >= panelWidth || y >= panelHeight)
            {
                Console.WriteLine(">");
                return -1;
            }

            number = Convert.ToInt32((y / rowWidth - 1) * numb + x / colWidth) ;
            if (number < 0)
                number = 0;

            return number;
        }
    }
}
