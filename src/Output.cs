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

using System;
using System.Configuration;
using System.IO;
using System.Text;
using iText.Html2pdf;
using Scriban;
using PhoneList.Model;

namespace PhoneList.Output
{
    public class OutputGenerator
    {
        private List<Contact> Contacts;
        private List<ContactGroup> ContactGroups;

        public OutputGenerator(List<Contact> contacts, List<ContactGroup> contactGroups)
        {
            Contacts = contacts;
            ContactGroups = contactGroups;
        }

        public void CreateOutput()
        {
            // get folder configutation
            string templateFolder = ConfigurationManager.AppSettings.Get("TemplatePath") ?? "";
            string outputFolder = ConfigurationManager.AppSettings.Get("OutputPath") ?? "";

            // scan template path
            foreach(string template in Directory.EnumerateFiles(templateFolder, "*.tpl"))
            {
                Console.WriteLine("Template {0}", template);
                string templateFileName = template.Substring(templateFolder.Length + 1);
                string outputName = Path.Combine(outputFolder, templateFileName) + ".pdf";
                this.CreateOutput(template, outputName);
            }
        }

        public void CreateOutput(string templateFilename, string outputFilename)
        {
            string htmlTemplate = File.ReadAllText(templateFilename, Encoding.UTF8);
            Template template = Template.Parse(htmlTemplate);
            string renderedTemplate = template.Render(new { contacts = Contacts, contactGroups = ContactGroups });
            FileStream pdfFile = File.Open(outputFilename, FileMode.OpenOrCreate);
            HtmlConverter.ConvertToPdf(renderedTemplate, pdfFile);
        }

    }

}
