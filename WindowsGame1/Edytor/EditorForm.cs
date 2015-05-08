using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsGame1;

namespace Editor
{
    public partial class EditorForm : Form
    {
        BindingList<StaticModel> blist;
        Scene scene;

        XmlConverter<List<SceneSaveData>> x;
        public IntPtr CanvasHandle
        {
            get { return pictureBox1.Handle; }
        }

        public Size ViewportSize
        {
            get { return pictureBox1.Size; }
        }


        public void AddNewModel()
        {

        }
        public void SetDataSource(List<StaticModel> list)
        {
            blist = new BindingList<StaticModel>(list);
            blist.AllowEdit = true;

            var source = new BindingSource(blist, null);

            dataGridView1.RowHeadersVisible = false;

            dataGridView1.DataSource = source;
            dataGridView1.Columns["path"].ReadOnly = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        public void SetComboBoxSource(List<string> list, Scene scene)
        {
            comboBox1.DataSource = list;
            this.scene = scene;

        }
        public EditorForm()
        {

            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            x = new XmlConverter<List<SceneSaveData>>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                DataGridViewCell cell = dataGridView1.Rows[0].Cells[0];
                dataGridView1.CurrentCell = cell;
                dataGridView1.BeginEdit(true);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {



        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            scene.AddStaticModel(comboBox1.Text, new Microsoft.Xna.Framework.Vector3(0), new Microsoft.Xna.Framework.Vector3(0), 1, comboBox1.Text);
            this.SetDataSource(scene.staticModelsList);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            string file = "";
            List<SceneSaveData> dataList = new List<SceneSaveData>();
            saveFileDialog1.InitialDirectory = "../../../../";
            saveFileDialog1.FileName = "scene.xml";
            saveFileDialog1.Filter = "Sceny w XML (*.xml)|*.xml|Wszystkie pliki (*.*)|*.*";
            saveFileDialog1.ShowDialog(); // Test result.
            file = saveFileDialog1.FileName;
            
            foreach (StaticModel n in scene.staticModelsList)
            {
                dataList.Add(new SceneSaveData(n.path, n.Name, n.Scale, n.Position, n.Rotation));
            }

            x.Serialize(file, dataList);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            string file = "";
            List<SceneSaveData> dataList = new List<SceneSaveData>();
            openFileDialog1.InitialDirectory = "../../../../";
            openFileDialog1.FileName = "scene.xml";
            openFileDialog1.Filter = "Sceny w XML (*.xml)|*.xml|Wszystkie pliki (*.*)|*.*";
            openFileDialog1.ShowDialog(); // Test result.
            file = openFileDialog1.FileName;
            dataList = x.Deserialize(file);

            scene.staticModelsList = new List<StaticModel>();
            foreach (SceneSaveData n in dataList)
                scene.AddStaticModel(n.path, n.Position, n.Rotation, n.Scale, n.Name);

            this.SetDataSource(scene.staticModelsList);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void EditorForm_ResizeEnd(object sender, EventArgs e)
        {
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
