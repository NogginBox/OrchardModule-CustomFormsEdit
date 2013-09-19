using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Security;

namespace NogginBox.CustomFormsEdit
{
	public class EditLinkShapeProvider : IShapeTableProvider
	{
		private readonly CustomFormsEditService _customFormsEditService;
		private readonly IOrchardServices _services;

		public EditLinkShapeProvider(IAuthorizer authorizer, IOrchardServices services)
		{
			_services = services;
			_customFormsEditService = new CustomFormsEditService(authorizer);
		}

		public void Discover(ShapeTableBuilder builder)
		{
			builder.Describe("Content")
				.OnDisplaying(context => {
					var currentUser = _services.WorkContext.CurrentUser;
					if(currentUser == null) return;

					// Check it's a detail view and user has edit permission for content
					if (context.ShapeMetadata.DisplayType != "Detail" ||
							!_customFormsEditService.UserHasPermissionToEditThisContent(context.Shape.ContentItem, currentUser)) return;

					context.ShapeMetadata.Wrappers.Add("CustomFormsEditWrapper");
				});
		}
	}
}