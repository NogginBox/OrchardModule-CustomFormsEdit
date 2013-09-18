using Orchard.DisplayManagement.Descriptors;

namespace NogginBox.CustomFormsEdit
{
	public class EditLinkShapeProvider : IShapeTableProvider
	{
		public void Discover(ShapeTableBuilder builder)
		{
			builder.Describe("Content")
				.OnDisplaying(context => {
					context.ShapeMetadata.Wrappers.Add("CustomFormsEditWrapper");
				});
		}
	}
}