using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace NogginBox.CustomFormsEdit
{
	public class Migration : DataMigrationImpl
	{
		public int Create() {
			ContentDefinitionManager.AlterPartDefinition("CustomFormsEditLinkPart",
				builder => builder.Attachable());

			return 1;
		}
	}
}