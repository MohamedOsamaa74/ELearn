using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.GroupDTOs
{
    public class UserGroupDTO
    {
        public required string UserName { get; set; }
        public required int GroupId { get; set; }
    }
}
