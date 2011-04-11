using System.Data;
using ContactListWeb.Infrastructure;
using ContactListWeb.Infrastructure.NHibernate;
using ContactListWeb.Models;
using FluentNHibernate.Testing;
using NHibernate;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace ContactListTest
{
    [TestFixture]
    public class MappingTest
    {
        private ISession _session;

        [SetUp]
        public void SetUp()
        {
            _session = SessionManager.Instance.SessionFactory.OpenSession();
            _session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        [TearDown]
        public void TearDown()
        {
            _session.Transaction.Rollback();
            _session.Dispose();
        }

        [Test]
        public void CanMapPerson()
        {
            new PersistenceSpecification<Person>(_session)
                .CheckProperty(x => x.Firstname, "firstname")
                .CheckProperty(x => x.Lastname, "lastname")
                .VerifyTheMappings();
        }

        [Test]
        public void CanMapAddress()
        {
            int personId = (int) _session.Save(new Person {Firstname = "firstname", Lastname = "lastname"});
            var person = _session.Get<Person>(personId);

            new PersistenceSpecification<Address>(_session)
                .CheckProperty(x => x.AddressType, AddressType.Work)
                .CheckProperty(x => x.Streetname, "street")
                .CheckProperty(x => x.City, "city")
                .CheckReference(x => x.Owner, person)
                .VerifyTheMappings();
        }

        [Test]
        public void CanMapAddressAutomatically()
        {
            var address = new Fixture()
                .Build<Address>()
                .With(x => x.Owner, _session.Load<Person>(1000))
                .CreateAnonymous();

            new PersistenceSpecification<Address>(_session)
                .VerifyTheMappings(address);
        }
    }
} 