using API.Models;
using Domain;
namespace API.BusinessLogic
{
    public class RequestMapper
    {
        public static ApplicationInfo MapApplicationInfoRequest(ApplicationInfoRequest req)
        {
            ApplicationInfo model = new ApplicationInfo
            {
                //DisplayName = req.DisplayName,
                //InstallDate = APIHelpers.GetInstallDateTime(req.InstallDate),
                //PublisherId = req.Publisher,
                //DisplayVersion = req.DisplayVersion,
                //PSComputerName = req.PSComputerName
            };

            return model;
        }
    }
}