using System;

namespace tthk.ContactsRegistry.Data
{
    public class ContactPhoneNumber
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public string Number { get; set; }

        public PhoneNumberType? Type { get; set; }
        public bool IsDefault { get; set; }
    }
}
