using ContactListWeb.Infrastructure;

namespace ContactListWeb.Models
{
    public class Address
    {
        public virtual int Id { get; set; }

        public virtual Person Owner { get; set; }

        public virtual string Streetname { get; set; }

        public virtual string City { get; set; }

        public virtual AddressType AddressType { get; set; }
    }
}