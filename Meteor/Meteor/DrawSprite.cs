using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor
{
    class DrawSprite
    {
        public Image image;

        int x, y;
        int w, h;
        public int currentTime;


        public DrawSprite(Image image)
        {

            this.image = image;
            this.w = image.Width;
            this.h = image.Height;
            this.currentTime = 0;

        }

        public DrawSprite(int w, int h, Image image)
        {

            this.image = image;
            this.w = w;
            this.h = h;
            this.currentTime = 0;

        }

        public Graphics timeDrawA(int x, int y, int endTime, Graphics gg)
        {
            /*Console.WriteLine(this.currentTime);*/
            if (this.currentTime < endTime)
            {
                if (this.currentTime % 15 > 5)
                {
                    gg.DrawImage(image, new Rectangle(x - (w / 2), y - (h / 2), w, h));
                }
                this.currentTime++;
            }

            return gg;
        }

        public Graphics timeDrawB(int x, int y, int endTime, Graphics gg)
        {
            /*Console.WriteLine(this.currentTime);*/
            if (this.currentTime < endTime)
            {
                if (this.currentTime % 15 > 5)
                {
                    gg.DrawImage(image, new Rectangle(x - (w / 2), y - (h / 2), w, h));
                }
                this.currentTime++;
            }

            return gg;
        }

        public Graphics timeDrawC(int x, int y, int currentTime, Graphics gg)
        {
            /*  Console.WriteLine(this.currentTime);*/
            if (currentTime <70)
            {
                gg.DrawImage(
                    image,
                    new Rectangle(
                        x - (w + currentTime ) / 2,
                        y - (h + currentTime ) / 2,
                        w + currentTime,
                        h + currentTime
                        ));
            }

            return gg;
        }

    }

}
