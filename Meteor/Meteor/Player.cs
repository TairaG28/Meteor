using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor
{
    class Player
    {

		public int x, y;
		public int W, H;
        public int hp;
		public Image Image = Image.FromFile(@"..\..\Resources\p_player.png");

		public Player() { }

		public Player(int W, int H, int hp)
		{
			this.W = W;
			this.H = H;
			this.hp = hp;
			

		}

		public Player(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
}
