using CsvHelper.Configuration;
using ELearn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.CSV_Validation
{
    public class ApplicationUserMap : ClassMap<ApplicationUser>
    {
        public ApplicationUserMap()
        {

            Map(m => m.FirstName);
            Map(m => m.LastName);
            Map(m => m.BirthDate).TypeConverterOption.Format("dd/mm/yyyy");
            Map(m => m.Address);
            Map(m => m.Nationality);
            Map(m => m.NId);
            Map(m => m.UserName);
            Map(m => m.PhoneNumber);
            Map(m => m.DepartmentId);
        }
    }
}
