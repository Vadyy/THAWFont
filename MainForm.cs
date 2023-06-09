using System.Windows.Forms;

namespace THAWFontWinForm
{
    public partial class MainForm : Form
    {
        private string selectedFilePath;

        public MainForm()
        {
            InitializeComponent();

            comboBoxGameVersion.Items.Add("Tony Hawk's American Wasteland");
            comboBoxGameVersion.Items.Add("Tony Hawk's Underground 2");

            comboBoxGameVersion.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void OpenFont_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "FNT Files (*.fnt; *.fnt.wpc; *.fnt.xbx)|*.fnt;*.fnt.wpc;*.fnt.xbx|All Files (*.*)|*.*";
                openFileDialog.Title = "Select FNT File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName;
                    string selectedGameVersion = comboBoxGameVersion.SelectedItem.ToString();

                    FontDecoder reader = new FontDecoder();

                    Bitmap? img = null;

                    // Process the selected file based on the game version
                    if (selectedGameVersion == "Tony Hawk's American Wasteland")
                    {
                        img = reader.ReadTHAW(selectedFilePath).Bitmap;

                    }
                    else if (selectedGameVersion == "Tony Hawk's Underground 2")
                    {

                        img = reader.ReadTHUG2(selectedFilePath).Bitmap;
                    }


                    if (img != null)
                    {
                        pictureBoxFont.Image?.Dispose(); // Dispose the old image if it exists
                        pictureBoxFont.Image = img;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"The file was not found: '{selectedFilePath}'");
            }
            catch (IOException)
            {
                MessageBox.Show("An I/O error occurred while opening the file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void convertToThawButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogThug = new OpenFileDialog();
            openFileDialogThug.Filter = "FNT Files (*.fnt; *.fnt.xbx)|*.fnt;*.fnt.xbx|All Files (*.*)|*.*";
            openFileDialogThug.Title = "Select THUG2 FNT File";

            if (openFileDialogThug.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            OpenFileDialog openFileDialogThaw = new OpenFileDialog();
            openFileDialogThaw.Filter = "FNT Files (*.fnt; *.fnt.wpc)|*.fnt;*.fnt.wpc|All Files (*.*)|*.*";
            openFileDialogThaw.Title = "Select THAW FNT File";

            if (openFileDialogThaw.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            FontDecoder reader = new FontDecoder();

            try
            {
                /*reader.ConvertTHUG2toTHAW(openFileDialogThug.FileName, openFileDialogThaw.FileName, "C:\\Users\\Vady\\Documents\\Programming\\Reverse Engineering\\THAW\\Converted\\test.fnt.wpc");
                MessageBox.Show("Converted succesfully");*/
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "WPC Font Files (*.fnt.wpc)|*.fnt.wpc";
                    saveFileDialog.Title = "Save Converted File";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string destinationFile = saveFileDialog.FileName;
                        reader.ConvertTHUG2toTHAW(openFileDialogThug.FileName, openFileDialogThaw.FileName, destinationFile);
                        MessageBox.Show("Conversion completed successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}