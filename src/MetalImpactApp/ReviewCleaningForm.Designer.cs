namespace MetalImpactApp
{
    partial class ReviewCleaningForm
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
            this.TitleLabel = new System.Windows.Forms.Label();
            this.reviewerLabel = new System.Windows.Forms.Label();
            this.IdLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DeleteToNextRB = new System.Windows.Forms.RadioButton();
            this.DeleteMatchRB = new System.Windows.Forms.RadioButton();
            this.KeepContentRB = new System.Windows.Forms.RadioButton();
            this.DoNothingRB = new System.Windows.Forms.RadioButton();
            this.RepeatInReviewsCB = new System.Windows.Forms.CheckBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.FinishButton = new System.Windows.Forms.Button();
            this.ComparisonGB = new System.Windows.Forms.GroupBox();
            this.FinalTextRTB = new System.Windows.Forms.RichTextBox();
            this.ReplacementTextLb = new System.Windows.Forms.Label();
            this.DeletedTextLb = new System.Windows.Forms.Label();
            this.ModifiedTextRTB = new System.Windows.Forms.RichTextBox();
            this.OriginalTextRTB = new System.Windows.Forms.RichTextBox();
            this.ModifiedTextLb = new System.Windows.Forms.Label();
            this.OrginalTextLb = new System.Windows.Forms.Label();
            this.CountInfos = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ComparisonGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TitleLabel);
            this.groupBox1.Controls.Add(this.reviewerLabel);
            this.groupBox1.Controls.Add(this.IdLabel);
            this.groupBox1.Location = new System.Drawing.Point(629, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 114);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informations générales";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoEllipsis = true;
            this.TitleLabel.Location = new System.Drawing.Point(7, 54);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(337, 13);
            this.TitleLabel.TabIndex = 2;
            this.TitleLabel.Text = "label1";
            // 
            // reviewerLabel
            // 
            this.reviewerLabel.AutoEllipsis = true;
            this.reviewerLabel.Location = new System.Drawing.Point(7, 88);
            this.reviewerLabel.Name = "reviewerLabel";
            this.reviewerLabel.Size = new System.Drawing.Size(337, 13);
            this.reviewerLabel.TabIndex = 1;
            this.reviewerLabel.Text = "label1";
            // 
            // IdLabel
            // 
            this.IdLabel.AutoEllipsis = true;
            this.IdLabel.Location = new System.Drawing.Point(7, 20);
            this.IdLabel.Name = "IdLabel";
            this.IdLabel.Size = new System.Drawing.Size(337, 13);
            this.IdLabel.TabIndex = 0;
            this.IdLabel.Text = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DeleteToNextRB);
            this.groupBox2.Controls.Add(this.DeleteMatchRB);
            this.groupBox2.Controls.Add(this.KeepContentRB);
            this.groupBox2.Controls.Add(this.DoNothingRB);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(375, 114);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Choisissez une méthode de remplacement :";
            // 
            // DeleteToNextRB
            // 
            this.DeleteToNextRB.AutoSize = true;
            this.DeleteToNextRB.Location = new System.Drawing.Point(5, 88);
            this.DeleteToNextRB.Name = "DeleteToNextRB";
            this.DeleteToNextRB.Size = new System.Drawing.Size(332, 17);
            this.DeleteToNextRB.TabIndex = 4;
            this.DeleteToNextRB.TabStop = true;
            this.DeleteToNextRB.Text = "Supprimer du début de la balise jusqu\'à la prochaine modification.";
            this.DeleteToNextRB.UseVisualStyleBackColor = true;
            this.DeleteToNextRB.CheckedChanged += new System.EventHandler(this.DeleteToNextRB_CheckedChanged);
            // 
            // DeleteMatchRB
            // 
            this.DeleteMatchRB.AutoSize = true;
            this.DeleteMatchRB.Location = new System.Drawing.Point(5, 65);
            this.DeleteMatchRB.Name = "DeleteMatchRB";
            this.DeleteMatchRB.Size = new System.Drawing.Size(200, 17);
            this.DeleteMatchRB.TabIndex = 3;
            this.DeleteMatchRB.TabStop = true;
            this.DeleteMatchRB.Text = "Supprimer les balises et leur contenu.";
            this.DeleteMatchRB.UseVisualStyleBackColor = true;
            this.DeleteMatchRB.CheckedChanged += new System.EventHandler(this.DeleteMatchRB_CheckedChanged);
            // 
            // KeepContentRB
            // 
            this.KeepContentRB.AutoSize = true;
            this.KeepContentRB.Location = new System.Drawing.Point(6, 42);
            this.KeepContentRB.Name = "KeepContentRB";
            this.KeepContentRB.Size = new System.Drawing.Size(168, 17);
            this.KeepContentRB.TabIndex = 2;
            this.KeepContentRB.TabStop = true;
            this.KeepContentRB.Text = "Garder le contenu des balises.";
            this.KeepContentRB.UseVisualStyleBackColor = true;
            this.KeepContentRB.CheckedChanged += new System.EventHandler(this.KeepContentRB_CheckedChanged);
            // 
            // DoNothingRB
            // 
            this.DoNothingRB.AutoSize = true;
            this.DoNothingRB.Location = new System.Drawing.Point(6, 19);
            this.DoNothingRB.Name = "DoNothingRB";
            this.DoNothingRB.Size = new System.Drawing.Size(85, 17);
            this.DoNothingRB.TabIndex = 1;
            this.DoNothingRB.TabStop = true;
            this.DoNothingRB.Text = "Ne rien faire.";
            this.DoNothingRB.UseVisualStyleBackColor = true;
            this.DoNothingRB.CheckedChanged += new System.EventHandler(this.DoNothingRB_CheckedChanged);
            // 
            // RepeatInReviewsCB
            // 
            this.RepeatInReviewsCB.AutoEllipsis = true;
            this.RepeatInReviewsCB.Location = new System.Drawing.Point(393, 12);
            this.RepeatInReviewsCB.Name = "RepeatInReviewsCB";
            this.RepeatInReviewsCB.Size = new System.Drawing.Size(230, 35);
            this.RepeatInReviewsCB.TabIndex = 2;
            this.RepeatInReviewsCB.Text = "Répeter la même actions dans les autres reviews?";
            this.RepeatInReviewsCB.UseVisualStyleBackColor = true;
            // 
            // StopButton
            // 
            this.StopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.StopButton.Location = new System.Drawing.Point(548, 77);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 3;
            this.StopButton.Text = "Annuler";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // FinishButton
            // 
            this.FinishButton.Location = new System.Drawing.Point(393, 77);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(75, 23);
            this.FinishButton.TabIndex = 4;
            this.FinishButton.Text = "Continuer";
            this.FinishButton.UseVisualStyleBackColor = true;
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // ComparisonGB
            // 
            this.ComparisonGB.Controls.Add(this.FinalTextRTB);
            this.ComparisonGB.Controls.Add(this.ReplacementTextLb);
            this.ComparisonGB.Controls.Add(this.DeletedTextLb);
            this.ComparisonGB.Controls.Add(this.ModifiedTextRTB);
            this.ComparisonGB.Controls.Add(this.OriginalTextRTB);
            this.ComparisonGB.Controls.Add(this.ModifiedTextLb);
            this.ComparisonGB.Controls.Add(this.OrginalTextLb);
            this.ComparisonGB.Location = new System.Drawing.Point(13, 153);
            this.ComparisonGB.Name = "ComparisonGB";
            this.ComparisonGB.Size = new System.Drawing.Size(991, 569);
            this.ComparisonGB.TabIndex = 5;
            this.ComparisonGB.TabStop = false;
            this.ComparisonGB.Text = "Comparaison";
            // 
            // FinalTextRTB
            // 
            this.FinalTextRTB.Location = new System.Drawing.Point(191, 36);
            this.FinalTextRTB.Name = "FinalTextRTB";
            this.FinalTextRTB.Size = new System.Drawing.Size(601, 510);
            this.FinalTextRTB.TabIndex = 6;
            this.FinalTextRTB.Text = "";
            // 
            // ReplacementTextLb
            // 
            this.ReplacementTextLb.AutoSize = true;
            this.ReplacementTextLb.BackColor = System.Drawing.Color.Green;
            this.ReplacementTextLb.Location = new System.Drawing.Point(511, 549);
            this.ReplacementTextLb.Name = "ReplacementTextLb";
            this.ReplacementTextLb.Size = new System.Drawing.Size(112, 13);
            this.ReplacementTextLb.TabIndex = 5;
            this.ReplacementTextLb.Text = "Text de remplacement";
            // 
            // DeletedTextLb
            // 
            this.DeletedTextLb.AutoSize = true;
            this.DeletedTextLb.BackColor = System.Drawing.Color.Red;
            this.DeletedTextLb.Location = new System.Drawing.Point(6, 549);
            this.DeletedTextLb.Name = "DeletedTextLb";
            this.DeletedTextLb.Size = new System.Drawing.Size(70, 13);
            this.DeletedTextLb.TabIndex = 4;
            this.DeletedTextLb.Text = "Text suppimé";
            // 
            // ModifiedTextRTB
            // 
            this.ModifiedTextRTB.Location = new System.Drawing.Point(514, 49);
            this.ModifiedTextRTB.Name = "ModifiedTextRTB";
            this.ModifiedTextRTB.Size = new System.Drawing.Size(471, 497);
            this.ModifiedTextRTB.TabIndex = 3;
            this.ModifiedTextRTB.Text = "";
            // 
            // OriginalTextRTB
            // 
            this.OriginalTextRTB.Location = new System.Drawing.Point(7, 49);
            this.OriginalTextRTB.Name = "OriginalTextRTB";
            this.OriginalTextRTB.Size = new System.Drawing.Size(480, 497);
            this.OriginalTextRTB.TabIndex = 2;
            this.OriginalTextRTB.Text = "";
            // 
            // ModifiedTextLb
            // 
            this.ModifiedTextLb.AutoSize = true;
            this.ModifiedTextLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModifiedTextLb.Location = new System.Drawing.Point(740, 20);
            this.ModifiedTextLb.Name = "ModifiedTextLb";
            this.ModifiedTextLb.Size = new System.Drawing.Size(102, 16);
            this.ModifiedTextLb.TabIndex = 1;
            this.ModifiedTextLb.Text = "Texte modifié";
            // 
            // OrginalTextLb
            // 
            this.OrginalTextLb.AutoSize = true;
            this.OrginalTextLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrginalTextLb.Location = new System.Drawing.Point(143, 20);
            this.OrginalTextLb.Name = "OrginalTextLb";
            this.OrginalTextLb.Size = new System.Drawing.Size(112, 16);
            this.OrginalTextLb.TabIndex = 0;
            this.OrginalTextLb.Text = "Texte d\'origine";
            // 
            // CountInfos
            // 
            this.CountInfos.AutoSize = true;
            this.CountInfos.Location = new System.Drawing.Point(394, 134);
            this.CountInfos.Name = "CountInfos";
            this.CountInfos.Size = new System.Drawing.Size(35, 13);
            this.CountInfos.TabIndex = 6;
            this.CountInfos.Text = "label5";
            // 
            // ReviewCleaningForm
            // 
            this.AcceptButton = this.FinishButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 734);
            this.Controls.Add(this.CountInfos);
            this.Controls.Add(this.ComparisonGB);
            this.Controls.Add(this.FinishButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.RepeatInReviewsCB);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ReviewCleaningForm";
            this.Text = "Essai";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReviewCleaningForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ComparisonGB.ResumeLayout(false);
            this.ComparisonGB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label IdLabel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label reviewerLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton DeleteMatchRB;
        private System.Windows.Forms.RadioButton KeepContentRB;
        private System.Windows.Forms.RadioButton DoNothingRB;
        private System.Windows.Forms.RadioButton DeleteToNextRB;
        private System.Windows.Forms.CheckBox RepeatInReviewsCB;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button FinishButton;
        private System.Windows.Forms.GroupBox ComparisonGB;
        private System.Windows.Forms.Label ModifiedTextLb;
        private System.Windows.Forms.Label OrginalTextLb;
        private System.Windows.Forms.RichTextBox ModifiedTextRTB;
        private System.Windows.Forms.RichTextBox OriginalTextRTB;
        private System.Windows.Forms.Label ReplacementTextLb;
        private System.Windows.Forms.Label DeletedTextLb;
        private System.Windows.Forms.Label CountInfos;
        private System.Windows.Forms.RichTextBox FinalTextRTB;
    }
}