using System.Collections.Generic;
namespace ASPNETMVCFitlerSortingWithKTable.Models
{

public class FilterVM
{
    public string ColumnName { set; get; }
    public string SearchType { set; get; }
    public string SearchText { set; get; }
    public string StartDate { set; get; }
    public string EndDate { set; get; }
}
public class ListFilterVM
{
    public List<FilterVM> Filters { set; get; }
    public string SortColumnName { set; get; }
    public string SortType { set; get; }
    public string LastEvent { set; get; }
}

}
