using abledoc.Models;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace abledoc.Utility
{
    // Created By: 
    // Created On: 
    public class CommonHelper
    {
        public static string Version = "1.16";
        public static string APPPath = System.Configuration.ConfigurationManager.AppSettings["PysicalPathWeb"];
        public static string FromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"];
        public static string FromEmailPassword = System.Configuration.ConfigurationManager.AppSettings["FromEmailPassword"];
        public static string SMTP_HOST = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];
        public static Int32 SMTP_PORT = Utility.CommonHelper.GetDBInt(System.Configuration.ConfigurationManager.AppSettings["port"]);
        // Created By: 
        // Created On: 
        public static string GetDBString(object value)
        {
            if (value == null || value == DBNull.Value || Convert.ToString(value) == "&nbsp;")
                return "";
            else
                return Convert.ToString(value).Trim();
        }

        public static decimal GetDBDecimal(object value)
        {
            if (value == null || value == DBNull.Value || Convert.ToString(value).Trim() == "")
                return 0;
            else
                return Convert.ToDecimal(value);
        }

        public static DateTime GetDBDate(object value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static bool GetDBBoolean(object value)
        {
            if (value == null || value == DBNull.Value || Convert.ToString(value).Trim() == "")
                return false;
            else
                return Convert.ToBoolean(value);
        }

        public static int GetDBInt(object value)
        {

            if (value == null || value == DBNull.Value || Convert.ToString(value).Trim() == "")
                return 0;
            else
                return Convert.ToInt32(value);
        }


        public static double GetDBDouble(object value)
        {
            if (value == null || value == DBNull.Value || Convert.ToString(value).Trim() == "")
                return 0;
            else
                return Convert.ToDouble(value);
        }

        public static bool IsNumber(string value)
        {
            bool value1 = true;
            foreach (char c in value.ToCharArray())
            {
                value1 = value1 && char.IsDigit(c);
            }
            return value1;
        }
        public static bool IsValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public static string Encrypt(string str)
        {
            try
            {
                byte[] encData_byte = new byte[str.Length];

                encData_byte = System.Text.Encoding.UTF8.GetBytes(str);

                string encodedData = Convert.ToBase64String(encData_byte);

                return encodedData;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public static string Decrypt(string str)
        {
            try
            {

                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();

                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(str);

                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

                char[] decoded_char = new char[charCount];

                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

                string result = new String(decoded_char);

                return result;
            }
            catch (Exception ex)
            {
                //throw new Exception("Error in base64Decode" + ex.Message);
                return string.Empty;
            }
        }

        public static decimal GetDecimal(string strValue)
        {
            decimal value = 0;
            if (decimal.TryParse(strValue, out value))
                return value;
            else
                return 0;
        }

        public static double GetDouble(string strValue)
        {
            double value = 0;
            if (double.TryParse(strValue, out value))
                return value;
            else
                return 0;
        }

        public static string GetDateSuffix(DateTime dtDate)
        {
            string suffix;

            if (new List<int> { 11, 12, 13 }.Contains(dtDate.Day))
            {
                suffix = "th";
            }
            else if (dtDate.Day % 10 == 1)
            {
                suffix = "st";
            }
            else if (dtDate.Day % 10 == 2)
            {
                suffix = "nd";
            }
            else if (dtDate.Day % 10 == 3)
            {
                suffix = "rd";
            }
            else
            {
                suffix = "th";
            }

            return suffix;
        }

        public static string ConvertToJson(DataTable dt)
        {
            if (dt != null)
            {
                string[] StrDc = new string[dt.Columns.Count];
                string HeadStr = string.Empty;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    StrDc[i] = dt.Columns[i].Caption;
                    HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
                }
                HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
                StringBuilder Sb = new StringBuilder();
                Sb.Append("{\"" + dt.TableName + "\" : [");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string TempStr = HeadStr; Sb.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string columnvalue = dt.Rows[i][j].ToString().ToString().Replace("\"", "");
                        TempStr = TempStr.Replace(dt.Columns[j] + j.ToString() + "¾", columnvalue);
                    }
                    Sb.Append(TempStr + "},");
                }
                Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
                Sb.Append("]}");
                return Sb.ToString();
            }
            return "";
        }
        public static string EncryptString(string ClearText)
        {

            byte[] clearTextBytes = Encoding.UTF8.GetBytes(ClearText);

            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();

            MemoryStream ms = new MemoryStream();
            byte[] rgbIV = Encoding.ASCII.GetBytes("ryojvlzmdalyglrj");
            byte[] key = Encoding.ASCII.GetBytes("hcxilkqbbhczfeultgbskdmaunivmfuo");
            CryptoStream cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIV),
            CryptoStreamMode.Write);

            cs.Write(clearTextBytes, 0, clearTextBytes.Length);

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptString(string EncryptedText)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(EncryptedText);

            MemoryStream ms = new MemoryStream();

            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();


            byte[] rgbIV = Encoding.ASCII.GetBytes("ryojvlzmdalyglrj");
            byte[] key = Encoding.ASCII.GetBytes("hcxilkqbbhczfeultgbskdmaunivmfuo");

            CryptoStream cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIV),
            CryptoStreamMode.Write);

            cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());

        }

        // Create By : 
        //Created Date : 29/1/2016
        public static bool SendMail(string sFrom, string sTo, string sSubject, string sBody, string CC, string BCC)
        {
            bool retval = false;
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient smtpserver = new SmtpClient();
                SmtpClient smtp = new SmtpClient();
                smtp.Host = SMTP_HOST;
                smtp.Port = SMTP_PORT;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(FromEmail, FromEmailPassword);
                // MAIL MESSAGE CONFIGURATION
                mail.From = new MailAddress(sFrom);
                mail.Subject = sSubject;
                mail.IsBodyHtml = true;
                mail.Body = sBody;
                mail.To.Add(sTo);
                if (CC != "")
                {
                    string[] ccMails = CC.Split(',');
                    if (ccMails.Length > 0)
                    {
                        for (int i = 0; i < ccMails.Length; i++)
                        {
                            mail.CC.Add(new MailAddress(ccMails[i].ToString()));
                        }
                    }
                }
                if (BCC != "")
                {
                    string[] bccMails = BCC.Split(',');
                    if (bccMails.Length > 0)
                    {
                        for (int i = 0; i < bccMails.Length; i++)
                        {
                            mail.Bcc.Add(new MailAddress(bccMails[i].ToString()));
                        }
                    }
                }
                //var attachment = new Attachment(filename);
                //mail.Attachments.Add(attachment);
                // SEND EMAIL
                smtp.Send(mail);
                retval = true;

            }
            catch (Exception)
            {
                retval = false;
            }

            return retval;
        }

        public static double RoundedOffHour(double num)
        {
            var floorNum = Math.Floor(num);
            double afterDecimal = num - floorNum;

            afterDecimal = Math.Round(afterDecimal, 2);

            double validHour = 0;
            if (afterDecimal == 0.00)
            {
                validHour = floorNum;
            }
            else if (afterDecimal >= 0.01 && afterDecimal < 0.13)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.13 && afterDecimal < 0.37)
            {
                validHour = floorNum + 0.25;
            }
            else if (afterDecimal >= 0.37 && afterDecimal < 0.63)
            {
                validHour = floorNum + 0.50;
            }
            else if (afterDecimal >= 0.63 && afterDecimal < 0.87)
            {
                validHour = floorNum + 0.75;
            }
            else
            {
                validHour = floorNum + 1;
            }

            return validHour;
        }

        public static bool MakeFolder(string JobID)
        {
            bool retval = false;
            string subPath = Path.Combine(
                  Directory.GetCurrentDirectory(), "wwwroot", "ERP", JobID);

            bool exists = System.IO.Directory.Exists(subPath);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(subPath);
                retval = true;
            }
            return retval;
        }

        public static string Nl2br(string str)
        {
            return Nl2br(str, true);
        }

        public static string Nl2br(string str, bool isXHTML)
        {
            string brTag = "<br>";
            if (isXHTML)
            {
                brTag = "<br />";
            }
            return str.Replace("\r\n", brTag + "\r\n");
        }

        public static Dictionary<string, string> PDFMetadata(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                Dictionary<string, string> metadic = new Dictionary<string, string>();
                metadic = reader.Info;
                metadic.Add("Pages", Utility.CommonHelper.GetDBString(reader.NumberOfPages));

                return metadic;
            }
        }
        public static long ConvertToTimestamp(DateTime value)
        {
            TimeZoneInfo NYTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime NyTime = TimeZoneInfo.ConvertTime(value, NYTimeZone);
            TimeZone localZone = TimeZone.CurrentTimeZone;
            System.Globalization.DaylightTime dst = localZone.GetDaylightChanges(NyTime.Year);
            NyTime = NyTime.AddHours(-1);
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            TimeSpan span = (NyTime - epoch);
            return (long)Convert.ToDouble(span.TotalSeconds);
        }

        public static string TimerCompareStateServer(int tStart, int tStop)
        {
            int secondsDifference = tStop - tStart;
            int totalSeconds = secondsDifference;
            string realClockTimer = SecondsToTimeFormat(totalSeconds);
            return realClockTimer;
        }

        public static string SecondsToTimeFormat(int seconds)
        {
            string timeInFormat = "";
            // Get the remianing seconds
            decimal remSec = seconds % 60;
            decimal remSecDiv = seconds / 60;

            decimal remMin = remSecDiv % 60;
            decimal remMinDiv = remSecDiv / 60;

            decimal remHour = remMinDiv;

            remSec = Utility.CommonHelper.GetDBInt(remSec);
            remMin = Utility.CommonHelper.GetDBInt(remMin);
            remHour = Utility.CommonHelper.GetDBInt(remHour);
            return Utility.CommonHelper.GetDBString(remHour) + ":" + Utility.CommonHelper.GetDBString(remMin) + ":" + Utility.CommonHelper.GetDBString(remSec);
        }

        public static int TimeFormatToSeconds(string timeFormat)
        {
            string[] timeFormatExplode = timeFormat.Split(":");
            int hours = Utility.CommonHelper.GetDBInt(timeFormatExplode[0]);
            int minutes = Utility.CommonHelper.GetDBInt(timeFormatExplode[1]);
            int seconds = Utility.CommonHelper.GetDBInt(timeFormatExplode[2]);

            int totalSeconds = (hours * 60 * 60) + (minutes * 60) + seconds;

            return totalSeconds;
        }

        public static string TimerServerCompare(string fileID, string phase,string databasename)
        {
            int Id = 0;
            JobsFilesCheckouts objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.FileID = Utility.CommonHelper.GetDBInt(fileID);
            objJobsFilesCheckouts.State = phase;
            objJobsFilesCheckouts.databasename = databasename;
            objJobsFilesCheckouts.GetJobsFilesCheckoutsByFileIDState();
            Id = objJobsFilesCheckouts.ID;
            var format = "yyyy-MM-dd H:mm:ss";
            var stringDate = DateTime.Now.ToString(format);
            int datetime2 = Utility.CommonHelper.GetDBInt(Utility.CommonHelper.ConvertToTimestamp(DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture)));


            int secondsDifference = datetime2 - objJobsFilesCheckouts.TimerCompare;

            // Add these seconds to the old time seconds
            int oldTimerSeconds = TimeFormatToSeconds(objJobsFilesCheckouts.Timer);

            int totalSeconds = oldTimerSeconds + secondsDifference;

            string realClockTimer = SecondsToTimeFormat(totalSeconds);
            objJobsFilesCheckouts = new JobsFilesCheckouts();
            objJobsFilesCheckouts.ID = Id;
            objJobsFilesCheckouts.Timer = realClockTimer;
            objJobsFilesCheckouts.TimerCompare = Utility.CommonHelper.GetDBInt(Utility.CommonHelper.ConvertToTimestamp(DateTime.ParseExact(stringDate, format, CultureInfo.InvariantCulture)));
            objJobsFilesCheckouts.databasename = databasename;
            objJobsFilesCheckouts.UpdateJobsFilesCheckOutTimerTimerCompareById();
            return realClockTimer;
        }

        public static string GetDateDifference(DateTime strt_date, DateTime end_date)
        {
            try
            {
                strt_date = DateTime.Now;
                end_date = Convert.ToDateTime("10/1/2017 23:59:59");
                //DateTime add_days = end_date.AddDays(1);
                TimeSpan nod = (end_date - strt_date);
                return Convert.ToString(nod);
            }
            catch
            {
                return null;
            }
        }

        public static int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }

        public static string Getabledocs(string flag)
        {
            if (flag == "1")
            {
                return Models.Utility.Constants.ABLEDOCS_EU_DB;
            }
            else {
                return Models.Utility.Constants.ABLEDOCS_DB;
            }
        }

        public static string Getlegacy(string flag)
        {
            if (flag == "1")
            {
                return Models.Utility.Constants.LEGACY_EU_DB;
            }
            else
            {
                return Models.Utility.Constants.LEGACY_DB;
            }
        }

        public static string Getadscan(string flag)
        {
            if (flag == "1")
            {
                return Models.Utility.Constants.ADSCAN_EU_DB;
            }
            else
            {
                return Models.Utility.Constants.ADSCAN_DB;
            }
        }

        public static string Getgateway(string flag)
        {
            if (flag == "1")
            {
                return Models.Utility.Constants.GATEWAY_EU_DB;
            }
            else
            {
                return Models.Utility.Constants.GATEWAY_DB;
            }
        }

        public static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        //public static AssignedMenu GetPageAccess(string menuName)
        //{
        //    if (Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu") != null && Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu").Any(x => x.menuname.ToLower() == menuName.ToLower()))
        //        return Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu").FirstOrDefault(x => x.menuname.ToLower() == menuName.ToLower());
        //    else
        //        return new AssignedMenu();
        //}

        public class MyCustomDateProvider : IFormatProvider, ICustomFormatter
        {
            public object GetFormat(Type formatType)
            {
                if (formatType == typeof(ICustomFormatter))
                    return this;

                return null;
            }

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (!(arg is DateTime)) throw new NotSupportedException();

                var dt = (DateTime)arg;

                string suffix;

                if (new[] { 11, 12, 13 }.Contains(dt.Day))
                {
                    suffix = "th";
                }
                else if (dt.Day % 10 == 1)
                {
                    suffix = "st";
                }
                else if (dt.Day % 10 == 2)
                {
                    suffix = "nd";
                }
                else if (dt.Day % 10 == 3)
                {
                    suffix = "rd";
                }
                else
                {
                    suffix = "th";
                }

                return string.Format("{0:dddd}, {0:MMM} {1}{2} '{0:yy}", arg, dt.Day, suffix);
            }


        }

    }
}
