using ContactListWeb.Models;
using FluentNHibernate.Mapping;

namespace ContactListWeb.Infrastructure.NHibernate.Mappings
{
    public class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Table("Address");
            Cache.ReadWrite();

            Id(x => x.Id).GeneratedBy.HiLo("1000");
            References(x => x.Owner).Not.Nullable();
            Map(x => x.Streetname);
            Map(x => x.City);
            Map(x => x.AddressType);
        }
    }
}