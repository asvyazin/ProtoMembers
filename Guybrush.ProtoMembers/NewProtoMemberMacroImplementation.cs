using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.LiveTemplates.CSharp.Macros;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace Guybrush.ProtoMembers
{
	[MacroImplementation(Definition = typeof (NewProtoMemberMacroDefinition), ScopeProvider = typeof (CSharpImpl))]
	public class NewProtoMemberMacroImplementation : SimpleMacroImplementation
	{
		private static readonly ILogger Log = Logger.GetLogger(typeof (NewProtoMemberMacroImplementation));

		private static readonly IClrTypeName ProtoMemberTypeName = new ClrTypeName("ProtoBuf.ProtoMemberAttribute");

		private readonly ILanguageManager languageManager;
		private readonly IMacroParameterValueNew tagArgument;

		public NewProtoMemberMacroImplementation(ILanguageManager languageManager, [Optional] MacroParameterValueCollection arguments)
		{
			Log.Info("Creating NewProtoMemberMacroImplementation, arguments: [{0}]", string.Join(", ", (arguments ?? new MacroParameterValueCollection()).Select(x => x.GetValue())));
			this.languageManager = languageManager;
			tagArgument = arguments.OptionalFirstOrDefault();
		}

		public override string EvaluateQuickResult([NotNull] IHotspotContext context)
		{
			if (tagArgument == null)
				return string.Empty;

			var macroUtil = languageManager.GetService<IMacroUtil, CSharpLanguage>();
			if (macroUtil == null)
				throw new InvalidOperationException("Could not get MacroUtil");
			var currentExpression = macroUtil.AsExpression(tagArgument.GetValue(), context);
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
		private static IEnumerable<IAttributeInstance> GetProtoMemberAttributes([NotNull] IClassLikeDeclaration declaration)
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