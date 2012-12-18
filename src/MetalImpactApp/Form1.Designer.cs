namespace MetalImpactApp
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FilterOnReviewersCB = new System.Windows.Forms.CheckBox();
            this.NewReviewersTB = new System.Windows.Forms.TextBox();
            this.IdsToFilterTB = new System.Windows.Forms.TextBox();
            this.FilterOnReviewsIdsCB = new System.Windows.Forms.CheckBox();
            this.DestFTPUserLB = new System.Windows.Forms.Label();
            this.DestFTPPasswordLB = new System.Windows.Forms.Label();
            this.DestFTPPasswordTB = new System.Windows.Forms.TextBox();
            this.DestFTPUserTB = new System.Windows.Forms.TextBox();
            this.DestFileLB = new System.Windows.Forms.Label();
            this.DestFileTB = new System.Windows.Forms.TextBox();
            this.DestFTPDirLB = new System.Windows.Forms.Label();
            this.DestFTPDirTB = new System.Windows.Forms.TextBox();
            this.DestFtpRB = new System.Windows.Forms.RadioButton();
            this.DestFileRB = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SourceFileLB = new System.Windows.Forms.Label();
            this.SourceFileTB = new System.Windows.Forms.TextBox();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ReviewsWorkerInfos = new System.Windows.Forms.Label();
            this.ReviewsBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.LastUpdateDateLB = new System.Windows.Forms.Label();
            this.LastUpdateDateDTP = new System.Windows.Forms.DateTimePicker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.OperationsListBox = new System.Windows.Forms.CheckedListBox();
            this.DoReviewCleaning = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FilterOnReviewersCB);
            this.groupBox1.Controls.Add(this.NewReviewersTB);
            this.groupBox1.Controls.Add(this.IdsToFilterTB);
            this.groupBox1.Controls.Add(this.FilterOnReviewsIdsCB);
            this.groupBox1.Controls.Add(this.DestFTPUserLB);
            this.groupBox1.Controls.Add(this.DestFTPPasswordLB);
            this.groupBox1.Controls.Add(this.DestFTPPasswordTB);
            this.groupBox1.Controls.Add(this.DestFTPUserTB);
            this.groupBox1.Controls.Add(this.DestFileLB);
            this.groupBox1.Controls.Add(this.DestFileTB);
            this.groupBox1.Controls.Add(this.DestFTPDirLB);
            this.groupBox1.Controls.Add(this.DestFTPDirTB);
            this.groupBox1.Controls.Add(this.DestFtpRB);
            this.groupBox1.Controls.Add(this.DestFileRB);
            this.groupBox1.Location = new System.Drawing.Point(12, 253);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(612, 128);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Destination des XMLs générés";
            // 
            // FilterOnReviewersCB
            // 
            this.FilterOnReviewersCB.AutoSize = true;
            this.FilterOnReviewersCB.Location = new System.Drawing.Point(11, 104);
            this.FilterOnReviewersCB.Name = "FilterOnReviewersCB";
            this.FilterOnReviewersCB.Size = new System.Drawing.Size(154, 17);
            this.FilterOnReviewersCB.TabIndex = 22;
            this.FilterOnReviewersCB.Text = "Filtrer sur les chroniqueurs :";
            this.FilterOnReviewersCB.UseVisualStyleBackColor = true;
            // 
            // NewReviewersTB
            // 
            this.NewReviewersTB.Location = new System.Drawing.Point(171, 102);
            this.NewReviewersTB.Name = "NewReviewersTB";
            this.NewReviewersTB.Size = new System.Drawing.Size(435, 20);
            this.NewReviewersTB.TabIndex = 21;
            // 
            // IdsToFilterTB
            // 
            this.IdsToFilterTB.Location = new System.Drawing.Point(124, 77);
            this.IdsToFilterTB.Name = "IdsToFilterTB";
            this.IdsToFilterTB.Size = new System.Drawing.Size(248, 20);
            this.IdsToFilterTB.TabIndex = 19;
            // 
            // FilterOnReviewsIdsCB
            // 
            this.FilterOnReviewsIdsCB.AutoSize = true;
            this.FilterOnReviewsIdsCB.Location = new System.Drawing.Point(11, 77);
            this.FilterOnReviewsIdsCB.Name = "FilterOnReviewsIdsCB";
            this.FilterOnReviewsIdsCB.Size = new System.Drawing.Size(106, 17);
            this.FilterOnReviewsIdsCB.TabIndex = 18;
            this.FilterOnReviewsIdsCB.Text = "Filtrer sur les ids :";
            this.FilterOnReviewsIdsCB.UseVisualStyleBackColor = true;
            // 
            // DestFTPUserLB
            // 
            this.DestFTPUserLB.AutoSize = true;
            this.DestFTPUserLB.Location = new System.Drawing.Point(378, 51);
            this.DestFTPUserLB.Name = "DestFTPUserLB";
            this.DestFTPUserLB.Size = new System.Drawing.Size(64, 13);
            this.DestFTPUserLB.TabIndex = 17;
            this.DestFTPUserLB.Text = "User name :";
            // 
            // DestFTPPasswordLB
            // 
            this.DestFTPPasswordLB.AutoSize = true;
            this.DestFTPPasswordLB.Location = new System.Drawing.Point(378, 77);
            this.DestFTPPasswordLB.Name = "DestFTPPasswordLB";
            this.DestFTPPasswordLB.Size = new System.Drawing.Size(80, 13);
            this.DestFTPPasswordLB.TabIndex = 16;
            this.DestFTPPasswordLB.Text = "Mot de passe : ";
            // 
            // DestFTPPasswordTB
            // 
            this.DestFTPPasswordTB.Location = new System.Drawing.Point(459, 74);
            this.DestFTPPasswordTB.Name = "DestFTPPasswordTB";
            this.DestFTPPasswordTB.Size = new System.Drawing.Size(147, 20);
            this.DestFTPPasswordTB.TabIndex = 15;
            this.DestFTPPasswordTB.UseSystemPasswordChar = true;
            // 
            // DestFTPUserTB
            // 
            this.DestFTPUserTB.Location = new System.Drawing.Point(459, 48);
            this.DestFTPUserTB.Name = "DestFTPUserTB";
            this.DestFTPUserTB.Size = new System.Drawing.Size(147, 20);
            this.DestFTPUserTB.TabIndex = 14;
            // 
            // DestFileLB
            // 
            this.DestFileLB.AutoSize = true;
            this.DestFileLB.Location = new System.Drawing.Point(86, 39);
            this.DestFileLB.Name = "DestFileLB";
            this.DestFileLB.Size = new System.Drawing.Size(87, 13);
            this.DestFileLB.TabIndex = 13;
            this.DestFileLB.Text = "Répertoire local :";
            // 
            // DestFileTB
            // 
            this.DestFileTB.Location = new System.Drawing.Point(179, 36);
            this.DestFileTB.Name = "DestFileTB";
            this.DestFileTB.Size = new System.Drawing.Size(210, 20);
            this.DestFileTB.TabIndex = 12;
            // 
            // DestFTPDirLB
            // 
            this.DestFTPDirLB.AutoSize = true;
            this.DestFTPDirLB.Location = new System.Drawing.Point(6, 51);
            this.DestFTPDirLB.Name = "DestFTPDirLB";
            this.DestFTPDirLB.Size = new System.Drawing.Size(73, 13);
            this.DestFTPDirLB.TabIndex = 11;
            this.DestFTPDirLB.Text = "Serveur FTP :";
            // 
            // DestFTPDirTB
            // 
            this.DestFTPDirTB.Location = new System.Drawing.Point(99, 48);
            this.DestFTPDirTB.Name = "DestFTPDirTB";
            this.DestFTPDirTB.Size = new System.Drawing.Size(210, 20);
            this.DestFTPDirTB.TabIndex = 10;
            // 
            // DestFtpRB
            // 
            this.DestFtpRB.AutoSize = true;
            this.DestFtpRB.Location = new System.Drawing.Point(116, 19);
            this.DestFtpRB.Name = "DestFtpRB";
            this.DestFtpRB.Size = new System.Drawing.Size(97, 17);
            this.DestFtpRB.TabIndex = 5;
            this.DestFtpRB.TabStop = true;
            this.DestFtpRB.Text = "Repertoire FTP";
            this.DestFtpRB.UseVisualStyleBackColor = true;
            this.DestFtpRB.CheckedChanged += new System.EventHandler(this.DestRB_CheckedChanged);
            // 
            // DestFileRB
            // 
            this.DestFileRB.AutoSize = true;
            this.DestFileRB.Location = new System.Drawing.Point(11, 19);
            this.DestFileRB.Name = "DestFileRB";
            this.DestFileRB.Size = new System.Drawing.Size(99, 17);
            this.DestFileRB.TabIndex = 4;
            this.DestFileRB.TabStop = true;
            this.DestFileRB.Text = "Répertoire local";
            this.DestFileRB.UseVisualStyleBackColor = true;
            this.DestFileRB.CheckedChanged += new System.EventHandler(this.DestRB_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DoReviewCleaning);
            this.groupBox2.Controls.Add(this.SourceFileLB);
            this.groupBox2.Controls.Add(this.SourceFileTB);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(612, 44);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Source de données";
            // 
            // SourceFileLB
            // 
            this.SourceFileLB.AutoSize = true;
            this.SourceFileLB.Location = new System.Drawing.Point(6, 16);
            this.SourceFileLB.Name = "SourceFileLB";
            this.SourceFileLB.Size = new System.Drawing.Size(155, 13);
            this.SourceFileLB.TabIndex = 12;
            this.SourceFileLB.Text = "Chemin du fichier .csv source : ";
            // 
            // SourceFileTB
            // 
            this.SourceFileTB.Location = new System.Drawing.Point(162, 13);
            this.SourceFileTB.Name = "SourceFileTB";
            this.SourceFileTB.Size = new System.Drawing.Size(210, 20);
            this.SourceFileTB.TabIndex = 11;
            // 
            // StartStopButton
            // 
            this.StartStopButton.Location = new System.Drawing.Point(543, 402);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(75, 23);
            this.StartStopButton.TabIndex = 7;
            this.StartStopButton.Text = "Générer";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 439);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(612, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // ReviewsWorkerInfos
            // 
            this.ReviewsWorkerInfos.AutoSize = true;
            this.ReviewsWorkerInfos.Location = new System.Drawing.Point(9, 471);
            this.ReviewsWorkerInfos.Name = "ReviewsWorkerInfos";
            this.ReviewsWorkerInfos.Size = new System.Drawing.Size(35, 13);
            this.ReviewsWorkerInfos.TabIndex = 3;
            this.ReviewsWorkerInfos.Text = "label3";
            // 
            // ReviewsBackgroundWorker
            // 
            this.ReviewsBackgroundWorker.WorkerReportsProgress = true;
            this.ReviewsBackgroundWorker.WorkerSupportsCancellation = true;
            this.ReviewsBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ReviewsBackgroundWorker_DoWork);
            this.ReviewsBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ReviewsBackgroundWorker_ProgressChanged);
            this.ReviewsBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ReviewsBackgroundWorker_RunWorkerCompleted);
            // 
            // LastUpdateDateLB
            // 
            this.LastUpdateDateLB.AutoSize = true;
            this.LastUpdateDateLB.Location = new System.Drawing.Point(16, 405);
            this.LastUpdateDateLB.Name = "LastUpdateDateLB";
            this.LastUpdateDateLB.Size = new System.Drawing.Size(195, 13);
            this.LastUpdateDateLB.TabIndex = 19;
            this.LastUpdateDateLB.Text = "Date de dernière génération des XMLs :";
            // 
            // LastUpdateDateDTP
            // 
            this.LastUpdateDateDTP.Location = new System.Drawing.Point(211, 403);
            this.LastUpdateDateDTP.Name = "LastUpdateDateDTP";
            this.LastUpdateDateDTP.Size = new System.Drawing.Size(200, 20);
            this.LastUpdateDateDTP.TabIndex = 20;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.OperationsListBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 62);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(612, 185);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Opérations";
            // 
            // OperationsListBox
            // 
            this.OperationsListBox.CheckOnClick = true;
            this.OperationsListBox.FormattingEnabled = true;
            this.OperationsListBox.Location = new System.Drawing.Point(6, 19);
            this.OperationsListBox.Name = "OperationsListBox";
            this.OperationsListBox.Size = new System.Drawing.Size(334, 154);
            this.OperationsListBox.TabIndex = 0;
            // 
            // doReviewCleaning
            // 
            this.DoReviewCleaning.AutoSize = true;
            this.DoReviewCleaning.Location = new System.Drawing.Point(413, 16);
            this.DoReviewCleaning.Name = "DoReviewCleaning";
            this.DoReviewCleaning.Size = new System.Drawing.Size(163, 17);
            this.DoReviewCleaning.TabIndex = 13;
            this.DoReviewCleaning.Text = "Nettoyer le texte de la review";
            this.DoReviewCleaning.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 489);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.LastUpdateDateDTP);
            this.Controls.Add(this.LastUpdateDateLB);
            this.Controls.Add(this.StartStopButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ReviewsWorkerInfos);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Metal Impact";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label ReviewsWorkerInfos;
        private System.ComponentModel.BackgroundWorker ReviewsBackgroundWorker;
        private System.Windows.Forms.Label SourceFileLB;
        private System.Windows.Forms.TextBox SourceFileTB;
        private System.Windows.Forms.RadioButton DestFileRB;
        private System.Windows.Forms.Label DestFTPUserLB;
        private System.Windows.Forms.Label DestFTPPasswordLB;
        private System.Windows.Forms.TextBox DestFTPPasswordTB;
        private System.Windows.Forms.TextBox DestFTPUserTB;
        private System.Windows.Forms.Label DestFileLB;
        private System.Windows.Forms.TextBox DestFileTB;
        private System.Windows.Forms.Label DestFTPDirLB;
        private System.Windows.Forms.TextBox DestFTPDirTB;
        private System.Windows.Forms.RadioButton DestFtpRB;
        private System.Windows.Forms.Label LastUpdateDateLB;
        private System.Windows.Forms.DateTimePicker LastUpdateDateDTP;
        private System.Windows.Forms.TextBox IdsToFilterTB;
        private System.Windows.Forms.CheckBox FilterOnReviewsIdsCB;
        private System.Windows.Forms.TextBox NewReviewersTB;
        private System.Windows.Forms.CheckBox FilterOnReviewersCB;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox OperationsListBox;
        private System.Windows.Forms.CheckBox DoReviewCleaning;
    }
}

