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

using PhoneList.DataSources;
using PhoneList.Output;

public class Program
{

    public static void Main()
    {
        DataSource ds = new ActiveDirectoryDataSource();
        OutputGenerator output = new OutputGenerator(ds.GetContacts(), ds.GetContactGroups());
        output.CreateOutput();
    }

}
