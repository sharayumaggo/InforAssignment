using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

namespace Infor_Assignment.Controllers
{
    public class CLSGetData
    {
        public DataTable GetDataSourceFromFile(string fileName)
        {
            //DataTable dt = new DataTable("CreditCards");
            string[] columns = null;


            //Regex parser = new Regex(@"\s+");

            // modified code (replace with your file path)
            DataTable result = new DataTable();
            string[] line = File.ReadAllLines(@"C:\Users\varun\downloads\A.txt");

            return result;


            //string readText = File.ReadAllText("path to my file.txt");

            //List<string> listStrLineElements = List<string> listStrLineElements = line.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();// You need using System.Linq at the top.
            //List<string> rowList = listStrLineElements.SelectMany(s => s.Split('\t')).ToList();// The \t is an *escape character* meaning tab.


            //var lines = File.ReadAllLines(fileName);

            //// assuming the first row contains the columns information
            //if (lines.Count() > 0)
            //{
            //    columns = lines[0].Split(new char[] { ',' });

            //    foreach (var column in columns)
            //        dt.Columns.Add(column);
            //}

            //// reading rest of the data
            //for (int i = 1; i < lines.Count(); i++)
            //{
            //    DataRow dr = dt.NewRow();
            //    string[] values = lines[i].Split(new char[] { ',' });

            //    for (int j = 0; j < values.Count() && j < columns.Count(); j++)
            //        dr[j] = values[j];

            //    dt.Rows.Add(dr);
            //}
        }
    }

    public class FileUploadController : Controller
    {
        // GET: /file/
        public ActionResult Index()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult upload(HttpPostedFileBase file)
        {
            bool result = false;
            StringBuilder strbuild = new StringBuilder();

            string OpenPath, contents;
            int tabSize = 3;
            string[] arInfo;
            string line;

            // regular expression for split row:

            DataTable table1 = new DataTable("table_name");
            table1.Columns.Add("name", typeof(String));
            table1.Columns.Add("id", typeof(Int64));
            table1.Columns.Add("place", typeof(String));

                //start reading the textfile
            StreamReader reader = new StreamReader(@"C:\Users\varun\downloads\A.txt", Encoding.UTF8);
            //string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                //make sure it has 3 items
                if (items.Length == 3)
                {
                    DataRow row1 = table1.NewRow();
                    row1["name"] = items[0];
                    row1["id"] = Int64.Parse(items[1]);
                    row1["place"] = items[2];
                    table1.Rows.Add(row1);
                }
            }
            reader.Close();
            reader.Dispose();

            // make use of the table
            table1.Rows.Count.ToString();

            // when use is done dispose it
            table1.Dispose();



            // Create new DataTable.

            CLSGetData objnew = new CLSGetData();

            objnew.GetDataSourceFromFile(file.FileName);

            DataTable table = CreateTable();
            DataRow row;

            try
            {
                if (file.ContentLength == 0)
                    throw new Exception("Zero length file!");
                else
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Document"), fileName);
                    file.SaveAs(filePath);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        int curLine = 0;
                        string fileType = "";
                        StreamReader objStreamReader;
                        objStreamReader = System.IO.File.OpenText(filePath);

                        while ((line = objStreamReader.ReadLine()) != null)
                        {
                                curLine++;
                            if(curLine==1)
                            {
                               
                            }
                            else
                            {
                                ArrayList charDataList = new ArrayList();
                                for (int i=0; i<line.Length; i++)
                                {
                                    charDataList.Add(line[i].ToString());
                                }
                                //string name = line.Split('\t')[0];
                                //    string test = line.Split('\t')[1];
                                //    string desc = line.Split('\t')[2];

                                ArrayList BookName = charDataList.GetRange(0, 28);

                                //ArrayList ISBN = charDataList.GetRange(28, 46);
                                //ArrayList Author = charDataList.GetRange(46, 61);

                                string BookNameResult = BookName.ToString();
                            }


                        }
                        objStreamReader.Close();


                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return new JsonResult { Data = table };
        }


        


        private DataTable CreateTable()
        {
            try
            {
                DataTable table = new DataTable();

                // Declare DataColumn and DataRow variables.
                DataColumn column;

                // Create new DataColumn, set DataType, ColumnName
                // and add to DataTable.    
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "Name";
                table.Columns.Add(column);

                // Create second column.
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "ISBN";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "Author";
                table.Columns.Add(column);


                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

