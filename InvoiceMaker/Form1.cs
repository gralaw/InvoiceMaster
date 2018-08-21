using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Globalization;


namespace InvoiceMaker
{
    public partial class Form1 : Form
    {
        List<InvoiceLine> callList = new List<InvoiceLine>();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void FileExplorerBtn_Click(object sender, EventArgs e)
        {

            if (OpencsvFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = OpencsvFileDialog.FileName;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            writeMessage("Processing Started");
            processCSVtoDatabase(OpencsvFileDialog.FileName);
            populateDatabase();
            countMinutes();
            string commdate = weekCommencingDate(OpencsvFileDialog.FileName);
            constructExcelOutput(commdate);
            writeMessage("Processing Complete");
        }

        private void processCSVtoDatabase(string inputFilePath)
        {
            TextReader textReader = new StreamReader(inputFilePath);
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<InputLine>();            

            int count = 0;
            progressBar1.Maximum = 1400;
            DataClasses1DataContext db = new DataClasses1DataContext();
            foreach (var record in records)
            {

                InvoiceLine line = new InvoiceLine();
                // Work out how many mins this call is for
                var startTime = DateTime.ParseExact(record.time_from, "H:mm", null, System.Globalization.DateTimeStyles.None);
                var endTime = DateTime.ParseExact(record.time_to, "H:mm", null, System.Globalization.DateTimeStyles.None);
                TimeSpan span = endTime.Subtract(startTime);

                line.firstname = record.client_fname;
                line.lastname = record.client_sname;
                //line.weekCommencing = master_weekcommencing.ToShortDateString();
                
                line.minutesThisCall = (int)span.TotalMinutes;
                line.providerID = "1136762";
                var query =
                    (from c in db.references
                     where c.firstname == line.firstname && c.lastname == line.lastname
                     select new { c.ref_number, c.customer }).SingleOrDefault();
                if (query == null)
                {
                    string errorString = "Error: " + line.firstname + " " + line.lastname + " does not have an entry in the reference database table.";
                    writeMessage(errorString);
                }
                else if (query.customer == 0)
                {
                    line.asdReference = query.ref_number;
                    callList.Add(line);
                }

                count++;
                progressBar1.PerformStep();

            }
            writeMessage(count.ToString() + " Items processed from csv file");
            textReader.Close();
            db.Dispose();
        }

        private void populateDatabase()
        {
            writeMessage("Populating Database");
            using (DataClasses1DataContext dc = new DataClasses1DataContext())
            {
                dc.ExecuteCommand("TRUNCATE TABLE invoicelines");
                foreach (var line in callList)
                {
                    invoiceline objref = new invoiceline();
                    objref.firstname = line.firstname;
                    objref.lastname = line.lastname;
                    objref.ref_number = line.asdReference;
                    objref.hoursthisperiod = 0;
                    dc.invoicelines.InsertOnSubmit(objref);
                    dc.SubmitChanges();
                }
                // remove duplicate entries
                var duplicates = (from r in dc.invoicelines
                                  group r by new { r.firstname, r.lastname } into results
                                  select results.Skip(1)
                 ).SelectMany(a => a);

                dc.invoicelines.DeleteAllOnSubmit(duplicates);
                dc.SubmitChanges();
            }
        }

        private void countMinutes()
        {
            writeMessage("Counting Minutes");
            using (DataClasses1DataContext dc = new DataClasses1DataContext())
            {
                IQueryable<invoiceline> invoicelines =
                from invoiceline in dc.invoicelines
                select invoiceline;

                foreach (var line in invoicelines)
                {

                    foreach (var entry in callList)
                    {
                        if (line.firstname == entry.firstname && line.lastname == entry.lastname)
                        {
                            line.hoursthisperiod += entry.minutesThisCall;
                        }
                    }
                    line.hoursthisperiod = decimal.Truncate((decimal)line.hoursthisperiod / 15) * .25m;
                }
                dc.SubmitChanges();
            }

        }
        public string weekCommencingDate(string inputFilePath)
        {
            DateTime master_weekcommencing = DateTime.Now;
            TextReader textReader = new StreamReader(inputFilePath);

            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<InputLine>();

            // is this weekcommencing the base date?
            writeMessage("Scanning for week commencing date.");

            foreach (var record in records)
            {
                var date = DateTime.Parse(record.booking_date, new CultureInfo("en-GB", true));
                if (date < master_weekcommencing)
                {
                    master_weekcommencing = date;
                }
            }
            textReader.Close();
            writeMessage("Week Commencing Date is " + master_weekcommencing.ToShortDateString());
            return master_weekcommencing.ToShortDateString(); 
        }

        private void constructExcelOutput(string wkCommencing)
        {
            writeMessage("Constructing Excel Workbook");
            Excel.Application xlApp = new
            Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "Provider ID";
            xlWorkSheet.Cells[1, 2] = "Week Commencing";
            xlWorkSheet.Cells[1, 3] = "Swift/AIS ID";
            xlWorkSheet.Cells[1, 4] = "Number of Hours this week";
            xlWorkSheet.Cells[1, 5] = "Comments";
            xlWorkSheet.Cells[1, 6] = "Hospital Admission Date";
            xlWorkSheet.Cells[1, 7] = "Delete";
            xlWorkSheet.Cells[1, 8] = "Delete";
            int row = 2;


            using (DataClasses1DataContext dc = new DataClasses1DataContext())
            {
                IQueryable<invoiceline> invoicelines =
                from invoiceline in dc.invoicelines
                select invoiceline;

                foreach (var line in invoicelines)
                {
                    xlWorkSheet.Cells[row, 1] = "1136762";
                    xlWorkSheet.Cells[row, 2] = "'" + wkCommencing;
                    xlWorkSheet.Cells[row, 3] = line.ref_number;
                    xlWorkSheet.Cells[row, 4] = line.hoursthisperiod;
                    xlWorkSheet.Cells[row, 7] = line.firstname;
                    xlWorkSheet.Cells[row, 8] = line.lastname;
                    row++;
                }

            }
            string savefile = textBox2.Text + "\\PEP_" + wkCommencing.Replace(@"/", string.Empty) + ".xls";
            xlWorkBook.SaveAs(savefile, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            writeMessage("Excel file created. You can find the file at " + savefile);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 reference = new Form2();
            reference.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void writeMessage(string msg)
        {
            DateTime now = DateTime.Now;
            string msgline = now.ToLongTimeString() + "   " + msg;

            if (msg.StartsWith("Error"))
            {
                msgline = now.ToLongTimeString() + " " + msg;
                int length = richTextBox1.TextLength;  // at end of text
                richTextBox1.AppendText(msgline + "\r\n");
                richTextBox1.SelectionStart = length;
                richTextBox1.SelectionLength = msg.Length;
                richTextBox1.SelectionColor = Color.Red;
            }
            else
            {
                richTextBox1.AppendText(msgline + "\r\n");
            }
            richTextBox1.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\InvoiceMaster.uns"))
            {
                System.IO.StreamReader theReader = new System.IO.StreamReader(Application.StartupPath + "\\InvoiceMaster.uns");

                if (theReader.EndOfStream == false)
                {
                    this.textBox2.Text = theReader.ReadLine();
                }

                theReader.Close();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.IO.StreamWriter theWriter = new System.IO.StreamWriter(Application.StartupPath + "\\InvoiceMaster.uns");
            theWriter.Write(textBox2.Text);
            theWriter.Close();
        }
    }
}
