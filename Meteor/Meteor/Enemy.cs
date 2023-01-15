using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor
{
    class Enemy
    {
      
        public int x, y,w,h;        //位置
        public int hp;      //耐久値、攻撃力
        public int RR;          //半径サイズ
        public Image Image = Image.FromFile(@"..\..\Resources\p_enemy.png");
        public int xVelocity = -5;
        public int yVelocity = -5;
        public int cPx, cPy;

        public Enemy()
        {
            xVelocity = -5;
            yVelocity = -5;
        }
    

        public Enemy(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public Enemy(int RR, int hp, int x, int y) : this(x, y)
        {
            this.RR = RR;
            this.hp = hp;

        }

        public Enemy(int x, int y, int w, int h, int hp) : this(x, y)
        {
            this.w = w;
            this.h = h;
            this.hp = hp;

        }
    }
}



