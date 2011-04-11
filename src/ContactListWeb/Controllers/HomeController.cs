using System.Web.Mvc;
using ContactListWeb.Infrastructure.NHibernate;
using ContactListWeb.Models;
using NHibernate;

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
                .Cacheable()
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

            //person = session.CreateCriteria<Person>()
            //    .Add(Restrictions.Eq("Id", id))
            //    .CreateAlias("Addresses", "addresses", JoinType.LeftOuterJoin)
            //    .CreateAlias("PhoneNumbers", "phoneNumbers", JoinType.LeftOuterJoin)
            //    .UniqueResult<Person>();

            //person = (from p in session.Query<Person>()
            //          where p.Id == id
            //          select p).SingleOrDefault();

            //person = session.CreateQuery(@"from p in Person where p = :id")
            //    .SetInt32("id", id)
            //    .UniqueResult<Person>();

            //// Already in first level cache
            //var person = session.Get<Person>(id);

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

        public ActionResult CreateAddress()
        {
            var address = new Address();

            return View(address);
        }

        [HttpPost]
        public ActionResult CreateAddress(int id, Address address)
        {
            var person = session.Load<Person>(id);
            address.Owner = person;
            session.Save(address);

            return new RedirectResult("/Home/Details/" + id);
        }

        public ActionResult DeleteAddress(int id, int personId)
        {
            var address = session.Load<Address>(id);
            session.Delete(address);

            return new RedirectResult("/Home/Details/" + personId);
        }

        public ActionResult CreatePhone()
        {
            var phone = new Phone();

            return View(phone);
        }

        [HttpPost]
        public ActionResult CreatePhone(int id, Phone phone)
        {
            var person = session.Load<Person>(id);
            phone.Owner = person;
            session.Save(phone);

            return new RedirectResult("/Home/Details/" + id);
        }

        public ActionResult DeletePhone(int id, int personId)
        {
            var phone = session.Load<Phone>(id);
            session.Delete(phone);

            return new RedirectResult("/Home/Details/" + personId);
        }
    }
}
