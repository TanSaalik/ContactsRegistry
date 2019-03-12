using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace tthk.ContactsRegistry.Data
{
    public class Listitem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public virtual ICollection<ContactPhoneNumber> PhoneNumbers { get; set; } = new Collection<ContactPhoneNumber>();
        public virtual ICollection<ContactEmail> Emails { get; set; } = new Collection<ContactEmail>();
    }

    public class ListItemData
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
    }
}
