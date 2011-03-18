using System;
using System.Linq;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using MetalImpactApp.Properties;
using MIProgram.Core;
using MIProgram.Core.Logging;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.Writers;
using System.Globalization;
using System.Collections.Generic;
using MIProgram.Model;

namespace MetalImpactApp
{
    public partial class Form1 : Form
    {
        //private OperationsManager_Deprecated _operationsManagerDeprecated;
        private IOperationsLauncher _operationLauncher;
        private readonly IFormatProvider _culture = new CultureInfo("fr-FR", true);
        public static IList<Operation> MainOperations = new List<Operation>
        {
            new Operation(OperationType.PublishDiffDeezerListing, "Publier le listing des dernières reviews pour Deezer"),
            new Operation(OperationType.PublishFullDeezerListing, "Publier le listing de toutes les reviews pour Deezer"),
            new Operation(OperationType.PublishMusicStyles, "Publier les styles de musique"),
            new Operation(OperationType.PublishAlbumTypes, "Publier les types d'albums"),
            new Operation(OperationType.PublishAlbumCountries, "Publier les pays d'artistes"),
            new Operation(OperationType.PublishReviewsWithNewModel, "Publier les reviews avec le nouveau modèle"),
            new Operation(OperationType.PublishSiteMap, "Publier le sitemap")
        };

        public Form1()
        {
            Logging.SetInstance(new FileLogger(Constants.LogFilePath));
            InitializeComponent();
            InitializeView();
            MappingConfigurations.Init();
        }

        #region Display

        private void LockApp(bool isLocked)
        {
            SourceFileTB.Enabled = !isLocked;
            StartStopButton.Enabled = !isLocked;
            DestFileRB.Enabled = !isLocked;
            DestFtpRB.Enabled = !isLocked;
            DestFileTB.Enabled = !isLocked;
            DestFTPDirTB.Enabled = !isLocked;
            DestFTPUserTB.Enabled = !isLocked;
            DestFTPPasswordTB.Enabled = !isLocked;
            LastUpdateDateDTP.Enabled = !isLocked;
            FilterOnReviewsIdsCB.Enabled = !isLocked;
            FilterOnReviewersCB.Enabled = !isLocked;
            IdsToFilterTB.Enabled = !isLocked;
            NewReviewersTB.Enabled = !isLocked;
        }

        private void InitializeView()
        {
            //Sources
            DestFileLB.Location = DestFTPDirLB.Location;
            DestFileTB.Location = DestFTPDirTB.Location;
            SourceFileTB.Text = ConfigurationManager.AppSettings["DefaultSourceFile"];
            FilterOnReviewsIdsCB.Checked = false;
            FilterOnReviewersCB.Checked = false;
            NewReviewersTB.Text = string.Empty;

            ReviewsWorkerInfos.Text = string.Empty;

            //Destinations
            DestFileLB.Visible = false;
            DestFileTB.Visible = false;
            DestFileTB.Text = ConfigurationManager.AppSettings["DefaultDestFile"];
            DestFTPDirLB.Visible = false;
            DestFTPDirTB.Visible = false;
            DestFTPDirTB.Text = ConfigurationManager.AppSettings["DefaultDestFTPDirectory"];
            DestFTPUserLB.Visible = false;
            DestFTPUserTB.Visible = false;
            DestFTPUserTB.Text = ConfigurationManager.AppSettings["DefaultDestFTPUser"];
            DestFTPPasswordLB.Visible = false;
            DestFTPPasswordTB.Visible = false;
            DestFTPPasswordTB.Text = ConfigurationManager.AppSettings["DefaultDestFTPPassword"];

            switch (ConfigurationManager.AppSettings["DefaultDest"].ToUpperInvariant())
            {
                case "FTP":
                    DestFtpRB.Checked = true;
                    break;
                case "FILE":
                    DestFileRB.Checked = true;
                    break;
                default:
                    throw new NotImplementedException(string.Format("La valeur '{0}' de la clé de configuration 'DefaultDest' n'est pas reconnue", ConfigurationManager.AppSettings["DefaultDest"]));
            }

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["LastUpdateDate"]))
                LastUpdateDateDTP.Value = DateTime.Now;
            else
                LastUpdateDateDTP.Value = DateTime.Parse(ConfigurationManager.AppSettings["LastUpdateDate"], _culture);

            //Operations
            OperationsListBox.Items.Clear();
            foreach (var op in MainOperations)
            {
                OperationsListBox.Items.Add(op);
            }

            IList<char> checks = new List<char>(ConfigurationManager.AppSettings["DefaultOperations"].ToCharArray());
            for (int i = 0; i < checks.Count && i < OperationsListBox.Items.Count; i++)
            {
                OperationsListBox.SetItemChecked(i, checks[i] == '1');
            }
        }

        private void OnClose()
        {
            //Sources
            ConfigurationManager.AppSettings["DefaultSourceFile"] = SourceFileTB.Text;

            ReviewsWorkerInfos.Text = string.Empty;

            //Destinations
            ConfigurationManager.AppSettings["DefaultDestFile"] = DestFileTB.Text;
            ConfigurationManager.AppSettings["DefaultDestFTPDirectory"] = DestFTPDirTB.Text;
            ConfigurationManager.AppSettings["DefaultDestFTPUser"] = DestFTPUserTB.Text;
            ConfigurationManager.AppSettings["DefaultDestFTPPassword"] = DestFTPPasswordTB.Text;

            if (DestFileRB.Checked)
            {
                ConfigurationManager.AppSettings["DefaultDest"] = "FILE";
            }

            if (DestFtpRB.Checked)
            {
                ConfigurationManager.AppSettings["DefaultDest"] = "FTP";
            }

            ConfigurationManager.AppSettings["LastUpdateDate"] = LastUpdateDateDTP.Value.ToString(_culture);
            var defaultOperations = string.Empty;
            for (int i = 0; i < OperationsListBox.Items.Count; i++)
            {
                defaultOperations += OperationsListBox.GetItemChecked(i) ? "1" : "0";
            }
            ConfigurationManager.AppSettings["DefaultOperations"] = defaultOperations;
        }

        #endregion

        private bool CheckForm()
        {
            if (!DestFtpRB.Checked && !DestFileRB.Checked)
            {
                MessageBox.Show(Resources.SelectADestinationType, Resources.ErrorLabel, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (LastUpdateDateDTP.Value > DateTime.Now || LastUpdateDateDTP.Value == LastUpdateDateDTP.MinDate)
            {
                MessageBox.Show(Resources.GenerationDateMustBeBeforeToday, Resources.ErrorLabel, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (OperationsListBox.CheckedItems.Count == 0)
            {
                MessageBox.Show(Resources.SelectAtLeastOneOperation, Resources.ErrorLabel, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #region Events management

        private void StartStopButton_Click(object sender, EventArgs e)
        {
            LockApp(true);

            if (_operationLauncher != null && _operationLauncher.IsWorking)
            {
                ReviewsBackgroundWorker.CancelAsync();
                ReviewsWorkerInfos.Text = Resources.StopInProgress;
                StartStopButton.Enabled = false;

            }
            else
            {
                if (!CheckForm())
                    return;
                //IMIRecordsProvider provider;
                //CSVFileProvider.TryBuildProvider(SourceFileTB.Text, out provider);

                var filters = new List<Func<Product, bool>>();

                if (FilterOnReviewsIdsCB.Checked)
                {
                    filters.Add(ProductFiltersBuilder.NewFilterOnIds(IdsToFilterTB.Text));
                }
                else
                {
                    var reviewers = ConfigurationManager.AppSettings["Reviewers"];
                    filters.Add(ProductFiltersBuilder.NewFilterOnReviewers(reviewers));
                    if (FilterOnReviewersCB.Checked)
                    {
                        filters.Add(ProductFiltersBuilder.NewFilterOnReviewers(NewReviewersTB.Text));
                    }
                }

                //Output
                IWriter writer;
                if (DestFileRB.Checked)
                    writer = new LocalFileWriter(DestFileTB.Text);
                else if (DestFtpRB.Checked)
                    writer = new FTPFileWriter(DestFTPDirTB.Text, DestFTPUserTB.Text, DestFTPPasswordTB.Text);
                else
                    throw new InvalidOperationException("Impossible de créer le Writer de sortie. vous n'êtes pas supposés passer dans ce code");

                //_operationsManagerDeprecated = new OperationsManager_Deprecated(provider, writer, LastUpdateDateDTP.Value, OperationsListBox.CheckedItems.Cast<Operation>().ToList());
                var reviewProcessorBuilder = new ReviewProcessorBuilder();
                var operations = OperationsListBox.CheckedItems.Cast<Operation>().ToList();
                ReviewProcessor<Album> reviewProcessor = reviewProcessorBuilder.BuildFor<Album>(SourceFileTB.Text, writer, filters, operations);
                _operationLauncher = new AlbumOperationManager(reviewProcessor, OperationsListBox.CheckedItems.Cast<Operation>().ToList(), writer, filtersDefinitions);

                ReviewsBackgroundWorker.RunWorkerAsync();
                StartStopButton.Text = Resources.CancelLabel;
                StartStopButton.Enabled = true;
            }
        }

        #region ReviewsBackgroundWorker events

        private void ReviewsBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            e.Result = _operationLauncher.Process(worker, e);
        }

        private void ReviewsBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            ReviewsWorkerInfos.Text = _operationLauncher.Infos;
        }

        private void ReviewsBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _operationLauncher.EndWork();

            if (e.Error != null)
            {
                ReviewsWorkerInfos.Text = Resources.ErrorOccured + e.Error.Message;
            }
            else if (e.Cancelled)
            {
                ReviewsWorkerInfos.Text = Resources.OperationStopped;
            }
            else
            {
                LastUpdateDateDTP.Value = DateTime.Now;
                ReviewsWorkerInfos.Text = string.Format("Opération terminée ! {0} reviews ont été traitées avec succès ", e.Result);
            }

            LockApp(false);
            StartStopButton.Text = Resources.GenerateLabel;
            StartStopButton.Enabled = true;
            NewReviewersTB.Text = string.Empty;
        }

        #endregion

        #region RadioButtonsEvents

        private void DestRB_CheckedChanged(object sender, EventArgs e)
        {
            var destIsFile = (DestFileRB.Checked) ? true : false;

            DestFileLB.Visible = destIsFile;
            DestFileTB.Visible = destIsFile;
            DestFTPDirLB.Visible = !destIsFile;
            DestFTPDirTB.Visible = !destIsFile;
            DestFTPPasswordLB.Visible = !destIsFile;
            DestFTPPasswordTB.Visible = !destIsFile;
            DestFTPUserLB.Visible = !destIsFile;
            DestFTPUserTB.Visible = !destIsFile;
        }


        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(Resources.DoYouWantToQuitProgram, Resources.CloseQuestion, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
            else
                OnClose();
        }

        #endregion
    }
}
