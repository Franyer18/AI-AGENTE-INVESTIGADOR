using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace Agente_Investigador
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GuardarDatos()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
            }
        }

        public void SavePromptAndResponse(string prompt, string response)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                return;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("INSERT INTO OpenAIResults (Prompt, Response, CreatedAt) VALUES (@Prompt, @Response, GETDATE())", conn);
                    cmd.Parameters.AddWithValue("@Prompt", prompt);
                    cmd.Parameters.AddWithValue("@Response", response);
                    int filas = cmd.ExecuteNonQuery();
                    // MessageBox.Show("Filas insertadas: " + filas); // Línea comentada para no mostrar alerta
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en la base de datos: " + ex.Message);
            }
        }
    }
}

