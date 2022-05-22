using System;
using System.IO;

namespace DesktopClient.Models
{
    public class StudentInformation
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public Stream FaceCapture { get; set; }
        public Stream CardCapture { get; set; }
    }
}
