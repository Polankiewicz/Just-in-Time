using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsGame1;
using Microsoft.Xna.Framework;
namespace Editor
{
    public partial class EditorForm : Form
    {
        BindingList<StaticModel> blist;
        BindingList<DrawableBoundingBox> boxlist;
        Scene scene;
       
        
        public IntPtr CanvasHandle
        {
            get { return pictureBox1.Handle; }
        }

        public Size ViewportSize
        {
            get { return pictureBox1.Size; }
        }

        public void SetDataSource(List<StaticModel> list)
        {
            blist = new BindingList<StaticModel>(list);
            blist.AllowEdit = true;

            var source = new BindingSource(blist, null);

            dataGridView1.RowHeadersVisible = false;

            dataGridView1.DataSource = source;
            dataGridView1.Columns["path"].ReadOnly = true;
           
        }
        public void SetBoundingBoxDataSource(List<DrawableBoundingBox> list)
        {
            boxlist = new BindingList<DrawableBoundingBox>(list);
            boxlist.AllowEdit = true;

            var sourc = new BindingSource(boxlist, null);

            dataGridView2.RowHeadersVisible = false;

            dataGridView2.DataSource = sourc;
            //dataGridView1.Columns["path"].ReadOnly = true;

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
            dataGridView1.EditMode = DataGridViewEditMode.EditOnF2;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.EditMode = DataGridViewEditMode.EditOnF2;



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
            XmlConverter<SceneSaveDataNew> x = new XmlConverter<SceneSaveDataNew>();
            string file = "";
            SceneSaveDataNew dataToSave = new SceneSaveDataNew();
          
            saveFileDialog1.InitialDirectory = "../../../../";
            saveFileDialog1.FileName = "scene.xml";
            saveFileDialog1.Filter = "Sceny w XML (*.xml)|*.xml|Wszystkie pliki (*.*)|*.*";
            saveFileDialog1.ShowDialog(); // Test result.
            file = saveFileDialog1.FileName;
            
            foreach (StaticModel n in scene.staticModelsList)
            {
                dataToSave.AddModel(n.path, n.Name, n.Scale, n.Position, n.Rotation);
            }
            foreach (DrawableBoundingBox n in scene.boundingBoxesList)
            {
                dataToSave.AddBoundingBox(n.min, n.max);
            }
            x.Serialize(file, dataToSave);
        }
        private void LoadOld()
        {
            XmlConverter<List<SceneSaveData>> x = new XmlConverter<List<SceneSaveData>>();
            string file = "";
            List<SceneSaveData> dataList = new List<SceneSaveData>();
            openFileDialog1.InitialDirectory = "../../../../";
            openFileDialog1.FileName = "scene.xml";
            openFileDialog1.Filter = "Sceny w XML (*.xml)|*.xml|Wszystkie pliki (*.*)|*.*";
            openFileDialog1.ShowDialog(); // Test result.
            file = openFileDialog1.FileName;
            dataList = x.Deserialize(file);

            scene.staticModelsList = new List<StaticModel>();
            scene.boundingBoxesList = new List<DrawableBoundingBox>();
            foreach (SceneSaveData n in dataList)
                scene.AddStaticModel(n.path, n.Position, n.Rotation, n.Scale, n.Name);

            this.SetDataSource(scene.staticModelsList);
            this.SetBoundingBoxDataSource(scene.boundingBoxesList);
        }
        private void LoadNew()
        {
            XmlConverter<SceneSaveDataNew> x = new XmlConverter<SceneSaveDataNew>();
            string file = "";
            SceneSaveDataNew dataList = new SceneSaveDataNew();
            openFileDialog1.InitialDirectory = "../../../../";
            openFileDialog1.FileName = "scene.xml";
            openFileDialog1.Filter = "Sceny w XML (*.xml)|*.xml|Wszystkie pliki (*.*)|*.*";
            openFileDialog1.ShowDialog(); // Test result.
            file = openFileDialog1.FileName;
            dataList = x.Deserialize(file);

            scene.staticModelsList = new List<StaticModel>();
            scene.boundingBoxesList = new List<DrawableBoundingBox>();
            foreach (SceneSaveData n in dataList.modelsList)
                scene.AddStaticModel(n.path, n.Position, n.Rotation, n.Scale, n.Name);

            foreach (BoundingBoxSaveData n in dataList.boundingBoxesList)
                scene.AddBoundingBox(n.min,n.max);

            this.SetDataSource(scene.staticModelsList);
            this.SetBoundingBoxDataSource(scene.boundingBoxesList);
        }
        private void LoadButton_Click(object sender, EventArgs e)
        {
            LoadNew();
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

        private void dataGridView1_KeyDown_1(object sender, KeyEventArgs e)
        {
                if (e.KeyCode == Keys.Escape)
                {
                    dataGridView1.EndEdit();
                    dataGridView1.ClearSelection();
                }
        }

        private void AddBoudningBoxButton_Click(object sender, EventArgs e)
        {
            scene.AddBoundingBox(new Vector3(0), new Vector3(10));
            this.SetBoundingBoxDataSource(scene.boundingBoxesList);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadOldButton_Click(object sender, EventArgs e)
        {
            LoadOld();
        }
    }
}
