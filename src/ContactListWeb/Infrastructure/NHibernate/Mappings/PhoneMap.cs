using ContactListWeb.Models;
using FluentNHibernate.Mapping;

namespace ContactListWeb.Infrastructure.NHibernate.Mappings
{
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