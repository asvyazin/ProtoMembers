using System;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve.Managed;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Guybrush.ProtoMembers
{
	[ContextAction(Group = "C#", Name = "Reassign ProtoMember attributes", Description = "Reassign ProtoMember attributes description")]
	public class AssignProtoMembersAction: ContextActionBase
	{
		private static readonly IClrTypeName ProtoMemberTypeName = new ClrTypeName("ProtoBuf.ProtoMemberAttribute");

		private readonly ICSharpContextActionDataProvider dataProvider;

		public AssignProtoMembersAction([NotNull] ICSharpContextActionDataProvider dataProvider)
		{
			this.dataProvider = dataProvider;
		}

		protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
		{
			DoAssignProtoMembers();
			return null;
		}

		private void DoAssignProtoMembers()
		{
			var declaration = dataProvider.GetSelectedElement<IClassLikeDeclaration>();
			if (declaration == null)
				return;
			var currentId = 1;
			var factory = CSharpElementFactory.GetInstance(dataProvider.PsiModule);
			var resolveContext = new UniversalContext(dataProvider.PsiModule);
			foreach (var property in declaration.PropertyDeclarations)
			{
				var protoMemberAttribute = property.Attributes.FirstOrDefault(attribute =>
					attribute.GetAttributeInstance().GetClrName().Equals(ProtoMemberTypeName));
				if (protoMemberAttribute == null)
					continue;
				var argumentExpression = protoMemberAttribute.ConstructorArgumentExpressions.FirstOrDefault(arg => arg.IsConstantValue(resolveContext) && arg.ConstantValue.IsInteger());
				if (argumentExpression == null)
					continue;
				var newIdExpression = factory.CreateExpressionByConstantValue(new ConstantValue(currentId++, dataProvider.PsiModule));
				if (newIdExpression != null)
				{
					using (declaration.CreateWriteLock())
						argumentExpression.ReplaceBy(newIdExpression);
				}
			}
		}

		public override string Text
		{
			get { return "Assign ProtoMember attributes"; }
		}

		public override bool IsAvailable(IUserDataHolder cache)
		{
			return dataProvider.GetSelectedElement<IClassLikeDeclaration>() != null;
		}
	}
}