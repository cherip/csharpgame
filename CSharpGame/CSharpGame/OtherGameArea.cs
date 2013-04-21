using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace CSharpGame
{
    class OtherGameArea : Panel
    {
        public GameArea gameArea;
        Label userName;
        Label gameStatus;

        public OtherGameArea()
        {
            gameArea = new GameArea();
        }

        public OtherGameArea(Point areaLocat, Size areaSize) {
            Init(areaLocat, areaSize);
        }

        public void Init(Point areaLocat, Size areaSize)
        {
            this.Location = areaLocat;
            this.Size = areaSize;

            userName = createLable(new Point(0, 0), new Size(80, 10), "userxxx");
            gameStatus = createLable(new Point(85, 0), new Size(80, 10), "64/64");

            gameArea.Init(new Point(0, 12), new Size(areaSize.Width, areaSize.Height - 12));
            //gameArea = new GameArea();

            this.Controls.Add(userName);
            this.Controls.Add(gameStatus);
            this.Controls.Add(gameArea);
        }

        private Label createLable(Point p, Size s, string txt)
        {
            Label lbl = new Label();
            lbl.Location = p;
            lbl.Size = s;
            lbl.Text = txt;

            return lbl;
        }

        private void UpdateStatus() 
        { 
        }

        public void CleanButton(int a, int b)
        {
            gameArea.CleanBtnPair(a, b);
            UpdateStatus();
        }
    }
}
