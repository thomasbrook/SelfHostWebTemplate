using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplate.Model.Dto
{
    public class StudentInfoDto
    {
        private string studentName;
        private string studentId;
        private string studentNumber;
        private string sex;
        private string classId;

        public string StudentName { get => studentName; set => studentName = value; }
        public string StudentId { get => studentId; set => studentId = value; }
        public string StudentNumber { get => studentNumber; set => studentNumber = value; }
        public string Sex { get => sex; set => sex = value; }
        public string ClassId { get => classId; set => classId = value; }
    }
}
