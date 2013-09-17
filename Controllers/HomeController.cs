using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Themes;
using Orchard.UI.Notify;
using OrchardContents = Orchard.Core.Contents;

using Orchard.Security;
//using Orchard.ContentPermissions.ViewModels;

namespace NogginBox.CustomFormsEdit.Controllers
{
	[ValidateInput(false), Themed]
	public class HomeController : Controller, IUpdateModel
	{
		private readonly IAuthorizer _authorizer;
		private readonly IAuthenticationService _authenticationService;
		private readonly IOrchardServices _services;

		public Localizer T { get; set; }

		public HomeController(IAuthorizer authorizer, IAuthenticationService authenticationService, IOrchardServices services)
		{
			_authorizer = authorizer;
			_authenticationService = authenticationService;
			_services = services;
		}


		public ActionResult Edit(int contentId)
		{
			var content = _services.ContentManager.Get(contentId);
			if (content == null) return HttpNotFound();

			// Todo: Check for common part

			if(!HasPermissionToEditThisContent(content))
			{
				_services.Notifier.Error(T("You don't have permission to edit this content."));
				return View();
			}

			var shape = _services.ContentManager.BuildEditor(content);
			return View((object)shape);
		}

		[HttpPost, ActionName("Edit")]
		public ActionResult EditContent(int contentId)
		{
			var content = _services.ContentManager.Get(contentId);
			if (content == null) return HttpNotFound();

			if(!HasPermissionToEditThisContent(content))
			{
				_services.Notifier.Error(T("You don't have permission to edit this content."));
				// Todo: Redirect them back to actual content
				return View();
			}

            var shape = _services.ContentManager.UpdateEditor(content, this);
            if (!ModelState.IsValid)
			{
				_services.TransactionManager.Cancel();
				return View("Edit", (object)shape);
            }

            _services.Notifier.Information(T("The content has been saved."));

			// Todo: Redirect them back to actual content
            return RedirectToAction("Edit");
        }

		private bool HasPermissionToEditThisContent(IContent content)
		{
			return
				_authorizer.Authorize(OrchardContents.Permissions.EditContent, content)
				||
				(_authorizer.Authorize(OrchardContents.Permissions.EditOwnContent, content) && HasOwnership(_services.WorkContext.CurrentUser, content));
		}

		private static bool HasOwnership(IUser user, IContent content)
		{
            if (user == null || content == null)
                return false;

            var common = content.As<ICommonPart>();
            if (common == null || common.Owner == null)
                return false;

            return user.Id == common.Owner.Id;
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