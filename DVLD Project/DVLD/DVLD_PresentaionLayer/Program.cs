using DVLDManagement.Manage_Application_Types;
using DVLDManagement.Manage_Applications;
using DVLDManagement.Manage_LDLApplications.Tests;
using DVLDManagement.Manage_Test_Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagement
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new frmLogin());
        }
    }
}
