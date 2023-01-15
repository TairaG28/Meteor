using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor
{
    class Missile : Enemy
    {
        public Missile() : base()
        { }

        public Missile(int x, int y) : base(x, y)
        {
            this.Image = Image.FromFile(@"..\..\Resources\p_enemy.png");
            this.hp = 1;
            this.RR =10;
            this.w = 20;
            this.h = 20;
        }


    }
}
