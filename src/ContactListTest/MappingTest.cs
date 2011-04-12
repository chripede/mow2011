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


    }
} 