// <copyright file="MainWindow.xaml.cs" company="costdata GmbH">
//     (C) costcata GmbH 2018
// </copyright>
namespace SampleCostdataIntegration
{
    #region usings

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
        /// integration test class
        /// </summary>
        private Test integrationTest = new Test();

        /// <summary>
        /// excel test
        /// </summary>
        private TestExcel excel = null;

        #endregion

        #region constrruction

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
        /// Handle click event of button start excel
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">event parameter</param>
        private void StartExcel_Click(object sender, RoutedEventArgs e)
        {
            this.excel = this.integrationTest.Excel;
            this.excelText.DataContext = this.excel;
            this.excel.Start();
        }

        #endregion
    }
}
