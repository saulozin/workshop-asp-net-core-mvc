using Microsoft.EntityFrameworkCore;
using SallesWebMvc.Data;
using SallesWebMvc.Models;
using SallesWebMvc.Services.Exceptions;

namespace SallesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SallesWebMvcContext _context;

        public SalesRecordService(SallesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindAllAsync()
        {
            return await _context.SalesRecord
                .Include(sr => sr.Seller)
                .ThenInclude(s => s.Department)
                .ToListAsync();
        }

        public async Task InsertAsync(SalesRecord obj)
        {
            obj.Date = DateTime.SpecifyKind(obj.Date, DateTimeKind.Utc);
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<SalesRecord?> FindByIdAsync(int id)
        {
            return await _context.SalesRecord
                .Include(sr => sr.Seller)
                .ThenInclude(s => s.Department)
                .FirstOrDefaultAsync(sr => sr.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.SalesRecord.FindAsync(id);

            if (obj != null)
            {
                _context.SalesRecord.Remove(obj);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(SalesRecord obj)
        {
            bool hasAny = await _context.SalesRecord.AnyAsync(x => x.Id == obj.Id);

            if (!hasAny)
            {
                throw new NotFoundException("SalesRecord not found!");
            }

            try
            {
                obj.Date = DateTime.SpecifyKind(obj.Date, DateTimeKind.Utc);

                _context.Update(obj);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException err)
            {
                throw new DbConcurrencyException(err.Message);
            }
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {

            if (minDate.HasValue)
            {
                minDate = DateTime.SpecifyKind(minDate.Value, DateTimeKind.Utc);
            }

            if (maxDate.HasValue)
            {
                maxDate = DateTime.SpecifyKind(maxDate.Value, DateTimeKind.Utc);
            }

            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {

            if (minDate.HasValue)
            {
                minDate = DateTime.SpecifyKind(minDate.Value, DateTimeKind.Utc);
            }

            if (maxDate.HasValue)
            {
                maxDate = DateTime.SpecifyKind(maxDate.Value, DateTimeKind.Utc);
            }

            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
