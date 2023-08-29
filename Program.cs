using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageToBase64
{
    public partial class MainForm : Form
    {
        private DataGridView dataGridView;

        public MainForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            DownloadAndDisplayImage();
        }

        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            Controls.Add(dataGridView);
        }

        private void InitializeDataGridView()
        {
            dataGridView.Columns.Add("Type", "Tipo");
            dataGridView.Columns.Add("Name", "Nome");
            dataGridView.Columns.Add("Base64Snippet", "Base64 (trecho)");

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private async void DownloadAndDisplayImage()
        {
            string imageUrl = ""; // <== URL da imagem aqui
            string imagePath = "logo.png";
            string base64FilePath = "logo_base64.txt";

            using (HttpClient client = new HttpClient())
            {
                byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);
                File.WriteAllBytes(imagePath, imageBytes);
                Console.WriteLine("Imagem baixada e salva.");

                // Lê a imagem e converte para Base64
                byte[] base64Bytes = File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(base64Bytes);
                File.WriteAllText(base64FilePath, base64String);
                Console.WriteLine("Imagem convertida para Base64 e salva em arquivo.");

                // Adiciona informações ao DataGridView
                dataGridView.Rows.Add("Imagem", Path.GetFileName(imagePath), base64String.Substring(0, 50) + "...");
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
