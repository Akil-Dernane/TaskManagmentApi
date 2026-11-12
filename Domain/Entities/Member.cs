using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Member
    {
        public Guid Id { get; }
        [Required,EmailAddress]
        public string Email { get; }

    }
}
