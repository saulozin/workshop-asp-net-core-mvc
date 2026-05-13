using SallesWebMvc.Models;
using SallesWebMvc.Models.Enums;

namespace SallesWebMvc.Data
{
    public class SeedingService
    {
        private readonly SallesWebMvcContext _context;

        public SeedingService(SallesWebMvcContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Department.Any() ||
                _context.Seller.Any() ||
                _context.SalesRecord.Any())
            {
                return;
            }

            // =========================
            // DEPARTMENTS
            // =========================

            var d1 = new Department("Computers");
            var d2 = new Department("Electronics");
            var d3 = new Department("Fashion");
            var d4 = new Department("Books");

            _context.Department.AddRange(d1, d2, d3, d4);
            _context.SaveChanges();

            // =========================
            // SELLERS
            // =========================

            var s1 = new Seller
            {
                Name = "Bob Brown",
                Email = "bob@email.com",
                BirthDate = DateTime.SpecifyKind(
                    new DateTime(1998, 4, 21),
                    DateTimeKind.Utc),
                BaseSalary = 1800.0,
                DepartmentId = d1.Id
            };

            var s2 = new Seller
            {
                Name = "Maria Green",
                Email = "maria@email.com",
                BirthDate = DateTime.SpecifyKind(
                    new DateTime(1979, 12, 31),
                    DateTimeKind.Utc),
                BaseSalary = 3500.0,
                DepartmentId = d2.Id
            };

            var s3 = new Seller
            {
                Name = "Alex Grey",
                Email = "alex@email.com",
                BirthDate = DateTime.SpecifyKind(
                    new DateTime(1988, 1, 15),
                    DateTimeKind.Utc),
                BaseSalary = 2200.0,
                DepartmentId = d1.Id
            };

            var s4 = new Seller
            {
                Name = "Martha Red",
                Email = "martha@email.com",
                BirthDate = DateTime.SpecifyKind(
                    new DateTime(1993, 11, 30),
                    DateTimeKind.Utc),
                BaseSalary = 3000.0,
                DepartmentId = d4.Id
            };

            var s5 = new Seller
            {
                Name = "Donald Blue",
                Email = "donald@email.com",
                BirthDate = DateTime.SpecifyKind(
                    new DateTime(2000, 1, 9),
                    DateTimeKind.Utc),
                BaseSalary = 4000.0,
                DepartmentId = d3.Id
            };

            var s6 = new Seller
            {
                Name = "Ana Pink",
                Email = "ana@email.com",
                BirthDate = DateTime.SpecifyKind(
                    new DateTime(1997, 3, 4),
                    DateTimeKind.Utc),
                BaseSalary = 3000.0,
                DepartmentId = d2.Id
            };

            _context.Seller.AddRange(s1, s2, s3, s4, s5, s6);
            _context.SaveChanges();

            // =========================
            // SALES RECORDS
            // =========================

            var r1 = new SalesRecord
            {
                Date = DateTime.SpecifyKind(
                    new DateTime(2026, 5, 10),
                    DateTimeKind.Utc),

                Amount = 11000.0,
                Status = SalleStatus.Billed,
                SellerId = s1.Id
            };

            var r2 = new SalesRecord
            {
                Date = DateTime.SpecifyKind(
                    new DateTime(2026, 5, 4),
                    DateTimeKind.Utc),

                Amount = 7000.0,
                Status = SalleStatus.Billed,
                SellerId = s5.Id
            };

            var r3 = new SalesRecord
            {
                Date = DateTime.SpecifyKind(
                    new DateTime(2026, 5, 13),
                    DateTimeKind.Utc),

                Amount = 4000.0,
                Status = SalleStatus.Canceled,
                SellerId = s4.Id
            };

            _context.SalesRecord.AddRange(r1, r2, r3);

            _context.SaveChanges();
        }
    }
}