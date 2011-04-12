using ContactListWeb.Models;
using FluentNHibernate.Mapping;

namespace ContactListWeb.Infrastructure.NHibernate.Mappings
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Table("Person");
            Cache.ReadWrite();

            Id(x => x.Id).GeneratedBy.HiLo("1000");
            Map(x => x.Firstname);
            Map(x => x.Lastname);
            
            HasMany(x => x.Addresses)
                .AsSet()
                .KeyColumn("Owner_id")
                .Inverse()
                .Cascade.All();

            HasMany(x => x.PhoneNumbers)
                .AsSet()
                .KeyColumn("Owner_id")
                .Inverse()
                .Cascade.All();
        }
    }
}