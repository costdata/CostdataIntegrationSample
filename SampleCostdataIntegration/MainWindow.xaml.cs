// <copyright file="MainWindow.xaml.cs" company="costdata GmbH">
//     (C) costcata GmbH 2018
// </copyright>
namespace SampleCostdataIntegration
{
    #region usings

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    using Costdata.Integration;

    #endregion

    /// <summary>
    /// class main window
    /// </summary>
    public partial class MainWindow : Window
    {
        #region fields

        /// <summary>
        /// costdata integration class
        /// </summary>
        private CostdataImportData costdata = null;

        #endregion

        #region construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region handle events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event parameter</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.costdata = new CostdataImportData();
            this.costdata.ImportCanceled = this.ImportCanceled;
            this.costdata.ImportWage = this.ImportWage;
            this.costdata.ImportSalary = this.ImportSalary;
            this.UpdateView();
        }


        /// <summary>
        /// Handle click event of install costdata start excel
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event parameter</param>
        private async void InstallCostdata_Click(object sender, RoutedEventArgs e)
        {
            this.installCostdata.IsEnabled = false;
            this.importText.Text = string.Empty;
            this.importText.TextAlignment = TextAlignment.Right;

            try
            {
                await this.costdata.Install(this.user.Text, this.password.Password, 60);
            }
            finally
            {
                this.installCostdata.IsEnabled = true;
            }

            this.UpdateView();
        }

        private async void StartCostdata_Click(object sender, RoutedEventArgs e)
        {
            this.startCostdata.IsEnabled = false;
            this.importText.Text = string.Empty;
            this.importText.TextAlignment = TextAlignment.Right;

            try
            {
                await this.costdata.Start(60);
            }
            finally
            {
                this.startCostdata.IsEnabled = true;
            }

            this.UpdateView();
        }

        private void CheckLicense_Click(object sender, RoutedEventArgs e)
        {
            this.checkLicenze.IsEnabled = false;

            try
            {
                this.costdata.CheckLicense();
            }
            finally
            {
                this.checkLicenze.IsEnabled = true;
            }

            this.UpdateView();
        }

        private void ImportWage_Click(object sender, RoutedEventArgs e)
        {
            this.importWage.IsEnabled = false;
            this.import.Text = string.Empty;
            this.importText.Text = string.Empty;
            this.importSalary.IsEnabled = false;

            try
            {
                this.costdata.ViewWage();
            }
            finally
            {
                this.importWage.IsEnabled = true;
                this.importSalary.IsEnabled = true;
            }

            this.UpdateView();
        }

        private void ImportSalary_Click(object sender, RoutedEventArgs e)
        {
            this.importSalary.IsEnabled = false;
            this.import.Text = string.Empty;
            this.importText.Text = string.Empty;
            this.importWage.IsEnabled = false;

            try
            {
                this.costdata.ViewSalary();
            }
            finally
            {
                this.importSalary.IsEnabled = true;
                this.importWage.IsEnabled = true;
            }

            this.UpdateView();
        }


        #endregion

        #region call back

        private async Task ImportWage(CostdataWage wage)
        {
            await this.DispatchAction(() =>
            {
                this.import.Text = "Wage";
                this.importText.Text = wage.WageInEuro.ToString("N2");
                this.postImport.Text = "€";
                this.importSalary.IsEnabled = true;
                this.importWage.IsEnabled = true;
            });
        }

        private async Task ImportSalary(CostdataSalary salary)
        {
            await this.DispatchAction(() =>
            {
                this.import.Text = "Salary";
                this.importText.Text = salary.SalaryInEuro.ToString("N2");
                this.postImport.Text = "€";
                this.importSalary.IsEnabled = true;
                this.importWage.IsEnabled = true;
            });
        }

        private async Task ImportCanceled()
        {
            await this.DispatchAction(() =>
            {
                this.import.Text = string.Empty;
                this.importText.Text = string.Empty;
                this.postImport.Text = string.Empty;
                this.importSalary.IsEnabled = true;
                this.importWage.IsEnabled = true;
            });
        }

        #endregion

        #region private methods

        /// <summary>
        /// Dispatch action
        /// </summary>
        /// <param name="action">action to dispatch</param>
        private async Task DispatchAction(Action action)
        {
            try
            {
                if (Dispatcher.Thread == Thread.CurrentThread)
                {
                    action();
                }
                else
                {
                    await this.Dispatcher.InvokeAsync(action);
                }
            }
            catch
            {
            }
        }

        private void UpdateView()
        {
            if (this.costdata.IsInstallationRequired)
            {
                this.start.Visibility = Visibility.Visible;
                this.labelUser.Visibility = Visibility.Visible;
                this.user.Visibility = Visibility.Visible;
                this.labelPassword.Visibility = Visibility.Visible;
                this.password.Visibility = Visibility.Visible;
                this.installCostdata.Visibility = Visibility.Visible;
                this.installCostdata.IsEnabled = true;
                if (this.costdata.InstallationFailed != null)
                {
                    this.importText.Text = this.costdata.InstallationFailed.Message;
                    this.importText.TextAlignment = TextAlignment.Left;
                    this.postImport.Text = string.Empty;
                }
                else
                {
                    this.importText.Text = string.Empty;
                    this.importText.TextAlignment = TextAlignment.Right;
                    this.postImport.Text = string.Empty;
                }

                this.info.Visibility = Visibility.Collapsed;
                this.buttons.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.labelUser.Visibility = Visibility.Collapsed;
                this.user.Visibility = Visibility.Collapsed;
                this.labelPassword.Visibility = Visibility.Collapsed;
                this.password.Visibility = Visibility.Collapsed;
                this.installCostdata.Visibility = Visibility.Collapsed;

                if (this.costdata.IsStartRequired)
                {
                    this.start.Visibility = Visibility.Visible;
                    this.startCostdata.Visibility = Visibility.Visible;
                    this.info.Visibility = Visibility.Collapsed;
                    this.buttons.Visibility = Visibility.Collapsed;
                    if (this.costdata.StartFailed != null)
                    {
                        this.importText.Text = this.costdata.StartFailed.Message;
                        this.importText.TextAlignment = TextAlignment.Left;
                        this.postImport.Text = string.Empty;
                    }
                    else
                    {
                        this.importText.Text = string.Empty;
                        this.importText.TextAlignment = TextAlignment.Right;
                        this.postImport.Text = string.Empty;
                    }
                }
                else
                {
                    this.start.Visibility = Visibility.Collapsed;
                    this.startCostdata.Visibility = Visibility.Collapsed;
                    this.info.Visibility = Visibility.Visible;
                    this.remoteVersion.Text = $"Version {this.costdata.Version}";
                    if (!this.costdata.IsLicenseValid)
                    {
                        this.buttons.Visibility = Visibility.Collapsed;
                        this.checkLicenze.Visibility = Visibility.Visible;
                        this.importText.Text = string.Empty;
                        this.importText.TextAlignment = TextAlignment.Right;
                        this.postImport.Text = string.Empty;
                    }
                    else
                    {
                        this.checkLicenze.Visibility = Visibility.Collapsed;
                        this.buttons.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        #endregion
    }
}
