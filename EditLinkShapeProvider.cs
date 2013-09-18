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

					// Todo: Think about best thing to check for when displaying this. Perhaps check for edit wrapper instead
					if (context.ShapeMetadata.DisplayType != "Detail" || !_customFormsEditService.UserOwnsContentAndHasPermissionToEdit(context.Shape.ContentItem, currentUser)) return;

					context.ShapeMetadata.Wrappers.Add("CustomFormsEditWrapper");
				});
		}
	}
}