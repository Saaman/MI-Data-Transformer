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
            groupBox1 = new System.Windows.Forms.GroupBox();
            FilterOnReviewersCB = new System.Windows.Forms.CheckBox();
            NewReviewersTB = new System.Windows.Forms.TextBox();
            IdsToFilterTB = new System.Windows.Forms.TextBox();
            FilterOnReviewsIdsCB = new System.Windows.Forms.CheckBox();
            DestFTPUserLB = new System.Windows.Forms.Label();
            DestFTPPasswordLB = new System.Windows.Forms.Label();
            DestFTPPasswordTB = new System.Windows.Forms.TextBox();
            DestFTPUserTB = new System.Windows.Forms.TextBox();
            DestFileLB = new System.Windows.Forms.Label();
            DestFileTB = new System.Windows.Forms.TextBox();
            DestFTPDirLB = new System.Windows.Forms.Label();
            DestFTPDirTB = new System.Windows.Forms.TextBox();
            DestFtpRB = new System.Windows.Forms.RadioButton();
            DestFileRB = new System.Windows.Forms.RadioButton();
            groupBox2 = new System.Windows.Forms.GroupBox();
            SourceFileLB = new System.Windows.Forms.Label();
            SourceFileTB = new System.Windows.Forms.TextBox();
            StartStopButton = new System.Windows.Forms.Button();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            ReviewsWorkerInfos = new System.Windows.Forms.Label();
            ReviewsBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            LastUpdateDateLB = new System.Windows.Forms.Label();
            LastUpdateDateDTP = new System.Windows.Forms.DateTimePicker();
            groupBox3 = new System.Windows.Forms.GroupBox();
            OperationsListBox = new System.Windows.Forms.CheckedListBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(FilterOnReviewersCB);
            groupBox1.Controls.Add(NewReviewersTB);
            groupBox1.Controls.Add(IdsToFilterTB);
            groupBox1.Controls.Add(FilterOnReviewsIdsCB);
            groupBox1.Controls.Add(DestFTPUserLB);
            groupBox1.Controls.Add(DestFTPPasswordLB);
            groupBox1.Controls.Add(DestFTPPasswordTB);
            groupBox1.Controls.Add(DestFTPUserTB);
            groupBox1.Controls.Add(DestFileLB);
            groupBox1.Controls.Add(DestFileTB);
            groupBox1.Controls.Add(DestFTPDirLB);
            groupBox1.Controls.Add(DestFTPDirTB);
            groupBox1.Controls.Add(DestFtpRB);
            groupBox1.Controls.Add(DestFileRB);
            groupBox1.Location = new System.Drawing.Point(12, 253);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(612, 128);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Destination des XMLs générés";
            // 
            // FilterOnReviewersCB
            // 
            FilterOnReviewersCB.AutoSize = true;
            FilterOnReviewersCB.Location = new System.Drawing.Point(11, 104);
            FilterOnReviewersCB.Name = "FilterOnReviewersCB";
            FilterOnReviewersCB.Size = new System.Drawing.Size(154, 17);
            FilterOnReviewersCB.TabIndex = 22;
            FilterOnReviewersCB.Text = "Filtrer sur les chroniqueurs :";
            FilterOnReviewersCB.UseVisualStyleBackColor = true;
            // 
            // NewReviewersTB
            // 
            NewReviewersTB.Location = new System.Drawing.Point(171, 102);
            NewReviewersTB.Name = "NewReviewersTB";
            NewReviewersTB.Size = new System.Drawing.Size(435, 20);
            NewReviewersTB.TabIndex = 21;
            // 
            // IdsToFilterTB
            // 
            IdsToFilterTB.Location = new System.Drawing.Point(124, 77);
            IdsToFilterTB.Name = "IdsToFilterTB";
            IdsToFilterTB.Size = new System.Drawing.Size(248, 20);
            IdsToFilterTB.TabIndex = 19;
            // 
            // FilterOnReviewsIdsCB
            // 
            FilterOnReviewsIdsCB.AutoSize = true;
            FilterOnReviewsIdsCB.Location = new System.Drawing.Point(11, 77);
            FilterOnReviewsIdsCB.Name = "FilterOnReviewsIdsCB";
            FilterOnReviewsIdsCB.Size = new System.Drawing.Size(106, 17);
            FilterOnReviewsIdsCB.TabIndex = 18;
            FilterOnReviewsIdsCB.Text = "Filtrer sur les ids :";
            FilterOnReviewsIdsCB.UseVisualStyleBackColor = true;
            // 
            // DestFTPUserLB
            // 
            DestFTPUserLB.AutoSize = true;
            DestFTPUserLB.Location = new System.Drawing.Point(378, 51);
            DestFTPUserLB.Name = "DestFTPUserLB";
            DestFTPUserLB.Size = new System.Drawing.Size(64, 13);
            DestFTPUserLB.TabIndex = 17;
            DestFTPUserLB.Text = "User name :";
            // 
            // DestFTPPasswordLB
            // 
            DestFTPPasswordLB.AutoSize = true;
            DestFTPPasswordLB.Location = new System.Drawing.Point(378, 77);
            DestFTPPasswordLB.Name = "DestFTPPasswordLB";
            DestFTPPasswordLB.Size = new System.Drawing.Size(80, 13);
            DestFTPPasswordLB.TabIndex = 16;
            DestFTPPasswordLB.Text = "Mot de passe : ";
            // 
            // DestFTPPasswordTB
            // 
            DestFTPPasswordTB.Location = new System.Drawing.Point(459, 74);
            DestFTPPasswordTB.Name = "DestFTPPasswordTB";
            DestFTPPasswordTB.Size = new System.Drawing.Size(147, 20);
            DestFTPPasswordTB.TabIndex = 15;
            DestFTPPasswordTB.UseSystemPasswordChar = true;
            // 
            // DestFTPUserTB
            // 
            DestFTPUserTB.Location = new System.Drawing.Point(459, 48);
            DestFTPUserTB.Name = "DestFTPUserTB";
            DestFTPUserTB.Size = new System.Drawing.Size(147, 20);
            DestFTPUserTB.TabIndex = 14;
            // 
            // DestFileLB
            // 
            DestFileLB.AutoSize = true;
            DestFileLB.Location = new System.Drawing.Point(86, 39);
            DestFileLB.Name = "DestFileLB";
            DestFileLB.Size = new System.Drawing.Size(87, 13);
            DestFileLB.TabIndex = 13;
            DestFileLB.Text = "Répertoire local :";
            // 
            // DestFileTB
            // 
            DestFileTB.Location = new System.Drawing.Point(179, 36);
            DestFileTB.Name = "DestFileTB";
            DestFileTB.Size = new System.Drawing.Size(210, 20);
            DestFileTB.TabIndex = 12;
            // 
            // DestFTPDirLB
            // 
            DestFTPDirLB.AutoSize = true;
            DestFTPDirLB.Location = new System.Drawing.Point(6, 51);
            DestFTPDirLB.Name = "DestFTPDirLB";
            DestFTPDirLB.Size = new System.Drawing.Size(73, 13);
            DestFTPDirLB.TabIndex = 11;
            DestFTPDirLB.Text = "Serveur FTP :";
            // 
            // DestFTPDirTB
            // 
            DestFTPDirTB.Location = new System.Drawing.Point(99, 48);
            DestFTPDirTB.Name = "DestFTPDirTB";
            DestFTPDirTB.Size = new System.Drawing.Size(210, 20);
            DestFTPDirTB.TabIndex = 10;
            // 
            // DestFtpRB
            // 
            DestFtpRB.AutoSize = true;
            DestFtpRB.Location = new System.Drawing.Point(116, 19);
            DestFtpRB.Name = "DestFtpRB";
            DestFtpRB.Size = new System.Drawing.Size(97, 17);
            DestFtpRB.TabIndex = 5;
            DestFtpRB.TabStop = true;
            DestFtpRB.Text = "Repertoire FTP";
            DestFtpRB.UseVisualStyleBackColor = true;
            DestFtpRB.CheckedChanged += new System.EventHandler(DestRB_CheckedChanged);
            // 
            // DestFileRB
            // 
            DestFileRB.AutoSize = true;
            DestFileRB.Location = new System.Drawing.Point(11, 19);
            DestFileRB.Name = "DestFileRB";
            DestFileRB.Size = new System.Drawing.Size(99, 17);
            DestFileRB.TabIndex = 4;
            DestFileRB.TabStop = true;
            DestFileRB.Text = "Répertoire local";
            DestFileRB.UseVisualStyleBackColor = true;
            DestFileRB.CheckedChanged += new System.EventHandler(DestRB_CheckedChanged);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(SourceFileLB);
            groupBox2.Controls.Add(SourceFileTB);
            groupBox2.Location = new System.Drawing.Point(12, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(612, 44);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "Source de données";
            // 
            // SourceFileLB
            // 
            SourceFileLB.AutoSize = true;
            SourceFileLB.Location = new System.Drawing.Point(6, 16);
            SourceFileLB.Name = "SourceFileLB";
            SourceFileLB.Size = new System.Drawing.Size(155, 13);
            SourceFileLB.TabIndex = 12;
            SourceFileLB.Text = "Chemin du fichier .csv source : ";
            // 
            // SourceFileTB
            // 
            SourceFileTB.Location = new System.Drawing.Point(162, 13);
            SourceFileTB.Name = "SourceFileTB";
            SourceFileTB.Size = new System.Drawing.Size(210, 20);
            SourceFileTB.TabIndex = 11;
            // 
            // StartStopButton
            // 
            StartStopButton.Location = new System.Drawing.Point(543, 402);
            StartStopButton.Name = "StartStopButton";
            StartStopButton.Size = new System.Drawing.Size(75, 23);
            StartStopButton.TabIndex = 7;
            StartStopButton.Text = "Générer";
            StartStopButton.UseVisualStyleBackColor = true;
            StartStopButton.Click += new System.EventHandler(StartStopButton_Click);
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(12, 439);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(612, 23);
            progressBar1.TabIndex = 2;
            // 
            // ReviewsWorkerInfos
            // 
            ReviewsWorkerInfos.AutoSize = true;
            ReviewsWorkerInfos.Location = new System.Drawing.Point(9, 471);
            ReviewsWorkerInfos.Name = "ReviewsWorkerInfos";
            ReviewsWorkerInfos.Size = new System.Drawing.Size(35, 13);
            ReviewsWorkerInfos.TabIndex = 3;
            ReviewsWorkerInfos.Text = "label3";
            // 
            // ReviewsBackgroundWorker
            // 
            ReviewsBackgroundWorker.WorkerReportsProgress = true;
            ReviewsBackgroundWorker.WorkerSupportsCancellation = true;
            ReviewsBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(ReviewsBackgroundWorker_DoWork);
            ReviewsBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(ReviewsBackgroundWorker_RunWorkerCompleted);
            ReviewsBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(ReviewsBackgroundWorker_ProgressChanged);
            // 
            // LastUpdateDateLB
            // 
            LastUpdateDateLB.AutoSize = true;
            LastUpdateDateLB.Location = new System.Drawing.Point(16, 405);
            LastUpdateDateLB.Name = "LastUpdateDateLB";
            LastUpdateDateLB.Size = new System.Drawing.Size(195, 13);
            LastUpdateDateLB.TabIndex = 19;
            LastUpdateDateLB.Text = "Date de dernière génération des XMLs :";
            // 
            // LastUpdateDateDTP
            // 
            LastUpdateDateDTP.Location = new System.Drawing.Point(211, 403);
            LastUpdateDateDTP.Name = "LastUpdateDateDTP";
            LastUpdateDateDTP.Size = new System.Drawing.Size(200, 20);
            LastUpdateDateDTP.TabIndex = 20;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(OperationsListBox);
            groupBox3.Location = new System.Drawing.Point(12, 62);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(612, 185);
            groupBox3.TabIndex = 13;
            groupBox3.TabStop = false;
            groupBox3.Text = "Opérations";
            // 
            // OperationsListBox
            // 
            OperationsListBox.CheckOnClick = true;
            OperationsListBox.FormattingEnabled = true;
            OperationsListBox.Location = new System.Drawing.Point(6, 19);
            OperationsListBox.Name = "OperationsListBox";
            OperationsListBox.Size = new System.Drawing.Size(334, 154);
            OperationsListBox.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(636, 489);
            Controls.Add(groupBox3);
            Controls.Add(LastUpdateDateDTP);
            Controls.Add(LastUpdateDateLB);
            Controls.Add(StartStopButton);
            Controls.Add(groupBox2);
            Controls.Add(ReviewsWorkerInfos);
            Controls.Add(progressBar1);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Metal Impact";
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

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
    }
}

