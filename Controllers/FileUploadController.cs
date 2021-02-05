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
    public class FileUploadController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: /file/
        public ActionResult Index()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult upload(HttpPostedFileBase file)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowelement;
            StringBuilder strbuild = new StringBuilder();
            string line;
            DataTable table1 = new DataTable("Book");
            table1.Columns.Add("Name", typeof(String));
            table1.Columns.Add("ISBN", typeof(Int64));
            table1.Columns.Add("Author", typeof(String));

            try
            {
                // Logging framework using Adapter Pattern 
                log.Info("entering application, fetch list!");

                if (file.ContentLength == 0)
                {
                    log.Info("Zero lenght file!");

                    throw new Exception("Zero length file!");
                }
                else
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Document"), fileName);
                    file.SaveAs(filePath);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        //start reading the textfile
                        log.Info("Reading File!");

                        StreamReader reader = new StreamReader(filePath, Encoding.UTF8);
                        //string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] items = line.Split('\t').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                            //make sure it has 3 items
                            if (items.Length == 3)
                            {
                                DataRow row1 = table1.NewRow();
                                row1["Name"] = items[0];
                                row1["ISBN"] = Int64.Parse(items[1]);
                                row1["Author"] = items[2];
                                table1.Rows.Add(row1);
                            }
                        }
                        reader.Close();
                        reader.Dispose();

                        if (table1.Rows.Count > 0) //if data is there in dataTable add it to dictionary
                        {
                            log.Info("Book List added to datatable!");

                            foreach (DataRow dr in table1.Rows)
                            {
                                rowelement = new Dictionary<string, object>();
                                foreach (DataColumn col in table1.Columns)
                                {
                                    rowelement.Add(col.ColumnName, dr[col]); //adding columnn  
                                }
                                rows.Add(rowelement);
                            }
                        }
                        //// when use is done dispose it
                        table1.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return null;
            }
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}

