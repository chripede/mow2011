using System.Collections.Generic;

namespace ContactListWeb.Models
{
    public class Person
    {
        public virtual int Id { get; protected set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual ICollection<Address> Addresses { get; protected set; }

        public virtual ICollection<Phone> PhoneNumbers { get; protected set; }

        public Person()
        {
            Addresses = new List<Address>();
            PhoneNumbers = new List<Phone>();
        }
    }
}

