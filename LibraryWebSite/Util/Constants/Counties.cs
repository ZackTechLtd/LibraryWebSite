
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebSite.Util.Constants
{
    public class Counties
    {
        public static string[] GetCounties = {
            "Avon"
            , "Bedfordshire"
            , "Berkshire"
            , "Borders"
            , "Buckinghamshire"
            , "Cambridgeshire"
            , "Central"
            , "Cheshire"
            , "Cleveland"
            , "Clwyd"
            , "Cornwall"
            , "County Antrim"
            , "County Armagh"
            , "County Down"
            , "County Fermanagh"
            , "County Londonderry"
            , "County Tyrone"
            , "Cumbria"
            , "Derbyshire"
            , "Devon"
            , "Dorset"
            , "Dumfries and Galloway"
            , "Durham"
            , "Dyfed"
            , "East Riding of Yorkshire"
            , "East Sussex"
            , "Essex"
            , "Fife"
            , "Gloucestershire"
            , "Grampian"
            , "Greater London"
            , "Greater Manchester"
            , "Gwent"
            , "Gwynedd County"
            , "Hampshire"
            , "Herefordshire"
            , "Hertfordshire"
            , "Highlands and Islands"
            , "Humberside"
            , "Isle of Wight"
            , "Kent"
            , "Lancashire"
            , "Leicestershire"
            , "Lincolnshire"
            , "Lothian"
            , "Merseyside"
            , "Mid Glamorgan"
            , "Middlesex"
            , "Norfolk"
            , "North Yorkshire"
            , "Northamptonshire"
            , "Northumberland"
            , "Nottinghamshire"
            , "Oxfordshire"
            , "Powys"
            , "Rutland"
            , "Shropshire"
            , "Somerset"
            , "South Glamorgan"
            , "South Yorkshire"
            , "Staffordshire"
            , "Strathclyde"
            , "Suffolk"
            , "Surrey"
            , "Tayside"
            , "Tyne and Wear"
            , "Warwickshire"
            , "West Glamorgan"
            , "West Midlands"
            , "West Sussex"
            , "West Yorkshire"
            , "Wiltshire"
            , "Worcestershire"
        };

        public static List<SelectListItem> PopulateCountySelectList(string selected = null)
        {
            List<SelectListItem> counties = new List<SelectListItem>();
            foreach (string item in Counties.GetCounties)
            {
                counties.Add(new SelectListItem() { Text = item, Value = item });
            }

            SelectListItem sli = null;
            if (selected != null)
            {
                sli = counties.FirstOrDefault(x => x.Value == selected);
            }
            
            if (sli != null)
            {
                int index = counties.IndexOf(sli);
                counties.ElementAt(index).Selected = true;
            }

            return counties;
        }
    }
}
