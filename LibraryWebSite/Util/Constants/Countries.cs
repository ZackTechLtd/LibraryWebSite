using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebSite.Util.Constants
{
    public class Countries
    {
        public static List<string> GetCountries()
        {
            string[] Countries =
            {
              "United Kingdom",
              "Andorra",
              "United Arab Emirates",
              "Afghanistan",
              "Antigua and Barbuda",
              "Albania",
              "Armenia",
              "Angola",
              "Argentina",
              "Austria",
              "Australia",
              "Azerbaijan",
              "Bosnia and Herzegovina",
              "Barbados",
              "Bangladesh",
              "Belgium",
              "Burkina Faso",
              "Bulgaria",
              "Bahrain",
              "Burundi",
              "Benin",
              "Brunei Darussalam",
              "Bolivia",
              "Brazil",
              "Bahamas",
              "Bhutan",
              "Botswana",
              "Belarus",
              "Belize",
              "Canada",
              "Democratic Republic of the Congo",
              "Central African Republic",
              "Congo",
              "Switzerland",
              "Côte d'Ivoire",
              "Chile",
              "Cameroon",
              "China",
              "Colombia",
              "Costa Rica",
              "Cuba",
              "Cape Verde",
              "Cyprus",
              "Czech Republic",
              "Germany",
              "Djibouti",
              "Denmark",
              "Dominica",
              "Dominican Republic",
              "Algeria",
              "Ecuador",
              "Estonia",
              "Egypt",
              "Eritrea",
              "Spain",
              "Ethiopia",
              "Finland",
              "Fiji",
              "Micronesia (Federated States of)",
              "France",
              "Gabon",
              "Grenada",
              "Georgia",
              "Ghana",
              "Gambia",
              "Guinea",
              "Equatorial Guinea",
              "Greece",
              "Guatemala",
              "Guinea-Bissau",
              "Guyana",
              "Honduras",
              "Croatia",
              "Haiti",
              "Hungary",
              "Indonesia",
              "Ireland",
              "Israel",
              "India",
              "Iraq",
              "Iran (Islamic Republic of)",
              "Iceland",
              "Italy",
              "Jamaica",
              "Jordan",
              "Japan",
              "Kenya",
              "Kyrgyzstan",
              "Cambodia",
              "Kiribati",
              "Comoros",
              "Saint Kitts and Nevis",
              "Democratic People's Republic of Korea",
              "Republic of Korea",
              "Kuwait",
              "Kazakhstan",
              "Lao People's Democratic Republic",
              "Lebanon",
              "Saint Lucia",
              "Liechtenstein",
              "Sri Lanka",
              "Liberia",
              "Lesotho",
              "Lithuania",
              "Luxembourg",
              "Latvia",
              "Libyan Arab Jamahiriya",
              "Morocco",
              "Monaco",
              "Republic of Moldova",
              "Montenegro",
              "Madagascar",
              "Marshall Islands",
              "Macedonia",
              "Mali",
              "Myanmar",
              "Mongolia",
              "Mauritania",
              "Malta",
              "Mauritius",
              "Maldives",
              "Malawi",
              "Mexico",
              "Malaysia",
              "Mozambique",
              "Namibia",
              "Niger",
              "Nigeria",
              "Nicaragua",
              "Netherlands",
              "Norway",
              "Nepal",
              "Nauru",
              "New Zealand",
              "Oman",
              "Panama",
              "Peru",
              "Papua New Guinea",
              "Philippines",
              "Pakistan",
              "Poland",
              "Portugal",
              "Palau",
              "Paraguay",
              "Qatar",
              "Romania",
              "Serbia",
              "Russian Federation",
              "Rwanda",
              "Saudi Arabia",
              "Solomon Islands",
              "Seychelles",
              "Sudan",
              "Sweden",
              "Singapore",
              "Slovenia",
              "Slovakia",
              "Sierra Leone",
              "San Marino",
              "Senegal",
              "Somalia",
              "Suriname",
              "South Sudan",
              "Sao Tome and Principe",
              "El Salvador",
              "Syrian Arab Republic",
              "Swaziland",
              "Chad",
              "Togo",
              "Thailand",
              "Tajikistan",
              "Timor-Leste",
              "Turkmenistan",
              "Tunisia",
              "Tonga",
              "Turkey",
              "Trinidad and Tobago",
              "Tuvalu",
              "United Republic of Tanzania",
              "Ukraine",
              "Uganda",
              "United States of America",
              "Uruguay",
              "Uzbekistan",
              "Saint Vincent and the Grenadines",
              "Venezuela",
              "Viet Nam",
              "Vanuatu",
              "Samoa",
              "Yemen",
              "South Africa",
              "Zambia",
              "Zimbabwe"
            };

            return new List<string>(Countries);
        }

        public static List<SelectListItem> PopulateCountrySelectList(string selected = null)
        {
            

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (string item in Countries.GetCountries())
            {
                countries.Add(new SelectListItem() { Text = item, Value = item });
            }

            SelectListItem sli = null;
            if (selected != null)
            {
                sli = countries.FirstOrDefault(x => x.Value == selected);
            }
            else
            {
                sli = countries.FirstOrDefault(x => x.Value == "United Kingdom");
            }

            if (sli != null)
            {
                int index = countries.IndexOf(sli);
                countries.ElementAt(index).Selected = true;
            }

            return countries;
        }
    }
}
