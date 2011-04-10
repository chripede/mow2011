using ContactListWeb.Models;
using FluentNHibernate.Mapping;

namespace ContactListWeb.Infrastructure.NHibernate.Mappings
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Table("Person");

            Id(x => x.Id).GeneratedBy.HiLo("1000");
            Map(x => x.Firstname);
            Map(x => x.Lastname);
            
            HasMany(x => x.Addresses)
                .KeyColumn("Owner_id")
                .Inverse()
                .Cascade.All();

            HasMany(x => x.PhoneNumbers)
                .KeyColumn("Owner_id")
                .Inverse()
                .Cascade.All();
        }
    }

    public class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Table("Address");

            Id(x => x.Id).GeneratedBy.HiLo("1000");
            References(x => x.Owner);
            Map(x => x.Streetname);
            Map(x => x.City);
            Map(x => x.AddressType);
        }
    }

    public class PhoneMap : ClassMap<Phone>
    {
        public PhoneMap()
        {
            Table("Phone");

            Id(x => x.Id).GeneratedBy.HiLo("1000");
            References(x => x.Owner);
            Map(x => x.PhoneNumber);
            Map(x => x.PhoneType);
        }
    }
}