using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Security;

namespace NogginBox.CustomFormsEdit.Drivers
{
	public class CustomFormsEditLinkPart : ContentPart {}

	public class ContentRatingDriver : ContentPartDriver<CustomFormsEditLinkPart>
	{
		private readonly CustomFormsEditService _customFormsEditService;
		private readonly IOrchardServices _services;

		public ContentRatingDriver(IAuthorizer authorizer, IOrchardServices services)
		{
			_services = services;
			_customFormsEditService = new CustomFormsEditService(authorizer);
		}

		protected override DriverResult Display(CustomFormsEditLinkPart part, string displayType, dynamic shapeHelper)
		{
			var currentUser = _services.WorkContext.CurrentUser;
			if (currentUser == null) return null;

			// Check user has edit permission for content
			if (!_customFormsEditService.UserHasPermissionToEditThisContent(part, currentUser)) return null;

			return ContentShape("Parts_CustomFormsEditLink",
				() => shapeHelper.Parts_CustomFormsEditLink(
					CotentId: 1));
		}
	}
}