using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessKing.Application.DataObjects
{
    public class UserTokenData
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime ExpiresAtUtc { get; set; }
    }
}
