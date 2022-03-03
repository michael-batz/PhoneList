/**
* PhoneList - Creates a PDF phone book out of Active Directory data
* Copyright (C) 2022 Landratsamt Wuerzburg <m.batz@lra-wue.bayern.de>
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Affero General Public License for more details.
*
* You should have received a copy of the GNU Affero General Public License
* along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Configuration;
using System.Collections;
using System.DirectoryServices;
using PhoneList.Model;

namespace PhoneList.DataSources
{
    public abstract class DataSource
    {
        public abstract List<Contact> GetContacts();

        public List<ContactGroup> GetContactGroups()
        {
            List<Contact> contacts = this.GetContacts();
            Dictionary<string, ContactGroup> contactGroups = new Dictionary<string, ContactGroup>();
            foreach(Contact contact in contacts)
            {
                string contactGroupName = contact.Department;

                // if ContactGroup does not exist yet, create one
                if(!contactGroups.ContainsKey(contact.Department))
                {
                    // create a new contact group
                    ContactGroup contactGroup = new ContactGroup(contactGroupName);
                    contactGroups.Add(contactGroupName, contactGroup);
                }

                // add contact to ContactGroup
                contactGroups[contactGroupName].AddContact(contact);
            }
        
            // move contactGroups dict to list
            List<ContactGroup> contactGroupList = new List<ContactGroup>();
            foreach(ContactGroup contactGroup in contactGroups.Values)
            {
                contactGroupList.Add(contactGroup);
            }

            // sort ContactGroup list
            contactGroupList.Sort();

            return contactGroupList;

        }

        public bool CheckPhoneNumberFilter(string phoneNumber)
        {
            //get configuration
            string configFilteredNumbers = ConfigurationManager.AppSettings.Get("FilteredPhoneNumbers") ?? "";
            List<string> filteredNumbers = new List<string>(configFilteredNumbers.Split(";"));

            //filter
            if(phoneNumber == "" || filteredNumbers.Contains(phoneNumber))
            {
                return false;
            }
            return true;
        }

    }

    public class TestDataSource : DataSource
    {
        public override List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            contacts.Add(new Contact("John", "Doe", "Dep 1", "A264", "0123-5000-1111"));
            contacts.Add(new Contact("Jane", "Doe", "Dep 2", "B303", "0123-5000-1112"));
            contacts.Add(new Contact("Mike", "Smith", "Dep 1", "A265", "0123-5000-1113"));
            contacts.Add(new Contact("Melissa", "Smith", "Dep 2", "B304", "0123-5000-1114"));
            contacts.Sort();
            return contacts;
        }

    }

    public class ActiveDirectoryDataSource : DataSource
    {
        public override List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();

            // get configuration
            string confAdDomain = ConfigurationManager.AppSettings.Get("ActiveDirectoryDomain") ?? "";
            string confAdPath = ConfigurationManager.AppSettings.Get("ActiveDirectoryPath") ?? "";

            // connect to Active Directory
            DirectoryEntry adConnection = new DirectoryEntry(confAdDomain);
            adConnection.Path = confAdPath;
            adConnection.AuthenticationType = AuthenticationTypes.Secure;

            // define search in Active Directory
            DirectorySearcher adSearch = new DirectorySearcher(adConnection);
            adSearch.PropertiesToLoad.Add("sn");
            adSearch.PropertiesToLoad.Add("givenName");
            adSearch.PropertiesToLoad.Add("telephoneNumber");
            adSearch.PropertiesToLoad.Add("department");
            adSearch.PropertiesToLoad.Add("physicalDeliveryOfficeName");

            // get search results and create contact objects
            SearchResultCollection users = adSearch.FindAll();
            foreach(SearchResult user in users)
            {
                string firstName = "";
                if(user.Properties["givenName"].Count > 0)
                {
                    firstName = user.Properties["givenName"][0].ToString() ?? "";
                }

                string lastName = "";
                if(user.Properties["sn"].Count > 0)
                {
                    lastName = user.Properties["sn"][0].ToString() ?? "";
                }

                string department = "";
                if(user.Properties["department"].Count > 0)
                {
                    department = user.Properties["department"][0].ToString() ?? "";
                }

                string room = "";
                if(user.Properties["physicalDeliveryOfficeName"].Count > 0)
                {
                    room = user.Properties["physicalDeliveryOfficeName"][0].ToString() ?? "";
                }

                string phoneNumber = "";
                if(user.Properties["telephoneNumber"].Count > 0)
                {
                    phoneNumber = user.Properties["telephoneNumber"][0].ToString() ?? "";
                }

                // create contact only of the user object has last name, first name, phone number and department
                if(lastName != "" && this.CheckPhoneNumberFilter(phoneNumber) && firstName != "" & department != "")
                {
                    contacts.Add(new Contact(firstName, lastName, department, room, phoneNumber));
                }
            }

            // sort list
            contacts.Sort();

            return contacts;
        }

    }
}
