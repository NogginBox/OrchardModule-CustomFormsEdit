using System.Web.Mvc;
using Orchard;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Themes;
using Orchard.UI.Notify;
using OrchardContents = Orchard.Core.Contents;

namespace NogginBox.CustomFormsEdit.Controllers
{
	[ValidateInput(false), Themed]
	public class HomeController : Controller, IUpdateModel
	{
		private readonly CustomFormsEditService _customFormsEditService;
		private readonly IOrchardServices _services;

		public Localizer T { get; set; }

		public HomeController(IAuthorizer authorizer, IOrchardServices services)
		{
			_customFormsEditService = new CustomFormsEditService(authorizer);
			_services = services;
		}

		[HttpPost]
		public ActionResult Delete(int contentId)
		{
			var content = _services.ContentManager.Get(contentId);
			if (content == null) return HttpNotFound();

			if(!_customFormsEditService.UserHasPermissionToDeleteThisContent(content, _services.WorkContext.CurrentUser))
			{
				_services.Notifier.Error(T("You don't have permission to delete this content."));
				return RedirectFor(content);
			}

            _services.ContentManager.Remove(content);
            

            _services.Notifier.Information(T("The content has been deleted."));

			return Redirect("~/");
		}


		public ActionResult Edit(int contentId)
		{
			var content = _services.ContentManager.Get(contentId);
			if (content == null) return HttpNotFound();

			// Todo: Check for common part

			var currentUser = _services.WorkContext.CurrentUser;

			if(!_customFormsEditService.UserHasPermissionToEditThisContent(content, currentUser))
			{
				_services.Notifier.Error(T("You don't have permission to edit this content."));
				return View();
			}

			var shape = _services.ContentManager.BuildEditor(content);
			shape.ShowDeleteButton = _customFormsEditService.UserHasPermissionToDeleteThisContent(content, currentUser);

			return View((object)shape);
		}

		[HttpPost, ActionName("Edit")]
		public ActionResult EditContent(int contentId)
		{
			var content = _services.ContentManager.Get(contentId);
			if (content == null) return HttpNotFound();

			if(!_customFormsEditService.UserHasPermissionToEditThisContent(content, _services.WorkContext.CurrentUser))
			{
				_services.Notifier.Error(T("You don't have permission to edit this content."));
				return RedirectFor(content);
			}

            var shape = _services.ContentManager.UpdateEditor(content, this);
            if (!ModelState.IsValid)
			{
				_services.TransactionManager.Cancel();
				return View("Edit", (object)shape);
            }

            _services.Notifier.Information(T("The content has been saved."));

            return RedirectFor(content);
        }

		private ActionResult RedirectFor(IContent content)
		{
			var route = content.As<AutoroutePart>();
			if (route != null)
			{
				return Redirect("~/" + route.Path);
			}

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