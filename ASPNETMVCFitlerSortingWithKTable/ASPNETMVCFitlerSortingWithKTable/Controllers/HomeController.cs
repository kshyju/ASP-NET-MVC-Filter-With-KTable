using System.Web.Mvc;
using ASPNETMVCFitlerSortingWithKTable.Models;
using ASPNETMVCFitlerSortingWithKTable.Services;
using System.Collections.Generic;
using System.Linq;
namespace ASPNETMVCFitlerSortingWithKTable.Controllers
{
    public class HomeController : BaseController
    {
        readonly CustomerService customerService;
        public HomeController()
        {
            customerService = new CustomerService();
        }

public ActionResult Index(int? page, int? size, ListFilterVM filter = null)
{
    int pageNumber = 1;
    int pageSize = 5;
    if (page.HasValue)
        pageNumber = page.Value;

    if (size.HasValue)
        pageSize = size.Value;

    var customerList = customerService.GetCustomers();

    if (filter != null)
    {
        customerList = customerService.ApplyFilters(customerList, filter);
    }

    int totalRowCount = customerList.Count();
    int itemstoSkip = pageSize * (pageNumber - 1);
    customerList = customerList.Skip(itemstoSkip).Take(pageSize).ToList();

    var customerListVM = new CustomerListViewModel { Customers = customerList };
    ApplyPagingInformation(pageNumber, pageSize, customerListVM.PagingDetails, totalRowCount);

    if (Request.IsAjaxRequest())
    {
        // If it is an ajax request ( from Sorting/Filtering event or by clicking on a page number), send the response in JSON format
        var pagingMarkup = RenderPartialView("TablePagingFooter", customerListVM.PagingDetails, new ViewDataDictionary { { "baseurl", Url.Action("Index", "Home") + "?c" } });
        return Json(new { ListingMarkup = RenderPartialView("Partial/Index", customerListVM), PagingMarkup = pagingMarkup }, JsonRequestBehavior.AllowGet);
    }
    return View(customerListVM);
}

    }
}
