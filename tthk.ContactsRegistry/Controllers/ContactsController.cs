using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tthk.ContactsRegistry.Data;

namespace tthk.ContactsRegistry.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsContext _context;

        public ContactsController(ContactsContext context) { _context = context; }

        [HttpGet]
        public IActionResult Get([FromQuery] string term)
        {
            //IQueryble<Contact> seed = seed.
            // seed = seed.Where(x => x.Name.Contains(term)
            IQueryable<Contact> stuff = _context.Contacts;

            if (term != null)
            {
                stuff = _context.Contacts.Where(x => x.Name.Contains(term) ||
                x.PhoneNumbers.OrderByDescending(pn => pn.IsDefault).FirstOrDefault().Number.Contains(term) ||
                x.Emails.OrderByDescending(pn => pn.IsDefault).FirstOrDefault().Email.Contains(term));
            } else
            {
                stuff = _context.Contacts;
            }

            return Ok(stuff
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    defaultPhoneNumber = x.PhoneNumbers.OrderByDescending(pn => pn.IsDefault).FirstOrDefault().Number,
                    defaultEmail = x.Emails.OrderByDescending(pn => pn.IsDefault).FirstOrDefault().Email
                })
            .ToList());
            //return Ok(_context.Contacts);
        }

        [HttpGet("{id}")]
        public IActionResult GetContact([FromRoute] Guid id)
        {
            var contact = _context.Contacts
                .Include(x => x.PhoneNumbers)
                .Include(x => x.Emails)
                .FirstOrDefault(x => x.Id == id);


            if (contact == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                Id = contact.Id,
                Name = contact.Name,
                Initials = contact.Initials,
                PhoneNumbers = contact.PhoneNumbers.Select(pn => new
                {
                    id = pn.Id,
                    Number = pn.Number,
                    type = pn.Type?.ToString(),
                    IsDefault = pn.IsDefault
                }),
                Emails = contact.Emails.Select(em => new
                {
                    id = em.Id,
                    Email = em.Email,
                    type = em.Type?.ToString(),
                    IsDefault = em.IsDefault
                })

            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromRoute] Guid id, [FromBody] Contact contact)
        {
            //var contact = new Contact { Name = model.Name, Initials.... };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.Id)
            {
                return BadRequest();
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Redirect("http://localhost:64640/fetch-data");
            return NoContent();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Update([FromBody] Contact contact)
        {
            //var contact = await _context.Contacts.Include(x => x.Emails).Include(...Phone).FirstOrDefaultAsync(x => x.Id == model.Id)...
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var contact1 = await _context.Contacts.Include(x => x.Emails).Include(x => x.PhoneNumbers).FirstOrDefaultAsync(x => x.Id == contact.Id);

            _context.Contacts.Update(contact);
            
            _context.SaveChanges();
            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return Ok(contact);
        }

        private bool ContactExists(Guid id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

        private Contact Dto(Contact listitem)
        {
            return new Contact
            {
                Id = listitem.Id,
                Name = listitem.Name,
                Initials = listitem.Initials,
                PhoneNumbers = listitem.PhoneNumbers,
                Emails = listitem.Emails
            };
        }
    }
}