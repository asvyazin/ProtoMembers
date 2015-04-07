using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Intentions.ContextActions.ClrFormatString;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Guybrush.ProtoMembers
{
//	[ShellComponent]
//	[ContextAction(Group = "C#", Description = "Reorder format arguments", Name = "Reorder format arguments")]
//	public class ReorderStringFormatArgumentsAction: ClrUseFormatStringActionBase
//	{
//		private readonly ICSharpContextActionDataProvider dataProvider;
//		private IStringLiteralAlterer myLiteralWrapper;
//		private ICSharpArgumentsOwner myArgumentsOwner;
//		private ICSharpArgument myArgument;
//
//		public ReorderStringFormatArgumentsAction(ICSharpContextActionDataProvider dataProvider) : base(dataProvider)
//		{
//			this.dataProvider = dataProvider;
//		}
//
//		private class RangeToReplace
//		{
//			public TextRange TextRange { get; set; }
//			public int Index { get; set; }
//		}
//
//		protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
//		{
//			var formatArgumentIdx = myArgumentsOwner.Arguments.IndexOf(myArgument);
//			var formatParamsCount = myArgumentsOwner.Arguments.Count - formatArgumentIdx + 1;
//			var currentPermutation = Enumerable.Range(0, formatParamsCount).ToArray();
//
//			myStringConcatenationClrWrapper = StringConcatenationClrWrapperUtil.CreateWidestContainingStringConcatenation(myLiteralWrapper.Expression);
//			var expectedIdx = 0;
//			foreach (var str in myStringConcatenationClrWrapper.GetWidestConstantValues())
//			{
//				var rangesToReplace = new List<RangeToReplace>();
//				foreach (var formatItem in FormatStringParser.Parse(str))
//				{
//					var idxSubstr = str.Substring(formatItem.IndexRange);
//					int idx;
//					if (int.TryParse(idxSubstr, out idx))
//					{
//						if (expectedIdx != idx)
//						{
//							var elemsTransposition = GenerateTransposition(idx, expectedIdx, currentPermutation.Length);
//							currentPermutation = CombinePermutations(currentPermutation, elemsTransposition);
//
//							rangesToReplace.Add(new RangeToReplace
//							{
//								Index = expectedIdx,
//								TextRange = formatItem.IndexRange,
//							});
//						}
//					}
//					++expectedIdx;
//				}
//				var literalAlterer = StringLiteralAltererUtil.CreateStringLiteralByValue(str, dataProvider.PsiModule);
//				if (literalAlterer == null)
//					throw new InvalidOperationException("IStringLiteralAlterer is null");
//				foreach (var range in rangesToReplace.OrderByDescending(x => x.TextRange.StartOffset))
//				{
//					literalAlterer.Remove(range.TextRange, dataProvider.PsiModule);
//					literalAlterer.Insert(range.TextRange.StartOffset, string.Format("{{{0}}}", range.Index), dataProvider.PsiModule);
//				}
//				var expression = CSharpStringConcatenationClrWrapper.GetWidestStringConcatenationExpression((ICSharpExpression)myLiteralWrapper.Expression);
//			}
//			return null;
//		}
//
//		public override string Text
//		{
//			get { return "Reorder format arguments"; }
//		}
//
//		protected override bool IsAvailableInternal()
//		{
//			var selectedElement = dataProvider.GetSelectedElement<IExpression>();
//			if (selectedElement == null)
//				return false;
//			myArgumentsOwner = dataProvider.GetSelectedElement<ICSharpArgumentsOwner>();
//			if (myArgumentsOwner == null)
//				return false;
//			myArgument = dataProvider.GetSelectedElement<ICSharpArgument>();
//			if (myArgument == null)
//				return false;
//			myLiteralWrapper = StringLiteralAltererUtil.TryCreateStringLiteralByExpression(selectedElement);
//			return true;
//		}
//
//		private static int[] CombinePermutations(int[] permutation1, int[] permutation2)
//		{
//			if (permutation1.Length != permutation2.Length)
//				throw new InvalidOperationException();
//
//			var result = new int[permutation1.Length];
//			for (var i = 0; i < permutation1.Length; i++)
//				result[i] = permutation2[permutation1[i]];
//
//			return result;
//		}
//
//		private static int[] GenerateTransposition(int idx1, int idx2, int length)
//		{
//			var result = Enumerable.Range(0, length).ToArray();
//			result[idx1] = idx2;
//			result[idx2] = idx1;
//			return result;
//		}
//	}
}