#Test task: 
 
#####Create web solution with 2 projects (using .NET Core): WebPortal & API. 
#####Create test virtual machine (Windows 7 or higher). 
#####Main task is to show on WebPortal computer’s Add/Remove programs list (Application name/Version/Manufacturer).
 
- You should create PowerShell client (script) which will grab information from computer each 30 minutes and send it to API using REST. 
- API should receive information from client and insert information to DB.  We suggest to use Entity Framework. 
- WebPortal should receive application list from API and show it using DevExtreme library, DataGrid component.
- Create Pie Chart with legend which will show application count by software manufacturer using DevExtreme.
- Apply responsive design (rows, columns) to make page fit for iPhone 6 screen (375x667). You may use Google Chrome screen emulation.
 
#####When solution will be ready, deploy it to Microsoft Azure: 
- Register Trial account in Microsoft Azure, this will give you a free credit;
- Create Resource group; 
- Create 2 free Web Apps – one for portal, one for API;
- Create smallest SQL DB instance (Single Database, Basic);
- Deploy your projects to Web Apps;
 
###As result we expect: archive with source code (solution file, PowerShell script) and portal link.
 
####Links:
* DevExtreme https://js.devexpress.com/
* Microsoft Azure Portal https://portal.azure.com
