using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
/*using System.Windows.Input;*/
using Microsoft.DirectX.AudioVideoPlayback;

namespace Meteor
{
    public partial class Form1 : Form
    {
        //メンバ変数
        #region MemberVariable



        //キャンバスの用意
        static Bitmap canvas = new Bitmap(480, 700);
        Graphics gg = Graphics.FromImage(canvas);

        //GIFアニメ設定　バーニア
        Image animatedImage = Image.FromFile(@"..\..\Resources\p_vernier_a.gif");
        FrameDimension fd;
        int animeFrame;

        //ミッション失敗と成功の画像
        Image failedImageText = Image.FromFile(@"..\..\Resources\p_failed.png");
        Image successImageText = Image.FromFile(@"..\..\Resources\p_clear.png");


        //誘導弾・発射位置
        double missileX;
        double missileY;

        //左上
        float tx;
        float ty;
        //右上
        float tx1;
        float ty1;
        //左下
        float tx2;
        float ty2;
        //ミサイルの傾き角度
        double angle;
        //ミサイルの傾き角度のラジアン値
        double radStr;
        int misilCnt;

        //サウンドプレイヤー 
        List<Audio> sndList = new List<Audio>();

        //プレイヤー枠
        Player player;
        System.Drawing.Point Cpos; //カーソル座標

        ChngOpcty cO = new ChngOpcty();


        //ランダム生成
        Random rand = new Random();

        bool bossFlg; //ボスフラグ
        bool bossInChk;　//ボス導入チェック

        int bossIntvl; //ボス登場時間
        int radomIntvl; //ボス変速
        int randAdd;
        int randAddZako;

        bool hitFlg; //true:当たった
        int ecnt, ex, ey; //爆発演出用
        int expSize;　//爆発倍率
        long msgcnt; //メッセージ用カウンタ

        bool titleFlg; //true：タイトル表示中
        bool clearFlg; //クリアフラグ
        bool gameFlg; //ゲーム中か否か
        int gameTime;

        bool feedFlgTforG;
        bool feedFlgRforT;
        int feedIntv;
        const int feedIntvTime = 1000;

        long score; //スコア

        //PrivateFontCollectionオブジェクトを作成する
        System.Drawing.Text.PrivateFontCollection pfc =
           new System.Drawing.Text.PrivateFontCollection();

        /*Font myFont = new Font("Arial", 16);*/
        Font myFont;

        bool mutekiFlg;

        List<Bullet> bullets_player = new List<Bullet>();
        List<Bullet> bullets_enemy = new List<Bullet>();
        List<Enemy> enemys = new List<Enemy>();
        List<Explosion> explosions = new List<Explosion>();

        ImageMov bgMov = new ImageMov(Image.FromFile(@"..\..\Resources\p_BG_anime.gif"));

        Image currentImage = Image.FromFile(@"..\..\Resources\p_BG_change_blk.png");
        int currentAlphaPercent;

        DrawSprite msgStart;
        DrawSprite msgWarning;
        DrawSprite drawExplosion;


        #endregion

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            /*            pMeteor.Hide();
                        pPlayer.Hide();
                        pBG.Hide();
                        pExp.Hide();
                        pGameover.Hide();
                        pMsg.Hide();
                        pTitle.Hide();*/


            //音源・読み込み
            #region AudioAll
            sndList.Add(new Audio(@"..\..\Resources\wav\Runway.wav"));  //０、タイトルBGM
            sndList.Add(new Audio(@"..\..\Resources\wav\Hyper_Chase.wav"));    //１、ゲームBGM
            sndList.Add(new Audio(@"..\..\Resources\wav\Crouching_Noise.wav"));    //２、ボスBGM
            sndList.Add(new Audio(@"..\..\Resources\wav\ジングル素材08.wav"));    //３、ゲームクリア・ジングル
            sndList.Add(new Audio(@"..\..\Resources\wav\クラッシュタイム.wav"));    //４、ゲームオーバー・ジングル
            sndList.Add(new Audio(@"..\..\Resources\wav\Stairs_to_the_future.wav"));    //５、リザルトBGM
            sndList.Add(new Audio(@"..\..\Resources\wav\短いビーム音.wav"));    //６、選択音
            sndList.Add(new Audio(@"..\..\Resources\wav\ビームライフル_cstm.wav"));    //７、発射音
            sndList.Add(new Audio(@"..\..\Resources\wav\ゴージャスエクスプロージョン.wav"));    //８、撃墜音
            sndList.Add(new Audio(@"..\..\Resources\wav\レーザービームのような場面転換.wav"));    //９、被撃墜音
            #endregion

            initGame();
            mutekiFlg = false;
            feedFlgTforG = false;
            feedFlgRforT = false;
            fd = new FrameDimension(animatedImage.FrameDimensionsList[0]);
            animatedImage.SelectActiveFrame(fd, 0);	//ここで1コマ目を取得したつもり

        }

        //ゲーム開始時の初期設定
        private void initGame()
        {
            //プレイヤー召喚
            player = new Player(40, 50, 1);

            //敵の召喚(10体)
            enemys.Clear();
            for (int i = 0; i < 10; i++)
            {
                enemys.Add(
                    new Enemy(
                        70 / 2,                           //敵の半径
                        3,                              //敵のHP
                        rand.Next(1, 450),              //敵の初期位置X
                        rand.Next(1, 900) - 1000        //敵の初期位置Y
                    )
                );

            }



            //敵位置の重複チェック
            for (int i = 0; i < enemys.Count; i++)
            {
                enemyPositionCheck(enemys[i], 0);

            }

            hitFlg = false; //false:当たっていない
            ecnt = 40; //爆発の初めの処理で位置を変更する
            msgcnt = 0;

            titleFlg = true; //true：タイトル表示中

            gameFlg = true;
            gameTime = 0;
            animeFrame = 0;

            bossFlg = false;
            bossInChk = false;
            bossIntvl = 0;
            expSize = 1;

            clearFlg = false;
            currentAlphaPercent = 100;

            msgStart = new DrawSprite(350, 60, Image.FromFile(@"..\..\Resources\p_start.png"));
            msgWarning = new DrawSprite(480, 70, Image.FromFile(@"..\..\Resources\p_WARNING.png"));
            drawExplosion = new DrawSprite(50, 50, Image.FromFile(@"..\..\Resources\p_explosion.png"));

            score = 0;
            /* bullets_enemy.Add(new Bullet(240, 300));*/

            //PrivateFontCollectionにフォントを追加する
            pfc.AddFontFile(@"..\..\Resources\font\DotGothic16-Regular.ttf");
            //同様にして、複数のフォントを追加できる
            //pfc.AddFontFile(@"C:\test\wlmaru20044u.ttf");

            //PrivateFontCollectionの先頭のフォントのFontオブジェクトを作成する
            System.Drawing.Font usefont =
                new System.Drawing.Font(pfc.Families[0], 10);

            myFont = usefont;

            bullets_enemy.Clear();
            bullets_player.Clear();
            explosions.Clear();

            for (int i = 0; i < sndList.Count; i++)
            {
                sndList[i].Stop();
            }
        }

        //画面のクリック処理
        private void pBase_Click(object sender, EventArgs e)
        {
            //無敵モード切替
            if (Control.ModifierKeys == Keys.Control)
            {
                Cpos = PointToClient(Cursor.Position);
                if (!mutekiFlg &&
                        Cpos.X > 0 && Cpos.X < 50 &&
                        Cpos.Y > 0 && Cpos.Y < 50)
                {
                    mutekiFlg = true;
                    sndList[6].Stop();
                    sndList[6].Play();
                }
                else if (mutekiFlg &&
                    Cpos.X > 0 && Cpos.X < 50 &&
                    Cpos.Y > 0 && Cpos.Y < 50)
                {
                    mutekiFlg = false;
                    sndList[6].Stop();
                    sndList[6].Play();
                }
            }

            //タイトル画面でのクリック処理
            if (titleFlg && !feedFlgTforG)
            {
                if (msgcnt > 20)
                {

                    currentAlphaPercent = 0;
                    feedIntv = feedIntvTime;


                    sndList[6].Stop();
                    sndList[6].Play();

                    feedFlgTforG = true;
                }
                return;
            }

            //リザルト画面時のクリック処理
            if (msgcnt > 280 && !feedFlgRforT)
            {

                currentAlphaPercent = 0;
                feedIntv = feedIntvTime;

                sndList[6].Stop();
                sndList[6].Play();

                feedFlgRforT = true;

            }

            //ゲーム中クリック時の処理
            if (gameFlg && gameTime > 20 && !hitFlg && !clearFlg)
            {
                bullets_player.Add(new Bullet(player.x + (player.W / 2), player.y, 10, 20));
                sndList[7].Stop();
                sndList[7].Play();
            }
        }

        //ゲームオーバー演出
        private void playerExplosion()
        {
            //爆発演出中の描画は、すべてここで行う
            gg.DrawImage(bgMov.stopFrame(), new Rectangle(0, 0, 464, 664));
            /*gg.DrawImage(pBG.Image, new Rectangle(0, 0, 480, 700));*/

            //敵の描画
            for (int i = 0; i < enemys.Count; i++)
            {
                if (typeof(Missile) != enemys[i].GetType())
                {
                    gg.DrawImage(
                    enemys[i].Image, new Rectangle(enemys[i].x, enemys[i].y, enemys[i].RR * 2, enemys[i].RR * 2)
                );
                }
            }

            ecnt += 4;
            msgcnt++;

            //爆発自機
            if (msgcnt < 60)
            {
                if (ecnt > 40)
                {
                    if (msgcnt < 50)
                    {
                        ecnt = 8;
                        ex = (player.x + (player.W / 2)) + rand.Next(-20, 21);//爆発の位置を変更
                        ey = (player.y + (player.H / 2)) + rand.Next(-25, 26);

                    }
                    else
                    {
                        ecnt = 8;
                        ex = (player.x + (player.W / 2));
                        ey = (player.y + (player.H / 2));
                        expSize = 3;
                    }



                    //爆発音
                    sndList[8].Stop();
                    sndList[8].Play();
                }

                //プレイヤーと爆発描画
                gg.DrawImage(player.Image, new Rectangle(player.x, player.y, player.W, player.H));
                gg.DrawImage(pExp.Image, new Rectangle(ex - ((ecnt * expSize) / 2), ey - (ecnt * expSize) / 2, ecnt * expSize, ecnt * expSize));

            }

            //ゲームオーバー時のジングル再生
            if (msgcnt == 70)
            {
                sndList[4].Stop();
                sndList[4].Play();
            }

            //ゲーム結果文字
            if (msgcnt > 70 && msgcnt < 280)
            {
                gg.DrawImage(failedImageText, new Rectangle(70, 277, 350, 60));
            }

            resultCommon(msgcnt);
            mutekiModoInfo();
        }

        //ゲームクリア
        private void gameClear()
        {
            //爆発演出中の描画は、すべてここで行う
            /*gg.DrawImage(pBG.Image, new Rectangle(0, 0, 480, 700));*/

            if (msgcnt > 200 )
            {
                gg.DrawImage(bgMov.moveFrame(), new Rectangle(0, 0, 464, 664));
            }
            else
            {
                gg.DrawImage(bgMov.stopFrame(), new Rectangle(0, 0, 464, 664));
            }


            ecnt += 4;
            msgcnt++;
            int bN = 0;

            for (int i = 0; i < enemys.Count; i++)
            {
                if (typeof(Boss) == enemys[i].GetType())
                {
                    bN = i;
                }
            }

            if (msgcnt < 60)
            {
                if (ecnt > 40)
                {
                    if (msgcnt < 50)
                    {
                        ecnt = 8;
                        ex = enemys[bN].x + enemys[bN].RR + rand.Next(-50, 50);
                        ey = enemys[bN].y + enemys[bN].RR + rand.Next(-50, 50);
                        expSize = 3;

                    }
                    else
                    {
                        ecnt = 8;
                        ex = enemys[bN].x + enemys[bN].RR;
                        ey = enemys[bN].y + enemys[bN].RR;
                        expSize = 10;
                    }

                    /* Console.WriteLine(msgcnt);*/

                    //爆発音
                    sndList[8].Stop();
                    sndList[8].Play();
                }

                //敵と爆発描画
                gg.DrawImage(enemys[bN].Image, new Rectangle(enemys[bN].x, enemys[bN].y, enemys[bN].RR * 2, enemys[bN].RR * 2));
                gg.DrawImage(pExp.Image, new Rectangle(ex - ((ecnt * expSize) / 2), ey - ((ecnt * expSize) / 2), ecnt * expSize, ecnt * expSize));

            }

            //プレイヤー描画


            gg.DrawImage(animatedImage, new Rectangle(player.x + (player.W / 2) - 10, player.y + player.H + 15, 20, -20));
            gg.DrawImage(player.Image, new Rectangle(player.x, player.y, player.W, player.H));

            //クリア時のジングル再生
            if (msgcnt == 70)
            {
                sndList[3].Stop();
                sndList[3].Play();
            }

            //ゲームクリア文字
            if (msgcnt > 70 && msgcnt < 280)
            {
                gg.DrawImage(successImageText, new Rectangle(70, 277, 350, 60));

            }

            if (msgcnt > 200 && msgcnt < 280)
            {
                player.y -= 10;
                if (animeFrame > animatedImage.GetFrameCount(fd) - 1) { animeFrame = 0; }
                animatedImage.SelectActiveFrame(fd, animeFrame);
                animeFrame++;
            }

            resultCommon(msgcnt);
            /*
                        gg.DrawString("SCORE:" + score.ToString(),
                            myFont, Brushes.White, 10, 10);

                        gg.DrawString("Game Clear",
                        myFont, Brushes.White, 240, 300);
            */

            mutekiModoInfo();


        }

        //タイトル
        private void dispTile()
        {

            if (msgcnt == 0) sndList[0].Play();

            msgcnt++;
            //タイトル表示中の描画はすべてここで行う


            gg.DrawImage(bgMov.moveFrame(), new Rectangle(0, 0, 464, 664));

            /*gg.DrawImage(pBG.Image, new Rectangle(0, 0, 480, 700));*/

            gg.DrawImage(pTitle.Image, new Rectangle(70, 277, 350, 60));

            if (msgcnt % 60 > 20)
            {
                gg.DrawImage(pMsg.Image, new Rectangle(110, 387, 271, 26));
            }

            mutekiModoInfo();

            if (sndList[0].CurrentPosition == sndList[0].Duration)
            {
                sndList[0].CurrentPosition = 0;
            }

        }

        //プレイ画面の描画
        private void dispGame()
        {
            if (gameTime == 0)
            {
                sndList[0].Stop();
                sndList[1].Stop();
                sndList[1].Play();
            }

            gameTime++;
            gg.DrawImage(bgMov.moveFrame(), new Rectangle(0, 0, 464, 664));

            /*gg.DrawImage(pBG.Image, new Rectangle(0, 0, 480, 700));*/


            //敵の爆発の描画
            for (int i = 0; i < explosions.Count; i++)
            {

                if (explosions.Count <= 0) { return; }

                if(explosions[i].currentTime > 70)
                {
                    if (explosions.Count > 0)
                    {
                        explosions.RemoveAt(i);
                        i--;
                        if (i < 0)
                        {
                            i = 0;
                        }
                    }

                }
                else
                {
                    drawExplosion.timeDrawC(explosions[i].x, explosions[i].y, explosions[i].currentTime, gg);
                    explosions[i].currentTime += 10;
                }

            }

            //敵機の描画と移動
            for (int i = 0; i < enemys.Count; i++)
            {
                Image bullEneImage = Image.FromFile(@"..\..\Resources\p_bullet_e.png");

                //敵のタイプがミサイルの時の動作
                if (typeof(Missile) == enemys[i].GetType())
                {
                    bool delms = missileMove(enemys[i], i);
                    if (delms)
                    {
                        i--;
                    }
                }

                //敵のタイプがボスの時の動作
                else if (typeof(Boss) == enemys[i].GetType())
                {

                    //ボスの登場動作
                    if (bossIntvl < 180)
                    {
                        enemys[i].y += 5;

                        bossIntvl++;
                    }

                    //ボスの戦闘動作
                    else
                    {
                        //ミサイルの有無チェック
                        misilCnt = 0;
                        foreach (Enemy enemy in enemys)
                        {
                            if (typeof(Missile) == enemy.GetType())
                            {
                                misilCnt++;
                            }

                        }

                        //攻撃動作の抽選
                        randAdd = rand.Next(0, 51);


                        if (randAdd == 0)
                        {
                            bullets_enemy.Add(new Bullet(enemys[i].x + enemys[i].RR - 50, enemys[i].y + enemys[i].RR + 100, 10, -20, bullEneImage));
                            bullets_enemy.Add(new Bullet(enemys[i].x + enemys[i].RR + 50, enemys[i].y + enemys[i].RR + 100, 10, -20, bullEneImage));
                        }
                        else if (randAdd == 10)
                        {
                            bullets_enemy.Add(new Bullet(enemys[i].x + enemys[i].RR - 80, enemys[i].y + enemys[i].RR + 80, 10, -20, bullEneImage));
                            bullets_enemy.Add(new Bullet(enemys[i].x + enemys[i].RR + 80, enemys[i].y + enemys[i].RR + 80, 10, -20, bullEneImage));

                        }
                        else if (randAdd == 5 && misilCnt == 0)
                        {
                            enemys.Add(new Missile(enemys[i].x + enemys[i].RR, enemys[i].y + enemys[i].RR + 80));
                        }

                        //ボスの移動動作
                        radomIntvl++;

                        //横移動
                        if (enemys[i].x < 0 || enemys[i].x > Width - ((enemys[i].RR) * 2))
                        {
                            enemys[i].xVelocity *= -1;

                        }
                        else if (radomIntvl > 100 && randAdd % 2 == 0)
                        {
                            enemys[i].xVelocity *= -1;
                            radomIntvl = 0;
                        }
                        enemys[i].x += enemys[i].xVelocity;

                        //縦移動
                        if (enemys[i].y < 0 || enemys[i].y > Height / 3)
                        {
                            enemys[i].yVelocity *= -1;

                        }
                        else if (radomIntvl > 100 && randAdd % 2 != 0)
                        {

                            enemys[i].yVelocity *= -1;
                            radomIntvl = 0;
                        }
                        enemys[i].y += enemys[i].yVelocity;
                    }


                }

                //敵のタイプがボス、ミサイルでない時の動作
                else
                {
                    //敵の移動処理
                    enemys[i].y += 5;

                    //弾の生成処理
                    randAddZako = rand.Next(0, 150);

                    if (randAddZako == 0
                        && enemys[i].x > 0 && enemys[i].x < pBase.Width
                        && enemys[i].y > 0 && enemys[i].y < pBase.Width)
                    {
                        //三角関数の傾きの計算
                        double A = Cpos.X - enemys[i].x;
                        double B = Cpos.Y - enemys[i].y;
                        double AB = (A * A) + (B * B);

                        //傾きがゼロでなければ（）
                        if (AB != 0)
                        {
                            //誘導弾の位置処理
                            double C = Math.Sqrt(AB);
                            double addX = A / C;
                            double addY = B / C;

                            int bullVelocX = (int)(addX * 10.0);
                            int bullVelocY = (int)(addY * 10.0);

                            double radStr = Math.Atan2(B, A);
                            double angle = radStr * 180 / Math.PI;

                            //ラジアン単位に変換
                            double radAns = (angle - 90) / (180 / Math.PI);

                            bullets_enemy.Add(new Bullet(
                                enemys[i].x + enemys[i].RR, enemys[i].y + enemys[i].RR + 10,
                                10, -20,
                                bullVelocX, bullVelocY, true, radAns, bullEneImage
                            ));
                        }

                    }


                }

                //敵のタイプがミサイル型でない場合の描画
                if (typeof(Missile) != enemys[i].GetType())
                {
                    gg.DrawImage(
                    enemys[i].Image,
                    new Rectangle(enemys[i].x, enemys[i].y, enemys[i].RR * 2, enemys[i].RR * 2)
                );

                }

                // 敵が画面下部外に出た際の処理
                if (enemys[i].y > pBase.Height)
                {
                    //ボスが非出現時
                    if (!bossFlg)
                    {
                        //敵を画面上部外に戻す
                        enemys[i].x = rand.Next(1, 450);
                        enemys[i].y = rand.Next(1, 300) - 400;

                        //敵位置の重複チェック
                        enemyPositionCheck(enemys[i], 1);

                    }
                    else
                    {
                        //敵のタイプがボスじゃなければ、消す
                        if (typeof(Boss) != enemys[i].GetType())
                        {
                            enemys.RemoveAt(i);
                            i--;

                        }

                    }

                }

            }

            //敵の弾の描画
            for (int i = 0; i < bullets_enemy.Count; i++)
            {
                if (bullets_enemy.Count <= 0) { return; }

                var eB = bullets_enemy[i];

                if (!eB.isSlating)
                {
                    eB.y += 10;
                    gg.DrawImage(eB.Image, new Rectangle(eB.x - 5, eB.y, eB.w, eB.h));

                    //画面外に出たら弾削除
                    if (eB.y > Height)
                    {
                        if (bullets_enemy.Count > 0)
                        {
                            bullets_enemy.RemoveAt(i);
                            i--;
                            if (i < 0)
                            {
                                i = 0;
                            }
                        }

                    }
                }
                else
                {
                    if (eB.vX >= 5 && eB.vY >= 5)
                    {
                        eB.x += (int)(eB.vX * 0.7);
                        eB.y += (int)(eB.vY * 0.7);
                    }
                    else
                    {
                        eB.x += eB.vX;
                        eB.y += eB.vY;
                    }

                    //新しい座標位置を計算する
                    //左上
                    float tx = (float)eB.x;
                    float ty = (float)eB.y;
                    //右上
                    float tx1 = tx + eB.w * (float)Math.Cos(eB.radAns);
                    float ty1 = ty + eB.w * (float)Math.Sin(eB.radAns);
                    //左下
                    float tx2 = tx - eB.h * (float)Math.Sin(eB.radAns);
                    float ty2 = ty + eB.h * (float)Math.Cos(eB.radAns);

                    //PointF配列を作成
                    PointF[] destinationPoints = {
                    new PointF(tx, ty),
                    new PointF(tx1, ty1),
                    new PointF(tx2, ty2)
                    };

                    //傾けた画像の中心を設定
                    eB.cPx = (int)((tx1 + tx2) / 2);
                    eB.cPy = (int)((ty1 + ty2) / 2);

                    gg.DrawImage(eB.Image, destinationPoints);

                    //弾が範囲外に出たら消す
                    if (eB.cPx < 0 || eB.cPx > Width || eB.cPx < 0 || eB.cPx > Height)
                    {
                        if (bullets_enemy.Count > 0)
                        {
                            bullets_enemy.RemoveAt(i);
                            i--;
                            if (i < 0)
                            {
                                i = 0;
                            }
                        }
                    }

                }





            }

            //自機の弾の描画
            for (int i = 0; i < bullets_player.Count; i++)
            {
                if (bullets_player.Count <= 0) { return; }
                var eB = bullets_player[i];

                if (!eB.isSlating)
                {
                    eB.y -= 10;

                    gg.DrawImage(
                        eB.Image, new Rectangle(eB.x - 5, eB.y, eB.w, eB.h)
                    );

                    //画面外に出たら弾削除
                    if (eB.y < 0)
                    {
                        if (bullets_player.Count > 0)
                        {
                            bullets_player.RemoveAt(i);
                            i--;
                            if (i < 0)
                            {
                                i = 0;
                            }
                        }

                    }

                }
                else
                {

                }
            }

            //自機の描画と移動
            #region PlayerDrawMove
            Cpos = PointToClient(Cursor.Position);

            if (Cpos.X < 0) { Cpos.X = 20; }
            if (Cpos.X > Width - player.W) { Cpos.X = Width - player.W; }

            if (Cpos.Y < 25) { Cpos.Y = 25; }
            if (Cpos.Y > Height - player.H - 15) { Cpos.Y = Height - player.H - 15; }

            player.x = Cpos.X - (player.W / 2);
            player.y = Cpos.Y - (player.H / 2);

            if (animeFrame > animatedImage.GetFrameCount(fd) - 1) { animeFrame = 0; }
            animatedImage.SelectActiveFrame(fd, animeFrame);
            animeFrame++;

            gg.DrawImage(animatedImage, new Rectangle(player.x + (player.W / 2) - 10, player.y + player.H + 15, 20, -20));
            gg.DrawImage(player.Image, new Rectangle(player.x, player.y, player.W, player.H));


            #endregion

            //ＢＧＭのループ処理
            if (!bossFlg)
            {
                if (sndList[1].CurrentPosition == sndList[1].Duration)
                {
                    sndList[1].CurrentPosition = 0;
                }

            }
            else
            {
                if (sndList[2].CurrentPosition == sndList[2].Duration)
                {
                    sndList[2].CurrentPosition = 0;
                }

            }

            msgStart.timeDrawA(240, 350, 35, gg);
            if (bossFlg) msgWarning.timeDrawB(240, 350, 60, gg);

            gg.DrawString("SCORE:" + score.ToString(),
            myFont, Brushes.White, 30, 30);

            mutekiModoInfo();

            bossIn();
            hitCheck();
        }

        //タイマー処理
        private void timer1_Tick(object sender, EventArgs e)
        {
            /* Console.WriteLine(msgcnt + ":" + feedIntv);*/

            if (titleFlg)
            {
                dispTile();
            }

            if (hitFlg)
            {
                playerExplosion();
            }

            if (clearFlg)
            {
                gameClear();
            }

            if (!hitFlg && !clearFlg && !titleFlg)
            {
                dispGame();
            }

            //暗転処理（タイトルからゲーム）
            if (feedFlgTforG)
            {


                if (feedIntv <= feedIntvTime - 100 && feedIntv >= 100)
                {
                    Image imageIntv = Image.FromFile(@"..\..\Resources\p_BG_change.png");
                    gg.DrawImage(imageIntv, new Rectangle(0, 0, 480, 700));
                }

                if (feedIntv <= feedIntvTime && feedIntv >= feedIntvTime - 100 || feedIntv <= 200 && feedIntv >= 100)
                {
                    currentAlphaPercent += 10;
                    Image imgBlk2 = cO.CreateTranslucentImage(currentImage, currentAlphaPercent * 0.01f);
                    gg.DrawImage(imgBlk2, new Rectangle(0, 0, 480, 700));
                }

                if (feedIntv <= feedIntvTime - 100 && feedIntv >= feedIntvTime - 200 || feedIntv <= 100 && feedIntv >= 0)
                {
                    currentAlphaPercent -= 10;
                    Image imgBlk2 = cO.CreateTranslucentImage(currentImage, currentAlphaPercent * 0.01f);
                    gg.DrawImage(imgBlk2, new Rectangle(0, 0, 480, 700));
                }


                if (feedIntv == 100)
                {
                    msgcnt = 0;
                    titleFlg = false;
                }

                feedIntv -= 10;

                if (feedIntv == 0) { feedFlgTforG = false; }
            }

            //暗転処理（リザルトからタイトル）
            if (feedFlgRforT)
            {

                if (feedIntv <= feedIntvTime - 100 && feedIntv >= 100)
                {
                    Image imageIntv = Image.FromFile(@"..\..\Resources\p_BG_change.png");
                    gg.DrawImage(imageIntv, new Rectangle(0, 0, 480, 700));
                }

                if (feedIntv <= feedIntvTime && feedIntv >= feedIntvTime - 100 || feedIntv <= 200 && feedIntv >= 100)
                {
                    currentAlphaPercent += 10;
                    Image imgBlk2 = cO.CreateTranslucentImage(currentImage, currentAlphaPercent * 0.01f);
                    gg.DrawImage(imgBlk2, new Rectangle(0, 0, 480, 700));
                }

                if (feedIntv <= feedIntvTime - 100 && feedIntv >= feedIntvTime - 200 || feedIntv <= 100 && feedIntv >= 0)
                {
                    currentAlphaPercent -= 10;
                    Image imgBlk2 = cO.CreateTranslucentImage(currentImage, currentAlphaPercent * 0.01f);
                    gg.DrawImage(imgBlk2, new Rectangle(0, 0, 480, 700));
                }

                if (feedIntv == 100)
                {
                    msgcnt = 0;
                    initGame();
                }

                feedIntv -= 10;

                if (feedIntv == 0) { feedFlgRforT = false; }
            }

            pBase.Image = canvas;

        }

        //モード表記
        private void mutekiModoInfo()
        {
            if (mutekiFlg)
            {
                //StringFormatを作成
                StringFormat sf = new StringFormat();
                //文字を真ん中に表示
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                gg.DrawString("無敵モード中", myFont, Brushes.White, 240, 620, sf);
            }
        }

        //自機と被弾チェック処理
        private void hitCheck()
        {
            int pcx = Cpos.X;　//自機の中心座標
            int pcy = Cpos.Y;

            int ecx, ecy, dis;   //自機と隕石の距離計算用

            /*  Text = ""; //タイトルを消す*/

            //敵と自機のアタリ判定
            for (int i = 0; i < enemys.Count; i++)
            {
                if (typeof(Missile) != enemys[i].GetType())
                {
                    ecx = enemys[i].x + enemys[i].RR;
                    ecy = enemys[i].y + enemys[i].RR;
                }
                else
                {
                    ecx = enemys[i].cPx;
                    ecy = enemys[i].cPy;
                }

                //AB2=(x2 - x1 )2+(y2 - y1 )2　距離を出す公式に沿って半径を２乗している
                dis = (ecx - pcx) * (ecx - pcx) + (ecy - pcy) * (ecy - pcy);
                if (dis < (enemys[i].RR * enemys[i].RR) + ((player.W / 2) * (player.W / 2)))
                {
                    hitPlayerResult();

                    break;   //forから抜ける
                }
            }

            //敵の弾と自機のアタリ判定
            for (int i = 0; i < bullets_enemy.Count; i++)
            {
                if (bullets_enemy.Count <= 0) { return; }

                if (!bullets_enemy[i].isSlating)
                {
                    ecx = bullets_enemy[i].x + bullets_enemy[i].w;
                    ecy = bullets_enemy[i].y + bullets_enemy[i].w;
                }
                else
                {
                    ecx = bullets_enemy[i].cPx;
                    ecy = bullets_enemy[i].cPy;
                }

                //AB2=(x2 - x1 )2+(y2 - y1 )2　距離を出す公式に沿って半径を２乗している
                dis = (ecx - pcx) * (ecx - pcx) + (ecy - pcy) * (ecy - pcy);
                if (dis < (bullets_enemy[i].w * bullets_enemy[i].w)/* + ((player.W / 2) * (player.W / 2))*/)
                {

                    hitPlayerResult();
                    break;   //forから抜ける
                }
            }

            //敵の被弾チェッカー
            hitEnemyCheck();
        }

        //自機の被弾結果処理
        private void hitPlayerResult()
        {
            if (!mutekiFlg)
            {
                hitFlg = true; //ture:当たった
                gameFlg = false;
                sndList[1].Stop();
                sndList[2].Stop();

            }

        }

        //敵の被弾チェック処理
        private void hitEnemyCheck()
        {

            int ecx, ecy, dis;   //敵機と弾の距離計算用

            for (int j = 0; j < bullets_player.Count; j++)
            {
                if (bullets_player.Count <= 0) { return; }
                var pB = bullets_player[j];


                for (int i = 0; i < enemys.Count; i++)
                {
                    if (typeof(Missile) != enemys[i].GetType())
                    {
                        ecx = enemys[i].x + enemys[i].RR;
                        ecy = enemys[i].y + enemys[i].RR;
                    }
                    else
                    {
                        ecx = enemys[i].cPx;
                        ecy = enemys[i].cPy;
                    }

                    //AB2=(x2 - x1 )2+(y2 - y1 )2　距離を出す公式に沿って半径を２乗している
                    dis = (ecx - pB.x) * (ecx - pB.x) + (ecy - pB.y) * (ecy - pB.y);
                    if (dis < enemys[i].RR * enemys[i].RR)
                    {

                        //敵のダメージ処理
                        enemys[i].hp -= 1;

                        //敵の撃墜処理
                        if (enemys[i].hp <= 0)
                        {


                            if (typeof(Boss) != enemys[i].GetType())
                            {
                                //爆発音
                                sndList[8].Stop();
                                sndList[8].Play();
                                explosions.Add(new Explosion(ecx, ecy));

                                if (!bossFlg)
                                {
                                    enemys[i].x = rand.Next(1, 450);
                                    enemys[i].y = rand.Next(1, 300) - 400;
                                    enemys[i].hp = 3;
                                    score += 100;

                                    //敵位置の重複チェック
                                    enemyPositionCheck(enemys[i], 1);

                                }
                                else
                                {

                                    enemys.RemoveAt(i);
                                    score += 100;
                                    i--;
                                }


                            }
                            else//ボスだった場合
                            {
                                score += 10000;
                                clearFlg = true;
                                sndList[2].Stop();

                            }


                        }

                        //弾の消失処理
                        if (bullets_player.Count > 0)
                        {
                            bullets_player.RemoveAt(j);
                            j--;

                            if (j < 0)
                            {
                                j = 0;
                            }

                        }
                    }
                }

            }
        }

        //敵機・再配置チェック
        private void enemyPositionCheck(Enemy enemy, int numCheck)
        {
            int dup;
            do
            {
                dup = 0;

                for (int k = 0; k < enemys.Count; k++)
                {

                    if (enemy.Equals(enemys[k]))
                    {
                        //同じ個体同士は何もしない

                    }
                    else if (



                       enemys[k].x - ((enemys[k].RR) * 2) <= enemy.x &&
                       enemy.x <= enemys[k].x + ((enemys[k].RR) * 2) &&

                       enemys[k].y - ((enemys[k].RR) * 2) <= enemy.y &&
                       enemy.y <= enemys[k].y + ((enemys[k].RR) * 2)

                       )
                    {
                        if (numCheck == 0)
                        {
                            enemy.x = rand.Next(1, 450);
                            enemy.y = rand.Next(1, 900) - 1000;
                        }
                        else
                        {
                            enemy.x = rand.Next(1, 450);
                            enemy.y = rand.Next(1, 300) - 400;

                        }

                        dup++;
                    }

                }

            } while (dup != 0);
        }

        //ボス出現フラグ
        private void bossIn()
        {
            if (score >= 1000)
            {
                bossFlg = true;


                if (!bossInChk)
                {
                    enemys.Add(new Boss(240 - 125, -900));
                    bossInChk = true;
                    sndList[1].Stop();
                    sndList[2].Stop();
                    sndList[2].Play();
                }

            }

        }

        //誘導弾
        private bool missileMove(Enemy enemy, int num)
        {

            //誘導弾の初期値記憶
            var eB = enemy;
            missileX = (double)eB.x;
            missileY = (double)eB.y;

            //三角関数の傾きの計算
            double A = Cpos.X - eB.x;
            double B = Cpos.Y - eB.y;
            double AB = (A * A) + (B * B);

            //傾きがゼロでなければ（）
            if (AB != 0)
            {
                //誘導弾の位置処理
                double C = Math.Sqrt(AB);

                double addX = (A / C) * 3.0;
                double addY = (B / C) * 3.0;

                if (addX >= 3.0 && addY >= 3.0)
                {
                    addX *= 0.7;
                    addY *= 0.7;
                }

                missileX += addX;
                missileY += addY;



                eB.x = (int)missileX;
                eB.y = (int)missileY;

                //画像の回転処理
                radStr = Math.Atan2(B, A);
                angle = radStr * 180 / Math.PI;

                //ラジアン単位に変換
                double radAns = (angle + 90) / (180 / Math.PI);

                //新しい座標位置を計算する
                //左上
                tx = (float)eB.x;
                ty = (float)eB.y;
                //右上
                tx1 = tx + eB.w * (float)Math.Cos(radAns);
                ty1 = ty + eB.w * (float)Math.Sin(radAns);
                //左下
                tx2 = tx - eB.h * (float)Math.Sin(radAns);
                ty2 = ty + eB.h * (float)Math.Cos(radAns);


            }

            //PointF配列を作成
            PointF[] destinationPoints = {
                    new PointF(tx, ty),
                    new PointF(tx1, ty1),
                    new PointF(tx2, ty2)
                };

            //傾けた画像の中心を設定
            eB.cPx = (int)((tx1 + tx2) / 2);
            eB.cPy = (int)((ty1 + ty2) / 2);

            gg.DrawImage(eB.Image, destinationPoints);

            //弾が範囲外に出たら消す
            if (eB.x < 0 || eB.x > Width || eB.y < 0 || eB.y > Height)
            {

                enemys.RemoveAt(num);
                return true;
            }

            return false;

        }

        //リザルト共通動作
        private void resultCommon(long msgCunt)
        {
            //リザルトBGM再生
            if (msgCunt == 280)
            {
                sndList[5].Stop();
                sndList[5].Play();
            }

            //クリックメッセージの表示
            if (msgCunt > 280)
            {

                int numTxt = -100;
                Image imgBlk2 = cO.CreateTranslucentImage(currentImage, 75 * 0.01f);
                gg.DrawImage(imgBlk2, new Rectangle(0, 0, 480, 700));

                gg.DrawImage(pGameover.Image, new Rectangle(70, 277 + numTxt, 350, 60));

                if (msgCunt % 60 > 20)
                {
                    gg.DrawImage(pMsg.Image, new Rectangle(110, 387 + numTxt, 271, 26));
                }

                //StringFormatを作成
                StringFormat sf = new StringFormat();
                //文字を真ん中に表示
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                gg.DrawString("ＲＥＳＵＬＴ　ＳＣＯＲＥ", myFont, Brushes.White, 240, 450 + numTxt, sf);
                gg.DrawString("【" + score.ToString() + "】", myFont, Brushes.White, 240, 475 + numTxt, sf);

                gg.DrawString("ＴＨＡＮＫ　ＹＯＵ", myFont, Brushes.White, 240, 550 + numTxt, sf);
                gg.DrawString("ＦＯＲ", myFont, Brushes.White, 240, 575 + numTxt, sf);
                gg.DrawString("ＰＬＡＹＩＮＧ！", myFont, Brushes.White, 240, 600 + numTxt, sf);

                //BGMのループ再生処理
                if (sndList[5].CurrentPosition == sndList[5].Duration)
                {
                    sndList[5].CurrentPosition = 0;
                }
            }
        }


    }
}
