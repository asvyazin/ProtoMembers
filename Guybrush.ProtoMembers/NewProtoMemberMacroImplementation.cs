using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.LiveTemplates.CSharp.Macros;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve.Managed;
using JetBrains.ReSharper.Psi.Resolve.Managed;

namespace Guybrush.ProtoMembers
{
	[MacroImplementation(Definition = typeof(NewProtoMemberMacroDefinition), ScopeProvider = typeof(CSharpImpl))]
	public class NewProtoMemberMacroImplementation: SimpleMacroImplementation
	{
		private readonly ILanguageManager languageManager;
		private static readonly IClrTypeName ProtoMemberTypeName = new ClrTypeName("ProtoBuf.ProtoMemberAttribute");

		public NewProtoMemberMacroImplementation(ILanguageManager languageManager)
		{
			this.languageManager = languageManager;
		}

		public override string EvaluateQuickResult([NotNull] IHotspotContext context)
		{
			var macroUtil = languageManager.GetService<IMacroUtil, CSharpLanguage>();
			if (macroUtil == null)
				throw new InvalidOperationException("Could not get MacroUtil");
			var currentExpression = macroUtil.AsExpression(context.ExpressionRange.GetText(), context);
			if (currentExpression == null)
				throw new InvalidOperationException("Could not get current expression");
			var classDeclaration = currentExpression.GetContainingNode<IClassLikeDeclaration>();
			if (classDeclaration == null)
				throw new InvalidOperationException("Could not get class declaration");
			var currentMaxProtoMemberTag = GetMaxProtoMemberTag(classDeclaration);
			return string.Format("{0}", currentMaxProtoMemberTag + 1);
		}

		private static int GetMaxProtoMemberTag([NotNull] IClassLikeDeclaration declaration)
		{
			var resolveContext = new UniversalContext(declaration);
			return GetProtoMemberAttributes(declaration)
				.Select(a => GetTagFromProtoMember(a, resolveContext))
				.Max()
				.GetValueOrDefault(0);
		}

		private static int? GetTagFromProtoMember([NotNull] IAttribute attribute, [NotNull] IResolveContext resolveContext)
		{
			var tagExpression = attribute.ConstructorArgumentExpressions.FirstOrDefault(
				arg => arg.IsConstantValue(resolveContext) && arg.ConstantValue.IsInteger());
			if (tagExpression == null)
				return null;
			return Convert.ToInt32(tagExpression.ConstantValue.Value);
		}

		[NotNull]
		private static IEnumerable<IAttribute> GetProtoMemberAttributes([NotNull] IClassLikeDeclaration declaration)
		{
			return declaration.PropertyDeclarations.SelectMany(p => p.Attributes.Where(IsProtoMemberAttribute));
		}

		private static bool IsProtoMemberAttribute([NotNull] IAttribute attribute)
		{
			return attribute.GetAttributeInstance().GetClrName().Equals(ProtoMemberTypeName);
		}
	}
}