using CleanArchCQRSandMediator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchCQRSandMediator.Application.Dtos.Users
{
    public class UserProfileResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string FirstSurname { get; set; } = string.Empty;
        public string SecondSurname { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string CellPhone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? WhatsApp { get; set; }
        public string? Address { get; set; }
    }
}
