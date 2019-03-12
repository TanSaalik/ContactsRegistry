using System;

namespace tthk.ContactsRegistry.Data
{
    public class ContactEmail
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public string Email { get; set; }
        public EmailType? Type { get; set; }
        public bool IsDefault { get; set; }

    }
}
