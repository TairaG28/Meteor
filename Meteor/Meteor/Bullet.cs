using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor
{
    class Bullet
    {
        public int x, y, w, h;
        public bool isSlating = false;  //発射パターンが斜めかどうか
        public int cPx, cPy;
        public int vX, vY; //斜め発射用の速度
        public double radAns; 
        public Image Image = Image.FromFile(@"..\..\Resources\p_bullet_p.png");


        public Bullet() { }

        public Bullet(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public Bullet(int x, int y, int w, int h, Image image) : this(x, y, w, h)
        {
            this.Image = image;
        }

        public Bullet(int x, int y, int w, int h, int vX, int vY, bool isSlating, double radAns, Image image) : this(x, y, w, h)
        {
            this.vX = vX;
            this.vY = vY;
            this.isSlating = isSlating;
            this.radAns = radAns;
            this.Image = image;
        }

    }
}
