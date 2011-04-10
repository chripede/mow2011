using ContactListWeb.Infrastructure;

namespace ContactListWeb.Models
{
    public class Phone
    {
        public virtual int Id { get; protected set; }

        public virtual string PhoneNumber { get; set; }

        public virtual PhoneType PhoneType { get; set; }

        public virtual Person Owner { get; set; }
    }
}