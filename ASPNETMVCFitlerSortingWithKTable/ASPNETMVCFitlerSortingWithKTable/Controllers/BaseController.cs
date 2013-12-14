using System.IO;
using System.Web.Mvc;
using ASPNETMVCFitlerSortingWithKTable.Models;
using ASPNETMVCFitlerSortingWithKTable.Services;

namespace ASPNETMVCFitlerSortingWithKTable.Controllers
{
public class BaseController : Controller
{
    protected void ApplyPagingInformation(int pageNumber, int pageSize, PagingDetails pagingDetails, int totalRowCount)
    {
        pagingDetails.TotalRecords = totalRowCount;
        pagingDetails.CurrentPage = pageNumber;
        int nextPageItemCount = pagingDetails.TotalRecords - (pageSize * pageNumber);
        pagingDetails.HasNextPage = (nextPageItemCount > 0);
        pagingDetails.HasPrevPage = (pageNumber > 1);

        var reminder = pagingDetails.TotalRecords % pageSize;

        pagingDetails.TotalPages = (pagingDetails.TotalRecords / pageSize);
        if (reminder > 0)
            pagingDetails.TotalPages++;
    }
protected string RenderPartialView(string viewName, object model, ViewDataDictionary dictionary = null)
{
    if (string.IsNullOrEmpty(viewName))
        viewName = this.ControllerContext.RouteData.GetRequiredString("action");

    this.ViewData.Model = model;
    if (dictionary != null)
    {
        foreach (var item in dictionary.Keys)
        {
            this.ViewData.Add(item, dictionary[item]);
        }
    }
    using (var sw = new StringWriter())
    {
        ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
        var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
        viewResult.View.Render(viewContext, sw);

        return sw.GetStringBuilder().ToString();
    }
}

}
}
