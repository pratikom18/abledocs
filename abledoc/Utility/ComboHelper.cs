using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using abledoc.Models;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace abledoc.Utility
{
    public class ComboHelper
    {
        public static MultiSelectList GetClientsList(string selectedValue = "")
        {
            Jobs objJobMaster = new Jobs();
            List<Jobs> listParentMenu = objJobMaster.GetClientsList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listParentMenu, "Code", "Company");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listParentMenu, "Code", "Company", ints);
            }

        }
        public static MultiSelectList GetCountryCurrencyList(string selectedValue = "")
        {
            Countries objJobMaster = new Countries();
            List<Countries> listCountry = objJobMaster.GetCountryCurrencyList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listCountry, "code", "currencycode");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(listCountry, "code", "currencycode", values);
            }

        }
        public static MultiSelectList GetCountryList(string selectedValue = "")
        {
            Countries objJobMaster = new Countries();
            List<Countries> listCountry = objJobMaster.GetCountryList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listCountry, "code", "country");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(listCountry, "code", "country", values);
            }

        }

        public static MultiSelectList GetCACountryList(string selectedValue = "")
        {
            Countries objJobMaster = new Countries();
            List<Countries> listCountry = objJobMaster.GetCA_CountryList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listCountry, "code", "country");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(listCountry, "code", "country", values);
            }

        }

        public static MultiSelectList GetEUCountryList(string selectedValue = "")
        {
            Countries objJobMaster = new Countries();
            List<Countries> listCountry = objJobMaster.GetEU_CountryList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listCountry, "code", "country");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(listCountry, "code", "country", values);
            }

        }

        public static MultiSelectList GetCountryidList(string selectedValue = "")
        {
            Countries objJobMaster = new Countries();
            List<Countries> listCountry = objJobMaster.GetCountryList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listCountry, "id", "country");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(listCountry, "id", "country", values);
            }

        }

        public static MultiSelectList GetCompanyList(string selectedValue = "", string databasename = "")
        {
            Clients objCompnayMaster = new Clients();
            List<Clients> listCompany = objCompnayMaster.GetCompanyList(databasename);
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listCompany, "ID", "Company");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listCompany, "ID", "Company", ints);
            }

        }
        public static MultiSelectList GetManageCompanyList(string selectedValue = "")
        {
            ManageCompany objComapnyMaster = new ManageCompany();
            List<ManageCompany> listParentMenu = objComapnyMaster.GetManageCompanyList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listParentMenu, "code", "name");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listParentMenu, "code", "name", ints);
            }

        }
        public static MultiSelectList GetLangList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "EN", Text = "English" });
            SizeUnitIn.Add(new SelectListItem { Value = "FR", Text = "French" });
            SizeUnitIn.Add(new SelectListItem { Value = "DA", Text = "Danish" });
            SizeUnitIn.Add(new SelectListItem { Value = "DE", Text = "German" });
            SizeUnitIn.Add(new SelectListItem { Value = "ES", Text = "Spanish" });
            SizeUnitIn.Add(new SelectListItem { Value = "SV", Text = "Swedish" });
            SizeUnitIn.Add(new SelectListItem { Value = "IS", Text = "Icelandic" });
            SizeUnitIn.Add(new SelectListItem { Value = "FI", Text = "Finnish" });
            SizeUnitIn.Add(new SelectListItem { Value = "NN", Text = "Norwegian" });
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }
        public static MultiSelectList GetCrawlStatusList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "new", Text = "New" });
            SizeUnitIn.Add(new SelectListItem { Value = "start", Text = "Start" });
            SizeUnitIn.Add(new SelectListItem { Value = "Crawling", Text = "Crawling" });
            SizeUnitIn.Add(new SelectListItem { Value = "Complete", Text = "Complete" });
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }
        public static MultiSelectList GetAssigUserList(string selectedValue = "")
        {
            Users objUserMaster = new Users();
            
            List<Users> listParentMenu = objUserMaster.GetAssignedUsersList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listParentMenu, "ID", "FullName");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listParentMenu, "ID", "FullName", ints);
            }

        }

        public static MultiSelectList GetOwnerList(string selectedValue = "")
        {
            Users objUserMaster = new Users();
            List<Users> listParentMenu = objUserMaster.GetOwnerList(Convert.ToInt32(selectedValue));
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listParentMenu, "ID", "FullName");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listParentMenu, "ID", "FullName", ints);
            }

        }
        public static MultiSelectList GetJobContactList(string ContactID, string status = "", string selectedValue = "", string loginusercountry = "", string databasename = "")
        {
            ClientsContacts objUserMaster = new ClientsContacts();
            objUserMaster.databasename = databasename;
            List<ClientsContacts> listParentMenu = objUserMaster.GetJobContactList(Convert.ToInt32(ContactID), status);
            List<SelectListItem> list = new List<SelectListItem>();
            if (listParentMenu != null)
            {
                foreach (var data in listParentMenu)
                {
                    string FullName = string.Empty;
                    /*if (loginusercountry != "")
                    {
                        if (loginusercountry != data.Country)
                        {
                            FullName = "XXXXX";
                        }
                        else
                        {
                            FullName = data.FullName.ToString();
                        }
                    }
                    else
                    {
                        FullName = data.FullName.ToString();
                    }*/
                    FullName = data.FullName.ToString();

                    list.Add(new SelectListItem { Value = data.ID.ToString(), Text = FullName });
                }
            }
            else
            {
                list.Add(new SelectListItem { Value = "", Text = "" });
            }

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(list, "Value", "Text");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(list, "Value", "Text", ints);
            }

        }

        public static MultiSelectList GetContactType(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "CC on invoice", Text = "CC on invoice" });
            SizeUnitIn.Add(new SelectListItem { Value = "Secondary billing contact", Text = "Secondary billing contact" });
            SizeUnitIn.Add(new SelectListItem { Value = "Internal File Delivery", Text = "Internal File Delivery" });
            SizeUnitIn.Add(new SelectListItem { Value = "Approver", Text = "Approver" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetPhoneEmailType(string selectedValue = "", string flag = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "Work", Text = "Work" });
            SizeUnitIn.Add(new SelectListItem { Value = "Home", Text = "Home" });
            if (flag == "M")
            {
                SizeUnitIn.Add(new SelectListItem { Value = "Mobile", Text = "Mobile" });
            }

            SizeUnitIn.Add(new SelectListItem { Value = "Other", Text = "Other" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetJobTypeList(string selectedValue = "")
        {
            Jobstype objJobTypeMaster = new Jobstype();
            List<Jobstype> listCompany = objJobTypeMaster.GetJobTypeList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listCompany, "jobtype", "jobtypename");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listCompany, "jobtype", "jobtypename", ints);
            }

        }

        public static MultiSelectList GetJobStatusList(string selectedValue = "")
        {
            Jobsstatus objStatusMaster = new Jobsstatus();
            List<Jobsstatus> listStatus = objStatusMaster.GetJobStatusList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listStatus, "status", "statusname");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listStatus, "status", "statusname", ints);
            }

        }

        public static MultiSelectList GetSupervisorList(string selectedValue = "")
        {
            Users objUsers = new Users();
            List<Users> listSupervisor = objUsers.GetSupervisorList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listSupervisor, "ID", "FullName");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listSupervisor, "ID", "FullName", ints);
            }

        }

        public static MultiSelectList GetUserRoleList(string selectedValue = "")
        {
            UserRoles objUserRoles = new UserRoles();
            List<UserRoles> listUserRoles = objUserRoles.GetUserRolesList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listUserRoles, "RoleId", "RoleName");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listUserRoles, "RoleId", "RoleName", ints);
            }

        }
        public static MultiSelectList GetParentMenuList(string selectedValue = "")
        {
            MenuMaster objMenuMaster = new MenuMaster();
            List<MenuMaster> listParentMenu = objMenuMaster.GetParentMenuList();
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listParentMenu, "MenuMasterId", "MenuName");
            else
            {
                int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
                return new MultiSelectList(listParentMenu, "MenuMasterId", "MenuName", ints);
            }

        }

        //public static MultiSelectList GetCommonMasterList(string type,string selectedValue = "")
        //{
        //    CommonMaster objCommonMaster = new CommonMaster();
        //    List<CommonMaster> list = objCommonMaster.GetCommonMasterList(type);
        //    if (string.IsNullOrWhiteSpace(selectedValue))
        //        return new SelectList(list, "typename", "typename");
        //    else
        //    {
        //        int[] ints = selectedValue.Split(',').Select(int.Parse).ToArray();
        //        return new MultiSelectList(list, "typename", "typename", ints);
        //    }

        //}
        public static SelectList GetCommonMasterList(string type, string selectedValue = "")
        {
            List<CommonMaster> list = new CommonMaster().GetCommonMasterList(type);
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(list, "typecode", "typename");
            else
                return new SelectList(list, "typecode", "typename", selectedValue);
        }

        public static MultiSelectList GetStatusAdmin(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "TAGGING", Text = "Phase 1" });
            SizeUnitIn.Add(new SelectListItem { Value = "REVIEW", Text = "Phase 2" });
            //SizeUnitIn.Add(new SelectListItem { Value = "FINAL", Text = "Phase 4" });
            SizeUnitIn.Add(new SelectListItem { Value = "QC", Text = "Phase 3" });
            SizeUnitIn.Add(new SelectListItem { Value = "TOBEDELIVERED", Text = "To Be Delivered" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetStatusTagger(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "TAGGING", Text = "Phase 1" });
            SizeUnitIn.Add(new SelectListItem { Value = "REVIEW", Text = "Phase 2" });


            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetStatusReviewer(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "TAGGING", Text = "Phase 1" });
            SizeUnitIn.Add(new SelectListItem { Value = "REVIEW", Text = "Phase 2" });
            //SizeUnitIn.Add(new SelectListItem { Value = "FINAL", Text = "Phase 4" });


            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetStatusFinalizer(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            //SizeUnitIn.Add(new SelectListItem { Value = "FINAL", Text = "Phase 4" });
            SizeUnitIn.Add(new SelectListItem { Value = "QC", Text = "Phase 3" });


            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }
        public static MultiSelectList GetStatusQC(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            //SizeUnitIn.Add(new SelectListItem { Value = "FINAL", Text = "Phase 4" });
            SizeUnitIn.Add(new SelectListItem { Value = "QC", Text = "Phase 3" });
            SizeUnitIn.Add(new SelectListItem { Value = "TOBEDELIVERED", Text = "To Be Delivered" });


            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetFileStatus(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "PENDING", Text = "Pending" });
            SizeUnitIn.Add(new SelectListItem { Value = "TAGGING", Text = "Phase 1" });
            SizeUnitIn.Add(new SelectListItem { Value = "REVIEW", Text = "Phase 2" });
            /* SizeUnitIn.Add(new SelectListItem { Value = "FINAL", Text = "Phase 4" });*/
            SizeUnitIn.Add(new SelectListItem { Value = "QC", Text = "Phase 3" });
            SizeUnitIn.Add(new SelectListItem { Value = "COMPLETE", Text = "Final" });
            SizeUnitIn.Add(new SelectListItem { Value = "DELIVERY", Text = "Delivery" });
            SizeUnitIn.Add(new SelectListItem { Value = "TOBEDELIVERED", Text = "To Be Delivered" });


            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetFileType(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "SOURCE", Text = "Source" });
            SizeUnitIn.Add(new SelectListItem { Value = "TAGGING", Text = "Tagging" });
            SizeUnitIn.Add(new SelectListItem { Value = "REVIEW", Text = "Review" });
            SizeUnitIn.Add(new SelectListItem { Value = "QC", Text = "Quality Control" });
            SizeUnitIn.Add(new SelectListItem { Value = "FINAL", Text = "Final" });
            SizeUnitIn.Add(new SelectListItem { Value = "DELIVERY", Text = "Delivery" });


            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetAltTextStatusList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "0", Text = "Pending" });
            SizeUnitIn.Add(new SelectListItem { Value = "5", Text = "Reviewed - ALT Supplied" });
            SizeUnitIn.Add(new SelectListItem { Value = "1", Text = "Reviewed - No ALT Required" });
            SizeUnitIn.Add(new SelectListItem { Value = "2", Text = "Reviewed - ALT Required" });
            SizeUnitIn.Add(new SelectListItem { Value = "4", Text = "Reviewed - ALT Written" });
            SizeUnitIn.Add(new SelectListItem { Value = "3", Text = "ALT Added" });
            SizeUnitIn.Add(new SelectListItem { Value = "6", Text = "Finalized - Waiting for ALT" });
            SizeUnitIn.Add(new SelectListItem { Value = "7", Text = "Finalized - ALT Written" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }
        public static MultiSelectList GetQueryType(string selectedValue = "")
        {
            List<SelectListItem> QueryType = new List<SelectListItem>();
            QueryType.Add(new SelectListItem { Value = "IT Support", Text = "IT Support" });
            QueryType.Add(new SelectListItem { Value = "Meeting", Text = "Meeting" });
            QueryType.Add(new SelectListItem { Value = "Questions", Text = "Questions" });


            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(QueryType, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(QueryType, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetTimeType(string selectedValue = "")
        {
            List<SelectListItem> QueryType = new List<SelectListItem>();
            QueryType.Add(new SelectListItem { Value = "IT Support", Text = "IT Support" });
            QueryType.Add(new SelectListItem { Value = "Meeting", Text = "Meeting" });
            QueryType.Add(new SelectListItem { Value = "Training", Text = "Training" });
            QueryType.Add(new SelectListItem { Value = "Statutory", Text = "Statutory Holiday" });
            QueryType.Add(new SelectListItem { Value = "Personal", Text = "Personal Holiday" });
            QueryType.Add(new SelectListItem { Value = "Vacation", Text = "Vacation" });
            QueryType.Add(new SelectListItem { Value = "Fixes", Text = "Fixes" });
            QueryType.Add(new SelectListItem { Value = "Others", Text = "Others" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(QueryType, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(QueryType, "Value", "Text", values);
            }
        }
        public static MultiSelectList GetTimeTypeNew(string selectedValue = "")
        {
            List<SelectListItem> QueryType = new List<SelectListItem>();
            QueryType.Add(new SelectListItem { Value = "Contract", Text = "Contract" });
            QueryType.Add(new SelectListItem { Value = "Personal", Text = "Personal Holiday" });
            QueryType.Add(new SelectListItem { Value = "Statutory", Text = "Statutory Holiday" });
            QueryType.Add(new SelectListItem { Value = "Training", Text = "Training" });
            QueryType.Add(new SelectListItem { Value = "Meetings", Text = "Meetings" });
            QueryType.Add(new SelectListItem { Value = "Others", Text = "Others" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(QueryType, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(QueryType, "Value", "Text", values);
            }
        }

        public static string UppercaseFirst(string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static string RandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;

        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public static Dictionary<string, object> Languages()
        {
            var langus = new Dictionary<string, object>();
            langus.Add("EN", "English");
            langus.Add("FR", "French");
            langus.Add("DA", "Danish");
            langus.Add("DE", "German");
            langus.Add("ES", "Spanish");
            langus.Add("SV", "Swedish");
            langus.Add("IS", "Icelandic");
            langus.Add("FI", "Finnish");
            langus.Add("NN", "Norwegian");
            return langus;

        }

        public static MultiSelectList GetSeverityList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "10", Text = "Feature" });
            SizeUnitIn.Add(new SelectListItem { Value = "20", Text = "Trivial" });
            SizeUnitIn.Add(new SelectListItem { Value = "30", Text = "Text" });
            SizeUnitIn.Add(new SelectListItem { Value = "40", Text = "Tweak" });
            SizeUnitIn.Add(new SelectListItem { Value = "50", Text = "Minor" });
            SizeUnitIn.Add(new SelectListItem { Value = "60", Text = "Major" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }

        }

        public static MultiSelectList GetPriorityList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "10", Text = "None" });
            SizeUnitIn.Add(new SelectListItem { Value = "20", Text = "Low" });
            SizeUnitIn.Add(new SelectListItem { Value = "30", Text = "Normal" });
            SizeUnitIn.Add(new SelectListItem { Value = "40", Text = "High" });
            SizeUnitIn.Add(new SelectListItem { Value = "50", Text = "Urgent" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }

        }

        public static MultiSelectList GetNotificationEmailList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "production@abledocs.com", Text = "production@abledocs.com" });
            SizeUnitIn.Add(new SelectListItem { Value = "produktion@abledoc.com", Text = "produktion@abledoc.com" });


            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetSearchList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "ID", Text = "File ID" });
            SizeUnitIn.Add(new SelectListItem { Value = "JobID", Text = "Job ID" });
            SizeUnitIn.Add(new SelectListItem { Value = "Client", Text = "Client" });
            SizeUnitIn.Add(new SelectListItem { Value = "Contact", Text = "Contact" });
            SizeUnitIn.Add(new SelectListItem { Value = "EngagementNum", Text = "Engagement Number" });
            SizeUnitIn.Add(new SelectListItem { Value = "Status", Text = "Status" });
            SizeUnitIn.Add(new SelectListItem { Value = "FileName", Text = "File Name" });
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetMothList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            SizeUnitIn.Add(new SelectListItem { Value = "01", Text = "January" });
            SizeUnitIn.Add(new SelectListItem { Value = "02", Text = "February" });
            SizeUnitIn.Add(new SelectListItem { Value = "03", Text = "March" });
            SizeUnitIn.Add(new SelectListItem { Value = "04", Text = "April" });
            SizeUnitIn.Add(new SelectListItem { Value = "05", Text = "May" });
            SizeUnitIn.Add(new SelectListItem { Value = "06", Text = "June" });
            SizeUnitIn.Add(new SelectListItem { Value = "07", Text = "July" });
            SizeUnitIn.Add(new SelectListItem { Value = "08", Text = "August" });
            SizeUnitIn.Add(new SelectListItem { Value = "09", Text = "September" });
            SizeUnitIn.Add(new SelectListItem { Value = "10", Text = "October" });
            SizeUnitIn.Add(new SelectListItem { Value = "11", Text = "November" });
            SizeUnitIn.Add(new SelectListItem { Value = "12", Text = "December" });
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }

        public static MultiSelectList GetYearList(string selectedValue = "")
        {
            List<SelectListItem> SizeUnitIn = new List<SelectListItem>();
            List<int> listYears = Enumerable.Range(2017, DateTime.Now.Year - 2017 + 1).ToList();
            foreach (int item in listYears)
            {
                SizeUnitIn.Add(new SelectListItem { Value = item.ToString(), Text = item.ToString() });
            }

            //SizeUnitIn.Add(new SelectListItem { Value = "2017", Text = "2017" });
            //SizeUnitIn.Add(new SelectListItem { Value = "2018", Text = "2018" });
            //SizeUnitIn.Add(new SelectListItem { Value = "2019", Text = "2019" });
            //SizeUnitIn.Add(new SelectListItem { Value = "2020", Text = "2020" });
            //SizeUnitIn.Add(new SelectListItem { Value = "2021", Text = "2021" });

            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(SizeUnitIn, "Value", "Text");
            else
            {
                string[] values = selectedValue.Split(',').ToArray();
                return new MultiSelectList(SizeUnitIn, "Value", "Text", values);
            }
        }
        public static MultiSelectList GetDescriptionList(string country = "",string language = "",string selectedValue = "")
        {
            DiscriptionMaster objDescriptionMaster = new DiscriptionMaster();
            List<DiscriptionMaster> listStatus = objDescriptionMaster.GetDescriptionMasterList(country, language);
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(listStatus, "ProductName", "ProductName");
            else
            {
                string[] ints = selectedValue.Split(',').ToArray();
                return new MultiSelectList(listStatus, "ProductName", "ProductName", ints);
            }

        }

        public static MultiSelectList GetUnitByDescription(string descriptionText = "", string selectedValue = "")
        {
            
            DiscriptionMaster discriptionMaster = new DiscriptionMaster();
            discriptionMaster = discriptionMaster.GetContentByName(descriptionText);
            CommonMaster objCommon = new CommonMaster();
            var unit = "";
            if (discriptionMaster.unit != null) {
             unit = discriptionMaster.unit.ToString();
            }
            List<CommonMaster> unitMaster = objCommon.GetCommonMasterByMultiId(unit);
            if (string.IsNullOrWhiteSpace(selectedValue))
                return new SelectList(unitMaster, "typecode", "typename");
            else
            {
                string[] ints = selectedValue.Split(',').ToArray();
                return new MultiSelectList(unitMaster, "typecode", "typename", ints);
            }

        }


    }
}
