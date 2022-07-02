using abledoc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace abledoc.Utility
{
    public class EmailHelper
    {

        public static Attachment GetAttachFile(byte[] data, string fileName)
        {
            MemoryStream ms = new MemoryStream(data);
            var attachment = new Attachment(ms, fileName, GetMimeTypeByFileName(fileName));
            return attachment;
        }

        public static string GetMimeTypeByFileName(string sFileName)
        {
            string sMime = "application/octet-stream";

            string sExtension = System.IO.Path.GetExtension(sFileName);
            if (!string.IsNullOrEmpty(sExtension))
            {
                sExtension = sExtension.Replace(".", "");
                sExtension = sExtension.ToLower();

                if (sExtension == "xls" || sExtension == "xlsx")
                {
                    sMime = "application/ms-excel";
                }
                else if (sExtension == "doc" || sExtension == "docx")
                {
                    sMime = "application/msword";
                }
                else if (sExtension == "ppt" || sExtension == "pptx")
                {
                    sMime = "application/ms-powerpoint";
                }
                else if (sExtension == "rtf")
                {
                    sMime = "application/rtf";
                }
                else if (sExtension == "zip")
                {
                    sMime = "application/zip";
                }
                else if (sExtension == "mp3")
                {
                    sMime = "audio/mpeg";
                }
                else if (sExtension == "bmp")
                {
                    sMime = "image/bmp";
                }
                else if (sExtension == "gif")
                {
                    sMime = "image/gif";
                }
                else if (sExtension == "jpg" || sExtension == "jpeg")
                {
                    sMime = "image/jpeg";
                }
                else if (sExtension == "png")
                {
                    sMime = "image/png";
                }
                else if (sExtension == "tiff" || sExtension == "tif")
                {
                    sMime = "image/tiff";
                }
                else if (sExtension == "txt")
                {
                    sMime = "text/plain";
                }
                else if (sExtension == "pdf")
                {
                    sMime = "application/pdf";
                }
            }

            return sMime;
        }

        public static void Email(string Country, string sTo, string ccEmail, string bccEmail, string Subject, string message, Attachment attach)
        {

            if (!string.IsNullOrWhiteSpace(message))
            {
                EmailHelper.SendMailAttach(Country, sTo, Subject, message, ccEmail, bccEmail, true, attach);
            }
        }

        public static string SendMailAttach(string Country, string sTo, string sSubject, string sBody, string CC, string BCC, bool attach, Attachment attachment)
        {
            string retval = "";
            try
            {
                string FromEmail = string.Empty;
                string FromEmailPassword = string.Empty;
                string SMTP_HOST = string.Empty;
                Int32 SMTP_PORT = 0;

                Countries objCounntry = new Countries();
                objCounntry.code = Country;
                objCounntry = objCounntry.GetCountryByCode();

                if (objCounntry != null)
                {
                    FromEmail = objCounntry.emailaddress;
                    FromEmailPassword = objCounntry.password;
                    SMTP_HOST = objCounntry.smtphost;
                    SMTP_PORT = Utility.CommonHelper.GetDBInt(objCounntry.smtpport);
                }
                else
                {
                    objCounntry = new Countries();
                    objCounntry = objCounntry.GetCountryByDefaultSMTP();

                    if (objCounntry != null)
                    {
                        FromEmail = objCounntry.emailaddress;
                        FromEmailPassword = objCounntry.password;
                        SMTP_HOST = objCounntry.smtphost;
                        SMTP_PORT = Utility.CommonHelper.GetDBInt(objCounntry.smtpport);
                    }
                }



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
                mail.From = new MailAddress(FromEmail);
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
                if (attach)
                {

                    mail.Attachments.Add(attachment);
                }

                // SEND EMAIL
                smtp.Send(mail);
                retval = "send";

            }
            catch (Exception ex)
            {
                retval = ex.Message;
            }

            return retval;
        }

        public static void EmailThis(string sTo, string ccEmail, string bccEmail, string Subject, string message,string FromEmail, bool attach, Attachment attachment)
        {

            if (!string.IsNullOrWhiteSpace(message))
            {
                EmailHelper.SendMailAttachThis(sTo, Subject, message, ccEmail, bccEmail, FromEmail, attach, attachment);
            }
        }

        public static string SendMailAttachThis(string sTo, string sSubject, string sBody, string CC, string BCC, string FromEmail, bool attach, Attachment attachment)
        {
            string retval = "";
            try
            {
                if (FromEmail == "") {FromEmail = "info@abledocs.com"; }
                
                if (sBody == "") { sBody = sSubject; }

                //string FromEmail = string.Empty;
                string FromEmailPassword = string.Empty;
                string SMTP_HOST = "mail.smtp2go.com";//string.Empty;
                Int32 SMTP_PORT = 587;
                string Username = "abledocs";
                string Password = "M2llOHFkaHFwaWMw";

                
                MailMessage mail = new MailMessage();
                SmtpClient smtpserver = new SmtpClient();
                SmtpClient smtp = new SmtpClient();
                smtp.Host = SMTP_HOST;
                smtp.Port = SMTP_PORT;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Username, Password);
                // MAIL MESSAGE CONFIGURATION
                mail.From = new MailAddress(FromEmail);
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
                if (attach)
                {

                    mail.Attachments.Add(attachment);
                }


                // SEND EMAIL
                // TEMPORARY DISABLE SENDING EMAIL
                //smtp.Send(mail);
                retval = "send";

            }
            catch (Exception ex)
            {
                retval = ex.Message;
            }

            return retval;
        }


    }
}
