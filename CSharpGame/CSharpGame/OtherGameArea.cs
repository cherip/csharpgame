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



            gameArea.Init(new Point(0, 12), new Size(areaSize.Width, areaSize.Height - 12));
            gameArea.BorderStyle = BorderStyle.None;
            //gameArea = new GameArea();

            this.Controls.Add(userName);
            this.Controls.Add(gameStatus);
            this.Controls.Add(gameArea);
        }



    }
}
