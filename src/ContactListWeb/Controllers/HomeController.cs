using System.Linq;
using System.Web.Mvc;
using ContactListWeb.Infrastructure.NHibernate;
using ContactListWeb.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;

namespace ContactListWeb.Controllers
{
    public class HomeController : Controller
    {
        private ISession session;

        public HomeController()
        {
            session = SessionManager.Instance.SessionFactory.GetCurrentSession();
        }

        public ActionResult Index()
        {
            var contacts = session.QueryOver<Person>()
                .OrderBy(x => x.Firstname).Asc
                .List();

            return View(contacts);
        }

        public ActionResult Details(int id)
        {
            var person = session.QueryOver<Person>()
                .Where(x => x.Id == id)
                .Fetch(x => x.Addresses).Eager
                .Fetch(x => x.PhoneNumbers).Eager
                .SingleOrDefault();

            person = session.CreateCriteria<Person>()
                .Add(Restrictions.Eq("Id", id))
                .CreateAlias("Addresses", "addresses", JoinType.LeftOuterJoin)
                .CreateAlias("PhoneNumbers", "phoneNumbers", JoinType.LeftOuterJoin)
                .UniqueResult<Person>();

            person = (from p in session.Query<Person>()
                      join a in session.Query<Address>() on p.Id equals a.Owner.Id
                      join n in session.Query<Phone>() on p.Id equals n.Owner.Id
                      where p.Id == id
                      select p).SingleOrDefault();

            person = session.CreateQuery(@"select p, a, n from p in Person 
                                           left outer join p.Addresses as a 
                                           left outer join p.PhoneNumbers as n 
                                           where p = :id")
                .SetInt32("id", id)
                .UniqueResult().As<object[]>()[0] as Person;

            // Already in first level cache
            person = session.Get<Person>(id);

            return View(person);
        }

        public ActionResult Delete(int id)
        {
            var person = session.QueryOver<Person>()
                .Where(x => x.Id == id)
                .Fetch(x => x.Addresses).Eager
                .Fetch(x => x.PhoneNumbers).Eager
                .SingleOrDefault();
            
            session.Delete(person);

            return new RedirectResult("/");
        }

        public ActionResult Edit(int id)
        {
            var person = session.Get<Person>(id);

            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(int id, Person updatedPerson)
        {
            var person = session.Get<Person>(id);
            person.Firstname = updatedPerson.Firstname;
            person.Lastname = updatedPerson.Lastname;
            
            return new RedirectResult("/");
        }

        public ActionResult Create()
        {
            var person = new Person();

            return View(person);
        }

        [HttpPost]
        public ActionResult Create(Person person)
        {
            session.Save(person);

            return new RedirectResult("/");
        }
    }
}
