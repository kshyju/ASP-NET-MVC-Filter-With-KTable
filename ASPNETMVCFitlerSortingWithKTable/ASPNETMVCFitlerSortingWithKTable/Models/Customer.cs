using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCFitlerSortingWithKTable.Models
{
public class Customer
{
    public int ID { set; get; }
    public string FirstName { set; get; }  
    public string City { set; get; }
    public DateTime RegistrationDate { set; get; }
}

public class CustomerListViewModel
{
    public List<Customer> Customers { set; get; }
    public PagingDetails PagingDetails { set; get; }
    public CustomerListViewModel()
    {
        Customers = new List<Customer>();
        PagingDetails = new PagingDetails();
    }
}

}