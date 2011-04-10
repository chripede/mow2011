using ContactList.Entities;
using FluentNHibernate.Mapping;

namespace ContactList.Mappings
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Table("Person");

            Id(x => x.Id).GeneratedBy.HiLo("100");
            Map(x => x.Firstname);
            Map(x => x.Lastname);
        }
    }
}