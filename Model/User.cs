﻿using System;

namespace Model
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string PassWord { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTime LoginTime { get; set; }
    }
}