using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Security;
using Orchard.Security.Permissions;
using OrchardContents = Orchard.Core.Contents;

namespace NogginBox.CustomFormsEdit
{
	public class CustomFormsEditService
	{
		private readonly IAuthorizer _authorizer;

		public CustomFormsEditService(IAuthorizer authorizer)
		{
			_authorizer = authorizer;
		}

		public bool UserHasPermissionToDeleteThisContent(ContentItem content, IUser user)
		{
			return _authorizer.Authorize(OrchardContents.Permissions.DeleteContent, content) ||
					UserOwnsContentAndHasPermissionTo(content, user, OrchardContents.Permissions.DeleteOwnContent);
		}

		public bool UserHasPermissionToEditThisContent(IContent content, IUser user)
		{
			return _authorizer.Authorize(OrchardContents.Permissions.EditContent, content) ||
					UserOwnsContentAndHasPermissionTo(content, user, OrchardContents.Permissions.EditOwnContent);
		}

		private bool UserOwnsContentAndHasPermissionTo(IContent content, IUser user, Permission permission)
		{
			return _authorizer.Authorize(permission, content) && HasOwnership(user, content);
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
	}
}