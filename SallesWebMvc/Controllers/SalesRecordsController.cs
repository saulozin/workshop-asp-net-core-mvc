using Microsoft.AspNetCore.Mvc;
using SallesWebMvc.Models;
using SallesWebMvc.Models.ViewModels;
using SallesWebMvc.Services;

namespace SallesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var sellers = await _sellerService.FindAllAsync();

            var viewModel = new SalesRecordFormViewModel
            {
                Sellers = sellers
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesRecord salesRecord)
        {
            if (!ModelState.IsValid)
            {
                var sellers = await _sellerService.FindAllAsync();

                var viewModel = new SalesRecordFormViewModel
                {
                    SalesRecord = salesRecord,
                    Sellers = sellers
                };

                return View(viewModel);
            }

            await _salesRecordService.InsertAsync(salesRecord);

            return RedirectToAction(nameof(SimpleSearch));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(SimpleSearch));
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(SimpleSearch));
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(SimpleSearch));
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(SimpleSearch));
            }

            var sellers = await _sellerService.FindAllAsync();

            var viewModel = new SalesRecordFormViewModel
            {
                SalesRecord = obj,
                Sellers = sellers
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesRecord salesRecord)
        {
            if (id != salesRecord.Id)
            {
                return RedirectToAction(nameof(SimpleSearch));
            }

            if (!ModelState.IsValid)
            {
                var sellers = await _sellerService.FindAllAsync();

                var viewModel = new SalesRecordFormViewModel
                {
                    SalesRecord = salesRecord,
                    Sellers = sellers
                };

                return View(viewModel);
            }

            await _salesRecordService.UpdateAsync(salesRecord);

            return RedirectToAction(nameof(SimpleSearch));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(SimpleSearch));
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(SimpleSearch));
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _salesRecordService.RemoveAsync(id);

            return RedirectToAction(nameof(SimpleSearch));
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);
        }
    }
}
