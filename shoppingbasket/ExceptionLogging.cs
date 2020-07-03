using System;
using System.IO;

namespace shoppingbasket
{
    public static class ExceptionLogging
    {

        /// <summary>
        /// Method is responsible for log the exception
        /// </summary>
        /// <param name="ex"></param>
        public static void SendErrorToText(Exception ex)
        {
            string errorMsg = ex.GetType().Name.ToString();
            string exType = ex.GetType().ToString();
            string ErrorLocation = ex.Message.ToString();

            try
            {
                string filepath ="ExceptionDetailsFile/";  //Text File Path

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + "Error Message:" + " " + errorMsg + "Exception Type:" + " " + exType ;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}
