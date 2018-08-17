using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvoiceMaker
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'invoiceprocessDataSet.reference' table. You can move, or remove it, as needed.
            this.referenceTableAdapter.Fill(this.invoiceprocessDataSet.reference);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            reference reference = new reference();

            int rowindex = dataGridView1.CurrentRow.Index; // here rowindex will get through currentrow property of datagridview.

            reference.firstname = Convert.ToString(dataGridView1.Rows[rowindex].Cells[1].Value);
            reference.lastname = Convert.ToString(dataGridView1.Rows[rowindex].Cells[2].Value);
            reference.ref_number = Convert.ToString(dataGridView1.Rows[rowindex].Cells[3].Value);
            reference.customer = Convert.ToInt16(dataGridView1.Rows[rowindex].Cells[4].Value);

            dc.references.InsertOnSubmit(reference); //InsertOnSubmit queries will automatic call thats the data context class handle it.
            dc.SubmitChanges();
            rowindex = 0;
            DataGridRefresh();


        }

        private void DataGridRefresh()
        {

            DataClasses1DataContext dc = new DataClasses1DataContext();
            reference reference = new reference();

            var query = from q in dc.references
                        select q;

            dataGridView1.DataSource = query;// Attaching the all data with Datagridview

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {

        }
    }
}
