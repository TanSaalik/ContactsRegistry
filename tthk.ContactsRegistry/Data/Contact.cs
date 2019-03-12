using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace tthk.ContactsRegistry.Data
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public virtual ICollection<ContactPhoneNumber> PhoneNumbers { get; set; } = new Collection<ContactPhoneNumber>();
        public virtual ICollection<ContactEmail> Emails { get; set; } = new Collection<ContactEmail>();
    }
}
