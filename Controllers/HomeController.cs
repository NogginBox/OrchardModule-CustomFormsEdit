using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace NogginBox.CustomFormsEdit.Controllers
{
	[ValidateInput(false), Themed]
	public class HomeController : Controller, IUpdateModel
	{
		private IOrchardServices Services { get; set; }
		public Localizer T { get; set; }

		public HomeController(IOrchardServices services)
		{
			Services = services;
		}


		public ActionResult Edit(int contentId)
		{
			var content = Services.ContentManager.Get(contentId);
			if (content == null) return HttpNotFound();

			// Todo: Check permissions
			/*if(!Services.Authorizer.Authorize(Permissions.ViewProfiles, user, null)) {
                return HttpNotFound();
            }*/

			var shape = Services.ContentManager.BuildEditor(content);
			return View((object)shape);
		}

		[HttpPost, ActionName("Edit")]
		public ActionResult EditContent(int contentId)
		{
			var content = Services.ContentManager.Get(contentId);
			if (content == null) return HttpNotFound();

			/*if (Services.WorkContext.CurrentUser == null) {
				return HttpNotFound();
			}*/

            //IUser user = Services.WorkContext.CurrentUser;

            var shape = Services.ContentManager.UpdateEditor(content, this);
            if (!ModelState.IsValid)
			{
				Services.TransactionManager.Cancel();
				return View("Edit", (object)shape);
            }

            Services.Notifier.Information(T("Your content has been saved."));

            return RedirectToAction("Edit");
        }

		#region IUpdateModel Methods

		bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
		{
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
		{
            ModelState.AddModelError(key, errorMessage.ToString());
        }
		
		#endregion
	}
}