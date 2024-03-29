﻿using AirFinder.Domain.Common;
using AirFinder.Domain.People.Enums;

namespace AirFinder.Domain.People
{
    public class Person : BaseModel
    {
        public Person(string name, string email, DateTime birthday, string cpf, Gender gender, string phone) 
        {
            Name= name;
            Email= email;
            Birthday= birthday;
            CPF = cpf;
            Gender= gender;
            Phone= phone;
        }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public DateTime Birthday { get; set; }
        public string CPF { get; set; } = String.Empty;
        public Gender Gender { get; set; }
        public string Phone { get; set; } = String.Empty;
    }
}
