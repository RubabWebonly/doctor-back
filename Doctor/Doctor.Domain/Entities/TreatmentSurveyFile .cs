using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Domain.Entities
{
    public class TreatmentSurveyFile : BaseEntity
    {
        public int TreatmentSurveyId { get; set; }
        public TreatmentSurvey TreatmentSurvey { get; set; }

        public string FilePath { get; set; }
        public string FileType { get; set; }
    }
}
