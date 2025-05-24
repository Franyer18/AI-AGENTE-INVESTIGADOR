using System;
using System.Windows.Forms;

namespace Agente_Investigador
{
    public partial class Form1 : Form
    {
        private OpenAIService _openAIService = new OpenAIService("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");
        private DatabaseService _dbService = new DatabaseService("Server=DESKTOP-VF4FHTO\\SQLEXPRESS;Database=AgenteInvestigadorDB;Trusted_Connection=True;TrustServerCertificate=True;"
  );

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnInvestigar_Click(object sender, EventArgs e)
        {
            lblEstado.Text = "Consultando IA...";
            try
            {
                string prompt = txtPrompt.Text;
                string resultado = await _openAIService.GetOpenAIResponseAsync(prompt);
                txtResultado.Text = resultado;
                lblEstado.Text = "Consulta completada.";
                _dbService.SavePromptAndResponse(prompt, resultado);
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error en la consulta: " + ex.Message;
            }
        }

        private void btnGenerarDocumentos_Click(object sender, EventArgs e)
        {
            lblEstado.Text = "Generando documentos...";
            try
            {
                string prompt = txtPrompt.Text;
                string resultado = txtResultado.Text;
                string carpeta = System.IO.Path.Combine(
                    Application.StartupPath,
                    $"Investigacion_{DateTime.Now:yyyyMMdd_HHmmss}"
                );

              
                string archivoWord = WordGenerator.GenerarWord(prompt, resultado, carpeta);

                string archivoPpt = PowerPointGenerator.GenerarPowerPoint(prompt, resultado, carpeta);

                lblEstado.Text = "Documentos generados en: " + carpeta;
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error al generar documentos: " + ex.Message;
            }
        }
    }
}



