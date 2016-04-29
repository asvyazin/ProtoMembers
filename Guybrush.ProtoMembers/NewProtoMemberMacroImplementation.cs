using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace Guybrush.ProtoMembers
{
	[MacroImplementation(Definition = typeof(NewProtoMemberMacroDefinition),
		ScopeProvider = typeof(NewProtoMemberMacroScopeProvider))]
	public class NewProtoMemberMacroImplementation : SimpleMacroImplementation
	{
		private static readonly ILogger Log = Logger.GetLogger(typeof(NewProtoMemberMacroImplementation));

		private static readonly IClrTypeName ProtoMemberTypeName = new ClrTypeName("ProtoBuf.ProtoMemberAttribute");

		public override string EvaluateQuickResult([NotNull] IHotspotContext context)
		{
			var psiSourceFile = context.ExpressionRange.Document.GetPsiSourceFile(context.SessionContext.Solution);
			if (psiSourceFile == null)
				return null;

			using (ReadLockCookie.Create())
			{
				var primaryPsiFile = psiSourceFile.GetPrimaryPsiFile();
				if (primaryPsiFile == null)
					return null;

				var treeTextRange = primaryPsiFile.Translate(context.ExpressionRange);
				var tokenNode = primaryPsiFile.FindTokenAt(treeTextRange.StartOffset) as ITokenNode;

				var classDeclaration = tokenNode?.GetContainingNode<IClassLikeDeclaration>();
				if (classDeclaration == null)
					return null;

				var currentMaxProtoMemberTag = GetMaxProtoMemberTag(classDeclaration);
				return $"{currentMaxProtoMemberTag + 1}";
			}
		}

		private static int GetMaxProtoMemberTag([NotNull] IClassLikeDeclaration declaration)
		{
			return GetProtoMemberAttributes(declaration)
				.Select(GetTagFromProtoMember)
				.Max()
				.GetValueOrDefault(0);
		}

		private static int? GetTagFromProtoMember(IAttributeInstance attribute)
		{
			var tagParameter = attribute.PositionParameter(0);
			if (!tagParameter.IsConstant)
				return null;
			return Convert.ToInt32(tagParameter.ConstantValue.Value);
		}

		[NotNull]
		private static IEnumerable<IAttributeInstance> GetProtoMemberAttributes(
			[NotNull] IClassLikeDeclaration declaration)
		{
			return GetProtoMemberAttributes(declaration.DeclaredElement)
				.Concat(declaration.SuperTypes
					.Select(t => t.GetTypeElement())
					.Where(x => x != null)
					.SelectMany(GetProtoMemberAttributes));
		}

		private static IEnumerable<IAttributeInstance> GetProtoMemberAttributes(ITypeElement classLikeType)
		{
			Log.Info("Getting ProtoMember attributes from {0}", classLikeType);
			return classLikeType.Properties.SelectMany(p => p.GetAttributeInstances(ProtoMemberTypeName, true));
		}
	}
}