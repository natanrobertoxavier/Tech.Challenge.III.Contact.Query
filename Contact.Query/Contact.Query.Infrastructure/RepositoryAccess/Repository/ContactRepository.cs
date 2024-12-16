using Contact.Query.Domain.Repositories.Contact;
using Microsoft.EntityFrameworkCore;

namespace Contact.Query.Infrastructure.RepositoryAccess.Repository;
public class ContactRepository(
    TechChallengeContext context) : IContactReadOnlyRepository
{
    private readonly TechChallengeContext _context = context;

#pragma warning disable CS8603 // Possível retorno de referência nula.

    public Task<bool> ThereIsRegisteredContact(Guid dddId, string phoneNumber) =>
        _context.Contacts.AnyAsync(c => c.PhoneNumber.Equals(phoneNumber) &&
                                   c.DDDId.Equals(dddId));

    public async Task<IEnumerable<Domain.Entities.Contact>> RecoverAllAsync() =>
    await _context.Contacts.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Domain.Entities.Contact>> RecoverByDDDIdAsync(Guid id) =>
        await _context.Contacts.Where(c => c.DDDId.Equals(id)).ToListAsync();

    public async Task<Domain.Entities.Contact> RecoverByContactIdAsync(Guid id) =>
        await _context.Contacts.Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task<IEnumerable<Domain.Entities.Contact>> RecoverAllByDDDIdAsync(IEnumerable<Guid> dddIds) =>
        await _context.Contacts.Where(c => dddIds.Contains(c.DDDId)).ToListAsync();
}
