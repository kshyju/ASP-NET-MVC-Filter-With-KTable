
namespace ASPNETMVCFitlerSortingWithKTable.Models
{
public class PagingDetails
{ 
    public string BaseURL { get; set; }
    public int CurrentPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPrevPage { get; set; }
    public int QryStrID { get; set; }
    public string QryStrType { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}



}
