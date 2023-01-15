
namespace Meteor
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pBase = new System.Windows.Forms.PictureBox();
            this.pGameover = new System.Windows.Forms.PictureBox();
            this.pPlayer = new System.Windows.Forms.PictureBox();
            this.pExp = new System.Windows.Forms.PictureBox();
            this.pTitle = new System.Windows.Forms.PictureBox();
            this.pMsg = new System.Windows.Forms.PictureBox();
            this.pMeteor = new System.Windows.Forms.PictureBox();
            this.pBG = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGameover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMsg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMeteor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBG)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pBase
            // 
            this.pBase.Location = new System.Drawing.Point(0, 0);
            this.pBase.Name = "pBase";
            this.pBase.Size = new System.Drawing.Size(464, 664);
            this.pBase.TabIndex = 7;
            this.pBase.TabStop = false;
            this.pBase.Click += new System.EventHandler(this.pBase_Click);
            // 
            // pGameover
            // 
            this.pGameover.Image = global::Meteor.Properties.Resources.p_gameover;
            this.pGameover.Location = new System.Drawing.Point(98, 103);
            this.pGameover.Name = "pGameover";
            this.pGameover.Size = new System.Drawing.Size(100, 50);
            this.pGameover.TabIndex = 6;
            this.pGameover.TabStop = false;
            // 
            // pPlayer
            // 
            this.pPlayer.Image = global::Meteor.Properties.Resources.p_player;
            this.pPlayer.Location = new System.Drawing.Point(256, 220);
            this.pPlayer.Name = "pPlayer";
            this.pPlayer.Size = new System.Drawing.Size(100, 50);
            this.pPlayer.TabIndex = 5;
            this.pPlayer.TabStop = false;
            // 
            // pExp
            // 
            this.pExp.Image = global::Meteor.Properties.Resources.p_explosion;
            this.pExp.Location = new System.Drawing.Point(113, 23);
            this.pExp.Name = "pExp";
            this.pExp.Size = new System.Drawing.Size(169, 100);
            this.pExp.TabIndex = 4;
            this.pExp.TabStop = false;
            // 
            // pTitle
            // 
            this.pTitle.Image = global::Meteor.Properties.Resources.p_title;
            this.pTitle.Location = new System.Drawing.Point(81, 235);
            this.pTitle.Name = "pTitle";
            this.pTitle.Size = new System.Drawing.Size(100, 50);
            this.pTitle.TabIndex = 3;
            this.pTitle.TabStop = false;
            // 
            // pMsg
            // 
            this.pMsg.Image = global::Meteor.Properties.Resources.p_msg;
            this.pMsg.Location = new System.Drawing.Point(98, 150);
            this.pMsg.Name = "pMsg";
            this.pMsg.Size = new System.Drawing.Size(100, 50);
            this.pMsg.TabIndex = 2;
            this.pMsg.TabStop = false;
            // 
            // pMeteor
            // 
            this.pMeteor.Image = global::Meteor.Properties.Resources.p_meteor;
            this.pMeteor.Location = new System.Drawing.Point(171, 276);
            this.pMeteor.Name = "pMeteor";
            this.pMeteor.Size = new System.Drawing.Size(100, 50);
            this.pMeteor.TabIndex = 1;
            this.pMeteor.TabStop = false;
            // 
            // pBG
            // 
            this.pBG.Image = global::Meteor.Properties.Resources.p_bg;
            this.pBG.Location = new System.Drawing.Point(243, 94);
            this.pBG.Name = "pBG";
            this.pBG.Size = new System.Drawing.Size(78, 107);
            this.pBG.TabIndex = 0;
            this.pBG.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 661);
            this.Controls.Add(this.pBase);
            this.Controls.Add(this.pGameover);
            this.Controls.Add(this.pPlayer);
            this.Controls.Add(this.pExp);
            this.Controls.Add(this.pTitle);
            this.Controls.Add(this.pMsg);
            this.Controls.Add(this.pMeteor);
            this.Controls.Add(this.pBG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGameover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMsg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMeteor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBG)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBG;
        private System.Windows.Forms.PictureBox pMeteor;
        private System.Windows.Forms.PictureBox pMsg;
        private System.Windows.Forms.PictureBox pTitle;
        private System.Windows.Forms.PictureBox pExp;
        private System.Windows.Forms.PictureBox pPlayer;
        private System.Windows.Forms.PictureBox pGameover;
        private System.Windows.Forms.PictureBox pBase;
        private System.Windows.Forms.Timer timer1;
    }
}

