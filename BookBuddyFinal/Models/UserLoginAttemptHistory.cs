﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BookBuddyFinal.Models
{
    public partial class UserLoginAttemptHistory
    {
        public int UserLoginAttemptHistoryId { get; set; }
        public int UserId { get; set; }
        public bool LoginSuccess { get; set; }
        public DateTime? AttemptAt { get; set; }

        public virtual Users User { get; set; }
    }
}
