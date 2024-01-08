using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.ManageRequest
{
    public interface IManageRequestService :IDisposable
    {

        List<ManageRequestViewModel> GetManageRequest();
        List<ManageRequestViewModel> GetUserManageRequest(string userId);

        List<ManageRequestViewModel> GetRejectRequest(int Id);
        void PostRequestReject(ManageRequestViewModel model);

        List<ManageRequestViewModel> GetAcceptRequest(int Id);
        void PostAcceptRequest(ManageRequestViewModel model);

        List<ManageRequestViewModel> GetBackBook(int Id);
        void PostBackBook(ManageRequestViewModel model);
    }
}
