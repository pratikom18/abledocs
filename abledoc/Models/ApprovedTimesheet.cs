using System.Collections.Generic;


namespace abledoc.Models
{
    public class ApprovedTimesheet
    {
        public string timesheetWeekRange { get; set; }
        public string billableHours { get; set; }
        public string equivalentHours { get; set; }
        public List<string> dateRange { get; set; }
        public List<string> weeklyHourList { get; set; }
        public List<string> weekDays { get; set; }
        public List<carriedForwardFileString> carriedForwardFileStringsList { get; set; }
        public List<List<clubbedFile>> clubbedFileList1 { get; set; }
        public string carriedForwardTotal { get; set; }

    }
}
