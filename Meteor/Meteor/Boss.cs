using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Meteor
{
    class Boss : Enemy
    {



        public Boss() : base()
        {        }

        public Boss(int x, int y) : base(x, y)
        {
            this.RR = 125;
            this.hp = 50;
            this.Image = Image.FromFile(@"..\..\Resources\p_boss.png");

        }

    }
}
