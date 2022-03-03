# PhoneList
Small tool to create a PDF phone list out of ActiveDirectory user information

## project state
This project was intended to create a quick solution for replacing an existing software. There is a lot of room for
(usability) improvements :-)

## functionality
This tool reads out all users of a specific OU of your ActiveDirectory to create a PDF phone list. The phone list will
be created from template files in your template directory. The tool will scan your configured template directory and
create a PDF from every template. A template is an HTML file with additional syntax to add the Active Directory data.
Generated PDFs will be placed in the configured output directory.


## build from source
The tool is written in C# with .NET 6.0. For building the software, you need to install the .NET SDK under
https://dotnet.microsoft.com/en-us/download. 

Use the following command in the src subdirectory to build the software:

```
dotnet publish -r win10-x64 --self-contained true
```

This will build the tool with all required dependencies under ./bin


## configuration
Configuration is done in PhoneList.dll.config

|parameter | description |
|----------|-------------|
|ActiveDirectoryDomain | your AD domain name |
|ActiveDirectoryPath | OU of the AD users, which should be used as a datasource |
|FilteredPhoneNumbers | users with a specific phone number can be filtered out. Multiple numbers can be devided by ';' |
|TemplatePath | full path to your template files (UNC paths are allowed) |
|OutputPath | full path where PDF output files are placed |


## templates
Please see an example template in the examples/template directory. Templates are HTML files with additional syntax.
Technically, the Scriban template engine is used to create an HTML file, which then will be converted to a PDF by the
iText 7 library. A documentation for the template syntax can be found in the [scriban docs](https://github.com/scriban/scriban/tree/master/doc).

The following variables are exported to the template:

```
## contacts
{{contacts}} # collection with all AD contacts

# walk through all contacts and use the parameters
{{for contact in contacts}}
    {{contact.last_name}} # last name of the AD user
    {{contact.first_name}} # first name of the AD user
    {{contact.department}} # department of the AD user
    {{contact.room}} # office of the AD user
    {{contact.phone_number}} # phone_number of the AD user
{{end}}


## contact groups
{{contact_groups}} # contacts grouped by department

# walk through all departments
{{for contactgroup in contact_groups}}
    {{contactgroup.group_name}} # name of the department

    # walk through all contacts of the department
    {{for contact in contactgroup.contacts}}
        {{contact.last_name}} # last name of the AD user
        {{contact.first_name}} # first name of the AD user
        {{contact.department}} # department of the AD user
        {{contact.room}} # office of the AD user
        {{contact.phone_number}} # phone_number of the AD user
    {{end}}
{{end}}
```




## run
If the configuration was done, simply execute PhoneList.exe
