using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class QcFile
    {
        public JobsFiles jobsFiles { get; set; }
        public Jobs jobs { get; set; }
        public Clients clients { get; set; }
        public AllTimers allTimers { get; set; }
        public List<AltTexts> altTextsList { get; set; }
        public List<JobsFilesFinal> jobsFilesFinalList { get; set; }
        public List<JobsFilesQC> jobsFilesQCList { get; set; }
        public List<JobsFilesReviews> jobsFilesCommentsList { get; set; }
        public string flag { get; set; }
    }
}
