using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class File
    {
        public Jobs jobs { get; set; }
        public JobsFiles jobsFiles { get; set; }
        public List<JobsFiles> JobFileTree { get;set;}
        public List<JobsFilesVersions> JobsFilesVersionsTree { get; set; }
        public List<JobsFilesVersions> ReferenceFile { get; set; }
        public List<JobsFilesQC> QCLCList { get; set; }
        public List<Conversations> ConversationsList { get; set; }
        public List<JobsFilesFinal> FLCList { get; set; }
        public List<JobsFilesReviews> RLCList { get; set; }
        public List<ErrorReport> ErrorReportList { get; set; }
        public List<JobsFilesReviews> JobsFilesCommentsList { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public string databasename { get; set; }
        public string flag { get; set; }
    }

}
