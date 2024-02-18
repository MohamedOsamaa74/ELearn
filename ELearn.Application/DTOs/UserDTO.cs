using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class UserDTO
    {
        public required string FirstName {  get; set; }
        public required string LastName {  get; set; }
        public required string Nationality {  get; set; }
        public required string NId {  get; set; }
        public required string UserName {  get; set; }
        public required string PhoneNumber {  get; set; }
        public required string Address {  get; set; }
        public required DateTime BirthDate {  get; set; }
        public int DepartmentId { get; set; }
    }
}
