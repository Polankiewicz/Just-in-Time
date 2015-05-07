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
        int index;
        XmlConverter<List<StaticModel>> x;
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
         
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        public void SetComboBoxSource(List<string> list,Scene scene)
        {
            comboBox1.DataSource = list;
            this.scene = scene;

        }
        public EditorForm()
        {
           //  dataGridView1.CellContentClick+= new System.EventHandler(dataGridView1_CellContentClick)
            index = 0;
            InitializeComponent();
            dataGridView1.KeyDown += dataGridView1_KeyDown;
            x = new XmlConverter<List<StaticModel>>();
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
            if (e.KeyCode == Keys.Enter )
            {
                e.Handled = true;
                DataGridViewCell cell = dataGridView1.Rows[0].Cells[0];
                dataGridView1.CurrentCell = cell;
                dataGridView1.BeginEdit(true);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            
                dataGridView1.CurrentCell = cell;
                index = e.RowIndex;
            
                dataGridView1.BeginEdit(true);
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           // blist[index].Position = float.Parse(pos_x.Text);

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            scene.AddStaticModel(comboBox1.Text, new Microsoft.Xna.Framework.Vector3(0), new Microsoft.Xna.Framework.Vector3(0), 1, "Nowy");
            this.SetDataSource(scene.staticModelsList);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            x.Serialize("../../../scene.xml", scene.staticModelsList);
        }
    }
}
