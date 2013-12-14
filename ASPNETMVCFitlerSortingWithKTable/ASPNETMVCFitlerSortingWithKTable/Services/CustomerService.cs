using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETMVCFitlerSortingWithKTable.Models;

namespace ASPNETMVCFitlerSortingWithKTable.Services
{
public class CustomerService
{
    public List<Customer> GetCustomers()
    {
        return new List<Customer> {
            new Customer { FirstName = "Michael", City = "Scranton", RegistrationDate = DateTime.Today.AddDays(-45) },
            new Customer { FirstName = "Dwight", City = "Detroit", RegistrationDate = DateTime.Today.AddDays(-44) },
            new Customer { FirstName = "Jim", City = "Scranton", RegistrationDate = DateTime.Today.AddDays(-34) },
            new Customer { FirstName = "Mike", City = "Ann Arbor", RegistrationDate = DateTime.Today.AddDays(-38) },
            new Customer { FirstName = "Andy", City = "Seattle", RegistrationDate = DateTime.Today.AddDays(-48) },
            new Customer { FirstName = "Angela", City = "Ann Arbor", RegistrationDate = DateTime.Today.AddDays(-45) },
            new Customer { FirstName = "Toby", City = "Scranton", RegistrationDate = DateTime.Today.AddDays(-42) },
            new Customer { FirstName = "Pamela", City = "Scranton", RegistrationDate = DateTime.Today.AddDays(-45) },
            new Customer { FirstName = "Jan", City = "Ann Arbor", RegistrationDate = DateTime.Today.AddDays(-41) },
            new Customer { FirstName = "Kevin", City = "Ann Arbor", RegistrationDate = DateTime.Today.AddDays(-35) },
            new Customer { FirstName = "Robert", City = "Ann Arbor", RegistrationDate = DateTime.Today.AddDays(-48) },
            new Customer { FirstName = "Kelly", City = "Ann Arbor", RegistrationDate = DateTime.Today.AddDays(-40) }        
        };
    }

private List<Customer> SortClientList(List<Customer> clientList, ListFilterVM filter)
{
    //Let's sort first and then filter
    if (!String.IsNullOrEmpty(filter.SortColumnName))
    {
        if (filter.SortColumnName.ToUpper() == "FIRSTNAME")
        {
            if (!String.IsNullOrEmpty(filter.SortType) && filter.SortType.ToUpper() == "DESC")
                clientList = clientList.OrderByDescending(s => s.FirstName).ToList();
            else
                clientList = clientList.OrderBy(s => s.FirstName).ToList();
        }
        else if (filter.SortColumnName.ToUpper() == "CITY")
        {
            if (!String.IsNullOrEmpty(filter.SortType) && filter.SortType.ToUpper() == "DESC")
                clientList = clientList.OrderByDescending(s => s.City).ToList();
            else
                clientList = clientList.OrderBy(s => s.City).ToList();
        }
        else if (filter.SortColumnName.ToUpper() == "REGISTRATIONDATE")
        {
            if (!String.IsNullOrEmpty(filter.SortType) && filter.SortType.ToUpper() == "DESC")
                clientList = clientList.OrderByDescending(s => s.RegistrationDate).ToList();
            else
                clientList = clientList.OrderBy(s => s.RegistrationDate).ToList();
        }           
    }
    return clientList;
}

public List<Customer> ApplyFilters(List<Customer> clientList, ListFilterVM search)
{
    // If the last event was SEARCH (That means first event was Sort), Let's sort the results first before it the search criteria runs on it.
    if ((!String.IsNullOrEmpty(search.LastEvent) && (search.LastEvent.ToUpper() == "FILTER")))
    {
        clientList = SortClientList(clientList, search);
    }
    if (search.Filters != null)
    {
        foreach (var filter in search.Filters)
        {
            clientList = FilterWithName(clientList, filter).ToList();
            clientList = FilterWithCity(clientList, filter).ToList();
            clientList = FilterWithRegistrationDate(clientList, filter).ToList();              
        }
    }
    if ((!String.IsNullOrEmpty(search.LastEvent)) && (search.LastEvent.ToUpper() == "SORT"))
    {
        // If the last event was SORT (That means first event was Sort), Let's sort the results now
        clientList = SortClientList(clientList, search);
    }
    return clientList;
}
private static IEnumerable<Customer> FilterWithName(IEnumerable<Customer> clientList, FilterVM filter)
{
    if ((!String.IsNullOrEmpty(filter.ColumnName)) && (filter.ColumnName.ToUpper() == "FIRSTNAME"))
    {
        if (!String.IsNullOrEmpty(filter.SearchText))
        {
            if (filter.SearchType.ToUpper() == "CONTAINS")
                clientList = clientList.Where(s => s.FirstName.ToUpper().Contains(filter.SearchText.ToUpper())).ToList();

            else if (filter.SearchType.ToUpper() == "STARTSWITH")
                clientList = clientList.Where(s => s.FirstName.StartsWith(filter.SearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();

            else if (filter.SearchType.ToUpper() == "ENDSWITH")
                clientList = clientList.Where(s => s.FirstName.EndsWith(filter.SearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();

            else if ((filter.SearchType.ToUpper() == "EQUALS") || (filter.SearchType == "="))
                clientList = clientList.Where(s => s.FirstName.Equals(filter.SearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();

        }
    }
    return clientList;
}
private static IEnumerable<Customer> FilterWithCity(IEnumerable<Customer> clientList, FilterVM filter)
{
    if ((!String.IsNullOrEmpty(filter.ColumnName)) && (filter.ColumnName.ToUpper() == "CITY"))
    {
        if (!String.IsNullOrEmpty(filter.SearchText))
        {
            if (filter.SearchType.ToUpper() == "CONTAINS")
                clientList = clientList.Where(s => s.City.ToUpper().Contains(filter.SearchText.ToUpper())).ToList();

            else if (filter.SearchType.ToUpper() == "STARTSWITH")
                clientList = clientList.Where(s => s.City.StartsWith(filter.SearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();

            else if (filter.SearchType.ToUpper() == "ENDSWITH")
                clientList = clientList.Where(s => s.City.EndsWith(filter.SearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();

            else if ((filter.SearchType.ToUpper() == "EQUALS") || (filter.SearchType == "="))
                clientList = clientList.Where(s => s.City.Equals(filter.SearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();

        }
    }
    return clientList;
}
private static IEnumerable<Customer> FilterWithRegistrationDate(IEnumerable<Customer> orderList, FilterVM filter)
{
    if ((!String.IsNullOrEmpty(filter.ColumnName)) && (filter.ColumnName.ToUpper() == "REGISTRATIONDATE"))
    {
        if (!String.IsNullOrEmpty(filter.StartDate))
        {
            DateTime dateToCompare;
            if (DateTime.TryParse(filter.StartDate, out dateToCompare))
            {
                if ((filter.SearchType.ToUpper() == "EQUALS") || (filter.SearchType == "="))
                    orderList = orderList.Where(s => s.RegistrationDate.Date == dateToCompare.Date).ToList();

                else if (filter.SearchType.ToUpper() == "BEFORE")
                    orderList = orderList.Where(s => s.RegistrationDate.Date < dateToCompare.Date).ToList();

                else if (filter.SearchType.ToUpper() == "AFTER")
                    orderList = orderList.Where(s => s.RegistrationDate.Date > dateToCompare.Date).ToList();

                else if (filter.SearchType.ToUpper() == "BETWEEN")
                {
                    DateTime endDateToCompare;
                    if (DateTime.TryParse(filter.EndDate, out endDateToCompare))
                    {
                        orderList = orderList.Where(s => s.RegistrationDate.Date > dateToCompare.Date && s.RegistrationDate.Date < endDateToCompare.Date).ToList();
                    }
                }
            }
        }
    }
    return orderList;
}


}
}