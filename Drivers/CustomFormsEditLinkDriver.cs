using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace NogginBox.CustomFormsEdit.Drivers
{
	public class CustomFormsEditLinkPart : ContentPart {}

	public class ContentRatingDriver : ContentPartDriver<CustomFormsEditLinkPart>
	{
		protected override DriverResult Display(CustomFormsEditLinkPart part, string displayType, dynamic shapeHelper)
		{
			return ContentShape("Parts_CustomFormsEditLink",
				() => shapeHelper.Parts_CustomFormsEditLink(
					CotentId: 1));
		}
	}
}