using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor
{
    class ChngOpcty
    {
        public ChngOpcty()
        {

        }

        public Image CreateTranslucentImage(Image img, float alpha)
        {
            //半透明の画像の描画先となるImageオブジェクトを作成
            Bitmap transImg = new Bitmap(img.Width, img.Height);
            //transImgのGraphicsオブジェクトを取得
            Graphics g = Graphics.FromImage(transImg);

            //imgを半透明にしてtransImgに描画
            System.Drawing.Imaging.ColorMatrix cm =
                new System.Drawing.Imaging.ColorMatrix();

            cm.Matrix00 = 1;
            cm.Matrix11 = 1;
            cm.Matrix22 = 1;
            cm.Matrix33 = alpha;
            cm.Matrix44 = 1;

            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();

            ia.SetColorMatrix(cm);
            g.DrawImage(img,
                new Rectangle(0, 0, img.Width, img.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

            //リソースを解放する
            g.Dispose();
            return transImg;
        }



    }
}
