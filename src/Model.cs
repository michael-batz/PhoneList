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

namespace PhoneList.Model
{
    public class Contact : IComparable<Contact>
    {

        public string FirstName;
        public string LastName;
        public string Department;
        public string Room;
        public string PhoneNumber;

        public Contact(string firstName, 
                       string lastName, 
                       string department, 
                       string room, 
                       string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Department = department;
            Room = room;
            PhoneNumber = phoneNumber;
        }

        public int CompareTo(Contact? other)
        {
            if(other == null) return 1;
            return this.ToString().CompareTo(other.ToString());
        }

        public override string ToString()
        {
            return $"Conact: {LastName}, {FirstName}: {PhoneNumber}";
        }
    }

    public class ContactGroup : IComparable<ContactGroup>
    {
        public string GroupName;
        public List<Contact> Contacts;

        public ContactGroup(string groupName)
        {
            GroupName = groupName;
            Contacts = new List<Contact>();
        }

        public void AddContact(Contact contact)
        {
            Contacts.Add(contact);
        }

        public int CompareTo(ContactGroup? other)
        {
            if(other == null) return 1;
            return this.GroupName.CompareTo(other.GroupName);
        }

    }
}
