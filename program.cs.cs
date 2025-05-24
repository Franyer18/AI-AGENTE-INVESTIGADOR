using System;
using System.Windows.Forms;

namespace Agente_Investigador
{
    internal static class ProgramSecundario 
    {
        /// <summary>
       
        /// </summary>
        [STAThread]
        static void MainSecundario() // Cambié el nombre del método
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}