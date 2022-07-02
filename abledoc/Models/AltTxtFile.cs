using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class AltTxtFile
    {
        public Jobs jobsfileid { get; set; }
        public List<JobsFilesVersions>jobsFilesVersionsList { get; set; }

        public JobsFiles jobsFiles { get; set; }
        public Jobs jobsjobid { get; set; }
        public Clients clients { get; set; }
        public List<JobsFilesCheckouts> jobsfilescheckoutslist { get; set; }
        public AllTimers allTimers { get; set; }
        public List<AltTexts> alttextslistbyclients { get; set; }
        public List<AltTexts> alttextslistbyfile { get; set; }
        public List<BrandImage> brandimagelist { get; set; }
        public JobsFilesVersions JobsFilesVersionsSource { get; set; }
        public JobsFilesVersions JobsFilesVersionsTaging { get; set; }
        public string databasename { get; set; }
        public string flag { get; set; }
    }
}
