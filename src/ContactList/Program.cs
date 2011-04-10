using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContactList.Entities;

namespace ContactList
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            for (int x = 0; x < 210; x++)
            {
                var session = new SessionManager().Session;
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(new Person {Firstname = "Christian", Lastname = "Pedersen"});
                    transaction.Commit();
                }
                session.Close();
            }

            var session1 = new SessionManager().Session;

            var persons = session1.QueryOver<Person>().List();

            foreach (var person in persons)
            {
                Console.WriteLine("Found: " + person.Firstname + " " + person.Lastname);
            }

            session1.Close();

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
