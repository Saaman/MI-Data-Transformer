using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MetalImpactApp.Properties;
using MIProgram.Core.BodyCleaning;
using MIProgram.Core;
using System.Text.RegularExpressions;

namespace MetalImpactApp
{
    public partial class ReviewCleaningForm : Form
    {
        private readonly ReviewCleaningFormResult _reviewCleaningFormResult;
        private readonly IList<string> _matchedStrings;
        private readonly string _originalText;
        private string _currentText;
        private int _reviewId;
        private bool _allowCloseWithoutWarning;

        private int _currentMatchedStringIdx;

        private bool IsFinalStep { get { return _currentMatchedStringIdx == _matchedStrings.Count; } }

        public ReviewCleaningForm(ref ReviewCleaningFormResult reviewCleaningFormResult, RemovalsPresenter presenter)
        {
            _reviewCleaningFormResult = reviewCleaningFormResult;
            _originalText = presenter.ReviewText;
            _matchedStrings = presenter.MatchedStrings.Values;

            InitializeComponent();
            InitializeFromPresenter(presenter);
            ResetForm();
        }

        private void ResetForm()
        {
            _currentMatchedStringIdx = 0;
            _reviewCleaningFormResult.CleanRemovals();
            _currentText = _originalText;
            OriginalTextRTB.Text = _currentText;
            ModifiedTextRTB.Text = _currentText;
            SetFormStep();
        }

        private void InitializeFromPresenter(RemovalsPresenter presenter)
        {
            _reviewId = presenter.ReviewId;
            Text = string.Format("Review {0} - {1}", presenter.ReviewId, presenter.Title);
            IdLabel.Text = string.Format("Review d'ID : {0}", presenter.ReviewId);
            IdLabel.AutoEllipsis = true;
            TitleLabel.Text = string.Format("Titre de la review : {0}", presenter.Title);
            TitleLabel.AutoEllipsis = true;
            reviewerLabel.Text = string.Format("chroniqué par {0} le {1}", presenter.ReviewerName, presenter.CreatedDate);
            reviewerLabel.AutoEllipsis = true;
            OriginalTextRTB.ReadOnly = true;
            ModifiedTextRTB.ReadOnly = true;
            FinalTextRTB.ReadOnly = false;
        }

        private void SetFormStep()
        {
            DoNothingRB.Checked = true;

            if (IsFinalStep)
            {
                FinishButton.Text = Resources.TerminateLabel;
                CountInfos.Text = string.Format("Vous pouvez modifier manuellement le texte avant de valider définitivement vos modifications");
            }
            else
            {
                FinishButton.Text = Resources.ContinueLabel;
                CountInfos.Text = string.Format("Il reste {0} changements à effectuer", _matchedStrings.Count - _currentMatchedStringIdx);
            }

            DoNothingRB.Enabled = !IsFinalStep;
            KeepContentRB.Enabled = !IsFinalStep;
            DeleteMatchRB.Enabled = !IsFinalStep;
            DeleteToNextRB.Enabled = !IsFinalStep;
            RepeatInReviewsCB.Enabled = !IsFinalStep;
            OriginalTextRTB.Visible = !IsFinalStep;
            ModifiedTextRTB.Visible = !IsFinalStep;
            FinalTextRTB.Visible = IsFinalStep;
            OrginalTextLb.Visible = !IsFinalStep;
            ModifiedTextLb.Visible = !IsFinalStep;
            DeletedTextLb.Visible = !IsFinalStep;
            ReplacementTextLb.Visible = !IsFinalStep;
            RepeatInReviewsCB.Checked = true;
        }

        #region replacements

        private void ResetRTBs()
        {
            OriginalTextRTB.Text = _currentText;
            ModifiedTextRTB.Text = _currentText;

            OriginalTextRTB.SelectAll();
            OriginalTextRTB.SelectionBackColor = Color.Transparent;
            OriginalTextRTB.DeselectAll();
            ModifiedTextRTB.SelectAll();
            ModifiedTextRTB.SelectionBackColor = Color.Transparent;
            ModifiedTextRTB.DeselectAll();
        }

        private static void HiglightText(RichTextBox rtb, string textToHighlight, Color color)
        {
            var startPos = rtb.Text.IndexOf(textToHighlight);

            rtb.Find(textToHighlight, RichTextBoxFinds.MatchCase);
            rtb.SelectionBackColor = color;
            rtb.DeselectAll();
            rtb.SelectionStart = startPos;
            rtb.ScrollToCaret();
        }

        private static void ReplaceText(RichTextBox rtb, string textToDelete, string textToReplace)
        {
            var startPos = rtb.Text.IndexOf(textToDelete);

            rtb.Find(textToDelete, RichTextBoxFinds.MatchCase);
            rtb.Text = rtb.Text.Replace(textToDelete, textToReplace);
            rtb.DeselectAll();
            startPos = Math.Min(startPos, rtb.Text.Length-1);
            if (textToReplace.Length == 0)
            {
                if (rtb.Text[startPos] == ' ')
                {
                    rtb.SelectionStart = startPos;
                    rtb.SelectionLength = 1;
                }
                
                if (rtb.Text[startPos-1] == ' ')
                {
                    rtb.SelectionStart = startPos -1;
                    rtb.SelectionLength = 1;
                }
            }
            else
            {
                rtb.SelectionStart = startPos;
                rtb.SelectionLength = textToReplace.Length;
            }
            rtb.SelectionBackColor = Color.Green;
            rtb.DeselectAll();
            rtb.SelectionStart = startPos;
            rtb.ScrollToCaret();
        }

        private void DoNothingRB_CheckedChanged(object sender, EventArgs e)
        {
            if (!DoNothingRB.Checked || IsFinalStep)
            {
                RepeatInReviewsCB.Enabled = true;
                return;
            }

            RepeatInReviewsCB.Checked = false;
            RepeatInReviewsCB.Enabled = false;
            ResetRTBs();
            HiglightText(OriginalTextRTB, _matchedStrings[_currentMatchedStringIdx], Color.SlateGray);
            HiglightText(ModifiedTextRTB, _matchedStrings[_currentMatchedStringIdx], Color.SlateGray);
        }

        private void DeleteMatchRB_CheckedChanged(object sender, EventArgs e)
        {
            if (!DeleteMatchRB.Checked || _currentMatchedStringIdx == _matchedStrings.Count)
            {
                return;
            }

            ResetRTBs();

            HiglightText(OriginalTextRTB, _matchedStrings[_currentMatchedStringIdx], Color.Red);
            ReplaceText(ModifiedTextRTB, _matchedStrings[_currentMatchedStringIdx], string.Empty);
        }

        private void KeepContentRB_CheckedChanged(object sender, EventArgs e)
        {
            if (!KeepContentRB.Checked || _currentMatchedStringIdx == _matchedStrings.Count)
            {
                return;
            }

            ResetRTBs();

            var contentRegex = new Regex(@"^.*(<.+>).*$");
            var replacementString = _matchedStrings[_currentMatchedStringIdx];
            while (contentRegex.IsMatch(replacementString))
            {
                replacementString = replacementString.Replace((contentRegex.Match(replacementString).Groups[1]).ToString(), string.Empty);
            }

            HiglightText(OriginalTextRTB, _matchedStrings[_currentMatchedStringIdx], Color.Red);
            ReplaceText(ModifiedTextRTB, _matchedStrings[_currentMatchedStringIdx], replacementString);

        }

        private void DeleteToNextRB_CheckedChanged(object sender, EventArgs e)
        {
            if (!DeleteToNextRB.Checked || _currentMatchedStringIdx == _matchedStrings.Count)
            {
                return;
            }

            ResetRTBs();

            var textToDelete = OriginalTextRTB.Text.Substring(OriginalTextRTB.Text.IndexOf(_matchedStrings[_currentMatchedStringIdx]));
            if(_currentMatchedStringIdx < _matchedStrings.Count -1)
                textToDelete = textToDelete.Remove(textToDelete.IndexOf(_matchedStrings[_currentMatchedStringIdx + 1]));

            HiglightText(OriginalTextRTB, textToDelete, Color.Red);
            ReplaceText(ModifiedTextRTB, textToDelete, string.Empty);
        }

        #endregion 

        
        #region Global mecanism

        private void FinishButton_Click(object sender, EventArgs e)
        {
            if (IsFinalStep)
            {
                if (_currentText != FinalTextRTB.Text)
                {
                    if (_currentText.Contains(FinalTextRTB.Text))
                    {
                        var startIndex = _currentText.IndexOf(FinalTextRTB.Text);
                        if(startIndex > 0)
                            _reviewCleaningFormResult.AddRemoval(_reviewId, _currentText.Substring(0, startIndex), string.Empty, false);
                        _reviewCleaningFormResult.AddRemoval(_reviewId, _currentText.Substring(startIndex + FinalTextRTB.Text.Length), string.Empty, false);
                    }
                    else
                    {
                        _reviewCleaningFormResult.CleanRemovals();
                        _reviewCleaningFormResult.AddRemoval(_reviewId, _originalText, FinalTextRTB.Text, false);
                    }
                }
                _allowCloseWithoutWarning = true;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            if (!KeepContentRB.Checked && !DoNothingRB.Checked && !DeleteToNextRB.Checked && !DeleteMatchRB.Checked)
            {
                MessageBox.Show(Resources.SelectAnActionToPerform, Resources.ErrorLabel, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (KeepContentRB.Checked)
            {
                var contentRegex = new Regex(@"^.*(<.+>).*$");
                var replacementString = _matchedStrings[_currentMatchedStringIdx];
                while (contentRegex.IsMatch(replacementString))
                {
                    replacementString = replacementString.Replace((contentRegex.Match(replacementString).Groups[1]).ToString(), string.Empty);
                }

                _reviewCleaningFormResult.AddRemoval(_reviewId, _matchedStrings[_currentMatchedStringIdx], replacementString, RepeatInReviewsCB.Checked);
                _currentText = _currentText.Replace(_matchedStrings[_currentMatchedStringIdx], replacementString);
            }
            else if (DeleteMatchRB.Checked)
            {
                _reviewCleaningFormResult.AddRemoval(_reviewId, _matchedStrings[_currentMatchedStringIdx], string.Empty, RepeatInReviewsCB.Checked);
                _currentText = _currentText.Replace(_matchedStrings[_currentMatchedStringIdx], string.Empty);
            }
            else if (DeleteToNextRB.Checked)
            {
                var textToDelete = OriginalTextRTB.Text.Substring(OriginalTextRTB.Text.IndexOf(_matchedStrings[_currentMatchedStringIdx]));
                if (_currentMatchedStringIdx < _matchedStrings.Count - 1)
                    textToDelete = textToDelete.Remove(textToDelete.IndexOf(_matchedStrings[_currentMatchedStringIdx + 1]));

                _reviewCleaningFormResult.AddRemoval(_reviewId, textToDelete, string.Empty, RepeatInReviewsCB.Checked);
                _currentText = _currentText.Replace(textToDelete, string.Empty);
            }
            else if (DoNothingRB.Checked)
            {
                _reviewCleaningFormResult.AddRemoval(_reviewId, _matchedStrings[_currentMatchedStringIdx], _matchedStrings[_currentMatchedStringIdx], RepeatInReviewsCB.Checked);
            }

            OriginalTextRTB.Text = _currentText;
            ModifiedTextRTB.Text = _currentText;
            FinalTextRTB.Text = _currentText;
            _currentMatchedStringIdx++;
            SetFormStep();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(null, Resources.CancelAllModificationsAndRestart, Resources.CancellationLabel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                ResetForm();
            }
        }

        private void ReviewCleaningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!_allowCloseWithoutWarning)
                {
                    var res = MessageBox.Show(null, Resources.CancelAllOperationsAndQuit, Resources.CancellationLabel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (res == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        DialogResult = DialogResult.Cancel;
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }

        }

        #endregion
    }
}
