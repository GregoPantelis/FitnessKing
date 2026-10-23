using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessKing.Application.DataObjects
{
    public class UserAuthData
    {
        public long UserId { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime UtcCreatedAt { get; set; }
    }
}
