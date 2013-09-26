using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace NogginBox.CustomFormsEdit
{
	public class Migration : DataMigrationImpl
	{
		public int Create() {
			ContentDefinitionManager.AlterPartDefinition("CustomFormsEditLinkPart",
				builder => builder
					.Attachable()
					.WithDescription("Attach this part to add custom form edit links to a content type.")
			);

			return 1;
		}
	}
}