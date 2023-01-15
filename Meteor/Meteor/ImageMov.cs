using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor
{
    class ImageMov
    {

        public Image image;

        int x, y;
        int w, h;
        FrameDimension frmdimsn;
        int animeFrame;

        public ImageMov(Image image)
        {

            this.image = image;
            this.w = image.Width;
            this.h = image.Height;
            this.frmdimsn = new FrameDimension(image.FrameDimensionsList[0]);

        }

        public Image moveFrame()
        {
            if (this.animeFrame > this.image.GetFrameCount(this.frmdimsn) - 1) { this.animeFrame = 0; }
            this.image.SelectActiveFrame(this.frmdimsn, this.animeFrame);
            animeFrame++;
            return this.image;
        }

        public Image removeFrame()
        {
            
            if (this.animeFrame == 0)
            {
                this.animeFrame = this.image.GetFrameCount(this.frmdimsn) - 1;
            }

            this.image.SelectActiveFrame(this.frmdimsn, this.animeFrame);
            animeFrame--;
            return this.image;
        }

        public Image stopFrame()
        {
            return this.image;
        }







    }
}
