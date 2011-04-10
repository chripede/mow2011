namespace ContactList.Entities
{
    public class Person
    {
        public virtual int Id { get; protected set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }
    }
}