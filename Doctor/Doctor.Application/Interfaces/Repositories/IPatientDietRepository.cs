using Doctor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Application.Interfaces.Repositories
{
    public interface IPatientDietRepository : IGenericRepository<PatientDiet>
    {
        Task<List<PatientDiet>> GetAllWithFilesAsync();
    }

}
