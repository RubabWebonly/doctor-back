using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Domain.Entities
{
    public class TreatmentSurveyDiet : BaseEntity
    {
        public int TreatmentSurveyId { get; set; }
        public TreatmentSurvey TreatmentSurvey { get; set; }

        public int DietId { get; set; }
        public Diet Diet { get; set; }
    }
}
